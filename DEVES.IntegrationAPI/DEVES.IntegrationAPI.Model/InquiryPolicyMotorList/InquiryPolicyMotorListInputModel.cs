using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.InquiryPolicyMotorList
{
    public class InquiryPolicyMotorListInputModel
    {
        public string policyNo { get; set; }
        public string carChassisNo { get; set; }
        public string carRegisNo { get; set; }
        public string carRegisProv { get; set; }
    }
}
