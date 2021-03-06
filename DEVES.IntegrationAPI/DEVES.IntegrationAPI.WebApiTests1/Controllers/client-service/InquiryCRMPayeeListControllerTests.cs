﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class InquiryCRMPayeeListControllerTests
    {
        [TestMethod()]
        public void Post_InquiryCRMPayeeList_Test()
        {
            // Arrange
            var controller = new InquiryCRMPayeeListController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = @"
            {
              'requester': 'MC',
              'sapVendorCode': '',
              'clientType': 'P',
              'polisyClientId': '',
              'roleCode': 'G',
              'emcsCode': '',
              'taxBranchCode': '',
              'fullname': '',
              'taxNo': '88888888'
            }";

            //Assert
            var response = (HttpResponseMessage)controller.Post(JObject.Parse(input));
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);
            var outputJson = JObject.Parse(output?.Result);
            Assert.AreEqual("200", outputJson["code"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["message"]?.ToString()));

           // var outputData = outputJson["data"][0];
           // Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
           // Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));

            //ส่วนนี้ถ้าภายหลังต้นทางเปลี่ยนก็จะไม่ผ่าน
            //Assert.AreEqual("Test WTH Corp1", outputData["profileInfo"]["name1"]?.ToString());

        }

        [TestMethod()]
        public void Post_InquiryCRMPayeeList_Search_NotFound_Test()
        {
            // Arrange
            var controller = new InquiryCRMPayeeListController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = @"
            {
              'requester': 'MC',
              'sapVendorCode': '',
              'clientType': 'P',
              'polisyClientId': '',
              'roleCode': 'G',
              'emcsCode': '',
              'taxBranchCode': '',
              'fullname': '',
              'taxNo': '5450690070'
            }";

            //Assert
            var response = (HttpResponseMessage)controller.Post(JObject.Parse(input));
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);
            var outputJson = JObject.Parse(output?.Result);
            Assert.AreEqual("200", outputJson["code"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["message"]?.ToString()));

            // var outputData = outputJson["data"][0];
            // Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            // Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));

            //ส่วนนี้ถ้าภายหลังต้นทางเปลี่ยนก็จะไม่ผ่าน
            //Assert.AreEqual("Test WTH Corp1", outputData["profileInfo"]["name1"]?.ToString());

        }
    }
}