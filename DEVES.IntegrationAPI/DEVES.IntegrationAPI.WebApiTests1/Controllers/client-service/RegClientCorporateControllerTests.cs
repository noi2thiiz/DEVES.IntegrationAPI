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
        public JToken Post_CreateRegClientCorporate_Create_GeneralClient_Test()
        {

            // Arrange
            var controller = new RegClientCorporateController();
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


            return outputData;


        }

    }
}