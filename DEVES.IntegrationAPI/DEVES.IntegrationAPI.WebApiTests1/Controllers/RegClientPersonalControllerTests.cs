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
using DEVES.IntegrationAPI.Model.RegClientPersonal;
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

                Assert.AreEqual("Error:EWI-0010E, Message:Salut Sex not = Clnt Sex|", outputJson["message"]?.ToString());

            }



        }
    }
}