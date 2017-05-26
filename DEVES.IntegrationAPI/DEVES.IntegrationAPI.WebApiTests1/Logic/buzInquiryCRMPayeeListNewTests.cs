using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.Tests
{
    [TestClass()]
    public class buzInquiryCRMPayeeListNewTests
    {
        [TestMethod()]
        public void ExecuteInquiryAPARPayeeListTest()
        {
            AppConfig.Instance.StartupForUnitTest();

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
        public void ExecuteInputTest()
        {
            AppConfig.Instance.StartupForUnitTest();

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

            AppConfig.Instance.StartupForUnitTest();
            var listSearchResult = new List<InquiryCrmPayeeListDataModel>();
            listSearchResult.Add(new InquiryCrmPayeeListDataModel
            {
                cleansingId = "C2017-100000341",
                polisyClientId = ""
            });
            listSearchResult.Add(new InquiryCrmPayeeListDataModel
            {
                cleansingId = "C2017-100000341",
                polisyClientId = "0"
            });
            listSearchResult.Add(new InquiryCrmPayeeListDataModel
            {
                cleansingId = "C2017-100000341"
            });
            listSearchResult.Add(new InquiryCrmPayeeListDataModel
            {
                cleansingId = "C2017-99979999",
                polisyClientId = "0"
            });
            var cmd = new buzInquiryCRMPayeeListNew();
            var result =cmd.FixEmptyPolisyClientId( listSearchResult,"P");

            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
            Assert.AreEqual("16962218", result[0].polisyClientId);
            Assert.AreEqual("16962218", result[1].polisyClientId);
            Assert.AreEqual("16962218", result[2].polisyClientId);
            Assert.AreEqual("", result[3].polisyClientId);

        }
    }
}