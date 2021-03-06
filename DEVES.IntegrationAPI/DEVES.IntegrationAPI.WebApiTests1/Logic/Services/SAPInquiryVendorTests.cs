﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.SAP;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class SAPInquiryVendorTests
    {
        [TestMethod()]
        public void Execute_SAPInquiryVendorTest()
        {
            
            var result = SAPInquiryVendor.Instance.Execute(new SAPInquiryVendorInputModel
            {
                PREVACC = "14555916"
            });
            Console.WriteLine("==============result===============");
            Console.WriteLine(result.ToJson());
            Assert.AreEqual(true, result.VendorInfo.Any());
        }

 

    }
}