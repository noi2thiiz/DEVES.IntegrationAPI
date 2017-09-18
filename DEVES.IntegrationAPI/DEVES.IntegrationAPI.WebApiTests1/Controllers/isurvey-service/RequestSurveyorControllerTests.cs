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
    public class RequestSurveyorControllerTests : BaseControllersTests
    {
        [TestMethod]
        public void Post_RequestSurveyorController_It_Should_Fail_When_Give_Duplicate_Input_Test()
        {
            //input
            var jsonString = @"
                {
                  ""incidentId"": ""567808D0-036E-E711-80DA-0050568D615F"",
                  ""currentUserId"": ""50529F88-2BE9-E611-80D4-0050568D1874""
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RequestSurveyorController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);
            // Assert Return code 200
            //Assert.AreEqual("ส่งข้อมูลเข้า i-Survey ไม่สำเร็จ: เลขรับแจ้งมีอยู่แล้วในระบบ", outputJson["errorMessage"]?.ToString());
            Assert.AreEqual("MOTOR Error:เลขรับแจ้งมีอยู่แล้วในระบบ", outputJson["errorMessage"]?.ToString());
            //MOTOR Error:เลขรับแจ้งมีอยู่แล้วในระบบ
        }

    }
}
