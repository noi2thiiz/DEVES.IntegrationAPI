﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.Polisy400;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class COMPInquiryClientMasterTests
    {
   
        [TestMethod()]
        public void ExecuteTest()
        {
           
            var service = new COMPInquiryClientMasterService();
            var result = service.Execute(new COMPInquiryClientMasterInputModel
            {
                fullName = "พรชัย",
                cltType = "P"
            });
            Console.WriteLine("==================result================");
            Console.WriteLine(result.ToJson());
            Assert.AreEqual(true, result.clientListCollection.Any());
        }
    }
}