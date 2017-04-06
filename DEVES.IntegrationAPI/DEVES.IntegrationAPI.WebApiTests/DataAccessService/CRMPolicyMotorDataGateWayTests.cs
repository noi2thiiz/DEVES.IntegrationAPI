using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.DataAccessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.Core.ExtensionMethods;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.Tests
{
    [TestClass()]
    public class CRMPolicyMotorDataGateWayTests
    {
        [TestMethod()]
        public void FetchAllTest()
        {
            CRMPolicyMotorDataGateWay policyDataGateway = new CRMPolicyMotorDataGateWay();
            var reqPolicy = new DataRequest();
            reqPolicy.AddParam("policyNo", "2010034076831");
            reqPolicy.AddParam("chassisNo", "");
            reqPolicy.AddParam("carRegisNo", "ตม4533");
            reqPolicy.AddParam("carRegisProve", "");
            var result = policyDataGateway.FetchAll(reqPolicy);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 1);
        }

        [TestMethod()]
        public void FetchAllTest_It_Shoud_Not_Return_Record()
        {
            CRMPolicyMotorDataGateWay policyDataGateway = new CRMPolicyMotorDataGateWay();
            var reqPolicy = new DataRequest();
            reqPolicy.AddParam("policyNo", "2010034076831");
            reqPolicy.AddParam("chassisNo", "");
            reqPolicy.AddParam("carRegisNo", "ตม4533XXX");
            reqPolicy.AddParam("carRegisProve", "");
            var result = policyDataGateway.FetchAll(reqPolicy);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 0);

            var data = result.Data[0];
            Assert.IsNotNull(data);
        }

        [TestMethod()]
        public void FetchAllTest_It_Shoud_Return_Data_crmPolicyDetailId()
        {
            CRMPolicyMotorDataGateWay policyDataGateway = new CRMPolicyMotorDataGateWay();
            var reqPolicy = new DataRequest();
            reqPolicy.AddParam("policyNo", "2010034076831");
            reqPolicy.AddParam("chassisNo", "");
            reqPolicy.AddParam("carRegisNo", "ตม4533");
            reqPolicy.AddParam("carRegisProve", "");
            var result = policyDataGateway.FetchAll(reqPolicy);

            Assert.Fail();

        }

        [TestMethod()]
        public void FetchAllTest1()
        {
            Assert.Fail();
        }
    }
}