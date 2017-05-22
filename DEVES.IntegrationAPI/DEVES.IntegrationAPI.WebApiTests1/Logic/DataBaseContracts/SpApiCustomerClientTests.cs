﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            AppConfig.Instance.StartupForUnitTest();
         


            var result = SpApiCustomerClient.Instance.SearchCrmContactClientId("P", "CRM5555");

            Assert.AreEqual("201611-0000187", result.FirstOrDefault());

         

            


        }
        [TestMethod()]
        public void SearchCrmContactClientIdShoudNotFoundTest()
        {
            var result = SpApiCustomerClient.Instance.SearchCrmContactClientId("C", "C2017-999999999");

            Assert.AreEqual(null, result.FirstOrDefault());
        }

        [TestMethod()]
        public void GetCrmContactClientIdTest()
        {
           var result = SpApiCustomerClient.Instance.GetCrmContactClientId("P", "C2017-000007565");

            Assert.AreEqual("201009-0005559", result);
        }
    }
}