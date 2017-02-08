using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateSurveyStatus
{
    public class UpdateSurveyStatusOutputModel
    {
        public string code { get; set; } // service's response code
        public string message { get; set; } // service's response description
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime transactionDateTime { get; set; }
        public UpdateSurveyStatusDataOutputModel data { get; set; }
    }

    public class UpdateSurveyStatusDataOutputModel
    {
        public string descItem { get; set; }
        public string longdesc { get; set; }
        public string shortdesc { get; set; }
    }
}
