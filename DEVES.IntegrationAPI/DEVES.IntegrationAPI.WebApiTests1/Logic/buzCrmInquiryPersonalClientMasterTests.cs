using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class buzCrmInquiryPersonalClientMasterTests
    {
       
        [TestMethod()]
        public void ExecuteBuzCrmInquiryPersonalClientMasterTest()
        {
            AppConfig.Instance.StartupForUnitTest();

            var cmd = new buzCrmInquiryPersonalClientMaster();
            var input = new InquiryClientMasterInputModel
            {

                conditionHeader = new ConditionHeaderModel
                {
                    clientType = "P",
                    roleCode = "G"
                },
                conditionDetail = new ConditionDetailModel
                {
                    clientFullname = "พรชัย"
                }
            };
           var result =  cmd.Execute(input);

            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
        }
    }
}