using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class MotorInquiryAparPayeeListTests
    {
        [TestMethod()]
        public void Execute_MotorInquiryAparPayeeListTest()
        {
            AppConfig.Instance.StartupForUnitTest();
            var result = MotorInquiryAparPayeeList.Instance.Execute(new InquiryAPARPayeeListInputModel
            {
                fullName = "พรชัย",
                requester = "MC",

            });
           Console.WriteLine(result.ToJson());
            Assert.AreEqual(true,result.aparPayeeListCollection.Any());
        }

        [TestMethod()]
        public void SearchByVendorCode()
        {
            AppConfig.Instance.StartupForUnitTest();
            var result = MotorInquiryAparPayeeList.Instance.Execute(new InquiryAPARPayeeListInputModel
            {
                polisyClntnum = "16161629",
                requester = "MC",

            });

            Assert.AreEqual(true, result.aparPayeeListCollection.Any());
        }


        [TestMethod()]
        public void SearchByTax()
        {
            AppConfig.Instance.StartupForUnitTest();
            var result = MotorInquiryAparPayeeList.Instance.Execute(new InquiryAPARPayeeListInputModel
            {
                taxNo = "3100800445795",
                taxBranchCode = "",
                requester = "MC",

            });

            Assert.AreEqual(true, result.aparPayeeListCollection.Any());
        }
    }
}