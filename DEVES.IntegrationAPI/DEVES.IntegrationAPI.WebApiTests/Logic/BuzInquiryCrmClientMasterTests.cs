using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class BuzInquiryCrmClientMasterTests
    {
        [TestMethod()]
        public void ExecuteInputTest()
        {
           

            var cmd = new BuzInquiryCrmGeneralClient();
            var result = cmd.Execute(new InquiryClientMasterInputModel
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
            });

            Console.WriteLine("===========Execute Output===============");
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);


        }
    }
}