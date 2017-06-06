using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Core.Util;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegClientCorporate;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class buzCreateCrmClientCorporateTests
    {
        [TestMethod()]
        //ทดสอบถ้าไม่มีเลข cleansingId จะต้อง return error
        public void Execute_buzCreateCrmClientCorporateTest()
        {

            var input = new RegClientCorporateInputModel
            {
                generalHeader = new GeneralHeaderModel
                {
                    roleCode = "P",
                    cleansingId = "",
                    polisyClientId = "",
                    crmClientId = "",
                    clientAdditionalExistFlag = "N"
                },
                profileHeader = new ProfileHeaderModel
                {
                   
                    corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                    corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                 
                    idTax = RandomValueGenerator.RandomNumber(13),
              
                    dateInCorporate = DateTime.Now,
                 
                    language = RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                   
                    riskLevel = "",
                    vipStatus = RandomValueGenerator.RandomOptions(new[] { "Y", "N" })
                  
                },
                contactHeader = new ContactHeaderModel
                {
                    telephone1 = "",
                    telephone1Ext = "",
                    telephone2 = "",
                    telephone2Ext = "",
                    telephone3 = "",
                    telephone3Ext = "",
                    mobilePhone = "",
                    fax = "",
                    emailAddress = "",
                    lineID = "",
                    facebook = ""
                },
                addressHeader = new AddressHeaderModel
                {
                    address1 = "",
                    address2 = "",
                    address3 = "",
                    subDistrictCode = "",
                    districtCode = "",
                    provinceCode = "",
                    postalCode = "",
                    country = "",
                    addressType = "",
                    latitude = "",
                    longtitude = ""
                }
            };
            var cmd = new buzCreateCrmClientCorporate();
            var result = cmd.Execute(input);

            Console.WriteLine("======= Execute result =========");
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
            var modelResult = (CreateCrmCorporateInfoOutputModel)result;
            Assert.AreEqual("500", modelResult.code);

        }
    }
}