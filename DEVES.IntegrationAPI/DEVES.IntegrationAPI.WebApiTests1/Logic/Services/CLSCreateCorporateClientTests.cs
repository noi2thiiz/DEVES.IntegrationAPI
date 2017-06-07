using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.CLS;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class CLSCreateCorporateClientTests
    {
        [TestMethod()]
        public void Execute_CLSCreateCorporateClientTest()
        {
            var service = new CLSCreateCorporateClient("");
            var result = service.Execute(new CLSCreateCorporateClientInputModel
            {

            });
            Assert.IsNotNull(result);

          Console.WriteLine("==================result======================");
          Console.WriteLine(result.ToJson());
        }
    }
}