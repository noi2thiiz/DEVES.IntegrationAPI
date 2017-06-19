using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateSurveyStatus
{
    public class UpdateSurveyStatusOutputModel_Fail
    {
        public string code { get; set; } // service's response code
        public string message { get; set; } // service's response description
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; } // yyyy-MM-dd HH:mm:ss
        public UpdateSurveyStatusDataOutputModel_Fail data { get; set; }
       
    }

    public class UpdateSurveyStatusDataOutputModel_Fail
    {
        public List<UpdateSurveyStatusFieldErrorOutputModel_Fail> fieldError { get; set; }
    }

    public class UpdateSurveyStatusFieldErrorOutputModel_Fail
    {
        public string name { get; set; }
        public string message { get; set; }

        public UpdateSurveyStatusFieldErrorOutputModel_Fail (string n, string m)
        {
            name = n;
            message = m;
        }
    }

}
