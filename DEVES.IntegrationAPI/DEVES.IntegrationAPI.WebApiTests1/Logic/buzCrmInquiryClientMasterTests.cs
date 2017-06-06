using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class buzCrmInquiryClientMasterTests
    {
        [TestMethod()]
        public void Execute_buzCrmInquiryClientMasterTest()
        {
            
            try
            {
                var cmd = new buzCrmInquiryClientMaster();
                var input = new InquiryClientMasterInputModel
                {
                    conditionHeader = new ConditionHeaderModel
                    {
                        clientType="P",
                        roleCode="G"
                    },
                    conditionDetail = new ConditionDetailModel
                    {
                       polisyClientId = "14290379"
                    }
                };
                var result = cmd.Execute(input);
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryClientContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(true, ewiResponse.data.Any());
                Assert.AreEqual("14290379", ewiResponse.data[0].generalHeader.polisyClientId);
               // Assert.AreEqual("CLS", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzCrmInquiryClientMaster_SearchByFullnameTest()
        {
           
            try
            {
                var cmd = new buzCrmInquiryClientMaster();
                var input = new InquiryClientMasterInputModel
                {
                    conditionHeader = new ConditionHeaderModel
                    {
                        clientType = "P",
                        roleCode = "G"
                    },
                    conditionDetail = new ConditionDetailModel
                    {
                        clientFullname = "พรชัย สว่างรุ่งเรือง"
                    }
                };
                var result = cmd.Execute(input);
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryClientContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(true, ewiResponse.data.Any());
                Assert.AreEqual("14290379", ewiResponse.data[0].generalHeader.polisyClientId);
                Assert.AreEqual("พรชัย สว่างรุ่งเรือง", ewiResponse.data[0].profileInfo.fullName);
                // Assert.AreEqual("CLS", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzCrmInquiryClientMaster_SearchByFirstNameAndLastNameTest()
        {
           
            try
            {
                var cmd = new buzCrmInquiryClientMaster();
                var input = new InquiryClientMasterInputModel
                {
                    conditionHeader = new ConditionHeaderModel
                    {
                        clientType = "P",
                        roleCode = "G"
                    },
                    conditionDetail = new ConditionDetailModel
                    {
                        clientName1 = "พรชัย",clientName2 = "สว่างรุ่งเรือง"
                     
                    }
                };
                var result = cmd.Execute(input);
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryClientContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(true, ewiResponse.data.Any());
                Assert.AreEqual("14290379", ewiResponse.data[0].generalHeader.polisyClientId);
                Assert.AreEqual("พรชัย สว่างรุ่งเรือง", ewiResponse.data[0].profileInfo.fullName);
                // Assert.AreEqual("CLS", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzCrmInquiryClientMaster_SearchBySubFullnameTest()
        {
           
            try
            {
                var cmd = new buzCrmInquiryClientMaster();
                var input = new InquiryClientMasterInputModel
                {
                    conditionHeader = new ConditionHeaderModel
                    {
                        clientType = "P",
                        roleCode = "G"
                    },
                    conditionDetail = new ConditionDetailModel
                    {
                        clientFullname = "พร"
                    }
                };
                var result = cmd.Execute(input);
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryClientContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(true, ewiResponse.data.Any());
                Assert.AreEqual(true, ewiResponse.data.Count>20);

                // Assert.AreEqual("CLS", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }


        [TestMethod()]
        public void Execute_buzCrmInquiryClientMaster_SearchBySubFullname_NotFoundTest()
        {
           
            try
            {
                var cmd = new buzCrmInquiryClientMaster();
                var input = new InquiryClientMasterInputModel
                {
                    conditionHeader = new ConditionHeaderModel
                    {
                        clientType = "P",
                        roleCode = "G"
                    },
                    conditionDetail = new ConditionDetailModel
                    {
                        clientFullname = "พรชัยชัยกรรม กรร"
                    }
                };
                var result = cmd.Execute(input);
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryClientContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(false, ewiResponse.data.Any());
               

                // Assert.AreEqual("CLS", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzCrmInquiryClientMaster_SearchByPolisyClientId_NotFoundTest()
        {
           
            try
            {
                var cmd = new buzCrmInquiryClientMaster();
                var input = new InquiryClientMasterInputModel
                {
                    conditionHeader = new ConditionHeaderModel
                    {
                        clientType = "P",
                        roleCode = "G"
                    },
                    conditionDetail = new ConditionDetailModel
                    {
                        polisyClientId = "9999"
                    }
                };
                var result = cmd.Execute(input);
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryClientContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(false, ewiResponse.data.Any());


                // Assert.AreEqual("CLS", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzCrmInquiryClientMaster_SearchByIdCardTest()
        {
            
            try
            {
                var cmd = new buzCrmInquiryClientMaster();
                var input = new InquiryClientMasterInputModel
                {
                    conditionHeader = new ConditionHeaderModel
                    {
                        clientType = "P",
                        roleCode = "G"
                    },
                    conditionDetail = new ConditionDetailModel
                    {
                        idCard = "3500900949465"
                    }
                };
                var result = cmd.Execute(input);
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryClientContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(true, ewiResponse.data.Any());
                Assert.AreEqual("13372519", ewiResponse.data[0].generalHeader.polisyClientId);

                // Assert.AreEqual("CLS", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzCrmInquiryClientMaster_SearchByIdCard_Type_C_Test()
        {
            
            try
            {
                var cmd = new buzCrmInquiryClientMaster();
                var input = new InquiryClientMasterInputModel
                {
                    conditionHeader = new ConditionHeaderModel
                    {
                        clientType = "C",
                        roleCode = "G"
                    },
                    conditionDetail = new ConditionDetailModel
                    {
                        idCard = "0107559000371"
                    }
                };
                var result = cmd.Execute(input);
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                try
                {
                    var ewiResponse = (CRMInquiryClientContentOutputModel)result;

                    Assert.AreEqual("200", ewiResponse.code);
                    Assert.AreEqual(true, ewiResponse.data.Any());
                    Assert.AreEqual("0107559000371", ewiResponse.data[0].profileInfo.idTax);
                }
                catch (Exception e)
                {
                    var ewiResponse = (OutputGenericDataModel<object>)result;

                    Assert.AreEqual("200", ewiResponse.code);
                 
                  

                }
                


                

                // Assert.AreEqual("CLS", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzCrmInquiryClientMaster_SearchByFullname_Type_C_Test()
        {
          
            try
            {
                var cmd = new buzCrmInquiryClientMaster();
                var input = new InquiryClientMasterInputModel
                {
                    conditionHeader = new ConditionHeaderModel
                    {
                        clientType = "C",
                        roleCode = "G"
                    },
                    conditionDetail = new ConditionDetailModel
                    {
                       clientFullname = "ห้างหุ้นส่วนจำกัด บี พรชัยการค้า"
                    }
                };
                var result = cmd.Execute(input);
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryClientContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(true, ewiResponse.data.Any());
                Assert.AreEqual("15814599", ewiResponse.data[0].generalHeader.polisyClientId);
                Assert.AreEqual("0307334800149", ewiResponse.data[0].profileInfo.idTax);

                // Assert.AreEqual("CLS", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzCrmInquiryClientMaster_SearchByPolisyClientId_Type_C_Test()
        {
            
            try
            {
                var cmd = new buzCrmInquiryClientMaster();
                var input = new InquiryClientMasterInputModel
                {
                    conditionHeader = new ConditionHeaderModel
                    {
                        clientType = "C",
                        roleCode = "G"
                    },
                    conditionDetail = new ConditionDetailModel
                    {
                        polisyClientId = "17364357"
                    }
                };
                var result = cmd.Execute(input);
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryClientContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(true, ewiResponse.data.Any());
                Assert.AreEqual("17364357", ewiResponse.data[0].generalHeader.polisyClientId);

                // Assert.AreEqual("CLS", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzCrmInquiryClientMaster_SearchByPolisyClientId_Type_C_NotfoundTest()
        {
            
            try
            {
                var cmd = new buzCrmInquiryClientMaster();
                var input = new InquiryClientMasterInputModel
                {
                    conditionHeader = new ConditionHeaderModel
                    {
                        clientType = "C",
                        roleCode = "G"
                    },
                    conditionDetail = new ConditionDetailModel
                    {
                        polisyClientId = "179999357"
                    }
                };
                var result = cmd.Execute(input);
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryClientContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(false, ewiResponse.data.Any());
        

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }
    }
}
