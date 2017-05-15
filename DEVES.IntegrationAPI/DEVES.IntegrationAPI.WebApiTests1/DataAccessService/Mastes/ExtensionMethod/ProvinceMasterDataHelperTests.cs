using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.DataAccessService.Mastes.ExtensionMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.Mastes.ExtensionMethod.Tests
{
    [TestClass()]
    public class ProvinceMasterDataHelperTests
    {
        [TestMethod()]
        public void ToProvinceNameTest()
        {
            AppConfig.Instance.StartupForUnitTest();
            ProvinceMasterData.Instance.InitData();
            
            Assert.AreEqual("กรุงเทพมหานคร", ("10").ToProvinceName());

            Assert.AreEqual("กรุงเทพมหานคร", ("10").ToProvinceNameWithLongPrefix());

            Assert.AreEqual("กรุงเทพมหานคร", ("10").ToProvinceNameWithShortPrefix());


            Assert.AreEqual("ขอนแก่น", ("40").ToProvinceName());

            Assert.AreEqual("จังหวัดขอนแก่น", ("40").ToProvinceNameWithLongPrefix());

            Assert.AreEqual("จ.ขอนแก่น", ("40").ToProvinceNameWithShortPrefix());

            Assert.AreEqual("", ("00").ToProvinceNameWithShortPrefix());

            Assert.AreEqual("", ("121212").ToProvinceNameWithShortPrefix());
        }
    }
}