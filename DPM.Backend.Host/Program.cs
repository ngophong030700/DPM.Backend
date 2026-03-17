using IED.VTVMS.Host.Helpers;
using IED.VTVMS.Host.Middleware;
using IED.VTVMS.Host.SwaggerConfigs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Application.BaseClass;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.ExternalServices;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Giới hạn kích thước upload
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = Convert.ToInt64(builder.Configuration["MaxUploadSize"] ?? "524288000");
});

// Đăng ký CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Cấu hình Kestrel endpoints
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5080, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });

    serverOptions.ListenAnyIP(5081, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });

    // Giới hạn request body
    serverOptions.Limits.MaxRequestBodySize = Convert.ToInt64(builder.Configuration["MaxLengthRequest"] ?? "524288000");
});

// Tắt validate tự động
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers(options =>
{
    // Thêm global action filter
    options.Filters.Add<ValidateModelAttribute>();
})
.AddNewtonsoftJson(options =>
{
    // Cấu hình camelCase cho JSON
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy()
    };
});

// Add ApplicationDbContext
builder.Services.AddApplicationDbContext(builder.Configuration);

// Đăng ký MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.Load("Identity.Application"));
});

// Cấu hình camelCase cho route và query string
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

// Cấu hình HttpClient cho ExternalApiClient
builder.Services.AddHttpClient<IExternalApiClient, ExternalApiClient>(client =>
{
    // Base URL của API external, lấy từ appsettings.json
    client.BaseAddress = new Uri(builder.Configuration["ExternalApiUrl"]!);

    // Timeout
    client.Timeout = TimeSpan.FromSeconds(120);
});

// Các config khác
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IED VTVMS API", Version = "v1" });
    //c.SchemaFilter<SnakeCaseSchemaFilter>();
    c.SchemaFilter<EnumSchemaFilter>();
    c.OperationFilter<AutoErrorResponsesOperationFilter>();
    c.OperationFilter<CamelCaseQueryParameterOperationFilter>();

    // Thêm cấu hình cho JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập JWT token vào đây: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, true);
});

// Đăng ký CurrentUserService
builder.Services.AddHttpContextAccessor();

// Thêm Authentication + JWT
var jwtKey = builder.Configuration["JwtSettings:SecretKey"];
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
var jwtAudience = builder.Configuration["JwtSettings:Audience"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey ?? throw new Exception("Vui lòng cấu hình JWT"))
        )
    };

    // Thêm xử lý 401 trả về ErrorResponse
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            // Ngăn response mặc định
            context.HandleResponse();

            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse("UNAUTHORIZED", "Bạn không có quyền truy cập.");
            return context.Response.WriteAsJsonAsync(response);
        }
    };
});
builder.Services.AddAuthorization();

// Build app
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowFrontend");
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); // Map tất cả endpoints từ controllers
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "E:/Storage" : "/storage"),
    RequestPath = "/storage",
    OnPrepareResponse = ctx =>
    {
        // Cache 30 giây
        ctx.Context.Response.Headers["Cache-Control"] = "public, max-age=30";
        ctx.Context.Response.Headers["Pragma"] = "";
        ctx.Context.Response.Headers["Expires"] = DateTime.UtcNow.AddSeconds(30).ToString("R");
    }
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "E:/Templates" : "/templates"),
    RequestPath = "/templates",
    OnPrepareResponse = ctx =>
    {
        // Cache 30 giây
        ctx.Context.Response.Headers["Cache-Control"] = "public, max-age=30";
        ctx.Context.Response.Headers["Pragma"] = "";
        ctx.Context.Response.Headers["Expires"] = DateTime.UtcNow.AddSeconds(30).ToString("R");
    }
});

app.Run();
