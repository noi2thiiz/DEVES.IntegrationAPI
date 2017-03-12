using DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP;
using DEVES.IntegrationAPI.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegClaimRequestFromRVPController : ApiController
    {
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(InquiryConsultingHistoryController));

        OrganizationServiceProxy _serviceProxy;
        private Guid _accountId;

        public object Post([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new RegClaimRequestFromRVPOutputModel_Pass();
            var outputFail = new RegClaimRequestFromRVPOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<RegClaimRequestFromRVPInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/RegClaimRequestFromRVP_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new RegClaimRequestFromRVPOutputModel_Pass();
                _logImportantMessage += "rvpCliamNo: " + contentModel.rvpCliamNo;
                outputPass = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<RegClaimRequestFromRVPOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new RegClaimRequestFromRVPOutputModel_Fail();
                outputFail.data = new RegClaimRequestFromRVPDataOutputModel_Fail();
                outputFail.data.fieldErrors = new RegClaimRequestFromRVPFieldErrors();

                var dataFail = outputFail.data;
                dataFail.fieldErrors.name = "Invalid Input(s)";
                dataFail.fieldErrors.message = "Some of your input is invalid. Please recheck again.";
                // dataFail.fieldError = "Field xxx is ";

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", dataFail.fieldErrors.name, Environment.NewLine, dataFail.fieldErrors.message);
                return Request.CreateResponse<RegClaimRequestFromRVPDataOutputModel_Fail>(dataFail);
            }
        }

        private RegClaimRequestFromRVPOutputModel_Pass HandleMessage(string valueText, RegClaimRequestFromRVPInputModel content)
        {
            //TODO: Do what you want
            var output = new RegClaimRequestFromRVPOutputModel_Pass();

            _log.Info("HandleMessage");

            try
            {
                var RegClaimRequestFromRVPOutput = new RegClaimRequestFromRVPDataOutputModel_Pass();

                var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
                _serviceProxy = connection.OrganizationServiceProxy;
                ServiceContext svcContext = new ServiceContext(_serviceProxy);

                try
                {

                    // 1. create case in CRM entity
                    if(content.accidentPartyInfo == null)
                    {
                        /*
                        // Create only "Case"
                        Incident incidentCase = new Incident();
                        incidentCase.pfc_surveyor_name = "Napat เอง";
                        incidentCase.pfc_accident_desc = "กรณีไม่มี accidentPartyInfo เลยสร้างแค่ Case";
                        _serviceProxy.Create(incidentCase);
                        */
                    }
                    else
                    {
                        /*
                        // Create 3 un
                        // Case
                        Incident incidentCase = new Incident();
                        incidentCase.pfc_surveyor_name = "Napat เอง";
                        incidentCase.pfc_accident_desc = "กรณีมี accidentPartyInfo เลยสร้าง Case, Motor Accident และ Motor Accident Parties";
                        _serviceProxy.Create(incidentCase);
                        */

                        // Motor Accident 
                        //pfc_motor_accident MotorAccident = new pfc_motor_accident();
                        //_serviceProxy.Create(MotorAccident);

                        // Motor Accident Parties
                        //pfc_motor_accident_parties MotorAccidentParties = new pfc_motor_accident_parties();
                        //_serviceProxy.Create(MotorAccidentParties);

                    }

                    // 2. execute stored

                    // 3. update vaule to crm form

                }
                catch (Exception e)
                {
                    output.description = "Create Motor Accident PROBLEM";
                    return output;
                }

                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "Update ClaimNo is done!";
                //output.transactionId = content.ticketNo;
                output.transactionId = "{0} ticketNo need to be added from stored";
                output.transactionDateTime = DateTime.Now.ToString();
                //RegClaimRequestFromRVPOutput.ticketNo = "ticketNo: " + content.ticketNo;
                RegClaimRequestFromRVPOutput.ticketNo = "{1} ticketNo need to be added from stored";
                RegClaimRequestFromRVPOutput.claimNotiNo = "{1} claimNotiNo need to be added from stored";
                output.data = RegClaimRequestFromRVPOutput;
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