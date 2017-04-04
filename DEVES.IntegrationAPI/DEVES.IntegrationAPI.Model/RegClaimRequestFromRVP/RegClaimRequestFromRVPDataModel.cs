using System.Collections.Generic;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP
{
    public class CRMregClaimRequestFromRVPInputModel:BaseDataModel
    {
        public string rvpCliamNo { get; set; }
        public PolicyInfoModel policyInfo { get; set; }
        public PolicyDriverInfoModel policyDriverInfo { get; set; }
        public ClaimInformModel claimInform { get; set; }
        public List<AccidentPartyInfoModel> accidentPartyInfo { get; set; }
    }

    public class CrmregClaimRequestFromRVPContentOutputModel : BaseContentJsonProxyOutputModel
    {
        [JsonProperty(Order=20)]
        public CrmRegClaimRequestFromRVPDataOutputModel data { set; get; }
    }

    public class CrmRegClaimRequestFromRVPDataOutputModel:BaseDataModel
    {
        public string ticketNo { get; set; }
        public string claimNotiNo { get; set; }
    }




}