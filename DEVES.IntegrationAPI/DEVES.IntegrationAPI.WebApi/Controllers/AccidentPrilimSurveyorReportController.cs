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
using System.Linq;

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

                List<string> errorMessage = JsonHelper.getReturnError();
                foreach (var text in errorMessage)
                {
                    string fieldMessage = "";
                    string fieldName = "";
                    if (text.Contains("Required properties"))
                    {
                        int indexEnd = 0;
                        for (int i = 0; i < text.Length -1 ; i++)
                        {
                            if (text.Substring(i, 1).Equals(":"))
                            {
                                fieldMessage = text.Substring(0, i);
                                indexEnd = i + 1;
                            }
                            if (text.Substring(i, 1).Equals("."))
                            {
                                fieldName = text.Substring(indexEnd, i - indexEnd).Trim();
                                break;
                            }
                        }

                        string[] splitProp = fieldName.Split(',');
                        for(int i = 0; i < splitProp.Length; i++)
                        {
                            outputFail.data.fieldErrors.Add(new AccidentPrilimSurveyorReportFieldErrorsModel(splitProp[i].Trim(), fieldMessage));
                        }
                    }
                    else if (text.Contains("exceeds maximum length"))
                    {
                        bool isMessage = false;
                        int endMessage = 0;
                        int startName = 0;
                        int endName = 0;
                        for (int i = 0; i < text.Length - 4; i++)
                        {
                            if (text.Substring(i, 4).Equals("Path"))
                            {
                                fieldMessage = text.Substring(0, i - 1);
                                isMessage = true;
                                endMessage = i + "Path".Length;
                            }
                            if (isMessage)
                            {
                                if (text.Substring(i, 1).Equals("'"))
                                {
                                    if (startName == 0)
                                    {
                                        startName = i + 1;
                                    }
                                    else if (endName == 0)
                                    {
                                        endName = i - 1;
                                    }
                                }
                                if (startName != 0 && endName != 0)
                                {
                                    fieldName = text.Substring(startName, i - startName).Trim();
                                    break;
                                }
                            }
                        }
                    }
                    else if (text.Contains("minimum length"))
                    {
                        bool isMessage = false;
                        int startName = 0;
                        int endName = 0;
                        for (int i = 0; i < text.Length - 7; i++)
                        {
                            string check = text.Substring(i, 7);
                            if (text.Substring(i, 7).Equals("minimum"))
                            {
                                fieldMessage = "Required field must not be null";
                                isMessage = true;
                            }
                            if (isMessage)
                            {
                                if (text.Substring(i, 1).Equals("'"))
                                {
                                    if (startName == 0)
                                    {
                                        startName = i + 1;
                                    }
                                    else if (endName == 0)
                                    {
                                        endName = i - 1;
                                    }
                                }
                                if (startName != 0 && endName != 0)
                                {
                                    fieldName = text.Substring(startName, i - startName).Trim();
                                    break;
                                }
                            }
                        }
                    }
                    else if (text.Contains("Invalid type."))
                    {
                        int startIndex = "Invalid type.".Length;
                        int endMessage = 0;
                        int startName = 0;
                        int endName = 0;
                        for (int i = startIndex; i < text.Length - 1; i++)
                        {
                            if (text.Substring(i, 1).Equals("."))
                            {
                                fieldMessage = text.Substring(0, i);
                                endMessage = i + 1;
                            }
                            if (text.Substring(i, 1).Equals("'"))
                            {
                                if (startName == 0)
                                {
                                    startName = i + 1;
                                }
                                else if (endName == 0)
                                {
                                    endName = i - 1;
                                }
                            }
                            if (startName != 0 && endName != 0)
                            {
                                fieldName = text.Substring(startName, i - startName).Trim();
                                break;
                            }
                        }
                    }
                    else if (text.Contains("not defined in enum"))
                    {
                        int startName = 0;
                        int endName = 0;
                        bool isChecking = true;
                        for (int i = 0; i < text.Length - 1; i++)
                        {
                            if (text.Substring(i, 1).Equals(".") && isChecking)
                            {
                                fieldMessage = text.Substring(0, i);
                                isChecking = false;
                            }
                            if (text.Substring(i, 1).Equals("'"))
                            {
                                if (startName == 0)
                                {
                                    startName = i + 1;
                                }
                                else if (endName == 0)
                                {
                                    endName = i - 1;
                                }
                            }
                            if (startName != 0 && endName != 0)
                            {
                                fieldName = text.Substring(startName, i - startName).Trim();
                                break;
                            }
                        }
                    }

                    if (!text.Contains("Required properties"))
                    {
                        outputFail.data.fieldErrors.Add(new AccidentPrilimSurveyorReportFieldErrorsModel(fieldName, fieldMessage));
                    }
                        
                }

                outputFail.code = "400";
                outputFail.message = "Invalid Input(s)";
                outputFail.description = "Some of your input is invalid. Please recheck again.";
                outputFail.transactionId = "Ticket ID: " + contentModel.ticketNo + ", Claim Noti No: " + contentModel.claimNotiNo;
                outputFail.transactionDateTime = DateTime.Now.ToString();


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
                _serviceProxy = connection.OrganizationServiceProxy;
                ServiceContext svcContext = new ServiceContext(_serviceProxy);

                var query = from c in svcContext.IncidentSet
                            where c.pfc_claim_noti_number == content.claimNotiNo
                            select c;

                Incident incident = query.FirstOrDefault<Incident>();
                // Incident GUID
                _accountId = new Guid(incident.IncidentId.ToString());
                // Motor Accident GUID
                Guid _motorId = new Guid();

                Incident retrievedIncident = (Incident)_serviceProxy.Retrieve(Incident.EntityLogicalName, _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

                if (retrievedIncident.pfc_isurvey_status.Value < 100000070)
                {
                    output.code = "500";
                    output.message = "False";
                    output.description = "iSurvey Status ไม่ได้อยู่ในสถานะเสร็จสมบูรณ์";
                    output.transactionId = "";
                    output.transactionDateTime = DateTime.Now.ToString();

                    output.data = new AccidentPrilimSurveyorReportDataOutputModel_Pass();
                    output.data.message = null;

                    return output;
                }
                
                // Incident
                try
                {
                    if(content.eventDetailInfo.numOfTowTruck > 20)
                    {
                        content.eventDetailInfo.numOfTowTruck = 20;
                    }
                    if (content.eventDetailInfo.numOfAccidentInjury > 20)
                    {
                        content.eventDetailInfo.numOfAccidentInjury = 20;
                    }
                    if (content.eventDetailInfo.numOfDeath > 20)
                    {
                        content.eventDetailInfo.numOfDeath = 20;
                    }

                    retrievedIncident.pfc_accident_legal_result = new OptionSetValue(Int32.Parse(convertOptionSet(Incident.EntityLogicalName, "", content.eventDetailInfo.accidentLegalResult)));
                    retrievedIncident.pfc_police_station = content.eventDetailInfo.policeStation;
                    retrievedIncident.pfc_police_record_id = content.eventDetailInfo.policeRecordId;
                    retrievedIncident.pfc_police_record_date = convertDateTime(content.eventDetailInfo.policeRecordDate);
                    retrievedIncident.pfc_police_bail_flag = convertBool(content.eventDetailInfo.policeBailFlag);
                    retrievedIncident.pfc_num_of_tow_truck = new OptionSetValue(Int32.Parse(convertOptionSet(Incident.EntityLogicalName, "", content.eventDetailInfo.numOfTowTruck.ToString())));
                    retrievedIncident.pfc_num_of_accident_injuries = new OptionSetValue(Int32.Parse(convertOptionSet(Incident.EntityLogicalName, "", content.eventDetailInfo.numOfAccidentInjury.ToString())));
                    retrievedIncident.pfc_num_of_death = new OptionSetValue(Int32.Parse(convertOptionSet(Incident.EntityLogicalName, "", content.eventDetailInfo.numOfDeath.ToString())));
                    retrievedIncident.pfc_excess_fee = (decimal)content.eventDetailInfo.excessFee;
                    retrievedIncident.pfc_deductable_fee = (decimal)content.eventDetailInfo.deductibleFee;
                    // retrievedIncident.pfc_deductable_fee = content.eventDetailInfo.fee;
                    retrievedIncident.pfc_accident_prilim_surveyor_report_date = convertDateTime(content.reportAccidentResultDate);
                    retrievedIncident.pfc_isurvey_status = new OptionSetValue(Int32.Parse("100000071"));
                    retrievedIncident.pfc_isurvey_status_on = DateTime.Now;
                    retrievedIncident.pfc_motor_accident_sum = 1;


                    _serviceProxy.Update(retrievedIncident);
                }
                catch(Exception e)
                {
                    output.code = "501";
                    output.message = "Update Entity Incident PROBLEM"; ;
                    output.description = e.ToString();
                    output.transactionId = "";
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = new AccidentPrilimSurveyorReportDataOutputModel_Pass();
                    output.data.message = e.StackTrace;

                    return output;
                }

                // Motor Accident 
                try
                {
                    var query_motorAcc = from c in svcContext.pfc_motor_accidentSet
                                         where c.pfc_parent_caseId.Id == _accountId
                                         select c;
                    pfc_motor_accident motorAccident1 = query_motorAcc.FirstOrDefault<pfc_motor_accident>();
                    
                    motorAccident1.pfc_motor_accident_name = "เหตุการณ์ที่ 1";
                    //motorAccident1.pfc_parent_caseId = new Microsoft.Xrm.Sdk.EntityReference(Incident.EntityLogicalName, _accountId);
                    //motorAccident1.pfc_activity_date = convertDateTime(content.eventDetailInfo.accidentOn);
                    motorAccident1.pfc_event_code = content.eventId; // ให้เป็นไปตาม Logic. ของหน้า Motor Accident
                    motorAccident1.pfc_event_sequence = 1;
                    motorAccident1.pfc_ref_isurvey_eventid = content.eventId;
                    motorAccident1.pfc_accident_event_detail = content.eventDetailInfo.accidentNatureDesc;
                    //motorAccident1.pfc_more_document; // ไม่มี moreDocument
                    motorAccident1.pfc_accident_latitude = content.eventDetailInfo.accidentLatitude;
                    motorAccident1.pfc_accident_Longitude = content.eventDetailInfo.accidentLongitude;
                    motorAccident1.pfc_accident_location = content.eventDetailInfo.accidentPlace;
                    motorAccident1.pfc_accident_remark = content.eventDetailInfo.accidentRemark;
                    motorAccident1.pfc_ref_isurvey_total_event = content.eventDetailInfo.totalEvent;
                    motorAccident1.pfc_ref_isurvey_created_date = convertDateTime(content.eventDetailInfo.iSurveyCreatedDate);
                    motorAccident1.pfc_ref_isurvey_modified_date = convertDateTime(content.eventDetailInfo.iSurveyModifiedDate);
                    motorAccident1.pfc_ref_isurvey_isdeleted = convertBool(content.eventDetailInfo.iSurveyIsDeleted);
                    motorAccident1.pfc_ref_isurvey_isdeleted_date = convertDateTime(content.eventDetailInfo.iSurveyIsDeletedDate);
                    motorAccident1.pfc_motor_accident_parties_sum = content.eventDetailInfo.numOfAccidentParty;

                    svcContext.UpdateObject(motorAccident1);
                    svcContext.SaveChanges();
                    // _serviceProxy.Update(motorAccident1);
                    _motorId = motorAccident1.Id;
                    /*
                    pfc_motor_accident motorAccident = new pfc_motor_accident();
                    motorAccident.pfc_motor_accident_name = "เหตุการณ์ที่ 1";
                    motorAccident.pfc_parent_caseId = new Microsoft.Xrm.Sdk.EntityReference(Incident.EntityLogicalName, _accountId);
                    motorAccident.pfc_activity_date = convertDateTime(content.eventDetailInfo.accidentOn);
                    motorAccident.pfc_event_code = content.eventId; // ให้เป็นไปตาม Logic. ของหน้า Motor Accident
                    motorAccident.pfc_event_sequence = 1;
                    motorAccident.pfc_ref_isurvey_eventid = content.eventId;
                    motorAccident.pfc_accident_event_detail = content.eventDetailInfo.accidentNatureDesc;
                    //motorAccident.pfc_more_document; // ไม่มี moreDocument
                    motorAccident.pfc_accident_latitude = content.eventDetailInfo.accidentLatitude;
                    motorAccident.pfc_accident_Longitude = content.eventDetailInfo.accidentLongitude;
                    motorAccident.pfc_accident_location = content.eventDetailInfo.accidentPlace;
                    motorAccident.pfc_accident_remark = content.eventDetailInfo.accidentRemark;
                    motorAccident.pfc_ref_isurvey_total_event = content.eventDetailInfo.totalEvent;
                    motorAccident.pfc_ref_isurvey_created_date = convertDateTime(content.eventDetailInfo.iSurveyCreatedDate);
                    motorAccident.pfc_ref_isurvey_modified_date = convertDateTime(content.eventDetailInfo.iSurveyModifiedDate);
                    motorAccident.pfc_ref_isurvey_isdeleted = convertBool(content.eventDetailInfo.iSurveyIsDeleted);
                    motorAccident.pfc_ref_isurvey_isdeleted_date = convertDateTime(content.eventDetailInfo.iSurveyIsDeletedDate);
                    motorAccident.pfc_motor_accident_parties_sum = content.eventDetailInfo.numOfAccidentParty;

                    Guid motorId = _serviceProxy.Create(motorAccident);

                    // getting GUID of motorAccident and store in "_motorId" variable
                    _motorId = motorId;

                    */
                }
                catch (Exception e)
                {
                    output.code = "501";
                    output.message = "Update Entity Motor Accident PROBLEM";
                    output.description = e.ToString();
                    output.transactionId = "";
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = new AccidentPrilimSurveyorReportDataOutputModel_Pass();
                    output.data.message = e.StackTrace;

                    return output;
                }

                // Motor Accident Parties  
                try
                {
                    List<PartiesInfoModel> party = content.partiesInfo;

                    foreach (PartiesInfoModel a in party)
                    {
                        pfc_motor_accident_parties motorAccidentParties = new pfc_motor_accident_parties();
                        motorAccidentParties.pfc_parent_motor_accidentId = new EntityReference(pfc_motor_accident_parties.EntityLogicalName, _motorId);

                        motorAccidentParties.pfc_motor_accident_parties_name = a.partiesFullname;
                        motorAccidentParties.pfc_ref_isurvey_partiesid = a.partiesId;
                        motorAccidentParties.pfc_ref_isurvey_eventid = a.partiesEventId;
                        motorAccidentParties.pfc_ref_isurvey_eventitem = a.partiesEventItem;
                        motorAccidentParties.pfc_event_code = content.eventId;
                        motorAccidentParties.pfc_event_sequence = 1;
                        if(motorAccidentParties.pfc_parties_sequence < 1 || motorAccidentParties.pfc_parties_sequence == null)
                        {
                            motorAccidentParties.pfc_parties_sequence = 1;
                        }
                        else
                        {
                            motorAccidentParties.pfc_parties_sequence += 1;
                        }
                        motorAccidentParties.pfc_parties_fullname = a.partiesFullname;
                        motorAccidentParties.pfc_parties_type = new OptionSetValue(Int32.Parse(a.partiesType.ToString())); // ไม่มี field นี้
                        motorAccidentParties.pfc_licence_province = a.partiesCarPlateProv;
                        motorAccidentParties.pfc_licence_no = a.partiesCarPlateNo;
                        motorAccidentParties.pfc_brand = a.partiesCarBrand;
                        motorAccidentParties.pfc_model = a.partiesCarModels;
                        motorAccidentParties.pfc_color = a.partiesCarColor;
                        motorAccidentParties.pfc_phoneno = a.partiesPartyPhone;
                        motorAccidentParties.pfc_insurance_name = a.partiesInsuranceCompany;
                        motorAccidentParties.pfc_policyno = a.partiesPolicyNumber;
                        motorAccidentParties.pfc_policy_type = a.partiesPolicyType;
                        motorAccidentParties.pfc_ref_isurvey_created_date = convertDateTime(a.partiesCreatedDate);
                        motorAccidentParties.pfc_ref_isurvey_modified_date = convertDateTime(a.partiesModifiedDate);
                        motorAccidentParties.pfc_ref_isurvey_isdeleted = convertBool(a.partiesIsDeleted);
                        motorAccidentParties.pfc_ref_isurvey_isdeleted_date = convertDateTime(a.partiesIsDeletedDate);

                        Guid partiesID = _serviceProxy.Create(motorAccidentParties);

                        // (Motor Accident Part) get "pfc_ref_isurvey_partiesid"
                        string partyId = motorAccidentParties.pfc_ref_isurvey_partiesid;
                        // Motor Accident Parties Part
                        try
                        {
                            List<ClaimDetailPartiesInfoModel> partiesPart = content.claimDetailPartiesInfo;

                            foreach (ClaimDetailPartiesInfoModel data in partiesPart)
                            {
                                if (data.claimDetailPartiesPartiesId == partyId)
                                {
                                    pfc_motor_accident_parties_parts motorAccidentPartiesPart = new pfc_motor_accident_parties_parts();
                                    motorAccidentPartiesPart.pfc_motor_accident_partiesId = new EntityReference(pfc_motor_accident_parties_parts.EntityLogicalName, partiesID);

                                    motorAccidentPartiesPart.pfc_motor_accident_parties_parts_name = data.claimDetailPartiesDetail; // motorAccidentPartiesPart.pfc_detail;
                                    motorAccidentPartiesPart.pfc_ref_isurvey_partiesid = data.claimDetailPartiesPartiesId;
                                    motorAccidentPartiesPart.pfc_ref_isurvey_item = data.claimDetailPartiesItem;
                                    motorAccidentPartiesPart.pfc_ref_isurvey_detailid = data.claimDetailPartiesDetailId;
                                    motorAccidentPartiesPart.pfc_detail = data.claimDetailPartiesDetail;
                                    motorAccidentPartiesPart.pfc_damage_levels = new OptionSetValue(Int32.Parse(convertClaimDetailLevels(data.claimDetailPartiesLevels)));
                                    motorAccidentPartiesPart.pfc_is_repair = convertBool(data.claimDetailPartieslIsRepair);
                                    motorAccidentPartiesPart.pfc_remark = data.claimDetailPartiesRemark;
                                    motorAccidentPartiesPart.pfc_ref_isurvey_created_date = convertDateTime(data.claimDetailPartiesCreatedDate);
                                    motorAccidentPartiesPart.pfc_ref_isurvey_modified_date = convertDateTime(data.claimDetailPartiesModifiedDate);
                                    motorAccidentPartiesPart.pfc_ref_isurvey_isdeleted = convertBool(data.claimDetailPartiesIsDeleted);
                                    motorAccidentPartiesPart.pfc_ref_isurvey_isdeleted_date = convertDateTime(data.claimDetailPartiesIsDeletedDate);
                                    motorAccidentPartiesPart.pfc_motor_accident_event_code = content.eventId; 
                                    motorAccidentPartiesPart.pfc_motor_accident_event_sequence = 1;
                                    if(motorAccidentPartiesPart.pfc_motor_accident_parties_sequence < 1 || motorAccidentPartiesPart.pfc_motor_accident_parties_sequence == null)
                                    {
                                        motorAccidentPartiesPart.pfc_motor_accident_parties_sequence = 1;
                                    } 
                                    else
                                    {
                                        motorAccidentPartiesPart.pfc_motor_accident_parties_sequence += 1;
                                    }
                                        
                                    _serviceProxy.Create(motorAccidentPartiesPart);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            output.code = "501";
                            output.message = "Create Entity (Case/Motor) PROBLEM";
                            output.description = e.ToString();
                            output.transactionId = "";
                            output.transactionDateTime = DateTime.Now.ToString();
                            output.data = new AccidentPrilimSurveyorReportDataOutputModel_Pass();
                            output.data.message = e.StackTrace;

                            return output;
                        }

                    }
                }
                catch (Exception e)
                {
                    output.code = "501";
                    output.message = "Create Entity Motor Accident Parties PROBLEM";
                    output.description = e.ToString();
                    output.transactionId = "";
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = new AccidentPrilimSurveyorReportDataOutputModel_Pass();
                    output.data.message = e.StackTrace;

                    return output;
                }

                // Motor Accident Part (ผูกกับ parties)
                try
                {
                    List<ClaimDetailInfoModel> parts = content.claimDetailInfo;

                    foreach(ClaimDetailInfoModel a in parts)
                    {
                        pfc_motor_accident_parts motorAccidentPart = new pfc_motor_accident_parts();

                        motorAccidentPart.pfc_motor_accidentId = new EntityReference(pfc_motor_accident_parts.EntityLogicalName, _motorId);
                        motorAccidentPart.pfc_motor_accident_parts_name = a.claimDetailDetail; // motorAccidentPart.pfc_detail;

                        motorAccidentPart.pfc_ref_isurvey_eventid = a.claimDetailEventId;
                        motorAccidentPart.pfc_ref_isurvey_item = a.claimDetailItem;
                        motorAccidentPart.pfc_ref_isurvey_detailid = a.claimDetailDetailid;
                        motorAccidentPart.pfc_detail = a.claimDetailDetail;
                        motorAccidentPart.pfc_damage_levels = new OptionSetValue(Int32.Parse(convertClaimDetailLevels(a.claimDetailLevels)));
                        motorAccidentPart.pfc_is_repair = convertBool(a.claimDetailIsRepair);
                        motorAccidentPart.pfc_remark = a.claimDetailRemark;
                        motorAccidentPart.pfc_ref_isurvey_created_date = convertDateTime(a.claimDetailCreatedDate);
                        motorAccidentPart.pfc_ref_isurvey_modified_date = convertDateTime(a.claimDetailModifiedDate);
                        motorAccidentPart.pfc_ref_isurvey_isdeleted = convertBool(a.claimDetailIsDeleted);
                        motorAccidentPart.pfc_ref_isurvey_isdeleted_date = convertDateTime(a.claimDetailIsDeletedDate);
                        motorAccidentPart.pfc_motor_accident_event_code = content.eventId;
                        motorAccidentPart.pfc_motor_accident_event_sequence = 1;

                        _serviceProxy.Create(motorAccidentPart);

                    }

                }
                catch (Exception e)
                {
                    output.code = "501";
                    output.message = "Create Entity Motor Accident Part PROBLEM";
                    output.description = e.ToString();
                    output.transactionId = "";
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = new AccidentPrilimSurveyorReportDataOutputModel_Pass();
                    output.data.message = e.StackTrace;

                    return output;
                }
                

                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "AccidentPrilimSurveyorReportOutput is done!";
                output.transactionId = "";
                //output.transactionId = Request.Properties["TransactionID"].ToString();
                output.transactionDateTime = DateTime.Now.ToString();
                output.data = AccidentPrilimSurveyorReportOutput;
                output.data.message = "Ticket ID: " + content.ticketNo + ", Claim Noti No: " + content.claimNotiNo;

            }
            catch (System.ServiceModel.FaultException e)
            {
                output.code = "500";
                output.message = "CRM PROBLEM";
                output.description = e.ToString();
                output.transactionId = "";
                output.transactionDateTime = DateTime.Now.ToString();
                output.data = new AccidentPrilimSurveyorReportDataOutputModel_Pass();
                output.data.message = e.StackTrace;

                return output;
            }
            catch (Exception e)
            {
                var errorMessage = e.GetType().FullName + ": " + null + Environment.NewLine;
                errorMessage += "StackTrace: " + e.StackTrace;

                if (e.InnerException != null)
                {
                    errorMessage += Environment.NewLine + "(InnerException)" + e.InnerException.GetType().FullName + " - " + e.InnerException.Message + Environment.NewLine;
                    errorMessage += "StackStrace: " + e.InnerException.StackTrace;
                }
                _log.Error("RequestId - " + _logImportantMessage);
                _log.Error(errorMessage);

                output.code = "400";
                output.message = "False";
                output.description = "ไม่พบ claimNotiNo";
                output.transactionId = "Claim Noti No: null";
                output.transactionDateTime = DateTime.Now.ToString();
                output.data = new AccidentPrilimSurveyorReportDataOutputModel_Pass();
                output.data.message = e.StackTrace;
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

        private string convertClaimDetailLevels(string value)
        {
            string valOption = null;

            if (value.Equals("S"))
            {
                valOption = "100000000";
            }
            else if (value.Equals("M"))
            {
                valOption = "100000001";
            }
            else if (value.Equals("H"))
            {
                valOption = "100000002";
            }
            else if (value.Equals("X"))
            {
                valOption = "100000003";
            }

            return valOption;
        }

        private DateTime convertDateTime(string dt)
        {
            DateTime datetime = new DateTime();

            if (dt == null)
            {
                // do nothing (ไม่อัพเดต)
            }
            else if (dt != null)
            {
                if (Convert.ToDateTime(dt).Year < 1900)
                {
                    // do nothing (ไม่อัพเดต)
                }
                else
                {
                    datetime = Convert.ToDateTime(dt);
                }
            }

            return datetime;
        }

    }
}