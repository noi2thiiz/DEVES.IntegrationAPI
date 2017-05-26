using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.SAP;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class SAPInquiryVendorTests
    {
        [TestMethod()]
        public void ExecuteSAPInquiryVendorTest()
        {
            AppConfig.Instance.StartupForUnitTest();
            var result = SAPInquiryVendor.Instance.Execute(new SAPInquiryVendorInputModel
            {
                PREVACC = "14645096"
            });
            Assert.AreEqual(true, result.VendorInfo.Any());
        }

        [TestMethod()]
        public void SAPInquiryVendorTest()
        {
            AppConfig.Instance.StartupForUnitTest();
            var service = new SAPInquiryVendor();
            Assert.IsNotNull(service);
        }

        [TestMethod()]
        public void ExecuteTest()
        {
            AppConfig.Instance.StartupForUnitTest();
            var service = new SAPInquiryVendor();
            var result = service.Execute(new SAPInquiryVendorInputModel
            {
                PREVACC = "14645096"
            });
            Assert.AreEqual(true, result.VendorInfo.Any());
        }
    }
}