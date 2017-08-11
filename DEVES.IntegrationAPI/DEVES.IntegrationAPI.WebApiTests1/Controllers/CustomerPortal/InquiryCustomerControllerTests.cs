using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.Controllers.Tests
{
    [TestClass()]
    public class InquiryCustomerControllerTests : TestControllerBase
    {
        [TestMethod()]
        public void Post_InquiryCustomer_It_Should_Success_When_Give_A_Valid_cleansindIdTest()
        {
            string input = @"
            {
              'generalHeader': {
                'requester': 'WEB'
              },
              'conditions': {
                'fullName':''
                'idCard': '',
                'phoneNumber': '',
                'cleansingId': 'C2017-005367960',
                'crmClientId': '',
                'email': '',
                'customerType':''
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

            Assert.IsNotNull(outputJson["data"], "data is null");
            
            // ทดสอบว่า data รายการแรก มี ค่าที่เป็น Mandatory  
            //Assert.IsNotNull(outputJson["crmPolicyDetailId"], "Policy Detail Id is null");
            //Assert.IsNotNull(outputJson["policyNo"], "Policy Number Is null");
        }
    }
}