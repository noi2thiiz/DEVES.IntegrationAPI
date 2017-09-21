using System.Collections.Generic;
using System.Text.RegularExpressions;
using DEVES.IntegrationAPI.Model;
using Newtonsoft.Json.Schema;

namespace DEVES.IntegrationAPI.Core.JsonSchemaValidator
{
    public class JsonSchemaErrorMessageParser
    {
        public OutputModelFailData Parse(IEnumerable<ValidationError> errorMessage)
        {
            var outputFail = new OutputModelFailData();
            foreach (var error in errorMessage)
            {
                var text = error.Message;
                var fieldMessage = "";
                var fieldName = "";
                var errorType = "";

                fieldName = error.Path;
                fieldMessage = error.Message;
                errorType = error.ErrorType.ToString();

                if (string.IsNullOrEmpty(fieldName))
                {
                    var pattern = new Regex(@"(?<message>(.+?)): (?<field>.+).", RegexOptions.Singleline);
                    var match = pattern.Match(text);


                    if (!match.Success) continue;
                    fieldName = match.Groups["field"].Value;
                    fieldMessage = match.Groups["message"].Value;

                    foreach (var field in fieldName.Split(','))
                        outputFail.AddFieldError(field?.Trim(), fieldMessage, errorType);
                }
                else
                {
                    outputFail.AddFieldError(fieldName, fieldMessage, errorType);
                }
            }
            return outputFail;
        }
    }
}