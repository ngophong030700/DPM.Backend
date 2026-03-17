using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;

namespace IED.VTVMS.Host.Helpers
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            if (!type.IsEnum)
                return;

            schema.Enum.Clear();
            schema.Type = "integer"; // Swagger hiểu gửi integer
            schema.Format = "int32";

            schema.Description ??= "";
            schema.Description += " (";

            var enumValues = Enum.GetValues(type).Cast<Enum>().ToList();

            foreach (var enumValue in enumValues)
            {
                var member = type.GetMember(enumValue.ToString()).FirstOrDefault();
                var descriptionAttr = member?.GetCustomAttribute<DescriptionAttribute>();
                var description = descriptionAttr?.Description ?? enumValue.ToString();
                var intValue = Convert.ToInt32(enumValue);

                schema.Enum.Add(new OpenApiInteger(intValue));

                // Thêm mô tả vào schema
                schema.Description += $"{description} = {intValue}, ";
            }

            schema.Description = schema.Description.TrimEnd(',', ' ') + ")";
        }
    }
}
