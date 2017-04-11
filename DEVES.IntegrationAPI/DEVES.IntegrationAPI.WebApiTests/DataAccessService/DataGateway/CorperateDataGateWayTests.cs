using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.DataGateway.Tests
{
    [TestClass()]
    public class CorperateDataGateWayTests
    {
        [TestMethod()]
        public void FindByPolisyClientIdTest()
        {
            // TEST FOR DEV
            CorperateDataGateWay db = new CorperateDataGateWay();
            var result = db.FindByPolisyClientId("10077508");
            Assert.IsNotNull(result);
            //  Assert.AreEqual("", result.ToJSON());
            Assert.IsNotNull(result.Name); //"บริษัททดสอบเจริญ15 รุ่งเรือง รุ่งเรือง", 
            Assert.IsNotNull(result.Id);
           // Assert.AreEqual(new Guid("92363a20-2c1d-e711-80d4-0050568d1874"), result.Id);

        }
    }
}