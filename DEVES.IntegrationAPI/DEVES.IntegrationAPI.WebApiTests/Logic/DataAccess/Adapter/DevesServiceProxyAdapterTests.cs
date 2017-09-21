using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.DataAccess.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.DataAccess.Adapter.Tests
{
    [TestClass()]
    public class DevesServiceProxyAdapterTests
    {
        [TestMethod()]
        public void RequestTest() { 

/*
         DevesServiceProxyAdapter DataApepter = new DevesServiceProxyAdapter();
        var inqAPARIn = new InquiryAPARPayeeListInputModel
            {
                polisyClntnum = "",
                fullName = "อัมพร",
                taxNo = "",
                taxBranchCode = "",

                requester = "MOTORCLAIM"
            };


            var result =
                DataApepter.Request<InquiryAPARPayeeOutputModel, InquiryAPARPayeeContentModel>(
                    "EWI_ENDPOINT_APARInquiryPayeeList", inqAPARIn);
            Console.WriteLine(result.ToJson());
            Assert.IsNotNull(result);
            Assert.AreEqual(100, result.aparPayeeListCollection?.Count);
            */
        }

        [TestMethod()]
        public void GetEWIEndpointTest()
        {
            /*
            DevesServiceProxyAdapter dataApepter = new DevesServiceProxyAdapter();
            string endpoint = dataApepter.GetEWIEndpoint("EWI_ENDPOINT_APARInquiryPayeeList");
            Assert.AreEqual("XXX", endpoint);*/
        }
    }
}