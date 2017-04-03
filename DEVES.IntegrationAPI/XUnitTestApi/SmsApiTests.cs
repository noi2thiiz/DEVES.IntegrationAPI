using TestApi.Common;
using Xunit;
using Xunit.Abstractions;

namespace TestApi
{
    public class SmsApiTests : ApiControllerTests
    {


        public SmsApiTests(ITestOutputHelper output) : base(output)
        {
            _endPoint = "http://localhost:5001/api/Sms/Send";
            _modelRequestSchemaFileName = "SMSRequestModel_Input_Schema.json";
            _exampleValidInputs.Add( @"{
                'message' : 'ทดสอบข้อความ',
                'mobileNumber' : '0865557013'
            }");

            _exampleInValidInputs.Add(@"{
                'message' : ''
            }");
            _exampleInValidInputs.Add(@"{
                'message' : '',
                'mobileNumber' :''
            }");
        }

    }
}