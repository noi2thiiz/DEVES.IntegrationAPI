using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.AssignedSurveyor
{
    public class AssignedSurveyorInputModel
    {
        public string ticketNo { get; set; }
        public string claimNotiNo { get; set; }
        public string iSurveyStatusOn { get; set; } // DateTime yyyy-MM-dd HH:mm:ss
        public string surveyMeetingLatitude { get; set; }
        public string surveyMeetingLongtitude { get; set; }
        public string surveyMeetingDistrict { get; set; }
        public string surveyMeetingProvince { get; set; }
        public string surveyMeetingPlace { get; set; }
        public string surveyMeetingDate { get; set; } // DateTime yyyy-MM-dd HH:mm:ss
        public string surveyorCode { get; set; }
        public string surveyorClientNumber { get; set; }
        public string surveyorName { get; set; }
        public string surveyorCompanyName { get; set; }
        public string surveyorCompanyMobile { get; set; }
        public string surveyType { get; set; }
        public string surveyTeam { get; set; }
        public string surveyorMobile { get; set; }
    }
}
