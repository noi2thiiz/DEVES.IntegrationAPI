using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.AccidentPrilimSurveyorReport;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.Xrm.Sdk.Client;
using System.Configuration;
using Microsoft.Xrm.Tooling.Connector;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class AccidentPrilimSurveyorReportController : ApiController
    {

        //To use with log
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AccidentPrilimSurveyorReportController));

        OrganizationServiceProxy _serviceProxy;
        private Guid _accountId;

        public object Post([FromBody]object value)
        {
            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new AccidentPrilimSurveyorReportOutputModel_Pass();
            var outputFail = new AccidentPrilimSurveyorReportOutputModel_Fail();

            var contentText = value.ToString();
            //_logImportantMessage = string.Format(_logImportantMessage, output.transactionId);
            var contentModel = JsonConvert.DeserializeObject<AccidentPrilimSurveyorReportInputModel>(contentText);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/AccidentPrilimSurveyorReport_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                _logImportantMessage += "TicketNo: " + contentModel.ticketNo;
                outputPass = HandleMessage(contentText, contentModel);

                return Request.CreateResponse<AccidentPrilimSurveyorReportOutputModel_Pass>(outputPass);
            }
            else
            {

                outputFail = new AccidentPrilimSurveyorReportOutputModel_Fail();
                outputFail.data = new AccidentPrilimSurveyorReportDataOutputModel_Fail();
                outputFail.data.fieldErrors = new List<AccidentPrilimSurveyorReportFieldErrorsModel>();

                outputFail.message = "Failed";
                outputFail.description = "Validation json input is invalid!";

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorName: {0} {1} ErrorDescription: {1}", outputFail.message, Environment.NewLine, outputFail.description);

                return Request.CreateResponse<AccidentPrilimSurveyorReportOutputModel_Fail>(outputFail);
            }
        }

        private AccidentPrilimSurveyorReportOutputModel_Pass HandleMessage(string valueText, AccidentPrilimSurveyorReportInputModel content)
        {
            //TODO: Do what you want
            var output = new AccidentPrilimSurveyorReportOutputModel_Pass();
            var AccidentPrilimSurveyorReportOutput = new AccidentPrilimSurveyorReportDataOutputModel_Pass();
            _log.Info("HandleMessage");
            try
            {
                var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);

                //OrganizationServiceProxy _serviceProxy;
                //private Guid _accountId;
                _serviceProxy = connection.OrganizationServiceProxy;
                ServiceContext svcContext = new ServiceContext(_serviceProxy);
                
                try
                {
                    
                    // Incident (Case)
                    Incident incident = new Incident();
                    incident.pfc_accident_legal_result = new OptionSetValue(Int32.Parse(convertOptionSet(Incident.EntityLogicalName, "", content.eventDetailInfo.accidentLegalResult)));
                    incident.pfc_police_station = content.eventDetailInfo.policeStation;
                    incident.pfc_police_record_id = content.eventDetailInfo.policeRecordId;
                    incident.pfc_police_record_date = Convert.ToDateTime(content.eventDetailInfo.policeRecordDate);
                    incident.pfc_police_bail_flag = convertBool(content.eventDetailInfo.policeBailFlag);
                    incident.pfc_num_of_tow_truck = new OptionSetValue(Int32.Parse(convertOptionSet(Incident.EntityLogicalName, "", content.eventDetailInfo.numOfTowTruck.ToString())));
                    incident.pfc_num_of_accident_injuries = new OptionSetValue(Int32.Parse(convertOptionSet(Incident.EntityLogicalName, "", content.eventDetailInfo.numOfAccidentInjury.ToString())));
                    incident.pfc_num_of_death = new OptionSetValue(Int32.Parse(convertOptionSet(Incident.EntityLogicalName, "", content.eventDetailInfo.numOfDeath.ToString())));
                    incident.pfc_excess_fee = content.eventDetailInfo.excessFee;
                    incident.pfc_deductable_fee = content.eventDetailInfo.deductibleFee;
                    incident.pfc_accident_prilim_surveyor_report_date = content.reportAccidentResultDate;
                    // _serviceProxy.Create(incident);

                    // Motor Accident 
                    pfc_motor_accident motorAccident = new pfc_motor_accident();
                    //motorAccident.pfc_motor_accident_name = ""; // ให้เป็นไปตาม Logic. ของหน้า Motor Accident Parties
                    //motorAccident.pfc_parent_caseId = ""; //ให้ผูกกับ Case ด้านบน ??
                    motorAccident.pfc_activity_date = Convert.ToDateTime(content.eventDetailInfo.accidentOn);
                    //motorAccident.pfc_event_code = ""; // ให้เป็นไปตาม Logic. ของหน้า Motor Accident
                    motorAccident.pfc_ref_isurvey_eventid = content.eventId;
                    motorAccident.pfc_accident_event_detail = content.eventDetailInfo.accidentNatureDesc;
                    // motorAccident.pfc_more_document; // ไม่มี moreDocument
                    motorAccident.pfc_accident_latitude = content.eventDetailInfo.accidentLatitude;
                    motorAccident.pfc_accident_Longitude = content.eventDetailInfo.accidentLongitude;
                    motorAccident.pfc_accident_location = content.eventDetailInfo.accidentPlace;
                    motorAccident.pfc_accident_remark = content.eventDetailInfo.accidentRemark;
                    motorAccident.pfc_ref_isurvey_total_event = content.eventDetailInfo.totalEvent;
                    motorAccident.pfc_ref_isurvey_created_date = Convert.ToDateTime(content.eventDetailInfo.iSurveyCreatedDate);
                    motorAccident.pfc_ref_isurvey_modified_date = Convert.ToDateTime(content.eventDetailInfo.iSurveyModifiedDate);
                    motorAccident.pfc_ref_isurvey_isdeleted = convertBool(content.eventDetailInfo.iSurveyIsDeleted);
                    motorAccident.pfc_ref_isurvey_isdeleted_date = Convert.ToDateTime(content.eventDetailInfo.iSurveyIsDeletedDate);
                    motorAccident.pfc_motor_accident_parties_sum = content.eventDetailInfo.numOfAccidentParty;
                    // _serviceProxy.Create(motorAccident);

                    // Motor Accident Parties   
                    pfc_motor_accident_parties motorAccidentParties = new pfc_motor_accident_parties();
                    //motorAccidentParties.pfc_ref_isurvey_partiesid
                    //motorAccidentParties.pfc_ref_isurvey_eventid
                    //motorAccidentParties.pfc_ref_isurvey_eventitem
                    //motorAccidentParties.pfc_parties_fullname
                    //motorAccidentParties.pfc_parties_type // ไม่มี field นี้
                    //motorAccidentParties.pfc_licence_province
                    //motorAccidentParties.pfc_licence_no
                    //motorAccidentParties.pfc_brand
                    //motorAccidentParties.pfc_model
                    //motorAccidentParties.pfc_color
                    //motorAccidentParties.pfc_phoneno
                    //motorAccidentParties.pfc_insurance_name
                    //motorAccidentParties.pfc_policyno
                    //motorAccidentParties.pfc_policy_type
                    //motorAccidentParties.pfc_ref_isurvey_created_date
                    //motorAccidentParties.pfc_ref_isurvey_modified_date
                    //motorAccidentParties.pfc_ref_isurvey_isdeleted
                    //_serviceProxy.Create(MotorAccidentParties);

                    // Motor Accident Part
                    pfc_motor_accident_parts motorAccidentPart = new pfc_motor_accident_parts();
                    //motorAccidentPart.pfc_ref_isurvey_eventid
                    //motorAccidentPart.pfc_ref_isurvey_item
                    //motorAccidentPart.pfc_ref_isurvey_detailid
                    //motorAccidentPart.pfc_detail
                    //motorAccidentPart.pfc_damage_levels
                    //motorAccidentPart.pfc_is_repair
                    //motorAccidentPart.pfc_remark
                    //motorAccidentPart.pfc_ref_isurvey_created_date
                    //motorAccidentPart.pfc_ref_isurvey_modified_date
                    //motorAccidentPart.pfc_ref_isurvey_isdeleted
                    //motorAccidentPart.pfc_ref_isurvey_isdeleted_date
                    //motorAccidentPart.pfc_motor_accident_event_code // ให้เป็นไปตาม Logic. ของหน้า Motor Accident Parts
                    //motorAccidentPart.pfc_motor_accident_event_sequence // ให้เป็นไปตาม Logic. ของหน้า Motor Accident Parts
                    //motorAccidentPart.pfc_motor_accident_parties_parts_name // ให้เป็นไปตาม Logic. ของหน้า Motor Accident Parties Parts
                    //motorAccidentPart.pfc_motor_accident_partiesId // ให้ผูกกับ Motor Accident Parties ด้านบน
                    //_serviceProxy.Create(motorAccidentParty);

                    // Motor Accident Parties Part
                    pfc_motor_accident_parties_parts motorAccidentPartiesPart = new pfc_motor_accident_parties_parts();
                    var accPartiesPart = new List<ClaimDetailPartiesInfoModel>();
                    int cntApp = 0;
                    foreach(var val in content.claimDetailPartiesInfo)
                    {
                        motorAccidentPartiesPart.pfc_ref_isurvey_partiesid += content.claimDetailPartiesInfo[cntApp].claimDetailPartiesPartiesId;
                    }
                    //content.claimDetailPartiesInfo
                    //motorAccidentPartiesPart.pfc_ref_isurvey_partiesid
                    //motorAccidentPartiesPart.pfc_ref_isurvey_item
                    //motorAccidentPartiesPart.pfc_ref_isurvey_detailid
                    //motorAccidentPartiesPart.pfc_detail
                    //motorAccidentPartiesPart.pfc_damage_levels
                    //motorAccidentPartiesPart.pfc_is_repair
                    //motorAccidentPartiesPart.pfc_remark
                    //motorAccidentPartiesPart.pfc_ref_isurvey_created_date
                    //motorAccidentPartiesPart.pfc_ref_isurvey_modified_date
                    //motorAccidentPartiesPart.pfc_ref_isurvey_isdeleted
                    //motorAccidentPartiesPart.pfc_ref_isurvey_isdeleted_date
                    //motorAccidentPartiesPart.pfc_motor_accident_event_code // ให้เป็นไปตาม Logic. ของหน้า Motor Accident Parties Parts
                    //motorAccidentPartiesPart.pfc_motor_accident_event_sequence // ให้เป็นไปตาม Logic. ของหน้า Motor Accident Parties Parts
                    //motorAccidentPartiesPart.pfc_motor_accident_parties_sequence // ให้เป็นไปตาม Logic. ของหน้า Motor Accident Parties Parts
                    //_serviceProxy.Create(motorAccidentPartiesPart);

                }
                catch (Exception e)
                {
                    output.description = "Create Entity (Case/Motor) PROBLEM";
                    return output;
                }

                //TODO: Do something
                output.code = 200;
                output.message = "Success";

                output.data = AccidentPrilimSurveyorReportOutput;

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

                output.message = e.ToString();
            }

            return output;
        }

        private string convertOptionSet(object entity, string fieldName, string value)
        {
            string valOption;
            if (value.Length == 1)
            {
                valOption = "10000000" + value;
            }
            else
            {
                valOption = "1000000" + value;
            }

            return valOption;
        }

        private bool convertBool(string value)
        {
            if(value.Equals("Y") || value.Equals("0"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}