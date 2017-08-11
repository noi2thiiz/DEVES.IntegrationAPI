﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.Controllers.Tests
{
    [TestClass()]
    public class InquiryCustomerClaimControllerTests : TestControllerBase
    {
        [TestMethod()]
        public void Post_InquiryCustomerPolicy_It_Should_Success_When_Give_Valid_CleansingIdTest()
        {

            string input = @"
            {
              'generalHeader': {
                'requester': 'WEB'
              },
              'conditions': {
                'cleansingId': 'C2017-005367960',
                'crmClientId': '',
                'policyCarRegisterNo': '',
                'policyNo': '',
                'chassisNo': ''
              }
            }";

            var output = PostMessage("InquiryCustomerClaim", input);
            Console.WriteLine(output);

            //Assert
            var outputJson = JObject.Parse(output);
            Assert.AreEqual("200", outputJson["code"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["message"]?.ToString()));




            // ทดสอบว่า data มีค่า

            Assert.IsNotNull(outputJson["data"], "data is null");

            // ทดสอบว่า data รายการแรก มี ค่าที่เป็น Mandatory  
            //Assert.IsNotNull(outputJson["crmPolicyDetailId"], "Policy Detail Id is null");
            //Assert.IsNotNull(outputJson["policyNo"], "Policy Number Is null");

            // var outputData = outputJson["data"][0];
            // Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            // Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));



        }
    }
}