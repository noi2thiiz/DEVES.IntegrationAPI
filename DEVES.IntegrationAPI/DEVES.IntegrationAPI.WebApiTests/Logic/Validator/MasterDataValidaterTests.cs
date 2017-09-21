using System;
using DEVES.IntegrationAPI.WebApi;
using DEVES.IntegrationAPI.WebApi.Logic.Validator;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DEVES.IntegrationAPI.WebApiTests1.Logic.Validator
{
    [TestClass()]
    public class MasterDataValidaterTests
    {
        public MasterDataValidaterTests()
        {
           
            AppBootstrap.Instance.Start();
        }
        [TestMethod()]
        public void TryConvertToPolisyCodeTest()
        {
          
            var validator = new MasterDataValidator();
            var policyCode = validator.TryConvertSalutationCode( "profileInfo.salutation", "0001");
            Console.WriteLine("profileInfo.salutation");
            Console.WriteLine(validator.fieldErrorData.ToJson());
            Console.WriteLine("profileInfo.salutation");
            Assert.AreEqual(false, validator.Invalid());
            Assert.AreEqual("0023", policyCode);

        }

        [TestMethod()]
        public void TryConvertToPolisyCodeWhenGiveNullTest()
        {
            
            var validator = new MasterDataValidator();
            var policyCode = validator.TryConvertSalutationCode( "profileInfo.salutation", null);

           
            Assert.AreEqual("0023", policyCode);//return default 0023 คุณ

        }
    }
}