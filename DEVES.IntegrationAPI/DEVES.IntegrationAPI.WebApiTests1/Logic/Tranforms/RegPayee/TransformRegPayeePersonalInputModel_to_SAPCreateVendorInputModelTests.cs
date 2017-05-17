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
            string name1 = "";
            string name2 = "";
            Console.WriteLine(fullname.Length);
            if (fullname.Length > 40)
            {
                name1 = fullname.Substring(0, 40);
                Console.WriteLine(name1);
                name2 = fullname.Substring(40, fullname.Length - 40);
                Console.WriteLine(name2);
            }
            else
            {
                name1 = fullname;
            }
            Assert.AreEqual(40, name1.Length);
            Assert.AreEqual(30, name2.Length);
            Assert.AreEqual("AAAAABBBBBCCCCCDDDDDEEEEEFFFFFQQQQQWWWWW", name1);
            Assert.AreEqual("OOOOODDDDDLLLLLCCCCCPPPPPSSSSS", name2);
        }
    }
}