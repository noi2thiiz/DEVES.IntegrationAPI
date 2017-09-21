using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class CleansingClientServiceTests
    {
        [TestMethod()]
        public void RemoveByCleansingIdTest()
        {
          
            var result = CleansingClientService.Instance.RemoveByCleansingId("C2017-100000343");
            Console.WriteLine(result);
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.success);
        }

        [TestMethod()]
        public void InquiryPersonalClientTest()
        {
           
            var input = new CLSInquiryPersonalClientInputModel
            {


                personalFullName = "อนันต์",
                idCitizen = "",
                emailAddress = "",
                roleCode = "",

                telephone = "",
                clientId = ""

            };

            var result = CleansingClientService.Instance.InquiryPersonalClient(input);
            Console.WriteLine("=====================result=========================");
            Console.WriteLine(result);
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);

            Assert.AreEqual(true, result.success);
        }

        [TestMethod()]
        public void CreatePersonalClientTest()
        {
            //ทดสอบบน QA เท่านั้น
           
            var input = new CLSCreatePersonalClientInputModel
            {
                roleCode = "G",
                personalName = "ทดสอบสร้างคน",
                personalSurname = "ลองสร้างใหม่",
                salutation  ="0001",
                sex="M"
            };
            

            var result = CleansingClientService.Instance.CreatePersonalClient(input);
            Console.WriteLine("=====================result=========================");
            Console.WriteLine(result);
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);

            Assert.AreEqual(true, result.success);
            
        }
    }
}