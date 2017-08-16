using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateSurveyStatus
{
    public class UpdateSurveyStatusOutputModel_Pass : BaseDataModel
    {
        public string code { get; set; } // service's response code
        public string message { get; set; } // service's response description
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public List<string> errorMessage { get; set; }
        public UpdateSurveyStatusDataOutputModel_Pass data { get; set; }
    }

    public class UpdateSurveyStatusDataOutputModel_Pass
    {
        public string message { get; set; }
    }

}
