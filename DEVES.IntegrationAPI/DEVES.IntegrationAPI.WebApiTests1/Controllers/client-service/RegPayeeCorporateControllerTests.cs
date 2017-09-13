using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Core.Util;
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

    }


}