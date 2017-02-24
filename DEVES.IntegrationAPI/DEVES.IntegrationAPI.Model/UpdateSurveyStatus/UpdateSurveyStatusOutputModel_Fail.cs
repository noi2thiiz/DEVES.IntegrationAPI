using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateSurveyStatus
{
    public class UpdateSurveyStatusOutputModel_Fail
    {
        public int code { get; set; } // service's response code
        public string message { get; set; } // service's response description
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime transactionDateTime { get; set; }
        public UpdateSurveyStatusDataOutputModel_Fail data { get; set; }
    }

    public class UpdateSurveyStatusDataOutputModel_Fail
    {
        public UpdateSurveyStatusFieldErrorOutputModel_Fail fieldError { get; set; }
    }

    public class UpdateSurveyStatusFieldErrorOutputModel_Fail
    {
        public string name { get; set; }
        public string message { get; set; }
    }

}
