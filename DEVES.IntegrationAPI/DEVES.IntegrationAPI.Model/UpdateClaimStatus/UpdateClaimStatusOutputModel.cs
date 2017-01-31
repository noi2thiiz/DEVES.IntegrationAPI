using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateClaimStatus
{
    public class UpdateClaimStatusOutputModel
    {
        public string code { get; set; }
        public string message { get; set; }
        public string transactionId { get; set; }
        public UpdateClaimStatusDataOutputModel data { get; set; }
    }

    public class UpdateClaimStatusDataOutputModel
    {
        public string itemCode { get; set; }
        public string lognDescription { get; set; }
        public string shortDescription { get; set; }
    }
}
