using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.DataAccessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.Core.ExtensionMethods;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.Tests
{
    [TestClass()]
    public class SpReqMotorClaimNotiNoDataGatewayTests
    {
        [TestMethod()]
        public void ExcecuteTest()
        {
            //incident // ticket
            var sp = new SpReqMotorClaimNotiNoDataGateway();
            var result = sp.Excecute("E7E760A9-F0C1-E611-80CB-0050568D1874","ADMIN TEST");

          //  DECLARE @ticketNo AS NVARCHAR(20) = 'CAS-00012-L1X2C8';
          //  DECLARE @uniqueID AS UNIQUEIDENTIFIER = 'E7E760A9-F0C1-E611-80CB-0050568D1874';
          //  DECLARE @requestByName AS NVARCHAR(250) = 'ADMIN TEST';

            Assert.IsTrue(result.Success);
        }
    }
}