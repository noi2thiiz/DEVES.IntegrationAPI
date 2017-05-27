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
    public class GlobalTransactionIdGeneratorTests
    {
        [TestMethod()]
        public void GetNewGuidTest()
        {
            var guid = GlobalTransactionIdGenerator.Instance.GetNewGuid();
            Console.WriteLine(guid);

            var guid2 = GlobalTransactionIdGenerator.Instance.GetNewGuid();
            Console.WriteLine(guid2);
            Assert.IsNotNull(guid2);
        }
    }
}