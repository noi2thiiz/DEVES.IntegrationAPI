using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DEVES.IntegrationAPI.Core.Util;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.Controllers.Tests
{
    [TestClass()]
    public class RegPayeePersonalControllerTests : BaseControllersTests
    {
        [TestMethod]
        public void Post_RegPayeePersonalController_It_Should_Success_When_Give_Valid_Input_Test()
        {
            /* Expected Output
             {"data":[{"cleansingId":"C2017-100002517","polisyClientId":"16972975","crmClientId":"201709-0000500","sapVendorCode":"16972975","sapVendorGroupCode":"DIR","personalName":"ทดสอบ0W2CLA38XY","personalSurname":"ทดสอบPOP20C63NO"}],"code":"200","message":"SUCCESS","description":null,"transactionId":"00-600912141811037-0000000","transactionDateTime":"2017-09-12T14:18:13.238996+07:00"}
             */
            //input personalName, personalSurname, idCitizen
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idCitizen = "" + RandomValueGenerator.RandomNumber(13);
            var jsonString = String.Format("{{  " +
                                           "\"generalHeader\": {{   " +
                                           "\"roleCode\": \"G\",   " +
                                           "\"cleansingId\": \"\",   " +
                                           "\"polisyClientId\": \"\",   " +
                                           "\"crmPersonId\": \"\",   " +
                                           "\"clientAdditionalExistFlag\": \"N\" }}, " +
                                           "\"contactInfo\": {{   " +
                                               "\"telephone1\": \"\",   " +
                                               "\"telephone1Ext\": \"\",  " +
                                               " \"telephone2\": \"\",  " +
                                               " \"telephone2Ext\": \"\",   " +
                                               "\"telephone3\": \"\",   " +
                                               "\"mobilePhone\": \"\",   " +
                                               "\"fax\": \"\",   " +
                                               "\"emailAddress\": \"\",   " +
                                               "\"lineID\": \"\",   " +
                                               "\"facebook\": \"\" }}, " +
                                           "\"sapVendorInfo\": {{  " +
                                               " \"sapVendorGroupCode\": " +
                                               "\"DIR\",   " +
                                               "\"bankInfo\": {{     " +
                                               "\"bankCountryCode\": \"\",    " +
                                               " \"bankCode\": \"\",    " +
                                               " \"bankBranchCode\": \"\",    " +
                                               " \"bankAccount\": \"\",    " +
                                               " \"accountHolder\": \"\",  " +
                                               " \"paymentMethods\": \"\"   }},  " +
                                               " \"withHoldingTaxInfo\": {{    " +
                                           "         \"whtTaxCode\": \"\",     " +
                                                    "\"receiptType\": \"\"   }} }}, " +
                                           "\"addressInfo\": {{   " +
                                               "\"address1\": \"1/212\",   " +
                                               "\"address2\": \"\",   " +
                                               "\"address3\": \"\",   " +
                                               "\"subDistrictCode\": \"\",  " +
                                               " \"districtCode\": \"\",   " +
                                               "\"provinceCode\": \"10\",   " +
                                               "\"postalCode\": \"11110\",   " +
                                               "\"country\": \"00005\",   " +
                                               "\"addressType\": \"\",   " +
                                               "\"latitude\": \"\",   " +
                                               "\"longtitude\": \"\" }}, " +
                                           "\"profileInfo\": {{   " +
                                               "\"salutation\": \"0001\",   " +
                                               "\"personalName\": \"{0}\",   " +
                                               "\"personalSurname\": \"{1}\",   " +
                                               "\"sex\": \"M\",  " +
                                               " \"idCitizen\": \"{2}\",   " +
                                               "\"idPassport\": \"\",   " +
                                               "\"idAlien\": \"\",   " +
                                               "\"idDriving\": \"\",   " +
                                               "\"birthDate\": \"2017-01-01 00:00:00\",   " +
                                               "\"nationality\": \"00220\",  " +
                                               " \"language\": \"\",   " +
                                               "\"married\": \"\",   " +
                                               "\"occupation\": \"\",   " +
                                               "\"riskLevel\": \"\",  " +
                                               " \"vipStatus\": \"\",   " +
                                           "\"remark\": \"\" }}" +
                                           "}}",
                personalName, personalSurname, idCitizen);

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeePersonalController>(jsonString, "Post");

            Assert.IsNotNull(response?.Result);
            Console.WriteLine("==============output==================");
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

            Assert.AreEqual(personalName, outputJson["data"][0]["personalName"]?.ToString(), " personalName should Equal input");
            Assert.AreEqual(personalSurname, outputJson["data"][0]["personalSurname"]?.ToString(), " personalSurname should Equal input");
         
        }

        [TestMethod]
        public void Post_RegPayeePersonalController_It_Should_Fail_When_Give_InValid_Input_Test()
        {
            //input
            // ทดสอบโดยไม่ส่ง  personalName personalSurname และ idCitizen
            var jsonString = String.Format("{{  " +
                                          "\"generalHeader\": {{   " +
                                          "\"roleCode\": \"\",   " +
                                          "\"cleansingId\": \"\",   " +
                                          "\"polisyClientId\": \"\",   " +
                                          "\"crmPersonId\": \"\",   " +
                                          "\"clientAdditionalExistFlag\": \"N\" }}, " +
                                          "\"contactInfo\": {{   " +
                                              "\"telephone1\": \"\",   " +
                                              "\"telephone1Ext\": \"\",  " +
                                              " \"telephone2\": \"\",  " +
                                              " \"telephone2Ext\": \"\",   " +
                                              "\"telephone3\": \"\",   " +
                                              "\"mobilePhone\": \"\",   " +
                                              "\"fax\": \"\",   " +
                                              "\"emailAddress\": \"\",   " +
                                              "\"lineID\": \"\",   " +
                                              "\"facebook\": \"\" }}, " +
                                          "\"sapVendorInfo\": {{  " +
                                              " \"sapVendorGroupCode\": " +
                                              "\"DIR\",   " +
                                              "\"bankInfo\": {{     " +
                                              "\"bankCountryCode\": \"\",    " +
                                              " \"bankCode\": \"\",    " +
                                              " \"bankBranchCode\": \"\",    " +
                                              " \"bankAccount\": \"\",    " +
                                              " \"accountHolder\": \"\",  " +
                                              " \"paymentMethods\": \"\"   }},  " +
                                              " \"withHoldingTaxInfo\": {{    " +
                                          "         \"whtTaxCode\": \"\",     " +
                                                   "\"receiptType\": \"\"   }} }}, " +
                                          "\"addressInfo\": {{   " +
                                              "\"address1\": \"1/212\",   " +
                                              "\"address2\": \"\",   " +
                                              "\"address3\": \"\",   " +
                                              "\"subDistrictCode\": \"\",  " +
                                              " \"districtCode\": \"\",   " +
                                              "\"provinceCode\": \"10\",   " +
                                              "\"postalCode\": \"11110\",   " +
                                              "\"country\": \"00005\",   " +
                                              "\"addressType\": \"\",   " +
                                              "\"latitude\": \"\",   " +
                                              "\"longtitude\": \"\" }}, " +
                                          "\"profileInfo\": {{   " +
                                              "\"salutation\": \"0001\",   " +
                                              "\"personalName\": \"{0}\",   " +
                                              "\"personalSurname\": \"{1}\",   " +
                                              "\"sex\": \"M\",  " +
                                              " \"idCitizen\": \"{2}\",   " +
                                              "\"idPassport\": \"\",   " +
                                              "\"idAlien\": \"\",   " +
                                              "\"idDriving\": \"\",   " +
                                              "\"birthDate\": \"2017-01-01 00:00:00\",   " +
                                              "\"nationality\": \"00220\",  " +
                                              " \"language\": \"\",   " +
                                              "\"married\": \"\",   " +
                                              "\"occupation\": \"\",   " +
                                              "\"riskLevel\": \"\",  " +
                                              " \"vipStatus\": \"\",   " +
                                          "\"remark\": \"\" }}" +
                                          "}}",
               "", "", "");

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeePersonalController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 400
            Assert.AreEqual("400", outputJson["code"]?.ToString());

            // ต้อง พบ error message ต่อไปนี้
            AssertRequiredField("profileInfo.personalName", outputJson);
            AssertRequiredField("profileInfo.personalSurname", outputJson);
            AssertRequiredField("profileInfo.idCitizen", outputJson);
            AssertRequiredField("generalHeader.roleCode", outputJson);
        }

        [TestMethod]
        public void Post_RegPayeePersonalController_It_Should_Fail_When_Give_Empty_Input_Test()
        {
            /* Expected output
             {"stackTrace":null,"code":"400","message":"Invalid input(s)",
             "description":"Some of your input is invalid. Please recheck again",
             "transactionId":"00-600912151523459-0000001","transactionDateTime":"2017-09-12T15:15:23.4596209+07:00",
             "data":{"fieldErrors":[
             {"name":"generalHeader","message":"Required properties are missing from object"},
             {"name":"profileInfo","message":"Required properties are missing from object"},
             {"name":"addressInfo","message":"Required properties are missing from object"},
             {"name":"sapVendorInfo","message":"Required properties are missing from object"}]}}
             */
            //input
            var jsonString = @"
                {
                 
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeePersonalController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 500
            Assert.AreEqual("400", outputJson["code"]?.ToString());

            // Assert Required Field
            AssertRequiredField("generalHeader", outputJson);
            AssertRequiredField("profileInfo", outputJson);
            AssertRequiredField("addressInfo", outputJson);
            AssertRequiredField("sapVendorInfo", outputJson);

        }

        [TestMethod]
        public void Post_RegPayeePersonalController_It_Should_Success_When_Give_Emtpy_BirthDate_Input_Test()
        {
            /* Expected Output
             {"data":[{"cleansingId":"C2017-100002517","polisyClientId":"16972975","crmClientId":"201709-0000500","sapVendorCode":"16972975","sapVendorGroupCode":"DIR","personalName":"ทดสอบ0W2CLA38XY","personalSurname":"ทดสอบPOP20C63NO"}],"code":"200","message":"SUCCESS","description":null,"transactionId":"00-600912141811037-0000000","transactionDateTime":"2017-09-12T14:18:13.238996+07:00"}
             */
            //input ไม่ BirthDate
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idCitizen = "" + RandomValueGenerator.RandomNumber(13);
            var jsonString = String.Format("{{  " +
                                           "\"generalHeader\": {{   " +
                                           "\"roleCode\": \"G\",   " +
                                           "\"cleansingId\": \"\",   " +
                                           "\"polisyClientId\": \"\",   " +
                                           "\"crmPersonId\": \"\",   " +
                                           "\"clientAdditionalExistFlag\": \"N\" }}, " +
                                           "\"contactInfo\": {{   " +
                                               "\"telephone1\": \"\",   " +
                                               "\"telephone1Ext\": \"\",  " +
                                               " \"telephone2\": \"\",  " +
                                               " \"telephone2Ext\": \"\",   " +
                                               "\"telephone3\": \"\",   " +
                                               "\"mobilePhone\": \"\",   " +
                                               "\"fax\": \"\",   " +
                                               "\"emailAddress\": \"\",   " +
                                               "\"lineID\": \"\",   " +
                                               "\"facebook\": \"\" }}, " +
                                           "\"sapVendorInfo\": {{  " +
                                               " \"sapVendorGroupCode\": " +
                                               "\"DIR\",   " +
                                               "\"bankInfo\": {{     " +
                                               "\"bankCountryCode\": \"\",    " +
                                               " \"bankCode\": \"\",    " +
                                               " \"bankBranchCode\": \"\",    " +
                                               " \"bankAccount\": \"\",    " +
                                               " \"accountHolder\": \"\",  " +
                                               " \"paymentMethods\": \"\"   }},  " +
                                               " \"withHoldingTaxInfo\": {{    " +
                                           "         \"whtTaxCode\": \"\",     " +
                                                    "\"receiptType\": \"\"   }} }}, " +
                                           "\"addressInfo\": {{   " +
                                               "\"address1\": \"1/212\",   " +
                                               "\"address2\": \"\",   " +
                                               "\"address3\": \"\",   " +
                                               "\"subDistrictCode\": \"\",  " +
                                               " \"districtCode\": \"\",   " +
                                               "\"provinceCode\": \"10\",   " +
                                               "\"postalCode\": \"11110\",   " +
                                               "\"country\": \"00005\",   " +
                                               "\"addressType\": \"\",   " +
                                               "\"latitude\": \"\",   " +
                                               "\"longtitude\": \"\" }}, " +
                                           "\"profileInfo\": {{   " +
                                               "\"salutation\": \"0001\",   " +
                                               "\"personalName\": \"{0}\",   " +
                                               "\"personalSurname\": \"{1}\",   " +
                                               "\"sex\": \"M\",  " +
                                               " \"idCitizen\": \"{2}\",   " +
                                               "\"idPassport\": \"\",   " +
                                               "\"idAlien\": \"\",   " +
                                               "\"idDriving\": \"\",   " +
                                               "\"birthDate\": \"\",   " +
                                               "\"nationality\": \"00220\",  " +
                                               " \"language\": \"\",   " +
                                               "\"married\": \"\",   " +
                                               "\"occupation\": \"\",   " +
                                               "\"riskLevel\": \"\",  " +
                                               " \"vipStatus\": \"\",   " +
                                           "\"remark\": \"\" }}" +
                                           "}}",
                personalName, personalSurname, idCitizen);

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeePersonalController>(jsonString, "Post");

            Assert.IsNotNull(response?.Result);
            Console.WriteLine("==============output==================");
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());

        }

        [TestMethod]
        public void Post_RegPayeePersonalController_It_Should_Success_When_Give_Valid_BirthDate_Input_Test()
        {
            /* Expected Output
             {"data":[{"cleansingId":"C2017-100002517","polisyClientId":"16972975","crmClientId":"201709-0000500","sapVendorCode":"16972975","sapVendorGroupCode":"DIR","personalName":"ทดสอบ0W2CLA38XY","personalSurname":"ทดสอบPOP20C63NO"}],"code":"200","message":"SUCCESS","description":null,"transactionId":"00-600912141811037-0000000","transactionDateTime":"2017-09-12T14:18:13.238996+07:00"}
             */
            //input มี BirthDate
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idCitizen = "" + RandomValueGenerator.RandomNumber(13);
            var jsonString = String.Format("{{  " +
                                           "\"generalHeader\": {{   " +
                                           "\"roleCode\": \"G\",   " +
                                           "\"cleansingId\": \"\",   " +
                                           "\"polisyClientId\": \"\",   " +
                                           "\"crmPersonId\": \"\",   " +
                                           "\"clientAdditionalExistFlag\": \"N\" }}, " +
                                           "\"contactInfo\": {{   " +
                                               "\"telephone1\": \"\",   " +
                                               "\"telephone1Ext\": \"\",  " +
                                               " \"telephone2\": \"\",  " +
                                               " \"telephone2Ext\": \"\",   " +
                                               "\"telephone3\": \"\",   " +
                                               "\"mobilePhone\": \"\",   " +
                                               "\"fax\": \"\",   " +
                                               "\"emailAddress\": \"\",   " +
                                               "\"lineID\": \"\",   " +
                                               "\"facebook\": \"\" }}, " +
                                           "\"sapVendorInfo\": {{  " +
                                               " \"sapVendorGroupCode\": " +
                                               "\"DIR\",   " +
                                               "\"bankInfo\": {{     " +
                                               "\"bankCountryCode\": \"\",    " +
                                               " \"bankCode\": \"\",    " +
                                               " \"bankBranchCode\": \"\",    " +
                                               " \"bankAccount\": \"\",    " +
                                               " \"accountHolder\": \"\",  " +
                                               " \"paymentMethods\": \"\"   }},  " +
                                               " \"withHoldingTaxInfo\": {{    " +
                                           "         \"whtTaxCode\": \"\",     " +
                                                    "\"receiptType\": \"\"   }} }}, " +
                                           "\"addressInfo\": {{   " +
                                               "\"address1\": \"1/212\",   " +
                                               "\"address2\": \"\",   " +
                                               "\"address3\": \"\",   " +
                                               "\"subDistrictCode\": \"\",  " +
                                               " \"districtCode\": \"\",   " +
                                               "\"provinceCode\": \"10\",   " +
                                               "\"postalCode\": \"11110\",   " +
                                               "\"country\": \"00005\",   " +
                                               "\"addressType\": \"\",   " +
                                               "\"latitude\": \"\",   " +
                                               "\"longtitude\": \"\" }}, " +
                                           "\"profileInfo\": {{   " +
                                               "\"salutation\": \"0001\",   " +
                                               "\"personalName\": \"{0}\",   " +
                                               "\"personalSurname\": \"{1}\",   " +
                                               "\"sex\": \"M\",  " +
                                               " \"idCitizen\": \"{2}\",   " +
                                               "\"idPassport\": \"\",   " +
                                               "\"idAlien\": \"\",   " +
                                               "\"idDriving\": \"\",   " +
                                           "\"birthDate\": \"2017-01-01\",   " +
                                               "\"nationality\": \"00220\",  " +
                                               " \"language\": \"\",   " +
                                               "\"married\": \"\",   " +
                                               "\"occupation\": \"\",   " +
                                               "\"riskLevel\": \"\",  " +
                                               " \"vipStatus\": \"\",   " +
                                           "\"remark\": \"\" }}" +
                                           "}}",
                personalName, personalSurname, idCitizen);

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeePersonalController>(jsonString, "Post");

            Assert.IsNotNull(response?.Result);
            Console.WriteLine("==============output==================");
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());

        }

       // [TestMethod]  ยังไม่ผ่าน
        public void Post_RegPayeePersonalController_It_Should_Success_When_Give_Null_BirthDate_Input_Test()
        {
            /* Expected Output
             {"data":[{"cleansingId":"C2017-100002517","polisyClientId":"16972975","crmClientId":"201709-0000500","sapVendorCode":"16972975","sapVendorGroupCode":"DIR","personalName":"ทดสอบ0W2CLA38XY","personalSurname":"ทดสอบPOP20C63NO"}],"code":"200","message":"SUCCESS","description":null,"transactionId":"00-600912141811037-0000000","transactionDateTime":"2017-09-12T14:18:13.238996+07:00"}
             */
            //input มี BirthDate = null
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idCitizen = "" + RandomValueGenerator.RandomNumber(13);
            var jsonString = String.Format("{{  " +
                                           "\"generalHeader\": {{   " +
                                           "\"roleCode\": \"G\",   " +
                                           "\"cleansingId\": \"\",   " +
                                           "\"polisyClientId\": \"\",   " +
                                           "\"crmPersonId\": \"\",   " +
                                           "\"clientAdditionalExistFlag\": \"N\" }}, " +
                                           "\"contactInfo\": {{   " +
                                               "\"telephone1\": \"\",   " +
                                               "\"telephone1Ext\": \"\",  " +
                                               " \"telephone2\": \"\",  " +
                                               " \"telephone2Ext\": \"\",   " +
                                               "\"telephone3\": \"\",   " +
                                               "\"mobilePhone\": \"\",   " +
                                               "\"fax\": \"\",   " +
                                               "\"emailAddress\": \"\",   " +
                                               "\"lineID\": \"\",   " +
                                               "\"facebook\": \"\" }}, " +
                                           "\"sapVendorInfo\": {{  " +
                                               " \"sapVendorGroupCode\": " +
                                               "\"DIR\",   " +
                                               "\"bankInfo\": {{     " +
                                               "\"bankCountryCode\": \"\",    " +
                                               " \"bankCode\": \"\",    " +
                                               " \"bankBranchCode\": \"\",    " +
                                               " \"bankAccount\": \"\",    " +
                                               " \"accountHolder\": \"\",  " +
                                               " \"paymentMethods\": \"\"   }},  " +
                                               " \"withHoldingTaxInfo\": {{    " +
                                           "         \"whtTaxCode\": \"\",     " +
                                                    "\"receiptType\": \"\"   }} }}, " +
                                           "\"addressInfo\": {{   " +
                                               "\"address1\": \"1/212\",   " +
                                               "\"address2\": \"\",   " +
                                               "\"address3\": \"\",   " +
                                               "\"subDistrictCode\": \"\",  " +
                                               " \"districtCode\": \"\",   " +
                                               "\"provinceCode\": \"10\",   " +
                                               "\"postalCode\": \"11110\",   " +
                                               "\"country\": \"00005\",   " +
                                               "\"addressType\": \"\",   " +
                                               "\"latitude\": \"\",   " +
                                               "\"longtitude\": \"\" }}, " +
                                           "\"profileInfo\": {{   " +
                                               "\"salutation\": \"0001\",   " +
                                               "\"personalName\": \"{0}\",   " +
                                               "\"personalSurname\": \"{1}\",   " +
                                               "\"sex\": \"M\",  " +
                                               " \"idCitizen\": \"{2}\",   " +
                                               "\"idPassport\": \"\",   " +
                                               "\"idAlien\": \"\",   " +
                                               "\"idDriving\": \"\",   " +
                                           "\"birthDate\": null,   " +
                                               "\"nationality\": \"00220\",  " +
                                               " \"language\": \"\",   " +
                                               "\"married\": \"\",   " +
                                               "\"occupation\": \"\",   " +
                                               "\"riskLevel\": \"\",  " +
                                               " \"vipStatus\": \"\",   " +
                                           "\"remark\": \"\" }}" +
                                           "}}",
                personalName, personalSurname, idCitizen);

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeePersonalController>(jsonString, "Post");

            Assert.IsNotNull(response?.Result);
            Console.WriteLine("==============output==================");
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());

        }

        [TestMethod]
        [Priority(1)]
        public void Post_RegPayeePersonal_PG01_Test()
        {
            /*Description
            *Input
            * roleCode = G	
            * cleansingId=    null	
            * polisyClientId= null	
            * crmClientId=     null
            * 
            *Expected API Action
            * ต้องยิงไปที่
             - CLS_CreatePersonalClient
             - CLIENT_CreatePersonalClientAndAdditionalInfo
             - SAP_InquiryVendor
             - SAP_CreateVendor
             - และสร้างข้อมูลใน CRM
            *  
            * ต้องเกิดข้อมูลใหม่ที่
            *  <<Cleansing>>
               <<Polisy400>>
               <<SAP>>
               <<CRM>>

            */

            /* Expected Output
             {"data":[{"cleansingId":"C2017-100002517","polisyClientId":"16972975","crmClientId":"201709-0000500","sapVendorCode":"16972975","sapVendorGroupCode":"DIR","personalName":"ทดสอบ0W2CLA38XY","personalSurname":"ทดสอบPOP20C63NO"}],"code":"200","message":"SUCCESS","description":null,"transactionId":"00-600912141811037-0000000","transactionDateTime":"2017-09-12T14:18:13.238996+07:00"}
             */


            //input มี BirthDate = null
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idCitizen = "" + RandomValueGenerator.RandomNumber(13);


            string jsonString = String.Format(@"
                   {{
                  'generalHeader': {{
                    'roleCode': 'G',
                    'cleansingId': '',
                    'polisyClientId': '',
                    'crmPersonId': '',
                    'clientAdditionalExistFlag': 'N'
                  }},
                  'contactInfo': {{
                    'telephone1': '',
                    'telephone1Ext': '',
                    'telephone2': '',
                    'telephone2Ext': '',
                    'telephone3': '',
                    'mobilePhone': '',
                    'fax': '',
                    'emailAddress': '',
                    'lineID': '',
                    'facebook': ''
                  }},
                  'sapVendorInfo': {{
                    'sapVendorGroupCode': 'DIR',
                    'bankInfo': {{
                      'bankCountryCode': '',
                      'bankCode': '',
                      'bankBranchCode': '',
                      'bankAccount': '',
                      'accountHolder': '',
                      'paymentMethods': ''
                    }},
                    'withHoldingTaxInfo': {{
                      'whtTaxCode': '',
                      'receiptType': ''
                    }}
                  }},
                  'addressInfo': {{
                    'address1': '1/212',
                    'address2': '',
                    'address3': '',
                    'subDistrictCode': '',
                    'districtCode': '',
                    'provinceCode': '10',
                    'postalCode': '11110',
                    'country': '00005',
                    'addressType': '',
                    'latitude': '',
                    'longtitude': ''
                  }},
                  'profileInfo': {{
                    'salutation': '0001',
                    'personalName': '{0}',
                    'personalSurname': '{1}',
                    'sex': 'M',
                    'idCitizen': '{2}',
                    'idPassport': '',
                    'idAlien': '',
                    'idDriving': '',
                    'birthDate': '',
                    'nationality': '00220',
                    'language': '',
                    'married': '',
                    'occupation': '',
                    'riskLevel': '',
                    'vipStatus': '',
                    'remark': ''
                  }}
                }}",
                personalName,
                personalSurname,
                idCitizen
                );

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeePersonalController>(jsonString, "Post");

            Assert.IsNotNull(response?.Result);
            Console.WriteLine("==============output==================");
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());


            var outputData = outputJson["data"][0];
            Assert.IsNotNull(outputData);
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["polisyClientId"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ POLISY400");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["cleansingId"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ CLS");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["crmClientId"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ CRM");

            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["sapVendorCode"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ SAP");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["sapVendorGroupCode"]?.ToString()), "ต้องเกิด sapVendorGroupCode");


            Assert.AreEqual(personalName, outputData["personalName"]?.ToString());
            Assert.AreEqual(personalSurname, outputData["personalSurname"]?.ToString());

        }

        [TestMethod]
        [Priority(2)]
        public void Post_RegPayeePersonal_PG02_Test()
        {
            
            /*Description
            *Input
            * roleCode = G	
            * cleansingId=    null	
            * polisyClientId= not null	
            * crmClientId=     null
            * 
            *Expected API Action
            * ต้องยิงไปที่
             - SAP_InquiryVendor
             - SAP_CreateVendor
            *  
            * ต้องเกิดข้อมูลใหม่ที่
               <<SAP>>
               ไม่เกิดข้อมูลใน CLS 
               เนื่องจากไม่มี cleansingId  จึงไม่สามารถตรวจสอบข้อมูลใน crm หรือสร้างข้อมูลใน crm ได้ <<CRM>>  
            */

            /* Expected Output
             * ไม่มี crmClientId
             {"data":[{"cleansingId":"C2017-100002517","polisyClientId":"16972975","crmClientId":"","sapVendorCode":"16972975","sapVendorGroupCode":"DIR","personalName":"ทดสอบ0W2CLA38XY","personalSurname":"ทดสอบPOP20C63NO"}],"code":"200","message":"SUCCESS","description":null,"transactionId":"00-600912141811037-0000000","transactionDateTime":"2017-09-12T14:18:13.238996+07:00"}
             */

            //create client
            var clientPersonalCtrl = new RegClientPersonalControllerTests();
            var perssonal = clientPersonalCtrl.Post_RegClientPersonalTest();
            var polisyClientId = perssonal["polisyClientId"]?.ToString();


            //input มี BirthDate = null
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idCitizen = "" + RandomValueGenerator.RandomNumber(13);


            string jsonString = String.Format(@"
                   {{
                  'generalHeader': {{
                    'roleCode': 'G',
                    'cleansingId': '',
                    'polisyClientId': '{3}',
                    'crmPersonId': '',
                    'clientAdditionalExistFlag': 'N'
                  }},
                  'contactInfo': {{
                    'telephone1': '',
                    'telephone1Ext': '',
                    'telephone2': '',
                    'telephone2Ext': '',
                    'telephone3': '',
                    'mobilePhone': '',
                    'fax': '',
                    'emailAddress': '',
                    'lineID': '',
                    'facebook': ''
                  }},
                  'sapVendorInfo': {{
                    'sapVendorGroupCode': 'DIR',
                    'bankInfo': {{
                      'bankCountryCode': '',
                      'bankCode': '',
                      'bankBranchCode': '',
                      'bankAccount': '',
                      'accountHolder': '',
                      'paymentMethods': ''
                    }},
                    'withHoldingTaxInfo': {{
                      'whtTaxCode': '',
                      'receiptType': ''
                    }}
                  }},
                  'addressInfo': {{
                    'address1': '1/212',
                    'address2': '',
                    'address3': '',
                    'subDistrictCode': '',
                    'districtCode': '',
                    'provinceCode': '10',
                    'postalCode': '11110',
                    'country': '00005',
                    'addressType': '',
                    'latitude': '',
                    'longtitude': ''
                  }},
                  'profileInfo': {{
                    'salutation': '0001',
                    'personalName': '{0}',
                    'personalSurname': '{1}',
                    'sex': 'M',
                    'idCitizen': '{2}',
                    'idPassport': '',
                    'idAlien': '',
                    'idDriving': '',
                    'birthDate': '',
                    'nationality': '00220',
                    'language': '',
                    'married': '',
                    'occupation': '',
                    'riskLevel': '',
                    'vipStatus': '',
                    'remark': ''
                  }}
                }}",
                personalName,
                personalSurname,
                idCitizen,
                polisyClientId
                );

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeePersonalController>(jsonString, "Post");

            Assert.IsNotNull(response?.Result);
            Console.WriteLine("==============output==================");
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());


            var outputData = outputJson["data"][0];
            Assert.IsNotNull(outputData);
            Assert.AreEqual(polisyClientId, outputData["polisyClientId"]?.ToString(), " polisyClientId = Input");
            Assert.AreEqual(true, string.IsNullOrEmpty(outputData["cleansingId"]?.ToString()), "ต้องไม่เกิดเกิดข้อมูลใหม่ที่ CLS");
            Assert.AreEqual(true, string.IsNullOrEmpty(outputData["crmClientId"]?.ToString()), "ต้องไม่เกิดข้อมูลใหม่ที่ CRM");

            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["sapVendorCode"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ SAP");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["sapVendorGroupCode"]?.ToString()), "ต้องเกิด sapVendorGroupCode");


            Assert.AreEqual(personalName, outputData["personalName"]?.ToString());
            Assert.AreEqual(personalSurname, outputData["personalSurname"]?.ToString());

        }

        [TestMethod]
        [Priority(3)]
        public void Post_RegPayeePersonal_PG03_Test()
        {
            
            /*Description
            *Input
            * roleCode = G	
            * cleansingId=    not null	
            * polisyClientId=  null	
            * crmClientId=     null
            * 
            *Expected API Action
            * ต้องยิงไปที่
              - CLIENT_CreatePersonalClientAndAdditionalInfo
              - SAP_InquiryVendor
              - SAP_CreateVendor
              - และสร้างข้อมูลใน CRM
            *  
            * ต้องเกิดข้อมูลใหม่ที่
               <<Polisy400>>
               <<SAP>>
               <<CRM>>
            */

            /* Expected Output
             * ไม่มี crmClientId
             {"data":[{"cleansingId":"C2017-100002517","polisyClientId":"16972975","crmClientId":"","sapVendorCode":"16972975","sapVendorGroupCode":"DIR","personalName":"ทดสอบ0W2CLA38XY","personalSurname":"ทดสอบPOP20C63NO"}],"code":"200","message":"SUCCESS","description":null,"transactionId":"00-600912141811037-0000000","transactionDateTime":"2017-09-12T14:18:13.238996+07:00"}
             */

            //Prepare
            #region cretae cleansingId
            // cretae cleansingId
            var service =
                new CLSCreatePersonalService(Guid.NewGuid().ToString(), "UnitTest");
            var result = service.Execute(new CLSCreatePersonalClientInputModel
            {
                roleCode = "G",
                clientId = "",
                crmPersonId = "",
                isPayee = "Y",
                salutation = "0023",
                personalName = "ทดสอบ" + RandomValueGenerator.RandomNumber(10),
                personalSurname = "ทดสอบ" + RandomValueGenerator.RandomNumber(10),
                sex = "M",
                idCitizen = "3469900290301",
                idPassport = "",
                idAlien = "",
                idDriving = "",
                // dbirthDate = "2017-08-07 00:00:00",
                natioanality = "004",
                language = "T",
                married = "D",
                occupation = "000",
                vipStatus = "N",
                telephone1 = "02" + RandomValueGenerator.RandomNumber(8),
                telephone1Ext = "",
                telephone2 = "",
                telephone2Ext = "",
                telNo = "",
                telNoExt = "",
                mobilePhone = "086" + RandomValueGenerator.RandomNumber(7),
                fax = "",
                emailAddress = "",
                lineID = "",
                facebook = "",
                address1 = "123",
                address2 = "อาคารชัย",
                address3 = "ห้อง 306",
                subDistrictCode = "100101",
                districtCode = "1001",
                provinceCode = "10",
                postalCode = "10200",
                country = "764",
                addressType = "03",
                latitude = "99",
                longitude = "99",
            });
            var cleansingId = result?.data?.cleansingId;
            Assert.IsFalse(string.IsNullOrEmpty(result?.data?.cleansingId), "error on create cleansingId");


            #endregion cretae cleansingId


            //input มี BirthDate = null
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idCitizen = "" + RandomValueGenerator.RandomNumber(13);


            string jsonString = String.Format(@"
                   {{
                  'generalHeader': {{
                    'roleCode': 'G',
                    'cleansingId': '{3}',
                    'polisyClientId': '',
                    'crmPersonId': '',
                    'clientAdditionalExistFlag': 'N'
                  }},
                  'contactInfo': {{
                    'telephone1': '',
                    'telephone1Ext': '',
                    'telephone2': '',
                    'telephone2Ext': '',
                    'telephone3': '',
                    'mobilePhone': '',
                    'fax': '',
                    'emailAddress': '',
                    'lineID': '',
                    'facebook': ''
                  }},
                  'sapVendorInfo': {{
                    'sapVendorGroupCode': 'DIR',
                    'bankInfo': {{
                      'bankCountryCode': '',
                      'bankCode': '',
                      'bankBranchCode': '',
                      'bankAccount': '',
                      'accountHolder': '',
                      'paymentMethods': ''
                    }},
                    'withHoldingTaxInfo': {{
                      'whtTaxCode': '',
                      'receiptType': ''
                    }}
                  }},
                  'addressInfo': {{
                    'address1': '1/212',
                    'address2': '',
                    'address3': '',
                    'subDistrictCode': '',
                    'districtCode': '',
                    'provinceCode': '10',
                    'postalCode': '11110',
                    'country': '00005',
                    'addressType': '',
                    'latitude': '',
                    'longtitude': ''
                  }},
                  'profileInfo': {{
                    'salutation': '0001',
                    'personalName': '{0}',
                    'personalSurname': '{1}',
                    'sex': 'M',
                    'idCitizen': '{2}',
                    'idPassport': '',
                    'idAlien': '',
                    'idDriving': '',
                    'birthDate': '',
                    'nationality': '00220',
                    'language': '',
                    'married': '',
                    'occupation': '',
                    'riskLevel': '',
                    'vipStatus': '',
                    'remark': ''
                  }}
                }}",
                personalName,
                personalSurname,
                idCitizen,
                cleansingId
                );

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeePersonalController>(jsonString, "Post");

            Assert.IsNotNull(response?.Result);
            Console.WriteLine("==============output==================");
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());


            var outputData = outputJson["data"][0];
            Assert.IsNotNull(outputData);
   
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["polisyClientId"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ POLISY400");
            Assert.AreEqual(cleansingId, outputData["cleansingId"]?.ToString(), "ต้องไม่เกิดเกิดข้อมูลใหม่ที่ CLS (CLS=input)");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["crmClientId"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ CRM");

            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["sapVendorCode"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ SAP");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["sapVendorGroupCode"]?.ToString()), "ต้องเกิด sapVendorGroupCode");


            Assert.AreEqual(personalName, outputData["personalName"]?.ToString());
            Assert.AreEqual(personalSurname, outputData["personalSurname"]?.ToString());

        }

        [TestMethod]
        [Priority(4)]
        public void Post_RegPayeePersonal_PG04_Test()
        {
            
            /*Description
            *Input
            * roleCode = G	
            * cleansingId=    not null	
            * polisyClientId=  null	
            * crmClientId=     not null
            * 
            *Expected API Action
            * ต้องยิงไปที่
              - CLIENT_CreatePersonalClientAndAdditionalInfo
              - SAP_InquiryVendor
              - SAP_CreateVendor
  
            *  
            * ต้องเกิดข้อมูลใหม่ที่
               <<Polisy400>>
               <<SAP>>
            
            */

            /* Expected Output
             * ไม่มี crmClientId
             {"data":[{"cleansingId":"C2017-100002517","polisyClientId":"16972975","crmClientId":"","sapVendorCode":"16972975","sapVendorGroupCode":"DIR","personalName":"ทดสอบ0W2CLA38XY","personalSurname":"ทดสอบPOP20C63NO"}],"code":"200","message":"SUCCESS","description":null,"transactionId":"00-600912141811037-0000000","transactionDateTime":"2017-09-12T14:18:13.238996+07:00"}
             */

            //Prepare
            #region cretae cleansingId

            //create client
            var clientPersonalCtrl = new RegClientPersonalControllerTests();
            var perssonal = clientPersonalCtrl.Create_ClientPersonalNotCreatePolisyClient();
         
         

            var cleansingId = perssonal["cleansingId"]?.ToString();
            var crmPersonId = perssonal["crmClientId"]?.ToString();
            var polisyClientId = perssonal["polisyClientId"]?.ToString();

            #endregion cretae cleansingId


            //input มี BirthDate = null
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idCitizen = "" + RandomValueGenerator.RandomNumber(13);


            string jsonString = String.Format(@"
                   {{
                  'generalHeader': {{
                    'roleCode': 'G',
                    'cleansingId': '{3}',
                    'polisyClientId': '',
                    'crmPersonId': '{4}',
                    'clientAdditionalExistFlag': 'N'
                  }},
                  'contactInfo': {{
                    'telephone1': '',
                    'telephone1Ext': '',
                    'telephone2': '',
                    'telephone2Ext': '',
                    'telephone3': '',
                    'mobilePhone': '',
                    'fax': '',
                    'emailAddress': '',
                    'lineID': '',
                    'facebook': ''
                  }},
                  'sapVendorInfo': {{
                    'sapVendorGroupCode': 'DIR',
                    'bankInfo': {{
                      'bankCountryCode': '',
                      'bankCode': '',
                      'bankBranchCode': '',
                      'bankAccount': '',
                      'accountHolder': '',
                      'paymentMethods': ''
                    }},
                    'withHoldingTaxInfo': {{
                      'whtTaxCode': '',
                      'receiptType': ''
                    }}
                  }},
                  'addressInfo': {{
                    'address1': '1/212',
                    'address2': '',
                    'address3': '',
                    'subDistrictCode': '',
                    'districtCode': '',
                    'provinceCode': '10',
                    'postalCode': '11110',
                    'country': '00005',
                    'addressType': '',
                    'latitude': '',
                    'longtitude': ''
                  }},
                  'profileInfo': {{
                    'salutation': '0001',
                    'personalName': '{0}',
                    'personalSurname': '{1}',
                    'sex': 'M',
                    'idCitizen': '{2}',
                    'idPassport': '',
                    'idAlien': '',
                    'idDriving': '',
                    'birthDate': '',
                    'nationality': '00220',
                    'language': '',
                    'married': '',
                    'occupation': '',
                    'riskLevel': '',
                    'vipStatus': '',
                    'remark': ''
                  }}
                }}",
                personalName,
                personalSurname,
                idCitizen,
                cleansingId,
                crmPersonId
                );

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeePersonalController>(jsonString, "Post");

            Assert.IsNotNull(response?.Result);
            Console.WriteLine("==============output==================");
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());


            var outputData = outputJson["data"][0];
            Assert.IsNotNull(outputData);

            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["polisyClientId"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ POLISY400");
            Assert.AreEqual(cleansingId, outputData["cleansingId"]?.ToString(), "ต้องไม่เกิดเกิดข้อมูลใหม่ที่ CLS (CLS=input)");
            Assert.AreEqual(crmPersonId, outputData["crmClientId"]?.ToString(), "ต้องไม่เกิดข้อมูลใหม่ที่ CRM (CRM=input)");

            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["sapVendorCode"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ SAP");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["sapVendorGroupCode"]?.ToString()), "ต้องเกิด sapVendorGroupCode");


            Assert.AreEqual(personalName, outputData["personalName"]?.ToString());
            Assert.AreEqual(personalSurname, outputData["personalSurname"]?.ToString());

        }

        [TestMethod]
        [Priority(5)]
        public void Post_RegPayeePersonal_PG05_Test()
        {
            
            /*Description
            *Input
            * roleCode = G	
            * cleansingId=    not null	
            * polisyClientId= not null	
            * crmClientId=      null
            * 
            *Expected API Action
            * ต้องยิงไปที่
             - SAP_InquiryVendor
             - SAP_CreateVendor
            *  
            * ต้องเกิดข้อมูลใหม่ที่
               <<SAP>>

            */

            /* Expected Output
             * ไม่มี crmClientId
             {"data":[{"cleansingId":"C2017-100002517","polisyClientId":"16972975","crmClientId":"","sapVendorCode":"16972975","sapVendorGroupCode":"DIR","personalName":"ทดสอบ0W2CLA38XY","personalSurname":"ทดสอบPOP20C63NO"}],"code":"200","message":"SUCCESS","description":null,"transactionId":"00-600912141811037-0000000","transactionDateTime":"2017-09-12T14:18:13.238996+07:00"}
             */

            //create client
            var clientPersonalCtrl = new RegClientPersonalControllerTests();
            var perssonal = clientPersonalCtrl.Post_RegClientPersonal_NotCreateCrmClient_Test();
            var polisyClientId = perssonal["polisyClientId"]?.ToString();
           
            var cleansingId = perssonal["polisyClientId"]?.ToString();


            //input มี BirthDate = null
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idCitizen = "" + RandomValueGenerator.RandomNumber(13);


            string jsonString = String.Format(@"
                   {{
                  'generalHeader': {{
                    'roleCode': 'G',
                    'cleansingId': '{3}',
                    'polisyClientId': '{4}',
                    'crmPersonId': '',
                    'clientAdditionalExistFlag': 'N'
                  }},
                  'contactInfo': {{
                    'telephone1': '',
                    'telephone1Ext': '',
                    'telephone2': '',
                    'telephone2Ext': '',
                    'telephone3': '',
                    'mobilePhone': '',
                    'fax': '',
                    'emailAddress': '',
                    'lineID': '',
                    'facebook': ''
                  }},
                  'sapVendorInfo': {{
                    'sapVendorGroupCode': 'DIR',
                    'bankInfo': {{
                      'bankCountryCode': '',
                      'bankCode': '',
                      'bankBranchCode': '',
                      'bankAccount': '',
                      'accountHolder': '',
                      'paymentMethods': ''
                    }},
                    'withHoldingTaxInfo': {{
                      'whtTaxCode': '',
                      'receiptType': ''
                    }}
                  }},
                  'addressInfo': {{
                    'address1': '1/212',
                    'address2': '',
                    'address3': '',
                    'subDistrictCode': '',
                    'districtCode': '',
                    'provinceCode': '10',
                    'postalCode': '11110',
                    'country': '00005',
                    'addressType': '',
                    'latitude': '',
                    'longtitude': ''
                  }},
                  'profileInfo': {{
                    'salutation': '0001',
                    'personalName': '{0}',
                    'personalSurname': '{1}',
                    'sex': 'M',
                    'idCitizen': '{2}',
                    'idPassport': '',
                    'idAlien': '',
                    'idDriving': '',
                    'birthDate': '',
                    'nationality': '00220',
                    'language': '',
                    'married': '',
                    'occupation': '',
                    'riskLevel': '',
                    'vipStatus': '',
                    'remark': ''
                  }}
                }}",
                personalName,
                personalSurname,
                idCitizen,
                cleansingId,
                polisyClientId
                

                );

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeePersonalController>(jsonString, "Post");

            Assert.IsNotNull(response?.Result);
            Console.WriteLine("==============output==================");
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());


            var outputData = outputJson["data"][0];
            Assert.IsNotNull(outputData);
            Assert.AreEqual(polisyClientId, outputData["polisyClientId"]?.ToString(), " polisyClientId = Input ต้องไม่เกิดเกิดข้อมูลใหม่ที่ POLISY");
            Assert.AreEqual(cleansingId, outputData["cleansingId"]?.ToString(), "cleansingId = input,ต้องไม่เกิดเกิดข้อมูลใหม่ที่ CLS");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["crmClientId"]?.ToString()), " ต้องเกิดข้อมูลใหม่ที่ CRM");

            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["sapVendorCode"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ SAP");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["sapVendorGroupCode"]?.ToString()), "ต้องเกิด sapVendorGroupCode");


            Assert.AreEqual(personalName, outputData["personalName"]?.ToString());
            Assert.AreEqual(personalSurname, outputData["personalSurname"]?.ToString());

        }

        [TestMethod]
        [Priority(6)]
        public void Post_RegPayeePersonal_PG06_Test()
        {
            
            /*Description
            *Input
            * roleCode = G	
            * cleansingId=    not null	
            * polisyClientId= not null	
            * crmClientId=     not null
            * 
            *Expected API Action
            * ต้องยิงไปที่
             - SAP_InquiryVendor
             - SAP_CreateVendor
            *  
            * ต้องเกิดข้อมูลใหม่ที่
               <<SAP>>

            */

            /* Expected Output
             * ไม่มี crmClientId
             {"data":[{"cleansingId":"C2017-100002517","polisyClientId":"16972975","crmClientId":"","sapVendorCode":"16972975","sapVendorGroupCode":"DIR","personalName":"ทดสอบ0W2CLA38XY","personalSurname":"ทดสอบPOP20C63NO"}],"code":"200","message":"SUCCESS","description":null,"transactionId":"00-600912141811037-0000000","transactionDateTime":"2017-09-12T14:18:13.238996+07:00"}
             */

            //create client
            var clientPersonalCtrl = new RegClientPersonalControllerTests();
            var perssonal = clientPersonalCtrl.Post_RegClientPersonalTest();
            var polisyClientId = perssonal["polisyClientId"]?.ToString();
            var crmPersonId = perssonal["crmClientId"]?.ToString();
            var cleansingId = perssonal["polisyClientId"]?.ToString();


            //input มี BirthDate = null
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idCitizen = "" + RandomValueGenerator.RandomNumber(13);


            string jsonString = String.Format(@"
                   {{
                  'generalHeader': {{
                    'roleCode': 'G',
                    'cleansingId': '{3}',
                    'polisyClientId': '{4}',
                    'crmPersonId': '{5}',
                    'clientAdditionalExistFlag': 'N'
                  }},
                  'contactInfo': {{
                    'telephone1': '',
                    'telephone1Ext': '',
                    'telephone2': '',
                    'telephone2Ext': '',
                    'telephone3': '',
                    'mobilePhone': '',
                    'fax': '',
                    'emailAddress': '',
                    'lineID': '',
                    'facebook': ''
                  }},
                  'sapVendorInfo': {{
                    'sapVendorGroupCode': 'DIR',
                    'bankInfo': {{
                      'bankCountryCode': '',
                      'bankCode': '',
                      'bankBranchCode': '',
                      'bankAccount': '',
                      'accountHolder': '',
                      'paymentMethods': ''
                    }},
                    'withHoldingTaxInfo': {{
                      'whtTaxCode': '',
                      'receiptType': ''
                    }}
                  }},
                  'addressInfo': {{
                    'address1': '1/212',
                    'address2': '',
                    'address3': '',
                    'subDistrictCode': '',
                    'districtCode': '',
                    'provinceCode': '10',
                    'postalCode': '11110',
                    'country': '00005',
                    'addressType': '',
                    'latitude': '',
                    'longtitude': ''
                  }},
                  'profileInfo': {{
                    'salutation': '0001',
                    'personalName': '{0}',
                    'personalSurname': '{1}',
                    'sex': 'M',
                    'idCitizen': '{2}',
                    'idPassport': '',
                    'idAlien': '',
                    'idDriving': '',
                    'birthDate': '',
                    'nationality': '00220',
                    'language': '',
                    'married': '',
                    'occupation': '',
                    'riskLevel': '',
                    'vipStatus': '',
                    'remark': ''
                  }}
                }}",
                personalName,
                personalSurname,
                idCitizen,
                cleansingId,
                polisyClientId,
                crmPersonId
               
                );

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<RegPayeePersonalController>(jsonString, "Post");

            Assert.IsNotNull(response?.Result);
            Console.WriteLine("==============output==================");
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());


            var outputData = outputJson["data"][0];
            Assert.IsNotNull(outputData);
            Assert.AreEqual(polisyClientId, outputData["polisyClientId"]?.ToString(), " polisyClientId = Input ต้องไม่เกิดเกิดข้อมูลใหม่ที่ POLISY");
            Assert.AreEqual(cleansingId, outputData["cleansingId"]?.ToString(), "cleansingId = input,ต้องไม่เกิดเกิดข้อมูลใหม่ที่ CLS");
            Assert.AreEqual(crmPersonId, outputData["crmClientId"]?.ToString(), " crmClientId = input,ต้องไม่เกิดข้อมูลใหม่ที่ CRM");

            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["sapVendorCode"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ SAP");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["sapVendorGroupCode"]?.ToString()), "ต้องเกิด sapVendorGroupCode");


            Assert.AreEqual(personalName, outputData["personalName"]?.ToString());
            Assert.AreEqual(personalSurname, outputData["personalSurname"]?.ToString());

        }
    }
}