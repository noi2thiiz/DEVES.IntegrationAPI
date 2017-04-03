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
    public class RegClientPersonalTests: ApiControllerTests
    {
        public RegClientPersonalTests(ITestOutputHelper output) : base(output)
        {
            this._endPoint = "http://localhost:5001/api/RegClientPersonal";
            this._modelRequestSchemaFileName = "RegClaimRequestFromClaimDiRequestModel_Input_Schema.json";
            this._exampleValidInputs.Add(@"{
                            'generalHeader' :{
                            'roleCode' : 'G',
                            'cleansingId' : 'C2017-100000006',
                            'polisyClientId' : 'Z1000000',
                            'crmClientId' : '201703-1000000',
                            'clientAdditionalExistFlag' : 'N'
                            },'profileInfo' : {
                            'salutation' : '0006',
                            'personalName' : 'ทนาชัย'
                            'personalSurname' : 'ตันตระกูล',
                            'sex' : 'M',
                            'idCitizen' : '4100600049461',
                            'idPassport' : '',
                            'idAlien' : '',
                            'idDriving' : '',
                            'birthDate' : '2017-01-01 09:10:11',
                            'nationality' : '764',
                            'language' : 'E',
                            'married' : 'M',
                            'occupation' : '',
                            'riskLevel' : '',
                            'vipStatus' : 'Y',
                            'remark' : ''
                            },'contactInfo' : {
                            'telephone1' : '0869079888',
                            'telephone1Ext' : '',
                            'telephone2' : '',
                            'telephone2Ext' : '',
                            'telephone3' : '',
                            'telephone3Ext' : '',
                            'mobilePhone' : '',
                            'fax' : '',
                            'emailAddress' : '',
                            'lineID' : '',
                            'facebook' : ''
                            },'addressInfo' : {
                            'address1' : '',
                            'address2' : '',
                            'address3' : '',
                            'subDistrictCode' : '',
                            'districtCode' : '',
                            'provinceCode' : '',
                            'postalCode' : '',
                            'country' : '',
                            'addressType' : '',
                            'latitude' : '',
                            'longtitude' : ''
                            }}");
        }


        [Fact]
        public async void Should_Success_When_GIve_Valid_Input()
        {
            Random rnd = new Random();
            string lertId = (rnd.Next(1, 999999)).ToString();  // 1 <= month < 13

            string dataJson = @"{
                            'generalHeader' :{
                            'roleCode' : 'G',
                            'cleansingId' : 'C2017-100000006',
                            'polisyClientId' : 'Z1000000',
                            'crmClientId' : '201703-1000000',
                            'clientAdditionalExistFlag' : 'N'
                            },'profileInfo' : {
                            'salutation' : '0006',
                            'personalName' : 'ทนาชัย'
                            'personalSurname' : 'ตันตระกูล',
                            'sex' : 'M',
                            'idCitizen' : '4100600049461',
                            'idPassport' : '',
                            'idAlien' : '',
                            'idDriving' : '',
                            'birthDate' : '2017-01-01 09:10:11',
                            'nationality' : '764',
                            'language' : 'E',
                            'married' : 'M',
                            'occupation' : '',
                            'riskLevel' : '',
                            'vipStatus' : 'Y',
                            'remark' : ''
                            },'contactInfo' : {
                            'telephone1' : '0869079888',
                            'telephone1Ext' : '',
                            'telephone2' : '',
                            'telephone2Ext' : '',
                            'telephone3' : '',
                            'telephone3Ext' : '',
                            'mobilePhone' : '',
                            'fax' : '',
                            'emailAddress' : '',
                            'lineID' : '',
                            'facebook' : ''
                            },'addressInfo' : {
                            'address1' : '',
                            'address2' : '',
                            'address3' : '',
                            'subDistrictCode' : '',
                            'districtCode' : '',
                            'provinceCode' : '',
                            'postalCode' : '',
                            'country' : '',
                            'addressType' : '',
                            'latitude' : '',
                            'longtitude' : ''
                            }}";



            AssertSuccessPostRequest(_endPoint, dataJson);
        }
    }
}
