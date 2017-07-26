using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.personSearchModel
{
    public class personSearchInputModel
    {
        public GeneralHeaderModel generalHeader { get; set; }
        public ConditionModel conditions { get; set; }
    }

    public class GeneralHeaderModel
    {
        public string requester { get; set; }
    }
    public class ConditionModel
    {
        public string fullName { get; set; }                                                                                                                     
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string idCard { get; set; }
        public string crmClientId { get; set; }
        public string line { get; set; }
        public string facebook { get; set; }
        public string cleansingId { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string customerType { get; set; }
    }

}


