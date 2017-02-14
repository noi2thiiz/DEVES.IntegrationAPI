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
using DEVES.IntegrationAPI.WebApi;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class ClaimRegistrationController : ApiController
    {
        //To use with log
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ClaimRegistrationController));

        // private QueryInfo q = new QueryInfo();
        // private System.Data.DataTable dt = new System.Data.DataTable();

        /*
        private object Mapping()
        {
            dt = new System.Data.DataTable();
            // DECLARE @ticketNo AS NVARCHAR(20) = 'CAS201702-00003';
            // DECLARE @uniqueID AS UNIQUEIDENTIFIER = 'D246086E-C1EE-E611-80D4-0050568D1874';
            dt = q.Queryinfo_CallerId("CAS201702-00003", "D246086E-C1EE-E611-80D4-0050568D1874");

            dt.Rows[0]["ticketNunber"].ToString();

            return "";
        }
        */

        public object Post([FromBody]object value)
        {
            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: Get", CommonHelper.GetIpAddress());

            var output = new ClaimRegistrationOutputModel();
            if (value==null)
            {
                return Request.CreateResponse<ClaimRegistrationOutputModel>(output);
            }
            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<ClaimRegistrationInputModel>(contentText);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/InternalClaimRegistration_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                _logImportantMessage += "ticketNo: " + contentModel.caseNo;
                output = HandleMessage(contentText, contentModel);
            }
            else
            {
                _logImportantMessage = "Error: Input is not valid.";
                output.claimID = _logImportantMessage;
                _log.Error(_logImportantMessage);
//                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.responseCode, Environment.NewLine, output.responseMessage);
            }
            return Request.CreateResponse<ClaimRegistrationOutputModel>(output);
        }

        private LocusClaimRegistrationOutputModel RegisterClaimOnLocus(string caseNo)
        {
            LocusClaimRegistrationInputModel locusInputModel = Mapping( caseNo );

        }

        private ClaimRegistrationOutputModel HandleMessage(string valueText, ClaimRegistrationInputModel content)
        {
            //TODO: Do what you want
            var output = new ClaimRegistrationOutputModel();
            LocusClaimRegistrationOutputModel locusClaimRegOutput = new LocusClaimRegistrationOutputModel();
            _log.Info("HandleMessage");
            try
            {
                //TODO: Do something
                locusClaimRegOutput = RegisterClaimOnLocus(content.caseNo);
                output.claimID = locusClaimRegOutput.data.claimId;
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

                output.claimID = errorMessage;
            }

            return output;
        }
    }
}
