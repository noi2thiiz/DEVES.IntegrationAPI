using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Core.Util;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;
using AddressInfoModel = DEVES.IntegrationAPI.Model.RegPayeePersonal.AddressInfoModel;
using ContactInfoModel = DEVES.IntegrationAPI.Model.RegPayeePersonal.ContactInfoModel;

using ProfileInfoModel = DEVES.IntegrationAPI.Model.RegPayeePersonal.ProfileInfoModel;

using ClientGeneralHeaderModel = DEVES.IntegrationAPI.Model.RegClientPersonal.GeneralHeaderModel;
using ClientAddressInfoModel = DEVES.IntegrationAPI.Model.RegClientPersonal.AddressInfoModel;
using ClientContactInfoModel = DEVES.IntegrationAPI.Model.RegClientPersonal.ContactInfoModel;
using ClientProfileInfoModel = DEVES.IntegrationAPI.Model.RegClientPersonal.ProfileInfoModel;

using PayeeGeneralHeaderModel = DEVES.IntegrationAPI.Model.RegPayeePersonal.GeneralHeaderModel;
using PayeeProfileInfoModel = DEVES.IntegrationAPI.Model.RegPayeePersonal.ProfileInfoModel;
using PayeeAddressInfoModel = DEVES.IntegrationAPI.Model.RegPayeePersonal.AddressInfoModel;
using PayeeContactInfoModel = DEVES.IntegrationAPI.Model.RegPayeePersonal.ContactInfoModel;


namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class buzCRMRegPayeePersonalTests
    {
        
        [TestMethod()]
        //ทดสอบสร้าง Payee ใหม่ที่ไม่เคยมีข้อมูลมาก่อน
        // จะต้องเกิดข้อมูลใน CLS ,400 ,SAP และ CRM ด้วย
        public void Execute_buzCRMRegPayeePersonalInput_CreateNewPayee_Test()
        {
            var input = new RegPayeePersonalInputModel
            {
                generalHeader = new PayeeGeneralHeaderModel
                {
                    polisyClientId = "",
                    cleansingId = "",
                    roleCode = "G",
                    clientAdditionalExistFlag = "N"
                },
                contactInfo = new ContactInfoModel
                {
                    
                },
                addressInfo = new AddressInfoModel
                {
                    address1= "TLvVDpigGb",
                    address2= "",
                    address3= "",
                    subDistrictCode= "",
                    districtCode= "",
                    provinceCode= "",
                    postalCode= "00000",
                    country= "00220",
                    addressType= "",
                    latitude= "",
                    longtitude= ""
                },
                profileInfo = new ProfileInfoModel
                {
                    salutation = "0001",
                    personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                    personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                    sex = "U",
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
                sapVendorInfo = new SapVendorInfoModel
                {
                    sapVendorGroupCode ="DIR",
                    bankInfo = new BankInfoModel
                    {
                        bankCountryCode= "",
                        bankCode= "",
                        bankBranchCode= "",
                        bankAccount="",
                        accountHolder="",
                        paymentMethods=""
                    },
                    withHoldingTaxInfo = new WithHoldingTaxInfoModel
                    {
                        receiptType = "03"
                    }
                }


            };

            try
            {
                var cmd = new buzCRMRegPayeePersonal();
                var result = cmd.Execute(input);

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var model = (RegPayeePersonalContentOutputModel)result;
                Assert.AreEqual("200", model.code);
                Assert.AreEqual(true, model.data.Any());

                var payeeData = (RegPayeePersonalDataOutputModel_Pass)model.data[0];
                Assert.AreEqual(true, model.data.Any());
                Assert.AreEqual(false, string.IsNullOrEmpty(payeeData.cleansingId));
                Assert.AreEqual(false, string.IsNullOrEmpty(payeeData.crmClientId));
                Assert.AreEqual(false, string.IsNullOrEmpty(payeeData.polisyClientId));
                Assert.AreEqual(false, string.IsNullOrEmpty(payeeData.sapVendorCode));
                Assert.AreEqual(false, string.IsNullOrEmpty(payeeData.sapVendorGroupCode));
            }
            catch (Exception e)
            {
               
                Assert.Fail(e.Message);
            }

        }

        [TestMethod()]
        // ทดสอบสร้าง Payee จาก Client โดยระบุเลข cleansingId และ polisyClientId
        // จะต้องเกิดข้อมูลใน SAP ไม่เกิดข้อมูลใน CRM ด้วย
        public void Execute_buzCRMRegPayeePersonalInput_ConvertNonePayeeToPayee_Test()
        {
            // create new client
            var inputClient = new RegClientPersonalInputModel
            {
                generalHeader = new ClientGeneralHeaderModel
                {
                    roleCode = "P",
                    cleansingId = "",
                    polisyClientId = "",
                    crmClientId = "",
                    clientAdditionalExistFlag = "N"
                },
                profileInfo = new ClientProfileInfoModel
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
                contactInfo = new ClientContactInfoModel
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
                addressInfo = new ClientAddressInfoModel
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
            var cmdClient = new buzCRMRegClientPersonal();
            var resultClient = (RegClientPersonalContentOutputModel)cmdClient.Execute(inputClient);
            var data = (RegClientPersonalDataOutputModel_Pass)resultClient.data[0];
            var cleansingId = data.cleansingId;
            var polisyClientId = data.polisyClientId;

            //convert client to payee
            var input = new RegPayeePersonalInputModel
            {
                generalHeader = new PayeeGeneralHeaderModel
                {
                    polisyClientId = "" + polisyClientId,
                    cleansingId = ""+cleansingId,
                    roleCode = "G",
                    clientAdditionalExistFlag = "N"
                },
                contactInfo = new PayeeContactInfoModel
                {

                },
                addressInfo = new PayeeAddressInfoModel
                {
                    address1 = "TLvVDpigGb",
                    address2 = "",
                    address3 = "",
                    subDistrictCode = "",
                    districtCode = "",
                    provinceCode = "",
                    postalCode = "00000",
                    country = "00220",
                    addressType = "",
                    latitude = "",
                    longtitude = ""
                },
                profileInfo = new PayeeProfileInfoModel
                {
                    salutation = "0001",
                    personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                    personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                    sex = "U",
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
                sapVendorInfo = new SapVendorInfoModel
                {
                    sapVendorGroupCode = "DIR",
                    bankInfo = new BankInfoModel
                    {
                        bankCountryCode = "",
                        bankCode = "",
                        bankBranchCode = "",
                        bankAccount = "",
                        accountHolder = "",
                        paymentMethods = ""
                    },
                    withHoldingTaxInfo = new WithHoldingTaxInfoModel
                    {
                        receiptType = "03"
                    }
                }


            };
            //Assert
            var cmd = new buzCRMRegPayeePersonal();
            var result = cmd.Execute(input);

            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
            var model = (RegPayeePersonalContentOutputModel)result;
            Assert.AreEqual("200", model.code);
            Assert.AreEqual(true, model.data.Any());
            var payeeData = (RegPayeePersonalDataOutputModel_Pass)model.data[0];
            Assert.AreEqual(true, model.data.Any());
            Assert.AreEqual(false, string.IsNullOrEmpty(payeeData.cleansingId), " cleansingId Should  Not NullOrEmpty"); 
            Assert.AreEqual(true, string.IsNullOrEmpty(payeeData.crmClientId), " crmClientId Should Null"); 
            Assert.AreEqual(polisyClientId, payeeData.polisyClientId, " polisyClientId Should  Not NullOrEmpty");
            Assert.AreEqual(cleansingId, payeeData.cleansingId);
            Assert.AreEqual(false, string.IsNullOrEmpty(payeeData.sapVendorCode), " sapVendorCode Should  Not NullOrEmpty");
            Assert.AreEqual(false, string.IsNullOrEmpty(payeeData.sapVendorGroupCode), " sapVendorGroupCode Should  Not NullOrEmpty");
        }

        [TestMethod()]
        // ทดสอบสร้าง Payee จาก Client โดยระบุเลข  polisyClientId
        // จะต้องเกิดข้อมูลใน SAP แต่ถ้าไม่มีเลข cleansingId ไม่ต้องเข้า CRM ด้วย
        public void Execute_buzCRMRegPayeePersonalInput_ConvertNonePayeeToPayee_When_Give_Only_PolicyClientId_Test()
        {
            // create new client
            var inputClient = new RegClientPersonalInputModel
            {
                generalHeader = new ClientGeneralHeaderModel
                {
                    roleCode = "P",
                    cleansingId = "",
                    polisyClientId = "",
                    crmClientId = "",
                    clientAdditionalExistFlag = "N"
                },
                profileInfo = new ClientProfileInfoModel
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
                contactInfo = new ClientContactInfoModel
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
                addressInfo = new ClientAddressInfoModel
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
            var cmdClient = new buzCRMRegClientPersonal();
            var resultClient = (RegClientPersonalContentOutputModel)cmdClient.Execute(inputClient);
            var data = (RegClientPersonalDataOutputModel_Pass)resultClient.data[0];
            var cleansingId = data.cleansingId;
            var polisyClientId = data.polisyClientId;
          
            //convert client to payee
            var input = new RegPayeePersonalInputModel
            {
                generalHeader = new PayeeGeneralHeaderModel
                {
                    polisyClientId = ""+ polisyClientId,
                    cleansingId = "",
                    roleCode = "G",
                    clientAdditionalExistFlag = "N"
                },
                contactInfo = new PayeeContactInfoModel
                {

                },
                addressInfo = new PayeeAddressInfoModel
                {
                    address1 = "TLvVDpigGb",
                    address2 = "",
                    address3 = "",
                    subDistrictCode = "",
                    districtCode = "",
                    provinceCode = "",
                    postalCode = "00000",
                    country = "00220",
                    addressType = "",
                    latitude = "",
                    longtitude = ""
                },
                profileInfo = new PayeeProfileInfoModel
                {
                    salutation = "0001",
                    personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                    personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                    sex = "U",
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
                sapVendorInfo = new SapVendorInfoModel
                {
                    sapVendorGroupCode = "DIR",
                    bankInfo = new BankInfoModel
                    {
                        bankCountryCode = "",
                        bankCode = "",
                        bankBranchCode = "",
                        bankAccount = "",
                        accountHolder = "",
                        paymentMethods = ""
                    },
                    withHoldingTaxInfo = new WithHoldingTaxInfoModel
                    {
                        receiptType = "03"
                    }
                }


            };
            //Assert
            var cmd = new buzCRMRegPayeePersonal();
            var result = cmd.Execute(input);

            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
            var model = (RegPayeePersonalContentOutputModel)result;
            Assert.AreEqual("200", model.code);
            Assert.AreEqual(true, model.data.Any());
            var payeeData = (RegPayeePersonalDataOutputModel_Pass)model.data[0];
            Assert.AreEqual(true, model.data.Any());
            Assert.AreEqual(true, string.IsNullOrEmpty(payeeData.cleansingId)); // ต้องไม่มีค่า
            Assert.AreEqual(true, string.IsNullOrEmpty(payeeData.crmClientId)); // ต้องไม่มีค่า
            Assert.AreEqual(polisyClientId, payeeData.polisyClientId);
            Assert.AreEqual(false, string.IsNullOrEmpty(payeeData.sapVendorCode));
            Assert.AreEqual(false, string.IsNullOrEmpty(payeeData.sapVendorGroupCode));
        }

    }
}