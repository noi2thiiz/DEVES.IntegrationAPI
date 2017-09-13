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
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Templates;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.Controllers.Tests
{
    [TestClass()]
    public class RegClientPersonalControllerTests
    {
        [TestMethod()]
        public void Post_RegClientPersonalTest()
        {

            // Arrange
            var controller = new RegClientPersonalController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);

            string input = String.Format(@"
            {{
                'generalHeader':  
                {{
                    'roleCode': 'G',
                    'cleansingId': '',
                    'polisyClientId': '',
                    'crmClientId': '',
                    'clientAdditionalExistFlag': 'N'
                }},'profileInfo': {{
                    'salutation': '0002',
                    'personalName': '{0}',
                    'personalSurname' : '{1}' ,
                    'sex' : 'M',
                    'idCitizen' :'{2}',
                    'idPassport' : '',
                    'idAlien' : '',
                    'idDriving': '',
                    'birthDate' :'',
                    'nationality' : '',
                    'language' : '{3}',
                    'married' : '{4}',
                    'occupation' : '',
                    'riskLevel' : '',
                    'vipStatus' : '{5}',
                    'remark' : ' test post'
                }},'contactInfo':{{
                    'telephone1' : '',
                    'telephone1Ext' :'',
                    'telephone2' : '',
                    'telephone2Ext' :'',
                    'telephone3' : '',
                    'telephone3Ext' : '',
                    'mobilePhone' : '',
                    'fax' : '',
                    'emailAddress' :'',
                    'lineID' : '',
                    'facebook' : ''
                }},'addressInfo':{{
                    'address1' : '',
                    'address2' : '',
                    'address3' : '',
                    'subDistrictCode' : '',
                    'districtCode' : '',
                    'provinceCode' : '',
                    'postalCode' :'',
                    'country' : '',
                    'addressType' : '',
                    'latitude' : '',
                    'longtitude' : ''
                }}
             }}", personalName,
                personalSurname,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "M", "W" }),
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
            Assert.AreEqual(personalName, outputData["personalName"]?.ToString());
            Assert.AreEqual(personalSurname, outputData["personalSurname"]?.ToString());





        }

        [TestMethod()]
        public void Post_RegClientPersonal_It_Shoude_Success_When_Give_BirthDate_Test()
        {

            // Arrange
            var controller = new RegClientPersonalController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);

            string input = String.Format(@"
            {{
                'generalHeader':  
                {{
                    'roleCode': 'G',
                    'cleansingId': '',
                    'polisyClientId': '',
                    'crmClientId': '',
                    'clientAdditionalExistFlag': 'N'
                }},'profileInfo': {{
                    'salutation': '0002',
                    'personalName': '{0}',
                    'personalSurname' : '{1}' ,
                    'sex' : 'M',
                    'idCitizen' :'{2}',
                    'idPassport' : '',
                    'idAlien' : '',
                    'idDriving': '',
                    'birthDate' :'2009-04-22',
                    'nationality' : '',
                    'language' : '{3}',
                    'married' : '{4}',
                    'occupation' : '',
                    'riskLevel' : '',
                    'vipStatus' : '{5}',
                    'remark' : ' test BirthDate'
                }},'contactInfo':{{
                    'telephone1' : '',
                    'telephone1Ext' :'',
                    'telephone2' : '',
                    'telephone2Ext' :'',
                    'telephone3' : '',
                    'telephone3Ext' : '',
                    'mobilePhone' : '',
                    'fax' : '',
                    'emailAddress' :'',
                    'lineID' : '',
                    'facebook' : ''
                }},'addressInfo':{{
                    'address1' : '',
                    'address2' : '',
                    'address3' : '',
                    'subDistrictCode' : '',
                    'districtCode' : '',
                    'provinceCode' : '',
                    'postalCode' :'',
                    'country' : '',
                    'addressType' : '',
                    'latitude' : '',
                    'longtitude' : ''
                }}
             }}", personalName,
                personalSurname,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "M", "W" }),
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
            Assert.AreEqual(personalName, outputData["personalName"]?.ToString());
            Assert.AreEqual(personalSurname, outputData["personalSurname"]?.ToString());





        }

        [TestMethod()]
        public void Post_RegClientPersonal_It_Shoude_Fail_When_Give_Invalid_Master_Data_Test()
        {

            // Arrange
            var controller = new RegClientPersonalController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);

            string input = String.Format(@"
            {{
                'generalHeader':  
                {{
                    'roleCode': 'G',
                    'cleansingId': '',
                    'polisyClientId': '',
                    'crmClientId': '',
                    'clientAdditionalExistFlag': 'N'
                }},'profileInfo': {{
                    'salutation': '0002',
                    'personalName': '{0}',
                    'personalSurname' : '{1}' ,
                    'sex' : 'M',
                    'idCitizen' :'{2}',
                    'idPassport' : '',
                    'idAlien' : '',
                    'idDriving': '',
                    'birthDate' :'2009-04-22',
                    'nationality' : '',
                    'language' : '{3}',
                    'married' : '{4}',
                    'occupation' : '',
                    'riskLevel' : '',
                    'vipStatus' : '{5}',
                    'remark' : ' test BirthDate'
                }},'contactInfo':{{
                    'telephone1' : '',
                    'telephone1Ext' :'',
                    'telephone2' : '',
                    'telephone2Ext' :'',
                    'telephone3' : '',
                    'telephone3Ext' : '',
                    'mobilePhone' : '',
                    'fax' : '',
                    'emailAddress' :'',
                    'lineID' : '',
                    'facebook' : ''
                }},'addressInfo':{{
                    'address1' : '',
                    'address2' : '',
                    'address3' : '',
                    'subDistrictCode' : '999999',
                    'districtCode' : '9999',
                    'provinceCode' : '99',
                    'postalCode' :'99999',
                    'country' : '99999',
                    'addressType' : '99',
                    'latitude' : '',
                    'longtitude' : ''
                }}
             }}", personalName,
                personalSurname,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "M", "W" }),
                RandomValueGenerator.RandomOptions(new[] { "Y", "N" }));

            //Assert
            var response = (HttpResponseMessage)controller.Post(JObject.Parse(input));
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);
            var outputJson = JObject.Parse(output?.Result);
            Assert.AreEqual("400", outputJson["code"]?.ToString());
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionId"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["transactionDateTime"]?.ToString()));
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["message"]?.ToString()));

            //Assert fieldErrors
            Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["data"]["fieldErrors"].ToString()));

            Assert.AreEqual(true, output?.Result.Contains("addressInfo.country"));
            Assert.AreEqual(true, output?.Result.Contains("addressInfo.provinceCode"));
            Assert.AreEqual(true, output?.Result.Contains("addressInfo.districtCode"));
            Assert.AreEqual(true, output?.Result.Contains("addressInfo.subDistrictCode"));
            Assert.AreEqual(true, output?.Result.Contains("addressInfo.addressType"));

            // Assert.AreEqual(false, string.IsNullOrEmpty(outputData["cleansingId"]?.ToString()));
            //Assert.AreEqual(personalName, outputData["personalName"]?.ToString());
            //Assert.AreEqual(personalSurname, outputData["personalSurname"]?.ToString());

        }


        [TestMethod()]
        public void Post_RegClientPersonal_It_Shoude_Success_When_Give_Valid_Master_Data_Test()
        {

            // Arrange
            var controller = new RegClientPersonalController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);

            string input = String.Format(@"
            {{
                'generalHeader':  
                {{
                    'roleCode': 'G',
                    'cleansingId': '',
                    'polisyClientId': '',
                    'crmClientId': '',
                    'clientAdditionalExistFlag': 'N'
                }},'profileInfo': {{
                    'salutation': '0002',
                    'personalName': '{0}',
                    'personalSurname' : '{1}' ,
                    'sex' : 'M',
                    'idCitizen' :'{2}',
                    'idPassport' : '',
                    'idAlien' : '',
                    'idDriving': '',
                    'birthDate' :'2009-04-22',
                    'nationality' : '00203',
                    'language' : '{3}',
                    'married' : '{4}',
                    'occupation' : '00002',
                    'riskLevel' : 'R1',
                    'vipStatus' : '{5}',
                    'remark' : ' test BirthDate'
                }},'contactInfo':{{
                    'telephone1' : '',
                    'telephone1Ext' :'',
                    'telephone2' : '',
                    'telephone2Ext' :'',
                    'telephone3' : '',
                    'telephone3Ext' : '',
                    'mobilePhone' : '',
                    'fax' : '',
                    'emailAddress' :'',
                    'lineID' : '',
                    'facebook' : ''
                }},'addressInfo':{{
                    'address1' : '',
                    'address2' : '',
                    'address3' : '',
                    'subDistrictCode' : '101001',
                    'districtCode' : '1010',
                    'provinceCode' : '10',
                    'postalCode' :'10210',
                    'country' : '00220',
                    'addressType' : '01',
                    'latitude' : '',
                    'longtitude' : ''
                }}
             }}", personalName,
                personalSurname,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "M", "W" }),
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
            Assert.AreEqual(personalName, outputData["personalName"]?.ToString());
            Assert.AreEqual(personalSurname, outputData["personalSurname"]?.ToString());

        }

        [TestMethod()]
        public void Post_RegClientPersonal_It_Shoude_Rollback_When_Give_Conflict_Sex_And_Salutation_Data_Test()
        {
            //ถ้าสามารถ Rollback ได้สำเร็จ จะต้องไม่ติด error duplicate data


            // Arrange
            var controller = new RegClientPersonalController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var idCard = RandomValueGenerator.RandomNumber(13);
            for (var i = 0; i <= 5; i++)
            {
                string input = String.Format(@"
            {{
                'generalHeader':  
                {{
                    'roleCode': 'G',
                    'cleansingId': '',
                    'polisyClientId': '',
                    'crmClientId': '',
                    'clientAdditionalExistFlag': 'N'
                }},'profileInfo': {{
                    'salutation': '0002',
                    'personalName': '{0}',
                    'personalSurname' : '{1}' ,
                    'sex' : 'F',
                    'idCitizen' :'{2}',
                    'idPassport' : '',
                    'idAlien' : '',
                    'idDriving': '',
                    'birthDate' :'2009-04-22',
                    'nationality' : '00203',
                    'language' : '{3}',
                    'married' : '{4}',
                    'occupation' : '00002',
                    'riskLevel' : 'R1',
                    'vipStatus' : '{5}',
                    'remark' : ' test BirthDate'
                }},'contactInfo':{{
                    'telephone1' : '',
                    'telephone1Ext' :'',
                    'telephone2' : '',
                    'telephone2Ext' :'',
                    'telephone3' : '',
                    'telephone3Ext' : '',
                    'mobilePhone' : '',
                    'fax' : '',
                    'emailAddress' :'',
                    'lineID' : '',
                    'facebook' : ''
                }},'addressInfo':{{
                    'address1' : '',
                    'address2' : '',
                    'address3' : '',
                    'subDistrictCode' : '101001',
                    'districtCode' : '1010',
                    'provinceCode' : '10',
                    'postalCode' :'10210',
                    'country' : '00220',
                    'addressType' : '01',
                    'latitude' : '',
                    'longtitude' : ''
                }}
             }}", personalName,
                    personalSurname,
                    idCard,
                    RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                    RandomValueGenerator.RandomOptions(new[] { "M", "W" }),
                    RandomValueGenerator.RandomOptions(new[] { "Y", "N" }));

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
                Assert.AreEqual(false, string.IsNullOrEmpty(outputJson["message"]?.ToString()));

                Assert.AreEqual("COMP Error:Salut Sex not = Clnt Sex|", outputJson["message"]?.ToString());
                //Assert.AreEqual("COMP Error:Salut Sex not = Clnt Sex|", outputJson["message"]?.ToString());
               
            }



        }

        public string polisyClientIdForPg2 = "16973081";

        [TestMethod()]
        [Priority(1)]
        public void Post_RegClientPersonal_BusinessTest_PG_01()
        {
            /*Description
             * ข้อมูลลูกค้าใหม่ที่ยังไม่มีใน <<Cleansing>> และ <<Polisy400>>
             *Input
             * roleCode = G	cleansingId=null=null	polisyClientId=null	crmClientId=null
             * 
             *Expected API Action
             * ต้องยิงไปที่
             *  - CLS_CreatePersonalClient
             *  - CLIENT_CreatePersonalClientAndAdditionalInfo
             *  - และสร้างข้อมูลใน CRM
             * ต้องเกิดข้อมูลใหม่ที่
             *  <<Cleansing>>
             *  <<Polisy400>>
             *  <<CRM>>
             */


            /*
             * Expected Output 
             {"data":[{"cleansingId":"C2017-100002646","polisyClientId":"16973078","crmClientId":"201709-0000591","personalName":"ทดสอบC59PM01F4U","personalSurname":"ทดสอบ1DXVY5VYKR"}],"code":"200","message":"Success","description":"","transactionId":"00-600913150406827-0000000","transactionDateTime":"2017-09-13T15:04:24.9765239+07"}
             */


            // Arrange
            var controller = new RegClientPersonalController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);

            string input = String.Format(@"
            {{
                'generalHeader':  
                {{
                    'roleCode': 'G',
                    'cleansingId': '',
                    'polisyClientId': '',
                    'crmClientId': '',
                    'clientAdditionalExistFlag': 'N'
                }},'profileInfo': {{
                    'salutation': '0002',
                    'personalName': '{0}',
                    'personalSurname' : '{1}' ,
                    'sex' : 'M',
                    'idCitizen' :'{2}',
                    'idPassport' : '',
                    'idAlien' : '',
                    'idDriving': '',
                    'birthDate' :'2009-04-22',
                    'nationality' : '00203',
                    'language' : '{3}',
                    'married' : '{4}',
                    'occupation' : '00002',
                    'riskLevel' : 'R1',
                    'vipStatus' : '{5}',
                    'remark' : ' test BirthDate'
                }},'contactInfo':{{
                    'telephone1' : '',
                    'telephone1Ext' :'',
                    'telephone2' : '',
                    'telephone2Ext' :'',
                    'telephone3' : '',
                    'telephone3Ext' : '',
                    'mobilePhone' : '',
                    'fax' : '',
                    'emailAddress' :'',
                    'lineID' : '',
                    'facebook' : ''
                }},'addressInfo':{{
                    'address1' : '',
                    'address2' : '',
                    'address3' : '',
                    'subDistrictCode' : '101001',
                    'districtCode' : '1010',
                    'provinceCode' : '10',
                    'postalCode' :'10210',
                    'country' : '00220',
                    'addressType' : '01',
                    'latitude' : '',
                    'longtitude' : ''
                }}
             }}", personalName,
                personalSurname,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "M", "W" }),
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
            Assert.IsNotNull(outputData);
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["polisyClientId"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ POLISY400");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["cleansingId"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ CLS");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["crmClientId"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ CRM");

            polisyClientIdForPg2 = outputData["polisyClientId"]?.ToString();
            Assert.AreEqual(personalName, outputData["personalName"]?.ToString());
            Assert.AreEqual(personalSurname, outputData["personalSurname"]?.ToString());


        }

        [TestMethod()]
        [Priority(2)]
        public void Post_RegClientPersonal_BusinessTest_PG_02()
        {
            /*Description
             * ข้อมูลลูกค้าที่ไม่มีใน <<Cleansing>> แต่มีใน <<Polisy400>>
             * เคสนี้เป็นข้อมูลลูกค้าที่ถูกสร้างตรงๆ ที่<<Polisy400>> ซึ่งข้อมูลนี้จะมีเลข CleansingId ในวันต่อมา 
             * และแค่มีเลข polisyClientId ก็ตอบโจทย์ของ Locus ในการเปิดเคลม
             * ในอนาคตหากจะต้องสร้าง เลข CleansingId ในเคสนี้ ต้องมาคุย Flow กันอีกที
             *Input
             * roleCode = G	
             * cleansingId=nulll	
             * polisyClientId=not null	
             * crmClientId=null
             * 
             *Expected API Action
             * ต้องยิงไปที่
             *  No Action
             *  
             * ต้องเกิดข้อมูลใหม่ที่
             *  None

             */


            /*
             * Expected Output 
             {
              "code": "500",
              "message": "The Service cannot process case:  'cleansingId' null and 'polisyClientId' not null",
              "description": "",
              "transactionId": "00-600913224827219-0000001",
              "transactionDateTime": "2017-09-13T22:48:27.22409+07:00",
              "data": null
            }
             */


            // Arrange
            var controller = new RegClientPersonalController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);

            string input = String.Format(@"
            {{
                'generalHeader':  
                {{
                    'roleCode': 'G',
                    'cleansingId': '',
                    'polisyClientId': '{6}',
                    'crmClientId': '',
                    'clientAdditionalExistFlag': 'N'
                }},'profileInfo': {{
                    'salutation': '0002',
                    'personalName': '{0}',
                    'personalSurname' : '{1}' ,
                    'sex' : 'M',
                    'idCitizen' :'{2}',
                    'idPassport' : '',
                    'idAlien' : '',
                    'idDriving': '',
                    'birthDate' :'2009-04-22',
                    'nationality' : '00203',
                    'language' : '{3}',
                    'married' : '{4}',
                    'occupation' : '00002',
                    'riskLevel' : 'R1',
                    'vipStatus' : '{5}',
                    'remark' : ' test BirthDate'
                }},'contactInfo':{{
                    'telephone1' : '',
                    'telephone1Ext' :'',
                    'telephone2' : '',
                    'telephone2Ext' :'',
                    'telephone3' : '',
                    'telephone3Ext' : '',
                    'mobilePhone' : '',
                    'fax' : '',
                    'emailAddress' :'',
                    'lineID' : '',
                    'facebook' : ''
                }},'addressInfo':{{
                    'address1' : '',
                    'address2' : '',
                    'address3' : '',
                    'subDistrictCode' : '101001',
                    'districtCode' : '1010',
                    'provinceCode' : '10',
                    'postalCode' :'10210',
                    'country' : '00220',
                    'addressType' : '01',
                    'latitude' : '',
                    'longtitude' : ''
                }}
             }}",
                personalName,
                personalSurname,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "M", "W" }),
                RandomValueGenerator.RandomOptions(new[] { "Y", "N" }),
                polisyClientIdForPg2);

            //Assert
            var response = (HttpResponseMessage)controller.Post(JObject.Parse(input));
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);

            var outputJson = JObject.Parse(output?.Result);
            Assert.AreEqual("500", outputJson["code"]?.ToString());
            Assert.AreEqual("The Service cannot process case:  \'cleansingId\' null and \'polisyClientId\' not null", outputJson["message"]?.ToString());
            







        }

        [TestMethod()]
        [Priority(3)]
        public void Post_RegClientPersonal_BusinessTest_PG_03()
        {
            /*Description
             * ข้อมูลลูกค้าที่มีใน <<Cleansing>> แต่ไม่มีใน <<Polisy400>> และ <<CRM>> (เช่น ข้อมูลลูกค้าใน WorryFree)
             *Input
             * roleCode = G	
             * cleansingId=not null	
             * polisyClientId= null	
             * crmClientId=null
             * 
             *Expected API Action
             * ต้องยิงไปที่
             *   - CLIENT_CreatePersonalClientAndAdditionalInfo
             *   - และสร้างข้อมูลใน CRM
             *  
             * ต้องเกิดข้อมูลใหม่ที่
             *  <<Polisy400>>
             *  <<CRM>>

             */


            /*
             * Expected Output 
            
             */


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

            // Arrange
            var controller = new RegClientPersonalController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10);
            var personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10);

            string input = String.Format(@"
            {{
                'generalHeader':  
                {{
                    'roleCode': 'G',
                    'cleansingId': '{6}',
                    'polisyClientId': '',
                    'crmClientId': '',
                    'clientAdditionalExistFlag': 'N'
                }},'profileInfo': {{
                    'salutation': '0002',
                    'personalName': '{0}',
                    'personalSurname' : '{1}' ,
                    'sex' : 'M',
                    'idCitizen' :'{2}',
                    'idPassport' : '',
                    'idAlien' : '',
                    'idDriving': '',
                    'birthDate' :'2009-04-22',
                    'nationality' : '00203',
                    'language' : '{3}',
                    'married' : '{4}',
                    'occupation' : '00002',
                    'riskLevel' : 'R1',
                    'vipStatus' : '{5}',
                    'remark' : ' test BirthDate'
                }},'contactInfo':{{
                    'telephone1' : '',
                    'telephone1Ext' :'',
                    'telephone2' : '',
                    'telephone2Ext' :'',
                    'telephone3' : '',
                    'telephone3Ext' : '',
                    'mobilePhone' : '',
                    'fax' : '',
                    'emailAddress' :'',
                    'lineID' : '',
                    'facebook' : ''
                }},'addressInfo':{{
                    'address1' : '',
                    'address2' : '',
                    'address3' : '',
                    'subDistrictCode' : '101001',
                    'districtCode' : '1010',
                    'provinceCode' : '10',
                    'postalCode' :'10210',
                    'country' : '00220',
                    'addressType' : '01',
                    'latitude' : '',
                    'longtitude' : ''
                }}
             }}",
                personalName,
                personalSurname,
                RandomValueGenerator.RandomNumber(13),
                RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                RandomValueGenerator.RandomOptions(new[] { "M", "W" }),
                RandomValueGenerator.RandomOptions(new[] { "Y", "N" }),
                cleansingId);

            //Assert
            var response = (HttpResponseMessage)controller.Post(JObject.Parse(input));
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);

            var outputJson = JObject.Parse(output?.Result);
            Assert.AreEqual("200", outputJson["code"]?.ToString());

            var outputData = outputJson["data"][0];
            Assert.IsNotNull(outputData);
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["polisyClientId"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ POLISY400");
            Assert.AreEqual(cleansingId, outputData["cleansingId"]?.ToString(), " CLS = input");
            Assert.AreEqual(false, string.IsNullOrEmpty(outputData["crmClientId"]?.ToString()), "ต้องเกิดข้อมูลใหม่ที่ CRM");

            polisyClientIdForPg2 = outputData["polisyClientId"]?.ToString();
            Assert.AreEqual(personalName, outputData["personalName"]?.ToString());
            Assert.AreEqual(personalSurname, outputData["personalSurname"]?.ToString());









        }
    }
}