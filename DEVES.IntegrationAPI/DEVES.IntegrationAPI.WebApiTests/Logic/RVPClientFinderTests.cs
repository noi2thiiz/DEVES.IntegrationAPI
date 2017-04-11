using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.RVP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.Logic.RVP.Tests
{
    [TestClass()]
    public class RVPClientFinderTests
    {
        [TestMethod()]
        public void FindTest()
        {


           

        var result = RVPClientFinder.Find( "P", "CRM5555");

            Assert.IsNotNull(result);
            Assert.AreEqual("0872977030", result.MobilePhone);
            Assert.AreEqual(new Guid("b55765f1-c4a4-e611-80ca-0050568d1874"), result.ContactId, " test ContactId as Guid");
            Assert.AreEqual(new Guid("506e354e-b8a1-e611-80c7-0050568d1874"), result.OwningBusinessUnit, " test OwningBusinessUnit  as Guid");
            


        }

        [TestMethod()]
        public void FindPersonalTest()
    {


           //  public const string POLICY_CLIENT_ID = "10515387";
             //public const string POLICY_CLIENT_TYPE = "P";
                 //public const string POLICY_CLIENT_NAME = "ยุทธชัย รุ่งมงคลนาม";

            var result = RVPClientFinder.Find("P", "10515387");

                   Assert.IsNotNull(result);
         //   Assert.AreEqual("0872977030", result.MobilePhone);
            Assert.AreEqual(new Guid("C25FF182-941B-E711-80D0-0050568D615F"), result.ContactId, " test ContactId as Guid");
           // Assert.AreEqual(new Guid("506e354e-b8a1-e611-80c7-0050568d1874"), result.OwningBusinessUnit, " test OwningBusinessUnit  as Guid");
            


        }
    }
}