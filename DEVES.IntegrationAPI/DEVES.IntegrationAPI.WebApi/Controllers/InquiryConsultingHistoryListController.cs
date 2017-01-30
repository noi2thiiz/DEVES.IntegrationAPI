using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.Enum;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.InquiryConsultingHistoryList;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class InquiryConsultingHistoryListController : ApiController
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(InquiryConsultingHistoryListController));

        public object Post([FromBody]object value)
        {
            _log.Info("HttpMethod: POST");
            _log.InfoFormat("IP ADDRESS: {0}", GetIpAddress());

            var output = new EWIResponse();
            if (!TryValidateNullMessage(value, out output))
            {
                return Request.CreateResponse<EWIResponse>(output);
            }
            var valueText = value.ToString();
            var input = JsonConvert.DeserializeObject<EWIRequest>(valueText);
            var contentText = input.content.ToString();
            var contentModel = JsonConvert.DeserializeObject<InquiryConsultingHistoryListInput>(contentText);
            string outvalidate = string.Empty;            
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/InquiryConsultingHistoryList_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                output = HandleMessage(contentText, HttpMethodType.POST);
            }
            else
            {
                output = new EWIResponse()
                {
                    username = string.Empty,
                    token = string.Empty,
                    success = false,
                    responseCode = EWIResponseCode.ETC.ToString(),
                    responseMessage = outvalidate,
                    hostscreen = string.Empty,
                    content = null
                };
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.responseCode, Environment.NewLine, output.responseMessage);
            }
            return Request.CreateResponse<EWIResponse>(output);
        }

        private EWIResponse HandleMessage(string valueText, HttpMethodType httpMethod)
        {
            //TODO: Do what you want
            var apiRequestId = string.Empty;
            var output = new EWIResponse();
            _log.Info("HandleMessage");
            try
            {
                //TODO: Do something

                output = new EWIResponse()
                {
                    username = string.Empty,
                    token = string.Empty,
                    success = false,
                    responseCode = EWIResponseCode.EWI0000I.ToString(),
                    responseMessage = "Success",
                    hostscreen = string.Empty,
                    content = null
                };
            }
            catch (Exception e)
            {
                var errorMessage = e.GetType().FullName + ": " + e.Message + Environment.NewLine;
                errorMessage += "StackTrace: " + e.StackTrace;

                if (e.InnerException != null)
                {
                    errorMessage += Environment.NewLine + "(InnerException)" + e.InnerException.GetType().FullName + " - " + e.InnerException.Message + Environment.NewLine;
                    errorMessage += "StackStrace: " + e.InnerException.StackTrace;
                }
                _log.Error("RequestId - " + apiRequestId);
                _log.Error(errorMessage);

                output = new EWIResponse()
                {
                    username = string.Empty,
                    token = string.Empty,
                    success = false,
                    responseCode = EWIResponseCode.ETC.ToString(),
                    responseMessage = "Internal process error",
                    hostscreen = string.Empty,
                    content = null
                };
            }

            return output;
        }

        private bool TryValidateNullMessage(object value, out EWIResponse output)
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

        private bool TryValidateJson(string jsontext, out string output)
        {
            var validatedText = "TryValidateJson: {0}";
            output = string.Empty;
            try
            {
                var schemaPath = "~/App_Data/JsonSchema/InquiryConsultingHistoryList_Input_Schema.json";
                _log.InfoFormat(validatedText, string.Empty);
                var filePath = HttpContext.Current.Server.MapPath(schemaPath);
                var schemaText = FileHelper.ReadTextFile(filePath);
                var schema = JSchema.Parse(schemaText);
                var jsonObj = JObject.Parse(jsontext);
                IList<string> errorMessages;
                bool valid = jsonObj.IsValid(schema, out errorMessages);
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

        private string GetIpAddress()
        {
            string ipaddress;
            ipaddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipaddress))
                ipaddress += HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            return ipaddress;
        }
    }
}