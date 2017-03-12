using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateClaimNo
{
    public class UpdateClaimNoInputModel
    {
        public string ticketNo { get; set; }
        public string claimNotiNo { get; set; }
        public string claimNo { get; set; }
        public string claimId { get; set; }
        public string claimStatusCode { get; set; }
        public string claimStatusDesc { get; set; }
    }
}
