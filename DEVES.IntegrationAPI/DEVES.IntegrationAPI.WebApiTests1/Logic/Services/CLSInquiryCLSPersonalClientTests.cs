using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.CLS;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class CLSInquiryCLSPersonalClientTests
    {


        [TestMethod()]
        public void Execute_CLSInquiryCLSPersonalClientTest()
        {

            var service = new CLSInquiryCLSPersonalClient();
            var result = service.Execute(new CLSInquiryPersonalClientInputModel
            {
                personalFullName = "พรชัย"
            });
            Console.WriteLine("==================result================");
            Console.WriteLine(result.ToJson());
            Assert.AreEqual(true, result.data.Any());
        }

        [TestMethod()]
        //ทดสอบ salutl ต้องเป็นระบบ MaterCode
        public void Execute_CLSInquiryCLSPersonalClient_salutl_Should_InFormat_400Code_Test()
        {
            var service = new CLSInquiryCLSPersonalClient();
            var result = service.Execute(new CLSInquiryPersonalClientInputModel
            {
                 clientId = "16962833",
                 roleCode= "G"
            });
            Console.WriteLine("==================result================");
            Console.WriteLine(result.ToJson());
            Assert.AreEqual(true, result.data.Any());
            
            Assert.AreEqual("0001", result.data[0].salutl);
        }
    }
}