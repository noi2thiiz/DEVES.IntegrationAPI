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
    public class DistricMasterDataTests
    {
        [TestMethod()]
        public void GetNameWithPrefixForBkkTest()
        {
            var result = DistricMasterData.Instance.FindByCode("1001");
            Assert.AreEqual("เขตพระนคร", DistricMasterData.Instance.GetNameWithPrefix(result));


        }

        [TestMethod()]
        public void GetNameWithPrefixForNotBkkTest()
        {
            var result2 = DistricMasterData.Instance.FindByCode("8407");
            Assert.AreEqual("อ.ท่าชนะ", DistricMasterData.Instance.GetNameWithPrefix(result2));
        }

        [TestMethod()]
        public void GetNameWithPrefixForDefaultTest()
        {
            var result2 = DistricMasterData.Instance.FindByCode("0000");
            Assert.AreEqual("", DistricMasterData.Instance.GetNameWithPrefix(result2));
        }
    }
}