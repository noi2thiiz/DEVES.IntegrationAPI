using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Core.Util;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class CLIENTCreatePersonalClientAndAdditionalInfoServiceTests
    {
        [TestMethod()]
        public void Execute_CLIENTCreatePersonal_It_should_success_when_give_valid_input()
        {
            try
            {
                var service =
                    new CLIENTCreatePersonalClientAndAdditionalInfoService(Guid.NewGuid().ToString(), "UnitTest");
                var result = service.Execute(new CLIENTCreatePersonalClientAndAdditionalInfoInputModel
                {
                    telephone1 = "123456",
                    telephone2 = "",
                    remark = "",
                    address1 = "123",
                    address2 = "อาคารชัย",
                    specialIndicator = "",
                    address3 = "ห้อง 306",
                    married = "D",
                    staffFlag = "",
                    facebook = "",
                    dtBirthDtate = RandomValueGenerator.RandomDateTime(),
                    telNo = "",
                    natioanality = "004",
                    occupation = "000",
                    fax = "",
                    vipStatus = "N",
                    passportId = "",
                    emailAddress = "",
                    nameFormat = "",
                    sTax = "",
                    country = "764",
                    soe = "",
                    mobilePhone = "086"+ RandomValueGenerator.RandomNumber(7),
                    taxId = "",
                    personalName =   "ทดสอบ" + RandomValueGenerator.RandomString(19),
                    directMail = "",
                    longtitude = "",
                    language = "T",
                    latitude = "",
                    companyDoctor = "",
                    mailing = "",
                    sex = "M",
                    riskLevel = "R1",
                    birthPlace = "",
                    deathDate = "",
                    documentNo = "",
                    oldIDNumber = "",
                    lineId = "",
                    pager = "",
                    idCard = "3469900290301",
                    clientStatus = "",
                    postCode = "10200",
                    busRes = "R",
                    cleansingId = "C2017-100002132",
                    address5 = "กรุงเทพมหานคร",
                    address4 = "แขวงพระบรมมหาราชวัง เขตพระนคร",
                    alientId = "",
                    taxInNumber = "",
                    driverlicense = "",
                    personalSurname = "ทดสอบ" + RandomValueGenerator.RandomString(19),
                    salutation = "0023",
                });

                Console.WriteLine("==================result======================");

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                Assert.IsTrue(!string.IsNullOrEmpty(result.clientID));
            }
            catch (BuzErrorException be)
            {
                Console.WriteLine("==================result BuzErrorException======================");
                Assert.Fail(be.GetOutputModel().ToJson());
            }
            catch (Exception e)
            {
                Console.WriteLine("==================result Exception======================");
                Assert.Fail(e.Message);
            }
        }

        [TestMethod()]
        public void Execute_CLIENTCreatePersonal_It_should_success_when_give_minimum_required_input()
        {
            try
            {
                var service =
                    new CLIENTCreatePersonalClientAndAdditionalInfoService(Guid.NewGuid().ToString(), "UnitTest");
                var result = service.Execute(new CLIENTCreatePersonalClientAndAdditionalInfoInputModel
                {
                    personalName = "ทดสอบ" + RandomValueGenerator.RandomString(10),
                    sex = "M",/*required*/
                    personalSurname = "ทดสอบ"+RandomValueGenerator.RandomString(10),
                    salutation = "0023", /*required*/
                });

                Console.WriteLine("==================result======================");

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                Assert.IsTrue(!string.IsNullOrEmpty(result.clientID));
            }
            catch (BuzErrorException be)
            {
                Console.WriteLine("==================result BuzErrorException======================");
                Assert.Fail(be.GetOutputModel().ToJson());
            }
            catch (Exception e)
            {
                Console.WriteLine("==================result Exception======================");
                Assert.Fail(e.Message);
            }
        }

        [TestMethod()]
        public void Execute_CLIENTCreatePersonal_It_should_return_code_EWI0010E_when_give_empty_json_input()
        {
            try
            {
                var service =
                    new CLIENTCreatePersonalClientAndAdditionalInfoService(Guid.NewGuid().ToString(), "UnitTest");
                var result = service.Execute(new CLIENTCreatePersonalClientAndAdditionalInfoInputModel());

                Console.WriteLine("==================result======================");

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
            }
            catch (BuzErrorException be)
            {
                Console.WriteLine("==================result BuzErrorException======================");
                /*{"Code":"EWI-0010E",
                   "Message":"COMP Error:Field cannot be blank   |Field must be entered   |",
                   "Description":"Error on execute \u0027CLIENT_CreatePersonalClientAndAdditionalInfo\u0027",
                   "SourceError":"COMP",
                   "TransactionId":"51cde2b2-23db-4982-82ab-906c2a1d4145"}*/
                Console.WriteLine(be.GetOutputModel().ToJson());
                Assert.AreEqual("EWI-0010E", be.Code);
                Assert.IsTrue(be.Message.Contains("Field cannot be blank"));

            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
           
           
        }
    }
}