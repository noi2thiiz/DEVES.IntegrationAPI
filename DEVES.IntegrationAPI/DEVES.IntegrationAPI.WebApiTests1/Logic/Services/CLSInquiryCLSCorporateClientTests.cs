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
    public class CLSInquiryCLSCorporateClientTests
    {
        public CLSInquiryCLSCorporateClient service { get; set; }

        [TestMethod()]
        public void CLSInquiryCLSCorporateClientTest()
        {
           
            service = new CLSInquiryCLSCorporateClient();
            Assert.IsNotNull(service);
        }

        [TestMethod()]
        public void Execute_CLSInquiryCLSCorporateClientTest()
        {
           
            service = new CLSInquiryCLSCorporateClient();
            var result = service.Execute(new CLSInquiryCorporateClientInputModel
            {
                corporateFullName="เทเวศ"
            });
            Console.WriteLine("==================result================");
            Console.WriteLine(result.ToJson());
            Assert.AreEqual(true,result.data.Any());
        }
    }
}