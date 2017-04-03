using TestApi;
using TestApi.Common;
using Xunit;
using Xunit.Abstractions;

namespace TestAp
{
    public class CtiVoiceRecordApiTests : ApiControllerTests
    {


        public CtiVoiceRecordApiTests(ITestOutputHelper output) : base(output)
        {
            this._endPoint = "http://localhost:5001/api/CTIWebDialer/VoiceRecord";
            this._modelRequestSchemaFileName = "VoiceRecordRequestModel_Input_Schema.json";
            this._exampleValidInputs.Add(@"{
            'callTransactionId' : 'string',
            'sessionId' : 'string',
            'eventAction' : 'STARTED',
            'userid' : 'string',
            'callernumber' : 'string',
            'callednumber' : 'string',
            'isConference': true,
            'sessionStartDate' : 000000001,
            'sessionDuration' : 00001,
            'sessionState' : 'string',
            'url' : 'string',
            'callType' : 'in'
            }");

            this._exampleInValidInputs.Add(@"{
            'callTransactionId' : 'string',
            'sessionId' : 'string',
            'eventAction' : 'STARTED',
            'userid' : 'string',
            'callernumber' : 'string',
            'callednumber' : 'string',
            'isConference': true,
            'sessionStartDate' : 000000001,
            'sessionDuration' : 00001,
            'sessionState' : 'string',

            'callType' : true
            }");
        }


        [Fact]
        public async void it_should_valid_JsonSchema()
        {

            AssertValidJsonShema("VoiceRecordRequestModel_Input_Schema.json",@"
            {
            'callTransactionId' : 'string',
            'sessionId' : 'string',
            'eventAction' : 'STARTED',
            'userid' : 'string',
            'callernumber' : 'string',
            'callednumber' : 'string',
            'isConference': true,
            'sessionStartDate' : 000000001,
            'sessionDuration' : 00001,
            'sessionState' : 'string',
            'url' : 'string',
            'callType' : 'in'
            }");


        }


        [Fact]
        public async void it_should_return_bad_request_when_give_invalid_enum_value()
        {
            AssertBadRequestPostRequest(_endPoint,
           @"{
            'callTransactionId' : 'string',
            'sessionId' : 'string',
            'eventAction' : 'TOP',
            'userid' : 'string',
            'callernumber' : 'string',
            'callednumber' : 'string',
            'isConference': true,
            'sessionStartDate' : 000000001,
            'sessionDuration' : 00001,
            'sessionState' : 'string',
            'url' : 'string',
            'callType' : 'in'
            }");
        }



    }
}