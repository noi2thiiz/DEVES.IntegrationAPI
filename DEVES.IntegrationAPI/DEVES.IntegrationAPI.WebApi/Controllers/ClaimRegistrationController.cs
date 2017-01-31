using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.ClaimRegistration;
using DEVES.IntegrationAPI.Model.EWI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class ClaimRegistrationController : ApiController
    {
        //To use with log
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ClaimRegistrationController));

        public object Post([FromBody]object value)
        {
            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: Get", CommonHelper.GetIpAddress());

            var output = new EWIResponse();
            if (!JsonHelper.TryValidateNullMessage(value, out output))
            {
                return Request.CreateResponse<EWIResponse>(output);
            }
            var valueText = value.ToString();
            var ewiRequest = JsonConvert.DeserializeObject<EWIRequest>(valueText);
            var contentText = ewiRequest.content.ToString();
            _logImportantMessage = "Username: {0}, Token: {1}, ";
            _logImportantMessage = string.Format(_logImportantMessage, ewiRequest.username, ewiRequest.token);
            var contentModel = JsonConvert.DeserializeObject<ClaimRegistrationInputModel>(contentText);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/ClaimRegistration_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                _logImportantMessage += "ticketNo: " + contentModel.claimHeader[0].ticketNo;
                output = HandleMessage(contentText, contentModel);
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
                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.responseCode, Environment.NewLine, output.responseMessage);
            }
            return Request.CreateResponse<EWIResponse>(output);
        }

        private EWIResponse HandleMessage(string valueText, ClaimRegistrationInputModel content)
        {
            //TODO: Do what you want
            var output = new EWIResponse();
            var updateClaimStatusOutput = new ClaimRegistrationOutputModel();
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
                _log.Error("RequestId - " + _logImportantMessage);
                _log.Error(errorMessage);

                output = new EWIResponse()
                {
                    username = string.Empty,
                    token = string.Empty,
                    success = false,
                    responseCode = EWIResponseCode.ETC.ToString(),
                    responseMessage = "Internal process error",
                    hostscreen = string.Empty,
                    content = updateClaimStatusOutput
                };
            }

            return output;
        }
    }
}
