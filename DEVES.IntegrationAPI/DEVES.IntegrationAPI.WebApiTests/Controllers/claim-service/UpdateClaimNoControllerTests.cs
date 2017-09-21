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
    public class UpdateClaimNoControllerTests : BaseControllersTests
    {
        [TestMethod]
        public void Post_UpdateClaimNoController_It_Should_Success_When_Give_Valid_Input_Test()
        {
            //input
            var jsonString = @"
                {
                  ""claimNo"": ""V0533404"",
                  ""claimId"": ""1395"",
                  ""highLossCaseFlag"": ""N"",
                  ""ticketNo"": ""CAS201709-00014"",
                  ""claimNotiNo"": ""1709-00011"",
                  ""legalCaseFlag"": ""N"",
                  ""vipCaseFlag"": ""N"",
                  ""claimStatusCode"": ""4"",
                  ""claimStatusDesc"": ""อนุมัติแล้วปรับยอดแล้ว (Adjusted.)""
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<UpdateClaimNoController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);
            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());
        }

        [TestMethod]
        public void Post_UpdateClaimNoController_It_Should_Fail_When_Give_NotExisting_ClaimNotiNo_Test()
        {
            //input
            var jsonString = @"
                {
                  ""claimNo"": ""V0533404"",
                  ""claimId"": ""1395"",
                  ""highLossCaseFlag"": ""N"",
                  ""ticketNo"": ""CAS999999-99999"",
                  ""claimNotiNo"": ""1709-00011"",
                  ""legalCaseFlag"": ""N"",
                  ""vipCaseFlag"": ""N"",
                  ""claimStatusCode"": ""4"",
                  ""claimStatusDesc"": ""อนุมัติแล้วปรับยอดแล้ว (Adjusted.)""
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<UpdateClaimNoController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);
            // Assert Return code 200
            Assert.AreEqual("500", outputJson["code"]?.ToString());
            Assert.AreEqual("claimNotiNo ไม่มีในระบบ CRM", outputJson["description"]?.ToString());

        }

        /*
        [TestMethod]
        public void Post_UpdateClaimNoController_It_Should_Fail_When_Give_NotExisting_ClaimNotiNo_And_TicketNo_Test()
        {
            //input
            var jsonString = @"
                {
                  ""claimNo"": ""V0533404"",
                  ""claimId"": ""1395"",
                  ""highLossCaseFlag"": ""N"",
                  ""ticketNo"": ""CAS999999-99999"",
                  ""claimNotiNo"": ""9999-99999"",
                  ""legalCaseFlag"": ""N"",
                  ""vipCaseFlag"": ""N"",
                  ""claimStatusCode"": ""4"",
                  ""claimStatusDesc"": ""อนุมัติแล้วปรับยอดแล้ว (Adjusted.)""
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<UpdateClaimNoController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);
            // Assert Return code 200
            Assert.AreEqual("500", outputJson["code"]?.ToString());
            Assert.AreEqual("TicketNumber และ claimNotiNo ไม่มีในระบบ CRM", outputJson["description"]?.ToString());

        }
        */

        [TestMethod]
        public void Post_UpdateClaimNoController_It_Should_Fail_When_Give_InValid_Input_Test()
        {

            //input
            var jsonString = @"
                {
                  ""claimNo"": """",
                  ""highLossCaseFlag"": ""N"",
                  ""claimNotiNo"": ""1709-00011"",
                  ""legalCaseFlag"": ""N"",
                  ""vipCaseFlag"": ""N"",
                  ""claimStatusCode"": ""4"",
                  ""claimStatusDesc"": """"
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<UpdateClaimNoController>(jsonString, "Post");
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
