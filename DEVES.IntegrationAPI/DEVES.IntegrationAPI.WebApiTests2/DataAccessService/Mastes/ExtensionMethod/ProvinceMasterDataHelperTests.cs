using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.DataAccessService.Mastes.ExtensionMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.Mastes.ExtensionMethod.Tests
{
    [TestClass()]
    public class ProvinceMasterDataHelperTests
    {
        [TestMethod()]
        public void ToProvinceNameTest()
        {
            Assert.AreEqual("","00".ToProvinceName());
        }
    }
}