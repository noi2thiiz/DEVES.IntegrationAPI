using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP;
using DEVES.IntegrationAPI.WebApi.DataAccessService.XrmEntity;
using DEVES.IntegrationAPI.WebApi.DataAccessService;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using DEVES.IntegrationAPI.WebApi.Core.ExtensionMethods;
using DEVES.IntegrationAPI.WebApi.Logic.RVP;
using DEVES.IntegrationAPI.Core.TechnicalService.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class BuzRegClaimRequestFromRVPCommandTests
    {
        protected CRMPolicyMotorEntity GetMockPolicy()
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
        protected CustomerClientEntity GetMockCustomerClient()
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
        protected CRMregClaimRequestFromRVPInputModel GetMockApiInput()
        {
            Random rnd = new Random();
            string code = rnd.Next(1, 9999).ToString();
            string rvpCliamNo = code.PadLeft(5, '0'); 
            
                        var apiInput = new CRMregClaimRequestFromRVPInputModel()
            {
                rvpCliamNo = rvpCliamNo,
                policyInfo = new PolicyInfoModel
                {
                    policyNo = "C7121677",//C7121677
                    carChassisNo = "MR0GR19G407004507",
                    currentCarRegisterNo = "ตม4533",
                    currentCarRegisterProv = "กท",
                    insuredClientId = "CRM5555",
                    insuredClientType = "P",
                    insuredFullName = "สมคิด ดวนสันเทียะ",
                    barcode = "2010034076831",
                    crmPolicyDetailCode = "",
                    crmPolicyDetailId = "",
                    fleetCarNo = "1",
                    insuredCleansingId = "",
                    renewalNo = "1"



                },
                claimInform = new ClaimInformModel
                {
                    accidentLongitude = "999.999999",
                    accidentLatitude = "888.888888",
                    accidentNatureDesc = "ผมขับรถมาตามปรกติ บนถนนหลวง หมายเลข 226 สุรินทร์-ศรีสะเกษ ผมมองกระจกหลังเห็นรถกระบะ DMAX แซงรถคันหลังมาเรื่อยๆ ผมมองกระจกหลังเห็นกระบะ",
                    accidentPlace = "บนถนนหลวง หมายเลข 226 สุรินทร์-ศรีสะเกษ",
                    accidentOn = DateTime.Now,
                    accidentDescCode = "101",
                    accidentProvn = "10",
                    accidentDist = "1001",
                    accidentLegalResult="1",


                    numOfAccidentParty = 2,
                    numOfAccidentInjury = 5,
                    numOfExpectInjury=1,
                    numOfDeath = 5,
                    claimType = "O",
                    sendOutSurveyorCode = "01",
                    policeStation= "สน. พระนคร",
                    policeBailFlag="Y",
                    policeRecordDate = DateTime.Now,
                    policeRecordId="4444",

                    caseOwnerCode= "sasipa.b",
                    caseOwnerFullName= "ษษิภา บัวใหญ่",

                    informByCrmId =  "sasipa.b",
                    informByCrmName = "ษษิภา บัวใหญ่",

                    submitByCrmId =  "sasipa.b",
                    submitByCrmName= "ษษิภา บัวใหญ่"

                },
                policyDriverInfo = new PolicyDriverInfoModel
                {
                    driverCleansingId = "",
                    driverFullName = "ออมศิริณร์ รักษ์วงศ์",
                    driverClientId = "CRM5555",
                    driverMobile = "98766655556",
                    driverPhoneNo = "0869999999"
                  
                },
               accidentPartyInfo = new List<AccidentPartyInfoModel>
            {
                new AccidentPartyInfoModel
                {
                    accidentPartyCarPlateNo = "ตก3456",
                    accidentPartyCarPlateProv = "กท",
                    accidentPartyFullname = "มานะ",
                    accidentPartyInsuranceCompany = "เทเวศประกันภัย",
                    accidentPartyPhone = "0987776543",
                    accidentPartyPolicyNumber = "STREDSSJ",
                    accidentPartyPolicyType = "พรบ.",
                    rvpAccidentPartySeq = 1,



                },
                new AccidentPartyInfoModel
                {
                    accidentPartyCarPlateNo = "พร8432",
                    accidentPartyCarPlateProv = "กท",
                    accidentPartyFullname = "ปิติ",
                    accidentPartyInsuranceCompany = "เทเวศประกันภัย",

                    accidentPartyPhone = "0976665432",
                    accidentPartyPolicyNumber = "3445D334W",
                    accidentPartyPolicyType = "พรบ.",
                    rvpAccidentPartySeq = 2
                }
            }
            };

           
            apiInput.claimInform.numOfExpectInjury = 1;
            apiInput.claimInform.numOfAccidentInjury = 5;
            apiInput.claimInform.numOfDeath = 9;
            apiInput.claimInform.excessFee = new decimal(200.50); // แก้ไข spect เดิมเป็น int
            apiInput.claimInform.deductibleFee = new decimal(500.50); // แก้ไข spect เดิมเป็น int


            return apiInput;
        }

        [TestMethod()]
        public void CreateIncidentMortorTest()
        {
            //    CRMregClaimRequestFromRVPInputModel apiInput, IncidentEntity incidentEntity
            var apiInput = GetMockApiInput();

            apiInput.accidentPartyInfo.Add(new AccidentPartyInfoModel());
            apiInput.accidentPartyInfo.Add(new AccidentPartyInfoModel());

            //TestCase
            var pfcActivityDate = DateTime.Now;
            var Incidentguid = new Guid("58b9dccc-9a1a-e711-80d4-0050568d1874");
            apiInput.claimInform.accidentOn = pfcActivityDate;
            apiInput.claimInform.accidentNatureDesc = "Accident Nature Desc";
            apiInput.claimInform.numOfAccidentParty = 10;
            apiInput.rvpCliamNo = "xxxxxxx";

            apiInput.claimInform.accidentLatitude = "101";
            apiInput.claimInform.accidentLongitude = "102";
            apiInput.claimInform.accidentPlace = "BKK";
            apiInput.claimInform.numOfAccidentInjury = 2;





            var cmd = new BuzRegClaimRequestFromRVPCommand();

            var guid = cmd.CreateIncidentMortor(apiInput, Incidentguid);
            Assert.IsNotNull(guid);
            Assert.IsInstanceOfType(guid, typeof(Guid));

            PfcMotorAccidentDataGateway dg = new PfcMotorAccidentDataGateway();
            var attributes = new ColumnSet();
            attributes.AddColumns(new String[] { "pfc_parent_caseid" ,"pfc_event_code",
                "pfc_motor_accident_name", "pfc_ref_rvp_claim_no", "pfc_activity_date" ,
                "pfc_event_sequence", "pfc_accident_event_detail" , "pfc_motor_accident_parties_sum","pfc_accident_location","pfc_accident_longitude","pfc_accident_latitude" });//, 
            var entity = dg.Retrieve(guid, attributes);
            //""


         
          


            Assert.AreEqual(new EntityReference("incident", Incidentguid), entity["pfc_parent_caseid"], "ให้ผูกกับ Case ด้านบน");
            Assert.AreEqual(1, entity["pfc_event_sequence"], "fix: 1 หาก params.numOfAccidentParty มีมากกว่า 0");
            //Assert.AreEqual(pfcActivityDate,(DateTime) entity["pfc_activity_date"], "ใช้ params.accidentOn ได้เลย");
            // accidentNatureDesc
            Assert.AreEqual(apiInput.claimInform.accidentNatureDesc, entity["pfc_accident_event_detail"], " = api.claimInform.accidentNatureDesc");

            Assert.AreEqual(apiInput.claimInform.numOfAccidentParty, entity["pfc_motor_accident_parties_sum"], " = apiInput.claimInform.numOfAccidentParty");

            Assert.AreEqual(apiInput.rvpCliamNo, entity["pfc_ref_rvp_claim_no"], " = apiInput.rvpClaimNo");

            //ในกรณี Inhouse ให้ใช้ eventdetail.EventID
            //ในกรณี Outsource ให้ใช้ Incident.pfc_isurvey_params_event_code
            Assert.AreEqual("X", entity["pfc_event_code"], " ให้เป็นไปตาม Logic. ของหน้า Motor Accident");

            Assert.AreEqual(apiInput.claimInform.accidentLatitude, entity["pfc_accident_latitude"], " = api");
            Assert.AreEqual(apiInput.claimInform.accidentLongitude, entity["pfc_accident_longitude"], " = api");
            Assert.AreEqual(apiInput.claimInform.accidentPlace, entity["pfc_accident_location"], " = api");


            //pfc_motor_accident_name

        }
        [TestMethod()]
        public void CreateIncidentMortor_Case_1_pfc_event_sequence_Shoud_1_if_Party_more_then_one_Test()
        {
            //    CRMregClaimRequestFromRVPInputModel apiInput, IncidentEntity incidentEntity
            var apiInput = GetMockApiInput();

            apiInput.accidentPartyInfo.Add(new AccidentPartyInfoModel());
            apiInput.accidentPartyInfo.Add(new AccidentPartyInfoModel());

            //TestCase
            var pfcActivityDate = DateTime.Now;
            var Incidentguid = new Guid("58b9dccc-9a1a-e711-80d4-0050568d1874");
            apiInput.claimInform.accidentOn = pfcActivityDate;
            apiInput.claimInform.numOfAccidentParty = 2;




            var cmd = new BuzRegClaimRequestFromRVPCommand();
            var guid = cmd.CreateIncidentMortor(apiInput, Incidentguid);
            Assert.IsNotNull(guid);
            Assert.IsInstanceOfType(guid, typeof(Guid));

            PfcMotorAccidentDataGateway dg = new PfcMotorAccidentDataGateway();
            var attributes = new ColumnSet();
            attributes.AddColumns(new String[] { "pfc_event_sequence", "pfc_motor_accident_parties_sum" });//, 
            var entity = dg.Retrieve(guid, attributes);

            Assert.AreEqual(1, (int)entity["pfc_event_sequence"], "fix: 1 หาก params.numOfAccidentParty มีมากกว่า 0");
            Assert.AreEqual(2, (int)entity["pfc_motor_accident_parties_sum"]);


        }
        [TestMethod()]
        public void CreateIncidentMortor_Case_2_pfc_event_sequence_Shoud_0_Test()
        {
            //    CRMregClaimRequestFromRVPInputModel apiInput, IncidentEntity incidentEntity
            var apiInput = GetMockApiInput();



            //TestCase
            var pfcActivityDate = DateTime.Now;//25B4B411-9E1B-E711-80D4-0050568D1874%257d
                    var Incidentguid = new Guid("58b9dccc-9a1a-e711-80d4-0050568d1874");
            apiInput.claimInform.accidentOn = pfcActivityDate;
            apiInput.claimInform.numOfAccidentParty = 0;




            var cmd = new BuzRegClaimRequestFromRVPCommand();
            var guid = cmd.CreateIncidentMortor(apiInput, Incidentguid);
            Assert.IsNotNull(guid);
            Assert.IsInstanceOfType(guid, typeof(Guid));

            PfcMotorAccidentDataGateway dg = new PfcMotorAccidentDataGateway();
            var attributes = new ColumnSet();
            attributes.AddColumns(new String[] { "pfc_event_sequence", "pfc_motor_accident_parties_sum" });//, 
            var entity = dg.Retrieve(guid, attributes);

            Assert.AreEqual(0, (int)entity["pfc_event_sequence"], "fix: 1 หาก params.numOfAccidentParty มีมากกว่า 0");
            Assert.AreEqual(0, (int)entity["pfc_motor_accident_parties_sum"]);


        }

        [TestMethod()]
        public void CreateIncidentTest()
        {


            // var Incidentguid = new Guid("58b9dccc-9a1a-e711-80d4-0050568d1874");
            //ค่าที่ต้องไป หามาจากตัวอื่น



            var apiInput = GetMockApiInput();
           

            var catName = "สินไหม (Motor)";
            var subCatName = "แจ้งอุบัติเหตุรถยนต์(ผ่าน บ.กลาง)";

            var policyEntity = GetMockPolicy();
            var customerClientEntity = GetMockCustomerClient();
            var informerEntity = GetMockCustomerClient();
            var driverEntity = GetMockCustomerClient();

            //TestCase
            policyEntity.policyVip = true;
            policyEntity.policyAdditionalName = "C7121677";
            customerClientEntity.pfc_customer_vip = false;
            apiInput.claimInform.numOfExpectInjury = 1;
            apiInput.claimInform.numOfAccidentInjury = 5;
            apiInput.claimInform.numOfDeath = 9;
            apiInput.claimInform.excessFee = new decimal(200.50); // แก้ไข spect เดิมเป็น int
            apiInput.claimInform.deductibleFee = new decimal(500.50); // แก้ไข spect เดิมเป็น int

            var cmd = new BuzRegClaimRequestFromRVPCommand();
            var guid = cmd.CreateIncident(apiInput, policyEntity, customerClientEntity, informerEntity, driverEntity);
            Assert.IsNotNull(guid);
            Assert.IsInstanceOfType(guid, typeof(Guid));




            //check Value
            PfcIncidentDataGateWay dg = new PfcIncidentDataGateWay();
            var attributes = new ColumnSet(); //"pfc_send_out_surveyor"
            attributes.AddColumns(new String[] {"title", "ticketnumber", "pfc_case_vip","pfc_claim_type",
                "pfc_num_of_expect_injuries", "caseorigincode","casetypecode", "pfc_source_data",
                "pfc_sub_categoryid", "pfc_categoryid","pfc_policy_additional_number"});
            //   attributes.AddColumns(new String[] { "ticketnumber" });
            attributes.AddColumn("pfc_send_out_surveyor");

            attributes.AddColumn("pfc_accident_province");
            attributes.AddColumn("pfc_accident_district");

            attributes.AddColumn("pfc_accident_latitude");
            attributes.AddColumn("pfc_accident_longitude");
            attributes.AddColumn("pfc_accident_place");
            attributes.AddColumn("pfc_accident_desc_code");
            //pfc_police_record_id pfc_police_bail_flag pfc_police_station
            attributes.AddColumn("pfc_police_record_id");
            attributes.AddColumn("pfc_police_bail_flag");
            attributes.AddColumn("pfc_police_station");
            attributes.AddColumn("pfc_accident_legal_result");
            attributes.AddColumn("pfc_num_of_accident_injuries");
            attributes.AddColumn("pfc_num_of_death");
            attributes.AddColumn("pfc_excess_fee");
            attributes.AddColumn("pfc_deductable_fee");
            attributes.AddColumn("pfc_motor_accident_sum");
           // attributes.AddColumn("description");
            

            // attributes.AddColumn("pfc_motor_accident_parties_sum");
            // attributes.AddColumn("pfc_accident_event_detail");

            var entity = dg.Retrieve(guid, attributes);

            // "rvpCliamNo": 999999,
            //"policyInfo": {
            //              "crmPolicyDetailId": "",
            //  "crmPolicyDetailCode": "",
            //  "policyNo": "C7121677",
            Assert.AreEqual("C7121677", entity["pfc_policy_additional_number"]);
            //   "renewalNo": "1",
            //   "fleetCarNo": "1",
            //   "barcode": "2010034076831",
            //   "carChassisNo": "MR0GR19G407004507",
            //   "currentCarRegisterNo": "ตม4533",
            //   "currentCarRegisterProv": "กท",
            //   "insuredCleansingId": "",
            //   "insuredClientType": "P",
            //   "insuredFullName": "สมคิด ดวนสันเทียะ",
            //   "insuredClientId": "CRM5555"
            //},

            //"pfc_send_out_surveyor"

            // claimInform": {
            //     "informerOn": "2017-01-15 14:01:25",
            //     "accidentOn": "2017-01-15 14:01:25",
            //     "accidentDescCode": "101",
                                        Assert.AreEqual("101", entity["pfc_accident_desc_code"]);

            //     "accidentDesc": "",
            //     "numOfExpectInjury": 1,
                                         Assert.AreEqual((new OptionSetValue(100000001)).Value, ((OptionSetValue)entity["pfc_num_of_expect_injuries"]).Value, "pfc_num_of_expect_injuries");
            //     "accidentLatitude": "888.888888",
                                        Assert.AreEqual("888.888888", entity["pfc_accident_latitude"], " error on pfc_accident_latitude");
            //     "accidentLongitude": "999.999999",
                                        Assert.AreEqual("999.999999", entity["pfc_accident_longitude"], " error on pfc_accident_longitude");
            //     "accidentPlace": "บนถนนหลวง หมายเลข 226 สุรินทร์-ศรีสะเกษ",
                                        Assert.AreEqual("บนถนนหลวง หมายเลข 226 สุรินทร์-ศรีสะเกษ", entity["pfc_accident_place"], " error on pfc_accident_place");
            //     "accidentProvn": "10",
                                        Assert.AreEqual("กรุงเทพมหานคร", entity["pfc_accident_province"], " error on pfc_accident_province");
            //     "accidentDist": "1001",
                                        Assert.AreEqual("พระนคร", entity["pfc_accident_district"], " error on pfc_accident_district");
            //     "claimType": "O",
                                        Assert.AreEqual(new OptionSetValue(200000001), entity["pfc_claim_type"], " error on pfc_claim_type");
            //     "sendOutSurveyorCode": "01",
                                        Assert.AreEqual(new OptionSetValue(100000001), entity["pfc_send_out_surveyor"]);//" error on pfc_send_out_surveyor เลือกหัวข้อย่อยของประเภทเคลม"
            //     "reportAccidentResultDate": "2017-01-15 14:01:25",
                                     //     "caseOwnerCode": "sasipa.b",
                                        //     "caseOwnerFullName": "ษษิภา บัวใหญ่",
                                     //     "informByCrmId": "sasipa.b",
                                      //     "informByCrmName": "ษษิภา บัวใหญ่",
                                     //     "submitByCrmId": "sasipa.b",
                                  //     "submitByCrmName": "ษษิภา บัวใหญ่",
           //     "accidentLegalResult": 1,
                                    Assert.AreEqual(new OptionSetValue(100000001), entity["pfc_accident_legal_result"], " error on pfc_accident_legal_result");

            //     "numOfAccidentInjury": 5,
                                   Assert.AreEqual((new OptionSetValue(100000005)).Value, ((OptionSetValue)entity["pfc_num_of_accident_injuries"]).Value, " error on pfc_num_of_accident_injuries");

            //     "numOfDeath": 9,
                                  Assert.AreEqual((new OptionSetValue(100000009)).Value, ((OptionSetValue)entity["pfc_num_of_death"]).Value, " error on pfc_num_of_death");

            //     "excessFee": 200,
                                    Assert.AreEqual(Decimal.Parse("200.50"), entity["pfc_excess_fee"], " error on pfc_excess_fee");

            //     "deductibleFee": 0,
                                    Assert.AreEqual(Decimal.Parse("500.50"), entity["pfc_deductable_fee"], " error on pfc_deductable_fee");


            //     "accidentNatureDesc": "ผมขับรถมาตามปรกติ บนถนนหลวง หมายเลข 226 สุรินทร์-ศรีสะเกษ ผมมองกระจกหลังเห็นรถกระบะ DMAX แซงรถคันหลังมาเรื่อยๆ ผมมองกระจกหลังเห็นกระบะ",
           // Assert.AreEqual(apiInput.claimInform.accidentDesc, entity["description"], " error on description");
            //     "policeStation": "สน. พระนคร",
            Assert.AreEqual("สน. พระนคร", entity["pfc_police_station"], " error on pfc_police_station");

            //     "policeRecordId": "444444",
                                                Assert.AreEqual("4444", entity["pfc_police_record_id"], " error on pfc_police_record_id");
            //     "policeRecordDate": "2017-01-15 14:01:25",
            //     "policeBailFlag": "Y",
                                                Assert.AreEqual(true, entity["pfc_police_bail_flag"], " error on pfc_police_bail_flag");
            //     "numOfAccidentParty": 2 =>MotorAcident
                                                
            //   }

            // pfc_driver_mobile = apiInput.policyDriverInfo.driverMobile,
            //   pfc_accident_legal_result = apiInput.claimInform.accidentLegalResult,


            //  pfc_police_record_date = apiInput.claimInform.policeRecordDate, 


            // internal data
            Assert.AreEqual(true, entity["pfc_case_vip"], "if (pfc_policy_additional.pfc_policy_vip = 1 or contact.pfc_customer_vip = 1) then 1 Else 0");
            Assert.AreEqual(true, entity["pfc_case_vip"]);
            Assert.AreEqual($"{catName} {subCatName} คุณ {apiInput.policyInfo.insuredFullName}", entity["title"], " มาจากการ Concast CaseType + Cate ตาม Business บนหน้าจอ");



            //Fix value
            Assert.AreEqual(new OptionSetValue(2), entity["casetypecode"], "fix: 100000002 (RVP)");
            Assert.AreEqual(new OptionSetValue(100000002), entity["caseorigincode"], "error on caseorigincode");
            Assert.AreEqual(new EntityReference("pfc_sub_category", new Guid("92D632D8-29AB-E611-80CA-0050568D1874")), entity["pfc_sub_categoryid"]);
            Assert.AreEqual(new EntityReference("pfc_category", new Guid("1DCB2B21-0AAB-E611-80CA-0050568D1874")), entity["pfc_categoryid"]);
            Assert.AreEqual(new OptionSetValue(100000002), entity["caseorigincode"]);
            Assert.AreEqual(new OptionSetValue(100000002), entity["pfc_source_data"]);
            Assert.AreEqual(1, entity["pfc_motor_accident_sum"]);
            
            // Auto
            Assert.IsNotNull(entity["ticketnumber"]);






        }

        private object OptionSetValue(int v)
        {
            throw new NotImplementedException();
        }

        private object GetCustomerClient()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        //
        ///<summary>
        ///เป็นไปตาม Logic. ของหน้า Case ที่ว่า ถ้า Policy หรือ Customer เป็น VIP Case นั้นๆต้องเป็น VIP 
        ///if(pfc_policy_additional.pfc_policy_vip = "1" or contact.pfc_customer_vip = "1") then "1" Else "0"
        /// </summary>
        public void CreateIncident_Case1_It_Shoud_Create_Case_VIP_When_Give_policyVip_Is_True_Test()
        {

            var informerEntity = GetMockCustomerClient();
            var driverEntity = GetMockCustomerClient();

            var apiInput = GetMockApiInput();
            var policyEntity = GetMockPolicy();
            var customerClientEntity = GetMockCustomerClient();

            //Test
            policyEntity.policyVip = true;
            customerClientEntity.pfc_customer_vip = false;

            var cmd = new BuzRegClaimRequestFromRVPCommand();
            var guid = cmd.CreateIncident(apiInput, policyEntity, customerClientEntity, informerEntity, driverEntity);
            Assert.IsNotNull(guid);
            Assert.IsInstanceOfType(guid, typeof(Guid));

            //check VIP
            PfcIncidentDataGateWay dg = new PfcIncidentDataGateWay();
            var attributes = new ColumnSet();
            attributes.AddColumn("pfc_case_vip");
            var entity = dg.Retrieve(guid, attributes);
            Assert.AreEqual(true, entity["pfc_case_vip"]);

        }

        [TestMethod]
        //
        ///<summary>
        ///เป็นไปตาม Logic. ของหน้า Case ที่ว่า ถ้า Policy หรือ Customer เป็น VIP Case นั้นๆต้องเป็น VIP 
        ///if(pfc_policy_additional.pfc_policy_vip = "1" or contact.pfc_customer_vip = "1") then "1" Else "0"
        /// </summary>
        public void CreateIncident_Case2_It_Shoud_Create_Case_VIP_When_Give_contactVip_Is_True_Test()
        {

            var informerEntity = GetMockCustomerClient();
            var driverEntity = GetMockCustomerClient();
            var apiInput = GetMockApiInput();
            var policyEntity = GetMockPolicy();
            var customerClientEntity = GetMockCustomerClient();

            //Test  Case
            policyEntity.policyVip = false;
            customerClientEntity.pfc_customer_vip = true;

            var cmd = new BuzRegClaimRequestFromRVPCommand();
            var guid = cmd.CreateIncident(apiInput, policyEntity, customerClientEntity, informerEntity, driverEntity);
            Assert.IsNotNull(guid);
            Assert.IsInstanceOfType(guid, typeof(Guid));

            //check VIP
            PfcIncidentDataGateWay dg = new PfcIncidentDataGateWay();
            var attributes = new ColumnSet();
            attributes.AddColumn("pfc_case_vip");
            var entity = dg.Retrieve(guid, attributes);
            Assert.AreEqual(true, entity["pfc_case_vip"]);

        }

        [TestMethod]
        //
        ///<summary>
        ///เป็นไปตาม Logic. ของหน้า Case ที่ว่า ถ้า Policy หรือ Customer เป็น VIP Case นั้นๆต้องเป็น VIP 
        ///if(pfc_policy_additional.pfc_policy_vip = "1" or contact.pfc_customer_vip = "1") then "1" Else "0"
        /// </summary>
        public void CreateIncident_Case3_It_Shoud_Create_None_Case_VIP_When_Give_contactVip_And_policyVip_Is_False_Test()
        {

            var informerEntity = GetMockCustomerClient();
            var driverEntity = GetMockCustomerClient();

            var apiInput = GetMockApiInput();
            var policyEntity = GetMockPolicy();
            var customerClientEntity = GetMockCustomerClient();

            //Test  Case
            policyEntity.policyVip = false;
            customerClientEntity.pfc_customer_vip = false;

            var cmd = new BuzRegClaimRequestFromRVPCommand();
            var guid = cmd.CreateIncident(apiInput, policyEntity, customerClientEntity, informerEntity, driverEntity);
            Assert.IsNotNull(guid);
            Assert.IsInstanceOfType(guid, typeof(Guid));

            //check VIP
            PfcIncidentDataGateWay dg = new PfcIncidentDataGateWay();
            var attributes = new ColumnSet();
            attributes.AddColumn("pfc_case_vip");
            var entity = dg.Retrieve(guid, attributes);
            Assert.AreEqual(false, entity["pfc_case_vip"]);

        }

        [TestMethod()]
        public void ExecuteTest()
        {
            var apiInput = GetMockApiInput();
            
            var cmd = new BuzRegClaimRequestFromRVPCommand();
            var result = cmd.Execute(apiInput);
            //Assert.AreEqual("",result.ToJSON());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.data.ticketNo);
            Assert.IsNotNull(result.data.claimNotiNo);

          //  Assert.AreEqual(apiInput.ToJSON(), "");
        }

        [TestMethod()]

        public void Execute_Shoud_thow_Error_Policy_NotFound_Test()
        {
            var apiInput = GetMockApiInput();
            apiInput.policyInfo.policyNo = "999999999999999999";
         

            Exception expectedExcetpion = null;
            try
            {
                var cmd = new BuzRegClaimRequestFromRVPCommand();
                cmd.Execute(apiInput);
            }
            catch (BuzInValidBusinessConditionException e)
            {
                expectedExcetpion = e;
                
            }
            catch (Exception e)
            {
                expectedExcetpion = e;
                
            }

            Assert.IsInstanceOfType(expectedExcetpion,typeof(BuzInValidBusinessConditionException));
            var ErrorMessage = ((BuzInValidBusinessConditionException)expectedExcetpion).ErrorMessage;
            Assert.IsNotNull(ErrorMessage);
            Assert.AreEqual("ERROR-RVP021", ErrorMessage.code);
            //  Assert.ThrowsException<BuzInValidBusinessConditionException>(() =>  cmd.Execute(apiInput));

        }

        [TestMethod()]
        public void GetInformerInfoTest()
        {
            var cmd = new BuzRegClaimRequestFromRVPCommand();

            Dictionary<string, dynamic> informer =cmd.GetInformerInfo();
            Assert.IsTrue(informer.ContainsKey("pfc_polisy_client_id"));
            Assert.AreEqual("10077508", informer["pfc_polisy_client_id"]);
            Assert.AreEqual(new Guid("81CBAA5F-AEB6-E611-80CA-0050568D1874"), informer["AccountId"]);
            // Assert.AreEqual()
        }
    }
}