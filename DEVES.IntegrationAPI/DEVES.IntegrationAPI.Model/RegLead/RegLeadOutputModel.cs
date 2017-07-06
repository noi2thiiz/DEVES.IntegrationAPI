using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegLead
{
    public class RegLeadOutputModel : BaseContentJsonProxyOutputModel
    {
        public RegLeadDataModel data { get; set; }
    }

    public class RegLeadDataModel
    {
        public string leadId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}
