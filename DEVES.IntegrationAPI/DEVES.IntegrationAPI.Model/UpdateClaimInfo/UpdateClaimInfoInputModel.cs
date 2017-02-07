using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateClaimInfo
{
    public class UpdateClaimInfoInputModel
    {

        public string ticketNo { get; set; }
        public string claimNotiNo { get; set; }
        public string claimNo { get; set; }
        public string claimStatusCode { get; set; }
        public string claimStatusDesc { get; set; }
        public string vipCaseFlag { get; set; }
        public string highLossCaseFlag { get; set; }
        public string LegalCaseFlag { get; set; }

    }
}
