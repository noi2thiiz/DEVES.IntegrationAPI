using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.UpdateClaimInfo;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class UpdateClaimInfoController : ApiController
    {
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AssignedSurveyorController));

        OrganizationServiceProxy _serviceProxy;
        private Guid _accountId;

        public object Post([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new UpdateClaimInfoOutputModel_Pass();
            var outputFail = new UpdateClaimInfoOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<EWIRequest>(contentText);

            var contentText2 = contentModel.content.ToString();
            var contentModel2 = JsonConvert.DeserializeObject<UpdateClaimInfoInputModel>(contentText2);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/UpdateClaimInfo_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText2, filePath, out outvalidate))
            {
                outputPass = new UpdateClaimInfoOutputModel_Pass();
                _logImportantMessage += "TicketNo: " + contentModel2.ticketNo;
                outputPass = HandleMessage(contentText2, contentModel2);

                EWIResponse_ReqSur res = new EWIResponse_ReqSur();

                if (outputPass.data == null)
                {
                    res.success = false;
                    res.responseMessage = "Something went wrong";
                }
                else
                {
                    res.gid = contentModel.gid;
                    res.username = contentModel.username;
                    res.token = contentModel.token;
                    res.success = true;
                    res.responseCode = "EWI0000I";
                    res.responseMessage = "Updating survey status is success!";
                    res.content = outputPass;
                }

                return Request.CreateResponse<EWIResponse_ReqSur>(res);
            }
            else
            {
                outputFail = new UpdateClaimInfoOutputModel_Fail();
                outputFail.data = new UpdateClaimInfoDataOutputModel_Fail();
                var dataFail = outputFail.data;
                dataFail.fieldError = new UpdateClaimInfoFieldErrorOutputModel_Fail();
                var fieldError = dataFail.fieldError;
                fieldError.name = "Invalid Input(s)";
                fieldError.message = "Some of your input was invalid. Please recheck again.";

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", fieldError.name, Environment.NewLine, fieldError.message);
                return Request.CreateResponse<UpdateClaimInfoFieldErrorOutputModel_Fail>(fieldError);
            }
        }

        private UpdateClaimInfoOutputModel_Pass HandleMessage(string valueText, UpdateClaimInfoInputModel content)
        {
            //TODO: Do what you want
            var output = new UpdateClaimInfoOutputModel_Pass();

            _log.Info("HandleMessage");
            try
            {
                var UpdateClaimInfoOutput = new UpdateClaimInfoDataOutputModel_Pass();

                var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
                _serviceProxy = connection.OrganizationServiceProxy;
                ServiceContext svcContext = new ServiceContext(_serviceProxy);

                var query = from c in svcContext.IncidentSet
                            where c.pfc_claim_noti_number == content.claimNotiNo
                            select c;

                Incident incident = query.FirstOrDefault<Incident>();
                _accountId = new Guid(incident.IncidentId.ToString());

                Incident retrievedIncident = (Incident)_serviceProxy.Retrieve(Incident.EntityLogicalName, _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                try
                {

                    retrievedIncident.pfc_locus_claim_status_code = content.claimStatusCode;
                    retrievedIncident.pfc_locus_claim_status_desc = content.claimStatusDesc;
                    retrievedIncident.pfc_customer_vip = true; //content.vipCaseFlag;
                    retrievedIncident.pfc_high_loss_case_flag = true; //content.highLossCaseFlag;
                    retrievedIncident.pfc_legal_case_flag = true; //content.LegalCaseFlag;

                    _serviceProxy.Update(retrievedIncident);

                }
                catch (Exception e)
                {
                    output.description = "Retrieving data PROBLEM";
                    return output;
                }

                //TODO: Do something

                output.code = 200;
                output.message = "Success";
                output.description = "Update claim info is done!";
                output.transactionId = content.ticketNo;
                output.transactionDateTime = System.DateTime.Now;
                output.data = UpdateClaimInfoOutput;
                output.data.message = "ClaimNoti Number: " + content.claimNotiNo +
                    " Claim Number: " + content.claimNo +
                    " Claim Status code: " + content.claimStatusCode +
                    " Claim Status Description: " + content.claimStatusDesc;
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

        private string convertOptionSet(object entity, string fieldName, string value)
        {
            string valOption = "10000000" + value;
            return valOption;
        }

        /*
        //To use with log
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UpdateClaimInfoController));

        public object Post([FromBody]object value)
        {
            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: Get", CommonHelper.GetIpAddress());

            //var output = new EWIResponse();
            //if (!JsonHelper.TryValidateNullMessage(value, out output))
            //{
            //    return Request.CreateResponse<EWIResponse>(output);
            //}
            //var valueText = value.ToString();
            //var ewiRequest = JsonConvert.DeserializeObject<EWIRequest>(valueText);
            //var contentText = ewiRequest.content.ToString();
            //_logImportantMessage = "Username: {0}, Token: {1}, ";
            //_logImportantMessage = string.Format(_logImportantMessage, ewiRequest.username, ewiRequest.token);
            
            var output = new UpdateClaimInfoOutputModel();
            //if (!JsonHelper.TryValidateNullMessage(value, out output))
            //{
            //    return Request.CreateResponse<UpdateClaimStatusInputModel>(output);
            //}
            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<UpdateClaimInfoInputModel>(contentText);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/UpdateClaimStatus_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                _logImportantMessage += "Code: " + contentModel.claimNo;
                output = HandleMessage(contentText, contentModel);
            }
            else
            {
                output = new UpdateClaimInfoOutputModel();
                _log.Error(_logImportantMessage);
               // _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.responseCode, Environment.NewLine, output.responseMessage);
            }

            return Request.CreateResponse<UpdateClaimInfoOutputModel>(output);
        }

        private UpdateClaimInfoOutputModel HandleMessage(string valueText, UpdateClaimInfoInputModel content)
        {
            //TODO: Do what you want
            var output = new UpdateClaimInfoOutputModel();
            var updateClaimInfoOutput = new UpdateClaimInfoDataOutputModel();
            _log.Info("HandleMessage");
            try
            {
                //TODO: Do something
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
        */
    }
}
