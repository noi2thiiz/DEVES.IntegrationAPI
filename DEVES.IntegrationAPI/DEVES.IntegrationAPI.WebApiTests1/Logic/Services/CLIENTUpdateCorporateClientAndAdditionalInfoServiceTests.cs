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
    public class CLIENTUpdateCorporateClientAndAdditionalInfoServiceTests
    {
       // [TestMethod()]
        public void Execute_CLIENTUpdateCorporate_It_should_success_when_give_valid_input()
        {
            Assert.Fail();
        }

       // [TestMethod()]
        public void Execute_CLIENTUpdateCorporate__It_should_success_when_give_minimum_required_input()
        {
            /*      Expected Result 
               {
                 "Code": "EWI-1100W",
                 "Message": "COMP Error:Enter at least one      |",
                 "Description": "Error on execute \u0027CLIENT_UpdateCorporateClientAndAdditionalInfo\u0027",
                 "SourceError": "COMP",
                 "TransactionId": "e3b4a32d-15db-4a5d-a3d2-e3fe47f27368"
               }
               */


            try
            {
                var service =
                    new CLIENTUpdateCorporateClientAndAdditionalInfoService(Guid.NewGuid().ToString(), "UnitTest");
                var result = service.Execute(new CLIENTUpdateCorporateClientAndAdditionalInfoInputModel
                {
                    clientNumber = "16972826",
                    telephones = "0927260990",
                    telephone2 = "",
                    remark = null,
                    address1 = "320/49 หมู่ที่ 2",
                    address2 = "",
                    specialIndicator = null,
                    address3 = "",


                    facebook = "",
                    dateInCorporateDate = DateTime.Now,


                    vipStatus = "",
                    passportId = "",
                    emailAddress = "",

                    sTax = null,
                    country = "764",


                    taxId = null,
                    corporateName1 = RandomValueGenerator.RandomString(9),
                    corporateName2 = RandomValueGenerator.RandomString(9),
                    corporateStaffNo = RandomValueGenerator.RandomString(9),
                    countryOrigin = "764",
                    directMail = null,
                    longtitude = "",
                    language = "T",
                    latitude = "",

                    mailing = null,

                    riskLevel = "R1",

                    lineId = "",

                    idCard = RandomValueGenerator.RandomNumber(13),
                    clientStatus = null,
                    postCode = "11110",

                    cleansingId = "",
                    address5 = "จ.นนทบุรี",
                    address4 = "ต.บางบัวทอง อ.บางบัวทอง",
                    alientId = "",
                    taxInNumber = null,
                    driverlicense = "",
                    assessorFlag = "Y"


                }
                );


                Console.WriteLine("==================result======================");

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);




            }
            catch (BuzErrorException be)
            {
                Console.WriteLine("==================result BuzErrorException======================");
                Console.WriteLine(be.GetOutputModel().ToJson());
                Assert.AreEqual("EWI-1100W", be.Code);
                Assert.IsTrue(be.Message.Contains("Enter at least one"));


            }
            catch (Exception e)
            {
                Console.WriteLine("==================result Exception======================");
                Assert.Fail(e.Message);
            }
        }

        [TestMethod()]
        public void Execute_CLIENTUpdateCorporate__It_should_return_code_EW1100W_when_give_empty_json_input()
        {
            /*      Expected Result 
                {
                  "Code": "EWI-1100W",
                  "Message": "COMP Error:Enter at least one      |",
                  "Description": "Error on execute \u0027CLIENT_UpdateCorporateClientAndAdditionalInfo\u0027",
                  "SourceError": "COMP",
                  "TransactionId": "e3b4a32d-15db-4a5d-a3d2-e3fe47f27368"
                }
                */


            try
            {
                var service =
                    new CLIENTUpdateCorporateClientAndAdditionalInfoService(Guid.NewGuid().ToString(), "UnitTest");
                    var result = service.Execute(new CLIENTUpdateCorporateClientAndAdditionalInfoInputModel());
                

                Console.WriteLine("==================result======================");

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);



               
            }
            catch (BuzErrorException be)
            {
                Console.WriteLine("==================result BuzErrorException======================");
                Console.WriteLine(be.GetOutputModel().ToJson());
                Assert.AreEqual("EWI-1100W",be.Code);
                Assert.IsTrue(be.Message.Contains("Enter at least one"));


            }
            catch (Exception e)
            {
                Console.WriteLine("==================result Exception======================");
                Assert.Fail(e.Message);
            }
        }
    }
}