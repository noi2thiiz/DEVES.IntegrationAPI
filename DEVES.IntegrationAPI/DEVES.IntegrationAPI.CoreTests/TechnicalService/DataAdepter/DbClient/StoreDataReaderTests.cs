using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi;
using DEVES.IntegrationAPI.WebApi.DataAccessService.Helper;

namespace DEVES.IntegrationAPI.WebApi.Core.DataAdepter.Tests
{
    [TestClass()]
    public class StoreDataReaderTests
    {
      

        [TestMethod()]
        public void StoreDataReaderTest()
        {
            var conectionString = "Data Source=DESKTOP-Q30CAGJ;Initial Catalog=CRM_CUSTOM_APP;User ID=sa;Password=patiwat";
            StoreDataReader reader = new StoreDataReader(conectionString);
            Assert.IsInstanceOfType(reader, typeof(StoreDataReader));
        }

        [TestMethod()]
        public void TestConnectionToQA()
        {
            var conectionString = "Data Source=192.168.8.121;Initial Catalog=CRMQA_MSCRM;Persist Security Info=True;User ID=CRMDevelop;Password=Develop%D";
            StoreDataReader reader = new StoreDataReader(conectionString);
            Assert.IsInstanceOfType(reader, typeof(StoreDataReader));
        }

        [TestMethod()]
        public void TestConnectionToDEV()
        {
            var conectionString = "Data Source=192.168.8.122;Initial Catalog=CRMDEV_MSCRM;Persist Security Info=True;User ID=CRMDevelop;Password=Develop%D";
            StoreDataReader reader = new StoreDataReader(conectionString);
            Assert.IsInstanceOfType(reader, typeof(StoreDataReader));
        }

        [TestMethod()]
        public void ExecuteTest()
        {
            var conectionString = "Data Source=DESKTOP-Q30CAGJ;Initial Catalog=CRM_CUSTOM_APP;User ID=sa;Password=patiwat";
            StoreDataReader reader = new StoreDataReader(conectionString);
            var req = new DbRequest()
            {
                StoreName = "sp_TEST"
            };
            req.AddParam("X", "1","NVARCHAR");
            req.AddParam("Y", "2", "NVARCHAR");
            var resutl = reader.Execute(req);
          
          
            Assert.AreEqual(10, resutl.Count);
        }

        [TestMethod()]
        public void TestConnectionConnectionString()
        {
            var conectionString = EnvironmentDataService.Instance.GetXrmConnectionString();
            StoreDataReader reader = new StoreDataReader(conectionString);
            Assert.IsInstanceOfType(reader, typeof(StoreDataReader));
        }

      
            


    }
}