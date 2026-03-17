using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json.Serialization;

public class SnakeCaseSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema?.Properties == null) return;

        var contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };

        var keys = schema.Properties.Keys.ToList();
        foreach (var key in keys)
        {
            var value = schema.Properties[key];
            var newKey = contractResolver.GetResolvedPropertyName(key);
            if (newKey != key)
            {
                schema.Properties.Remove(key);
                schema.Properties.Add(newKey, value);
            }
        }
    }
}
