using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP
{
    class RegClaimRequestFromRVPOutputModel
    {
    }

    public class RegClaimRequestFromRVPOutputModel_Pass
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public RegClaimRequestFromRVPDataOutputModel_Pass data { get; set; }
    }

    public class RegClaimRequestFromRVPDataOutputModel_Pass
    {
        public string ticketNo { get; set; }
        public string claimNotiNo { get; set; }
    }

    public class RegClaimRequestFromRVPOutputModel_Fail
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public RegClaimRequestFromRVPDataOutputModel_Fail data { get; set; }
    }

    public class RegClaimRequestFromRVPDataOutputModel_Fail
    {
        public List<RegClaimRequestFromRVPFieldErrors> fieldErrors { get; set; }
    }

    public class RegClaimRequestFromRVPFieldErrors
    {
        public string name { get; set; }
        public string message { get; set; }

        public RegClaimRequestFromRVPFieldErrors(string inName, string inMessage)
        {
            name = inName;
            message = inMessage;
        }
    }

}
