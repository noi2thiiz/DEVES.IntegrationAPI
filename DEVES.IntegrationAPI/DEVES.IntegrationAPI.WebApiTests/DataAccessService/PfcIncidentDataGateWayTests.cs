using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.DataAccessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.DataAccessService.XrmEntity;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.Tests
{
    [TestClass()]
    public class PfcIncidentDataGateWayTests
    {
        [TestMethod()]
        public void CreateTest()
        {
            // "crmPolicyDetailId": ,

            var policyAdditionalIdGuid = new Guid("3BFCB0A4-DCB6-E611-80CA-0050568D1874");

            var customeGuid = new Guid("b55765f1-c4a4-e611-80ca-0050568d1874");//account
            var informerGuid = new Guid("B55765F1-C4A4-E611-80CA-0050568D1874");//contract

             var driverGuid = new Guid("b55765f1-c4a4-e611-80ca-0050568d1874");//account

            var polisyGuid = new Guid("0EA8FB62-6DB7-E611-80CA-0050568D1874");//contract
            //e3c8d35e-aeb6-e611-80ca-0050568d1874 Does Not Exist
            var incidentEntity = new IncidentEntity(policyAdditionalIdGuid, customeGuid, informerGuid, driverGuid, polisyGuid);
            incidentEntity.caseorigincode = null;
            incidentEntity.pfc_case_vip = false;
            incidentEntity.pfc_policy_additional_number = "C7121677"; /* "policyNo" */
            /*(policyAdditional.pfc_policy_vip) (Policy.pfc_policy_mc_nmc) */
            incidentEntity.pfc_policy_vip = false; // Default 0 ;
            incidentEntity.pfc_policy_mc_nmc = null; //Default(Unassign)
            //(policyAdditional.pfc_reg_num)
            //(policyAdditional.pfc_reg_num_prov)
            incidentEntity.pfc_current_reg_num = ""; //api.currentCarRegisterNo
            incidentEntity.pfc_current_reg_num_prov = "";//api.currentCarRegisterProv
            incidentEntity.casetypecode = new OptionSetValue(2); //fix: 2 ( Service Request )
            incidentEntity.pfc_source_data = new OptionSetValue(100000002);
            incidentEntity.pfc_customer_vip = false; //default 
            //account/contact.pfc_customer_sensitive_level
            incidentEntity.pfc_customer_sensitive = new OptionSetValue(100000000); ; //Low: 100,000,000, Medium:100,000,001, High:100,000,002
            incidentEntity.pfc_customer_privilege = null;



            PfcIncidentDataGateWay db = new PfcIncidentDataGateWay();
            var guid = db.Create(incidentEntity);
            Assert.IsNotNull(guid);
        }
    }
}