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
    public class UpdateSurveyStatusControllerTests : BaseControllersTests
    {

        [TestMethod]
        public void Post_UpdateSurveyStatusController_It_Should_Success_When_Give_Valid_Input_Test()
        {

            //input
            var jsonString = @"
                {
                  ""ticketNo"": ""CAS201709-00014"",
                  ""claimNotiNo"": ""1709-00011"",
                  ""iSurveyStatus"": ""20"",
                  ""iSurveyStatusOn"": ""2017-09-06 14:21:00""
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<UpdateSurveyStatusController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());
        }

        [TestMethod]
        public void Post_UpdateSurveyStatusController_It_Should_Fail_When_Give_NotExisting_ClaimNotiNo_Test()
        {
            //input
            var jsonString = @"
                {
                  ""ticketNo"": ""CAS201709-00014"",
                  ""claimNotiNo"": ""9999-99999"",
                  ""iSurveyStatus"": ""20"",
                  ""iSurveyStatusOn"": ""2017-09-06 14:21:00""
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<UpdateSurveyStatusController>(jsonString, "Post");
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
        public void Post_UpdateSurveyStatusController_It_Should_Fail_When_Give_InValid_Input_Test()
        {

            //input
            var jsonString = @"
                {
                  ""ticketNo"": ""CAS201709-00014"",
                  ""iSurveyStatusOn"": ""2017-09-06 14:21:00""
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<UpdateSurveyStatusController>(jsonString, "Post");
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
