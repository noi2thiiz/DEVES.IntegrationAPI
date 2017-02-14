using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.ClaimRegistration
{
    public class ClaimRegistrationOutputModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime transactionDateTime { get; set; }
        public ClaimRegistrationDataOutputModel data { get; set; }
    }

    public class ClaimRegistrationDataOutputModel
    {
        public string claimId { get; set; }
        public string ticketNumber { get; set; }
    }
}