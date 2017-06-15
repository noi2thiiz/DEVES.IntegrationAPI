using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts.Tests
{
    [TestClass()]
    public class SpQueryGarageAssessmentFromLocusTests
    {
        [TestMethod()]
        public void SpQueryGarageAssessmentFromLocusTest()
        {
            var result =  SpQueryGarageAssessmentFromLocus.Instance.Excecute(new Dictionary<string, string> { { "BACK_DAY", "30" } });
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
        }
    }
}