using System;
using DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP;
using DEVES.IntegrationAPI.WebApi.DataAccessService;
using Microsoft.Xrm.Sdk;
using DEVES.IntegrationAPI.WebApi.DataAccessService.XrmEntity;
using System.Collections.Generic;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.Core.TechnicalService.Models;
using System.Runtime.Serialization;
using DEVES.IntegrationAPI.Core.TechnicalService.Exceptions;
using DEVES.IntegrationAPI.Core.ExtensionMethods;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataRepository;

namespace DEVES.IntegrationAPI.WebApi.Logic.RVP
{


    public class BuzRegClaimRequestFromRVPCommand
    {
        // forBusiness Requirements  Verification Process
        public static readonly Dictionary<String, ErrorMessage> RVPResponseInfo = new Dictionary<String, ErrorMessage>()
        {
            { "RvpRefCode_Duplicate",new ErrorMessage{  httpStatus="500",code ="ERROR-RVP010",
                                                fieldError ="rvpRefCode",
                                                message ="Duplicate rvpRefCode,The request have been rejected",
                                                description ="A rvpRefCode from this request already exists in CRM"} },

            { "Policy_NotFound",new ErrorMessage{  httpStatus="500",code ="ERROR-RVP021",
                                                fieldError ="policyInfo.policyNo",
                                                message ="Policy not found,The request have been rejected",
                                                description ="The policy number not found"} },

            { "Policy_MultipleFound",new ErrorMessage{  httpStatus="500", code="ERROR-RVP022",
                                                fieldError ="policyInfo.policyNo",
                                                message ="Duplicate policy,The request have been rejected",
                                                description ="The system not allow multiple policy number searches on multiple customer."} },

            {"InsuredClient_NotFound",new ErrorMessage{  httpStatus="500", code="ERROR-RVP031",
                                                fieldError ="policyInfo.insuredClientType,policyInfo.insuredClientId",
                                                message ="The request have been rejected",
                                                description ="The request found  multiple Insured Client"} },
            {"InsuredClient_MultipleFound",new ErrorMessage{  httpStatus="500", code="ERROR-RVP031",
                                                fieldError ="policyInfo.insuredClientType,policyInfo.insuredClientId",
                                                message ="The request have been rejected",
                                                description ="The request found  multiple Insured Client"} },

            {"DriverClient_NotFound",new ErrorMessage{  httpStatus="500", code="ERROR-RVP041",
                                                fieldError ="policyDriverInfo.driverClientId",
                                                message ="The request have been rejected",
                                                description ="The reques found multiple driver."} },

            { "DriverClient_MultipleFound",new ErrorMessage{  httpStatus="500", code="ERROR-RVP041",
                                                fieldError ="policyDriverInfo.driverClientId",
                                                message ="The request have been rejected",
                                                description ="The reques found multiple driver."} },

            { "CanNotCreateIncident",new ErrorMessage{  httpStatus="500", code="ERROR-RVP041",
                                                fieldError ="policyDriverInfo.driverClientId",
                                                message ="The request have been rejected",
                                                description ="Cannot Create IncidentEntity"} },

            { "CannotCreateMotorAccident",new ErrorMessage{  httpStatus="500", code="ERROR-RVP041",
                                                fieldError ="policyDriverInfo.driverClientId",
                                                message ="The request have been rejected",
                                                description ="Cannot Create IncidentEntity"} },


        };


        private PfcIncidentDataGateWay IncidentDataGateWay { get; } = new PfcIncidentDataGateWay();
        private CRMPolicyMotorDataGateWay policyDataGateway = new CRMPolicyMotorDataGateWay();
        private SpReqMotorClaimNotiNoDataGateway reqMotorClaimNotiNoGateway = new SpReqMotorClaimNotiNoDataGateway();


        public CrmregClaimRequestFromRVPContentOutputModel Execute(object input)
        {
            var apiInput = (CRMregClaimRequestFromRVPInputModel)input;
            CrmregClaimRequestFromRVPContentOutputModel outputContent = new CrmregClaimRequestFromRVPContentOutputModel()
            {
                data = new CrmRegClaimRequestFromRVPDataOutputModel()
                {
                    claimNotiNo = "",
                    ticketNo = "",
                    incidentGuid = ""
                }
            };

            #region validation
            // condition #1 

            // Search Policy Additional (sp_API_InquiryPolicyMotorList)
            // ตรวจสอบ  Policy และ PolicyAditional ถ้ามีมากกว่า 1 รายการ ให้ return error
            var resultPolicy = RVPPolicyAditionalFinder.Find(apiInput.policyInfo.policyNo,
                apiInput.policyInfo.carChassisNo,
                apiInput.policyInfo.currentCarRegisterNo,
                apiInput.policyInfo.currentCarRegisterProv
                );

            if (null == resultPolicy)
            {
                throw new BuzInValidBusinessConditionException(RVPResponseInfo["Policy_NotFound"]);
            }


            // condition #2

            // Search insureClientid (sp_API_CustomerClient)
            // หาว่ามี client แล้วหรือยัง ถ้ามีมากกว่า 1 รายการให้ return error ถ้าไม่เจอให้สร้างใหม่
            var insuredClientType = apiInput.policyInfo.insuredClientType; // C or P
            var insuredClientId = apiInput.policyInfo.insuredClientId; // id
            var resultClient = RVPClientFinder.Find(insuredClientType, insuredClientId);
            if (null == resultClient)
            {
                throw new BuzInValidBusinessConditionException(RVPResponseInfo["InsuredClient_NotFound"]);
                //throw new Exception("The above query returns multiple Insured Client.");
            }


            // condition #3

            // Search DriverClientid (sp_API_CustomerClient)
            // หาว่ามี client แล้วหรือยัง ถ้าไม่ใช่ 1 รายการให้ return

            var resultDriverClient = RVPClientFinder.Find("P", apiInput.policyDriverInfo.driverClientId);

            if (null == resultDriverClient)
            {

                throw new BuzInValidBusinessConditionException(RVPResponseInfo["DriverClient_NotFound"]);
            }


            //condition #4
            // Search บ.กลาง (informByCrmId)
            // หาว่ามี client แล้วหรือยัง ถ้ามีมากกว่า 1 รายการให้ return error ถ้าไม่เจอให้สร้างใหม่

            //var resultrvpClient = FindClient("clientType",apiInput.policyInfo.inf);

            //if (null == resultrvpClient)
            //{
            //    throw new Exception("The above query returns multiple  informer.");
            // }

            // var informerEntity = FindClient("clientType", "clientId");

            #endregion

            #region precess
            //Valid

            // step #7
            // Insert Incident
            // Get incident.incidentId
            var incidentGuid = RVPCreateIncidentCommand.Execute(apiInput, resultPolicy, resultClient, resultClient, resultDriverClient);

            // step #8
            // Insert pfc_motor_accident (pfc_parent_caseId = incident.incidentId) 
            // Get pfc_motor_accident.pfc_motor_accidentId

            if (null == incidentGuid)
            {
                throw new BuzInternalErrorException(RVPResponseInfo["CannotCreateIncident"]);

            }

            var IncidentEntity = IncidentDataGateWay.Find(incidentGuid);
            var IncidentMortorGuid = RVPCreateMotorAccidentCommand.Execute(apiInput, incidentGuid);


            if (null == IncidentMortorGuid)
            {
                throw new BuzInternalErrorException(RVPResponseInfo["CannotCreateMotorAccident"]);

            }
            var mortorAccidentEntity = RVPMortorAccidentFinder.Find(IncidentMortorGuid);
            // step #9
            // Loop Insert pfc_motor_accident_parties (pfc_parent_motor_accidentId = pfc_motor_accident.pfc_motor_accidentId)
            CreateMortorAccidentPartiesCommand.Execute(apiInput, IncidentEntity, mortorAccidentEntity, IncidentMortorGuid);
            // Run Store sp_ReqMotorClaimNotiNo เพื่อ Generate เลข Claim Noti Number
            //Console.WriteLine("IncidentMortorGuid = " + IncidentMortorGuid.ToString());

            reqMotorClaimNotiNoGateway.Excecute(incidentGuid.ToString(), "ADMIN TEST");


            // Return Output

            var IncidentEntity2 = IncidentDataGateWay.Find(incidentGuid);
            outputContent.data.claimNotiNo = IncidentEntity2.pfc_claim_noti_number;
            outputContent.data.ticketNo = IncidentEntity2.ticketnumber;
            outputContent.data.incidentGuid = incidentGuid.ToString();

            #endregion;

            return outputContent;
        }

        public Guid CreateIncidentMortor(CRMregClaimRequestFromRVPInputModel apiInput, Guid incidentguid)
        {
            return RVPCreateMotorAccidentCommand.Execute(apiInput, incidentguid);
        }

        public Guid CreateIncident(CRMregClaimRequestFromRVPInputModel apiInput, CRMPolicyMotorEntity policyEntity, CustomerClientEntity customerClientEntity, CustomerClientEntity informerEntity, CustomerClientEntity driverEntity)
        {
            return RVPCreateIncidentCommand.Execute(apiInput, policyEntity, customerClientEntity, informerEntity, driverEntity);
        }
        public Dictionary<string, dynamic> GetInformerInfo()
        {
            return RVPInformerFinder.Find();
        }
    }



    internal class MockRVPData
    {
        public static CRMPolicyMotorEntity GetMockPolicy()
        {
            return new CRMPolicyMotorEntity
            {
                crmPolicyDetailId = new Guid("3BFCB0A4-DCB6-E611-80CA-0050568D1874"),
                crmPolicyDetailCode = "00/2015-C7121677(0)-1",
                policyAdditionalName = "00/2015-C7121677-1",
                policyNo = "C7121677",
                renewalNo = "0",
                fleetCarNo = "1",
                barcode = "2010034076831",
                insureCardNo = null,
                MCM_SEQ = null,
                policySeqNo = "1",
                endorseNo = null,
                branchCode = "00",
                contractType = "MCP",
                policyProductTypeCode = "TP",
                policyProductTypeName = "พรบ.",
                policyIssueDate = "2015-10-13T00:00:00",
                policyEffectiveDate = "2015-10-18T00:00:00",
                policyExpiryDate = "2016-10-18T00:00:00",
                policyGarageFlag = "N",
                policyPaymentStatus = "N",
                policyCarRegisterNo = "ตม4533",
                policyCarRegisterProv = "กท",
                carChassisNo = "MR0GR19G407004507",
                carVehicleType = "1.40A",
                carVehicleModel = "TOYOTA",
                carVehicleYear = "2015",
                carVehicleBody = "PICK",
                carVehicleSize = "3/2.50",
                policyDeduct = 0,
                agentCode = "90002583",
                agentName = "สำนักงานนายหน้าประกันวินาศภัย ดวงเจริญ",
                agentBranch = null,
                insuredCleansingId = null,
                insuredClientId = "16575442",
                insuredClientType = "P",
                insuredFullName = "สมคิด ดวนสันเทียะ",
                policyStatus = "In Force",
                policyVip = false,
                policyId = new Guid("0ea8fb62-6db7-e611-80ca-0050568d1874"),
                policyMcNmc = 100000000

            };
        }

        public static CustomerClientEntity GetMockCustomerClient()
        {
            return new CustomerClientEntity
            {
                ContactId = new Guid("b55765f1-c4a4-e611-80ca-0050568d1874"),
                DefaultPriceLevelId = null,
                CustomerSizeCode = 1,
                CustomerTypeCode = 1,
                PreferredContactMethodCode = 4,
                LeadSourceCode = 1,
                OriginatingLeadId = null,
                OwningBusinessUnit = new Guid("506e354e-b8a1-e611-80c7-0050568d1874"),
                PaymentTermsCode = null,
                ShippingMethodCode = 1,
                ParticipatesInWorkflow = false,
                IsBackofficeCustomer = false,
                Salutation = "ว่าที่ร้อยตรีหญิง",
                JobTitle = null,
                FirstName = "ออมศิริณร์ ",
                Department = null,
                NickName = null,
                MiddleName = null,
                LastName = "รักษ์วงศ์",
                Suffix = null,
                YomiFirstName = null,
                FullName = "ออมศิริณร์ รักษ์วงศ์",
                YomiMiddleName = null,
                YomiLastName = null,
                Anniversary = null,
                BirthDate = null,
                GovernmentId = null,
                YomiFullName = "ออมศิริณร์ รักษ์วงศ์",
                Description = "เป็นลูกค้า VIP ชั้นดี ",
                EmployeeId = null,
                GenderCode = 2,
                AnnualIncome = null,
                HasChildrenCode = 1,
                EducationCode = 1,
                WebSiteUrl = null,
                FamilyStatusCode = 1,
                FtpSiteUrl = null,
                EMailAddress1 = "aomsirinrak@gmail.com",
                SpousesName = null,
                AssistantName = null,
                EMailAddress2 = null,
                AssistantPhone = null,
                EMailAddress3 = null,
                DoNotPhone = false,
                ManagerName = null,
                ManagerPhone = null,
                DoNotFax = true,
                DoNotEMail = true,
                DoNotPostalMail = true,
                DoNotBulkEMail = false,
                DoNotBulkPostalMail = false,
                AccountRoleCode = null,
                TerritoryCode = 1,
                IsPrivate = false,
                CreditLimit = null,
                CreatedOn = "2016-11-07T08=34=06",
                CreditOnHold = false,
                CreatedBy = new Guid("f38f354e-b8a1-e611-80c7-0050568d1874"),
                ModifiedOn = "2017-04-05T12=10=00",
                ModifiedBy = new Guid("71ffbed1-e819-e711-80d4-0050568d1874"),
                NumberOfChildren = null,
                ChildrensNames = null,
                VersionNumber = "AAAAAAmPjV8=",
                MobilePhone = "0872977030",
                Pager = null,
                Telephone1 = "022586992",
                Telephone2 = null,
                Telephone3 = null,
                Fax = null,
                Aging30 = null,
                StateCode = 0,
                Aging60 = null,
                StatusCode = 1,
                Aging90 = null,
                PreferredSystemUserId = null,
                PreferredServiceId = null,
                MasterId = null,
                PreferredAppointmentDayCode = null,
                PreferredAppointmentTimeCode = 1,
                DoNotSendMM = false,
                Merged = false,
                ExternalUserIdentifier = null,
                SubscriptionId = null,
                PreferredEquipmentId = null,
                LastUsedInCampaign = null,
                TransactionCurrencyId = new Guid("f7c3d6ad-b8a1-e611-80c7-0050568d1874"),
                OverriddenCreatedOn = null,
                ExchangeRate = 1,
                ImportSequenceNumber = null,
                TimeZoneRuleVersionNumber = 0,
                UTCConversionTimeZoneCode = null,
                AnnualIncome_Base = null,
                CreditLimit_Base = null,
                Aging60_Base = null,
                Aging90_Base = null,
                Aging30_Base = null,
                OwnerId = new Guid("f38f354e-b8a1-e611-80c7-0050568d1874"),
                CreatedOnBehalfBy = null,
                IsAutoCreate = false,
                ModifiedOnBehalfBy = null,
                ParentCustomerId = null,
                ParentCustomerIdType = null,
                ParentCustomerIdName = null,
                OwnerIdType = 8,
                ParentCustomerIdYomiName = null,
                ProcessId = null,
                EntityImageId = new Guid("da4605d2-fbb6-e611-80ca-0050568d1874"),
                StageId = null,
                Business2 = null,
                Company = null,
                TraversedPath = null,
                Home2 = null,
                Callback = null,
                CreatedByExternalParty = null,
                ModifiedByExternalParty = null,
                LastOnHoldTime = null,
                SLAId = null,
                OnHoldTime = null,
                SLAInvokedId = null,
                pfc_MobilePhone1 = null,
                pfc_MobilePhone2 = null,
                pfc_HomePhone = "074614298",
                pfc_crm_person_id = "201611-0000003",
                pfc_polisy_client_id = "CRM5555",
                pfc_polisy_secuity_num = null,
                pfc_citizen_id = "1930100031030",
                pfc_passport_id = "A00310300",
                pfc_customer_vip = true,

                pfc_crm_key_code1 = null,
                pfc_crm_key_code2 = null,

                pfc_client_legal_status = 100000005,
                pfc_AMLO_flag = 100000011,
                pfc_language = 100000003,
                pfc_source_data = 100000000,
                pfc_date_of_death = null,
                pfc_customer_sensitive_level = 100000002,
                pfc_customer_privilege_level = 100000004

            };
        }

        public static CRMregClaimRequestFromRVPInputModel GetMockApiInput()
        {
            var apiInput = new CRMregClaimRequestFromRVPInputModel()
            {
                claimInform = new ClaimInformModel(),
                policyInfo = new PolicyInfoModel(),
                accidentPartyInfo = new List<AccidentPartyInfoModel>()
            };
            return apiInput;
        }
    }
    internal static class RVPInformerFinder
    {
        private static Dictionary<string, dynamic> InformerInfo;
        public static Dictionary<string, dynamic> Find()
        {


            if (null == InformerInfo)
            {
                InformerInfo = new Dictionary<string, dynamic>();


                if (MemoryManager.Memory.ContainsKey("RVP_INFOMER"))
                {
                    InformerInfo = (Dictionary<string, dynamic>)MemoryManager.Memory.GetItem("RVP_INFOMER");
                }
                else
                {
                    SpGetInformerForRVPDataGateway dg = new SpGetInformerForRVPDataGateway();
                    var result = dg.Excecute();
                    if (result.Count > 0)
                    {
                        InformerInfo = (Dictionary<string, dynamic>)result.Data[0];
                    }
                    else
                    {
                        throw new Exception("Informer Not Found!!");
                    }

                }

            }
            return InformerInfo;
        }
    }
    public static class RVPClientFinder
    {
        public static void Clear()
        {
            throw new NotImplementedException();
        }

        public static CustomerClientEntity Find(string clientType, string clientId)
        {

            ClientMasterDataGateway clientMasterDataGateway = new ClientMasterDataGateway();
            var reqClient = new DataRequest();
            reqClient.AddParam("clientType", clientType); // Policy Owner Client Type , 
            reqClient.AddParam("clientId", clientId); // "CRM5555"
            var result = clientMasterDataGateway.FetchAll(reqClient);

            if (result.Count != 1)
            {
                return null;
            }
            else
            {
                return ((Dictionary<string, dynamic>)result.Data[0]).ToType<CustomerClientEntity>();
                // return  (CustomerClientEntity)result.Data[0];
                //return MockRVPData.GetMockCustomerClient();
            }


        }
    }
    internal static class RVPOwnerFinder
    {
        private static Dictionary<string, dynamic> Owner;
        public static Dictionary<string, dynamic> Find()
        {

            if (null == Owner)
            {
                Owner = new Dictionary<string, dynamic>();


                if (MemoryManager.Memory.ContainsKey("RVP_OWNER"))
                {
                    Owner = (Dictionary<string, dynamic>)MemoryManager.Memory.GetItem("RVP_OWNER");
                }
                else
                {
                    SpGetUserForRVPDataGateway dg = new SpGetUserForRVPDataGateway();
                    var result = dg.Excecute();
                    if (result.Count > 0)
                    {
                        Owner = (Dictionary<string, dynamic>)result.Data[0];
                    }
                    else
                    {
                        throw new Exception("Informer Not Found!!");
                    }

                }

            }
            return Owner;
        }
    }
    public static class RVPPolicyAditionalFinder
    {
        public static CRMPolicyMotorEntity Find(string policyNo, string chassisNo, string carRegisNo, string carRegisProve)
        {


            CRMPolicyMotorDataGateWay policyDataGateway = new CRMPolicyMotorDataGateWay();
            var reqPolicy = new DataRequest();
            reqPolicy.AddParam("policyNo", policyNo);//"2010034076831"
            reqPolicy.AddParam("chassisNo", chassisNo);
            reqPolicy.AddParam("carRegisNo", carRegisNo);// "ตม4533"
            reqPolicy.AddParam("carRegisProve", carRegisProve);// ""
            var result = policyDataGateway.FetchAll(reqPolicy);
            if (result.Count != 1)
            {
                return null;
            }
            else
            {
                // Dictionary < string, dynamic >
                return ((Dictionary<string, dynamic>)result.Data[0]).ToType<CRMPolicyMotorEntity>();
                // return MockRVPData.GetMockPolicy();
            }


        }
    }
    internal static class RVPCreateIncidentCommand
    {
        public static Guid Execute(CRMregClaimRequestFromRVPInputModel apiInput, CRMPolicyMotorEntity policyAdditionalEntity, CustomerClientEntity customerClientEntity, CustomerClientEntity informerEntity, CustomerClientEntity driverEntity)
        {
            if (null == apiInput.claimInform)
            {
                apiInput.claimInform = new ClaimInformModel();
            }

            var catName = "สินไหม (Motor)";
            var subCatName = "แจ้งอุบัติเหตุรถยนต์(ผ่าน บ.กลาง)";

            //var informerInfo = RVPInformerFinder.Find();
            var informerGuid = new Guid("b55765f1-c4a4-e611-80ca-0050568d1874");// informerInfo["AccountId"];

            var driverGuid = driverEntity.ContactId;
            var policyAdditionalIdGuid = policyAdditionalEntity.crmPolicyDetailId;// new Guid();



            var policyGuid = policyAdditionalEntity.policyId; //new Guid();

            var customeGuid = customerClientEntity.ContactId;

            //หา ชื่อจังหวัด และ อำเภอ
            // apiInput.claimInform.accidentProvn = "10";
            //apiInput.claimInform.accidentDist = "1002";
            if (apiInput.claimInform.accidentProvn == ""){ apiInput.claimInform.accidentProvn = "00";}
            if (apiInput.claimInform.accidentDist == "") { apiInput.claimInform.accidentProvn = "0000"; }
         
            var pfcAccidentProvince = ProvinceRepository.Instance.Find(apiInput.claimInform.accidentProvn).ProvinceName;
            var pfcAccidentDistrict = DistrictRepository.Instance.Find(apiInput.claimInform.accidentDist).DistrictName;

            var incidentEntity = new IncidentEntity(policyAdditionalIdGuid, customeGuid, informerGuid, driverGuid, policyGuid)
            {
                //spec 1: มาจากการ Concast CaseType+Case ตาม Business บนหน้าจอ "มาจากการ Concast CaseType+Cate ตาม Business บนหน้าจอ"
                title = $"{catName} {subCatName} คุณ {apiInput.policyInfo.insuredFullName}",

                //Fix
                caseorigincode = new OptionSetValue(100000002),
               // pfc_ref_isurvey_total_event = "1",
                pfc_policy_number = policyAdditionalEntity.policyNo,
                pfc_policy_vip = (policyAdditionalEntity.policyVip == true), // Default 0 ;
                casetypecode = new OptionSetValue(2), //spec : fix: 2 ( Service Request )
                pfc_source_data = new OptionSetValue(100000002),//spec: fix 100000002
                pfc_relation_cutomer_accident_party = new OptionSetValue(100000000),   //fix: 100000000 (ไม่ระบุ)
                pfc_isurvey_status = new OptionSetValue(100000099), //fix: 100000099 (ส่งเปิดเคลม)

                //fix dinamic
                //fix (Corperate)(บ.กลาง) ต้องหา Code หรือ Key ในการ Map กับ Corperate
                //incidentEntity.pfc_informer_name = "Corperate";

                //Map
                pfc_policy_additional_number = policyAdditionalEntity.policyAdditionalName,
                pfc_notification_date = apiInput.claimInform.informerOn,
                pfc_accident_on = apiInput.claimInform.accidentOn,
                pfc_accident_desc_code = apiInput.claimInform.accidentDescCode,
                pfc_accident_latitude = apiInput.claimInform.accidentLatitude,
                pfc_accident_longitude = apiInput.claimInform.accidentLongitude,
                pfc_accident_place = apiInput.claimInform.accidentPlace,
                pfc_driver_client_number = apiInput.policyDriverInfo.driverClientId,
                pfc_driver_mobile = apiInput.policyDriverInfo.driverMobile,
              
                pfc_police_station = apiInput.claimInform.policeStation,
                pfc_police_record_id = apiInput.claimInform.policeRecordId,
                pfc_police_record_date = apiInput.claimInform.policeRecordDate, 
                pfc_police_bail_flag = (apiInput.claimInform.policeBailFlag=="Y"),
                pfc_excess_fee = apiInput.claimInform.excessFee,
                pfc_deductable_fee = apiInput.claimInform.deductibleFee,

                pfc_motor_accident_sum = 1,

                //Map with Condition

                // เป็นไปตาม Logic. ของหน้า Case ที่ว่า ถ้า Policy หรือ Customer เป็น VIP Case นั้นๆต้องเป็น VIP
                // if (pfc_policy_additional.pfc_policy_vip = "1" or contact.pfc_customer_vip = "1") then "1" Else "0"
                pfc_case_vip = (policyAdditionalEntity.policyVip == true || customerClientEntity.pfc_customer_vip == true),


                //spec:account/contact.pfc_customer_vip
                pfc_customer_vip = (customerClientEntity.pfc_customer_vip == true),

                pfc_policy_mc_nmc = (!string.IsNullOrEmpty(policyAdditionalEntity.policyMcNmc.ToString())) ? new OptionSetValue(policyAdditionalEntity.policyMcNmc) : new OptionSetValue(),


                pfc_policyid = new EntityReference("pfc_policy", policyAdditionalEntity.policyId),


                

                /*spec : (policyAdditional.pfc_policy_vip) (Policy.pfc_policy_mc_nmc) */
                pfc_accident_province = pfcAccidentProvince,
                pfc_accident_district = pfcAccidentDistrict
            };


            //spec : (policyAdditional.pfc_reg_num) = api.currentCarRegisterNo
            incidentEntity.pfc_current_reg_num = apiInput.policyInfo.currentCarRegisterNo;
            //spec : (policyAdditional.pfc_reg_num_prov) = api.currentCarRegisterProv
            incidentEntity.pfc_current_reg_num_prov = apiInput.policyInfo.currentCarRegisterProv;



            //spec : (สินไหม (Motor))ต้องหา CategoryCode ในการ Mapping ห้าม Hardcode โดยใช้ GUID เด็ดขาด
            incidentEntity.pfc_categoryid = new EntityReference("pfc_category", new Guid("1DCB2B21-0AAB-E611-80CA-0050568D1874"));
            //[CRMDEV_MSCRM].[dbo].[pfc_categoryBase]:[pfc_category_code] =02010201 ,[pfc_category_name] = สินไหม (Motor)


            //fix: (แจ้งอุบัติเหตุรถยนต์ (ผ่าน บ.กลาง))ต้องหา Sub CategoryCode ในการ Mapping ห้าม Hardcode โดยใช้ GUID เด็ดขาด
            incidentEntity.pfc_sub_categoryid = new EntityReference("pfc_sub_category", new Guid("92D632D8-29AB-E611-80CA-0050568D1874"));
            //[CRMDEV_MSCRM].[dbo].[pfc_sub_categoryBase]:  [pfc_sub_category_code]=0201-013 [pfc_sub_category_name]=แจ้งอุบัติเหตุรถยนต์(ผ่าน บ.กลาง)





            //account/contact.pfc_customer_sensitive_level
            Console.WriteLine("customerClientEntity.pfc_customer_sensitive_level" + customerClientEntity.pfc_customer_sensitive_level);
            if (!string.IsNullOrEmpty(customerClientEntity.pfc_customer_sensitive_level.ToString()))
            {
                Console.WriteLine("Is pfc_customer_sensitive_level NOT NullOrEmpty = " + customerClientEntity.pfc_customer_sensitive_level.ToString());
                incidentEntity.pfc_customer_sensitive = new OptionSetValue(customerClientEntity.pfc_customer_sensitive_level); //Default(Unassign)
            }
            else
            {
                incidentEntity.pfc_customer_sensitive = new OptionSetValue(100000000);
            }

            //account/contact.pfc_customer_privilege_level
            Console.WriteLine("customerClientEntity.pfc_customer_privilege_level=" + customerClientEntity.pfc_customer_privilege_level);
            if (!string.IsNullOrEmpty(customerClientEntity.pfc_customer_privilege_level.ToString()))
            {
                Console.WriteLine("Is pfc_customer_privilege_level NOT NullOrEmpty = " + customerClientEntity.pfc_customer_privilege_level.ToString());
                incidentEntity.pfc_customer_privilege = new OptionSetValue(customerClientEntity.pfc_customer_privilege_level); //Default(Unassign)
            }
            else
            {
                incidentEntity.pfc_customer_privilege = new OptionSetValue(100000001);
            }


          


            //จำนวนผู้เสียชีวิต(คาดการณ์) ใส่ได้ไม่เกิน 20 คน หากเกินให้ใส่เป็น 20
            if (apiInput.claimInform.numOfDeath < 1)
            {
                apiInput.claimInform.numOfDeath = 0;
            }
            else if (apiInput.claimInform.numOfDeath > 20)
            {
                apiInput.claimInform.numOfDeath = 20;
            }


            {//Default(Unassign)
             //I: เคลมแห้ง(200, 000, 000) O: เคลมสด(200, 000, 001)
                var claimTypeValue = new OptionSetValue();
                if (null != apiInput.claimInform.claimType)
                {
                    switch (apiInput.claimInform.claimType.ToUpper())
                    {
                        case "I": claimTypeValue = new OptionSetValue(200000000); break;
                        case "O": claimTypeValue = new OptionSetValue(200000001); break;

                    }
                }

                incidentEntity.pfc_claim_type = claimTypeValue;
            }
            {
                //pfc_send_out_surveyor
                int nOut;
                var sendOutSurveyorCodeValue = new OptionSetValue();
                if (Int32.TryParse(apiInput.claimInform.sendOutSurveyorCode, out nOut))
                {
                    sendOutSurveyorCodeValue = new OptionSetValue(100000000 + nOut);
                }
                incidentEntity.pfc_send_out_surveyor = sendOutSurveyorCodeValue;

            }

            {
                //pfc_accident_legal_result
                int nOut;
                var accidentLegalResultValue = new OptionSetValue(100000000);
                if (Int32.TryParse(apiInput.claimInform.accidentLegalResult, out nOut))
                {
                    accidentLegalResultValue = new OptionSetValue(100000000 + nOut);
                }
                incidentEntity.pfc_accident_legal_result = accidentLegalResultValue;

            }


        

            //เป็นไปตาม Logic. ใน Case ว่าในกรณีที่มีการใช้รถยก (Incident.pfc_num_of_tow_truck is not null and pfc_num_of_tow_truck != 100000000) ให้ Fix: 1 (เสียหาย)

            // pfc_high_loss_case_flag

            //เป็นไปตาม Logic.ใน Case
            //แต่ บี ไม่ชั่วว่าจับจาก จำนวนผู้บาดเจ็บ หรือมีข้อมูลสถานีตำรวจ

            //วาในกรณีที่มีการใช้รถยก(Incident.pfc_num_of_accident_injuries is not null and pfc_num_of_accident_injuries != 100000000) 
            //ให้ Fix: 1(อาจจะมีคดีความ)

            // if(apiInput.claimInform.numOfAccidentInjury != 0)
            // {
            //     incidentEntity.pfc_legal_case_flag = true; ;
            // }



            // if (string.IsNullOrEmpty(apiInput.claimInform.deductibleFee.ToString()))
            //{
            //    incidentEntity.pfc_deductable_fee = apiInput.claimInform.deductibleFee;
            // }


            // incidentEntity.reportAccidentResultDate = apiInput.claimInform.reportAccidentResultDate;


            //ใส่ได้ไม่เกิน 20 คน หากเกินให้ใส่เป็น 20
            {
                if (apiInput.claimInform.numOfExpectInjury < 1)
                {
                    apiInput.claimInform.numOfExpectInjury = 0;
                }
                else if (apiInput.claimInform.numOfExpectInjury > 20)
                {
                    apiInput.claimInform.numOfExpectInjury = 20;
                }
                incidentEntity.pfc_num_of_expect_injuries = new OptionSetValue(apiInput.claimInform.numOfExpectInjury + 100000000);
            }

            //ใส่ได้ไม่เกิน 20 คน หากเกินให้ใส่เป็น 20
            {
                if (apiInput.claimInform.numOfAccidentInjury < 1)
                {
                    apiInput.claimInform.numOfAccidentInjury = 0;
                }
                else if (apiInput.claimInform.numOfAccidentInjury > 20)
                {
                    apiInput.claimInform.numOfAccidentInjury = 20;
                }

                incidentEntity.pfc_num_of_accident_injuries = new OptionSetValue(apiInput.claimInform.numOfAccidentInjury + 100000000);
            }

            { //ใส่ได้ไม่เกิน 20 คน หากเกินให้ใส่เป็น 20
              //pfc_num_of_death


                if (apiInput.claimInform.numOfDeath < 1)
                {
                    apiInput.claimInform.numOfDeath = 0;
                }
                else if (apiInput.claimInform.numOfDeath > 20)
                {
                    apiInput.claimInform.numOfDeath = 20;
                }
                incidentEntity.pfc_num_of_death = new OptionSetValue(apiInput.claimInform.numOfDeath + 100000000);

            }



            PfcIncidentDataGateWay db = new PfcIncidentDataGateWay();
            var guid = db.Create(incidentEntity);
            return guid;
        }

    }

    internal static class RVPMortorAccidentFinder
    {

        public static PfcMortorAccident Find(Guid incidentMortorGuid)
        {
            return (new PfcMotorAccidentDataGateway()).Find(incidentMortorGuid);
        }
    }

    internal static class RVPCreateMotorAccidentCommand
    {
        internal static PfcMotorAccidentDataGateway DataContract { get; } = new PfcMotorAccidentDataGateway();
        public static Guid Execute(CRMregClaimRequestFromRVPInputModel apiInput, Guid incidentGuid)
        {
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>CreateIncidentMortor RVP");

            // spec1 : ให้ผูกกับ Case ด้านบน
            PfcMortorAccident entity = new PfcMortorAccident(incidentGuid)
            {
                pfc_motor_accident_name = "เหตุการณ์ที่ 1",// fix
                pfc_activity_date = apiInput.claimInform.accidentOn,  
                pfc_accident_event_detail = apiInput.claimInform.accidentNatureDesc,
                pfc_accident_latitude = apiInput.claimInform.accidentLatitude, 
                pfc_accident_longitude = apiInput.claimInform.accidentLongitude,    
                pfc_accident_location = apiInput.claimInform.accidentPlace,
                pfc_ref_rvp_claim_no = apiInput.rvpCliamNo,

                //จำนวนคู่กรณี
               
                pfc_motor_accident_parties_sum = (apiInput.claimInform.numOfAccidentParty > 0) ? apiInput.claimInform.numOfAccidentParty : 0,

                // fix: 1 หาก params.numOfAccidentParty มีมากกว่า 0
                pfc_event_sequence = (apiInput.claimInform.numOfAccidentParty > 1) ? 1 : 0,

                //pfc_event_code = "" // ตัดออก

            };




            return DataContract.Create(entity);

        }
    }

    internal static class CreateMortorAccidentPartiesCommand
    {
        private static PfcMotorAccidentPartiesDataGateWay MortorAccidentPartyGateWay { get; } = new PfcMotorAccidentPartiesDataGateWay();

        public static void Execute(CRMregClaimRequestFromRVPInputModel apiInput, IncidentEntity incidentEntity, PfcMortorAccident mortorAccidentEntity, Guid mortorAccidentEntityGuid)
        {

            if (apiInput.accidentPartyInfo.Count > 1)
            {
                var i = 0;
                foreach (AccidentPartyInfoModel info in apiInput.accidentPartyInfo)
                {
                    ++i;
                    var entity = new PfcMotorAccidentParties(mortorAccidentEntityGuid)
                    {
                        pfc_event_code = "", //ให้เป็นไปตาม Logic. ของหน้า Motor  คุณไกดบอกให้ปล่อยว่าง
                        pfc_event_sequence = info.rvpAccidentPartySeq, //ให้เป็นไปตาม Logic. ของหน้า Motor 
                        pfc_motor_accident_parties_name = info.accidentPartyFullname,  //ให้เป็นไปตาม Logic. ของหน้า Motor Accident Parties
                        pfc_parties_sequence = i,              //ให้ Running ไปเรื่อยๆ

                        pfc_parties_fullname = info.accidentPartyFullname,
                        pfc_licence_province = info.accidentPartyCarPlateProv,
                        pfc_licence_no = info.accidentPartyCarPlateNo,
                        pfc_phoneno = info.accidentPartyPhone,
                        pfc_insurance_name = info.accidentPartyInsuranceCompany,
                        pfc_policyno = info.accidentPartyPolicyNumber,
                        pfc_policy_type = info.accidentPartyPolicyType,

                       
                        pfc_ref_rvp_claim_no = apiInput.rvpCliamNo,
                        pfc_ref_rvp_parties_seq = info.rvpAccidentPartySeq
                };
                   
                    //entity.pfc_parties_type

                    MortorAccidentPartyGateWay.Create(entity);
                }
            }

        }
    }
}
