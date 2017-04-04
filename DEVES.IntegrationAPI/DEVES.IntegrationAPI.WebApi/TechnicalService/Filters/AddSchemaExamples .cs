using System;
using Swashbuckle.Swagger;

namespace DEVES.IntegrationAPI.WebApi
{
    public class AddSchemaExamples : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if (type == typeof(string))
            {
                schema.example = "XXX";
            }
        }
    }
}