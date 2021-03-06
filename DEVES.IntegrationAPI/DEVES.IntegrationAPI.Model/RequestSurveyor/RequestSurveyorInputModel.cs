﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RequestSurveyor
{

    public class RequestSurveyorInputModel_WebService : BaseDataModel
    {
        public string incidentId { get; set; }
        public string currentUserId { get; set; }
    }
    public class RequestSurveyorInputModel: BaseEWIRequestContentModel
    {
        public string CaseID { get; set; }
        public string claimNotiNo { get; set; }
        public string claimNotirefer { get; set; }
        public string insureID { get; set; }
        public string rskNo { get; set; }
        public string tranNo { get; set; }
        public string notifyName { get; set; }
        public string mobile { get; set; }
        public string driver { get; set; }
        public string driverTel { get; set; }
        public string currentVehicleLicense { get; set; }
        public string currentProvince { get; set; }
        public DateTime? eventDate { get; set; }
        public DateTime? activityDate { get; set; }
        public string eventDetail { get; set; }
        public string isCasualty { get; set; }
        public string eventLocation { get; set; }
        public string accidentLocation { get; set; }
        public string accidentLat { get; set; }
        public string accidentLng { get; set; }
        public string ISVIP { get; set; }
        public string remark { get; set; }
        public string claimTypeID { get; set; }
        public string subClaimtypeID { get; set; }
        public string empCode { get; set; }
        public string appointLat { get; set; }
        public string appointLong { get; set; }
        public string appointLocation { get; set; }
        public DateTime? appointDate { get; set; }
        public string appointName { get; set; }
        public string appointPhone { get; set; }
        public string contractName { get; set; }
        public string contractPhone { get; set; }
        public string surveyTeam { get; set; }
    }
}
