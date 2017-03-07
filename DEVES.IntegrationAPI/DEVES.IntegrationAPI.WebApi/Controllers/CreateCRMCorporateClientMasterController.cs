using DEVES.IntegrationAPI.Core.Helper;
using System;
using DEVES.IntegrationAPI.Model.CreateCRMCorporateClientMaster;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using System.Net.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class CreateCRMCorporateClientMasterController : ApiController
    {
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AssignedSurveyorController));

        public object Post([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new CreateCRMCorporateClientMasterOutputModel_Pass();
            var outputFail = new CreateCRMCorporateClientMasterOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<CreateCRMCorporateClientMasterInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/CreateCRMCorporateClientMaster_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new CreateCRMCorporateClientMasterOutputModel_Pass();
                _logImportantMessage += "TicketNo: " + contentModel.generalHeader;
                //outputPass = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<CreateCRMCorporateClientMasterOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new CreateCRMCorporateClientMasterOutputModel_Fail();
                outputFail.data = new CreateCRMCorporateClientMasterDataOutputModel_Fail();
                var dataFail = outputFail.data;
                dataFail.name = "Invalid Input(s)";
                dataFail.message = "Some of your input is invalid. Please recheck again.";
                // dataFail.fieldError = "Field xxx is ";

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", dataFail.name, Environment.NewLine, dataFail.message);
                return Request.CreateResponse<CreateCRMCorporateClientMasterDataOutputModel_Fail>(dataFail);
            }
        }

        private CreateCRMCorporateClientMasterOutputModel_Pass HandleMessage(string valueText, CreateCRMCorporateClientMasterInputModel content)
        {
            var output = new CreateCRMCorporateClientMasterOutputModel_Pass();
            _log.Info("HandleMessage");

            try
            {
                var CreateCRMCorporateClientMasterOutput = new CreateCRMCorporateClientMasterDataOutputModel_Pass();

                output.data = CreateCRMCorporateClientMasterOutput;

                return output;
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
                output.description = "ไม่พบ claimNotiNo";

            }
            return output;

        }


    }
}