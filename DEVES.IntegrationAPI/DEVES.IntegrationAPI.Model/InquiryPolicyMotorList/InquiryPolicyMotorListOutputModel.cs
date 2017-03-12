using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.InquiryPolicyMotorList
{
    class InquiryPolicyMotorListOutputModel
    {
    }

    public class InquiryPolicyMotorListOutputModel_Pass
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public InquiryPolicyMotorListDataOutputModel_Pass data { get; set; }
    }

    public class InquiryPolicyMotorListDataOutputModel_Pass
    {
        public string crmPolicyDetailId { get; set; }
        public string crmPolicyDetailCode { get; set; }
        public string policyNo { get; set; }
        public string renewalNo { get; set; }
        public string fleetCarNo { get; set; }
        public string barcode { get; set; }
        public string insureCardNo { get; set; }
        public string policySeqNo { get; set; }
        public string endorseNo { get; set; }
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
        public string policyDeduct { get; set; }
        public string agentCode { get; set; }
        public string agentName { get; set; }
        public string agentBranch { get; set; }
        public string insuredCleansingId { get; set; }
        public string insuredClientId { get; set; }
        public string insuredClientType { get; set; }
        public string insuredFullName { get; set; }
        public string policyStatus { get; set; }
    }

    public class InquiryPolicyMotorListOutputModel_Fail
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public InquiryPolicyMotorListDataOutputModel_Fail data { get; set; }
    }

    public class InquiryPolicyMotorListDataOutputModel_Fail
    {
        public string fieldErrors { get; set; }
        public string message { get; set; }

    }

}
