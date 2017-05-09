using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.Logic.Validator.Tests
{
    [TestClass()]
    public class MasterDataValidaterTests
    {
        [TestMethod()]
        public void TryConvertToPolisyCodeTest()
        {
            var validator = new MasterDataValidator();
            var policyCode = validator.TryConvertSalutationCode("0001", "profileInfo.salutation");
            Assert.AreEqual("0023", policyCode);

        }

        [TestMethod()]
        public void TryConvertToPolisyCodeWhenGiveNullTest()
        {
            var validator = new MasterDataValidator();
            var policyCode = validator.TryConvertSalutationCode(null, "profileInfo.salutation");
            Assert.AreEqual("", policyCode);

        }
    }
}