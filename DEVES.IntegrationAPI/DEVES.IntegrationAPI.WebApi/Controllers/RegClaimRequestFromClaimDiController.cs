using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.RegClaimRequestFromClaimDi;
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
    public class RegClaimRequestFromClaimDiController : ApiController
    {
        //To use with log
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(RegClaimRequestFromClaimDiController));

        public object Post([FromBody]object value)
        {
            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var output = new RegClaimRequestFromClaimDiOutputModel();
            if (value == null)
            {
                output.code = "500";
                output.message = "Error";
                output.description = "innput is null";
                output.transactionDateTime = DateTime.Now;
                output.transactionId = "1234567";
                return Request.CreateResponse<RegClaimRequestFromClaimDiOutputModel>(output);
            }
            var valueText = value.ToString();
            //_logImportantMessage = "Username: {0}, Token: {1}, ";
            var contentModel = JsonConvert.DeserializeObject<RegClaimRequestFromClaimDiInputModel>(valueText);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/RegClaimRequestFromClaimDi_Input_Schema.json");

            if (JsonHelper.TryValidateJson(valueText, filePath, out outvalidate))
            {
//                _logImportantMessage += "Code: " + contentModel.claimNo;
                output = HandleMessage(valueText, contentModel);
            }
            else
            {
                output = new RegClaimRequestFromClaimDiOutputModel()
                {
                };
                _log.Error(_logImportantMessage);
                //_log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.responseCode, Environment.NewLine, output.responseMessage);
            }
            return Request.CreateResponse<RegClaimRequestFromClaimDiOutputModel>(output);
        }

        private RegClaimRequestFromClaimDiOutputModel HandleMessage(string valueText, RegClaimRequestFromClaimDiInputModel content)
        {
            //TODO: Do what you want
            RegClaimRequestFromClaimDiOutputModel output ;
            _log.Info("HandleMessage");
            try
            {
                //TODO: Do something

                output = new RegClaimRequestFromClaimDiOutputModel()
                {
                    code = "200",
                    message = "Success",
                    description = "Register ClaimRequest from ClaimDi success",
                    transactionDateTime = DateTime.Now,
                    transactionId = "1234567",
                    data = null
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

                output = new RegClaimRequestFromClaimDiOutputModel()
                {
                    code = "500",
                    message = "Error",
                    description = e.Message,
                    transactionDateTime = DateTime.Now,
                    transactionId = "1234567",
                    data = null
                };
            }

            return output;
        }
    }
}
