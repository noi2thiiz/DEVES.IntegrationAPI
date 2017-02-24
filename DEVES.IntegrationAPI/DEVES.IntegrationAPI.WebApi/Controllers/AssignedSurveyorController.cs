using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.AssignedSurveyor;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.Crm.Sdk;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class AssignedSurveyorController : ApiController
    {

        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AssignedSurveyorController));

        public object Post([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new AssignedSurveyorOutputModel_Pass();
            var outputFail = new AssignedSurveyorOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<EWIRequest>(contentText);

            var contentText2 = contentModel.content.ToString();
            var contentModel2 = JsonConvert.DeserializeObject<AssignedSurveyorInputModel>(contentText2);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/AssignedSurveyorInfo_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText2, filePath, out outvalidate))
            {
                outputPass = new AssignedSurveyorOutputModel_Pass();
                _logImportantMessage += "TicketNo: " + contentModel2.ticketNo;
                outputPass = HandleMessage(contentText2, contentModel2);
                return Request.CreateResponse<AssignedSurveyorOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new AssignedSurveyorOutputModel_Fail();
                outputFail.data = new AssignedSurveyorDataOutputModel_Fail();
                var dataFail = outputFail.data;
                dataFail.fieldError = new AssignedSurveyorFieldErrorOutputModel_Fail();
                var fieldError = dataFail.fieldError;
                fieldError.name = "Invalid Input(s)";
                fieldError.message = "Some of your input was invalid. Please recheck again.";

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", fieldError.name, Environment.NewLine, fieldError.message);
                return Request.CreateResponse<AssignedSurveyorFieldErrorOutputModel_Fail>(fieldError);
            }
        }

        private AssignedSurveyorOutputModel_Pass HandleMessage(string valueText, AssignedSurveyorInputModel content)
        {
            //TODO: Do what you want
            var output = new AssignedSurveyorOutputModel_Pass();

            _log.Info("HandleMessage");
            try
            {
                var AssignedSurveyorOutput = new AssignedSurveyorDataOutputModel_Pass();
                //TODO: Do something
                output.code = 200;
                output.message = "Success";
                output.description = "Assigning surveyor is done!";
                output.transactionId = content.ticketNo;
                output.transactionDateTime = System.DateTime.Now;
                output.data = AssignedSurveyorOutput;
                output.data.message = "ClaimNoti Number: " + content.claimNotiNo +
                    " Survey type: " + content.surveyType +
                    " Surveyor code: " + content.surveyorCode +
                    " Surveyor name: " + content.surveyorName;
                // string strSql = string.Format(output.data.message, content.claimNotiNo, content.surveyType, content.surveyorCode, content.surveyorName);
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

        /*
        //To use with log
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AssignedSurveyorController));

        public object Post([FromBody]object value)
        {
            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var output = new AssignedSurveyorOutputModel_Pass();

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
                output = new AssignedSurveyorOutputModel_Pass();
                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.code, Environment.NewLine, output.message);
            }
            return Request.CreateResponse<AssignedSurveyorOutputModel_Pass>(output);
        }

        private AssignedSurveyorOutputModel_Pass HandleMessage(string valueText, AssignedSurveyorInputModel content)
        {
            //TODO: Do what you want
            var output = new AssignedSurveyorOutputModel_Pass();
            
            _log.Info("HandleMessage");
            try
            {
                var AssignedSurveyorOutput = new AssignedSurveyorDataOutputModel_Pass();
                //TODO: Do something
                output.code = 200;
                output.message = "Success";

                output.transactionDateTime = System.DateTime.Now;
                output.data = AssignedSurveyorOutput;
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
        */


    }
}