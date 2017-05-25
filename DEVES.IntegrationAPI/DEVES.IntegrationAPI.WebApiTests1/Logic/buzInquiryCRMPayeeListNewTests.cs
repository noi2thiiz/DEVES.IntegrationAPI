using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class buzInquiryCRMPayeeListNewTests
    {
        [TestMethod()]
        public void ExecuteInquiryAPARPayeeListTest()
        {
            AppConfig.Instance.StartupForUnitTest();

            var cmd = new buzInquiryCRMPayeeListNew();
            var result = cmd.InquiryAPARPayeeList(new InquiryCRMPayeeListInputModel
            {
                requester = "MC",
                fullname = "พรชัย"
            });
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void ExecuteInputTest()
        {
            AppConfig.Instance.StartupForUnitTest();

            var cmd = new buzInquiryCRMPayeeListNew();
            var result = cmd.ExecuteInput(new InquiryCRMPayeeListInputModel
            {
                roleCode = "G",
                clientType = "P",
                requester = "MC",
                fullname = "พรชัย"
            });

            Console.WriteLine("===========Execute Output===============");
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
            
        }
    }
}