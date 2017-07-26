using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.InquiryCaseModel
{
    public class inquiryCaseInputModel
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
        public string cleansingId { get; set; }
        public string ticketNo { get; set; }
    }

}
