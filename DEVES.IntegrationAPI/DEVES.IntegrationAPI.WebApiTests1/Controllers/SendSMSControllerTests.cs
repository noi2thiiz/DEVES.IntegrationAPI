using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.Controllers.Tests
{
    [TestClass()]
    public class SendSMSControllerTests
    {
        [TestMethod()]
        public void PostTest()
        {
            // Arrange
            var controller = new SMSController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = @"
            {
              'message': 'บริษัท เทเวศประกันภัย จำกัด (ข้อความ 1) ขอขอบพระคุณอย่างสูงที่ท่านมอบความไว้วางใจในการใช้บริการ เพื่อการปรับปรุงบริการที่ดีขึ้น บริษัทขอความกรุณาท่านโปรดตอบแบบสอบถามตามลิงก์ด้านล่างจักเป็นพระคุณยิ่ง https://crmappqa.deves.co.th/survey/?ref=A1qrMHAEQy ',
              'uid': 'crmtest1',
              'mobileNumber': '0943481249'
              
            }";

            //Assert
            var response = (HttpResponseMessage)controller.Post(JObject.Parse(input));
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);
        }

        [TestMethod()]
        public void Post_SendSMSTest()
        {
            // Arrange
            var controller = new SendSMSController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = @"
            {
              'message': 'บริษัท เทเวศประกันภัย จำกัด (ข้อความ 2) ขอขอบพระคุณอย่างสูงที่ท่านมอบความไว้วางใจในการใช้บริการ เพื่อการปรับปรุงบริการที่ดีขึ้น บริษัทขอความกรุณาท่านโปรดตอบแบบสอบถามตามลิงก์ด้านล่างจักเป็นพระคุณยิ่ง https://crmappqa.deves.co.th/survey/?ref=A1qrMHAEQy ',
              'uid': 'crmtest1',
              'mobileNumber': '0943481249'
              
            }";

            //Assert
            var response = (HttpResponseMessage)controller.Post(JObject.Parse(input));
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);
        }
    }
}