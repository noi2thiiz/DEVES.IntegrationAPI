using DEVES.IntegrationAPI.WebApi.Controllers;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;


namespace DEVES.IntegrationAPI.WebApi.Controllers.Tests
{
    [TestClass()]
    public class InquiryCustomerCaseControllerTests : TestControllerBase
    {
        [TestMethod()]
        public void Post_InquiryCustomerClaim_It_Should_Success_When_Give_Valid_CleansingIdTest()
        {
            string input = @"{
	                        'generalHeader' :
                            {
                                        'requester' : 'WEB'

                            },
	                        'conditions':
	                        {
                                        'cleansingId':'',
	                                    'claimNotiNo':'',
	                                    'claimNo':'',
	                                    'policyNo':'',
	                                    'policyCarRegisterNo':'',
	                                    'parentPolicyId':''
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
            Assert.IsNotNull(outputJson["policyNo"], "policy number is null");
        }

        [TestMethod()]
        public void Post_InquiryCustomerClaim_It_Should_Invalid_Input()
        {
            string input = @"{
	                        'generalHeader' :
                            {
                                        'requester' : 'WEB'

                            },
	                        'conditions':
	                        {
                                        'cleansingId':'',
	                                    'claimNotiNo':'',
	                                    'claimNo':'',
	                                    'policyNo':'',
	                                    'policyCarRegisterNo':'',
	                                    'parentPolicyId':''
                            }
                                }";
            var output = PostMessage("InquiryCustomerClaim", input);
            Console.WriteLine(output);

            //Assert
            var outputJson = JObject.Parse(output);
            Assert.AreEqual("400", outputJson["code"]?.ToString());
            //Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            //Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            //Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["message"]?.ToString()));
            // ทดสอบว่า data มีค่า
            //Assert.IsNotNull(outputJson["data"], "data is null");
            //Assert.IsNotNull(outputJson["policyNo"], "policy number is null");
        }

        [TestMethod()]
        public void PostTest()
        {
            Assert.Fail();
        }
    }
}