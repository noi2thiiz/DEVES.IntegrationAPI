using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP
{
    class TEST_RegClaimRequestFromRVPOutputModel
    {
    }

    public class TEST_RegClaimRequestFromRVPOutputModel_Pass
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public TEST_RegClaimRequestFromRVPDataOutputModel_Pass data { get; set; }
    }

    public class TEST_RegClaimRequestFromRVPDataOutputModel_Pass
    {
        public string ticketNo { get; set; }
        public string claimNotiNo { get; set; }
    }

    public class TEST_RegClaimRequestFromRVPOutputModel_Fail
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public TEST_RegClaimRequestFromRVPDataOutputModel_Fail data { get; set; }
    }

    public class TEST_RegClaimRequestFromRVPDataOutputModel_Fail
    {
        public List<TEST_RegClaimRequestFromRVPFieldErrors> fieldErrors { get; set; }
    }

    public class TEST_RegClaimRequestFromRVPFieldErrors
    {
        public string name { get; set; }
        public string message { get; set; }

        public TEST_RegClaimRequestFromRVPFieldErrors(string inName, string inMessage)
        {
            name = inName;
            message = inMessage;
        }
    }

    
}
