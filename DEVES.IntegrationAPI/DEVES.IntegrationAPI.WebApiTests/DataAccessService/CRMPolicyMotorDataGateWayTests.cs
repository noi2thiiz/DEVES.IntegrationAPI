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
        public const string POLICY_NO = "C7121569";
        public const string POLICY_REGIS_NO = "ตค4414";
        public const string POLICY_REGIS_PROVE = "ชม";
        [TestMethod()]
        public void FetchAllTest()
        {
            CRMPolicyMotorDataGateWay policyDataGateway = new CRMPolicyMotorDataGateWay();
            var reqPolicy = new DataRequest();
            reqPolicy.AddParam("policyNo", POLICY_NO);
            reqPolicy.AddParam("chassisNo", "");
            reqPolicy.AddParam("carRegisNo", POLICY_REGIS_NO);
            reqPolicy.AddParam("carRegisProve", POLICY_REGIS_PROVE);
            var result = policyDataGateway.FetchAll(reqPolicy);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 1);
        }

        [TestMethod()]
        public void FetchAllTest_It_Shoud_Not_Return_Record()
        {
            CRMPolicyMotorDataGateWay policyDataGateway = new CRMPolicyMotorDataGateWay();
            var reqPolicy = new DataRequest();
            reqPolicy.AddParam("policyNo", "xxxx");
            reqPolicy.AddParam("chassisNo", "");
            reqPolicy.AddParam("carRegisNo", POLICY_REGIS_NO);
            reqPolicy.AddParam("carRegisProve", POLICY_REGIS_PROVE);
            var result = policyDataGateway.FetchAll(reqPolicy);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 0);

        
        }

        [TestMethod()]
        public void FetchAllTest_It_Shoud_Return_Data_crmPolicyDetailId()
        {
            CRMPolicyMotorDataGateWay policyDataGateway = new CRMPolicyMotorDataGateWay();
            var reqPolicy = new DataRequest();
            reqPolicy.AddParam("policyNo", POLICY_NO);
            reqPolicy.AddParam("chassisNo", "");
            reqPolicy.AddParam("carRegisNo", POLICY_REGIS_NO);
            reqPolicy.AddParam("carRegisProve", POLICY_REGIS_PROVE);
            var result = policyDataGateway.FetchAll(reqPolicy);

            Assert.IsNotNull(result);

        }

     
    }
}