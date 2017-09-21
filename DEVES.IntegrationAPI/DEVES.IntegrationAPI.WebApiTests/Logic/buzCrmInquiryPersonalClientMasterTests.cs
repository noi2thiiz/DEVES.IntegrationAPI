using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.Logic.Commands.Client;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class buzCrmInquiryPersonalClientMasterTests
    {
       
        [TestMethod()]
        public void Execute_BuzInquiryCrmClientMasterTest()
        {
           

            var cmd = new BuzCrmInquiryClientMaster();
            var result = cmd.Execute(new InquiryClientMasterInputModel
            {
                conditionHeader = new ConditionHeaderModel
                {
                    clientType = "P",
                    roleCode  ="G"
                },
                conditionDetail=new ConditionDetailModel
                {
                    clientFullname= "พรชัย"
                }
            });

            Console.WriteLine("===========Execute Output===============");
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);


        }
    }
}