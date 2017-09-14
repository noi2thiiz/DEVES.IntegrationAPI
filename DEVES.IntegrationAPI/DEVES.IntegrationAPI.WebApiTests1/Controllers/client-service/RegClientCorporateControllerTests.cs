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
using DEVES.IntegrationAPI.WebApi.Logic.Services.Tests;
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

        [TestMethod(), Priority(1)]
        public void Post_RegClientCorporate_CG01_Test()
        {
            /*Description
             * ข้อมูลลูกค้าใหม่ที่ยังไม่มีใน <<Cleansing>> และ <<Polisy400>>

           *Input
           * roleCode = G	
           * cleansingId=     null	
           * polisyClientId=  null	
           * crmClientId=      null
           * 
           *Expected API Action
           * ต้องยิงไปที่
             - CLS_CreatePersonalClient
             - CLIENT_CreatePersonalClientAndAdditionalInfo
             - และสร้างข้อมูลใน CRM
           *  
           * ต้องเกิดข้อมูลใหม่ที่
              <<Cleansing>>
              <<Polisy400>>
              <<CRM>>

           */

            /* Expected Output
             * 
             {
              "data": [
                {
                  "cleansingId": "C2017-100003034",
                  "polisyClientId": "16973429",
                  "crmClientId": "201709-0000916",
                  "corporateName1": "ทดสอบB4XUSVDAWS",
                  "corporateName2": "ทดสอบZPIEWBL2ST",
                  "corporateBranch": "0000"
                }
              ],
              "code": "200",
              "message": "Success",
              "description": "",
              "transactionId": "00-600914160112287-0000000",
              "transactionDateTime": "2017-09-14T16:01:17.969916+07:00"
            }
             */

           

            // Arrange
            var controller = new RegClientCorporateController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateBranch = RandomValueGenerator.RandomNumber(6);
            
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
                          
                          'profileHeader': {{
                            'corporateName1': '{0}',
                            'corporateName2': '{1}',
                            'contactPerson': '',
                            'idRegCorp': '{3}',
                            'idTax': '{3}',
                            'dateInCorporate': '',
                            'corporateBranch': '{2}',
                            'econActivity': '',
                            'countryOrigin': '00203',
                            'language': '{4}',
                            'riskLevel': '{5}',
                            'vipStatus': '{6}'
                          }},
                          'asrhHeader': {{
                            'assessorOregNum': '',
                            'assessorTerminateDate': '',
                            'solicitorOregNum': '',
                            'solicitorTerminateDate': '',
                            'repairerOregNum': '',
                            'repairerTerminateDate': ''
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
                        }}",
                        corporateName1,
                        corporateName2,
                        corporateBranch,
                        RandomValueGenerator.RandomNumber(13),
                        RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                        RandomValueGenerator.RandomOptions(new[] { "", "R1" }),
                        RandomValueGenerator.RandomOptions(new[] { "Y", "N" })
                        

                );

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

            //ค่าที่ต้องสร้างใหม่
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["polisyClientId"]?.ToString()), "polisyClientId should not null");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["cleansingId"]?.ToString()), "cleansingId should not null");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["crmClientId"]?.ToString()), "crmClientId should not null");

            //ค่าที่ต้อเหมือน input
            Assert.AreEqual(corporateName1, outputData["corporateName1"]?.ToString(), "corporateName1 should equal input");
            Assert.AreEqual(corporateName2, outputData["corporateName2"]?.ToString(), "corporateName2 should equal input");
            Assert.AreEqual(corporateBranch, outputData["corporateBranch"]?.ToString(), "corporateBranch should equal input");
            
        }

        [TestMethod(), Priority(2)]
        public void Post_RegClientCorporate_CG02_Test()
        {
            /*Description
             * ข้อมูลลูกค้าที่ไม่มีใน <<Cleansing>> แต่มีใน <<Polisy400>>
             * เคสนี้เป็นข้อมูลลูกค้าที่ถูกสร้างตรงๆ ที่<<Polisy400>> ซึ่งข้อมูลนี้จะมีเลข CleansingId ในวันต่อมา และแค่มีเลข polisyClientId ก็ตอบโจทย์ของ Locus ในการเปิดเคลม
             * ในอนาคตหากจะต้องสร้าง เลข CleansingId ในเคสนี้ ต้องมาคุย Flow กันอีกที
           *Input
           * roleCode = G	
           * cleansingId=     null	
           * polisyClientId=  not null	
           * crmClientId=      null
           * 
           *Expected API Action
           * ต้องยิงไปที่
             No Action
           *  
           * ต้องเกิดข้อมูลใหม่ที่
              No Action

           */

            /* Expected Output
             * 
               Return Error
               {
                  "code": "500",
                  "message": "The Service cannot process case:  'cleansingId' null and 'polisyClientId' not null",
                  "description": "",
                  "transactionId": "00-600914161759244-0000001",
                  "transactionDateTime": "2017-09-14T16:18:02.7423525+07:00",
                  "data": null
                }
             */

            //cretae polisy client id
            var ctl = new CLIENTCreateCorporateClientAndAdditionalInfoTests();
            string polisyClientId = ctl.Execute_CLIENTCreateCorporateClientAndAdditionalInfoTest();

            // Arrange
            var controller = new RegClientCorporateController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateBranch = RandomValueGenerator.RandomNumber(6);

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
                          
                          'profileHeader': {{
                            'corporateName1': '{0}',
                            'corporateName2': '{1}',
                            'contactPerson': '',
                            'idRegCorp': '{3}',
                            'idTax': '{3}',
                            'dateInCorporate': '',
                            'corporateBranch': '{2}',
                            'econActivity': '',
                            'countryOrigin': '00203',
                            'language': '{4}',
                            'riskLevel': '{5}',
                            'vipStatus': '{6}'
                          }},
                          'asrhHeader': {{
                            'assessorOregNum': '',
                            'assessorTerminateDate': '',
                            'solicitorOregNum': '',
                            'solicitorTerminateDate': '',
                            'repairerOregNum': '',
                            'repairerTerminateDate': ''
                          }},
                          'generalHeader': {{
                            'roleCode': 'G',
                            'cleansingId': '',
                            'polisyClientId': '{7}',
                            'crmClientId': '',
                            'clientAdditionalExistFlag': 'N',
                            'assessorFlag': 'N',
                            'solicitorFlag': 'N',
                            'repairerFlag': 'N',
                            'hospitalFlag': 'N'
                          }}
                        }}",
                corporateName1,
                corporateName2,
                corporateBranch,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "", "R1" }),
                RandomValueGenerator.RandomOptions(new[] { "Y", "N" }),
                polisyClientId

                );

            //Assert
            var response = (HttpResponseMessage)controller.Post(JObject.Parse(input));
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);
            var outputJson = JObject.Parse(output?.Result);
            Assert.AreEqual("500", outputJson["code"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            Assert.AreEqual("The Service cannot process case:  'cleansingId' null and 'polisyClientId' not null", outputJson["message"]?.ToString());


          

        }

        [TestMethod(), Priority(3)]
        public void Post_RegClientCorporate_CG03_Test()
        {
            /*Description
             * ข้อมูลลูกค้าที่มีใน <<Cleansing>> แต่ไม่มีใน <<Polisy400>> และ <<CRM>> (เช่น ข้อมูลลูกค้าใน WorryFree)
           *Input
           * roleCode = G	
           * cleansingId=      not null	
           * polisyClientId=   null	
           * crmClientId=      null
           * 
           *Expected API Action
           * ต้องยิงไปที่
            - CLIENT_CreatePersonalClientAndAdditionalInfo
            - และสร้างข้อมูลใน CRM
           *  
           *ต้องเกิดข้อมูลใหม่ที่
            - <<Polisy400>>
            - <<CRM>>

           */

            /* Expected Output
             * 
               Return SUCCESS
               
             */

            //cretae polisy client id
            //cretae cleansingId
            var ctl = new CLSCreateCorporateClientTests();
            string cleansingId = ctl.Execute_CLSCreateGeneralCorporateClientTest();

            // Arrange
            var controller = new RegClientCorporateController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateBranch = RandomValueGenerator.RandomNumber(6);

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
                          
                          'profileHeader': {{
                            'corporateName1': '{0}',
                            'corporateName2': '{1}',
                            'contactPerson': '',
                            'idRegCorp': '{3}',
                            'idTax': '{3}',
                            'dateInCorporate': '',
                            'corporateBranch': '{2}',
                            'econActivity': '',
                            'countryOrigin': '00203',
                            'language': '{4}',
                            'riskLevel': '{5}',
                            'vipStatus': '{6}'
                          }},
                          'asrhHeader': {{
                            'assessorOregNum': '',
                            'assessorTerminateDate': '',
                            'solicitorOregNum': '',
                            'solicitorTerminateDate': '',
                            'repairerOregNum': '',
                            'repairerTerminateDate': ''
                          }},
                          'generalHeader': {{
                            'roleCode': 'G',
                            'cleansingId': '{7}',
                            'polisyClientId': '',
                            'crmClientId': '',
                            'clientAdditionalExistFlag': 'N',
                            'assessorFlag': 'N',
                            'solicitorFlag': 'N',
                            'repairerFlag': 'N',
                            'hospitalFlag': 'N'
                          }}
                        }}",
                corporateName1,
                corporateName2,
                corporateBranch,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "", "R1" }),
                RandomValueGenerator.RandomOptions(new[] { "Y", "N" }),
                cleansingId

                );

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
           

            var outputData = outputJson["data"][0];

            //ค่าที่ต้องสร้างใหม่
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["polisyClientId"]?.ToString()), "polisyClientId should not null");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["cleansingId"]?.ToString()), "cleansingId should not null");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["crmClientId"]?.ToString()), "crmClientId should not null");

            //ค่าที่ต้อเหมือน input
            Assert.AreEqual(corporateName1, outputData["corporateName1"]?.ToString(), "corporateName1 should equal input");
            Assert.AreEqual(corporateName2, outputData["corporateName2"]?.ToString(), "corporateName2 should equal input");
            Assert.AreEqual(corporateBranch, outputData["corporateBranch"]?.ToString(), "corporateBranch should equal input");

            Assert.AreEqual(cleansingId, outputData["cleansingId"]?.ToString(), "cleansingId should equal input");


        }

        [TestMethod(), Priority(4)]
        public void Post_RegClientCorporate_CG04_Test()
        {
            /*Description
             * ข้อมูลลูกค้าที่มีใน <<Cleansing>> และ <<CRM>>
             * แต่ไม่มีใน <<Polisy400>> 
             * (เช่น ข้อมูลลูกค้าใน WorryFree)
           *Input
           * roleCode = G	
           * cleansingId=      not null	
           * polisyClientId=   null	
           * crmClientId=      not null
           * 
           *Expected API Action
           * ต้องยิงไปที่
            - CLIENT_CreatePersonalClientAndAdditionalInfo

           *  
           *ต้องเกิดข้อมูลใหม่ที่
            - <<Polisy400>>

           */

            /* Expected Output
             * 
               Return SUCCESS
               
             */

            //cretae polisy client id
            //cretae cleansingId
            var goutput = CreateGeneralClientCorporate_NotCreatePolisyClient();
            string cleansingId = goutput["cleansingId"]?.ToString();
            string crmClientId = goutput["crmClientId"]?.ToString();

            // Arrange
            var controller = new RegClientCorporateController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateBranch = RandomValueGenerator.RandomNumber(6);

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
                          
                          'profileHeader': {{
                            'corporateName1': '{0}',
                            'corporateName2': '{1}',
                            'contactPerson': '',
                            'idRegCorp': '{3}',
                            'idTax': '{3}',
                            'dateInCorporate': '',
                            'corporateBranch': '{2}',
                            'econActivity': '',
                            'countryOrigin': '00203',
                            'language': '{4}',
                            'riskLevel': '{5}',
                            'vipStatus': '{6}'
                          }},
                          'asrhHeader': {{
                            'assessorOregNum': '',
                            'assessorTerminateDate': '',
                            'solicitorOregNum': '',
                            'solicitorTerminateDate': '',
                            'repairerOregNum': '',
                            'repairerTerminateDate': ''
                          }},
                          'generalHeader': {{
                            'roleCode': 'G',
                            'cleansingId': '{7}',
                            'polisyClientId': '',
                            'crmClientId': '{8}',
                            'clientAdditionalExistFlag': 'N',
                            'assessorFlag': 'N',
                            'solicitorFlag': 'N',
                            'repairerFlag': 'N',
                            'hospitalFlag': 'N'
                          }}
                        }}",
                corporateName1,
                corporateName2,
                corporateBranch,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "", "R1" }),
                RandomValueGenerator.RandomOptions(new[] { "Y", "N" }),
                cleansingId,
                crmClientId

                );

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


            var outputData = outputJson["data"][0];

            //ค่าที่ต้องสร้างใหม่
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["polisyClientId"]?.ToString()), "polisyClientId should not null");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["cleansingId"]?.ToString()), "cleansingId should not null");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["crmClientId"]?.ToString()), "crmClientId should not null");

            //ค่าที่ต้อเหมือน input
            Assert.AreEqual(corporateName1, outputData["corporateName1"]?.ToString(), "corporateName1 should equal input");
            Assert.AreEqual(corporateName2, outputData["corporateName2"]?.ToString(), "corporateName2 should equal input");
            Assert.AreEqual(corporateBranch, outputData["corporateBranch"]?.ToString(), "corporateBranch should equal input");

            Assert.AreEqual(cleansingId, outputData["cleansingId"]?.ToString(), "cleansingId should equal input");
            Assert.AreEqual(crmClientId, outputData["crmClientId"]?.ToString(), "crmClientId should equal input");


        }

        [TestMethod(), Priority(5)]
        public void Post_RegClientCorporate_CG05_Test()
        {
            /*Description
             * ข้อมูลลูกค้าที่มีทั้งใน <<Cleansing>> และ <<Polisy400>>
             * เหตุการณ์นี้ไม่ควรส่งมาเพราะ มี Process การ Inquiry อยู่
           *Input
           * roleCode = G	
           * cleansingId=      not null	
           * polisyClientId=   not null	
           * crmClientId=      null
           * 
           *Expected API Action
           * ต้องยิงไปที่
            - CLIENT_CreatePersonalClientAndAdditionalInfo

           *  
           *ต้องเกิดข้อมูลใหม่ที่
            - <<Polisy400>>

           */

            /* Expected Output
             * 
               {
                  "code": "500",
                  "message": "The Service cannot process case:  'cleansingId' not null and 'polisyClientId' not null",
                  "description": "",
                  "transactionId": "00-600914164929717-0000001",
                  "transactionDateTime": "2017-09-14T16:49:29.7194113+07:00",
                  "data": null
                }
               
             */

            //cretae polisy client id
            //cretae cleansingId
            var goutput = CreateGeneralClientCorporate_NotCreateCrmClient();
            string cleansingId = goutput["cleansingId"]?.ToString();
            string polisyClientId = goutput["polisyClientId"]?.ToString();

            // Arrange
            var controller = new RegClientCorporateController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateBranch = RandomValueGenerator.RandomNumber(6);

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
                          
                          'profileHeader': {{
                            'corporateName1': '{0}',
                            'corporateName2': '{1}',
                            'contactPerson': '',
                            'idRegCorp': '{3}',
                            'idTax': '{3}',
                            'dateInCorporate': '',
                            'corporateBranch': '{2}',
                            'econActivity': '',
                            'countryOrigin': '00203',
                            'language': '{4}',
                            'riskLevel': '{5}',
                            'vipStatus': '{6}'
                          }},
                          'asrhHeader': {{
                            'assessorOregNum': '',
                            'assessorTerminateDate': '',
                            'solicitorOregNum': '',
                            'solicitorTerminateDate': '',
                            'repairerOregNum': '',
                            'repairerTerminateDate': ''
                          }},
                          'generalHeader': {{
                            'roleCode': 'G',
                            'cleansingId': '{7}',
                            'polisyClientId': '{8}',
                            'crmClientId': '',
                            'clientAdditionalExistFlag': 'N',
                            'assessorFlag': 'N',
                            'solicitorFlag': 'N',
                            'repairerFlag': 'N',
                            'hospitalFlag': 'N'
                          }}
                        }}",
                corporateName1,
                corporateName2,
                corporateBranch,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "", "R1" }),
                RandomValueGenerator.RandomOptions(new[] { "Y", "N" }),
                cleansingId,
                polisyClientId

                );

            //Assert
            var response = (HttpResponseMessage)controller.Post(JObject.Parse(input));
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);
            var outputJson = JObject.Parse(output?.Result);
            Assert.AreEqual("500", outputJson["code"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            Assert.AreEqual("The Service cannot process case:  'cleansingId' not null and 'polisyClientId' not null", outputJson["message"]?.ToString());


        }

        [TestMethod(), Priority(5)]
        public void Post_RegClientCorporate_CG06_Test()
        {
            /*Description
             * ข้อมูลลูกค้าที่มีทั้งใน <<Cleansing>> และ <<Polisy400>>
             * เหตุการณ์นี้ไม่ควรส่งมาเพราะ มี Process การ Inquiry อยู่
           *Input
           * roleCode = G	
           * cleansingId=      not null	
           * polisyClientId=   not null	
           * crmClientId=      not null
           * 
           *Expected API Action
           * ต้องยิงไปที่
            - CLIENT_CreatePersonalClientAndAdditionalInfo

           *  
           *ต้องเกิดข้อมูลใหม่ที่
            - <<Polisy400>>

           */

            /* Expected Output
             * 
              {
                  "code": "500",
                  "message": "The Service cannot process case:  'cleansingId' not null and 'polisyClientId' not null",
                  "description": "",
                  "transactionId": "00-600914164929717-0000001",
                  "transactionDateTime": "2017-09-14T16:49:29.7194113+07:00",
                  "data": null
                }
               
             */

            //cretae polisy client id
            //cretae cleansingId
            var goutput = CreateGeneralClientCorporate();
            string cleansingId = goutput["cleansingId"]?.ToString();
            string polisyClientId = goutput["polisyClientId"]?.ToString();
            string crmClientId = goutput["crmClientId"]?.ToString();

            // Arrange
            var controller = new RegClientCorporateController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateBranch = RandomValueGenerator.RandomNumber(6);

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
                          
                          'profileHeader': {{
                            'corporateName1': '{0}',
                            'corporateName2': '{1}',
                            'contactPerson': '',
                            'idRegCorp': '{3}',
                            'idTax': '{3}',
                            'dateInCorporate': '',
                            'corporateBranch': '{2}',
                            'econActivity': '',
                            'countryOrigin': '00203',
                            'language': '{4}',
                            'riskLevel': '{5}',
                            'vipStatus': '{6}'
                          }},
                          'asrhHeader': {{
                            'assessorOregNum': '',
                            'assessorTerminateDate': '',
                            'solicitorOregNum': '',
                            'solicitorTerminateDate': '',
                            'repairerOregNum': '',
                            'repairerTerminateDate': ''
                          }},
                          'generalHeader': {{
                            'roleCode': 'G',
                            'cleansingId': '{7}',
                            'polisyClientId': '{8}',
                            'crmClientId': '{9}',
                            'clientAdditionalExistFlag': 'N',
                            'assessorFlag': 'N',
                            'solicitorFlag': 'N',
                            'repairerFlag': 'N',
                            'hospitalFlag': 'N'
                          }}
                        }}",
                corporateName1,
                corporateName2,
                corporateBranch,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "", "R1" }),
                RandomValueGenerator.RandomOptions(new[] { "Y", "N" }),
                cleansingId,
                polisyClientId,
                crmClientId

                );

            //Assert
            var response = (HttpResponseMessage)controller.Post(JObject.Parse(input));
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);
            var outputJson = JObject.Parse(output?.Result);
            Assert.AreEqual("500", outputJson["code"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            Assert.AreEqual("The Service cannot process case:  'cleansingId' not null and 'polisyClientId' not null", outputJson["message"]?.ToString());


        }
    }
}