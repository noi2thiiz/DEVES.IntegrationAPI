using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.RegClaimRequestFromClaimDi
{
    public class RegClaimRequestFromClaimDiOutputModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime? transactionDateTime { get; set; }
        public RegClaimRequestFromClaimDiDataOutputModel data { get; set; }
    }

    public class RegClaimRequestFromClaimDiDataOutputModel
    {
    }

}
