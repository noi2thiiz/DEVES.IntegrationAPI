using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.AccidentPrilimSurveyorReport
{
    public class AccidentPrilimSurveyorReportInputModel
    {
        public string token { get; set; }
        public string ticketNo { get; set; }
        public string claimNotiNo { get; set; }
        public string reportAccidentResultDate { get; set; }
        public string accidentLefalResult { get; set; }
        public string policeStation { get; set; }
        public string policeRecordId { get; set; }
        public DateTime policeRecordDate { get; set; }
        public string policeBailFlag { get; set; }
        public string demageOfPolicyOwnerCar { get; set; }
        public int numOfTowTruck { get; set; }
        public string nameOfTowCompany { get; set; }
        public string detailOfTowEvent { get; set; }
        public int numOfAccidentInjury { get; set; }
        public string detailOfAccidentInjury { get; set; }
        public int numOfDeath { get; set; }
        public string detailOfDeath { get; set; }
        public string caseOwnercode { get; set; }
        public string caseOwnerFullname { get; set; }
        public string accidentPartyInfo { get; set; }
        public string accidentPartyFullname { get; set; }
        public string accidentPartyPhone { get; set; }
        public string accidentPartyCarPlateNumber { get; set; }
        public string accidentPartyCarModel { get; set; }
        public string accidentPartyInsuredFlag { get; set; }
        public string accidentPartyInsuranceCompany { get; set; }
        public string accidentPartyPolicyType { get; set; }
        public string accidentPartyPolicyNumber { get; set; }
        public DateTime accidentPartyPolicyExpDate { get; set; }
        public string demageOfPartyCar { get; set; }
    }
}
