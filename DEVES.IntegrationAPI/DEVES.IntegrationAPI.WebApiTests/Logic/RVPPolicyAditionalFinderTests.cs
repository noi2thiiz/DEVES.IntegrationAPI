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
    public class RVPPolicyAditionalFinderTests
    {
        [TestMethod()]
        public void FindTest()
        {

                var result = RVPPolicyAditionalFinder.Find("2010034076831","", "ตม4533","กท");
                Assert.IsNotNull(result);
                Assert.AreEqual(new Guid("3BFCB0A4-DCB6-E611-80CA-0050568D1874"), result.crmPolicyDetailId);
        }
    }
}