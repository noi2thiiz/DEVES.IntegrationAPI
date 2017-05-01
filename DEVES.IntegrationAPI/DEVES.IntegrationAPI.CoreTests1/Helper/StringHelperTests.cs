using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Core.Helper.Tests
{
    [TestClass()]
    public class StringHelperTests
    {
        [TestMethod()]
        public void ToUpperIgnoreNullTest()
        {
            string text = "x";
            Assert.AreEqual("X", text.ToUpperIgnoreNull());

            string textNull = null;
            Assert.AreEqual(null, textNull.ToUpperIgnoreNull());

            string cls_sex = "m";
            var sex= ((new[] { "M", "F", "U" }).Contains(cls_sex.ToUpperIgnoreNull()))
                ? cls_sex.ToUpperIgnoreNull()
                : "U";
            Assert.AreEqual("M", sex);


             cls_sex = null;
             sex = ((new[] { "M", "F", "U" }).Contains(cls_sex.ToUpperIgnoreNull()))
                ? cls_sex.ToUpperIgnoreNull()
                : "U";
            Assert.AreEqual("U", sex);

        }
    }
}
