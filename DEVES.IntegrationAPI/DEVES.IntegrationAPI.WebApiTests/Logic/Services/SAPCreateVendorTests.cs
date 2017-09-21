using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Core.Util;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.Model.SAP;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class SAPCreateVendorTests
    {
        [TestMethod()]
        public void Execute_SAPCreateVendor_It_should_success_when_give_valid_input()
        {
            #region create new client 

            var inputClient = new RegClientPersonalInputModel
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
            var cmdClient = new buzCRMRegClientPersonal();
            var resultClient = (RegClientPersonalContentOutputModel)cmdClient.Execute(inputClient);
            var data = (RegClientPersonalDataOutputModel_Pass)resultClient.data[0];
            var cleansingId = data.cleansingId;
            var polisyClientId = data.polisyClientId;

            #endregion create new client 


            // create sap
            var service = new SAPCreateVendor("","");
            var result = service.Execute(new SAPCreateVendorInputModel
            {
                VCODE = polisyClientId,
                VGROUP = "DIR",
                COMPANY = "2020",
                TITLE = "คุณ",
                NAME1 = data.personalName+" "+data.personalSurname,
                NAME2 = "",
                SEARCH = data.personalName,
                STREET1 = "123 อาคารชัย",
                STREET2 = "ห้อง 306",
                DISTRICT = "แขวงพระบรมมหาราชวัง เขตพระนคร",
                CITY = "กรุงเทพมหานคร",
                POSTCODE = "10200",
                COUNTRY = "TH",
                TEL1 = "123456",
                TEL2 = "0865557013",
                FAX = "",
                TAX1 = "",
                TAX2 = "",
                TAX3 = "3469900290301",
                TAX4 = "",
                CTRY = "TH",
                BANKCODE = "002",
                BANKBRANCH = "0312",
                BANKACC = "3332221234",
                ACCTHOLDER = "ต้น",
                PAYMETHOD = "C",
                WHTCTRY = "TH",
            });
            Console.WriteLine("==============result===============");
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);

            Assert.AreEqual(polisyClientId, result.VCODE,"เลข VCODE ตรงกัน");// แต่มีบาง GROUP ให้เลขไม่ตรง
            Assert.AreEqual($"Vendor {polisyClientId} was created in company code 2020", result.Message);
           

        }
    }
}