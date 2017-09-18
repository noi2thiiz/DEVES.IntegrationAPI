using DEVES.IntegrationAPI.WebApi.Controllers;
using DEVES.IntegrationAPI.WebApi.Controllers.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApiTests1.Controllers.claim_service
{
    [TestClass]
    public class ClaimRegistrationControllerTests : BaseControllersTests
    {

        public void Post_ClaimRegistrationController_It_Should_Pass()
        {
            //input
            var jsonString = @"
                {  
                   ""incidentId"":""99999999-9999-9999-9999-999999999999"",
                   ""currentUserId"":""50529F88-2BE9-E611-80D4-0050568D1874""
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<ClaimRegistrationController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);
            // Assert Return code 200
            Assert.IsNotNull(outputJson["claimID"]?.ToString());
            Assert.IsNotNull(outputJson["claimNo"]?.ToString());
            Assert.IsNull(outputJson["errorMessage"]?.ToString());
        }

        [TestMethod]
        public void Post_ClaimRegistrationController_It_Should_Fail_When_Give_Duplicate_Input_Test()
        {
            //input
            var jsonString = @"
                {  
                   ""incidentId"":""DC8C0ADA-4A91-E711-80DA-0050568D615F"",
                   ""currentUserId"":""50529F88-2BE9-E611-80D4-0050568D1874""
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<ClaimRegistrationController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);
            Assert.AreEqual("claimNotiNo is duplicated", outputJson["errorMessage"]?.ToString());
        }
    }
}
