using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.inquiryClaimModel
{
    public class inquiryClaimInputModel
    {
        public GeneralHeaderModel generalHeader { get; set; }
        public ConditionModel conditions { get; set; }
    }
    public class GeneralHeaderModel
    {
        public string requester { get; set; }
    }
    public class ConditionModel
    {
        public string cleansingId { get; set; }
        public string claimNotiNo { get; set; }
        public string claimNo { get; set; }
        public string policyNo { get; set; }
        public string policyCarRegisterNo { get; set; } 
        public string parentPolicyId { get; set; }
    }
}
