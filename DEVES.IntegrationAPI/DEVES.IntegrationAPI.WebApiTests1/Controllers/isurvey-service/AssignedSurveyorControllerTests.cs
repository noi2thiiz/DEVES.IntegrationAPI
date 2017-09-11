using DEVES.IntegrationAPI.WebApi.Controllers;
using DEVES.IntegrationAPI.WebApi.Controllers.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApiTests1.Controllers.isurvey_service
{
    [TestClass]
    public class AssignedSurveyorControllerTests : BaseControllersTests
    {

        [TestMethod]
        public void Post_AssignedSurveyorController_It_Should_Success_When_Give_Valid_Input_Test()
        {
            //input
            var jsonString = @"
                {
                   ""ticketNo"":""CAS201709-00005"",
                   ""claimNotiNo"":""1709-00004"",
                   ""iSurveyStatusOn"":""15"",
                   ""surveyMeetingLatitude"":""13.906943"",
                   ""surveyMeetingLongtitude"":""100.516936"",
                   ""surveyMeetingDistrict"":"""",
                   ""surveyMeetingProvince"":"""",
                   ""surveyMeetingPlace"":""ถนน เลี่ยงเมืองปากเกร็ด เทศบาลนครปากเกร็ด นนทบุรี ประเทศไทย"",
                   ""surveyMeetingDate"":""2017-07-18 15:46:00"",
                   ""surveyorCode"":""O-0015"",
                   ""surveyorClientNumber"":""10000324"",
                   ""surveyorName"":""ณัฐพล 083 611 5885 "",
                   ""surveyorCompanyName"":""บริษัท เซ็นเตอร์เคลม จำกัด (หลักสี่)"",
                   ""surveyorCompanyMobile"":""02-690-6782,02-691-3336"",
                   ""surveyType"":""1"",
                   ""surveyTeam"":""TEAM0004""
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<AssignedSurveyorController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);
            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());
        }

        [TestMethod]
        public void Post_AssignedSurveyorController_It_Should_Fail_When_Give_NotExisting_ClaimNotiNo_Test()
        {
            //input
            var jsonString = @"
                {
                   ""ticketNo"":""CAS201709-00005"",
                   ""claimNotiNo"":""9999-99999"",
                   ""iSurveyStatusOn"":""15"",
                   ""surveyMeetingLatitude"":""13.906943"",
                   ""surveyMeetingLongtitude"":""100.516936"",
                   ""surveyMeetingDistrict"":"""",
                   ""surveyMeetingProvince"":"""",
                   ""surveyMeetingPlace"":""ถนน เลี่ยงเมืองปากเกร็ด เทศบาลนครปากเกร็ด นนทบุรี ประเทศไทย"",
                   ""surveyMeetingDate"":""2017-07-18 15:46:00"",
                   ""surveyorCode"":""O-0015"",
                   ""surveyorClientNumber"":""10000324"",
                   ""surveyorName"":""ณัฐพล 083 611 5885 "",
                   ""surveyorCompanyName"":""บริษัท เซ็นเตอร์เคลม จำกัด (หลักสี่)"",
                   ""surveyorCompanyMobile"":""02-690-6782,02-691-3336"",
                   ""surveyType"":""1"",
                   ""surveyTeam"":""TEAM0004""
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<AssignedSurveyorController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);
            // Assert Return code 200
            Assert.AreEqual("500", outputJson["code"]?.ToString());
            Assert.AreEqual("claimNotiNo ไม่มีในระบบ CRM", outputJson["description"]?.ToString());

        }

        [TestMethod]
        public void Post_AssignedSurveyorController_It_Should_Fail_When_Give_InValid_Input_Test()
        {

            //input
            var jsonString = @"
                {
                  ""claimNo"": """",
                  ""highLossCaseFlag"": ""N"",
                  ""legalCaseFlag"": ""N"",
                  ""vipCaseFlag"": ""N"",
                  ""claimStatusCode"": ""4"",
                  ""claimStatusDesc"": """"
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<AssignedSurveyorController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 400
            Assert.AreEqual("400", outputJson["code"]?.ToString());
        }

    }
}
