using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.ConnectCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.Xrm.Sdk.Client;

namespace DEVES.IntegrationAPI.WebApi.ConnectCRM.Tests
{
    [TestClass()]
    public class ConnectionThreadSafeTests
    {
        [TestMethod()]
        public void GetOrganizationProxyTest()
        {
            try
            {
                var proxy = ConnectionThreadSafe.GetOrganizationProxy();
                Console.WriteLine(proxy.CallerId);
                Assert.IsNotNull(proxy);

                var test = new Microsoft.Xrm.Sdk.Messages.RetrieveTimestampRequest();
                var result= proxy.Execute(test);
                Assert.IsNotNull(result);
                Console.WriteLine(result.ToJson());
              

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.Fail();
            }
           
        }
    }
}