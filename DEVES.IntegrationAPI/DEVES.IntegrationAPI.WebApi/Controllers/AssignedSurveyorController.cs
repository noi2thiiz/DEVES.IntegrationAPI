using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.AssignedSurveyor;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class AssignedSurveyorController : ApiController
    {

        //To use with log
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AssignedSurveyorController));

        public object Post([FromBody]object value)
        {
            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var output = new AssignedSurveyorOutputModel();

            var contentText = value.ToString();
            //_logImportantMessage = string.Format(_logImportantMessage, output.transactionId);
            var contentModel = JsonConvert.DeserializeObject<AssignedSurveyorInputModel>(contentText);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/AssignedSurveyorInfo_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                _logImportantMessage += "Code: " + contentModel.ticketNo;
                output = HandleMessage(contentText, contentModel);
            }
            else
            {
                output = new AssignedSurveyorOutputModel();
                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.code, Environment.NewLine, output.message);
            }
            return Request.CreateResponse<AssignedSurveyorOutputModel>(output);
        }

        private AssignedSurveyorOutputModel HandleMessage(string valueText, AssignedSurveyorInputModel content)
        {
            //TODO: Do what you want
            var output = new AssignedSurveyorOutputModel();
            var updateClaimInfoOutput = new AssignedSurveyorDataOutputModel();
            _log.Info("HandleMessage");
            try
            {
                //TODO: Do something
                output.code = EWIResponseCode.EWI0000I.ToString();
                output.message = "PASS";
                output.description = "PASS PASS";
                output.transactionId = "1";
                output.transactionDateTime = System.DateTime.Now;
                updateClaimInfoOutput.descItem = "abc";
                updateClaimInfoOutput.shortdesc = "def";
                updateClaimInfoOutput.longdesc = "xyz";
                output.data = updateClaimInfoOutput;
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


            }

            return output;
        }
    }
}