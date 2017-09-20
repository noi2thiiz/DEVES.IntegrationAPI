using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Core.Util;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class CLSCreatePersonalServiceTests
    {
        [TestMethod()]
        public void Execute_CLSCreatePersonalService_It_should_success_when_give_valid_input()
        {
            /*      Expected Result 
                  {
                     "success": true,
                     "code": "200",
                     "message": "execute sucessfully",
                     "description": "",
                     "transactionId": "52486",
                     "transactionDateTime": "2017-09-08 16:31:28",
                     "data": {
                       "cleansingId": "C2017-100002132",
                        ...
                        ...
                     }
                   }
                  */


            try
            {
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

                Console.WriteLine("==================result======================");

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);



                Assert.AreEqual("200", result.code);
                Assert.AreEqual(true, result.success);
                Assert.AreEqual(false, string.IsNullOrEmpty(result.data.cleansingId), " cleansingId should not null");
            }
            catch (BuzErrorException be)
            {
                Console.WriteLine("==================result BuzErrorException======================");
                Console.WriteLine(be.GetOutputModel().ToJson());
                Assert.Fail(be.Message);


            }
            catch (Exception e)
            {
                Console.WriteLine("==================result Exception======================");
                Assert.Fail(e.Message);
            }
        }

        [TestMethod()]
        public void Execute_CLSCreatePersonalService_It_should_success_when_give_minimum_required_input()
        {
            /*      Expected Result 
                 {
                    "success": true,
                    "code": "200",
                    "message": "execute sucessfully",
                    "description": "",
                    "transactionId": "52486",
                    "transactionDateTime": "2017-09-08 16:31:28",
                    "data": {
                      "cleansingId": "C2017-100002132",
                       ...
                       ...
                    }
                  }
                 */


            try
            {
                var service =
                    new CLSCreatePersonalService(Guid.NewGuid().ToString(), "UnitTest");
                var result = service.Execute(new CLSCreatePersonalClientInputModel
                {
                    roleCode = "G",
                    clientId = "",
                    crmPersonId = "",
                    isPayee = "Y",
                    salutation = "0023",
                    personalName = "ทดสอบ"+RandomValueGenerator.RandomNumber(10),
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
                    telephone1 =  "02" + RandomValueGenerator.RandomNumber(8),
                    telephone1Ext = "",
                    telephone2 = "",
                    telephone2Ext = "",
                    telNo = "",
                    telNoExt = "",
                    mobilePhone = "086"  + RandomValueGenerator.RandomNumber(7),
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
                    latitude = "",
                    longitude = "",
                });

                Console.WriteLine("==================result======================");

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);



                Assert.AreEqual("200", result.code);
                Assert.AreEqual(true, result.success);
                Assert.AreEqual(false,string.IsNullOrEmpty(result.data.cleansingId), " cleansingId should not null");
            }
            catch (BuzErrorException be)
            {
                Console.WriteLine("==================result BuzErrorException======================");
                Console.WriteLine(be.GetOutputModel().ToJson());
                Assert.Fail(be.Message);


            }
            catch (Exception e)
            {
                Console.WriteLine("==================result Exception======================");
                Assert.Fail(e.Message);
            }
        }

        [TestMethod()]
        public void Execute_CLSCreatePersonalService_It_should_return_code_CLS1111_when_give_empty_json_input()
        {

            /*      Expected Result 
                 {     
                      "code": "CLS-1111",
                      "message": "salutation Mandatory/Required",
                      "description": "",
                      "transactionId": "52904",
                      "transactionDateTime": "2017-09-09 17:28:40",
                      "success": false,
                      "data": null
                    }
                 */


            try
            {
                var service =
                    new CLSCreatePersonalService(Guid.NewGuid().ToString(), "UnitTest");
                var result = service.Execute(new CLSCreatePersonalClientInputModel());

                Console.WriteLine("==================result======================");

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);

                

                Assert.AreEqual("CLS-1111", result.code);
                Assert.AreEqual(false, result.success);
            }
            catch (BuzErrorException be)
            {
                Console.WriteLine("==================result BuzErrorException======================");
                Console.WriteLine(be.GetOutputModel().ToJson());
                Assert.Fail(be.Message);


            }
            catch (Exception e)
            {
                Console.WriteLine("==================result Exception======================");
                Assert.Fail(e.Message);
            }
        }
    }
}