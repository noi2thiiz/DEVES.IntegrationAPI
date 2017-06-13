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
                  'assessmentType': 1,
                  'assessmentquestionnireid': 'D181A7B8-FA4C-E711-80DA-0050568D615F',
                  'assessmentrefcode': 'fade8b2add',
                  'assessmentScore1': 1,
                  'assessmentScore2': 2,
                  'assessmentScore3': 3,
                  'assessmentScore4': 4,
                  'assessmentScore5': 5,
                  'assessmentScore6': 6,
                  'assessmentScore7': 7,
                  'assessmentScore8': 8,
                  'assessmentScore9': 9,
                  'assessmentScore10': 10,
                  'assessmentComment': 'ABCD'
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