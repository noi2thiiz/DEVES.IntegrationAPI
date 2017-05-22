using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
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

            var cmd = new buzCrmInquiryClientMaster();
            var inputContent = @"{'conditionHeader':
            {
                'clientType' : 'P',
                'roleCode' : 'G'
            },
            'conditionDetail':
            {
                'clientFullname' : 'พรชัย'
            }
            }"  ;
            var jss = new JavaScriptSerializer();
            var input = jss.Deserialize<InquiryClientMasterInputModel>(inputContent);
            Console.WriteLine(input.ToJson());
            var result =  cmd.Execute(input);
            Console.WriteLine("==============result==================");
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
        }
    }
}