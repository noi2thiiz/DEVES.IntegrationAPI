using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegOpportunity
{
    public class RegOpportunityOutputModel : BaseContentJsonProxyOutputModel
    {
        public RegOpportunityDataModel data { get; set; }
    }

    public class RegOpportunityDataModel
    {
        public string opportunityId { get; set; }
        public string crmClientId { get; set; }
        public string cleansingId { get; set; }
        public string leadId { get; set; }
        public string firstName { get; set; }
    }
}
