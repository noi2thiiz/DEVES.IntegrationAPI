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
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegClaimRequestFromRVPController : ApiController
    {
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(RegClaimRequestFromRVPController));

        private QueryInfo q = new QueryInfo();
        private System.Data.DataTable dt = new System.Data.DataTable();

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
                outputFail.data.fieldErrors = new List<RegClaimRequestFromRVPFieldErrors>();

                List<string> errorMessage = JsonHelper.getReturnError();
                foreach (var text in errorMessage)
                {
                    string fieldMessage = "";
                    string fieldName = "";
                    if (text.Contains("Required properties"))
                    {
                        int indexEnd = 0;
                        for (int i = 0; i < text.Length - 1; i++)
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

                        for (int i = 0; i < text.Length - 1; i++)
                        {
                            if (text.Substring(i, 1).Equals("."))
                            {
                                fieldMessage = text.Substring(0, i);
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

                    outputFail.data.fieldErrors.Add(new RegClaimRequestFromRVPFieldErrors(fieldName, fieldMessage));
                }

                outputFail.code = "400";
                outputFail.message = "Invalid Input(s)";
                outputFail.description = "Some of your input is invalid. Please recheck again.";
                outputFail.transactionId = "rvpCliamNo: " + contentModel.rvpCliamNo;
                outputFail.transactionDateTime = DateTime.Now.ToString();

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", outputFail.code, Environment.NewLine, outputFail.description);
                return Request.CreateResponse<RegClaimRequestFromRVPOutputModel_Fail>(outputFail);
            }
        }

        private RegClaimRequestFromRVPOutputModel_Pass HandleMessage(string valueText, RegClaimRequestFromRVPInputModel content)
        {
            //TODO: Do what you want
            var output = new RegClaimRequestFromRVPOutputModel_Pass();

            _log.Info("HandleMessage");

            dt = new System.Data.DataTable();

            // Check number of policy that exec in stored 
            try
            {
                dt = q.Queryinfo_InquiryPolicyMotorList(content.policyInfo.policyNo, content.policyInfo.carChassisNo, content.policyInfo.currentCarRegisterNo, content.policyInfo.currentCarRegisterProv);
                
                if(dt.Rows.Count == 0)
                {
                    output.code = "400";
                    output.message = "พบปัญหาเกี่ยวกับการค้นหา Policy";
                    output.description = "ไม่พบ Policy";
                    output.transactionId = "";
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = new RegClaimRequestFromRVPDataOutputModel_Pass();
                    return output;

                }
                else if(dt.Rows.Count > 1)
                {
                    output.code = "400";
                    output.message = "พบปัญหาเกี่ยวกับการค้นหา Policy";
                    output.description = "พบ Policy มากกว่า 1 Policy";
                    output.transactionId = "";
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = null;

                    return output;
                }
            }
            catch (Exception e)
            {
                output.code = "500";
                output.message = "Connect Stored Procedured Error";
                output.description = e.ToString();
                output.transactionId = "";
                output.transactionDateTime = DateTime.Now.ToString();
                output.data = null;

                return output;
            }

            // create case, motor accident, motor accident party
            try
            {
                var RegClaimRequestFromRVPOutput = new RegClaimRequestFromRVPDataOutputModel_Pass();

                var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
                _serviceProxy = connection.OrganizationServiceProxy;
                ServiceContext svcContext = new ServiceContext(_serviceProxy);

                try
                {
                    /*
                    // ยังไงก็ต้องสร้าง Case
                    Incident incidentCase = new Incident();
                    incidentCase.Title = ""; // มาจากการ Concast CaseType+Cate ตาม Business บนหน้าจอ
                    incidentCase.CaseOriginCode = new OptionSetValue(100000002); // fix
                    incidentCase.pfc_case_vip = ""; // เป็นไปตาม Logic. ของหน้า Case ที่ว่า ถ้า Policy หรือ Customer เป็น VIP Case นั้นๆต้องเป็น VIP
                    incidentCase.pfc_policy_additionalId = ""; // เอา params.crmPolicyDetailId มาใช้หรือ หาจาก policyNo+renewalNo+fleetCarNo+barcodeเอา params.crmPolicyDetailId มาใช้หรือ หาจาก policyNo+renewalNo+fleetCarNo+barcode
                    incidentCase.pfc_policy_additional_number = ""; // (PolicyAdditional.pfc_policy_additional_name)
                    incidentCase.pfc_policyId = ""; // (policyAdditional.pfc_policyId)
                    incidentCase.pfc_policy_client_number = ""; // (Policy.pfc_cus_client_number)
                    incidentCase.pfc_policyId = ""; // (policyAdditional.pfc_policyId)
                    incidentCase.pfc_policy_client_number = ""; // (Policy.pfc_cus_client_number)
                    incidentCase.pfc_policy_number = ""; // (Policy.pfc_chdr_num), (PolicyAdditional.pfc_chdr_num)
                    incidentCase.pfc_policy_vip = ""; // (policyAdditional.pfc_policy_vip)
                    incidentCase.pfc_policy_mc_nmc = ""; // (Policy.pfc_policy_mc_nmc)
                    incidentCase.pfc_current_reg_num = content.policyInfo.currentCarRegisterNo;
                    incidentCase.pfc_current_reg_num_prov = content.policyInfo.currentCarRegisterProv;
                    incidentCase.CaseTypeCode = ""; // fix: 2 ( Service Request )
                    incidentCase.pfc_categoryId = ""; // fix: (สินไหม (Motor)), ต้องหา CategoryCode ในการ Mapping, ห้าม Hardcode โดยใช้ GUID เด็ดขาด
                    incidentCase.pfc_sub_categoryId = ""; // fix: (แจ้งอุบัติเหตุรถยนต์ (ผ่าน บ.กลาง)), ต้องหา Sub CategoryCode ในการ Mapping, ห้าม Hardcode โดยใช้ GUID เด็ดขาด
                    incidentCase.pfc_source_data = new OptionSetValue(100000002); // fix
                    incidentCase.CustomerId = // เอา params.insuredCleansingId, insuredClientId + insuredClientType มาใช้หรือ จะเอาจาก pfc_policy.pfc_customerId ก็ได้
                    incidentCase.pfc_customer_vip = ""; // account/contact.pfc_customer_sensitive_level
                    incidentCase.pfc_customer_privilege = "";  // account/contact.pfc_customer_privilege_level
                    incidentCase.pfc_notification_date = Convert.ToDateTime(content.claimInform.accidentOn);
                    incidentCase.pfc_accident_on = Convert.ToDateTime(content.claimInform.accidentOn);
                    incidentCase.pfc_accident_desc_code = content.claimInform.accidentDescCode;
                    incidentCase.pfc_accident_desc = content.claimInform.accidentDesc;
                    incidentCase.pfc_num_of_expect_injuries = new OptionSetValue(Int32.Parse(content.claimInform.numOfExpectInjury.ToString()));
                    incidentCase.pfc_accident_latitude = content.claimInform.accidentLatitude;
                    incidentCase.pfc_accident_longitude = content.claimInform.accidentLongitude;
                    incidentCase.pfc_accident_place = content.claimInform.accidentPlace;
                    incidentCase.pfc_informer_name = ""; // fix (Corperate)(บ.กลาง), ต้องหา Code หรือ Key ในการ Map กับ Corperate
                    incidentCase.pfc_driver_name = ""; // fix (Corperate)(บ.กลาง), ต้องหา Code หรือ Key ในการ Map กับ Corperate
                    incidentCase.pfc_driver_client_number = content.policyDriverInfo.driverClientId;
                    incidentCase.pfc_driver_mobile = content.policyDriverInfo.driverMobile;
                    incidentCase.pfc_relation_cutomer_accident_party = new OptionSetValue(100000000); // fix
                    incidentCase.pfc_high_loss_case_flag = true; // เป็นไปตาม Logic. ใน Case ว่าในกรณีที่มีการใช้รถยก (Incident.pfc_num_of_tow_truck is not null and pfc_num_of_tow_truck != 100000000) ให้ Fix: 1 (เสียหาย)
                    incidentCase.pfc_legal_case_flag = true; // not sure
                    incidentCase.pfc_claim_type = new OptionSetValue(Int32.Parse(content.claimInform.claimType));
                    incidentCase.pfc_send_out_surveyor = new OptionSetValue(Int32.Parse(content.claimInform.sendOutSurveyorCode));
                    incidentCase.pfc_isurvey_status = new OptionSetValue(100000099); // fix
                    incidentCase.pfc_accident_legal_result = new OptionSetValue(Int32.Parse(content.claimInform.accidentLegalResult));
                    incidentCase.pfc_police_station = content.claimInform.policeStation;
                    incidentCase.pfc_police_record_id = content.claimInform.policeRecordId;
                    incidentCase.pfc_police_record_date = Convert.ToDateTime(content.claimInform.policeRecordDate);
                    incidentCase.pfc_police_bail_flag = convertBool(content.claimInform.policeBailFlag);
                    incidentCase.pfc_num_of_accident_injuries = new OptionSetValue(content.claimInform.numOfAccidentInjury);
                    incidentCase.pfc_num_of_death = new OptionSetValue(content.claimInform.numOfDeath);
                    incidentCase.pfc_excess_fee = content.claimInform.excessFee;
                    incidentCase.pfc_deductable_fee = content.claimInform.deductibleFee;
                    incidentCase.reportAccidentResultDate = content.claimInform.reportAccidentResultDate; // หา reportAccidentResultDate ไม่เจอ

                    // Motor Accident   
                    pfc_motor_accident MotorAccident = new pfc_motor_accident();
                    MotorAccident.pfc_motor_accident_name // ให้เป็นไปตาม Logic. ของหน้า Motor Accident
                    MotorAccident.pfc_parent_caseId // ให้ผูกกับ Case ด้านบน
                    MotorAccident.pfc_activity_date // ใช้ params.accidentOn ได้เลย
                    MotorAccident.pfc_event_code // ให้เป็นไปตาม Logic. ของหน้า Motor Accident
                    MotorAccident.pfc_event_sequence = 1;
                    MotorAccident.pfc_accident_event_detail // accidentNatureDesc
                    MotorAccident.pfc_motor_accident_parties_sum = Int32.Parse(content.claimInform.numOfAccidentParty);
                    MotorAccident.pfc_ref_rvp_claim_no = content.rvpCliamNo;
                    MotorAccident.pfc_motor_accident_parties_name // ให้เป็นไปตาม Logic. ของหน้า Motor Accident Parties

                    */
                    if (Int32.Parse(content.claimInform.numOfAccidentParty) == 0)
                    {
                        //incidentCase.pfc_motor_accident_sum = 0; // fix: 1 หาก params.numOfAccidentParty มีมากกว่า 0
                        
                    }

                    else
                    {
                        // Create 3 un 
                        //incidentCase.pfc_motor_accident_sum = 1; // fix: 1 หาก params.numOfAccidentParty มีมากกว่า 0

                        // Motor Accident Parties <List>
                        pfc_motor_accident_parties MotorAccidentParties = new pfc_motor_accident_parties();

                        for (int i = 0; i < Int32.Parse(content.claimInform.numOfAccidentParty); i++)
                        {
                            // value in "pfc_motor_accident_parties"
                            /*
                            MotorAccidentParties.pfc_motor_accident_parties_name
                                MotorAccidentParties.pfc_event_code
                                MotorAccidentParties.pfc_event_sequence
                                MotorAccidentParties.pfc_parties_sequence
                                MotorAccidentParties.pfc_parent_motor_accidentId
                                MotorAccidentParties.pfc_parties_fullname
                                MotorAccidentParties.pfc_parties_type
                                MotorAccidentParties.pfc_licence_province
                                MotorAccidentParties.pfc_licence_no
                                MotorAccidentParties.pfc_phoneno
                                MotorAccidentParties.pfc_insurance_name
                                MotorAccidentParties.pfc_policyno
                                MotorAccidentParties.pfc_policy_type                                
                                MotorAccidentParties.pfc_ref_rvp_claim_no
                                MotorAccidentParties.pfc_ref_rvp_parties_seq
                                */

                    // value in array json input
                    //content.accidentPartyInfo[i].rvpAccidentPartySeq
                    //content.accidentPartyInfo[i].accidentPartyFullname
                    //content.accidentPartyInfo[i].accidentPartyPhone
                    //content.accidentPartyInfo[i].accidentPartyCarPlateNo
                    //content.accidentPartyInfo[i].accidentPartyCarPlateProv
                    //content.accidentPartyInfo[i].accidentPartyInsuranceCompany
                    //content.accidentPartyInfo[i].accidentPartyPolicyNumber
                    //content.accidentPartyInfo[i].accidentPartyPolicyType
                    //MotorAccidentParties

                    
                        }
                        
                        //_serviceProxy.Create(MotorAccidentParties);
                        
                    }
                    //_serviceProxy.Create(incidentCase);
                    //_serviceProxy.Create(MotorAccident);

                    // 2. execute stored

                    // 3. update vaule to crm form


                }
                catch (Exception e)
                {
                    output.code = "501";
                    output.message = "False";
                    output.description = "Create/Retrieving Motor data PROBLEM";
                    output.transactionId = "";
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = null;

                    return output;
                }

                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "ReqClaimRequestFromRVP is done!";
                //output.transactionId = content.ticketNo;
                output.transactionId = "{0} ticketNo need to be added from stored";
                output.transactionDateTime = DateTime.Now.ToString();
                //RegClaimRequestFromRVPOutput.ticketNo = "ticketNo: " + content.ticketNo;
                RegClaimRequestFromRVPOutput.ticketNo = "{1} ticketNo need to be added from stored";
                RegClaimRequestFromRVPOutput.claimNotiNo = "{1} claimNotiNo need to be added from stored";
                output.data = RegClaimRequestFromRVPOutput;
            }
            catch (System.ServiceModel.FaultException e)
            {
                output.code = "500";
                output.message = "False";
                output.description = "CRM PROBLEM";
                output.transactionId = "";
                output.transactionDateTime = DateTime.Now.ToString();
                output.data = null;

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

                output.code = "400";
                output.message = "False";
                output.description = "ไม่พบ rvpCliamNo";
                output.transactionId = "rvpCliamNo: null";
                output.transactionDateTime = DateTime.Now.ToString();
                output.data = null;
            }

            return output;
        }

        private bool convertBool(string value)
        {
            if (value.Equals("Y"))
            {
                return true;
            }
            else
            {
                return false;
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

    }
}