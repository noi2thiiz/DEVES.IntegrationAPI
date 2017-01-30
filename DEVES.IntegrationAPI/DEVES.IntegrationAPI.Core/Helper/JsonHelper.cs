using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Web;

namespace DEVES.IntegrationAPI.Core.Helper
{
    public static class JsonHelper
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(JsonHelper));

        public static bool TryValidateJson(string jsontext, string filePath, out string output)
        {
            var validatedText = "TryValidateJson: {0}";
            output = string.Empty;
            try
            {
                _log.InfoFormat(validatedText, string.Empty);
                var schemaText = FileHelper.ReadTextFile(filePath);
                var schema = JSchema.Parse(schemaText);
                var jsonObj = JObject.Parse(jsontext);
                IList<string> errorMessages;
                var valid = jsonObj.IsValid(schema, out errorMessages);
                _log.InfoFormat(validatedText, valid);
                output = valid.ToString() + Environment.NewLine;
                if (!valid)
                {
                    _log.ErrorFormat(validatedText, jsontext);
                }
                foreach (var errorMessage in errorMessages)
                {
                    output += errorMessage + Environment.NewLine;
                    _log.WarnFormat(validatedText, errorMessage);
                }
                return valid;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat(validatedText, ex.Message);
                _log.ErrorFormat(validatedText, ex.StackTrace);
                return false;
            }
        }
    }
}