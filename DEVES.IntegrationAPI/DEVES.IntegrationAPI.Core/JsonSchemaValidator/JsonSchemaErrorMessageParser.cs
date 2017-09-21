using System;
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
            {   Console.WriteLine(error.Message);
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
                    TransformMessage(fieldMessage, errorType,out fieldMessage, out errorType);
                       foreach (var field in fieldName.Split(','))
                        outputFail.AddFieldError(field?.Trim(), fieldMessage, errorType);
                }
                else
                {
                    TransformMessage(fieldMessage, errorType, out fieldMessage, out errorType);
                    outputFail.AddFieldError(fieldName, fieldMessage, errorType);
                }
            }
            return outputFail;
        }

        private void TransformMessage(string fieldMessage, string errorType, out string newFieldMessage, out string newErrorType)
        {
            if (fieldMessage == "String '' is less than minimum length of 1.")
            {
                newFieldMessage = "Required field must not be null or empty";
                newErrorType = "Required";


            }
            else
            {
                newFieldMessage = fieldMessage;
                newErrorType = errorType;
            }
        }
    }
    
}