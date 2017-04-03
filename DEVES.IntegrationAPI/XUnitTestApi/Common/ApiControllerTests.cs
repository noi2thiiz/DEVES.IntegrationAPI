using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace TestApi.Common
{
    public  abstract class ApiControllerTests : ControllerIntegrationTests
    {
        public string _endPoint;
        public string _modelRequestSchemaFileName = "";
        public string _exampleValidInput = @"{}";
        public List<string> _exampleValidInputs;
        public List<string> _exampleInValidInputs;

        public string _folderAppData =
        @"C:\workspace\crm\master\DEVES.IntegrationAPI-dev\DEVES.IntegrationAPI\DEVES.IntegrationAPI.WebApi\App_Data\JsonSchema\";


        public ApiControllerTests(ITestOutputHelper output) : base(output)
        {
            _exampleValidInputs = new List<string>();
            _exampleInValidInputs = new List<string>();
        }

        [Fact]
        public async void ModelRequestSchemaFile_should_exists()
        {

            AssertFileExists(_folderAppData+_modelRequestSchemaFileName);
        }

        [Fact]
        public async void it_should_have_endpoint()
        {
            AssertNotAcceptEmptyRequest(_endPoint);
        }

        [Fact]
        public async void it_should_return_bad_request_when_given_empty_request()
        {
            AssertNotAcceptEmptyRequest(_endPoint);
        }
        [Fact]
        public async void it_should_return_success_when_given_valid_JsonSchema()
        {
            foreach (var req in _exampleValidInputs)
            {

                AssertValidJsonShema(_modelRequestSchemaFileName,req.ToString());
            }

        }

        [Fact]
        public async void it_should_return_bad_request_when_given_invalid_JsonSchema()
        {
            foreach (var req in _exampleInValidInputs)
            {

                AssertInValidJsonShema(_modelRequestSchemaFileName,req.ToString());
            }

        }
    }
}