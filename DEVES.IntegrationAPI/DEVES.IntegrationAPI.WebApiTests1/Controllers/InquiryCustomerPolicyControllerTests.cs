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
        public void Post_InquiryCustomerCase_It_Should_Success_When_Give_Valid_CleansingIdTest()
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

            var output = PostMessage("InquiryCustomerCase", input);
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

            // var outputData = outputJson["data"][0];
           // Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            // Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));



        }



        /**
      * ทดสอบกรณีที่ ค้นแล้วพบข้อมูล ด้วย crmClientId
      */
        [TestMethod()]
        public void Post_InquiryCustomerCase_It_Should_Success_When_Give_Valid_crmClientIdTest()
        {
        }

        /**
         * ทดสอบกรณีที่ ใส่ json  input ไม่ครบ ที่ไม่มีอยู่จริง
         * ให้ทดสอบโดยไม่ระบุ  cleansingId
         */
        [TestMethod()]
        public void Post_InquiryCustomerCase_It_Should_Invalid_Input_Test()
        {
        }

        /**
         * ทดสอบกรณีที่ ระบุ cleansingId ที่ไม่มีอยู่จริง
         */
        [TestMethod()]
        public void Post_InquiryCustomerCase_It_Should_NotFound_When_Give_Invalid_CleansingId_Test()
        {
        }

        /**
         * ทดสอบกรณีที่ ระบุ crmClientId ที่ไม่มีอยู่จริง
         */
        [TestMethod()]
        public void Post_InquiryCustomerCase_It_Should_NotFound_When_Give_Invalid_crmClientId_Test()
        {
        }
    }
}