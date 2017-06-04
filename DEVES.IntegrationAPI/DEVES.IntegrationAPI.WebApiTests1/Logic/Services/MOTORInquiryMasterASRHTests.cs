using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.MASTER;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class MOTORInquiryMasterASRHTests
    {
        [TestMethod()]
        public void MOTORInquiryMasterASRHTest()
        {
            
            var service = new MOTORInquiryMasterASRH();
            Assert.IsNotNull(service);
        }

        [TestMethod()]
        public void Execute_MOTORInquiryMasterASRHTest()
        {
            
            var service = new MOTORInquiryMasterASRH();
            var result = service.Execute(new InquiryMasterASRHDataInputModel
            {
                fullName = "กรุงธน",
                asrhType = "H"
            
            });
            Console.WriteLine("==================result================");
            Console.WriteLine(result.ToJson());
            Assert.AreEqual(true, result?.ASRHListCollection?.Any());
        }
    }
}