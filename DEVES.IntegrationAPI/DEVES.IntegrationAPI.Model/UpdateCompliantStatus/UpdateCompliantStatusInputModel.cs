using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateCompliantStatus
{
    public class UpdateCompliantStatusInputModel : BaseDataModel
    {
        public string caseNo { get; set; }
        public int tempID { get; set; }
        public string complaintNo { get; set; }
        public string compliantStatus { get; set; }
        public string compliantStep { get; set; }
        public string complaintStepdate { get; set; }
    }
}
