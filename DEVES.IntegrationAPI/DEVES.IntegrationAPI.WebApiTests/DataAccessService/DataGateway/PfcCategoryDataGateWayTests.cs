using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.DataAccessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.Tests
{
    [TestClass()]
    public class PfcCategoryDataGateWayTests
    {
        [TestMethod()]
        public void GetDefaultForRVPTest()
        {
            PfcSubCategoryDataGateWay  db = new PfcSubCategoryDataGateWay();
            var entity = db.GetDefaultForRVP();
            Assert.IsNotNull(entity);
            Assert.AreEqual("0201-013", entity.Code);
            Assert.AreEqual(new Guid("92D632D8-29AB-E611-80CA-0050568D1874"), entity.Id);
        }
    }
}