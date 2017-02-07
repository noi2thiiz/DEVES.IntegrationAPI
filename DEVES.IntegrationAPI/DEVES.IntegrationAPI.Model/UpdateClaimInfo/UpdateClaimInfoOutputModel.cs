using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateClaimInfo
{
    public class UpdateClaimInfoOutputModel
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime transactionDateTime { get; set; }

        public UpdateClaimInfoDataOutputModel data { get; set; }
    }

    public class UpdateClaimInfoDataOutputModel
    {
        public string itemCode { get; set; }
        public string longDescription { get; set; }
        public string shortDescription { get; set; }
    }
}
