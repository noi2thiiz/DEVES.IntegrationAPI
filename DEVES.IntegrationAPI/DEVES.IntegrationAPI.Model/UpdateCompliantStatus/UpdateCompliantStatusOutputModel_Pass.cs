using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateCompliantStatus
{
    public class UpdateCompliantStatusOutputModel_Pass : BaseDataModel
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime transactionDateTime { get; set; }
        public UpdateCompliantStatusDataOutputModel_Pass data { get; set; }
    }

    public class UpdateCompliantStatusDataOutputModel_Pass : BaseDataModel
    {
        public string message { get; set; }
    }
}
