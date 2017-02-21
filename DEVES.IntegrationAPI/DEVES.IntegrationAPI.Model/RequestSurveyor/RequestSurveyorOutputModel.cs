using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RequestSurveyor
{
    public class RequestSurveyorOutputModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime transactionDateTime { get; set; }
        public RequestSurveyorDataOutputModel data { get; set; }
    }

    public class RequestSurveyorDataOutputModel
    {
        public string EventID { get; set; }
    }

}
