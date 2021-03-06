﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class PolisyClientServiceTests
    {

        [TestMethod()]
        public void FindByCleansingClientIdTest()
        {
            
            var result = PolisyClientService.Instance.FindByCleansingId("C2017-100002158", "P");
            Console.WriteLine(result.ToJson());
            
            Assert.IsNotNull(result); 
            Assert.AreEqual("16972697", result.clientNumber);
            Assert.AreEqual("C2017-100002158", result.cleansingId);
        }
    }
}