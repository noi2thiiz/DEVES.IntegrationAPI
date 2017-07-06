using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateOpportunity
{
    public class UpdateOpportunityOutputModel : BaseContentJsonProxyOutputModel
    {
        public UpdateOpportunityDataModel data { get; set; }
    }

    public class UpdateOpportunityDataModel
    {
        public string opportunityId { get; set; }
        public string crmClientId { get; set; }
        public string cleansingId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}
