using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class buzInquiryCRMPayeeListNewTests
    {
        [TestMethod()]
        public void Execute_InquiryAPARPayeeListTest()
        {
          

            var cmd = new buzInquiryCRMPayeeListNew();
            var result = cmd.InquiryAPARPayeeList(new InquiryCRMPayeeListInputModel
            {
                requester = "MC",
                fullname = "พรชัย"
            });
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void ExecuteInput_buzInquiryCRMPayeeListNewTest()
        {
            

            var cmd = new buzInquiryCRMPayeeListNew();
            var result = cmd.ExecuteInput(new InquiryCRMPayeeListInputModel
            {
                roleCode = "G",
                clientType = "P",
                requester = "MC",
                fullname = "พรชัย ชัน"
            });

            Console.WriteLine("===========Execute Output===============");
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);

        }

        [TestMethod()]
        public void ProcessDistinctTest()
        {
            var cmd = new buzInquiryCRMPayeeListNew();
            var input = new List<InquiryCrmPayeeListDataModel>();
            input.Add(new InquiryCrmPayeeListDataModel
            {
                fullName = "การดา",
                polisyClientId = "4444444",
                sapVendorCode = "11111",
                sapVendorGroupCode = "",
                taxNo = "",
                taxBranchCode = "",
                name1 = "",
                name2 = ""
            });

            input.Add(new InquiryCrmPayeeListDataModel
            {
                fullName = "อภิชาติ",
                polisyClientId = "99999999",
                sapVendorCode = "8888",
                sapVendorGroupCode = "",
                taxNo = "",
                taxBranchCode = "",
                name1 = "",
                name2 = ""
            });

            input.Add(new InquiryCrmPayeeListDataModel
            {
                fullName = "การดา",
                polisyClientId = "",
                sapVendorCode = "",
                sapVendorGroupCode = "",
                taxNo = "",
                taxBranchCode = "",
                name1 = "",
                name2 = "",
                address = "yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy"
            });

            input.Add(new InquiryCrmPayeeListDataModel
            {
                fullName = "การดา",
                polisyClientId = "",
                sapVendorCode = "",
                sapVendorGroupCode = "",
                taxNo = "",
                taxBranchCode = "",
                name1 = "",
                name2 = "",
                address = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
            });

            input.Add(new InquiryCrmPayeeListDataModel
            {
                fullName = "อภิชาติ",
                polisyClientId = "88888888",
                sapVendorCode = "9999",
                sapVendorGroupCode = "",
                taxNo = "",
                taxBranchCode = "",
                name1 = "",
                name2 = ""
            });
            var result = cmd.ProcessDistinct(input);
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);


        }

        [TestMethod()]
        public void ProcessOrderByTest()
        {
            var cmd = new buzInquiryCRMPayeeListNew();
            var input = new List<InquiryCrmPayeeListDataModel>();
            input.Add(new InquiryCrmPayeeListDataModel
            {
                fullName = "การดา",
                polisyClientId = "4444444",
                sapVendorCode = "11111",
                sapVendorGroupCode = "DIR",
                taxNo = "444444",
                taxBranchCode = "HQ",
                name1 = "การดา",
                name2 = ""
            });

            input.Add(new InquiryCrmPayeeListDataModel
            {
                fullName = "อภิชาติ",
                polisyClientId = "99999999",
                sapVendorCode = "8888",
                sapVendorGroupCode = "",
                taxNo = "",
                taxBranchCode = "",
                name1 = "",
                name2 = ""
            });

            input.Add(new InquiryCrmPayeeListDataModel
            {
                fullName = "การดา",
                polisyClientId = "777777",
                sapVendorCode = "3333",
                sapVendorGroupCode = "DIR",
                taxNo = "22222",
                taxBranchCode = "HQ",
                name1 = "การดา",
                name2 = "",
                address = "yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy"
            });

            input.Add(new InquiryCrmPayeeListDataModel
            {
                fullName = "การดา",
                polisyClientId = "777777",
                sapVendorCode = "3333",
                sapVendorGroupCode = "DIR",
                taxNo = "22222",
                taxBranchCode = "HQ",
                name1 = "การดา",
                name2 = "",
                address = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
            });

            input.Add(new InquiryCrmPayeeListDataModel
            {
                fullName = "อภิชาติ",
                polisyClientId = "88888888",
                sapVendorCode = "9999",
                sapVendorGroupCode = "",
                taxNo = "",
                taxBranchCode = "",
                name1 = "",
                name2 = ""
            });
            var resultDistinct = cmd.ProcessDistinct(input);
            var result = cmd.ProcessOrderBy(resultDistinct);
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("การดา", result[0].fullName);
            Assert.AreEqual("การดา", result[1].fullName);

            Assert.AreEqual("3333", result[0].sapVendorCode);
            Assert.AreEqual("11111", result[1].sapVendorCode);


            Assert.AreEqual("อภิชาติ", result[2].fullName);
            Assert.AreEqual("อภิชาติ", result[3].fullName);

            Assert.AreEqual("9999", result[2].sapVendorCode);
            Assert.AreEqual("8888", result[3].sapVendorCode);


        }

        [TestMethod()]
        public void FixEmptyPolisyClientIdTest()
        {

           
            var listSearchResult = new List<InquiryCrmPayeeListDataModel>();
            listSearchResult.Add(new InquiryCrmPayeeListDataModel
            {
                cleansingId = "C2017-100002158",
                polisyClientId = ""
            });
            listSearchResult.Add(new InquiryCrmPayeeListDataModel
            {
                cleansingId = "C2017-100002158",
                polisyClientId = "0"
            });
            listSearchResult.Add(new InquiryCrmPayeeListDataModel
            {
                cleansingId = "C2017-100002158"
            });
            listSearchResult.Add(new InquiryCrmPayeeListDataModel
            {
                cleansingId = "C2017-99979999",
                polisyClientId = "0"
            });
            var cmd = new buzInquiryCRMPayeeListNew();
            var result = cmd.FixEmptyPolisyClientId(listSearchResult, "P");

            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
            Assert.AreEqual("16972697", result[0].polisyClientId,"case 1");
            Assert.AreEqual("16972697", result[1].polisyClientId, "case 2");
            Assert.AreEqual("16972697", result[2].polisyClientId, "case 3");
            Assert.AreEqual("", result[3].polisyClientId, "case 4");

        }

        [TestMethod()]
        public void Execute_buzInquiryCRMPayeeList_LimitOutputTest()
        {
            
            try
            {
                var cmd = new buzInquiryCRMPayeeListNew();
                var result = cmd.ExecuteInput(new InquiryCRMPayeeListInputModel
                {
                    requester = "MC",
                    clientType = "P",
                    roleCode = "G",
                    fullname = "พรชัย"
                });
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
            }
            catch (BuzErrorException e)
            {
                Assert.IsNotNull("คุณระบุเงื่อนไขในการสืบค้นน้อยเกินไป", e.Message);
            }
           
           
        }

        [TestMethod()]
        public void Execute_buzInquiryCRMPayeeList_SearchByFullnameTest()
        {
            
            try
            {
                var cmd = new buzInquiryCRMPayeeListNew();
                var result = cmd.ExecuteInput(new InquiryCRMPayeeListInputModel
                {
                    requester = "MC",
                    clientType = "P",
                    roleCode = "G",
                    fullname = "พรชัย ลือถาวร"
                });
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryPayeeContentOutputModel)result;

             
                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(true, ewiResponse.data.Any());
                Assert.AreEqual("14555916", ewiResponse.data[0].polisyClientId);
                Assert.AreEqual("SAP", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzInquiryCRMPayeeList_SearchBypolisyClientIdTest()
        {
           
            try
            {
                var cmd = new buzInquiryCRMPayeeListNew();
                var result = cmd.ExecuteInput(new InquiryCRMPayeeListInputModel
                {
                    requester = "MC",
                    clientType = "P",
                    roleCode = "G",
                   
                    polisyClientId = "14555916"
                });
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryPayeeContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(true, ewiResponse.data.Any());
                Assert.AreEqual("14555916", ewiResponse.data[0].polisyClientId);
                Assert.AreEqual("SAP", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzInquiryCRMPayeeList_SearchBypolisyClientId_ShouldNotFoundTest()
        {
         
            try
            {
                var cmd = new buzInquiryCRMPayeeListNew();
                var result = cmd.ExecuteInput(new InquiryCRMPayeeListInputModel
                {
                    requester = "MC",
                    clientType = "P",
                    roleCode = "G",

                    polisyClientId = "999999999"
                });
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryPayeeContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(false, ewiResponse.data.Any());
               

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzInquiryCRMPayeeList_SearchByTaxNoTest()
        {
          
            try
            {
                var cmd = new buzInquiryCRMPayeeListNew();
                var result = cmd.ExecuteInput(new InquiryCRMPayeeListInputModel
                {
                    requester = "MC",
                    clientType = "P",
                    roleCode = "G",

                    taxNo = "3100500595490"
                });
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryPayeeContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(true, ewiResponse.data.Any());
                Assert.AreEqual("14555916", ewiResponse.data[0].polisyClientId);
                Assert.AreEqual("SAP", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzInquiryCRMPayeeList_SearchByTaxNo_ShouldNotFoundTest()
        {
           
            try
            {
                var cmd = new buzInquiryCRMPayeeListNew();
                var result = cmd.ExecuteInput(new InquiryCRMPayeeListInputModel
                {
                    requester = "MC",
                    clientType = "P",
                    roleCode = "G",

                    taxNo = "9900500595990"
                });
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryPayeeContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(false, ewiResponse.data.Any());
              

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }


        [TestMethod()]
        public void Execute_buzInquiryCRMPayeeList_SearchRoleCodeA()
        {
            
            try
            {
                var cmd = new buzInquiryCRMPayeeListNew();
                var result = cmd.ExecuteInput(new InquiryCRMPayeeListInputModel
                {
                    requester = "MC",
                    clientType = "C",
                    roleCode = "A",

                    fullname = "พรชัยประเสริฐ"
                });
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryPayeeContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(true, ewiResponse.data.Any());
                Assert.AreEqual("13884834", ewiResponse.data[0].polisyClientId);
                Assert.AreEqual("SAP", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzInquiryCRMPayeeList_SearchRoleCodeS()
        {
           
            try
            {
                var cmd = new buzInquiryCRMPayeeListNew();
                var result = cmd.ExecuteInput(new InquiryCRMPayeeListInputModel
                {
                    requester = "MC",
                    clientType = "C",
                    roleCode = "S",

                    fullname = "พรชัยประเสริฐ"
                });
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryPayeeContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(true, ewiResponse.data.Any());
                Assert.AreEqual("13884834", ewiResponse.data[0].polisyClientId);
                Assert.AreEqual("SAP", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzInquiryCRMPayeeList_SearchRoleCodeR()
        {
          
            try
            {
                var cmd = new buzInquiryCRMPayeeListNew();
                var result = cmd.ExecuteInput(new InquiryCRMPayeeListInputModel
                {
                    requester = "MC",
                    clientType = "C",
                    roleCode = "R",

                    fullname = "พรชัยประเสริฐ"
                });
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryPayeeContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(true, ewiResponse.data.Any());
                Assert.AreEqual("13884834", ewiResponse.data[0].polisyClientId);
                Assert.AreEqual("SAP", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }

        [TestMethod()]
        public void Execute_buzInquiryCRMPayeeList_SearchRoleCodeH()
        {
          
            try
            {
                var cmd = new buzInquiryCRMPayeeListNew();
                var result = cmd.ExecuteInput(new InquiryCRMPayeeListInputModel
                {
                    requester = "MC",
                    clientType = "C",
                    roleCode = "H",

                    fullname = "พรชัยประเสริฐ"
                });
                Console.WriteLine("==============result=============");
                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                var ewiResponse = (CRMInquiryPayeeContentOutputModel)result;


                Assert.AreEqual("200", ewiResponse.code);
                Assert.AreEqual(true, ewiResponse.data.Any());
                Assert.AreEqual("13884834", ewiResponse.data[0].polisyClientId);
                Assert.AreEqual("SAP", ewiResponse.data[0].sourceData);

            }
            catch (BuzErrorException e)
            {

                Assert.Fail();
            }


        }
    }
}