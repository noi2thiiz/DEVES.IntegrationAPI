using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Core.Util;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegClientPersonal;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class buzCreateCrmClientPersonalTests
    {
        [TestMethod()]
        //ทดสอบถ้าไม่มีเลข cleansingId จะต้อง return error
        public void Execute_buzCreateCrmClientPersonalTest()
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
                    longtitude = ""
                }
            };
            var cmd = new buzCreateCrmClientPersonal();
            var result = cmd.Execute(input);

            Console.WriteLine("======= Execute result =========");
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
            var modelResult = (CreateCrmPersonInfoOutputModel) result;
            Assert.AreEqual("500", modelResult.code);

        }
    }
}