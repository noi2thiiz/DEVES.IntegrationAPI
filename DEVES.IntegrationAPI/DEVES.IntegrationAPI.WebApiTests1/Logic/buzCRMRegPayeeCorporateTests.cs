using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Core.Util;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegClientCorporate;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;
using AddressHeaderModel = DEVES.IntegrationAPI.Model.RegPayeeCorporate.AddressHeaderModel;
using GeneralHeaderModel = DEVES.IntegrationAPI.Model.RegPayeeCorporate.GeneralHeaderModel;
using ProfileHeaderModel = DEVES.IntegrationAPI.Model.RegPayeeCorporate.ProfileHeaderModel;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class buzCRMRegPayeeCorporateTests
    {
        

        [TestMethod()]
        public void Execute_buzCRMRegPayeeCorporate_CreateNewPayeeTest()
        {
            var input = new RegPayeeCorporateInputModel
            {
                generalHeader = new GeneralHeaderModel
                {
                    cleansingId = "",
                    polisyClientId = "",
                    roleCode = "G",
                   crmClientId = "",
                   clientAdditionalExistFlag = "N"
                   
                },
                profileHeader =  new ProfileHeaderModel
                {
                    corporateName1 = RandomValueGenerator.RandomString(10),
                    corporateName2 = RandomValueGenerator.RandomString(10),
                    corporateBranch = RandomValueGenerator.RandomString(5),
                    idTax = RandomValueGenerator.RandomNumber(10)

                },
                addressHeader = new AddressHeaderModel
                {
                    address1 = "บ้านเลขที่ 11",
                    address2 = "อาคาร ทดสอบ",
                    postalCode = "10210"
                },
                sapVendorInfo = new SapVendorInfoModel
                {
                    sapVendorGroupCode = "DIR"

                   
                }
            };

            //Assert
            var cmd = new buzCRMRegPayeeCorporate();
            var resutl = cmd.Execute(input);

            Assert.IsNotNull(resutl);

           
            Console.WriteLine("==========Result================");
            Console.WriteLine(resutl.ToJson());
            try
            {
                var model = (RegPayeeCorporateContentOutputModel)resutl;

                Assert.AreEqual("200", model.code);
                Assert.AreEqual(true, model.data.Any());

                var data = (RegPayeeCorporateDataOutputModel_Pass) model.data[0];

                Assert.AreEqual(false, string.IsNullOrEmpty(data.cleansingId));
                Assert.AreEqual(false, string.IsNullOrEmpty(data.polisyClientId));
                Assert.AreEqual(false, string.IsNullOrEmpty(data.crmClientId));
                Assert.AreEqual(false, string.IsNullOrEmpty(data.corporateName1));
                Assert.AreEqual(false, string.IsNullOrEmpty(data.corporateName2));
                Assert.AreEqual(false, string.IsNullOrEmpty(data.corporateBranch));
            }
            catch (Exception e)
            {
                var model = (OutputModelFail)resutl;
                
                Assert.Fail(model.message);
            }



        }

        [TestMethod()]
        public void Execute_buzCRMRegPayeeCorporate_ConvertPayeeToNonePayeeTest()
        {

            var inputClient = new RegClientCorporateInputModel
            {
                generalHeader = new Model.RegClientCorporate.GeneralHeaderModel
                {
                    roleCode = "G",
                    polisyClientId = "",
                    cleansingId = "",
                    crmClientId = ""
                },
                profileHeader = new Model.RegClientCorporate.ProfileHeaderModel
                {
                    corporateName1 = RandomValueGenerator.RandomString(10),
                    corporateName2 = RandomValueGenerator.RandomString(10),
                    corporateBranch = RandomValueGenerator.RandomString(5),
                    idTax = RandomValueGenerator.RandomNumber(10)
                },
                addressHeader = new Model.RegClientCorporate.AddressHeaderModel
                {
                    address1 = "บ้านเลขที่ 11",
                    address2 = "อาคาร ทดสอบ",
                    postalCode = "10210"
                }
            };
            Console.WriteLine(inputClient.ToJson());
            var cmdClient = new buzCRMRegClientCorporate();
            var resultClient = cmdClient.Execute(inputClient);
            Console.WriteLine("==========RegClientCorporate. Result================");
            Console.WriteLine(resultClient.ToJson());
            var modelClient = (RegClientCorporateContentOutputModel)resultClient;
            var resutlClientData = (RegClientCorporateDataOutputModel_Pass)modelClient.data[0];

            var cleansingId = resutlClientData.cleansingId;
            var polisyClientId = resutlClientData.polisyClientId;

            //Assert.AreEqual(false, s );
            Assert.AreEqual(false, string.IsNullOrEmpty(cleansingId));
            Assert.AreEqual(false, string.IsNullOrEmpty(polisyClientId));

            Console.WriteLine("==========Start buzCRMRegPayeeCorporate ================");
            var input = new RegPayeeCorporateInputModel
                {
                    generalHeader = new GeneralHeaderModel
                    {
                        cleansingId = ""+ cleansingId,
                        polisyClientId = ""+ polisyClientId,
                        roleCode = "G",
                        crmClientId = "",
                        clientAdditionalExistFlag = "N"

                    },
                    profileHeader = new ProfileHeaderModel
                    {
                        corporateName1 = RandomValueGenerator.RandomString(10),
                        corporateName2 = RandomValueGenerator.RandomString(10),
                        corporateBranch = RandomValueGenerator.RandomString(5),
                        idTax = RandomValueGenerator.RandomNumber(10)

                    },
                    addressHeader = new AddressHeaderModel
                    {
                        address1 = "บ้านเลขที่ 11",
                        address2 = "อาคาร ทดสอบ",
                        postalCode = "10210"
                    },
                    sapVendorInfo = new SapVendorInfoModel
                    {
                        sapVendorGroupCode = "DIR"


                    }
                };
            
            

            //Assert
            var cmd = new buzCRMRegPayeeCorporate();
            var resutl = cmd.Execute(input);

            Assert.IsNotNull(resutl);
            Console.WriteLine("==========Result================");
            Console.WriteLine(resutl.ToJson());

            try
            {
                var model = (RegPayeeCorporateContentOutputModel)resutl;
                

                Assert.AreEqual("200", model.code);
                Assert.AreEqual(true, model.data.Any());
                var data = (RegPayeeCorporateDataOutputModel_Pass)model.data[0];

                Assert.AreEqual(false, string.IsNullOrEmpty(data.cleansingId), " cleansingId Should  Not NullOrEmpty");
                Assert.AreEqual(false, string.IsNullOrEmpty(data.polisyClientId), " polisyClientId Should  Not NullOrEmpty");
                Assert.AreEqual(true, string.IsNullOrEmpty(data.crmClientId), " crmClientId Should   Null");
                Assert.AreEqual(false, string.IsNullOrEmpty(data.corporateName1), " corporateName1 Should  Not NullOrEmpty");
                Assert.AreEqual(false, string.IsNullOrEmpty(data.corporateName2), " corporateName2 Should  Not NullOrEmpty");
                Assert.AreEqual(false, string.IsNullOrEmpty(data.corporateBranch), " corporateBranch Should  Not NullOrEmpty");

                Assert.AreEqual(cleansingId, data.cleansingId);
                Assert.AreEqual(polisyClientId, data.polisyClientId);
            }
            catch (Exception e)
            {
                
                Assert.Fail("ExceptionException"+e.Message);
            }
            



        }


    }
}