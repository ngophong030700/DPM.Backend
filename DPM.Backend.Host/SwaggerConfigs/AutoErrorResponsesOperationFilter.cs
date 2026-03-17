using Microsoft.OpenApi.Models;
using Shared.Application.BaseClass;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AutoErrorResponsesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Tạo schema thực tế cho ErrorResponse (đảm bảo component được sinh)
        var schema = context.SchemaGenerator.GenerateSchema(typeof(ErrorResponse), context.SchemaRepository);

        var content = new Dictionary<string, OpenApiMediaType>
        {
            ["application/json"] = new OpenApiMediaType { Schema = schema }
        };

        var responses = new Dictionary<string, string>
        {
            ["400"] = "Bad Request",
            ["401"] = "Unauthorized",
            ["404"] = "Not Found",
            ["409"] = "Conflict",
            ["500"] = "Server Error"
        };

        foreach (var (status, desc) in responses)
        {
            if (!operation.Responses.ContainsKey(status))
            {
                operation.Responses[status] = new OpenApiResponse
                {
                    Description = desc,
                    Content = content
                };
            }
        }
    }
}
