using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace DEVES.IntegrationAPI.Model.RegClaimRequestFromClaimDi
{
    public class RegClaimRequestFromClaimDiInputModel
    {
        public string caseNo { get; set; }
        public int tempID { get; set; }
        public string complaintNo { get; set; }
        public string complaintStatus { get; set; }
        public string complaintStep { get; set; }
        public DateTime complaintStepdate { get; set; }

    }
}
