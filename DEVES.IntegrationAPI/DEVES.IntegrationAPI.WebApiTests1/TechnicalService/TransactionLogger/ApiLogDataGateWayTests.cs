using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService.Tests
{
    [TestClass()]
    public class ApiLogDataGateWayTests
    {
        [TestMethod()]
        public void CallWebServiceTest()
        {
            
            
            ApiLogDataGateWay.CallWebService(new ApiLogEntry
            {
                TransactionID =  "XXXX"
               //ServiceName = "Test"
                // RequestTimestamp = DateTime.Now,ResponseTimestamp = DateTime.Now,
            });
           
        }
    }
}