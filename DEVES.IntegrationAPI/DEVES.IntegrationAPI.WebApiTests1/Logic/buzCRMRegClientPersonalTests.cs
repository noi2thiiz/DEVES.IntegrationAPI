using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.Core.Util;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegClientPersonal;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class buzCRMRegClientPersonalTests
    {
        [TestMethod()]
        public void Execute_buzCRMRegClientPersonalTest()
        {
         
            for (var i = 0; i <= 5; i++)
            {
                var input = new RegClientPersonalInputModel
                {
                    generalHeader = new GeneralHeaderModel
                    {
                        roleCode = "P",
                        cleansingId = "",
                        polisyClientId = "",
                        crmClientId = "",
                        clientAdditionalExistFlag = "N"
                    },
                    profileInfo = new ProfileInfoModel
                    {
                        salutation = "0002",
                        personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                        personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                        sex = "M",
                        idCitizen = RandomValueGenerator.RandomNumber(13),
                        idPassport = "",
                        idAlien = "",
                        idDriving = "",
                        birthDate = DateTime.Now,
                        nationality = "",
                        language = RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                        married = RandomValueGenerator.RandomOptions(new[] { "M", "W" }),
                        occupation = "",
                        riskLevel = "",
                        vipStatus = RandomValueGenerator.RandomOptions(new[] { "Y", "N" }),
                        remark = ""
                    },
                    contactInfo = new ContactInfoModel
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
                    addressInfo = new AddressInfoModel
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
                        longitude = ""
                    }
                };
                var cmd = new buzCRMRegClientPersonal();
                var result = (RegClientPersonalContentOutputModel)cmd.Execute(input);
                Console.WriteLine("==========result================");
                Console.WriteLine(result.ToJson());
                Assert.AreEqual("200", result.code);
                Assert.AreEqual(true, result.data.Any());
                var data = (RegClientPersonalDataOutputModel_Pass)result.data[0];
                Assert.AreEqual(false, string.IsNullOrEmpty(data.cleansingId));
                Assert.AreEqual(false, string.IsNullOrEmpty(data.crmClientId));
                Assert.AreEqual(input.profileInfo.personalName, data.personalName);
                Assert.AreEqual(input.profileInfo.personalSurname, data.personalSurname);
            }
            



        }

        [TestMethod()]
        public void Execute_buzCRMRegClientPersonal_MasterDataValidationTest()
        {
          

            var input = new RegClientPersonalInputModel
            {
                generalHeader = new GeneralHeaderModel
                {
                    roleCode = "P",
                    cleansingId = "",
                    polisyClientId = "",
                    crmClientId = "",
                    clientAdditionalExistFlag = "N"
                },
                profileInfo = new ProfileInfoModel
                {
                    salutation = "0002",
                    personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                    personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                    sex = "M",
                    idCitizen = RandomValueGenerator.RandomNumber(13),
                    idPassport = "",
                    idAlien = "",
                    idDriving = "",
                    birthDate = DateTime.Now,
                    nationality = "99999",
                    language = RandomValueGenerator.RandomOptions(new[] { "E", "T" }),
                    married = RandomValueGenerator.RandomOptions(new[] { "M", "W" }),
                    occupation = "",
                    riskLevel = "",
                    vipStatus = RandomValueGenerator.RandomOptions(new[] { "Y", "N" }),
                    remark = ""
                },
                contactInfo = new ContactInfoModel
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
                addressInfo = new AddressInfoModel
                {
                    address1 = "",
                    address2 = "",
                    address3 = "",
                    subDistrictCode = "999999",
                    districtCode = "9999",
                    provinceCode = "99",
                    postalCode = "99999",
                    country = "99999",
                    addressType = "99",
                    latitude = "",
                    longitude = ""
                }
            };
            var cmd = new buzCRMRegClientPersonal();
                var result = cmd.Execute(input);
                Console.WriteLine("==========result================");
                Console.WriteLine(result.ToJson());
                 var model = (OutputModelFail)cmd.Execute(input);
                Assert.AreEqual("400", model.code);
                Assert.AreEqual(true, model.data.fieldErrors.Any());
               
            
        }
    }
}