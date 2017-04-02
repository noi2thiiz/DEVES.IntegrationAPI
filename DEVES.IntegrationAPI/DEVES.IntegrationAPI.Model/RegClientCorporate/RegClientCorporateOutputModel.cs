using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegClientCorporate
{
    public class RegClientCorporateOutputModel : BaseContentJsonProxyOutputModel
    {
        public string data { set; get; }
    }

    public class RegClientCorporateOutputModel_Pass
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public List<RegClientCorporateDataOutputModel_Pass> data { get; set; }
    }

    public class RegClientCorporateDataOutputModel_Pass : BaseDataModel
    {
        public string cleansingId { get; set; }
        public string polisyClientId { get; set; }
        public string crmClientId { get; set; }
        public string corporateName1 { get; set; }
        public string corporateName2 { get; set; }
        public string corporateBranch { get; set; }
    }

    public class RegClientCorporateOutputModel_Fail 
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public List<RegClientCorporateDataOutputModel_Fail> data { get; set; }
    }

    public class RegClientCorporateDataOutputModel_Fail 
    {
        public string fieldErrors { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }
}
