using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Core.Util;
using DEVES.IntegrationAPI.Model.CLS;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class CLSCreateCorporateClientTests
    {

        [TestMethod()]
        public void Execute_CLSCreateCorporateClientTest_It_should_success_when_give_valid_input()
        {
            var service = new CLSCreateCorporateClientService(Guid.NewGuid().ToString(), "UnitTest");
            var input = new CLSCreateCorporateClientInputModel
            {
                cleansingId = null,
                clientId = "",
                roleCode = "G",
                crmPersonId = "",
                isPayee = null,
                corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                corporateName2 = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                contactPerson = "",
                idRegCorp = "9114027749590",
                dateInCorporate = RandomValueGenerator.RandomDateTime(),
                corporateBranch = "0000",
                idTax = "9114027749590",
                corporateStaffNo = "0000",
                econActivity = "",
                language = "T",
                vipStatus = "",
                telephone1 = "",
                telephone1Ext = "",
                telephone2 = "",
                telephone2Ext = "",
                telNo = "",
                telNoExt = "",
                mobilePhone = "",
                fax = "",
                emailAddress = "",
                lineID = "",
                facebook = "",
                address1 = "84/3 หมู่ 2",
                address2 = "ถ.ปู่เจ้าสมิงพราย",
                address3 = "",
                subDistrictCode = "110407",
                districtCode = "1104",
                provinceCode = "11",
                postalCode = "10130",
                country = "764",
                addressType = "03",
                latitude = "",
                longitude = "",
                OregNum = null,
                DelistFlag = null,
                BlackListFlag = null,
                TerminateDate = RandomValueGenerator.RandomDateTime(),
            };
            var result = service.Execute(input);
            Assert.IsNotNull(result);

            Console.WriteLine("==================result======================");
            /*
             {
                  "code": "200",
                  "message": "execute sucessfully",
                  "description": "",
                  "transactionId": "52793",
                  "transactionDateTime": "2017-09-09 12:01:08",
                  "success": true,
                  "data": {
                    "cleansingId": "C2017-100002242",
                    "clientId": null,
                    "roleCode": "G",
                    "corporateName1": "ทดสอบPHYQQNNR3P",
                    "corporateName2": "ทดสอบFECR2IYF5O",
                    "corporateFullName": "ทดสอบPHYQQNNR3P ทดสอบFECR2IYF5O",
                    "sex": "",
                    "contactPerson": "",
                    "idRegCorp": "9114027749590",
                    "idTax": "9114027749590",
                    "dateInCorporate": "2017-08-03 05:01:36",
                    "corporateStaffNo": "0000",
                    "econActivity": "",
                    "idPassport": "",
                    "idAlien": "",
                    "idDriving": "",
                    "natioanality": "",
                    "language": "T",
                    "married": "",
                    "vipStatus": "",
                    "cltPhone01": "",
                    "cltPhone02": "",
                    "fax": "",
                    "emailAddress": "",
                    "telephone1": "",
                    "telephone1Ext": "",
                    "telephone2Ext": "",
                    "telNo": "",
                    "telNoExt": "",
                    "mobilePhone": "",
                    "lineID": "",
                    "facebook": "",
                    "address1": "84/3 หมู่ 2",
                    "address2": "ถ.ปู่เจ้าสมิงพราย",
                    "address3": "",
                    "subDistrictCode": "110407",
                    "districtCode": "1104",
                    "provinceCode": "11",
                    "postalCode": "10130",
                    "country": "764",
                    "addressType": "03",
                    "latitude": "",
                    "longigude": "",
                    "OregNum": "",
                    "DelistFlag": "",
                    "BlackListFlag": "",
                    "TerminateDate": null
                  }
            */
            Console.WriteLine(result.ToJson());

            Assert.AreEqual("200", result.code);
            Assert.AreEqual(true, result.success);
            Assert.AreNotEqual("", result?.transactionId?.ToString() ?? "");
            Assert.AreNotEqual("", result?.transactionDateTime.ToString(CultureInfo.InvariantCulture));

            Assert.IsNotNull(result.data);
            Assert.AreEqual(input.corporateName1, result.data?.corporateName1, "corporateName1");
            Assert.AreEqual(input.corporateName2, result.data?.corporateName2, "corporateName2");
            Assert.AreEqual(input.corporateStaffNo, result.data?.corporateStaffNo, "corporateStaffNo");



        }


        [TestMethod()]
        public void Execute_CLSCreateCorporateClientTest_It_should_success_when_give_minimum_required_input()
        {
            var service = new CLSCreateCorporateClientService(Guid.NewGuid().ToString(), "UnitTest");
            var input = new CLSCreateCorporateClientInputModel
            {
                cleansingId = null,
                clientId = "",
                roleCode = "G", /*required*/
                crmPersonId = "",
                isPayee = null,
                corporateName1 = "ทดสอบ" + RandomValueGenerator.RandomString(10), /*required*/
                corporateName2 = "",
                contactPerson = "",
                idRegCorp = "",
                dateInCorporate = null,
                corporateBranch = "",
                idTax = "",
                corporateStaffNo = "00000", /*required*/
                econActivity = "",
                language = "T",
                vipStatus = "",
                telephone1 = "",
                telephone1Ext = "",
                telephone2 = "",
                telephone2Ext = "",
                telNo = "",
                telNoExt = "",
                mobilePhone = "",
                fax = "",
                emailAddress = "",
                lineID = "",
                facebook = "",
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
                longitude = "",
                OregNum = null,
                DelistFlag = null,
                BlackListFlag = null,
                TerminateDate = null,
            };
            var result = service.Execute(input);
            Assert.IsNotNull(result);

            Console.WriteLine("==================result======================");
            /*
             {
                  "code": "200",
                  "message": "execute sucessfully",
                  "description": "",
                  "transactionId": "52793",
                  "transactionDateTime": "2017-09-09 12:01:08",
                  "success": true,
                  "data": {
                    "cleansingId": "C2017-100002242",
                    "clientId": null,
                    "roleCode": "G",
                    "corporateName1": "ทดสอบPHYQQNNR3P",
                    "corporateName2": "ทดสอบFECR2IYF5O",
                    "corporateFullName": "ทดสอบPHYQQNNR3P ทดสอบFECR2IYF5O",
                    "sex": "",
                    "contactPerson": "",
                    "idRegCorp": "9114027749590",
                    "idTax": "9114027749590",
                    "dateInCorporate": "2017-08-03 05:01:36",
                    "corporateStaffNo": "0000",
                    "econActivity": "",
                    "idPassport": "",
                    "idAlien": "",
                    "idDriving": "",
                    "natioanality": "",
                    "language": "T",
                    "married": "",
                    "vipStatus": "",
                    "cltPhone01": "",
                    "cltPhone02": "",
                    "fax": "",
                    "emailAddress": "",
                    "telephone1": "",
                    "telephone1Ext": "",
                    "telephone2Ext": "",
                    "telNo": "",
                    "telNoExt": "",
                    "mobilePhone": "",
                    "lineID": "",
                    "facebook": "",
                    "address1": "84/3 หมู่ 2",
                    "address2": "ถ.ปู่เจ้าสมิงพราย",
                    "address3": "",
                    "subDistrictCode": "110407",
                    "districtCode": "1104",
                    "provinceCode": "11",
                    "postalCode": "10130",
                    "country": "764",
                    "addressType": "03",
                    "latitude": "",
                    "longitude": "",
                    "OregNum": "",
                    "DelistFlag": "",
                    "BlackListFlag": "",
                    "TerminateDate": null
                  }
            */
            Console.WriteLine(result.ToJson());

            Assert.AreEqual("200", result.code);
            Assert.AreEqual(true, result.success);
            Assert.AreNotEqual("", result?.transactionId?.ToString() ?? "");
            Assert.AreNotEqual("", result?.transactionDateTime.ToString(CultureInfo.InvariantCulture));

            Assert.IsNotNull(result.data);
            Assert.AreEqual(input.corporateName1, result.data?.corporateName1, "corporateName1");
            Assert.AreEqual(input.corporateName2, result.data?.corporateName2, "corporateName2");
            Assert.AreEqual(input.corporateStaffNo, result.data?.corporateStaffNo, "corporateStaffNo");



        }

        [TestMethod()]
        public void Execute_CLSCreateCorporateClientTest_It_should_return_fail_when_give_empty_json_input()
        {
            var service = new CLSCreateCorporateClientService(Guid.NewGuid().ToString(), "UnitTest");
            var result = service.Execute(new CLSCreateCorporateClientInputModel
            {

            });
            Assert.IsNotNull(result);

          Console.WriteLine("==================result======================");
            /*
             {
                "code": "CLS-1111",
                "message": "roleCode Mandatory/Required",
                "description": "",
                "transactionId": "52791",
                "transactionDateTime": "2017-09-09 11:35:39",
                "success": false,
                "data": null
              }
            */
            Console.WriteLine(result.ToJson());

            Assert.AreEqual("CLS-1111", result.code);
            Assert.AreEqual(false, result.success);
            Assert.IsNull(result.data);
            Assert.AreNotEqual("",result?.transactionId?.ToString()??"");
            Assert.AreNotEqual("",result?.transactionDateTime.ToString(CultureInfo.InvariantCulture));
        }
    }
}