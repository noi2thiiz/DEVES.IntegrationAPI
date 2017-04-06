using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.DataAccessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.DataAccessService.XrmEntity;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.Tests
{
    [TestClass()]
    public class PfcMotorAccidentDataGatewayTests
    {
        [TestMethod()]
        public void PfcMotorAccidentDataGatewayTest()
        {
            PfcMotorAccidentDataGateway db = new PfcMotorAccidentDataGateway();
            PfcMortorAccident entity = new PfcMortorAccident(new Guid("58b9dccc-9a1a-e711-80d4-0050568d1874"));
            var reuslt = db.Create(entity);
            Assert.IsNotNull(reuslt);
        }
    }
}