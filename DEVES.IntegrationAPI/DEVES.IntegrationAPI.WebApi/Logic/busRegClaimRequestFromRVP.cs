using System;
using System.Activities.Statements;
using System.Linq;
using System.Web.Helpers;
using DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP;
using DEVES.IntegrationAPI.WebApi.DataAccessService;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.Core.ExtensionMethods;
using Microsoft.Xrm.Sdk;
using DEVES.IntegrationAPI.WebApi.DataAccessService.XrmEntity;
using System.Collections.Generic;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class BuzRegClaimRequestFromRVPCommand
    {
        public PfcIncidentDataGateWay IncidentDataGateWay { get;} = new PfcIncidentDataGateWay();
        public CRMPolicyMotorDataGateWay policyDataGateway = new CRMPolicyMotorDataGateWay();


        public CrmregClaimRequestFromRVPContentOutputModel Execute(object input)
        {
            var apiInput = (CRMregClaimRequestFromRVPInputModel)input;
            CrmregClaimRequestFromRVPContentOutputModel outputContent = new CrmregClaimRequestFromRVPContentOutputModel();
            outputContent.data = new CrmRegClaimRequestFromRVPDataOutputModel()
            {
                claimNotiNo = "",
                ticketNo = ""
            };





            // condition #1 
            // Search Policy Additional (sp_API_InquiryPolicyMotorList)
            // ตรวจสอบ  Policy และ PolicyAditional ถ้ามีมากกว่า 1 รายการ ให้ return error
            var resultPolicy = FindPolicy("policyNo", "chassisNo", "carRegisNo", "carRegisProve");
            if (null == resultPolicy)
            {
                throw new Exception("invalidPolicyAndPolicyAditional");
            }


            // condition #2
            // Search insureClientid (sp_API_CustomerClient)
            // หาว่ามี client แล้วหรือยัง ถ้ามีมากกว่า 1 รายการให้ return error ถ้าไม่เจอให้สร้างใหม่
            var resultClient = FindClient("clientType", "clientId");
            if (null == resultClient)
            {
                throw new Exception("The above query returns multiple Insured Client.");
            }


            //condition #3
            // Search DriverClientid (sp_API_CustomerClient)
            // หาว่ามี client แล้วหรือยัง ถ้าไม่ใช่ 1 รายการให้ return

            var resultDriverClient = FindClient("clientType", "clientId");

            if (null == resultDriverClient)
            {
                throw new Exception("The above query returns multiple  driver client.");
            }


            //condition #4
            // Search บ.กลาง (sp_API_CustomerClient)
            // หาว่ามี client แล้วหรือยัง ถ้ามีมากกว่า 1 รายการให้ return error ถ้าไม่เจอให้สร้างใหม่

            var resultrvpClient = FindClient("clientType", "clientId");

            if (null == resultrvpClient)
            {
                throw new Exception("The above query returns multiple  rvp.");
            }

            var informerEntity = FindClient("clientType", "clientId");

            //Valid

            // step #7
            // Insert Incident
            // Get incident.incidentId
            var incidentGuid = CreateIncident(apiInput, resultPolicy, resultClient, informerEntity, resultDriverClient);

            // step #8
            // Insert pfc_motor_accident (pfc_parent_caseId = incident.incidentId) 
            // Get pfc_motor_accident.pfc_motor_accidentId
           
            if (null == incidentGuid || incidentGuid.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                throw new Exception("Cannot Create IncidentEntity ");
            }

            var IncidentEntity = IncidentDataGateWay.Find(incidentGuid);
            var IncidentMortorGuid = CreateIncidentMortor(apiInput, incidentGuid);

            if (null == IncidentMortorGuid || IncidentMortorGuid.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                throw new Exception("Cannot Create Incident Mortor ");
            }

            // step #9
            // Loop Insert pfc_motor_accident_parties (pfc_parent_motor_accidentId = pfc_motor_accident.pfc_motor_accidentId)

            // Run Store sp_ReqMotorClaimNotiNo เพื่อ Generate เลข Claim Noti Number

            // เรียก WebService เพื่อ Create Claim Web Service /ServiceProxy/ClaimMotor/jsonproxy/LOCUS_ClaimRegistration





            return outputContent;
        }

        public CRMPolicyMotorEntity FindPolicy(string policyNo, string chassisNo, string carRegisNo, string carRegisProve)
        {
            return MockRVPData.GetMockPolicy();
            /*
            CRMPolicyMotorDataGateWay policyDataGateway = new CRMPolicyMotorDataGateWay();
            var reqPolicy = new DataRequest();
            reqPolicy.AddParam("policyNo", "2010034076831");
            reqPolicy.AddParam("chassisNo", "");
            reqPolicy.AddParam("carRegisNo", "ตม4533");
            reqPolicy.AddParam("carRegisProve", "");
            var result = policyDataGateway.FetchAll(reqPolicy);
            return result;
            */
        }


        public CustomerClientEntity FindClient(string clientType, string clientId)
        {
            return MockRVPData.GetMockCustomerClient();
            /*
            ClientMasterDataGateway clientMasterDataGateway = new ClientMasterDataGateway();
            var reqClient = new DataRequest();
            reqClient.AddParam("clientType", "P"); // account =1 , 
            reqClient.AddParam("clientId", "CRM5555");
            return clientMasterDataGateway.FetchAll(reqClient);
            */
        }

        public Guid CreateIncident(CRMregClaimRequestFromRVPInputModel apiInput, CRMPolicyMotorEntity policyAdditionalEntity, CustomerClientEntity customerClientEntity, CustomerClientEntity informerEntity, CustomerClientEntity driverEntity)
        {
            var catName = "สินไหม (Motor)";
            var subCatName = "แจ้งอุบัติเหตุรถยนต์(ผ่าน บ.กลาง)";
            var informerGuid = new Guid(customerClientEntity.ContactId);
            var driverGuid = new Guid(informerEntity.ContactId);
            var policyAdditionalIdGuid = new Guid(policyAdditionalEntity.crmPolicyDetailId);
            Guid customeGuid = new Guid(customerClientEntity.ContactId);
            var incidentEntity = new IncidentEntity(policyAdditionalIdGuid, customeGuid, informerGuid, driverGuid);
            //spec 1: มาจากการ Concast CaseType+Case ตาม Business บนหน้าจอ "มาจากการ Concast CaseType+Cate ตาม Business บนหน้าจอ"
            incidentEntity.title = $"{catName} {subCatName} คุณ {apiInput.policyInfo.insuredFullName}";

            //spec 2: fix: 100000002 (RVP)
            incidentEntity.caseorigincode = new OptionSetValue(100000002);

            #region spec 3

            // เป็นไปตาม Logic. ของหน้า Case ที่ว่า ถ้า Policy หรือ Customer เป็น VIP Case นั้นๆต้องเป็น VIP
            // if (pfc_policy_additional.pfc_policy_vip = "1" or contact.pfc_customer_vip = "1") then "1" Else "0"

            incidentEntity.pfc_case_vip = (policyAdditionalEntity.policyVip == true || customerClientEntity.pfc_customer_vip == true);
            #endregion





            // spec : pfc_policy_additional_number = (PolicyAdditional.pfc_policy_additional_name)
            incidentEntity.pfc_policy_additional_number = policyAdditionalEntity.policyAdditionalName;

            //spec :pfc_policy_additional_number = (policyAdditional.policyNo)
            incidentEntity.pfc_policy_additional_number = policyAdditionalEntity.policyNo;

            //spec : (Policy.pfc_chdr_num)(PolicyAdditional.pfc_chdr_num)
            incidentEntity.pfc_policy_number = policyAdditionalEntity.policyNo;

            /*spec : (policyAdditional.pfc_policy_vip) (Policy.pfc_policy_mc_nmc) */
            // ไม่มี
            incidentEntity.pfc_policy_vip = (policyAdditionalEntity.policyVip == true); // Default 0 ;

            //spec : (PolicyAdditional.pfc_policy_additional_name)
            if (!string.IsNullOrEmpty(policyAdditionalEntity.policyMcNmc.ToString()))
            {
                incidentEntity.pfc_policy_mc_nmc = new OptionSetValue(policyAdditionalEntity.policyMcNmc); //Default(Unassign)
            }
            else
            {
                incidentEntity.pfc_policy_mc_nmc = new OptionSetValue();
            }


            incidentEntity.pfc_policyid = new EntityReference("pfc_policy", new Guid(policyAdditionalEntity.policyId));


            //spec : (policyAdditional.pfc_reg_num) = api.currentCarRegisterNo
            incidentEntity.pfc_current_reg_num = apiInput.policyInfo.currentCarRegisterNo;
            //spec : (policyAdditional.pfc_reg_num_prov) = api.currentCarRegisterProv
            incidentEntity.pfc_current_reg_num_prov = apiInput.policyInfo.currentCarRegisterProv;

            //spec : fix: 2 ( Service Request )
            incidentEntity.casetypecode = new OptionSetValue(2);

            //spec : (สินไหม (Motor))ต้องหา CategoryCode ในการ Mapping ห้าม Hardcode โดยใช้ GUID เด็ดขาด
            incidentEntity.pfc_categoryid = new EntityReference("pfc_category", new Guid("1DCB2B21-0AAB-E611-80CA-0050568D1874"));
            //[CRMDEV_MSCRM].[dbo].[pfc_categoryBase]:[pfc_category_code] =02010201 ,[pfc_category_name] = สินไหม (Motor)


            //fix: (แจ้งอุบัติเหตุรถยนต์ (ผ่าน บ.กลาง))ต้องหา Sub CategoryCode ในการ Mapping ห้าม Hardcode โดยใช้ GUID เด็ดขาด
            incidentEntity.pfc_sub_categoryid = new EntityReference("pfc_sub_category", new Guid("92D632D8-29AB-E611-80CA-0050568D1874"));
            //[CRMDEV_MSCRM].[dbo].[pfc_sub_categoryBase]:  [pfc_sub_category_code]=0201-013 [pfc_sub_category_name]=แจ้งอุบัติเหตุรถยนต์(ผ่าน บ.กลาง)

            //spec: fix 100000002
            incidentEntity.pfc_source_data = new OptionSetValue(100000002);

            //customer

            //spec:account/contact.pfc_customer_vip
            incidentEntity.pfc_customer_vip = (customerClientEntity.pfc_customer_vip == true);



            //account/contact.pfc_customer_sensitive_level
            Console.WriteLine("customerClientEntity.pfc_customer_sensitive_level" + customerClientEntity.pfc_customer_sensitive_level);
            if (!string.IsNullOrEmpty(customerClientEntity.pfc_customer_sensitive_level.ToString()))
            {
                Console.WriteLine("Is pfc_customer_sensitive_level NullOrEmpty");
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
                Console.WriteLine("Is pfc_customer_privilege_level NullOrEmpty");
                incidentEntity.pfc_customer_privilege = new OptionSetValue(customerClientEntity.pfc_customer_privilege_level); //Default(Unassign)
            }
            else
            {
                incidentEntity.pfc_customer_privilege = new OptionSetValue(100000001);
            }


            PfcIncidentDataGateWay db = new PfcIncidentDataGateWay();
            var guid = db.Create(incidentEntity);

            


            return guid;
        }

        public Guid CreateIncidentMortor(CRMregClaimRequestFromRVPInputModel apiInput, Guid incidentGuid)
        {
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>CreateIncidentMortor RVP");
            PfcMotorAccidentDataGateway db = new PfcMotorAccidentDataGateway();
            // spec1 : ให้ผูกกับ Case ด้านบน
            PfcMortorAccident entity = new PfcMortorAccident(incidentGuid);
            // spec2 : ใช้ params.accidentOn ได้เลย
            entity.pfc_activity_date = apiInput.claimInform.accidentOn;

            entity.pfc_accident_event_detail = apiInput.claimInform.accidentNatureDesc;

            //ให้เอา params.accidentLatitude มาใช้ได้เลย

            entity.pfc_accident_latitude = apiInput.claimInform.accidentLatitude;
            //ให้เอา params.accidentLongitude มาใช้ได้เลย

            entity.pfc_accident_Longitude = apiInput.claimInform.accidentLongitude;
            //ให้เอา params.accidentPlace มาใช้ได้เลย

            entity.pfc_accident_location = apiInput.claimInform.accidentPlace;
            //จำนวนคู่กรณี
          
                entity.pfc_motor_accident_parties_sum = 0+Int32.Parse(apiInput.claimInform.numOfAccidentParty);
                // fix: 1 หาก params.numOfAccidentParty มีมากกว่า 0
                entity.pfc_event_sequence = (apiInput.accidentPartyInfo.Count >1 )? 1 : 0;
            
            //เลขเคลมของ RVP
            entity.pfc_ref_rvp_claim_no = apiInput.rvpCliamNo;

            entity.pfc_event_code = "X";


            return db.Create(entity);

        }


       


    }

    internal class MockRVPData{

        public static CRMPolicyMotorEntity GetMockPolicy()
        {
            return new CRMPolicyMotorEntity
            {
                crmPolicyDetailId = "3BFCB0A4-DCB6-E611-80CA-0050568D1874",
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
                policyId = "0ea8fb62-6db7-e611-80ca-0050568d1874",
                policyMcNmc = 100000000

            };
        }
        public static CustomerClientEntity GetMockCustomerClient()
        {
            return new CustomerClientEntity
            {
                ContactId = "b55765f1-c4a4-e611-80ca-0050568d1874",
                DefaultPriceLevelId = null,
                CustomerSizeCode = 1,
                CustomerTypeCode = 1,
                PreferredContactMethodCode = 4,
                LeadSourceCode = 1,
                OriginatingLeadId = null,
                OwningBusinessUnit = "506e354e-b8a1-e611-80c7-0050568d1874",
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
                CreatedBy = "f38f354e-b8a1-e611-80c7-0050568d1874",
                ModifiedOn = "2017-04-05T12=10=00",
                ModifiedBy = "71ffbed1-e819-e711-80d4-0050568d1874",
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
                TransactionCurrencyId = "f7c3d6ad-b8a1-e611-80c7-0050568d1874",
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
                OwnerId = "f38f354e-b8a1-e611-80c7-0050568d1874",
                CreatedOnBehalfBy = null,
                IsAutoCreate = false,
                ModifiedOnBehalfBy = null,
                ParentCustomerId = null,
                ParentCustomerIdType = null,
                ParentCustomerIdName = null,
                OwnerIdType = 8,
                ParentCustomerIdYomiName = null,
                ProcessId = null,
                EntityImageId = "da4605d2-fbb6-e611-80ca-0050568d1874",
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
            var apiInput = new CRMregClaimRequestFromRVPInputModel();
            apiInput.claimInform = new ClaimInformModel();
            apiInput.policyInfo = new PolicyInfoModel();
            apiInput.accidentPartyInfo = new List<AccidentPartyInfoModel>();

            return apiInput;
        }
    }
}