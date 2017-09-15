using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using DEVES.IntegrationAPI.Core.Util;
using DEVES.IntegrationAPI.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApiTests1.Controllers.client_service.deves_test_case
{
    /*
     Create/Inquiry ข้อมูลที่สร้างใหม่ โดยที่ยังไม่เคยมีข้อมูลลูกค้านี้ในระบบ Polisy400 และ Cleansing เลย >> จะทดสอบ Inquiry ที่ Cleansing ไม่ได้
     */
    [TestClass]
    public class NonPayeeTest1
    {

        [TestMethod]
        public void NonPayee_Integraton_Test()
        {
            Console.WriteLine("\n\n\n================================================================================================");
            Console.WriteLine("\n==============1 Create New General Corporate====================================================");
            Console.WriteLine("\n================================================================================================");
            Create_New_General_Corporate_Case1();
            System.Threading.Thread.Sleep(10000);

            Console.WriteLine("\n\n\n=================================================================================================");
            Console.WriteLine("\n==============1.1 Inquiry Corporate Client in Case #1============================================");
            Console.WriteLine("\n=================================================================================================");
            Inquiry_Fullname_Corporate_Client_in_Case_1();

            Console.WriteLine("\n\n\n=================================================================================================");
            Console.WriteLine("\n==============1.2 Inquiry Corporate Client in Case #1============================================");
            Console.WriteLine("\n=================================================================================================");
            Inquiry_TaxId_Corporate_Client_in_Case_1();
            Console.WriteLine("\n\n\n=========================================================================================");
            Console.WriteLine("\n==============1.3 Inquiry Corporate Client in Case #1========================================");
            Console.WriteLine("\n=============================================================================================");
            Inquiry_TaxId_and_TaxBranch_Corporate_Client_in_Case_1();

            Console.WriteLine("\n\n\n=========================================================================================");
            Console.WriteLine("\n==============1.4 Inquiry Corporate Client in Case #1========================================");
            Console.WriteLine("\n=============================================================================================");
            Inquiry_PolisyClientId_Corporate_Client_in_Case_1();

            Console.WriteLine("\n\n\n=========================================================================================");
            Console.WriteLine("\n==============1.5 Inquiry Corporate Client in Case #1========================================");
            Console.WriteLine("\n=============================================================================================");
            Inquiry_CleansingId_Corporate_Client_in_Case_1();

            Console.WriteLine("\n\n\n=========================================================================================");
            Console.WriteLine("\n============== 2 Create New General Corporate เปลี่ยนที่อยู่ และ TaxBranch==========================");
            Console.WriteLine("\n=============================================================================================");
            Create_New_General_Corporate_Case2();
            System.Threading.Thread.Sleep(10000);

            Console.WriteLine("\n\n\n=================================================================================================");
            Console.WriteLine("\n==============2.1 Inquiry Corporate Client in Case #1============================================");
            Console.WriteLine("\n=================================================================================================");
            Inquiry_Fullname_Corporate_Client_in_Case_2();

            Console.WriteLine("\n\n\n=================================================================================================");
            Console.WriteLine("\n==============2.2 Inquiry Corporate Client in Case #1============================================");
            Console.WriteLine("\n=================================================================================================");
            Inquiry_TaxId_Corporate_Client_in_Case_2();
            Console.WriteLine("\n\n\n=========================================================================================");
            Console.WriteLine("\n==============2.3 Inquiry Corporate Client in Case #1========================================");
            Console.WriteLine("\n=============================================================================================");
            Inquiry_TaxId_and_TaxBranch_Corporate_Client_in_Case_2();

            Console.WriteLine("\n\n\n=========================================================================================");
            Console.WriteLine("\n==============2.4 Inquiry Corporate Client in Case #1========================================");
            Console.WriteLine("\n=============================================================================================");
            Inquiry_PolisyClientId_Corporate_Client_in_Case_2();

            Console.WriteLine("\n\n\n=========================================================================================");
            Console.WriteLine("\n==============2.5 Inquiry Corporate Client in Case #1========================================");
            Console.WriteLine("\n=============================================================================================");
            Inquiry_CleansingId_Corporate_Client_in_Case_2();



            Console.WriteLine("\n\n\n=========================================================================================");
            Console.WriteLine("\n==============3 Create New General Corporate  ไม่ระบุที่อยู่ แต่ TaxBranch เดิมเหมือน Case #2 (Warning Create Duplicate) =============");
            Console.WriteLine("\n=============================================================================================");
            Create_New_General_Corporate_Case3();
            System.Threading.Thread.Sleep(10000);
           

            Console.WriteLine("\n\n\n=========================================================================================");
            Console.WriteLine("\n==============4 Create New General Corporate  Profile เดิมเหมือน Case #1 แต่เปลี่ยนที่อยู่ โดย TaxBranch เดิม (Executed Successfully) =============");
            Console.WriteLine("\n=============================================================================================");
            Create_New_General_Corporate_Case4();
            System.Threading.Thread.Sleep(10000);


            Console.WriteLine("\n\n\n=========================================================================================");
            Console.WriteLine("\n==============5 Create New General Corporate  Profile เดิมเหมือน Case #1 ทั้งหมด (Warning Create Duplicate)=============");
            Console.WriteLine("\n=============================================================================================");
            Create_New_General_Corporate_Case5();
            System.Threading.Thread.Sleep(10000);

            Console.WriteLine("\n\n\n=========================================================================================");
            Console.WriteLine("\n==============6 Create New General Corporate  Profile เดิมเหมือน Case #2 ทั้งหมด (Warning Create Duplicate)=============");
            Console.WriteLine("\n=============================================================================================");
            Create_New_General_Corporate_Case6();
            System.Threading.Thread.Sleep(10000);


            Console.WriteLine("\n\n\n=========================================================================================");
            Console.WriteLine("\n==============7 Create New General Corporate  Profile เดิมเหมือน Case #2 ทั้งหมด แต่เปลี่ยนที่อยู่โดยไม่ระบุ 'หมู่ที่' และ 'ถ.' (Warning Create Duplicate)=============");
            Console.WriteLine("\n=============================================================================================");
            Create_New_General_Corporate_Case7();
            System.Threading.Thread.Sleep(10000);

        }


        private JObject inputCase1;
        private string inputStringCase1;
        private JToken  ouputCase1;
 
        public void Create_New_General_Corporate_Case1()
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
                            'corporateBranch': '00000',
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
            inputStringCase1 = input;
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

            ouputCase1 = outputData;
            inputCase1 = JObject.Parse(input);
        }

    
        public void  Inquiry_Fullname_Corporate_Client_in_Case_1()
        {
          
            var fullName = ouputCase1["corporateName1"]?.ToString()+" "+ ouputCase1["corporateName2"]?.ToString();
            var polisyClientId = ouputCase1["polisyClientId"]?.ToString();
            var cleansingId = ouputCase1["cleansingId"]?.ToString();

              // Arrange
            var controller = new CRMInquiryClientMasterController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
        
            string input = String.Format(@"{{
                      'conditionDetail': {{
                        'cleansingId': '',
                        'polisyClientId': '',
                        'crmClientId': '',
                        'clientName1': '',
                        'clientName2': '',
                        'clientFullname': '{0}',
                        'idCard': '',
                        'corporateBranch': '',
                        'emcsCode': ''
                      }},
                      'conditionHeader': {{
                        'clientType': 'C',
                        'roleCode': 'G'
                      }}
                    }}",
                fullName
                );
            Console.WriteLine("==============input==================");
            Console.WriteLine(input);

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

            Assert.AreEqual(1, outputJson["data"].Count(), "Return Client Profile in Case #1 เท่านั้น (1 รายการ)");

            var outputData = outputJson["data"][0];
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));
            Assert.AreEqual(polisyClientId, outputData["generalHeader"]["polisyClientId"]?.ToString());
            Assert.AreEqual(cleansingId,outputData["generalHeader"]["cleansingId"]?.ToString());



        }

        public void Inquiry_TaxId_Corporate_Client_in_Case_1()
        {
          
            var IdCard = inputCase1["profileHeader"]["idRegCorp"]?.ToString();
            var polisyClientId = ouputCase1["polisyClientId"]?.ToString();
            var cleansingId = ouputCase1["cleansingId"]?.ToString();

            // Arrange
            var controller = new CRMInquiryClientMasterController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act

            string input = String.Format(@"{{
                      'conditionDetail': {{
                        'cleansingId': '',
                        'polisyClientId': '',
                        'crmClientId': '',
                        'clientName1': '',
                        'clientName2': '',
                        'clientFullname': '',
                        'idCard': '{0}',
                        'corporateBranch': '',
                        'emcsCode': ''
                      }},
                      'conditionHeader': {{
                        'clientType': 'C',
                        'roleCode': 'G'
                      }}
                    }}",
                IdCard
                );
            Console.WriteLine("==============input==================");
            Console.WriteLine(input);

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


            
            Assert.AreEqual(1, outputJson["data"].Count(), "Return Client Profile in Case #1 เท่านั้น (1 รายการ)");

            var outputData = outputJson["data"][0];
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));
            Assert.AreEqual(polisyClientId,outputData["generalHeader"]["polisyClientId"]?.ToString());
            Assert.AreEqual(cleansingId, outputData["generalHeader"]["cleansingId"]?.ToString());



        }

        public void Inquiry_TaxId_and_TaxBranch_Corporate_Client_in_Case_1()
        {

            var IdCard = inputCase1["profileHeader"]["idRegCorp"]?.ToString();
            var corporateBranch = ouputCase1["corporateBranch"]?.ToString();
            var polisyClientId = ouputCase1["polisyClientId"]?.ToString();
            var cleansingId = ouputCase1["cleansingId"]?.ToString();

            // Arrange
            var controller = new CRMInquiryClientMasterController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = String.Format(@"{{
                      'conditionDetail': {{
                        'cleansingId': '',
                        'polisyClientId': '',
                        'crmClientId': '',
                        'clientName1': '',
                        'clientName2': '',
                        'clientFullname': '',
                        'idCard': '{0}',
                        'corporateBranch': '{1}',
                        'emcsCode': ''
                      }},
                      'conditionHeader': {{
                        'clientType': 'C',
                        'roleCode': 'G'
                      }}
                    }}",
                IdCard, corporateBranch
                );
            Console.WriteLine("==============input==================");
            Console.WriteLine(input);

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



            Assert.AreEqual(1, outputJson["data"].Count(), "Return Client Profile in Case #1 เท่านั้น (1 รายการ)");

            var outputData = outputJson["data"][0];
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));
            Assert.AreEqual(polisyClientId, outputData["generalHeader"]["polisyClientId"]?.ToString());
            Assert.AreEqual(cleansingId, outputData["generalHeader"]["cleansingId"]?.ToString());



        }

        public void Inquiry_PolisyClientId_Corporate_Client_in_Case_1()
        {

            var IdCard = inputCase1["profileHeader"]["idRegCorp"]?.ToString();
            var corporateBranch = ouputCase1["corporateBranch"]?.ToString();
            var polisyClientId = ouputCase1["polisyClientId"]?.ToString();
            var cleansingId = ouputCase1["cleansingId"]?.ToString();

            // Arrange
            var controller = new CRMInquiryClientMasterController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = String.Format(@"{{
                      'conditionDetail': {{
                        'cleansingId': '',
                        'polisyClientId': '{0}',
                        'crmClientId': '',
                        'clientName1': '',
                        'clientName2': '',
                        'clientFullname': '',
                        'idCard': '',
                        'corporateBranch': '',
                        'emcsCode': ''
                      }},
                      'conditionHeader': {{
                        'clientType': 'C',
                        'roleCode': 'G'
                      }}
                    }}",
                polisyClientId
                );
            Console.WriteLine("==============input==================");
            Console.WriteLine(input);

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



            Assert.AreEqual(1, outputJson["data"].Count(), "Return Client Profile in Case #1 เท่านั้น (1 รายการ)");

            var outputData = outputJson["data"][0];
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));
            Assert.AreEqual(polisyClientId, outputData["generalHeader"]["polisyClientId"]?.ToString());
            Assert.AreEqual(cleansingId, outputData["generalHeader"]["cleansingId"]?.ToString());



        }

        public void Inquiry_CleansingId_Corporate_Client_in_Case_1()
        {

            var IdCard = inputCase1["profileHeader"]["idRegCorp"]?.ToString();
            var corporateBranch = ouputCase1["corporateBranch"]?.ToString();
            var polisyClientId = ouputCase1["polisyClientId"]?.ToString();
            var cleansingId = ouputCase1["cleansingId"]?.ToString();

            // Arrange
            var controller = new CRMInquiryClientMasterController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = String.Format(@"{{
                      'conditionDetail': {{
                        'cleansingId': '{0}',
                        'polisyClientId': '',
                        'crmClientId': '',
                        'clientName1': '',
                        'clientName2': '',
                        'clientFullname': '',
                        'idCard': '',
                        'corporateBranch': '',
                        'emcsCode': ''
                      }},
                      'conditionHeader': {{
                        'clientType': 'C',
                        'roleCode': 'G'
                      }}
                    }}",
                cleansingId
                );
            Console.WriteLine("==============input==================");
            Console.WriteLine(input);

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



            Assert.AreEqual(1, outputJson["data"].Count(), "Return Client Profile in Case #1 เท่านั้น (1 รายการ)");

            var outputData = outputJson["data"][0];
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));
            Assert.AreEqual(polisyClientId, outputData["generalHeader"]["polisyClientId"]?.ToString());
            Assert.AreEqual(cleansingId, outputData["generalHeader"]["cleansingId"]?.ToString());



        }



        private JObject inputCase2;
        private string inputStringCase2;
        private JToken ouputCase2;

        public void Create_New_General_Corporate_Case2()
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
                            'address2': 'ถ.พระรามที่ 2',
                            'address3': '',
                            'subDistrictCode': '103503',
                            'districtCode': '1035',
                            'provinceCode': '10',
                            'postalCode': '10150',
                            'country': '00220',
                            'addressType': '02',
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
                            'corporateBranch': '00000',
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
            inputStringCase2 = input;
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

            ouputCase2 = outputData;
            inputCase2 = JObject.Parse(input);
        }



        public void Inquiry_Fullname_Corporate_Client_in_Case_2()
        {

            var fullName = ouputCase2["corporateName1"]?.ToString() + " " + ouputCase2["corporateName2"]?.ToString();
            var polisyClientId = ouputCase2["polisyClientId"]?.ToString();
            var cleansingId = ouputCase2["cleansingId"]?.ToString();

            // Arrange
            var controller = new CRMInquiryClientMasterController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act

            string input = String.Format(@"{{
                      'conditionDetail': {{
                        'cleansingId': '',
                        'polisyClientId': '',
                        'crmClientId': '',
                        'clientName1': '',
                        'clientName2': '',
                        'clientFullname': '{0}',
                        'idCard': '',
                        'corporateBranch': '',
                        'emcsCode': ''
                      }},
                      'conditionHeader': {{
                        'clientType': 'C',
                        'roleCode': 'G'
                      }}
                    }}",
                fullName
                );
            Console.WriteLine("==============input==================");
            Console.WriteLine(input);

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

            Assert.AreEqual(1, outputJson["data"].Count(), "Return Client Profile in Case #1 เท่านั้น (1 รายการ)");

            var outputData = outputJson["data"][0];
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));
            Assert.AreEqual(polisyClientId, outputData["generalHeader"]["polisyClientId"]?.ToString());
            Assert.AreEqual(cleansingId, outputData["generalHeader"]["cleansingId"]?.ToString());



        }

        public void Inquiry_TaxId_Corporate_Client_in_Case_2()
        {

            var IdCard = inputCase2["profileHeader"]["idRegCorp"]?.ToString();
            var polisyClientId = ouputCase2["polisyClientId"]?.ToString();
            var cleansingId = ouputCase2["cleansingId"]?.ToString();

            // Arrange
            var controller = new CRMInquiryClientMasterController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act

            string input = String.Format(@"{{
                      'conditionDetail': {{
                        'cleansingId': '',
                        'polisyClientId': '',
                        'crmClientId': '',
                        'clientName1': '',
                        'clientName2': '',
                        'clientFullname': '',
                        'idCard': '{0}',
                        'corporateBranch': '',
                        'emcsCode': ''
                      }},
                      'conditionHeader': {{
                        'clientType': 'C',
                        'roleCode': 'G'
                      }}
                    }}",
                IdCard
                );
            Console.WriteLine("==============input==================");
            Console.WriteLine(input);

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



            Assert.AreEqual(1, outputJson["data"].Count(), "Return Client Profile in Case #1 เท่านั้น (1 รายการ)");

            var outputData = outputJson["data"][0];
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));
            Assert.AreEqual(polisyClientId, outputData["generalHeader"]["polisyClientId"]?.ToString());
            Assert.AreEqual(cleansingId, outputData["generalHeader"]["cleansingId"]?.ToString());



        }

        public void Inquiry_TaxId_and_TaxBranch_Corporate_Client_in_Case_2()
        {

            var IdCard = inputCase2["profileHeader"]["idRegCorp"]?.ToString();
            var corporateBranch = ouputCase2["corporateBranch"]?.ToString();
            var polisyClientId = ouputCase2["polisyClientId"]?.ToString();
            var cleansingId = ouputCase2["cleansingId"]?.ToString();

            // Arrange
            var controller = new CRMInquiryClientMasterController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = String.Format(@"{{
                      'conditionDetail': {{
                        'cleansingId': '',
                        'polisyClientId': '',
                        'crmClientId': '',
                        'clientName1': '',
                        'clientName2': '',
                        'clientFullname': '',
                        'idCard': '{0}',
                        'corporateBranch': '{1}',
                        'emcsCode': ''
                      }},
                      'conditionHeader': {{
                        'clientType': 'C',
                        'roleCode': 'G'
                      }}
                    }}",
                IdCard, corporateBranch
                );
            Console.WriteLine("==============input==================");
            Console.WriteLine(input);

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



            Assert.AreEqual(1, outputJson["data"].Count(), "Return Client Profile in Case #1 เท่านั้น (1 รายการ)");

            var outputData = outputJson["data"][0];
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));
            Assert.AreEqual(polisyClientId, outputData["generalHeader"]["polisyClientId"]?.ToString());
            Assert.AreEqual(cleansingId, outputData["generalHeader"]["cleansingId"]?.ToString());



        }

        public void Inquiry_PolisyClientId_Corporate_Client_in_Case_2()
        {

            var IdCard = inputCase2["profileHeader"]["idRegCorp"]?.ToString();
            var corporateBranch = ouputCase2["corporateBranch"]?.ToString();
            var polisyClientId = ouputCase2["polisyClientId"]?.ToString();
            var cleansingId = ouputCase2["cleansingId"]?.ToString();

            // Arrange
            var controller = new CRMInquiryClientMasterController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = String.Format(@"{{
                      'conditionDetail': {{
                        'cleansingId': '',
                        'polisyClientId': '{0}',
                        'crmClientId': '',
                        'clientName1': '',
                        'clientName2': '',
                        'clientFullname': '',
                        'idCard': '',
                        'corporateBranch': '',
                        'emcsCode': ''
                      }},
                      'conditionHeader': {{
                        'clientType': 'C',
                        'roleCode': 'G'
                      }}
                    }}",
                polisyClientId
                );
            Console.WriteLine("==============input==================");
            Console.WriteLine(input);

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



            Assert.AreEqual(1, outputJson["data"].Count(), "Return Client Profile in Case #1 เท่านั้น (1 รายการ)");

            var outputData = outputJson["data"][0];
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));
            Assert.AreEqual(polisyClientId, outputData["generalHeader"]["polisyClientId"]?.ToString());
            Assert.AreEqual(cleansingId, outputData["generalHeader"]["cleansingId"]?.ToString());



        }

        public void Inquiry_CleansingId_Corporate_Client_in_Case_2()
        {

            var IdCard = inputCase2["profileHeader"]["idRegCorp"]?.ToString();
            var corporateBranch = ouputCase2["corporateBranch"]?.ToString();
            var polisyClientId = ouputCase2["polisyClientId"]?.ToString();
            var cleansingId = ouputCase2["cleansingId"]?.ToString();

            // Arrange
            var controller = new CRMInquiryClientMasterController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = String.Format(@"{{
                      'conditionDetail': {{
                        'cleansingId': '{0}',
                        'polisyClientId': '',
                        'crmClientId': '',
                        'clientName1': '',
                        'clientName2': '',
                        'clientFullname': '',
                        'idCard': '',
                        'corporateBranch': '',
                        'emcsCode': ''
                      }},
                      'conditionHeader': {{
                        'clientType': 'C',
                        'roleCode': 'G'
                      }}
                    }}",
                cleansingId
                );
            Console.WriteLine("==============input==================");
            Console.WriteLine(input);

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



            Assert.AreEqual(1, outputJson["data"].Count(), "Return Client Profile in Case #1 เท่านั้น (1 รายการ)");

            var outputData = outputJson["data"][0];
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["polisyClientId"]?.ToString()));
            Assert.IsFalse(string.IsNullOrEmpty(outputData["generalHeader"]["cleansingId"]?.ToString()));
            Assert.AreEqual(polisyClientId, outputData["generalHeader"]["polisyClientId"]?.ToString());
            Assert.AreEqual(cleansingId, outputData["generalHeader"]["cleansingId"]?.ToString());



        }


        public void Create_New_General_Corporate_Case3()
        {

            // Arrange
            var controller = new RegClientCorporateController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();


            var input3 = inputCase1;
            input3["addressHeader"]["address1"] = "";
            input3["addressHeader"]["address2"] = "";
            input3["addressHeader"]["address3"] = "";
            input3["addressHeader"]["subDistrictCode"] = "";
            input3["addressHeader"]["districtCode"] = "";
            input3["addressHeader"]["provinceCode"] = "";

           

            Console.WriteLine("==============input==================");
            Console.WriteLine(input3.ToString());

            //Assert
            var response = (HttpResponseMessage)controller.Post(input3);
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
           


        }

        public void Create_New_General_Corporate_Case4()
        {

            // Arrange
            var controller = new RegClientCorporateController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var input4 = inputCase1;
            input4["addressHeader"]["address1"] = "84/3 หมู่ 2";
            input4["addressHeader"]["address2"] = "ถ.พระรามที่ 2";
            input4["addressHeader"]["address3"] = "";
            input4["addressHeader"]["subDistrictCode"] = "103503";
            input4["addressHeader"]["districtCode"] = "1035";
            input4["addressHeader"]["provinceCode"] = "10";
            input4["addressHeader"]["country"] = "00220";
           
            //Assert
            var response = (HttpResponseMessage)controller.Post(input4);
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);
            var outputJson = JObject.Parse(output?.Result);
            Assert.AreEqual("200", outputJson["code"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["message"]?.ToString()));

            //success
            Assert.IsFalse((outputJson["message"]?.ToString() ?? "").Contains("Create data duplicate"));


        }

        public void Create_New_General_Corporate_Case5()
        {

            // Arrange
            var controller = new RegClientCorporateController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();


            //Assert
            var response = (HttpResponseMessage)controller.Post(JObject.Parse(inputStringCase1));
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);
            var outputJson = JObject.Parse(output?.Result);
            Assert.AreEqual("500", outputJson["code"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["message"]?.ToString()));

            //Create data duplicate
            Assert.IsTrue( (outputJson["message"]?.ToString()??"").Contains("Create data duplicate")  );
           

        }

        public void Create_New_General_Corporate_Case6()
        {

            // Arrange
            var controller = new RegClientCorporateController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();


            //Assert
            var response = (HttpResponseMessage)controller.Post(JObject.Parse(inputStringCase2));
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);
            var outputJson = JObject.Parse(output?.Result);
            Assert.AreEqual("500", outputJson["code"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["message"]?.ToString()));

            //Create data duplicate
            Assert.IsTrue((outputJson["message"]?.ToString() ?? "").Contains("Create data duplicate"));


        }


        public void Create_New_General_Corporate_Case7()
        {

            // Arrange
            var controller = new RegClientCorporateController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var input4 = inputCase1;
            input4["addressHeader"]["address1"] = "84/3";
            input4["addressHeader"]["address2"] = "";
            input4["addressHeader"]["address3"] = "";
       

            //Assert
            var response = (HttpResponseMessage)controller.Post(input4);
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);
            var outputJson = JObject.Parse(output?.Result);
            Assert.AreEqual("500", outputJson["code"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["message"]?.ToString()));

            //Create data duplicate
            Assert.IsTrue((outputJson["message"]?.ToString() ?? "").Contains("Create data duplicate"));


        }
    }
}
