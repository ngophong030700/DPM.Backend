using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IED.VTVMS.Host.SwaggerConfigs
{
    public class CamelCaseQueryParameterOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var param in operation.Parameters)
            {
                param.Name = char.ToLowerInvariant(param.Name[0]) + param.Name.Substring(1);
            }
        }
    }
}
