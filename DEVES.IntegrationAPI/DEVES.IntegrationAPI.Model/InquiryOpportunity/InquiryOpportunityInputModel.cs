using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.InquiryOpportunity
{
    public class InquiryOpportunityInputModel
    {
        public GeneralHeaderInputModel generalHeader { get; set; }
        public ConditionsModel conditions { get; set; }
    }

    public class GeneralHeaderInputModel
    {
        public string requester { get; set; }
    }

    public class ConditionsModel
    {
        public string opprtunityId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string mobilePhone { get; set; }
        public string businessPhone { get; set; }
        public string fax { get; set; }
        public string productGroup { get; set; }

    }

}
