using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DEVES.IntegrationAPI.Core.Helper.Tests
{
    [TestClass()]
    public class StringHelperTests
    {
        [TestMethod()]
        public void ToUpperIgnoreNullTest()
        {
            string text = "x";
            Assert.AreEqual("X",text.ToUpperIgnoreNull() );

            string textNull = null;
            Assert.AreEqual(null, textNull.ToUpperIgnoreNull());
        }
    }
}