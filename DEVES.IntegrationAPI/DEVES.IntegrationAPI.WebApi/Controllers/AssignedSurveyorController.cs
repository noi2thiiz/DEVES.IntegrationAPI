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
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using System.Linq;

// using earlybound;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class AssignedSurveyorController : ApiController
    {

        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AssignedSurveyorController));

        OrganizationServiceProxy _serviceProxy;
        private Guid _accountId;

        public object Post([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new AssignedSurveyorOutputModel_Pass();
            var outputFail = new AssignedSurveyorOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<AssignedSurveyorInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/AssignedSurveyorInfo_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new AssignedSurveyorOutputModel_Pass();
                _logImportantMessage += "TicketNo: " + contentModel.ticketNo;
                outputPass = HandleMessage(contentText, contentModel);
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
                fieldError.message = "Some of your input is invalid. Please recheck again.";

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

                var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
                // var connection = new CrmServiceClient(System.Configuration.ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);

                _serviceProxy = connection.OrganizationServiceProxy;
                // _accountId = content.

                ServiceContext svcContext = new ServiceContext(_serviceProxy);

                // Incident incident = new earlybound.Incident();
                var query = from c in svcContext.IncidentSet
                            where c.pfc_claim_noti_number == content.claimNotiNo
                            select c;

                Incident incident = query.FirstOrDefault<Incident>();
                _accountId = new Guid(incident.IncidentId.ToString());

                // Incident retrievedIncident = (Incident)_serviceProxy.Retrieve(Incident.EntityLogicalName, new Guid(header.gid), new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                Incident retrievedIncident = (Incident)_serviceProxy.Retrieve(Incident.EntityLogicalName, _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                try
                {
                    
                    retrievedIncident.pfc_survey_meeting_province = content.surveyMeetingProvince;
                    retrievedIncident.pfc_survey_meeting_place = content.surveyMeetingPlace;
                    retrievedIncident.pfc_survey_meeting_date = content.surveyMeetingDate;
                    retrievedIncident.pfc_surveyor_code = content.surveyorCode;
                    retrievedIncident.pfc_surveyor_client_number = content.surveyorClientNumber;
                    retrievedIncident.pfc_surveyor_name = content.surveyorName;
                    retrievedIncident.pfc_surveyor_company_name = content.surveyorCompanyName;
                    retrievedIncident.pfc_surveyor_company_mobile = content.surveyorCompanyMobile;
                    retrievedIncident.pfc_surveyor_type = new OptionSetValue(Int32.Parse(convertOptionSet(incident, "pfc_surveyor_type", content.surveyType)));
                    //retrievedIncident.pfc_surveyor_type = new OptionSetValue(Convert.ToInt32(content.surveyType));
                    retrievedIncident.pfc_isurvey_status = new OptionSetValue(Int32.Parse("100000015"));
                    retrievedIncident.pfc_isurvey_status_on = DateTime.Now;

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
            
            catch (System.ServiceModel.FaultException e)
            {
                output.description = "CRM PROBLEM";
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

        private string convertOptionSet(object entity, string fieldName, string value)
        {
            string valOption = "10000000" + value;
            return valOption;
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