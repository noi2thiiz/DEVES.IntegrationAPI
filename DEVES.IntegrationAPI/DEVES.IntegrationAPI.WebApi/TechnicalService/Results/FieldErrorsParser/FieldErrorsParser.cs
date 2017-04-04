using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;

namespace DEVES.IntegrationAPI.WebApi.Services.FieldErrorsParser
{
    public class FieldErrorsParser
    {
        public static List<dynamic> Parse(ModelStateDictionary modelState)
        {

            var fieldErrors = new List<dynamic>();

            foreach (string k in modelState.Keys)
            {
                var modelStateVal = modelState[k];
                //var countError = modelStateVal.Errors.Count;

                var errors = new List<string>();
                foreach (var error  in modelStateVal.Errors)
                {

                    errors.Add(error.Exception.Message);
                }
                var DistinctItems = errors.Select(x => x.ToString()).Distinct();
                var f = new
                {
                    name = string.Join(".", (k.ToString()).Split('.').Skip(1)),
                    message = "JSON parsing error.",
                    errors = DistinctItems
                };


                fieldErrors.Add(f);

            }
            return fieldErrors;
        }
    }
}