using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class TransformRegPayeePersonalInputModel_to_SAPCreateVendorInputModelTests
    {
        [TestMethod()]
        public void TransformModelTest()
        {
            var fullname = "AAAAABBBBBCCCCCDDDDDEEEEEFFFFFQQQQQWWWWWOOOOODDDDDLLLLLCCCCCPPPPPSSSSS";
            string NAME1 = "";
            string NAME2 = "";
            if (fullname.Length > 40)
            {
                NAME1 = fullname.Substring(0, 40);
                NAME2 = fullname.Substring(41, fullname.Length - 1);
            }
            else
            {
                NAME1 = fullname;
            }
            Assert.AreEqual(40, NAME1.Length);
            Assert.AreEqual("X", NAME1);
            Assert.AreEqual("Y", NAME2);
        }
    }
}