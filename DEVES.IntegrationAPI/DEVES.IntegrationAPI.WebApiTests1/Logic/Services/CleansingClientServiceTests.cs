using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class CleansingClientServiceTests
    {
        [TestMethod()]
        public void RemoveByCleansingIdTest()
        {
            AppConfig.Instance.StartupForUnitTest();
            var result = CleansingClientService.Instance.RemoveByCleansingId("C2017-100000343");
            Console.WriteLine(result);
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
            Assert.AreEqual(true,result.success);
        }
    }
}