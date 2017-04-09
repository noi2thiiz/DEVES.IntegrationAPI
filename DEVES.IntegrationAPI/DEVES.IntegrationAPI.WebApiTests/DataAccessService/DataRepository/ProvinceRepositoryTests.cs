using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.DataRepository.Tests
{
    [TestClass()]
    public class ProvinceRepositoryTests
    {
        [TestMethod()]
        public void FindTest()
        {
            var province = ProvinceRepository.Instance.Find("10");
            Assert.IsNotNull(province);
            Assert.AreEqual("10",province.ProvinceCode);
        }
    }
}