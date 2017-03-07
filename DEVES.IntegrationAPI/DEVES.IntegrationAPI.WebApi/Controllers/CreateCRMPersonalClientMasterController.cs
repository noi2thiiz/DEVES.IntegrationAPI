using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.CreateCRMPersonalClientMaster;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.Crm.Sdk;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using System.Linq;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class CreateCRMPersonalClientMasterController : ApiController
    {

        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AssignedSurveyorController));

        OrganizationServiceProxy _serviceProxy;
        private Guid _accountId;

        public object Post([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new CreateCRMPersonalClientMasterOutputModel_Pass();
            var outputFail = new CreateCRMPersonalClientMasterOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<CreateCRMPersonalClientMasterInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/AssignedSurveyorInfo_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new CreateCRMPersonalClientMasterOutputModel_Pass();
                _logImportantMessage += "TicketNo: " + contentModel.generalHeader;
                //outputPass = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<CreateCRMPersonalClientMasterOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new CreateCRMPersonalClientMasterOutputModel_Fail();
                outputFail.data = new CreateCRMPersonalClientMasterDataOutputModel_Fail();
                var dataFail = outputFail.data;
                dataFail.name = "Invalid Input(s)";
                dataFail.message = "Some of your input is invalid. Please recheck again.";
                // dataFail.fieldError = "Field xxx is ";

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", dataFail.name, Environment.NewLine, dataFail.message);
                return Request.CreateResponse<CreateCRMPersonalClientMasterDataOutputModel_Fail>(dataFail);
            }
        }

        private CreateCRMPersonalClientMasterOutputModel_Pass HandleMessage(string valueText, CreateCRMPersonalClientMasterInputModel content)
        {
            var output = new CreateCRMPersonalClientMasterOutputModel_Pass();
            _log.Info("HandleMessage");

            try
            {
                var CreateCRMPersonalClientMasterOutput = new CreateCRMPersonalClientMasterDataOutputModel_Pass();

                output.data = CreateCRMPersonalClientMasterOutput;
                /*
                output.code = 200;
                output.message = "Success";
                output.description = "Create CRMPersonal Client Master is done!";
                output.transactionId = content.;
                output.transactionDateTime = System.DateTime.Now;
                output.data = AssignedSurveyorOutput;
                output.data.message = "ClaimNoti Number: " + content.claimNotiNo +
                    " Survey type: " + content.surveyType +
                    " Surveyor code: " + content.surveyorCode +
                    " Surveyor name: " + content.surveyorName;

    */
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