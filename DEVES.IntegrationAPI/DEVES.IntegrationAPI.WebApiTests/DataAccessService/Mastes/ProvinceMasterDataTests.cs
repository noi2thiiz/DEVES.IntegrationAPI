using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData.Tests
{
    [TestClass()]
    public class ProvinceMasterDataTests
    {
        [TestMethod()]
        public void GetNameWithPrefixForBkkTest()
        {
            var result = ProvinceMasterData.Instance.FindByCode("10");
            Assert.AreEqual("กรุงเทพมหานคร", ProvinceMasterData.Instance.GetNameWithPrefix(result));

          

        }

        [TestMethod()]
        public void GetNameWithPrefixForNotBkkTest()
        {
            var result = ProvinceMasterData.Instance.FindByCode("11");
            Assert.AreEqual("จ.สมุทรปราการ", ProvinceMasterData.Instance.GetNameWithPrefix(result));
        }

        [TestMethod()]
        public void GetNameWithPrefixForDefaultTest()
        {
            var result = ProvinceMasterData.Instance.FindByCode("00");
            Assert.AreEqual("", ProvinceMasterData.Instance.GetNameWithPrefix(result));
        }
    }
}