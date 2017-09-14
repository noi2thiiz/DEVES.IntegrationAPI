using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Core.Util;
using DEVES.IntegrationAPI.Model.Polisy400;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class CLIENTCreateCorporateClientAndAdditionalInfoTests
    {
        [TestMethod()]
        public string Execute_CLIENTCreateCorporateClientAndAdditionalInfoTest()
        {
            var service = new CLIENTCreateCorporateClientAndAdditionalInfoService("");
            var result = service.Execute(new CLIENTCreateCorporateClientAndAdditionalInfoInputModel
            {
                telephones = "0927260990",
                telephone2 = "",
                remark = null,
                address1 = "320/49 หมู่ที่ 2",
                address2 = "",
                specialIndicator = null,
                address3 = "",
                
             
                facebook = "",
                dateInCorporateDate =DateTime.Now,
          
                
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
           
              
            });

            Assert.IsNotNull(result);
          
            Console.WriteLine("=================result=================");
            Console.WriteLine(result.ToJson());

            Assert.AreEqual(false, string.IsNullOrEmpty(result.clientID), "clientID should not null");

            return result.clientID;
        }
    }
}