using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.DataAccessService.Mastes.ExtensionMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.Mastes.ExtensionMethod.Tests
{
    [TestClass()]
    public class PersonalTitleMasterDataHelperTests
    {
        public PersonalTitleMasterDataHelperTests()
        {
           
            PersonalTitleMasterData.Instance.InitData();
        }
        [TestMethod()]
        public void ToPersonalTitleNameTest()
        {
           
            Assert.AreEqual("นางสาว", ("0005").ToPersonalTitleName());
        }

        [TestMethod()]
        public void ToPersonalTitlePolisyCodeTest()
        {

            Assert.AreEqual("0094", ("0005").ToPersonalTitlePolisyCode());
        }

        [TestMethod()]
        public void ToPersonalTitleForSAPTest()
        {
           
    
            Assert.AreEqual("น.ส.", ("0005").ToPersonalTitleForSap());


        }
    }
}