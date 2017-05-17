using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.Templates.Tests
{
    [TestClass()]
    public class BaseCommandTests
    {
        [TestMethod()]
        public void SearchCrmContactClientIdTest()
        {
            var testcmd = new TestCommand();
            List<string> lstCrmClientId = testcmd.SearchCrmContactClientId("C2017-100000351");
            Assert.IsNotNull(lstCrmClientId);
            Console.WriteLine(lstCrmClientId.ToJson());
            Assert.AreEqual("201705-0019591", lstCrmClientId[0]);
        }
    }
}