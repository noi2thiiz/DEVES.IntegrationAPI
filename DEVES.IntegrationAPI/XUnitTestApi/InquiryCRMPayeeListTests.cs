using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApi.Common;
using Xunit;
using Xunit.Abstractions;

namespace xUnitTestApi
{
    public class InquiryCRMPayeeListTests : ApiControllerTests
    {
        public InquiryCRMPayeeListTests(ITestOutputHelper output) : base(output)
        {
            this._endPoint = "http://localhost:50076/api/InquiryCRMPayeeList";
            this._modelRequestSchemaFileName = "RegClaimRequestFromClaimDiRequestModel_Input_Schema.json";
            this._exampleValidInputs.Add(@"{
                                             'clientType' : 'P',
                                             'roleCode' : 'G',
                                             'polisyClientId' : '',
                                             'sapVendorCode' : '15125039',
                                             'fullname' : 'ออโต้ การาจา',
                                             'taxNo' : '',
                                             'taxBranchCode' : '',
                                             'requester' : 'MOTORCLAIM',
                                             'emcsCode' : ''
                                            }

                                            ");
        }
        [Fact]
        public async void Should_Success_When_GIve_Valid_Input()
        {
            Random rnd = new Random();
            string lertId = (rnd.Next(1, 999999)).ToString();  // 1 <= month < 13

            string dataJson = @"{
                                             'clientType' : 'P',
                                             'roleCode' : 'G',
                                             'polisyClientId' : '',
                                             'sapVendorCode' : '15125039',
                                             'fullname' : 'ออโต้ การาจา',
                                             'taxNo' : '',
                                             'taxBranchCode' : '',
                                             'requester' : 'MOTORCLAIM',
                                             'emcsCode' : ''
                                            }";



            AssertSuccessPostRequest(_endPoint, dataJson);
        }
    }
}

