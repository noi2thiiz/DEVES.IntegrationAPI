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
    public class DistrictRepositoryTests
    {
        [TestMethod()]
        public void FindTest()
        {
            var district = DistrictRepository.Instance.Find("1002");
            Assert.IsNotNull(district);
            Assert.AreEqual("1002", district.DistrictCode);
            Assert.AreEqual("ดุสิต", district.DistrictName);
        }
    }
}