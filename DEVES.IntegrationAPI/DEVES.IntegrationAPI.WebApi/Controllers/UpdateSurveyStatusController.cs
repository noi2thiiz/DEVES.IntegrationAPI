using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.UpdateSurveyStatus;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class UpdateSurveyStatusController : ApiController
    {
        //To use with log
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UpdateSurveyStatusController));

        public object Post([FromBody]object value)
        {
            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var output = new UpdateSurveyStatusOutputModel_Pass();

            var contentText = value.ToString();
            //_logImportantMessage = string.Format(_logImportantMessage, output.transactionId);
            var contentModel = JsonConvert.DeserializeObject<UpdateSurveyStatusInputModel>(contentText);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/UpdateSurveyStatus_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                _logImportantMessage += "Code: " + contentModel.ticketNo;
                output = HandleMessage(contentText, contentModel);
            }
            else
            {

                output = new UpdateSurveyStatusOutputModel_Pass();
                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.code, Environment.NewLine, output.message);
            }
            return Request.CreateResponse<UpdateSurveyStatusOutputModel_Pass>(output);
        }

        private UpdateSurveyStatusOutputModel_Pass HandleMessage(string valueText, UpdateSurveyStatusInputModel content)
        {
            //TODO: Do what you want
            var output = new UpdateSurveyStatusOutputModel_Pass();
            // var UpdateSurveyStatusOutput = new UpdateSurveyStatusDataOutputModel();
            _log.Info("HandleMessage");
            try
            {
                var UpdateSurveyStatusOutput = new UpdateSurveyStatusDataOutputModel_Pass();
                //TODO: Do something
                output.code = 200;
                output.message = "Success";

                output.data = UpdateSurveyStatusOutput;

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