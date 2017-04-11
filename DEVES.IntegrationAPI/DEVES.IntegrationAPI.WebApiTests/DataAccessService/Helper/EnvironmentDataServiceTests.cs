using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.DataAccessService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.Helper.Tests
{
    [TestClass()]
    public class EnvironmentDataServiceTests
    {
        [TestMethod()]
        public void GetCaseCategoryForRVPTest()
        {
            var entiry = EnvironmentDataService.Instance.GetCaseCategoryForRVP();
            Assert.IsNotNull(entiry);
            Assert.AreEqual("0201", entiry.Code);
            Assert.AreEqual("สินไหม (Motor)", entiry.Name);

        }

        [TestMethod()]

        public void GetCaseSubCategoryForRVPTest()
        {
            var entiry = EnvironmentDataService.Instance.GetCaseSubCategoryForRVP();
            Assert.IsNotNull(entiry);
            Assert.AreEqual("0201-013", entiry.Code);
            Assert.AreEqual("แจ้งอุบัติเหตุรถยนต์ (ผ่าน บ.กลาง)", entiry.Name);

        }

        [TestMethod()]
        public void GetInformerForRVPTest()
        {
            var entiry = EnvironmentDataService.Instance.GetInformerForRVP();
            Assert.IsNotNull(entiry);
            Assert.AreEqual("10077508", entiry.PolisyClientId);
            Assert.AreEqual("บริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด", entiry.Name);
        }
    }
}