using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts.Tests
{
    [TestClass()]
    public class SpApiCustomerClientTests
    {
        [TestMethod()]
        public void SearchCrmContactClientIdTest()
        {
            var result = SpApiCustomerClient.Instance.SearchCrmContactClientId("C", "C2017-007753697");
           
            Assert.AreEqual("201011-0063734",result.FirstOrDefault());
        }
        [TestMethod()]
        public void SearchCrmContactClientIdShoudNotFoundTest()
        {
            var result = SpApiCustomerClient.Instance.SearchCrmContactClientId("C", "C2017-999999999");

            Assert.AreEqual(null, result.FirstOrDefault());
        }
    }
}