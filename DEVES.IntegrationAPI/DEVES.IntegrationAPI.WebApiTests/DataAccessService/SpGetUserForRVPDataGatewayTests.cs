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
    public class SpGetUserForRVPDataGatewayTests
    {
        [TestMethod()]
        public void ExcecuteTest()
        {
            SpGetUserForRVPDataGateway dg = new SpGetUserForRVPDataGateway();
            var result = dg.Excecute();
            var InformerInfo = (Dictionary<string, dynamic>)result.Data[0];
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("DVS\\sasipa.b", InformerInfo["DomainName"]);//DVS\\crmtest1
        }
    }
}