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

    public class ISurvey_RequestSurveyorDataOutputModel
    {
        public string gid { get; set; } 
        public string success { get; set; }
        public string responseMessage { get; set; }
        public ISurvey_RequestSurveyorContentDataOutputModel content { get; set; }
        public string username { get; set; }
        public string token { get; set; }
        public string responseCode { get; set; }
    }

    public class ISurvey_RequestSurveyorContentDataOutputModel
    {
        /*
        public ISurvey_RequestSurveyorContentDataOutputModel(EWI.EWIResponseContentData contentData)
        {
            this.eventid = contentData.EventID;
        }
        */
        public string eventid { get; set; }
    }

}
