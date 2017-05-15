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
        [TestMethod()]
        public void ToPersonalTitleForSAPTest()
        {
            AppConfig.Instance.StartupForUnitTest();
            PersonalTitleMasterData.Instance.InitData();

            Assert.AreEqual("นางสาว", ("0005").ToPersonalTitleName());

            Assert.AreEqual("น.ส.", ("0005").ToPersonalTitleForSap());

           

            Assert.AreEqual("0094", ("0005").ToPersonalTitlePolisyCode());



        }
    }
}