using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.AssignedSurveyor
{
    public class AssignedSurveyorOutputModel
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime transactionDateTime { get; set; }
        public AssignedSurveyorDataOutputModel data { get; set; }
    }

    public class AssignedSurveyorDataOutputModel
    {
        public string descItem { get; set; }
        public string longdesc { get; set; }
        public string shortdesc { get; set; }
    }
}
