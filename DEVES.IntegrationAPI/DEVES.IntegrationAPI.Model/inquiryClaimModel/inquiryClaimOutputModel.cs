using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.inquiryClaimModel
{
    public class inquiryClaimOutputModel : BaseContentJsonProxyOutputModel
    {
        public List<inquiryClaimDataOutput> data { get; set; }
    }
    public class inquiryClaimDataOutput
    {
        public inquiryClaimDataOutputPolicyinfo policyInfo { get; set; }
        public inquiryClaimDataOutputClaiminfo claimInfo { get; set; }
    }
    public class inquiryClaimDataOutputPolicyinfo
    {
        public string policyNo {get; set;}
        public string renewalNo {get; set;}
        public string fleetCarNo { get; set;}
        public string barcode { get; set;}
        public string insureCardNo {get; set;}
        public string policySeqNo {get; set;}
        public string branchCode {get; set;}
        public string contractType {get; set;}
        public string policyProductTypeCode { get; set;}
        public string policyProductTypeName{ get; set;}
        public DateTime policyIssueDate{ get; set;}
        public DateTime policyEffectiveDate{ get; set;}
        public DateTime policyExpiryDate{ get; set;}
        public string policyGarageFlag{ get; set;}
        public string policyPaymentStatus{ get; set;}
        public string policyCarRegisterNo{ get; set;}
        public string policyCarRegisterProv{ get; set;}
        public string policyStatus{ get; set;}
    }
    public class inquiryClaimDataOutputClaiminfo
    {
        public string driverFullName { get; set; }
        public string driverClientId { get; set; }
        public string driverMobile { get; set; }
        public string ticketNumber { get; set; }
        public string claimNotiNo { get; set; }
        public string claimNo { get; set; }
        public string agentCode { get; set; }
        public string agentName { get; set; }
        public string agentBranch { get; set; }
        public string informerFullName { get; set; }
        public string informerCleintId { get; set; }
        public string informerMobile { get; set; }
        public DateTime informerOn { get; set; }
        public DateTime accidentOn { get; set; }
        public string accidentDescCode { get; set; }
        public string accidnetDesc { get; set; }
        public string numOfExpectInjury { get; set; }
        public string accidentPlace { get; set; }
        public string accidentLatitude { get; set; }
        public string accidentLongitude { get; set; }
        public string accidentProvName { get; set; }
        public string accidentDistName { get; set; }
        public string sendOutSurveyorCode { get; set; }
    }
}
