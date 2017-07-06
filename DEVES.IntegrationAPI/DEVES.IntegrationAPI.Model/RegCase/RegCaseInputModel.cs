using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegCase
{
    public class RegCaseInputModel
    {
        public class RegLeadInputModel
        {
            public GeneralHeaderModel generalHeader { get; set; }
            public CaseInfoModel caseInfo { get; set; }
        }

        public class GeneralHeaderModel
        {
            public string requester { get; set; }
            public string cleansingId { get; set; }
            public string crmPolicyDetailId { get; set; }
            public string parentCaseNo { get; set; }
        }

        public class CaseInfoModel
        {
            public string caseTitle { get; set; }
            public string caseType { get; set; }
            public string category { get; set; }
            public string subCategory { get; set; }
            public string priority { get; set; }
            public string description { get; set; }
        }

    }
}
