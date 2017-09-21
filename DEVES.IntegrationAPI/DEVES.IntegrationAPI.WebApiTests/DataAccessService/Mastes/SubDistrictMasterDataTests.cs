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
    public class SubDistrictMasterDataTests
    {
        [TestMethod()]
        public void GetNameWithPrefixForBkkTest()
        {
            var result = SubDistrictMasterData.Instance.FindByCode("100103");
            Assert.AreEqual("แขวงวัดราชบพิธ", SubDistrictMasterData.Instance.GetNameWithPrefix(result));

        }

        [TestMethod()]
        public void GetNameWithPrefixForNotBkkTest()
        {
            var result2 = SubDistrictMasterData.Instance.FindByCode("961301");
            Assert.AreEqual("ต.จวบ", SubDistrictMasterData.Instance.GetNameWithPrefix(result2));
        }

        [TestMethod()]
        public void GetNameWithPrefixForDefaultTest()
        {
            var result2 = SubDistrictMasterData.Instance.FindByCode("000000");
            Assert.AreEqual("", SubDistrictMasterData.Instance.GetNameWithPrefix(result2));
        }
    }
}