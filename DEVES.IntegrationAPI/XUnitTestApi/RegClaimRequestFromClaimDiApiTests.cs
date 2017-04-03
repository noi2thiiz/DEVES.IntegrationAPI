using System;
using Newtonsoft.Json.Linq;
using TestApi.Common;
using Xunit;
using Xunit.Abstractions;

namespace TestApi
{
    public class RegClaimRequestFromClaimDiApiTests : ApiControllerTests
    {
        public RegClaimRequestFromClaimDiApiTests(ITestOutputHelper output) : base(output)
        {
            this._endPoint = "http://localhost:5001/api/claim/RegClaimRequestFromClaimDi";
            this._modelRequestSchemaFileName = "RegClaimRequestFromClaimDiRequestModel_Input_Schema.json";
            this._exampleValidInputs.Add(@"{
                                                  'requestChanel': 'string',
                                                  'eventLertId': 0,
                                                  'lertInfo': {
                                                    'lertId': 0,
                                                    'lertDateTime': '2017-03-17 14:01:25',
                                                    'lertBy': 'string',
                                                    'telNo': 'string'
                                                  },
                                                  'policyOwnerInfo': {
                                                    'policyNo': 'string',
                                                    'carLicenseNo': 'string',
                                                    'carLicense_province': 'string',
                                                    'ownerFirstName': 'string',
                                                    'ownerLastName': 'string',
                                                    'ownerMobileNo': 'string',
                                                    'driverFirstName': 'string',
                                                    'driverLastName': 'string',
                                                    'driverMobileNo': 'string',
                                                    'caseResult': 'RIGHT',
                                                    'caseDatetime': '2017-03-17 14:01:25'
                                                  },
                                                  'thirdPartyInfo': [
                                                    {
                                                      'policyNo': 'string',
                                                      'insurerCode': 'string',
                                                      'insurerName': 'string',
                                                      'carLicenseNo': 'string',
                                                      'carLicenseProvince': 'string',
                                                      'ownerFirstName': 'string',
                                                      'ownerLastName': 'string',
                                                      'ownerMobileNo': 'string',
                                                      'driverFirstName': 'string',
                                                      'driverLastName': 'string',
                                                      'driverMobileNo': 'string'
                                                    }
                                                  ],
                                                  'location': {
                                                    'place': 'string',
                                                    'latitude': 'string',
                                                    'longitude': 'string'
                                                  }
                                                }");
        }

        [Fact]
        public async void it_should_Success_Post__All_Field_KFK()
        {
            Random rnd = new Random();
            string lertId = (rnd.Next(1, 999999)).ToString();  // 1 <= month < 13

            string template = @"
            {
              'requestChanel': 'KFK',
              'eventLertId': {lertId},
              'lertInfo': {
                'lertId': {lertId},
                'lertDateTime': '2017-01-15 13:05:25',
                'lertBy': 'TEST',
                'telNo': '9999999999999'
              },
             'policyOwnerInfo': {
                 'policyNo': '123456',
                 'carLicenseNo': 'กข 1234',
                 'carLicense_province': 'กรุงเทพมหานคร',
                 'ownerFirstName': 'จาตุรันต์',
                 'ownerLastName': 'แสงมณีทิพย์',
                 'ownerMobileNo': '0843849876',
                 'driverFirstName': 'จาตุรัตน์',
                 'driverLastName': 'แสงมณีพย์',
                 'driverMobileNo': '0843849876',
                 'caseResult': 'RIGHT',
                 'caseDatetime': '2017-01-15 14:01:25'
              },
               'thirdPartyInfo': [
                    {
                        'policyNo': '123456',
                        'insurerCode': '2025',
                        'insurerName': 'บริษัท กรุงเทพประกันภัยจำกัด (หมาชน)',
                        'carLicenseNo': 'กข 1234',
                        'carLicenseProvince': 'กรุงเทพมหานคร',
                        'ownerFirstName': 'จาตุรันต์',
                        'ownerLastName': 'แสงมณีทิพย์',
                        'ownerMobileNo': '0843849876',
                        'driverFirstName': 'จาตุรันต์',
                        'driverLastName': 'แสงมณีทิพย์',
                        'driverMobileNo': '0843849876'
                    }
              ],
              'location': {
                'place': '53 ราชวิธี แขวง ถนนพยาไท เขต ราชเทวี กรุงเทพมหานคร 10400 ประเทษไทย',
                'latitude': '13.7619937',
                'longitude': '100.5418375'
              }
            }";


           string dataJson = template.Replace("{lertId}", lertId);
           // output.WriteLine(dataJson);
            AssertSuccessPostRequest(_endPoint, dataJson);
        }

        [Fact]
        public async void it_should_Success_Post__All_Field_KFK_And_Multiple_thirdPartyInfo()
        {
            Random rnd = new Random();
            string lertId = (rnd.Next(1, 999999)).ToString();  // 1 <= month < 13

            string template = @"
            {
              'requestChanel': 'KFK',
              'eventLertId': {lertId},
              'lertInfo': {
                'lertId': {lertId},
                'lertDateTime': '2017-01-15 13:05:25',
                'lertBy': 'TEST',
                'telNo': '9999999999999'
              },
             'policyOwnerInfo': {
                 'policyNo': '123456',
                 'carLicenseNo': 'กข 1234',
                 'carLicense_province': 'กรุงเทพมหานคร',
                 'ownerFirstName': 'จาตุรันต์',
                 'ownerLastName': 'แสงมณีทิพย์',
                 'ownerMobileNo': '0843849876',
                 'driverFirstName': 'จาตุรัตน์',
                 'driverLastName': 'แสงมณีพย์',
                 'driverMobileNo': '0843849876',
                 'caseResult': 'RIGHT',
                 'caseDatetime': '2017-01-15 14:01:25'
              },
               'thirdPartyInfo': [
                    {
                        'policyNo': '123456',
                        'insurerCode': '2025',
                        'insurerName': 'บริษัท กรุงเทพประกันภัยจำกัด (หมาชน)',
                        'carLicenseNo': 'กข 1234',
                        'carLicenseProvince': 'กรุงเทพมหานคร',
                        'ownerFirstName': 'จาตุรันต์',
                        'ownerLastName': 'แสงมณีทิพย์',
                        'ownerMobileNo': '0843849876',
                        'driverFirstName': 'จาตุรันต์',
                        'driverLastName': 'แสงมณีทิพย์',
                        'driverMobileNo': '0843849876'
                    },
  {
                        'policyNo': '123457',
                        'insurerCode': '2025',
                        'insurerName': 'บริษัท กรุงเทพประกันภัยจำกัด (หมาชน)',
                        'carLicenseNo': 'กข 1234',
                        'carLicenseProvince': 'กรุงเทพมหานคร',
                        'ownerFirstName': 'จาตุรันต์2',
                        'ownerLastName': 'แสงมณีทิพย์2',
                        'ownerMobileNo': '0843849876',
                        'driverFirstName': 'จาตุรันต์',
                        'driverLastName': 'แสงมณีทิพย์',
                        'driverMobileNo': '0843849876'
                    }
              ],
              'location': {
                'place': '53 ราชวิธี แขวง ถนนพยาไท เขต ราชเทวี กรุงเทพมหานคร 10400 ประเทษไทย',
                'latitude': '13.7619937',
                'longitude': '100.5418375'
              }
            }";


            string dataJson = template.Replace("{lertId}", lertId);
            // output.WriteLine(dataJson);
            AssertSuccessPostRequest(_endPoint, dataJson);
        }

        [Fact]
        public async void it_should_Success_Post_Only_Request()
        {
            AssertSuccessPostRequest(_endPoint, @"
                     {
                       'requestChanel': 'ilertu',
                       'eventLertId': 9999,
                       'lertInfo': {
                         'lertId': 9999,
                         'lertDateTime': '2017-03-25 13:04:18',
                         'lertBy': 'TEST',
                         'telNo': '999999'
                       },
                       'location': {
                         'latitude': '13.7619937',
                         'longitude': '100.5418375'
                       }
                     }");
        }

        [Fact]
        public async void it_should_Badrequest_with_ParseError()
        {
            var response = postData(_endPoint, @"
                     {
                       'requestChanel': ,
                       'eventLertId': ,
                       'lertInfo': {
                         'lertId': 9999,
                         'lertDateTime': '2017-03-25 13:04:18',
                         'lertBy': 'TEST',
                         'telNo': '999999'
                       },
                       'location': {
                         'latitude': '13.7619937',
                         'longitude': '100.5418375'
                       }
                     }");
            dynamic obj = JObject.Parse(response.content);
            // output.WriteLine(obj.ToString());
            Assert.Equal("ParseError", obj.data.type.ToString());
        }
    }
}