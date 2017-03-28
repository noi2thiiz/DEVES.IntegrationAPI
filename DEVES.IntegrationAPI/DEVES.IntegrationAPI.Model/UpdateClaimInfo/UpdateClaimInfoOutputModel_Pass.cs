using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateClaimInfo
{
    public class UpdateClaimInfoOutputModel_Pass
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public UpdateClaimInfoDataOutputModel_Pass data { get; set; }
    }

    public class UpdateClaimInfoDataOutputModel_Pass
    {
        public string message { get; set; }
    }
}
