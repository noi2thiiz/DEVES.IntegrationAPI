using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.RVP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.Logic.RVP.Tests
{
    [TestClass()]
    public class RVPInformerFinderTests
    {
        [TestMethod()]
        public void FindTest()
        {
            var informerGuid = RVPInformerFinder.Find().Id;
            Assert.IsNotNull(informerGuid);
            Assert.AreEqual(new Guid("81cbaa5f-aeb6-e611-80ca-0050568d1874"), informerGuid);
        }
    }
}