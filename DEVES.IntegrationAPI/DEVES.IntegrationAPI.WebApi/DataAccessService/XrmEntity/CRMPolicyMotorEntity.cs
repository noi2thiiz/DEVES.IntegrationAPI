using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.XrmEntity
{
    public class CRMPolicyMotorEntity
    {
        public string crmPolicyDetailId { get; set; }
        public string crmPolicyDetailCode { get; set; }
        public string policyNo { get; set; }
        public string renewalNo { get; set; }
        public string fleetCarNo { get; set; }
        public string barcode { get; set; }
        public object insureCardNo { get; set; }
        public bool policyVip { get; set; }
        public string policyAdditionalName { get; set; }
        public object MCM_SEQ { get; set; }
        public string policyId { get; set; }
        public int policyMcNmc { get; set; }
        public string policySeqNo { get; set; }
        public object endorseNo { get; set; }
        public string branchCode { get; set; }
        public string contractType { get; set; }
        public string policyProductTypeCode { get; set; }
        public string policyProductTypeName { get; set; }
        public string policyIssueDate { get; set; }
        public string policyEffectiveDate { get; set; }
        public string policyExpiryDate { get; set; }
        public string policyGarageFlag { get; set; }
        public string policyPaymentStatus { get; set; }
        public string policyCarRegisterNo { get; set; }
        public string policyCarRegisterProv { get; set; }
        public string carChassisNo { get; set; }
        public string carVehicleType { get; set; }
        public string carVehicleModel { get; set; }
        public string carVehicleYear { get; set; }
        public string carVehicleBody { get; set; }
        public string carVehicleSize { get; set; }
        public int policyDeduct { get; set; }
        public string agentCode { get; set; }
        public string agentName { get; set; }
        public object agentBranch { get; set; }
        public object insuredCleansingId { get; set; }
        public string insuredClientId { get; set; }
        public string insuredClientType { get; set; }
        public string insuredFullName { get; set; }
        public string policyStatus { get; set; }
    }
}