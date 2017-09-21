using System;
using System.Net.Http;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.Controllers.Tests
{
    [TestClass]
    public class UpdateClaimInfoControllerTests : BaseControllersTests 
    {
        [TestMethod]
        public void Post_UpdateClaimInfoController_It_Should_Success_When_Give_Valid_Input_Test()
        {
  
            //input
            var jsonString = @"
                {
                  ""claimNo"": ""V0533401"",
                  ""highLossCaseFlag"": ""N"",
                  ""ticketNo"": ""CAS201709-00014"",
                  ""claimNotiNo"": ""1709-00011"",
                  ""legalCaseFlag"": ""N"",
                  ""vipCaseFlag"": ""N"",
                  ""claimStatusCode"": ""4"",
                  ""claimStatusDesc"": ""อนุมัติแล้วปรับยอดแล้ว (Adjusted)""
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<UpdateClaimInfoController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());
            // ควรตรวจสอบค่าอื่นๆ 
        }

        [TestMethod]
        public void Post_UpdateClaimInfoController_It_Should_Fail_When_Give_InValid_Input_Test()
        {

            //input
            var jsonString = @"
                {
                  ""claimNo"": """",
                  ""highLossCaseFlag"": ""N"",
                  ""ticketNo"": """",
                  ""claimNotiNo"": ""1709-00011"",
                  ""legalCaseFlag"": ""N"",
                  ""vipCaseFlag"": ""N"",
                  ""claimStatusCode"": ""4"",
                  ""claimStatusDesc"": """"
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<UpdateClaimInfoController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 400
            Assert.AreEqual("400", outputJson["code"]?.ToString());
        }



        [TestMethod]
        public void Post_UpdateClaimInfoController_It_Should_Fail_When_Give_NotExisting_ClaimNotiNo_Test()
        {
    

            //input
            var jsonString = @"
                {
                  ""claimNo"": ""V0533401"",
                  ""highLossCaseFlag"": ""N"",
                  ""ticketNo"": ""CAS201709-00014"",
                  ""claimNotiNo"": ""9999-99999"",
                  ""legalCaseFlag"": ""N"",
                  ""vipCaseFlag"": ""N"",
                  ""claimStatusCode"": ""4"",
                  ""claimStatusDesc"": ""อนุมัติแล้วปรับยอดแล้ว (Adjusted)""
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<UpdateClaimInfoController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 500
            Assert.AreEqual("500", outputJson["code"]?.ToString());
            Assert.AreEqual("claimNotiNo หรือ ticketNo ไม่มีในระบบ CRM", outputJson["description"]?.ToString());
        }
    }
}