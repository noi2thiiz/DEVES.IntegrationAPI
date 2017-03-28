using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP
{
    public class RegClaimRequestFromRVPInputModel
    {
        public string rvpCliamNo { get; set; }
        public PolicyInfoModel policyInfo { get; set; }
        public PolicyDriverInfoModel policyDriverInfo { get; set; }
        public ClaimInformModel claimInform { get; set; }
        public List<AccidentPartyInfoModel> accidentPartyInfo { get; set; }
    }

    public class PolicyInfoModel
    {
        public string crmPolicyDetailId { get; set; }
        public string crmPolicyDetailCode { get; set; }
        public string policyNo { get; set; }
        public string renewalNo { get; set; }
        public string fleetCarNo { get; set; }
        public string barcode { get; set; }
        public string carChassisNo { get; set; }
        public string currentCarRegisterNo { get; set; }
        public string currentCarRegisterProv { get; set; }
        public string insuredCleansingId { get; set; }
        public string insuredClientType { get; set; }
        public string insuredFullName { get; set; }
        public string insuredClientId { get; set; }
    }

    public class PolicyDriverInfoModel
    {
        public string driverCleansingId { get; set; }
        public string driverFullName { get; set; }
        public string driverClientId { get; set; }
        public string driverMobile { get; set; }
        public string driverPhoneNo { get; set; }
    }

    public class ClaimInformModel
    {
        public string informerOn { get; set; }
        public string accidentOn { get; set; }
        public string accidentDescCode { get; set; }
        public string accidentDesc { get; set; }
        public int numOfExpectInjury { get; set; }
        public string accidentLatitude { get; set; }
        public string accidentLongitude { get; set; }
        public string accidentPlace { get; set; }
        public string accidentProvn { get; set; }
        public string accidentDist { get; set; }
        public string claimType { get; set; }
        public string sendOutSurveyorCode { get; set; }
        public string reportAccidentResultDate { get; set; }
        public string caseOwnerCode { get; set; }
        public string caseOwnerFullName { get; set; }
        public string informByCrmId { get; set; }
        public string informByCrmName { get; set; }
        public string submitByCrmId { get; set; }
        public string submitByCrmName { get; set; }
        public string accidentLegalResult { get; set; }
        public int numOfAccidentInjury { get; set; }
        public int numOfDeath { get; set; }
        public int excessFee { get; set; }
        public int deductibleFee { get; set; }
        public string accidentNatureDesc { get; set; }
        public string policeStation { get; set; }
        public string policeRecordId { get; set; }
        public string policeRecordDate { get; set; }
        public string policeBailFlag { get; set; }
        public string numOfAccidentParty { get; set; }
    }

    public class AccidentPartyInfoModel
    {
        public int rvpAccidentPartySeq { get; set; }
        public string accidentPartyFullname { get; set; }
        public string accidentPartyPhone { get; set; }
        public string accidentPartyCarPlateNo { get; set; }
        public string accidentPartyCarPlateProv { get; set; }
        public string accidentPartyInsuranceCompany { get; set; }
        public string accidentPartyPolicyNumber { get; set; }
        public string accidentPartyPolicyType { get; set; }

    }
}
