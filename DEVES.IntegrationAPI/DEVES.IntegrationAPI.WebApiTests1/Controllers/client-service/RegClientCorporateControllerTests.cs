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
    public class RegClientCorporateControllerTests
    {
  
        [TestMethod()]
        public void Post_CreateRegClientCorporate_Create_GeneralClient_Test()
        {

            CreateGeneralClientCorporate();


        }

        public JToken CreateGeneralClientCorporate()
        {

            // Arrange
            var controller = new RegClientCorporateController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);

            string input = String.Format(@"{{
                          'addressHeader': {{
                            'address1': '84/3 หมู่ 2',
                            'address2': 'ถ.ปู่เจ้าสมิงพราย',
                            'address3': '',
                            'subDistrictCode': '110407',
                            'districtCode': '1104',
                            'provinceCode': '11',
                            'postalCode': '10130',
                            'country': '00220',
                            'addressType': '03',
                            'latitude': '',
                            'longitude': ''
                          }},
                          'generalHeader': {{
                            'roleCode': 'G',
                            'cleansingId': '',
                            'polisyClientId': '',
                            'crmClientId': '',
                            'clientAdditionalExistFlag': 'N',
                            'assessorFlag': 'N',
                            'solicitorFlag': 'N',
                            'repairerFlag': 'N',
                            'hospitalFlag': 'N'
                          }},
                          'profileHeader': {{
                            'corporateName1': '{0}',
                            'corporateName2': '{1}',
                            'contactPerson': '',
                            'idRegCorp': '{2}',
                            'idTax': '{2}',
                            'dateInCorporate': '',
                            'corporateBranch': '0000',
                            'econActivity': '',
                            'countryOrigin': '00203',
                            'language': '{3}',
                            'riskLevel': '{4}',
                            'vipStatus': '{5}'
                          }},
                          'asrhHeader': {{
                            'assessorOregNum': '',
                            'assessorTerminateDate': '',
                            'solicitorOregNum': '',
                            'solicitorTerminateDate': '',
                            'repairerOregNum': '',
                            'repairerTerminateDate': ''
                          }}
                        }}",
                corporateName1,
                corporateName2,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "", "R1" }),
                RandomValueGenerator.RandomOptions(new[] { "Y", "N" }));

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
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["polisyClientId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["cleansingId"]?.ToString()));
            Assert.AreEqual(corporateName1, outputData["corporateName1"]?.ToString());
            Assert.AreEqual(corporateName2, outputData["corporateName2"]?.ToString());


            return outputData;

        }
        [TestMethod()]
        public void Post_CreateRegClientCorporate_Create_GeneralClient_NotCreatePolisyClient_Test()
        {

            CreateGeneralClientCorporate_NotCreatePolisyClient();


        }
        [TestMethod()]
        public void Post_CreateRegClientCorporate_Create_GeneralClient_NotCreateCrmClient_Test()
        {

            CreateGeneralClientCorporate_NotCreateCrmClient();


        }


        public JToken CreateGeneralClientCorporate_NotCreatePolisyClient()
        {

            // Arrange
            var controller = new RegClientCorporateController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);

            string input = String.Format(@"{{
                          'addressHeader': {{
                            'address1': '84/3 หมู่ 2',
                            'address2': 'ถ.ปู่เจ้าสมิงพราย',
                            'address3': '',
                            'subDistrictCode': '110407',
                            'districtCode': '1104',
                            'provinceCode': '11',
                            'postalCode': '10130',
                            'country': '00220',
                            'addressType': '03',
                            'latitude': '',
                            'longitude': ''
                          }},
                          'generalHeader': {{
                            'roleCode': 'G',
                            'cleansingId': '',
                            'polisyClientId': '',
                            'crmClientId': '',
                            'clientAdditionalExistFlag': 'N',
                            'assessorFlag': 'N',
                            'solicitorFlag': 'N',
                            'repairerFlag': 'N',
                            'hospitalFlag': 'N',
                            'notCreatePolisyClientFlag':'Y'
                          }},
                          'profileHeader': {{
                            'corporateName1': '{0}',
                            'corporateName2': '{1}',
                            'contactPerson': '',
                            'idRegCorp': '{2}',
                            'idTax': '{2}',
                            'dateInCorporate': '',
                            'corporateBranch': '0000',
                            'econActivity': '',
                            'countryOrigin': '00203',
                            'language': '{3}',
                            'riskLevel': '{4}',
                            'vipStatus': '{5}'
                          }},
                          'asrhHeader': {{
                            'assessorOregNum': '',
                            'assessorTerminateDate': '',
                            'solicitorOregNum': '',
                            'solicitorTerminateDate': '',
                            'repairerOregNum': '',
                            'repairerTerminateDate': ''
                          }}
                        }}",
                corporateName1,
                corporateName2,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "", "R1", "R2" }),
                RandomValueGenerator.RandomOptions(new[] { "Y", "N" }));

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
            Assert.AreEqual(true, string.IsNullOrEmpty(outputData["polisyClientId"]?.ToString()), "ต้องไม่สร้าง polisyClientId");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["cleansingId"]?.ToString()));
            Assert.AreEqual(corporateName1, outputData["corporateName1"]?.ToString());
            Assert.AreEqual(corporateName2, outputData["corporateName2"]?.ToString());


            return outputData;


        }

        public JToken CreateGeneralClientCorporate_NotCreateCrmClient()
        {

            // Arrange
            var controller = new RegClientCorporateController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);

            string input = String.Format(@"{{
                          'addressHeader': {{
                            'address1': '84/3 หมู่ 2',
                            'address2': 'ถ.ปู่เจ้าสมิงพราย',
                            'address3': '',
                            'subDistrictCode': '110407',
                            'districtCode': '1104',
                            'provinceCode': '11',
                            'postalCode': '10130',
                            'country': '00220',
                            'addressType': '03',
                            'latitude': '',
                            'longitude': ''
                          }},
                          'generalHeader': {{
                            'roleCode': 'G',
                            'cleansingId': '',
                            'polisyClientId': '',
                            'crmClientId': '',
                            'clientAdditionalExistFlag': 'N',
                            'assessorFlag': 'N',
                            'solicitorFlag': 'N',
                            'repairerFlag': 'N',
                            'hospitalFlag': 'N',
                            'notCreateCrmClientFlag':'Y'
                          }},
                          'profileHeader': {{
                            'corporateName1': '{0}',
                            'corporateName2': '{1}',
                            'contactPerson': '',
                            'idRegCorp': '{2}',
                            'idTax': '{2}',
                            'dateInCorporate': '',
                            'corporateBranch': '0000',
                            'econActivity': '',
                            'countryOrigin': '00203',
                            'language': '{3}',
                            'riskLevel': '{4}',
                            'vipStatus': '{5}'
                          }},
                          'asrhHeader': {{
                            'assessorOregNum': '',
                            'assessorTerminateDate': '',
                            'solicitorOregNum': '',
                            'solicitorTerminateDate': '',
                            'repairerOregNum': '',
                            'repairerTerminateDate': ''
                          }}
                        }}",
                corporateName1,
                corporateName2,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "", "R1", "R2" }),
                RandomValueGenerator.RandomOptions(new[] { "Y", "N" }));

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
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["polisyClientId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["cleansingId"]?.ToString()));
            Assert.AreEqual(true, string.IsNullOrEmpty(outputData["crmClientId"]?.ToString()), "ต้องไม่สร้าง crmClientId");
            Assert.AreEqual(corporateName1, outputData["corporateName1"]?.ToString());
            Assert.AreEqual(corporateName2, outputData["corporateName2"]?.ToString());


            return outputData;


        }

    }
}