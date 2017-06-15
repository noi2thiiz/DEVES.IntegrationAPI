using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DEVES.IntegrationAPI.Core.Util;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.Controllers.Tests
{
    [TestClass()]
    public class CRMInquiryClientMasterControllerTests
    {
        [TestMethod()]
        public void Post_CRMInquiryClientMaster_SearchBy_idCard_clientType_P_Test()
        {
            // Arrange
            var controller = new CRMInquiryClientMasterController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            

            string input = @"
            {
              'conditionDetail': {
                'cleansingId': '',
                'polisyClientId': '',
                'crmClientId': '',
                'clientName1': '',
                'clientName2': '',
                'clientFullname': '',
                'idCard': '3670500343046',
                'corporateBranch': '',
                'emcsCode': ''
              },
              'conditionHeader': {
                'clientType': 'P',
                'roleCode': 'G'
              }
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

            var outputData = outputJson["data"][0];
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));

            //ส่วนนี้ถ้าภายหลังต้นทางเปลี่ยนก็จะไม่ผ่าน
            Assert.AreEqual("วีนา", outputData["profileInfo"]["name1"]?.ToString());
           // Assert.AreEqual("<(ยกเลิกการใช้) ภู่แส>", outputData["profileInfo"]["name2"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["addressInfo"]["address"]?.ToString()));
            Assert.AreEqual("Thailand", outputData["addressInfo"]["countryText"]?.ToString());
            Assert.AreEqual("ที่อยู่ในสัญญา", outputData["addressInfo"]["addressTypeText"]?.ToString());

            
        }

        [TestMethod()]
        public void Post_CRMInquiryClientMaster_SearchBy_polisyClientId_clientType_P_Test()
        {
            // Arrange
            var controller = new CRMInquiryClientMasterController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = @"
            {
              'conditionDetail': {
                'cleansingId': '',
                'polisyClientId': '10000408',
                'crmClientId': '',
                'clientName1': '',
                'clientName2': '',
                'clientFullname': '',
                'idCard': '',
                'corporateBranch': '',
                'emcsCode': ''
              },
              'conditionHeader': {
                'clientType': 'P',
                'roleCode': 'G'
              }
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

            var outputData = outputJson["data"][0];
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));

            //ส่วนนี้ถ้าภายหลังต้นทางเปลี่ยนก็จะไม่ผ่าน
            Assert.AreEqual("วีนา", outputData["profileInfo"]["name1"]?.ToString());
            Assert.AreEqual("ภู่แส", outputData["profileInfo"]["name2"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["addressInfo"]["address"]?.ToString()));
            Assert.AreEqual("Thailand", outputData["addressInfo"]["countryText"]?.ToString());
            Assert.AreEqual("ที่อยู่ในสัญญา", outputData["addressInfo"]["addressTypeText"]?.ToString());

        }

        [TestMethod()]
        public void Post_CRMInquiryClientMaster_SearchBy_clientFullname_clientType_P_Test()
        {
            // Arrange
            var controller = new CRMInquiryClientMasterController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = @"
            {
              'conditionDetail': {
                'cleansingId': '',
                'polisyClientId': '',
                'crmClientId': '',
                'clientName1': '',
                'clientName2': '',
                'clientFullname': 'วีนา ภู่แส',
                'idCard': '',
                'corporateBranch': '',
                'emcsCode': ''
              },
              'conditionHeader': {
                'clientType': 'P',
                'roleCode': 'G'
              }
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

            var outputData = outputJson["data"][0];
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));

            //ส่วนนี้ถ้าภายหลังต้นทางเปลี่ยนก็จะไม่ผ่าน
            Assert.AreEqual("วีนา", outputData["profileInfo"]["name1"]?.ToString());
            Assert.AreEqual("ภู่แส", outputData["profileInfo"]["name2"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["addressInfo"]["address"]?.ToString()));
            Assert.AreEqual("Thailand", outputData["addressInfo"]["countryText"]?.ToString());
            Assert.AreEqual("ที่อยู่ในสัญญา", outputData["addressInfo"]["addressTypeText"]?.ToString());

        }

        [TestMethod()]
        public void Post_CRMInquiryClientMaster_SearchBy_clientName1AndName2_clientType_P_Test()
        {
            // Arrange
            var controller = new CRMInquiryClientMasterController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = @"
            {
              'conditionDetail': {
                'cleansingId': '',
                'polisyClientId': '',
                'crmClientId': '',
                'clientName1': 'วีนา',
                'clientName2': 'ภู่แส',
                'clientFullname': '',
                'idCard': '',
                'corporateBranch': '',
                'emcsCode': ''
              },
              'conditionHeader': {
                'clientType': 'P',
                'roleCode': 'G'
              }
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

            var outputData = outputJson["data"][0];
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));

            //ส่วนนี้ถ้าภายหลังต้นทางเปลี่ยนก็จะไม่ผ่าน
            Assert.AreEqual("วีนา", outputData["profileInfo"]["name1"]?.ToString());
            Assert.AreEqual("ภู่แส", outputData["profileInfo"]["name2"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["addressInfo"]["address"]?.ToString()));
            Assert.AreEqual("Thailand", outputData["addressInfo"]["countryText"]?.ToString());
            Assert.AreEqual("ที่อยู่ในสัญญา", outputData["addressInfo"]["addressTypeText"]?.ToString());

        }

        [TestMethod()]
        public void Post_CRMInquiryClientMaster_SearchBy_clientFullname_clientType_C_Test()
        {
            // Arrange
            var controller = new CRMInquiryClientMasterController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = @"
            {
              'conditionDetail': {
                'cleansingId': '',
                'polisyClientId': '',
                'crmClientId': '',
                'clientName1': '',
                'clientName2': '',
                'clientFullname': 'บมจ.เทเวศประกันภัย สาขาเชียงราย',
                'idCard': '',
                'corporateBranch': '',
                'emcsCode': ''
              },
              'conditionHeader': {
                'clientType': 'C',
                'roleCode': 'G'
              }
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

            var outputData = outputJson["data"][0];
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));

            //ส่วนนี้ถ้าภายหลังต้นทางเปลี่ยนก็จะไม่ผ่าน
            Assert.AreEqual("บมจ.เทเวศประกันภัย สาขาเชียงราย", outputData["profileInfo"]["name2"]?.ToString());
           

        }

        [TestMethod()]
        public void Post_CRMInquiryClientMaster_SearchBy_polisyClientId_clientType_C_Test()
        {
            // Arrange
            var controller = new CRMInquiryClientMasterController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = @"
            {
              'conditionDetail': {
                'cleansingId': '',
                'polisyClientId': '17243075',
                'crmClientId': '',
                'clientName1': '',
                'clientName2': '',
                'clientFullname': '',
                'idCard': '',
                'corporateBranch': '',
                'emcsCode': ''
              },
              'conditionHeader': {
                'clientType': 'C',
                'roleCode': 'G'
              }
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

            var outputData = outputJson["data"][0];
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));

            //ส่วนนี้ถ้าภายหลังต้นทางเปลี่ยนก็จะไม่ผ่าน
            Assert.AreEqual("17243075", outputData["generalHeader"]["polisyClientId"]?.ToString());


        }

        [TestMethod()]
        public void Post_CRMInquiryClientMaster_SearchBy_idCard_clientType_C_Test()
        {
            // Arrange
            var controller = new CRMInquiryClientMasterController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = @"
            {
              'conditionDetail': {
                'cleansingId': '',
                'polisyClientId': '',
                'crmClientId': '',
                'clientName1': '',
                'clientName2': '',
                'clientFullname': '',
                'idCard': '0205558018320',
                'corporateBranch': '',
                'emcsCode': ''
              },
              'conditionHeader': {
                'clientType': 'C',
                'roleCode': 'G'
              }
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

            var outputData = outputJson["data"][0];
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));

            //ส่วนนี้ถ้าภายหลังต้นทางเปลี่ยนก็จะไม่ผ่าน
            Assert.AreEqual("2 ชุดแล้ว ห้ามส่งดร้าฟ คุยให้จบก่อน", outputData["profileInfo"]["name1"]?.ToString());


        }

    }


}