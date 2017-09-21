﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace DEVES.IntegrationAPI.WebApi.Logic.Validator
{
    public class CountryOriginValidator : JsonValidator
    {
        public override void Validate(JToken value, JsonValidatorContext context)
        {
            if (value.Type == JTokenType.String)
            {
                string s = value.ToString();


                var validator = new MasterDataValidator();


                validator.TryConvertNationalityCode("countryOriginCode", s);
                if (validator.Invalid())
                {
                    context.RaiseError($"Value '{s}' is not a valid country origin code.");
                }
            }
        }

        public override bool CanValidate(JSchema schema)
        {
            // validator will run when a schema has a format of countryOrigin
            return (schema.Format == "countryOrigin");
        }
    }
}