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
    public class testConfigTests
    {
        [TestMethod()]
        public void GetCommBackDayKeyTest()
        {
            var test = new TestAppConst();
            Assert.AreEqual("7", test.GetCommBackDayKey());
        }
    }
}