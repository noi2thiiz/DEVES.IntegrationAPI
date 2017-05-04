using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter;

namespace DEVES.IntegrationAPI.WebApi.Core.DataAdepter.Tests
{
    [TestClass()]
    public class RESTClientTests
    {
        [TestMethod()]
        public  void StartWebRequestTest()
        {
         
            var client = new RESTClient("https://crmappqa.deves.co.th/internal-service/api/StoreService/ext");
            var req = new DbRequest {StoreName = "[sp_Insert_TransactionLog]"};
            req.AddParam("ServiceName", "TEST_1");


            var client2 = new RESTClient("https://crmappqa.deves.co.th/internal-service/api/StoreService/ext");
            var req2 = new DbRequest {StoreName = "[sp_Insert_TransactionLog]"};
            req2.AddParam("ServiceName", "TEST_2");




            var client3 = new RESTClient("https://crmappqa.deves.co.th/internal-service/api/StoreService/ext");
            var req3 = new DbRequest {StoreName = "[sp_Insert_TransactionLog]"};
            req3.AddParam("ServiceName", "TEST_3");


            client.PostAsync(req);
             client3.PostAsync(req3);
            client2.PostAsync(req2);
           
            Console.WriteLine("Sended");
        }

        [TestMethod()]
        public void GetUnknownTypeTest()
        {
            string req = "xxxx";
            Assert.AreEqual("String", RESTClient.GetUnknownType(req));
        }
    }
}