using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.CRM;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class buzCreateAssessmentFromLocusTests
    {
        [TestMethod()]
        public void ExecuteInputTest()
        {
            var cmd = new buzCreateAssessmentFromLocus();
            var result = cmd.Execute(new CreateAssessmentFromLocusInputModel
            {
                requestId = "1"
            });
            Assert.IsNotNull(result);
            Console.WriteLine("===============result.ToJson()===================");
            Console.WriteLine(result.ToJson());
        }

     
    }
}