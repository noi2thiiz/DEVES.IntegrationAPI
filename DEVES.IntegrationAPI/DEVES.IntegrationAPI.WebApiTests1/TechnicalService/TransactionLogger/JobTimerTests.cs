using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger.Tests
{
    [TestClass()]
    public class JobTimerTests
    {
        [TestMethod()]
        public void StartTest()
        {
           LogJobHandle.Start();
        }
    }
}