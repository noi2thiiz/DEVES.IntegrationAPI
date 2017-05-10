using DEVES.IntegrationAPI.WebApi.Logic.Validator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DEVES.IntegrationAPI.WebApiTests1.Logic.Validator
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