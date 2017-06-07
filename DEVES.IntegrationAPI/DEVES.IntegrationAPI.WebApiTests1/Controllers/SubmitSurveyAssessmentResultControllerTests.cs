using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.Controllers.Tests
{
    [TestClass()]
    public class SubmitSurveyAssessmentResultControllerTests
    {
        [TestMethod()]
        public void Post_SubmitSurveyAssessmentResultTest()
        {
            // Arrange
            var controller = new SubmitSurveyAssessmentResultController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            string input = @"
            {
              'assessmentrefcode': 'ABC',
              'assessmentClaimNotiScore': 10,
              'assessmentClaimNotiComment': 'ABC',
              'assessmentSurveyScore': 10,
              'assessmentSurveySpeedScore': 10,
              'assessmentSurveyComment': 'ABC',
              'assessmentSurveyByUserid': 'ABC'
            }";
          
        //Assert
        var response = (HttpResponseMessage)controller.Post(JObject.Parse(input));
            Console.WriteLine("==============output==================");
            var output = response?.Content?.ReadAsStringAsync();
            Assert.IsNotNull(output?.Result);
            Console.WriteLine(output?.Result);
        }
    }
}