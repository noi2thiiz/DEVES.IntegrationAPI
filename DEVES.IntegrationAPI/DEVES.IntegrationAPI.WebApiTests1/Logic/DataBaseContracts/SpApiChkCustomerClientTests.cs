using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts.Tests
{
    [TestClass()]
    public class SpApiChkCustomerClientTests
    {
        [TestMethod()]
        public void checkByCleansingIdTest()
        {
            var result = SpApiChkCustomerClient.Instance.CheckByCleansingId("C2017-000004582");
            Assert.AreEqual(true,result);
        }
    }
}