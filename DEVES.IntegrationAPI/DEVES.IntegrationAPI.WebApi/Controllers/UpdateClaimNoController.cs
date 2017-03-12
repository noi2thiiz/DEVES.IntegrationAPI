using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.UpdateClaimNo;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class UpdateClaimNoController : ApiController
    {

        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UpdateClaimNoController));

        OrganizationServiceProxy _serviceProxy;
        private Guid _accountId;

        public object Post([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new UpdateClaimNoOutputModel_Pass();
            var outputFail = new UpdateClaimNoOutputModel_Fail();
            outputFail.data = new UpdateClaimNoDataOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<UpdateClaimNoInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/UpdateClaimNo_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new UpdateClaimNoOutputModel_Pass();
                _logImportantMessage += "ticketNo: " + contentModel.ticketNo;
                outputPass = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<UpdateClaimNoOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new UpdateClaimNoOutputModel_Fail();
                outputFail.data = new UpdateClaimNoDataOutputModel_Fail();
                outputFail.data.fieldErrors = new UpdateClaimNoFieldErrors();

                var dataFail = outputFail.data;
                dataFail.fieldErrors.name = "Invalid type of Input(s)";
                dataFail.fieldErrors.message = "Some of your input is invalid. Please recheck again.";

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", dataFail.fieldErrors.name, Environment.NewLine, dataFail.fieldErrors.message);
                return Request.CreateResponse<UpdateClaimNoOutputModel_Fail>(outputFail);

            }
        }
        private UpdateClaimNoOutputModel_Pass HandleMessage(string valueText, UpdateClaimNoInputModel content)
        {
            //TODO: Do what you want
            var output = new UpdateClaimNoOutputModel_Pass();

            _log.Info("HandleMessage");
            try
            {
                var UpdateClaimNoOutput = new UpdateClaimNoDataOutputModel_Pass();

                var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
                _serviceProxy = connection.OrganizationServiceProxy;
                ServiceContext svcContext = new ServiceContext(_serviceProxy);

                var query = from c in svcContext.IncidentSet
                            where c.TicketNumber == content.ticketNo && c.pfc_claim_noti_number == content.claimNotiNo
                            select c;
                Incident incident = query.FirstOrDefault<Incident>();

                _accountId = new Guid(incident.IncidentId.ToString());
                Incident retrievedIncident = (Incident)_serviceProxy.Retrieve(Incident.EntityLogicalName, _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

                try
                {
                    string DB_claimNumber = retrievedIncident.pfc_claim_number;
                    string DB_claimStatusCode = retrievedIncident.pfc_locus_claim_status_code;
                    string DB_claimStatusDesc = retrievedIncident.pfc_locus_claim_status_desc;
                    // retrievedIncident.pfc_claimId = content.claimId;
                    retrievedIncident.pfc_claim_number = content.claimNo;
                    retrievedIncident.pfc_locus_claim_status_code = content.claimStatusCode;
                    retrievedIncident.pfc_locus_claim_status_desc = content.claimStatusDesc;

                    _serviceProxy.Update(retrievedIncident);
                }
                catch (Exception e)
                {
                    output.description = "Retrieving data PROBLEM";
                    return output;
                }

                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "Update ClaimNo is done!";
                output.transactionId = content.ticketNo;
                output.transactionDateTime = DateTime.Now.ToString();
                UpdateClaimNoOutput.message = "ticketNo: " + content.ticketNo
                    + " claimNotiNo: " + content.claimNotiNo
                    + " claimNo: " + content.claimNo;
                output.data = UpdateClaimNoOutput;
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
                output.description = "Mapping Error";

            }

            return output;
        }
    }
}