using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegClientCorporate
{

    public class RegClientCorporateContentOutputModel : BaseContentJsonProxyOutputModel
    {
        public List<RegClientCorporateDataOutputModel> data { set; get; }
    }

    public abstract class RegClientCorporateDataOutputModel : BaseDataModel
    {
    }



    //public class RegClientCorporateOutputModel : BaseContentJsonProxyOutputModel
    //{
    //    public string data { set; get; }
    //}

    //public class RegClientCorporateOutputModel_Pass
    //{
    //    public string code { get; set; }
    //    public string message { get; set; }
    //    public string description { get; set; }
    //    public string transactionId { get; set; }
    //    public string transactionDateTime { get; set; }
    //    public List<RegClientCorporateDataOutputModel_Pass> data { get; set; }
    //}

    public class RegClientCorporateDataOutputModel_Pass : RegClientCorporateDataOutputModel
    {
        public string cleansingId { get; set; }
        public string polisyClientId { get; set; }
        public string crmClientId { get; set; }
        public string corporateName1 { get; set; }
        public string corporateName2 { get; set; }
        public string corporateBranch { get; set; }
    }



    public class RegClientCorporateOutputModel_Fail : BaseContentJsonProxyOutputModel
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public RegClientCorporateDataOutputModel_Fail data { get; set; }
    }

    public class RegClientCorporateDataOutputModel_Fail : RegClientCorporateDataOutputModel
    {
        public List<RegClientCorporateFieldErrors> fieldErrors { get; set; }
    }

    public class RegClientCorporateFieldErrors
    {
        public string name { get; set; }
        public string message { get; set; }

        public RegClientCorporateFieldErrors(string m, string n)
        {
            name = n;
            message = m;
        }
    }
}
