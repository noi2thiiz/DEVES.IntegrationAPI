using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace DEVES.IntegrationAPI.WebApi.Logic.Validator
{
    public class CultureFormatValidator : JsonValidator
        {
        public override void Validate(JToken value, JsonValidatorContext context)
        {
            if (value.Type == JTokenType.String)
            {
                string s = value.ToString();
    
               try
               {
                    // test whether the string is a known culture, e.g. en-US, fr-FR
                   new CultureInfo(s);
               }
            catch (CultureNotFoundException)
            {
                context.RaiseError($"Text '{s}' is not a valid culture name.");
                    
            }
        }
    }

    public override bool CanValidate(JSchema schema)
    {
        // validator will run when a schema has a format of culture
        return (schema.Format == "culture");
    }
}
}