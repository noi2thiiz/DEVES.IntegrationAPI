using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.AssignedSurveyor
{
    public class AssignedSurveyorOutputModel_Fail : BaseDataModel
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public List<string> errorMessage { get; set; }
        public AssignedSurveyorDataOutputModel_Fail data { get; set; }
    }

    public class AssignedSurveyorDataOutputModel_Fail
    {
        public List<AssignedSurveyorFieldErrorOutputModel_Fail> fieldError { get; set; }
    }
    
    public class AssignedSurveyorFieldErrorOutputModel_Fail
    {
        public string name { get; set; }
        public string message { get; set; }

        public AssignedSurveyorFieldErrorOutputModel_Fail(string n, string m)
        {
            name = n;
            message = m;
        }
    }
}
