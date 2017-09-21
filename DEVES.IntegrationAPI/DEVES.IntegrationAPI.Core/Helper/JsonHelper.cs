using DEVES.IntegrationAPI.Model.EWI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Core.JsonSchemaValidator;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Logic.Validator;

namespace DEVES.IntegrationAPI.Core.Helper
{
    public static class JsonHelper
    {
        private static List<ValidationError> returnError = new List<ValidationError>();
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(JsonHelper));


        public static List<ValidationError> getReturnError()
        {
            return returnError;
        }

        public static void setReturnError()
        {
            returnError.Clear();
        }

        public static List<Newtonsoft.Json.Schema.JsonValidator> jsonValidators  = new List<JsonValidator>();
        public static JSchemaReaderSettings settings = new JSchemaReaderSettings
        {
            Validators = new List<Newtonsoft.Json.Schema.JsonValidator> { new CultureFormatValidator() }
        };

        public static void AddJsonValidator(Newtonsoft.Json.Schema.JsonValidator jsonValidator)
        {
            settings.Validators.Add(jsonValidator);
            
        }

        public static bool TryValidateJson(string jsontext, string filePath, out string output)
        {
            setReturnError();
            var validatedText = "TryValidateJson: {0}";
            output = string.Empty;
           
            try
            {
                // throw new Exception("Newtonsoft.Json.Schema.JSchemaException: The free-quota limit of 1000 schema validations per hour has been reached.");
                _log.InfoFormat(validatedText, string.Empty);
                var schemaText = FileHelper.ReadTextFile(filePath);
                var schema = JSchema.Parse(schemaText, settings);
                var jsonObj = JObject.Parse(jsontext);
                IList<ValidationError> errorMessages;
                var valid = jsonObj.IsValid(schema, out errorMessages);

                IList<string> errorMessagesString;
                var valid2 = jsonObj.IsValid(schema, out errorMessagesString);
                foreach (var msg in errorMessagesString)
                {
                    Console.WriteLine(msg);
                }

                _log.InfoFormat(validatedText, valid);
                output = valid.ToString() + Environment.NewLine;
                if (!valid)
                {   
                    _log.ErrorFormat(validatedText, jsontext);
                }
                foreach (var errorMessage in errorMessages)
                {
                    returnError.Add(errorMessage);
                    output += errorMessage + Environment.NewLine;
                    _log.WarnFormat(validatedText, errorMessage);
                }
                // returnError = output;
                return valid;
            }
            catch (Exception ex)
            {
                //returnError.Add("Message: " + ex.ToString());
                //returnError.Add("StackTrace: " + ex.StackTrace);
                _log.ErrorFormat(validatedText, ex.Message);
                _log.ErrorFormat(validatedText, ex.StackTrace);
                return false;
            }
        }
        
        public static bool TryValidateNullMessage(object value, out EWIResponse output)
        {
            output = new EWIResponse();
            if (value == null)
            {
                output = new EWIResponse()
                {
                    username = string.Empty,
                    token = string.Empty,
                    success = false,
                    responseCode = EWIResponseCode.ETC.ToString(),
                    responseMessage = "There is no message.",
                    hostscreen = string.Empty,
                    content = null
                };
               
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {2}", output.responseCode, Environment.NewLine, output.responseMessage);
                return false;
            }
            return true;
        }

        public static OutputModelFailData GetErrorMessageOutputModel()
        {
            var parser = new JsonSchemaErrorMessageParser();
            return parser.Parse(returnError);
        }
    }
}