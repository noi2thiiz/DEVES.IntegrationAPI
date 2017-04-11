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
    public class CrmOwnerAccountDataGateWayTests
    {
        [TestMethod()]
        public void FindByDomainNameTest()
        {
            CrmOwnerAccountDataGateWay db = new CrmOwnerAccountDataGateWay();
           var result =  db.FindByDomainName(@"sasipa.b");
            Assert.IsNotNull(result);
          //  Assert.AreEqual("", result.ToJSON());
            Assert.AreEqual("ษษิภา บัวใหญ่", result.FullName);
            //Assert.AreEqual(new Guid("15b4b411-9e1b-e711-80d4-0050568d1874"), result.Id);
          
        }
    }
}