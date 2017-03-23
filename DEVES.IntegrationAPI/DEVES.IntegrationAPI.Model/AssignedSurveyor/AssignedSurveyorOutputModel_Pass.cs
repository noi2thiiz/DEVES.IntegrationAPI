using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.AssignedSurveyor
{
    public class AssignedSurveyorOutputModel_Pass
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public AssignedSurveyorDataOutputModel_Pass data { get; set; }
    }

    public class AssignedSurveyorDataOutputModel_Pass
    {
        public string message { get; set; }
    }
}
