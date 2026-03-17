using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Application.BaseClass;
using Shared.Domain.Exceptions;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JsonSerializerSettings _jsonSettings;

    public GlobalExceptionMiddleware(RequestDelegate next, IOptions<MvcNewtonsoftJsonOptions> jsonOptions)
    {
        _next = next;
        _jsonSettings = jsonOptions.Value.SerializerSettings;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DuplicateException ex)
        {
            await WriteError(context, 409, "DUPLICATE", ex.Message);
        }
        catch (NotFoundException ex)
        {
            await WriteError(context, 404, "NOT_FOUND", ex.Message);
        }
        catch (DomainException ex)
        {
            await WriteError(context, 400, "BAD_REQUEST", ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            await WriteError(context, 401, "UNAUTHORIZED", "Bạn không có quyền thực hiện hành động này.");
        }
        catch (Exception ex)
        {
            await WriteError(context, 500, "SERVER_ERROR", "Có lỗi hệ thống xảy ra.", ex.ToString());
        }
    }

    private Task WriteError(HttpContext context, int status, string code, string message, string? exceptionMessage = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = status;

        var json = JsonConvert.SerializeObject(
            new ErrorResponse(code, message, exceptionMessage),
            _jsonSettings
        );

        return context.Response.WriteAsync(json);
    }
}
