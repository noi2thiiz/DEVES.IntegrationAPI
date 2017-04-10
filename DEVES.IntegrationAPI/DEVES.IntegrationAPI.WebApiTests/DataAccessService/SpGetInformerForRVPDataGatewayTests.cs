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
    public class SpGetInformerForRVPDataGatewayTests
    {
        [TestMethod()]
        public void ExcecuteTest()
        {
            SpGetInformerForRVPDataGateway dg = new SpGetInformerForRVPDataGateway();
            var result = dg.Excecute();
           var  InformerInfo = (Dictionary<string, dynamic>)result.Data[0];
            Assert.AreEqual(1, result.Count);
           // Assert.AreEqual("10077508", InformerInfo["pfc_polisy_client_id"]);
            Assert.AreEqual("10077508", InformerInfo["pfc_polisy_client_id"]);
        }
    }
}