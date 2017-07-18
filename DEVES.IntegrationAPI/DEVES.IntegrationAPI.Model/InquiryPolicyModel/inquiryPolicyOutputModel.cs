using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.InquiryPolicyModel
{
    public class inquiryPolicyOutputModel : BaseContentJsonProxyOutputModel
    {
        public List<inquiryPolicyDataOutput> data { get; set; }
    }

    public class inquiryPolicyDataOutput
    {
        [JsonIgnore]
        public string policyGuid { get; set; }

       public inquiryPolicyDataOutputPolicy policyInfo { get; set;  }
       public List<inquiryPolicyDataOutputClaims> claims { get; set; }

        [JsonIgnore]
        public DataRow rawData { get; set; }
    }
        public class inquiryPolicyDataOutputPolicy :BaseDataModel
    {
        public string crmPolicyDetailId { get; set; }
        public string policyNo { get; set; }
        public string renewalNo { get; set; }
        public string fleetCarNo { get; set; }
        public string barcode { get; set; }
        public string insureCardNo { get; set; }
        public string policySeqNo { get; set; }
        public string branchCode { get; set; }
        public string contractType { get; set; }
        public string policyProductTypeCode { get; set; }
        public string policyProductTypeName { get; set; }
        public DateTime policyIssueDate { get; set; }
        public DateTime policyEffectiveDate { get; set; }
        public DateTime policyExpiryDate { get; set; }
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
    public class inquiryPolicyDataOutputClaims : BaseDataModel
    {
        public string driverFullName { get; set; }
        public string driverCleansingId { get; set; }
        public string driverClientId { get; set; }
        public string claimNo { get; set; }
        public string customerName { get; set; }
        public string claimNotiNo { get; set; }
        public string claimAgentCode { get; set; }
        public string claimAgentName { get; set; }
        public string claimAgentBranch { get; set; }
        public DateTime claimOpenDate { get; set; }
        public string claimStatus { get; set; }
    }
}
