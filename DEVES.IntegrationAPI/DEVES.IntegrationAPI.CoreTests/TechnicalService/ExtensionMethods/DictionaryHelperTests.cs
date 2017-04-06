using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.Core.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Core.ExtensionMethods.Tests
{
    [TestClass()]
    public class DictionaryHelperTests
    {
        [TestMethod()]
        public void ToTypeTest()
        {
            var dict = new Dictionary<string, dynamic>
            {
                { "X", "A" },
                { "Y", 12 }
            };
            var obj = dict.ToType<TestType>();
            Assert.AreEqual(obj.X, "A");
        }
        internal class TestType
        {
            public string X { get; set; }
            public int Y { get; set; }
        }
    }
}