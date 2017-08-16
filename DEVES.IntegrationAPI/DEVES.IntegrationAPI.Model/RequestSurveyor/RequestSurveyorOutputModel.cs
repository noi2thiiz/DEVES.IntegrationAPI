using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.ClaimRegistration;
using DEVES.IntegrationAPI.Model.EWI;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.RequestSurveyor
{
    /*
    public class RequestSurveyorOutputModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime transactionDateTime { get; set; }
        public RequestSurveyorDataOutputModel data { get; set; }
    }
    */

    public class RequestSurveyorDataOutputModel : BaseDataModel
    {
        public string eventID { get; set; }
        public string errorMessage { get; set; }
    }

    /*
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
    */


    public class ISurvey_RequestSurveyoOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 1)]
        public ISurvey_RequestSurveyorContentOutputModel content { set; get; }
    }

    public class ISurvey_RequestSurveyorContentOutputModel : BaseContentJsonProxyOutputModel
    {
        [JsonProperty(Order = 2)]
        public ISurvey_RequestSurveyorContentDataOutputModel data { set; get; }
    }

    public class ISurvey_RequestSurveyorContentDataOutputModel
    {
        private EWIResponseContentData _data;


        public ISurvey_RequestSurveyorContentDataOutputModel(EWIResponseContentData contentData)
        {
            _data = contentData;
            this.eventid = contentData.EventID;
        }

        public string eventid { get; set; }
    }

}
