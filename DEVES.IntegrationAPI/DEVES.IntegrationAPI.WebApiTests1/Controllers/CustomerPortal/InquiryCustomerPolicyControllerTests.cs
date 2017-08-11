using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.Controllers.Tests
{
    [TestClass()]
    public class InquiryCustomerPolicyControllerTests : TestControllerBase
    {
        /**
         * ทดสอบกรณีที่ ค้นแล้วพบข้อมูล ด้วย cleansingId
         */
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

            var output = PostMessage("InquiryCustomerPolicy", input);
            Console.WriteLine(output);

            //Assert
            var outputJson = JObject.Parse(output);
            Assert.AreEqual("200", outputJson["code"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["message"]?.ToString()));




            // ทดสอบว่า data มีค่า
        
            Assert.IsNotNull(outputJson["data"],"data is null");

            // ทดสอบว่า data รายการแรก มี ค่าที่เป็น Mandatory  
            Assert.IsNotNull(outputJson["crmPolicyDetailId"], "Policy Detail Id is null");
            Assert.IsNotNull(outputJson["policyNo"], "Policy Number Is null");
            
            // var outputData = outputJson["data"][0];
           // Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            // Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));



        }



        /**
      * ทดสอบกรณีที่ ค้นแล้วพบข้อมูล ด้วย crmClientId 
      * 
      */
        [TestMethod()]
        public void Post_InquiryCustomerPolicy_It_Should_Success_When_Give_Valid_crmClientIdTest()
        {

            string input = @"
            {
              'generalHeader': {
                'requester': 'WEB'
              },
              'conditions': {
                'cleansingId': '',
                'crmClientId': '16893273',
                'policyCarRegisterNo': '',
                'policyNo': '',
                'chassisNo': ''
              }
            }";

            var output = PostMessage("InquiryCustomerPolicy", input);
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
            Assert.IsNotNull(outputJson["crmPolicyDetailId"], "Policy Detail Id is null");
            Assert.IsNotNull(outputJson["policyNo"], "Policy Number Is null");

            // var outputData = outputJson["data"][0];
            // Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            // Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));



        }

        /**
         * ทดสอบกรณีที่ ใส่ json  input ไม่ครบ ที่ไม่มีอยู่จริง
         * ให้ทดสอบโดยไม่ระบุ  cleansingId
         */
        [TestMethod()]
        public void Post_InquiryCustomerPolicy_It_Should_Invalid_Input_Test()
        {
            string input = @"
            {
              'generalHeader': {
                'requester': 'WEB'
              },
              'conditions': {
                'cleansingId': '',
                'crmClientId': '',
                'policyCarRegisterNo': '',
                'policyNo': '',
                'chassisNo': ''
              }
            }";

            var output = PostMessage("InquiryCustomerPolicy", input);
            Console.WriteLine(output);

            //Assert
            var outputJson = JObject.Parse(output);
            Assert.AreEqual("400", outputJson["code"]?.ToString());
            //Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            //Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            //Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["message"]?.ToString()));
           
        }

        /**
         * ทดสอบกรณีที่ ระบุ cleansingId ที่ไม่มีอยู่จริง
         */
        [TestMethod()]
        public void Post_InquiryCustomerPolicy_It_Should_NotFound_When_Give_Invalid_CleansingId_Test()
        {
            string input = @"
            {
              'generalHeader': {
                'requester': 'WEB'
              },
              'conditions': {
                'cleansingId': '12345',
                'crmClientId': '',
                'policyCarRegisterNo': '',
                'policyNo': '',
                'chassisNo': ''
              }
            }";

            var output = PostMessage("InquiryCustomerPolicy", input);
            Console.WriteLine(output);

            //Assert
            var outputJson = JObject.Parse(output);
            Assert.AreEqual("200", outputJson["code"]?.ToString());
            //Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            //Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            //Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["message"]?.ToString()));
            Assert.IsNull(outputJson["data"], "data is not null");
        }

        /**
         * ทดสอบกรณีที่ ระบุ crmClientId ที่ไม่มีอยู่จริง
         */
        [TestMethod()]
        public void Post_InquiryCustomerPolicy_It_Should_NotFound_When_Give_Invalid_crmClientId_Test()
        {
            string input = @"
            {
              'generalHeader': {
                'requester': 'WEB'
              },
              'conditions': {
                'cleansingId': '',
                'crmClientId': '12345',
                'policyCarRegisterNo': '',
                'policyNo': '',
                'chassisNo': ''
              }
            }";

            var output = PostMessage("InquiryCustomerPolicy", input);
            Console.WriteLine(output);

            //Assert
            var outputJson = JObject.Parse(output);
            Assert.AreEqual("200", outputJson["code"]?.ToString());
            Assert.IsNull(outputJson["data"], "data is not null");
            //Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            //Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            //Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["message"]?.ToString()));
        }
    }
}