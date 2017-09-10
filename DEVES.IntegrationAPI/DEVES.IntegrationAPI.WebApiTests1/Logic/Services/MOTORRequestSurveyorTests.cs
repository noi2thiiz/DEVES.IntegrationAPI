using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.RequestSurveyor;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class MOTORRequestSurveyorTests
    {
        [TestMethod()]
        public void Execute_MOTORRequestSurveyor_It_should_return_fail_when_give_empty_json_input()
        {
            try
            {
                var service =
                    new MOTORRequestSurveyor(Guid.NewGuid().ToString(), "UnitTest");
                var result = service.Execute( new RequestSurveyorInputModel());

                Console.WriteLine("==================result======================");

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                Assert.IsTrue(string.IsNullOrEmpty(result.eventId));        //ต้องไม่มีค่า
                Assert.IsTrue(string.IsNullOrEmpty(result.errorMessage));   //ต้องไม่มีค่า


                // Assert.AreEqual("400", result.code);
            }
            catch (BuzErrorException be)
            {
                Console.WriteLine("==================result BuzErrorException======================");
                Console.WriteLine(be.GetOutputModel().ToJson());
                Assert.Fail(be.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("==================result Exception======================");
                Assert.Fail(e.Message);
            }
        }
    }
}