using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class buzCrmInquiryPersonalClientMasterTests
    {
       
        [TestMethod()]
        public void Execute_BuzInquiryCrmClientMasterTest()
        {
           

            var cmd = new buzCrmInquiryClientMaster();
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