using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.AccidentPrilimSurveyorReport;
using System.Configuration;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Client;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzAccidentPrilimSurveyorReport : BuzCommand
    {
        public override BaseDataModel ExecuteInput(object input)
        {
            // Deserialize Input
            AccidentPrilimSurveyorReportInputModel contentInput = (AccidentPrilimSurveyorReportInputModel)input;

            // Preparation Variable
            AccidentPrilimSurveyorReportOutputModel_Pass output = new AccidentPrilimSurveyorReportOutputModel_Pass();

            // Preparation Linq query to CRM
            ServiceContext svcContext;
            var _serviceProxy = GetOrganizationServiceProxy(out svcContext);

            var query = from c in svcContext.IncidentSet
                        where c.pfc_claim_noti_number == contentInput.claimNotiNo
                        select c;

            if(query.FirstOrDefault<Incident>() == null)
            {
                output.code = CONST_CODE_FAILED;
                output.message = "ไม่สามารถ Update ได้";
                output.description = "claimNotiNo ไม่มีในระบบ CRM";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now.ToString();

                return output;
            }
            else
            {
                Incident incident = query.FirstOrDefault<Incident>();
                // Incident GUID
                Guid _accountId = new Guid(incident.IncidentId.ToString());
                // Motor Accident GUID
                Guid _motorId = new Guid();
                Incident retrievedIncident = (Incident)_serviceProxy.Retrieve(Incident.EntityLogicalName, _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

                try
                {
                    if (contentInput.eventDetailInfo.numOfTowTruck > 20)
                    {
                        contentInput.eventDetailInfo.numOfTowTruck = 20;
                    }
                    if (contentInput.eventDetailInfo.numOfAccidentInjury > 20)
                    {
                        contentInput.eventDetailInfo.numOfAccidentInjury = 20;
                    }
                    if (contentInput.eventDetailInfo.numOfDeath > 20)
                    {
                        contentInput.eventDetailInfo.numOfDeath = 20;
                    }

                    retrievedIncident.pfc_accident_legal_result = new Microsoft.Xrm.Sdk.OptionSetValue(Int32.Parse(convertOptionSet(Incident.EntityLogicalName, "", contentInput.eventDetailInfo.accidentLegalResult)));
                    retrievedIncident.pfc_police_station = contentInput.eventDetailInfo.policeStation;
                    retrievedIncident.pfc_police_record_id = contentInput.eventDetailInfo.policeRecordId;
                    retrievedIncident.pfc_police_record_date = convertDateTime(contentInput.eventDetailInfo.policeRecordDate);
                    retrievedIncident.pfc_police_bail_flag = convertBool(contentInput.eventDetailInfo.policeBailFlag);
                    retrievedIncident.pfc_num_of_tow_truck = new Microsoft.Xrm.Sdk.OptionSetValue(Int32.Parse(convertOptionSet(Incident.EntityLogicalName, "", contentInput.eventDetailInfo.numOfTowTruck.ToString())));
                    retrievedIncident.pfc_num_of_accident_injuries = new Microsoft.Xrm.Sdk.OptionSetValue(Int32.Parse(convertOptionSet(Incident.EntityLogicalName, "", contentInput.eventDetailInfo.numOfAccidentInjury.ToString())));
                    retrievedIncident.pfc_num_of_death = new Microsoft.Xrm.Sdk.OptionSetValue(Int32.Parse(convertOptionSet(Incident.EntityLogicalName, "", contentInput.eventDetailInfo.numOfDeath.ToString())));
                    retrievedIncident.pfc_excess_fee = (decimal)contentInput.eventDetailInfo.excessFee;
                    retrievedIncident.pfc_deductable_fee = (decimal)contentInput.eventDetailInfo.deductibleFee;
                    retrievedIncident.pfc_accident_prilim_surveyor_report_date = convertDateTime(contentInput.reportAccidentResultDate);
                    if (retrievedIncident.pfc_isurvey_status.Value >= 100000071)
                    {
                        // ถ้าค่าใน crm มากกว่า 71 อยู่แล้ว ไม่ต้องปรับ
                    }
                    else
                    {
                        retrievedIncident.pfc_isurvey_status = new Microsoft.Xrm.Sdk.OptionSetValue(Int32.Parse("100000071"));
                    }
                    retrievedIncident.pfc_isurvey_status_on = DateTime.Now;
                    retrievedIncident.pfc_motor_accident_sum = 1;


                    _serviceProxy.Update(retrievedIncident);
                }
                catch (Exception e)
                {
                    output.code = "501";
                    output.message = "False: Update Entity Incident PROBLEM"; ;
                    output.description = e.ToString();
                    output.transactionId = TransactionId;
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
                    //motorAccident1.pfc_activity_date = convertDateTime(contentInput.eventDetailInfo.accidentOn);
                    motorAccident1.pfc_event_code = contentInput.eventId; // ให้เป็นไปตาม Logic. ของหน้า Motor Accident
                    motorAccident1.pfc_event_sequence = 1;
                    motorAccident1.pfc_ref_isurvey_eventid = contentInput.eventId;
                    motorAccident1.pfc_accident_event_detail = contentInput.eventDetailInfo.accidentNatureDesc;
                    //motorAccident1.pfc_more_document; // ไม่มี moreDocument
                    motorAccident1.pfc_accident_latitude = contentInput.eventDetailInfo.accidentLatitude;
                    motorAccident1.pfc_accident_Longitude = contentInput.eventDetailInfo.accidentLongitude;
                    motorAccident1.pfc_accident_location = contentInput.eventDetailInfo.accidentPlace;
                    motorAccident1.pfc_accident_remark = contentInput.eventDetailInfo.accidentRemark;
                    motorAccident1.pfc_ref_isurvey_total_event = contentInput.eventDetailInfo.totalEvent;
                    motorAccident1.pfc_ref_isurvey_created_date = convertDateTime(contentInput.eventDetailInfo.iSurveyCreatedDate);
                    motorAccident1.pfc_ref_isurvey_modified_date = convertDateTime(contentInput.eventDetailInfo.iSurveyModifiedDate);
                    motorAccident1.pfc_ref_isurvey_isdeleted = convertBool(contentInput.eventDetailInfo.iSurveyIsDeleted);
                    motorAccident1.pfc_ref_isurvey_isdeleted_date = convertDateTime(contentInput.eventDetailInfo.iSurveyIsDeletedDate);
                    motorAccident1.pfc_motor_accident_parties_sum = contentInput.eventDetailInfo.numOfAccidentParty;

                    svcContext.UpdateObject(motorAccident1);
                    svcContext.SaveChanges();
                    // _serviceProxy.Update(motorAccident1);
                    _motorId = motorAccident1.Id;
                    /*
                    pfc_motor_accident motorAccident = new pfc_motor_accident();
                    motorAccident.pfc_motor_accident_name = "เหตุการณ์ที่ 1";
                    motorAccident.pfc_parent_caseId = new Microsoft.Xrm.Sdk.EntityReference(Incident.EntityLogicalName, _accountId);
                    motorAccident.pfc_activity_date = convertDateTime(contentInput.eventDetailInfo.accidentOn);
                    motorAccident.pfc_event_code = contentInput.eventId; // ให้เป็นไปตาม Logic. ของหน้า Motor Accident
                    motorAccident.pfc_event_sequence = 1;
                    motorAccident.pfc_ref_isurvey_eventid = contentInput.eventId;
                    motorAccident.pfc_accident_event_detail = contentInput.eventDetailInfo.accidentNatureDesc;
                    //motorAccident.pfc_more_document; // ไม่มี moreDocument
                    motorAccident.pfc_accident_latitude = contentInput.eventDetailInfo.accidentLatitude;
                    motorAccident.pfc_accident_Longitude = contentInput.eventDetailInfo.accidentLongitude;
                    motorAccident.pfc_accident_location = contentInput.eventDetailInfo.accidentPlace;
                    motorAccident.pfc_accident_remark = contentInput.eventDetailInfo.accidentRemark;
                    motorAccident.pfc_ref_isurvey_total_event = contentInput.eventDetailInfo.totalEvent;
                    motorAccident.pfc_ref_isurvey_created_date = convertDateTime(contentInput.eventDetailInfo.iSurveyCreatedDate);
                    motorAccident.pfc_ref_isurvey_modified_date = convertDateTime(contentInput.eventDetailInfo.iSurveyModifiedDate);
                    motorAccident.pfc_ref_isurvey_isdeleted = convertBool(contentInput.eventDetailInfo.iSurveyIsDeleted);
                    motorAccident.pfc_ref_isurvey_isdeleted_date = convertDateTime(contentInput.eventDetailInfo.iSurveyIsDeletedDate);
                    motorAccident.pfc_motor_accident_parties_sum = contentInput.eventDetailInfo.numOfAccidentParty;
                    Guid motorId = _serviceProxy.Create(motorAccident);
                    // getting GUID of motorAccident and store in "_motorId" variable
                    _motorId = motorId;
                    */
                }
                catch (Exception e)
                {
                    output.code = "501";
                    output.message = "False: Update Entity Motor Accident PROBLEM";
                    output.description = e.ToString();
                    output.transactionId = TransactionId;
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = new AccidentPrilimSurveyorReportDataOutputModel_Pass();
                    output.data.message = e.StackTrace;

                    return output;
                }

                // Motor Accident Parties  
                try
                {
                    List<PartiesInfoModel> party = contentInput.partiesInfo;

                    foreach (PartiesInfoModel a in party)
                    {
                        pfc_motor_accident_parties motorAccidentParties = new pfc_motor_accident_parties();
                        motorAccidentParties.pfc_parent_motor_accidentId = new Microsoft.Xrm.Sdk.EntityReference(pfc_motor_accident_parties.EntityLogicalName, _motorId);

                        motorAccidentParties.pfc_motor_accident_parties_name = a.partiesFullname;
                        motorAccidentParties.pfc_ref_isurvey_partiesid = a.partiesId;
                        motorAccidentParties.pfc_ref_isurvey_eventid = a.partiesEventId;
                        motorAccidentParties.pfc_ref_isurvey_eventitem = a.partiesEventItem;
                        motorAccidentParties.pfc_event_code = contentInput.eventId;
                        motorAccidentParties.pfc_event_sequence = 1;
                        if (motorAccidentParties.pfc_parties_sequence < 1 || motorAccidentParties.pfc_parties_sequence == null)
                        {
                            motorAccidentParties.pfc_parties_sequence = 1;
                        }
                        else
                        {
                            motorAccidentParties.pfc_parties_sequence += 1;
                        }
                        motorAccidentParties.pfc_parties_fullname = a.partiesFullname;
                        motorAccidentParties.pfc_parties_type = new Microsoft.Xrm.Sdk.OptionSetValue(Int32.Parse(a.partiesType.ToString())); // ไม่มี field นี้
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
                            List<ClaimDetailPartiesInfoModel> partiesPart = contentInput.claimDetailPartiesInfo;

                            foreach (ClaimDetailPartiesInfoModel data in partiesPart)
                            {
                                if (data.claimDetailPartiesPartiesId == partyId)
                                {
                                    pfc_motor_accident_parties_parts motorAccidentPartiesPart = new pfc_motor_accident_parties_parts();
                                    motorAccidentPartiesPart.pfc_motor_accident_partiesId = new Microsoft.Xrm.Sdk.EntityReference(pfc_motor_accident_parties_parts.EntityLogicalName, partiesID);

                                    motorAccidentPartiesPart.pfc_motor_accident_parties_parts_name = data.claimDetailPartiesDetail; // motorAccidentPartiesPart.pfc_detail;
                                    motorAccidentPartiesPart.pfc_ref_isurvey_partiesid = data.claimDetailPartiesPartiesId;
                                    motorAccidentPartiesPart.pfc_ref_isurvey_item = data.claimDetailPartiesItem;
                                    motorAccidentPartiesPart.pfc_ref_isurvey_detailid = data.claimDetailPartiesDetailId;
                                    motorAccidentPartiesPart.pfc_detail = data.claimDetailPartiesDetail;
                                    motorAccidentPartiesPart.pfc_damage_levels = new Microsoft.Xrm.Sdk.OptionSetValue(Int32.Parse(convertClaimDetailLevels(data.claimDetailPartiesLevels)));
                                    motorAccidentPartiesPart.pfc_is_repair = convertBool(data.claimDetailPartieslIsRepair);
                                    motorAccidentPartiesPart.pfc_remark = data.claimDetailPartiesRemark;
                                    motorAccidentPartiesPart.pfc_ref_isurvey_created_date = convertDateTime(data.claimDetailPartiesCreatedDate);
                                    motorAccidentPartiesPart.pfc_ref_isurvey_modified_date = convertDateTime(data.claimDetailPartiesModifiedDate);
                                    motorAccidentPartiesPart.pfc_ref_isurvey_isdeleted = convertBool(data.claimDetailPartiesIsDeleted);
                                    motorAccidentPartiesPart.pfc_ref_isurvey_isdeleted_date = convertDateTime(data.claimDetailPartiesIsDeletedDate);
                                    motorAccidentPartiesPart.pfc_motor_accident_event_code = contentInput.eventId;
                                    motorAccidentPartiesPart.pfc_motor_accident_event_sequence = 1;
                                    if (motorAccidentPartiesPart.pfc_motor_accident_parties_sequence < 1 || motorAccidentPartiesPart.pfc_motor_accident_parties_sequence == null)
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
                            output.message = "False: Create Entity (Case/Motor) PROBLEM";
                            output.description = e.ToString();
                            output.transactionId = TransactionId;
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
                    output.message = "False: Create Entity Motor Accident Parties PROBLEM";
                    output.description = e.ToString();
                    output.transactionId = TransactionId;
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = new AccidentPrilimSurveyorReportDataOutputModel_Pass();
                    output.data.message = e.StackTrace;

                    return output;
                }

                // Motor Accident Part (ผูกกับ parties)
                try
                {
                    List<ClaimDetailInfoModel> parts = contentInput.claimDetailInfo;

                    foreach (ClaimDetailInfoModel a in parts)
                    {
                        pfc_motor_accident_parts motorAccidentPart = new pfc_motor_accident_parts();

                        motorAccidentPart.pfc_motor_accidentId = new Microsoft.Xrm.Sdk.EntityReference(pfc_motor_accident_parts.EntityLogicalName, _motorId);
                        motorAccidentPart.pfc_motor_accident_parts_name = a.claimDetailDetail; // motorAccidentPart.pfc_detail;

                        motorAccidentPart.pfc_ref_isurvey_eventid = a.claimDetailEventId;
                        motorAccidentPart.pfc_ref_isurvey_item = a.claimDetailItem;
                        motorAccidentPart.pfc_ref_isurvey_detailid = a.claimDetailDetailid;
                        motorAccidentPart.pfc_detail = a.claimDetailDetail;
                        motorAccidentPart.pfc_damage_levels = new Microsoft.Xrm.Sdk.OptionSetValue(Int32.Parse(convertClaimDetailLevels(a.claimDetailLevels)));
                        motorAccidentPart.pfc_is_repair = convertBool(a.claimDetailIsRepair);
                        motorAccidentPart.pfc_remark = a.claimDetailRemark;
                        motorAccidentPart.pfc_ref_isurvey_created_date = convertDateTime(a.claimDetailCreatedDate);
                        motorAccidentPart.pfc_ref_isurvey_modified_date = convertDateTime(a.claimDetailModifiedDate);
                        motorAccidentPart.pfc_ref_isurvey_isdeleted = convertBool(a.claimDetailIsDeleted);
                        motorAccidentPart.pfc_ref_isurvey_isdeleted_date = convertDateTime(a.claimDetailIsDeletedDate);
                        motorAccidentPart.pfc_motor_accident_event_code = contentInput.eventId;
                        motorAccidentPart.pfc_motor_accident_event_sequence = 1;

                        _serviceProxy.Create(motorAccidentPart);

                    }

                }
                catch (Exception e)
                {
                    output.code = "501";
                    output.message = "False: Create Entity Motor Accident Part PROBLEM";
                    output.description = e.ToString();
                    output.transactionId = TransactionId;
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = new AccidentPrilimSurveyorReportDataOutputModel_Pass();
                    output.data.message = e.StackTrace;

                    return output;
                }


                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "AccidentPrilimSurveyorReportOutput is done!";
                output.transactionId = TransactionId;
                //output.transactionId = Request.Properties["TransactionID"].ToString();
                output.transactionDateTime = DateTime.Now.ToString();
                output.data = new AccidentPrilimSurveyorReportDataOutputModel_Pass();
                output.data.message = "Ticket ID: " + contentInput.ticketNo + ", Claim Noti No: " + contentInput.claimNotiNo;

                return output;
            }

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
            if (value.Equals("Y") || value.Equals("0"))
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