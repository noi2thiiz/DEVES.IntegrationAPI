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
        public void Post_RegPayeePersonalController_It_Should_Fail_When_Give_NotExisting_ClaimNotiNo_Test()
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
    }
}