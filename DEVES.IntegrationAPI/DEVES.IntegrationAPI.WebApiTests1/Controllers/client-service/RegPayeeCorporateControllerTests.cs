using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Core.Util;
using DEVES.IntegrationAPI.WebApi.Logic.Services.Tests;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.Controllers.Tests
{
    [TestClass()]
    public class RegPayeeCorporateControllerTests : BaseControllersTests
    {
        [TestMethod()]
        public void Post_RegPayeeCorporateController_It_Should_Success_When_Give_Valid_Input_Test()
        {
            /* Expected Output
             * {"data":[{"cleansingId":"C2017-100002580","polisyClientId":"16973022","crmClientId":"201709-0000541",
             * "sapVendorCode":"16973022","sapVendorGroupCode":"CMIS",
             * "corporateName1":"{corporateName1}","corporateName2":"{corporateName2}",
             * "corporateBranch":"3453"}],"code":"200","message":"Success","description":"",
             * "transactionId":"00-600912155524233-0000000","transactionDateTime":"2017-09-12T15:55:24.5754392+07:00"}

             */
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idTax = "" + RandomValueGenerator.RandomNumber(13);
            string jsonInput = @"{
                                'contactHeader': {
                                'telephone1': '023345433',
                                'telephone1Ext': '',
                                'telephone2': '034433344',
                                'telephone2Ext': '',
                                'telephone3': '043322233',
                                'telephone3Ext': '',
                                'mobilePhone': '',
                                'fax': '',
                                'emailAddress': '',
                                'lineID': '',
                                'facebook': ''
                            },
                            'addressHeader': {
                                'address1': '123',
                                'address2': '',
                                'address3': '',
                                'subDistrictCode': '100701',
                                'districtCode': '1007',
                                'provinceCode': '10',
                                'postalCode': '10330',
                                'country': '00220',
                                'addressType': '03',
                                'latitude': '',
                                'longtitude': ''
                            },
                            'sapVendorInfo': {
                                'sapVendorGroupCode': 'CMIS',
                                'bankInfo': {
                                    'bankCountryCode': '',
                                    'bankCode': '002',
                                    'bankBranchCode': '0808',
                                    'bankAccount': '3334442323',
                                    'accountHolder': 'ปฏิวัติ',
                                    'paymentMethods': 'C'
                                },
                                'withHoldingTaxInfo': {
                                    'whtTaxCode': '20',
                                    'receiptType': '03'
                                }
                            },
                            'generalHeader': {
                                'roleCode': 'G',
                                'cleansingId': '',
                                'polisyClientId': '',
                                'crmClientId': '',
                                'clientAdditionalExistFlag': 'N',
                                'assessorFlag': 'N',
                                'solicitorFlag': 'N',
                                'repairerFlag': 'N',
                                'hospitalFlag': 'N'
                            },
                            'profileHeader': {
                                'corporateName1': '{corporateName1}',
                                'corporateName2': '{corporateName2}',
                                'contactPerson': 'สมปอง การช่าง',
                                'idRegCorp': '{idTax}',
                                'idTax': '{idTax}',
                                'dateInCorporate': '2017-07-08',
                                'corporateBranch': '3453',
                                'econActivity': '001',
                                'countryOrigin': '00203',
                                'language': 'E',
                                'riskLevel': 'R2',
                                'vipStatus': 'N'
                            }
                        }";
            jsonInput = jsonInput.Replace("{corporateName1}", corporateName1);
            jsonInput = jsonInput.Replace("{corporateName2}", corporateName2);
            jsonInput = jsonInput.Replace("{idTax}", idTax);


            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeeCorporateController>(jsonInput, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());
         
            // ตรวจสอบค่าอื่นๆ 
            Assert.IsNotNull(outputJson["data"][0], " data should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["cleansingId"]?.ToString()), " cleansingId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["polisyClientId"]?.ToString()), " polisyClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["crmClientId"]?.ToString()), " crmClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorGroupCode"]?.ToString()), " sapVendorGroupCode should not null");

            Assert.AreEqual(corporateName1, outputJson["data"][0]["corporateName1"]?.ToString(), " personalName should Equal input");
            Assert.AreEqual(corporateName2, outputJson["data"][0]["corporateName2"]?.ToString(), " personalSurname should Equal input");

        }

        [TestMethod()]
        public void Post_RegPayeeCorporateController_It_Should_Success_When_Give_Minimum_Requried_Input_Test()
        {
            /* Expected Output
             * {"data":[{"cleansingId":"C2017-100002580","polisyClientId":"16973022","crmClientId":"201709-0000541",
             * "sapVendorCode":"16973022","sapVendorGroupCode":"CMIS",
             * "corporateName1":"{corporateName1}","corporateName2":"{corporateName2}",
             * "corporateBranch":"3453"}],"code":"200","message":"Success","description":"",
             * "transactionId":"00-600912155524233-0000000","transactionDateTime":"2017-09-12T15:55:24.5754392+07:00"}

             */
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idTax = "" + RandomValueGenerator.RandomNumber(13);
            string jsonInput = @"{
                                'contactHeader': {
                                'telephone1': '',
                                'telephone1Ext': '',
                                'telephone2': '',
                                'telephone2Ext': '',
                                'telephone3': '',
                                'telephone3Ext': '',
                                'mobilePhone': '',
                                'fax': '',
                                'emailAddress': '',
                                'lineID': '',
                                'facebook': ''
                            },
                            'addressHeader': {
                                'address1': '123',
                                'address2': '',
                                'address3': '',
                                'subDistrictCode': '100701',
                                'districtCode': '1007',
                                'provinceCode': '10',
                                'postalCode': '10330',
                                'country': '00220',
                                'addressType': '',
                                'latitude': '',
                                'longtitude': ''
                            },
                            'sapVendorInfo': {
                                'sapVendorGroupCode': 'CMIS',
                                'bankInfo': {
                                    'bankCountryCode': '',
                                    'bankCode': '002',
                                    'bankBranchCode': '0808',
                                    'bankAccount': '3334442323',
                                    'accountHolder': 'ปฏิวัติ',
                                    'paymentMethods': 'C'
                                },
                                'withHoldingTaxInfo': {
                                    'whtTaxCode': '20',
                                    'receiptType': '03'
                                }
                            },
                            'generalHeader': {
                                'roleCode': 'G',
                                'cleansingId': '',
                                'polisyClientId': '',
                                'crmClientId': '',
                                'clientAdditionalExistFlag': 'N',
                                'assessorFlag': 'N',
                                'solicitorFlag': 'N',
                                'repairerFlag': 'N',
                                'hospitalFlag': 'N'
                            },
                            'profileHeader': {
                                'corporateName1': '{corporateName1}',
                                'corporateName2': '{corporateName2}',
                                'contactPerson': '',
                                'idRegCorp': '{idTax}',
                                'idTax': '{idTax}',
                                'dateInCorporate': '',
                                'corporateBranch': '3453',
                                'econActivity': '',
                                'countryOrigin': '',
                                'language': '',
                                'riskLevel': '',
                                'vipStatus': ''
                            }
                        }";
            jsonInput = jsonInput.Replace("{corporateName1}", corporateName1);
            jsonInput = jsonInput.Replace("{corporateName2}", corporateName2);
            jsonInput = jsonInput.Replace("{idTax}", idTax);


            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeeCorporateController>(jsonInput, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());

            // ตรวจสอบค่าอื่นๆ 
            Assert.IsNotNull(outputJson["data"][0], " data should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["cleansingId"]?.ToString()), " cleansingId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["polisyClientId"]?.ToString()), " polisyClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["crmClientId"]?.ToString()), " crmClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorGroupCode"]?.ToString()), " sapVendorGroupCode should not null");

            Assert.AreEqual(corporateName1, outputJson["data"][0]["corporateName1"]?.ToString(), " personalName should Equal input");
            Assert.AreEqual(corporateName2, outputJson["data"][0]["corporateName2"]?.ToString(), " personalSurname should Equal input");

        }


        [TestMethod()]
        public void Post_RegPayeeCorporateController_Test_Flow1_Test()
        {
            /*
             CLS_CODE = 0	
             CLNTNUM  = 0	
             SAPID = 0;

            ถือว่าไม่เคยมี Payee นี้มาก่อนจึงต้องขอสร้างทั้งใน CLS, Polisy400 และ APAR


             API Step [1]=>[2.1]=>[2.2]=>[3.1]=>[3.2]=>[4.1]=>[4.2]

             */
            /* Expected Output
             * {"data":[{"cleansingId":"C2017-100002580","polisyClientId":"16973022","crmClientId":"201709-0000541",
             * "sapVendorCode":"16973022","sapVendorGroupCode":"CMIS",
             * "corporateName1":"{corporateName1}","corporateName2":"{corporateName2}",
             * "corporateBranch":"3453"}],"code":"200","message":"Success","description":"",
             * "transactionId":"00-600912155524233-0000000","transactionDateTime":"2017-09-12T15:55:24.5754392+07:00"}

             */
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idTax = "" + RandomValueGenerator.RandomNumber(13);
            string jsonInput = @"{
                                'contactHeader': {
                                'telephone1': '',
                                'telephone1Ext': '',
                                'telephone2': '',
                                'telephone2Ext': '',
                                'telephone3': '',
                                'telephone3Ext': '',
                                'mobilePhone': '',
                                'fax': '',
                                'emailAddress': '',
                                'lineID': '',
                                'facebook': ''
                            },
                            'addressHeader': {
                                'address1': '123',
                                'address2': '',
                                'address3': '',
                                'subDistrictCode': '100701',
                                'districtCode': '1007',
                                'provinceCode': '10',
                                'postalCode': '10330',
                                'country': '00220',
                                'addressType': '',
                                'latitude': '',
                                'longtitude': ''
                            },
                            'sapVendorInfo': {
                                'sapVendorGroupCode': 'CMIS',
                                'bankInfo': {
                                    'bankCountryCode': '',
                                    'bankCode': '002',
                                    'bankBranchCode': '0808',
                                    'bankAccount': '3334442323',
                                    'accountHolder': 'ปฏิวัติ',
                                    'paymentMethods': 'C'
                                },
                                'withHoldingTaxInfo': {
                                    'whtTaxCode': '20',
                                    'receiptType': '03'
                                }
                            },
                            'generalHeader': {
                                'roleCode': 'G',
                                'cleansingId': '',
                                'polisyClientId': '',
                                'crmClientId': '',
                                'clientAdditionalExistFlag': 'N',
                                'assessorFlag': 'N',
                                'solicitorFlag': 'N',
                                'repairerFlag': 'N',
                                'hospitalFlag': 'N'
                            },
                            'profileHeader': {
                                'corporateName1': '{corporateName1}',
                                'corporateName2': '{corporateName2}',
                                'contactPerson': '',
                                'idRegCorp': '{idTax}',
                                'idTax': '{idTax}',
                                'dateInCorporate': '',
                                'corporateBranch': '3453',
                                'econActivity': '',
                                'countryOrigin': '',
                                'language': '',
                                'riskLevel': '',
                                'vipStatus': ''
                            }
                        }";
            jsonInput = jsonInput.Replace("{corporateName1}", corporateName1);
            jsonInput = jsonInput.Replace("{corporateName2}", corporateName2);
            jsonInput = jsonInput.Replace("{idTax}", idTax);


            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeeCorporateController>(jsonInput, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());

            // ตรวจสอบค่าอื่นๆ 
            Assert.IsNotNull(outputJson["data"][0], " data should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["cleansingId"]?.ToString()), " cleansingId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["polisyClientId"]?.ToString()), " polisyClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["crmClientId"]?.ToString()), " crmClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorGroupCode"]?.ToString()), " sapVendorGroupCode should not null");

            Assert.AreEqual(corporateName1, outputJson["data"][0]["corporateName1"]?.ToString(), " personalName should Equal input");
            Assert.AreEqual(corporateName2, outputJson["data"][0]["corporateName2"]?.ToString(), " personalSurname should Equal input");

        }


        [TestMethod()]
        [Priority(1)]
        public void Post_RegPayeeCorporate_CG01_Test_Test()
        {
            /*Description
              * สร้างใหม่ทั้งหมด
              *Input
              * roleCode = G	
              * cleansingId=null=null	
              * polisyClientId=null	
              * crmClientId=null
              * 
              *Expected API Action
              * ต้องยิงไปที่
                - CLS_CreatePersonalClient
                - CLIENT_CreatePersonalClientAndAdditionalInfo
                - SAP_InquiryVendor
                - SAP_CreateVendor
                - และสร้างข้อมูลใน CRM

              * ต้องเกิดข้อมูลใหม่ที่
              *  <<Cleansing>>
              *  <<Polisy400>>
              *  <<SAP>>
              *  <<CRM>>
              */


            /*
             * Expected Output 
             {"data":[{"cleansingId":"C2017-100002646","polisyClientId":"16973078","crmClientId":"201709-0000591","personalName":"ทดสอบC59PM01F4U","personalSurname":"ทดสอบ1DXVY5VYKR"}],"code":"200","message":"Success","description":"","transactionId":"00-600913150406827-0000000","transactionDateTime":"2017-09-13T15:04:24.9765239+07"}
             */
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idTax = "" + RandomValueGenerator.RandomNumber(13);
            string jsonInput = String.Format(@"{{
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
                                idTax,
                                 RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                                 RandomValueGenerator.RandomOptions(new[] { "","R1", "R2" }),
                                 RandomValueGenerator.RandomOptions(new[] { "Y", "N" }));



            // ระบุ Controller ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeeCorporateController>(jsonInput, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());

            // ตรวจสอบค่าอื่นๆ 
            Assert.IsNotNull(outputJson["data"][0], " data should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["cleansingId"]?.ToString()), " cleansingId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["polisyClientId"]?.ToString()), " polisyClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["crmClientId"]?.ToString()), " crmClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorGroupCode"]?.ToString()), " sapVendorGroupCode should not null");

            Assert.AreEqual(corporateName1, outputJson["data"][0]["corporateName1"]?.ToString(), " personalName should Equal input");
            Assert.AreEqual(corporateName2, outputJson["data"][0]["corporateName2"]?.ToString(), " personalSurname should Equal input");

        }

        [TestMethod()]
        [Priority(2)]
        public void Post_RegPayeeCorporate_CG02_Test_Test()
        {
            /*Description
              * สร้างใหม่ทั้งหมด
              *Input
              * roleCode = G	
              * cleansingId=null=null	
              * polisyClientId=no tnull	
              * crmClientId=null
              * 
              *Expected API Action
              * ต้องยิงไปที่
                - CLS_CreatePersonalClient
                - CLIENT_CreatePersonalClientAndAdditionalInfo
                - SAP_InquiryVendor
                - SAP_CreateVendor
                - และสร้างข้อมูลใน CRM

              * ต้องเกิดข้อมูลใหม่ที่
              *  <<Cleansing>>
              *  <<Polisy400>>
              *  <<SAP>>
              *  <<CRM>>
              */


            /*
             * Expected Output 
             {"data":[{"cleansingId":"C2017-100002646","polisyClientId":"16973078","crmClientId":"201709-0000591","personalName":"ทดสอบC59PM01F4U","personalSurname":"ทดสอบ1DXVY5VYKR"}],"code":"200","message":"Success","description":"","transactionId":"00-600913150406827-0000000","transactionDateTime":"2017-09-13T15:04:24.9765239+07"}
             */

            //cretae polisy client id
            var ctl = new CLIENTCreateCorporateClientAndAdditionalInfoTests();
            string polisyClientId = ctl.Execute_CLIENTCreateCorporateClientAndAdditionalInfoTest();
           


            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idTax = "" + RandomValueGenerator.RandomNumber(13);
            string jsonInput = String.Format(@"{{
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
                                    'polisyClientId': '{6}',
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
                                idTax,
                                 RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                                 RandomValueGenerator.RandomOptions(new[] { "", "R1", "R2" }),
                                 RandomValueGenerator.RandomOptions(new[] { "Y", "N" }),
                                  polisyClientId

                                 );



            // ระบุ Controller ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeeCorporateController>(jsonInput, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());

            // ตรวจสอบค่าอื่นๆ 
            Assert.IsNotNull(outputJson["data"][0], " data should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["cleansingId"]?.ToString()), " cleansingId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["polisyClientId"]?.ToString()), " polisyClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["crmClientId"]?.ToString()), " crmClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorGroupCode"]?.ToString()), " sapVendorGroupCode should not null");

            Assert.AreEqual(corporateName1, outputJson["data"][0]["corporateName1"]?.ToString(), " personalName should Equal input");
            Assert.AreEqual(corporateName2, outputJson["data"][0]["corporateName2"]?.ToString(), " personalSurname should Equal input");

        }

        [TestMethod()]
        [Priority(3)]
        public void Post_RegPayeeCorporate_CG03_Test_Test()
        {
            /*Description
              * ข้อมูลที่มีแต่ใน <<CLS>>
              *Input
              * roleCode = G	
              * cleansingId=null=not null	
              * polisyClientId=null	
              * crmClientId=null
              * 
              *Expected API Action
              * ต้องยิงไปที่
                 - CLIENT_CreatePersonalClientAndAdditionalInfo
                 - SAP_InquiryVendor
                 - SAP_CreateVendor
                 - และสร้างข้อมูลใน CRM

              * ต้องเกิดข้อมูลใหม่ที่
              *  <<Polisy400>>
              *  <<SAP>>
              *  <<CRM>>
              */


            /*
             * Expected Output 
             * cleansingId = api input นอกนั้งสร้างใหม่
             {"data":[{"cleansingId":"C2017-100002646","polisyClientId":"16973078","crmClientId":"201709-0000591","personalName":"ทดสอบC59PM01F4U","personalSurname":"ทดสอบ1DXVY5VYKR"}],"code":"200","message":"Success","description":"","transactionId":"00-600913150406827-0000000","transactionDateTime":"2017-09-13T15:04:24.9765239+07"}
             */

            //cretae cleansingId
            var ctl = new CLSCreateCorporateClientTests();
            string cleansingId = ctl.Execute_CLSCreateGeneralCorporateClientTest();


            //Test
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idTax = "" + RandomValueGenerator.RandomNumber(13);
            string jsonInput = String.Format(@"{{
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
                                    'cleansingId': '{6}',
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
                                idTax,
                                 RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                                 RandomValueGenerator.RandomOptions(new[] { "", "R1", "R2" }),
                                 RandomValueGenerator.RandomOptions(new[] { "Y", "N" }),
                                 cleansingId

                                 );



            // ระบุ Controller ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeeCorporateController>(jsonInput, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());

            // ตรวจสอบค่าอื่นๆ 
            Assert.IsNotNull(outputJson["data"][0], " data should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["cleansingId"]?.ToString()), " cleansingId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["polisyClientId"]?.ToString()), " polisyClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["crmClientId"]?.ToString()), " crmClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorGroupCode"]?.ToString()), " sapVendorGroupCode should not null");

            Assert.AreEqual(corporateName1, outputJson["data"][0]["corporateName1"]?.ToString(), " personalName should Equal input");
            Assert.AreEqual(corporateName2, outputJson["data"][0]["corporateName2"]?.ToString(), " personalSurname should Equal input");

        }

        [TestMethod()]
        [Priority(4)]
        public void Post_RegPayeeCorporate_CG04_Test_Test()
        {
            /*Description
              * ข้อมูลที่มีแต่ใน <<CLS>> และ <<CRM>>
              *Input
              * roleCode = G	
              * cleansingId=not null	
              * polisyClientId=null	
              * crmClientId=not null
              * 
              *Expected API Action
              * ต้องยิงไปที่
                  - CLIENT_CreatePersonalClientAndAdditionalInfo
                  - SAP_InquiryVendor
                  - SAP_CreateVendor

              * ต้องเกิดข้อมูลใหม่ที่
              *  <<Polisy400>>
              *  <<SAP>>
              */


            /*
             * Expected Output 
             * cleansingId = api input นอกนั้งสร้างใหม่
             {"data":[{"cleansingId":"C2017-100002646","polisyClientId":"16973078","crmClientId":"201709-0000591","personalName":"ทดสอบC59PM01F4U","personalSurname":"ทดสอบ1DXVY5VYKR"}],"code":"200","message":"Success","description":"","transactionId":"00-600913150406827-0000000","transactionDateTime":"2017-09-13T15:04:24.9765239+07"}
             */

            //cretae cleansingId
            var ctl = new CLSCreateCorporateClientTests();
            string cleansingId = ctl.Execute_CLSCreateGeneralCorporateClientTest();


            //Test
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idTax = "" + RandomValueGenerator.RandomNumber(13);
            string jsonInput = String.Format(@"{{
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
                                    'cleansingId': '{6}',
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
                                idTax,
                                 RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                                 RandomValueGenerator.RandomOptions(new[] { "", "R1", "R2" }),
                                 RandomValueGenerator.RandomOptions(new[] { "Y", "N" }),
                                 cleansingId

                                 );



            // ระบุ Controller ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeeCorporateController>(jsonInput, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());

            // ตรวจสอบค่าอื่นๆ 
            Assert.IsNotNull(outputJson["data"][0], " data should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["cleansingId"]?.ToString()), " cleansingId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["polisyClientId"]?.ToString()), " polisyClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["crmClientId"]?.ToString()), " crmClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorGroupCode"]?.ToString()), " sapVendorGroupCode should not null");

            Assert.AreEqual(corporateName1, outputJson["data"][0]["corporateName1"]?.ToString(), " personalName should Equal input");
            Assert.AreEqual(corporateName2, outputJson["data"][0]["corporateName2"]?.ToString(), " personalSurname should Equal input");

        }

        [TestMethod()]
        [Priority(5)]
        public void Post_RegPayeeCorporate_CG05_Test_Test()
        {
            /*Description
              * ข้อมูลที่มีแต่ใน <<CLS>> และ <<POLISY400>>
              *Input
              * roleCode = G	
              * cleansingId=null=not null	
              * polisyClientId  =not null	
              * crmClientId     =null
              * 
              *Expected API Action
              * ต้องยิงไปที่
                   - SAP_InquiryVendor
                   - SAP_CreateVendor
                   - และสร้างข้อมูลใน CRM

              * ต้องเกิดข้อมูลใหม่ที่
              *  <<SAP>>
              *  <<CRM>>
              */


            /*
             * Expected Output 
             * cleansingId = api input นอกนั้งสร้างใหม่
             {"data":[{"cleansingId":"C2017-100002646","polisyClientId":"16973078","crmClientId":"201709-0000591","personalName":"ทดสอบC59PM01F4U","personalSurname":"ทดสอบ1DXVY5VYKR"}],"code":"200","message":"Success","description":"","transactionId":"00-600913150406827-0000000","transactionDateTime":"2017-09-13T15:04:24.9765239+07"}
             */

            //cretae cleansingId
            var ctl = new CLSCreateCorporateClientTests();
            string cleansingId = ctl.Execute_CLSCreateGeneralCorporateClientTest();


            //Test
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idTax = "" + RandomValueGenerator.RandomNumber(13);
            string jsonInput = String.Format(@"{{
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
                                    'cleansingId': '{6}',
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
                                idTax,
                                 RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                                 RandomValueGenerator.RandomOptions(new[] { "", "R1", "R2" }),
                                 RandomValueGenerator.RandomOptions(new[] { "Y", "N" }),
                                 cleansingId

                                 );



            // ระบุ Controller ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeeCorporateController>(jsonInput, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());

            // ตรวจสอบค่าอื่นๆ 
            Assert.IsNotNull(outputJson["data"][0], " data should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["cleansingId"]?.ToString()), " cleansingId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["polisyClientId"]?.ToString()), " polisyClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["crmClientId"]?.ToString()), " crmClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorGroupCode"]?.ToString()), " sapVendorGroupCode should not null");

            Assert.AreEqual(corporateName1, outputJson["data"][0]["corporateName1"]?.ToString(), " personalName should Equal input");
            Assert.AreEqual(corporateName2, outputJson["data"][0]["corporateName2"]?.ToString(), " personalSurname should Equal input");

        }


        [TestMethod()]
        [Priority(6)]
        public void Post_RegPayeeCorporate_CG06_Test_Test()
        {
            /*Description
              * ข้อมูลที่มีใน <<CLS>> และ <<POLISY400>> และ <<CRM>> ทั้งหมดแล้ว
              *Input
              * roleCode = G	
              * cleansingId=null=not null	
              * polisyClientId  =not null	
              * crmClientId     =not null
              * 
              *Expected API Action
              * ต้องยิงไปที่
                   - SAP_InquiryVendor
                   - SAP_CreateVendor

              * ต้องเกิดข้อมูลใหม่ที่
              *  <<SAP>>
              */


            /*
             * Expected Output 
             * cleansingId = api input นอกนั้งสร้างใหม่
             {"data":[{"cleansingId":"C2017-100002646","polisyClientId":"16973078","crmClientId":"201709-0000591","personalName":"ทดสอบC59PM01F4U","personalSurname":"ทดสอบ1DXVY5VYKR"}],"code":"200","message":"Success","description":"","transactionId":"00-600913150406827-0000000","transactionDateTime":"2017-09-13T15:04:24.9765239+07"}
             */

            //cretae cleansingId
            var ctl = new CLSCreateCorporateClientTests();
            string cleansingId = ctl.Execute_CLSCreateGeneralCorporateClientTest();


            //Test
            var corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idTax = "" + RandomValueGenerator.RandomNumber(13);
            string jsonInput = String.Format(@"{{
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
                                    'cleansingId': '{6}',
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
                                idTax,
                                 RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                                 RandomValueGenerator.RandomOptions(new[] { "", "R1", "R2" }),
                                 RandomValueGenerator.RandomOptions(new[] { "Y", "N" }),
                                 cleansingId

                                 );



            // ระบุ Controller ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeeCorporateController>(jsonInput, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());

            // ตรวจสอบค่าอื่นๆ 
            Assert.IsNotNull(outputJson["data"][0], " data should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["cleansingId"]?.ToString()), " cleansingId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["polisyClientId"]?.ToString()), " polisyClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["crmClientId"]?.ToString()), " crmClientId should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorCode"]?.ToString()), " sapVendorCode should not null");
            Assert.IsFalse(string.IsNullOrEmpty(outputJson["data"][0]["sapVendorGroupCode"]?.ToString()), " sapVendorGroupCode should not null");

            Assert.AreEqual(corporateName1, outputJson["data"][0]["corporateName1"]?.ToString(), " personalName should Equal input");
            Assert.AreEqual(corporateName2, outputJson["data"][0]["corporateName2"]?.ToString(), " personalSurname should Equal input");

        }
    }


}