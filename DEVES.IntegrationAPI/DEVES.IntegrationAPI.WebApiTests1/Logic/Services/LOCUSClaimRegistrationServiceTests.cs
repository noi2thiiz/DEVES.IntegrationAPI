using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Core.Util;
using DEVES.IntegrationAPI.Model.ClaimRegistration;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class LOCUSClaimRegistrationServiceTests
    {
        [TestMethod()]
        public void Execute_LOCUSClaimRegistrationService_It_should_return_success_when_give_valid_input()
        {
            /*Expected Result  แต่  input จะใช้ได้ถ้า เลข claimNotiNo  นี้โดนลบจาก locus ไปแล้ว
             {
              "data": {
                "claimId": "1398",
                "claimNo": "V0533407",
                "ticketNumber": "CAS209909-10654"
              },
              "code": "200",
              "message": "Success",
              "description": "Registration Success",
              "transactionId": "38399",
              "transactionDateTime": "2017-09-11 02:07:47"
            }
             
             */
            try
            {
                var service =
                    new LOCUSClaimRegistrationService(Guid.NewGuid().ToString(), "UnitTest");
                var result = service.Execute(new LocusClaimRegistrationInputModel
                {
                    claimHeader = new LocusClaimheaderModel
                    {
                        premiumClass = "MVS",
                        ticketNumber = "CAS209909-"+RandomValueGenerator.RandomNumber(5),
                        claimNotiNo = "9909-"+RandomValueGenerator.RandomNumber(5),
                        claimNotiRefer = null,
                        policyNo = "V0756004",
                        fleetCarNo = 1,
                        policySeqNo = 1,
                        renewalNo = 0,
                        barcode = "2080003047183",
                        insureCardNo = null,
                        policyIssueDate = DateTime.Now, //"2015-12-17 00:00:00"
                        policyEffectiveDate = DateTime.Now, //"2016-01-29 00:00:00"
                        policyExpiryDate = DateTime.Now, //"2017-10-29 00:00:00"
                        policyProductTypeCode = "FE",
                        policyProductTypeName = "ประเภท 2++",
                        policyGarageFlag = "N",
                        policyPaymentStatus = "N",
                        policyCarRegisterNo = "ษม7045",
                        policyCarRegisterProv = "กท",
                        carChassisNo = "MR053BK3005017550@",
                        carVehicleType = null,
                        carVehicleModel = "TOYOTA CAMRY",
                        carVehicleYear = "2004",
                        carVehicleBody = "SEDAN",
                        carVehicleSize = null,
                        policyDeduct = null,
                        agentCode = "90001521",
                        agentName = "หักส่วนลดหน้าตาราง",
                        agentBranch = "00",
                        vipCaseFlag = "N",
                        privilegeLevel = "1",
                        highLossCaseFlag = "N",
                        legalCaseFlag = "N",
                        claimNotiRemark = "claimNotiNo นี้ไม่มีจริงใน CRM  ทดสอบ API เท่านั้น",
                        claimType = "O",
                        informByCrmId = "crmtest1",
                        informByCrmName = "crm test1",
                        submitByCrmId = "crmtest1",
                        submitByCrmName = "crm test1",
                        serviceBranch = "00",
                        policyAdditionalID = new Guid("d78d5356-f4b6-e611-80ca-0050568d1874"),
                        policyBranch = "00"
                    },
                    claimInform = new LocusClaiminformModel
                    {
                        informerClientId = "14514669",
                        informerFullName = "ธวัชชัย จันทน์แดง",
                        informerMobile = "0863156332",
                        informerPhoneNo = null,
                        driverClientId = "14514669",
                        driverFullName = "claimNotiNo นี้ไม่มีจริงใน CRM",
                        driverMobile = "0863156332",
                        driverPhoneNo = null,
                        insuredClientId = "10033581",
                        insuredFullName = "ธวัชชัย จันทน์แดง",
                        insuredMobile = null,
                        insuredPhoneNo = null,
                        relationshipWithInsurer = "00",
                        currentCarRegisterNo = "ษม7045",
                        currentCarRegisterProv = "กท",
                        informerOn = DateTime.Now, /*"2017-09-08 13:08:00"*/
                        accidentOn = DateTime.Now, /*"2017-09-07 08:00:00"*/
                        accidentDescCode = "202",
                        accidentDesc =
                            " วันที่เกิดเหตุ : 7-9-2017 เวลา 08:00 น.\n การเกิดเหตุ : คู่กรณีชนแล้วหลบหนี จำทะเบียนได้",
                        numOfExpectInjury = 0,
                        accidentPlace = "97 ถนน ดินสอ แขวง วัดบวรนิเวศ เขต พระนคร กรุงเทพมหานคร 10200 ประเทศไทย",
                        accidentLatitude = "13.757",
                        accidentLongitude = "100.502",
                        accidentProvn = null,
                        accidentDist = null,
                        sendOutSurveyorCode = "01"
                    },
                    claimAssignSurv = new LocusClaimassignsurvModel
                    {
                        surveyorCode = null,
                        surveyorClientNumber = null,
                        surveyorName = null,
                        surveyorCompanyName = null,
                        surveyorCompanyMobile = null,
                        surveyorMobile = null,
                        surveyorType = null,
                        surveyTeam = "TEAM0099"
                    },
                    claimSurvInform = new LocusClaimsurvinformModel
                    {
                        deductibleFee = null,
                        excessFee = null,
                        reportAccidentResultDate = null,
                        accidentLegalResult = null,
                        policeStation = null,
                        policeRecordId = null,
                        policeRecordDate = null,
                        policeBailFlag = "N",
                        damageOfPolicyOwnerCar = null,
                        numOfTowTruck = null,
                        nameOfTowCompany = null,
                        detailOfTowEvent = null,
                        numOfAccidentInjury = null,
                        detailOfAccidentInjury = null,
                        numOfDeath = null,
                        detailOfDeath = null,
                        caseOwnerCode = null,
                        caseOwnerFullName = null
                    }
                });

                Console.WriteLine("==================result======================");

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);


                Assert.AreEqual("200", result.code);
                Assert.AreEqual("Success", result.message);
                Assert.AreEqual("Registration Success", result.description);
                Assert.IsNotNull(result.data);
                Assert.IsTrue(false==string.IsNullOrEmpty(result.data.claimId));
                Assert.IsTrue(false==string.IsNullOrEmpty(result.data.claimNo));
               
            }
            catch (BuzErrorException be)
            {
                Console.WriteLine("==================result BuzErrorException======================");
                Console.WriteLine(be.GetOutputModel().ToJson());
                Assert.Fail(be.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("==================result Exception======================");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Assert.Fail(e.Message);
            }
        }
        [TestMethod()]
        public void Execute_LOCUSClaimRegistrationService_It_should_return_fail_when_give_empty_json_input()
        {
            try
            {
                var service =
                    new LOCUSClaimRegistrationService(Guid.NewGuid().ToString(), "UnitTest");
                var result = service.Execute(new LocusClaimRegistrationInputModel
                {
                    claimHeader = new LocusClaimheaderModel
                    {
                        premiumClass = "",
                        ticketNumber = "",
                        claimNotiNo = "",
                        claimNotiRefer = null,
                        policyNo = "",
                        fleetCarNo = 0,
                        policySeqNo = 0,
                        renewalNo = 0,
                        barcode = "",
                        insureCardNo = null,
                        policyIssueDate = null,
                        policyEffectiveDate = null,
                        policyExpiryDate = null,
                        policyProductTypeCode = "",
                        policyProductTypeName = "",
                        policyGarageFlag = "",
                        policyPaymentStatus = "",
                        policyCarRegisterNo = "",
                        policyCarRegisterProv = "",
                        carChassisNo = "",
                        carVehicleType = null,
                        carVehicleModel = "",
                        carVehicleYear = "",
                        carVehicleBody = "",
                        carVehicleSize = null,
                        policyDeduct = null,
                        agentCode = "",
                        agentName = "",
                        agentBranch = "",
                        vipCaseFlag = "",
                        privilegeLevel = "",
                        highLossCaseFlag = "",
                        legalCaseFlag = "",
                        claimNotiRemark = "",
                        claimType = "O",
                        informByCrmId = "",
                        informByCrmName = "",
                        submitByCrmId = "",
                        submitByCrmName = "",
                        serviceBranch = "",
                        // policyAdditionalID = null,
                        policyBranch = ""
                    },
                    claimInform = new LocusClaiminformModel
                    {
                        informerClientId = "",
                        informerFullName = "",
                        informerMobile = "",
                        informerPhoneNo = null,
                        driverClientId = "",
                        driverFullName = "",
                        driverMobile = "",
                        driverPhoneNo = null,
                        insuredClientId = "",
                        insuredFullName = "",
                        insuredMobile = null,
                        insuredPhoneNo = null,
                        relationshipWithInsurer = "",
                        currentCarRegisterNo = "",
                        currentCarRegisterProv = "",
                        informerOn = null,
                        accidentOn = null,
                        accidentDescCode = "",
                        accidentDesc = "",
                        numOfExpectInjury = 0,
                        accidentPlace = "",
                        accidentLatitude = "",
                        accidentLongitude = "",
                        accidentProvn = null,
                        accidentDist = null,
                        sendOutSurveyorCode = ""
                    },
                    claimAssignSurv = new LocusClaimassignsurvModel
                    {
                        surveyorCode = null,
                        surveyorClientNumber = null,
                        surveyorName = null,
                        surveyorCompanyName = null,
                        surveyorCompanyMobile = null,
                        surveyorMobile = null,
                        surveyorType = null,
                        surveyTeam = ""
                    },
                    claimSurvInform = new LocusClaimsurvinformModel
                    {
                        deductibleFee = null,
                        excessFee = null,
                        reportAccidentResultDate = null,
                        accidentLegalResult = null,
                        policeStation = null,
                        policeRecordId = null,
                        policeRecordDate = null,
                        policeBailFlag = "N",
                        damageOfPolicyOwnerCar = null,
                        numOfTowTruck = null,
                        nameOfTowCompany = null,
                        detailOfTowEvent = null,
                        numOfAccidentInjury = null,
                        detailOfAccidentInjury = null,
                        numOfDeath = null,
                        detailOfDeath = null,
                        caseOwnerCode = null,
                        caseOwnerFullName = null
                    }
                });

                Console.WriteLine("==================result======================");

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);


                Assert.AreEqual("400", result.code);
            }
            catch (BuzErrorException be)
            {
                Console.WriteLine("==================result BuzErrorException======================");
                Console.WriteLine(be.GetOutputModel().ToJson());
                Assert.Fail(be.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("==================result Exception======================");
                Assert.Fail(e.Message);
            }
        }


        [TestMethod()]
        public void Execute_LOCUSClaimRegistrationService_It_should_return_fail_when_give_duplicated_claimNotiNo_input()
        {
            /*Expected Result  แต่  input จะใช้ได้ถ้า เลข claimNotiNo  นี้โดนลบจาก locus ไปแล้ว
             {
              "data": {
                "claimId": null,
                "claimNo": null,
                "ticketNumber": null
              },
              "code": "500",
              "message": "claimNotiNo is duplicated",
              "description": null,
              "transactionId": "38395",
              "transactionDateTime": "2017-09-11 02:00:42"
            }
             
             */
            try
            {
                var service =
                    new LOCUSClaimRegistrationService(Guid.NewGuid().ToString(), "UnitTest");
                var result = service.Execute(new LocusClaimRegistrationInputModel
                {
                    claimHeader = new LocusClaimheaderModel
                    {
                        premiumClass = "MVS",
                        ticketNumber = "CAS201709-00018",
                        claimNotiNo = "1709-00013",
                        claimNotiRefer = null,
                        policyNo = "V0756004",
                        fleetCarNo = 1,
                        policySeqNo = 1,
                        renewalNo = 0,
                        barcode = "2080003047183",
                        insureCardNo = null,
                        policyIssueDate = DateTime.Now, //"2015-12-17 00:00:00"
                        policyEffectiveDate = DateTime.Now, //"2016-01-29 00:00:00"
                        policyExpiryDate = DateTime.Now, //"2017-10-29 00:00:00"
                        policyProductTypeCode = "FE",
                        policyProductTypeName = "ประเภท 2++",
                        policyGarageFlag = "N",
                        policyPaymentStatus = "N",
                        policyCarRegisterNo = "ษม7045",
                        policyCarRegisterProv = "กท",
                        carChassisNo = "MR053BK3005017550@",
                        carVehicleType = null,
                        carVehicleModel = "TOYOTA CAMRY",
                        carVehicleYear = "2004",
                        carVehicleBody = "SEDAN",
                        carVehicleSize = null,
                        policyDeduct = null,
                        agentCode = "90001521",
                        agentName = "หักส่วนลดหน้าตาราง",
                        agentBranch = "00",
                        vipCaseFlag = "N",
                        privilegeLevel = "1",
                        highLossCaseFlag = "N",
                        legalCaseFlag = "N",
                        claimNotiRemark ="วันที่เกิดเหตุ : 7-9-2017 เวลา 08:00 น.\n การเกิดเหตุ : คู่กรณีชนแล้วหลบหนี จำทะเบียนได้",
                        claimType = "O",
                        informByCrmId = "crmtest1",
                        informByCrmName = "crm test1",
                        submitByCrmId = "crmtest1",
                        submitByCrmName = "crm test1",
                        serviceBranch = "00",
                        policyAdditionalID = new Guid("d78d5356-f4b6-e611-80ca-0050568d1874"),
                        policyBranch = "00"
                    },
                    claimInform = new LocusClaiminformModel
                    {
                        informerClientId = "14514669",
                        informerFullName = "ธวัชชัย จันทน์แดง",
                        informerMobile = "0863156332",
                        informerPhoneNo = null,
                        driverClientId = "14514669",
                        driverFullName = "ธวัชชัย จันทน์แดง",
                        driverMobile = "0863156332",
                        driverPhoneNo = null,
                        insuredClientId = "10033581",
                        insuredFullName = "ธวัชชัย จันทน์แดง",
                        insuredMobile = null,
                        insuredPhoneNo = null,
                        relationshipWithInsurer = "00",
                        currentCarRegisterNo = "ษม7045",
                        currentCarRegisterProv = "กท",
                        informerOn = DateTime.Now , /*"2017-09-08 13:08:00"*/
                        accidentOn = DateTime.Now, /*"2017-09-07 08:00:00"*/
                        accidentDescCode = "202",
                        accidentDesc =
                            "วันที่เกิดเหตุ : 7-9-2017 เวลา 08:00 น.\n การเกิดเหตุ : คู่กรณีชนแล้วหลบหนี จำทะเบียนได้",
                        numOfExpectInjury = 0,
                        accidentPlace = "97 ถนน ดินสอ แขวง วัดบวรนิเวศ เขต พระนคร กรุงเทพมหานคร 10200 ประเทศไทย",
                        accidentLatitude = "13.757",
                        accidentLongitude = "100.502",
                        accidentProvn = null,
                        accidentDist = null,
                        sendOutSurveyorCode = "01"
                    },
                    claimAssignSurv = new LocusClaimassignsurvModel
                    {
                        surveyorCode = null,
                        surveyorClientNumber = null,
                        surveyorName = null,
                        surveyorCompanyName = null,
                        surveyorCompanyMobile = null,
                        surveyorMobile = null,
                        surveyorType = null,
                        surveyTeam = "TEAM0099"
                    },
                    claimSurvInform = new LocusClaimsurvinformModel
                    {
                        deductibleFee = null,
                        excessFee = null,
                        reportAccidentResultDate = null,
                        accidentLegalResult = null,
                        policeStation = null,
                        policeRecordId = null,
                        policeRecordDate = null,
                        policeBailFlag = "N",
                        damageOfPolicyOwnerCar = null,
                        numOfTowTruck = null,
                        nameOfTowCompany = null,
                        detailOfTowEvent = null,
                        numOfAccidentInjury = null,
                        detailOfAccidentInjury = null,
                        numOfDeath = null,
                        detailOfDeath = null,
                        caseOwnerCode = null,
                        caseOwnerFullName = null
                    }
                });

                Console.WriteLine("==================result======================");

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);

 
                Assert.AreEqual("500", result.code);
                Assert.AreEqual("claimNotiNo is duplicated", result.message);
            }
            catch (BuzErrorException be)
            {
                Console.WriteLine("==================result BuzErrorException======================");
                Console.WriteLine(be.GetOutputModel().ToJson());
                Assert.Fail(be.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("==================result Exception======================");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Assert.Fail(e.Message);
            }
        }
    }
}