using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateCompliantStatus
{
    public class UpdateCompliantStatusInputModel
    {
        public string caseNo { get; set; }
        public int tempID { get; set; }
        public string complaintNo { get; set; }
        public char compliantStatus { get; set; }
        public string compliantStep { get; set; }
        public DateTime complaintStepdate { get; set; }
    }
}
