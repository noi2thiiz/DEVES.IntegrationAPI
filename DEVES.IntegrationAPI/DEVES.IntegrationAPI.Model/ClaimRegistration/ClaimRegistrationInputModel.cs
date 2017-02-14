using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.ClaimRegistration
{

    public class ClaimRegistrationInputModel
    {
        public string caseNo { get; set; }
    }
    public class LocusClaimRegistrationInputModel
    {
        public LocusClaimheaderModel claimHeader { get; set; }
        public LocusClaimtypeModel claimInform { get; set; }
        public LocusClaimassignsurvModel claimAssignSurv { get; set; }
        public LocusClaimsurvinformModel claimSurvInform { get; set; }
    }

    public class LocusClaimheaderModel
    {
        public string ticketNumber { get; set; }
        public string claimNotiNo { get; set; }
        public string claimNotiRefer { get; set; }
        public string policyNo { get; set; }
        public int fleetCarNo { get; set; }
        public int? policySeqNo { get; set; }
        public int? renewalNo { get; set; }
        public string barcode { get; set; }
        public string insureCardNo { get; set; }
        public string policyIssueDate { get; set; }
        public string policyEffectiveDate { get; set; }
        public string policyExpiryDate { get; set; }
        public string policyProductTypeCode { get; set; }
        public string policyProductTypeName { get; set; }
        public string policyGarageFlag { get; set; }
        public string policyPaymentStatus { get; set; }
        public string policyCarRegisterNo { get; set; }
        public string policyCarRegisterProv { get; set; }
        public string carChassisNo { get; set; }
        public string carVehicleType { get; set; }
        public string carVehicleModel { get; set; }
        public string carVehicleYear { get; set; }
        public string carVehicleBody { get; set; }
        public string carVehicleSize { get; set; }
        public int? policyDeduct { get; set; }
        public string vipCaseFlag { get; set; }
        public string privilegeLevel { get; set; }
        public string highLossCaseFlag { get; set; }
        public string legalCaseFlag { get; set; }
        public string claimNotiRemark { get; set; }
        public string claimType { get; set; }
    }

    public class LocusClaimtypeModel
    {
        public string informerClientId { get; set; }
        public string informerFullName { get; set; }
        public string informerMobile { get; set; }
        public string informerPhoneNo { get; set; }
        public string driverClientId { get; set; }
        public string driverFullName { get; set; }
        public string driverMobile { get; set; }
        public string driverPhoneNo { get; set; }
        public string insuredClientId { get; set; }
        public string insuredFullName { get; set; }
        public string insuredMobile { get; set; }
        public string insuredPhoneNo { get; set; }
        public string relationshipWithInsurer { get; set; }
        public string currentCarRegisterNo { get; set; }
        public string currentCarRegisterProv { get; set; }
        public string informerOn { get; set; }
        public string accidentOn { get; set; }
        public string accidentDescCode { get; set; }
        public int? numOfExpectInjury { get; set; }
        public string accidentPlace { get; set; }
        public string accidentLatitude { get; set; }
        public string accidentLongitude { get; set; }
        public string accidentProvn { get; set; }
        public string accidentDist { get; set; }
        public string sendOutSurveyorCode { get; set; }
    }

    public class LocusClaimassignsurvModel
    {
        public string surveyorCode { get; set; }
        public string surveyorClientNumber { get; set; }
        public string surveyorName { get; set; }
        public string surveyorCompanyName { get; set; }
        public string surveyorCompanyMobile { get; set; }
        public string surveyorMobile { get; set; }
        public string surveyorType { get; set; }
        public DateTime? reportAccidentResultDate { get; set; }
    }

    public class LocusClaimsurvinformModel
    {
        public string accidentLegalResult { get; set; }
        public string policeStation { get; set; }
        public string policeRecordId { get; set; }
        public DateTime? policeRecordDate { get; set; }
        public string policeBailFlag { get; set; }
        public string damageOfPolicyOwnerCar { get; set; }
        public int? numOfTowTruck { get; set; }
        public string nameOfTowCompany { get; set; }
        public string detailOfTowEvent { get; set; }
        public int? numOfAccidentInjury { get; set; }
        public string detailOfAccidentInjury { get; set; }
        public int? numOfDeath { get; set; }
        public string detailOfDeath { get; set; }
        public string caseOwnerCode { get; set; }
        public string caseOwnerFullName { get; set; }
        public LocusAccidentpartyinfo[] accidentPartyInfo { get; set; }
    }

    public class LocusAccidentpartyinfo
    {
        public string accidentPartyFullname { get; set; }
        public string accidentPartyPhone { get; set; }
        public string accidentPartyCarPlateNumber { get; set; }
        public string accidentPartyCarModel { get; set; }
        public string accidentPartyInsuredFlag { get; set; }
        public string accidentPartyInsuranceCompany { get; set; }
        public string accidentPartyPolicyType { get; set; }
        public string accidentPartyPolicyNumber { get; set; }
        public DateTime? accidentPartyPolicyExpdate { get; set; }
        public string demageOfPartyCar { get; set; }
    }
}