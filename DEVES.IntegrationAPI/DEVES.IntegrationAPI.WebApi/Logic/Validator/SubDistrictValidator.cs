using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace DEVES.IntegrationAPI.WebApi.Logic.Validator
{
    public class SubDistrictValidator : JsonValidator
    {
        public override void Validate(JToken value, JsonValidatorContext context)
        {
            if (value.Type == JTokenType.String)
            {
                var s = value.ToString();
                var validator = new MasterDataValidator();

                validator.TryConvertSubDistrictCode("subDistrictCode", s);
                if (validator.Invalid())
                    context.RaiseError($"Value '{s}' is not a valid sub district code.");
            }
        }

        public override bool CanValidate(JSchema schema)
        {
            // validator will run when a schema has a format of subDistrictCode
            return schema.Format == "subDistrict";
        }
    }
}