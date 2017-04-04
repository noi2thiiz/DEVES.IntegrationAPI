using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.ClaimRegistration
{

    public class ClaimRegistrationInputModel
    {
        [CrmMapping(FieldName = "caseNo", Source = ENUMDataSource.srcSQL)]
        public string caseNo { get; set; }

        public Guid IncidentId { get; set; }

        public Guid CurrentUserId { get; set; }

    }

    [CrmClassToMapData]
    public class LocusClaimRegistrationInputModel: BaseDataModel
    {
        public LocusClaimheaderModel claimHeader { get; set; }
        public LocusClaiminformModel claimInform { get; set; }
        public LocusClaimassignsurvModel claimAssignSurv { get; set; }
        public LocusClaimsurvinformModel claimSurvInform { get; set; }

    }

    /*

    [CrmClassToMapData]
    public class LocusClaimheaderModel: BaseDataModel
    {
        public string premiumClass { get; set; }
        public string teamCd { get; set; }
        // public string claimStatus { get; set; }
        public string ticketNumber { get; set; }
        public string claimNotiNo { get; set; }
        public string claimNotiRefer { get; set; }
        public string policyNo { get; set; }
        public int fleetCarNo { get; set; }
        public int? policySeqNo { get; set; }
        public int? renewalNo { get; set; }
        public string barcode { get; set; }
        public string insureCardNo { get; set; }
        public DateTime? policyIssueDate { get; set; }
        public DateTime? policyEffectiveDate { get; set; }
        public DateTime? policyExpiryDate { get; set; }
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
        public string serviceBranch { get; set; }
    }

    [CrmClassToMapData]
    public class LocusClaiminformModel: BaseDataModel
    {
        public string accidentDesc { get; set; }
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
        public DateTime? informerOn { get; set; }
        public DateTime? accidentOn { get; set; }
        public string accidentDescCode { get; set; }
        public int? numOfExpectInjury { get; set; }
        public string accidentPlace { get; set; }
        public string accidentLatitude { get; set; }
        public string accidentLongitude { get; set; }
        public string sendOutSurveyorCode { get; set; }
        public string accidentProvn { get; set; }
        public string accidentDist { get; set; }
    }

    [CrmClassToMapData]
    public class LocusClaimassignsurvModel: BaseDataModel
    {
        public string surveyorCode { get; set; }
        public string surveyorClientNumber { get; set; }
        public string surveyorName { get; set; }
        public string surveyorCompanyName { get; set; }
        public string surveyorCompanyMobile { get; set; }
        public string surveyorMobile { get; set; }
        public string surveyorType { get; set; }
        public string surveyTeam { get; set; }
        /
        //public DateTime? reportAccidentResultDate { get; set; }
        //public string branchSurvey { get; set; }
        //public string latitudeLongitude { get; set; }
        //public string location { get; set; }
        //public string createBy { get; set; }
        //public DateTime? createDate { get; set; } // DateTime format?
        //public string updateBy { get; set; }
        //public DateTime? updateDate { get; set; } // DateTime format?
        
    }

    [CrmClassToMapData]
    public class LocusClaimsurvinformModel: BaseDataModel
    {
        public int excessFee { get; set; }
        public int deductibleFee { get; set; }
        public DateTime? reportAccidentResultDate { get; set; }
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
        // this part added 
        public string accidentPartyFullname { get; set; }
        public string accidentPartyPhone { get; set; }
        public string accidentPartyCarPlateNumber { get; set; }
        public string accidentPartyCarModel { get; set; }
        public string accidentPartyInsuredFlag { get; set; }
        public string accidentPartyInsuranceCompany { get; set; }
        public string accidentPartyPolicyType { get; set; }
        public string accidentPartyPolicyNumber { get; set; }
        public DateTime? accidentPartyPolicyExpdate { get; set; }
        public string damageOfPartyCar { get; set; }
        // public LocusAccidentpartyinfo[] accidentPartyInfo { get; set; }
    }

    [CrmClassToMapData]
    public class LocusAccidentpartyinfo: BaseDataModel
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
        public string damageOfPartyCar { get; set; }
    }*/



    public class LocusClaimheaderModel : BaseDataModel
    {
        [CrmMapping(FieldName = "premiumClass", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_prod_type", Source = ENUMDataSource.srcCrm)]
        public String premiumClass { set; get; }
        [CrmMapping(FieldName = "ticketNumber", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "ticketnumber", Source = ENUMDataSource.srcCrm)]
        public String ticketNumber { set; get; }
        [CrmMapping(FieldName = "claimNotiNo", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_claim_noti_number", Source = ENUMDataSource.srcCrm)]
        public String claimNotiNo { set; get; }
        [CrmMapping(FieldName = "claimNotiRefer", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_claim_noti_number", Source = ENUMDataSource.srcCrm)]
        public String claimNotiRefer { set; get; }
        [CrmMapping(FieldName = "policyNo", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_policy_number", Source = ENUMDataSource.srcCrm)]
        public String policyNo { set; get; }
        [CrmMapping(FieldName = "fleetCarNo", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_rsk_num", Source = ENUMDataSource.srcCrm)]
        public int? fleetCarNo { set; get; }
        [CrmMapping(FieldName = "policySeqNo", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_tran_num", Source = ENUMDataSource.srcCrm)]
        public int? policySeqNo { set; get; }
        [CrmMapping(FieldName = "renewalNo", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_zren_num", Source = ENUMDataSource.srcCrm)]
        public int? renewalNo { set; get; }
        [CrmMapping(FieldName = "barcode", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_bar_code", Source = ENUMDataSource.srcCrm)]
        public String barcode { set; get; }
        [CrmMapping(FieldName = "insureCardNo", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_insurance_card", Source = ENUMDataSource.srcCrm)]
        public String insureCardNo { set; get; }
        [CrmMapping(FieldName = "policyIssueDate", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_issue_date", Source = ENUMDataSource.srcCrm)]
        public DateTime? policyIssueDate { set; get; }
        [CrmMapping(FieldName = "policyEffectiveDate", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_effective_date", Source = ENUMDataSource.srcCrm)]
        public DateTime? policyEffectiveDate { set; get; }
        [CrmMapping(FieldName = "policyExpiryDate", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_expiry_date", Source = ENUMDataSource.srcCrm)]
        public DateTime? policyExpiryDate { set; get; }
        [CrmMapping(FieldName = "policyProductTypeCode", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_cover_code", Source = ENUMDataSource.srcCrm)]
        public String policyProductTypeCode { set; get; }
        [CrmMapping(FieldName = "policyProductTypeName", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_cover_name", Source = ENUMDataSource.srcCrm)]
        public String policyProductTypeName { set; get; }
        [CrmMapping(FieldName = "policyGarageFlag", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_garage_flag", Source = ENUMDataSource.srcCrm)]
        public String policyGarageFlag { set; get; }
        [CrmMapping(FieldName = "policyPaymentStatus", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_receive_status", Source = ENUMDataSource.srcCrm)]
        public String policyPaymentStatus { set; get; }
        [CrmMapping(FieldName = "policyCarRegisterNo", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_reg_num", Source = ENUMDataSource.srcCrm)]
        public String policyCarRegisterNo { set; get; }
        [CrmMapping(FieldName = "policyCarRegisterProv", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_reg_num_prov", Source = ENUMDataSource.srcCrm)]
        public String policyCarRegisterProv { set; get; }
        [CrmMapping(FieldName = "carChassisNo", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_chassis_num", Source = ENUMDataSource.srcCrm)]
        public String carChassisNo { set; get; }
        [CrmMapping(FieldName = "carVehicleType", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_vehicle_type", Source = ENUMDataSource.srcCrm)]
        public String carVehicleType { set; get; }
        [CrmMapping(FieldName = "carVehicleModel", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_vehicle_brand_model", Source = ENUMDataSource.srcCrm)]
        public String carVehicleModel { set; get; }
        [CrmMapping(FieldName = "carVehicleYear", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_vehicle_year", Source = ENUMDataSource.srcCrm)]
        public String carVehicleYear { set; get; }
        [CrmMapping(FieldName = "carVehicleBody", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_vehicle_body", Source = ENUMDataSource.srcCrm)]
        public String carVehicleBody { set; get; }
        [CrmMapping(FieldName = "carVehicleSize", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_vehicle_size", Source = ENUMDataSource.srcCrm)]
        public String carVehicleSize { set; get; }
        [CrmMapping(FieldName = "policyDeduct", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_deduct", Source = ENUMDataSource.srcCrm)]
        public int? policyDeduct { set; get; }
        [CrmMapping(FieldName = "agentCode", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_agent_code", Source = ENUMDataSource.srcCrm)]
        public String agentCode { set; get; }
        [CrmMapping(FieldName = "agentName", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_agent_fullname", Source = ENUMDataSource.srcCrm)]
        public String agentName { set; get; }
        [CrmMapping(FieldName = "agentBranch ", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_agent_branch", Source = ENUMDataSource.srcCrm)]
        public String agentBranch { set; get; }
        [CrmMapping(FieldName = "vipCaseFlag", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_customer_vip", Source = ENUMDataSource.srcCrm)]
        public String vipCaseFlag { set; get; }
        [CrmMapping(FieldName = "privilegeLevel", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_customer_privilege", Source = ENUMDataSource.srcCrm)]
        public String privilegeLevel { set; get; }
        [CrmMapping(FieldName = "highLossCaseFlag", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_high_loss_case_flag", Source = ENUMDataSource.srcCrm)]
        public String highLossCaseFlag { set; get; }
        [CrmMapping(FieldName = "legalCaseFlag", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_legal_case_flag", Source = ENUMDataSource.srcCrm)]
        public String legalCaseFlag { set; get; }
        [CrmMapping(FieldName = "claimNotiRemark", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_case_of_claim_remark", Source = ENUMDataSource.srcCrm)]
        public String claimNotiRemark { set; get; }
        [CrmMapping(FieldName = "claimType", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_claim_type", Source = ENUMDataSource.srcCrm)]
        public String claimType { set; get; }
        [CrmMapping(FieldName = "informByCrmId", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_send_request_survey_by", Source = ENUMDataSource.srcCrm)]
        public String informByCrmId { set; get; }
        [CrmMapping(FieldName = "informByCrmName", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_send_request_survey_by", Source = ENUMDataSource.srcCrm)]
        public String informByCrmName { set; get; }
        [CrmMapping(FieldName = "submitByCrmId", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "currentUser", Source = ENUMDataSource.srcCrm)]
        public String submitByCrmId { set; get; }
        [CrmMapping(FieldName = "submitByCrmName", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "currentUser", Source = ENUMDataSource.srcCrm)]
        public String submitByCrmName { set; get; }
        [CrmMapping(FieldName = "serviceBranch", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "currentUser", Source = ENUMDataSource.srcCrm)]
        public String serviceBranch { set; get; }
    }
    public class LocusClaiminformModel : BaseDataModel
    {
        [CrmMapping(FieldName = "informerClientId", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_informer_client_number", Source = ENUMDataSource.srcCrm)]
        public String informerClientId { set; get; }
        [CrmMapping(FieldName = "informerFullName", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_informer_nameName", Source = ENUMDataSource.srcCrm)]
        public String informerFullName { set; get; }
        [CrmMapping(FieldName = "informerMobile", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_informer_mobile", Source = ENUMDataSource.srcCrm)]
        public String informerMobile { set; get; }
        [CrmMapping(FieldName = "informerPhoneNo", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "0", Source = ENUMDataSource.srcCrm)]
        public String informerPhoneNo { set; get; }
        [CrmMapping(FieldName = "driverClientId", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_driver_client_number", Source = ENUMDataSource.srcCrm)]
        public String driverClientId { set; get; }
        [CrmMapping(FieldName = "driverFullName", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_driver_nameName", Source = ENUMDataSource.srcCrm)]
        public String driverFullName { set; get; }
        [CrmMapping(FieldName = "driverMobile", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_driver_mobile", Source = ENUMDataSource.srcCrm)]
        public String driverMobile { set; get; }
        [CrmMapping(FieldName = "driverPhoneNo", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "0", Source = ENUMDataSource.srcCrm)]
        public String driverPhoneNo { set; get; }
        [CrmMapping(FieldName = "insuredClientId", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_cus_client_number", Source = ENUMDataSource.srcCrm)]
        public String insuredClientId { set; get; }
        [CrmMapping(FieldName = "insuredFullName", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_customerId", Source = ENUMDataSource.srcCrm)]
        public String insuredFullName { set; get; } // pfc_agent_fullname
        [CrmMapping(FieldName = "insuredMobile", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "0", Source = ENUMDataSource.srcCrm)]
        public String insuredMobile { set; get; }
        [CrmMapping(FieldName = "insuredPhoneNo", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "0", Source = ENUMDataSource.srcCrm)]
        public String insuredPhoneNo { set; get; }
        [CrmMapping(FieldName = "relationshipWithInsurer", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_relation_cutomer_accident_party", Source = ENUMDataSource.srcCrm)]
        public String relationshipWithInsurer { set; get; }
        [CrmMapping(FieldName = "currentCarRegisterNo", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_current_reg_num", Source = ENUMDataSource.srcCrm)]
        public String currentCarRegisterNo { set; get; }
        [CrmMapping(FieldName = "currentCarRegisterProv", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_current_reg_num_prov", Source = ENUMDataSource.srcCrm)]
        public String currentCarRegisterProv { set; get; }
        [CrmMapping(FieldName = "informerOn", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_isurvey_status_on", Source = ENUMDataSource.srcCrm)]
        public DateTime? informerOn { set; get; }
        [CrmMapping(FieldName = "accidentOn", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_accident_on", Source = ENUMDataSource.srcCrm)]
        public DateTime? accidentOn { set; get; }
        [CrmMapping(FieldName = "accidentDescCode", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_accident_desc_code", Source = ENUMDataSource.srcCrm)]
        public String accidentDescCode { set; get; }
        [CrmMapping(FieldName = "accidentDesc", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_accident_desc", Source = ENUMDataSource.srcCrm)]
        public String accidentDesc { set; get; }
        [CrmMapping(FieldName = "numOfExpectInjury", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_num_of_expect_injuries", Source = ENUMDataSource.srcCrm)]
        public int? numOfExpectInjury { set; get; }
        [CrmMapping(FieldName = "accidentPlace", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_accident_place", Source = ENUMDataSource.srcCrm)]
        public String accidentPlace { set; get; }
        [CrmMapping(FieldName = "accidentLatitude", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_accident_latitude", Source = ENUMDataSource.srcCrm)]
        public String accidentLatitude { set; get; }
        [CrmMapping(FieldName = "accidentLongitude", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_accident_longitude", Source = ENUMDataSource.srcCrm)]
        public String accidentLongitude { set; get; }
        [CrmMapping(FieldName = "accidentProvn", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "0", Source = ENUMDataSource.srcCrm)]
        public String accidentProvn { set; get; }
        [CrmMapping(FieldName = "accidentDist", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "0", Source = ENUMDataSource.srcCrm)]
        public String accidentDist { set; get; }
        [CrmMapping(FieldName = "sendOutSurveyorCode", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_send_out_surveyor", Source = ENUMDataSource.srcCrm)]
        public String sendOutSurveyorCode { set; get; }
    }
    public class LocusClaimassignsurvModel : BaseDataModel
    {
        [CrmMapping(FieldName = "surveyorCode", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_surveyor_code", Source = ENUMDataSource.srcCrm)]
        public String surveyorCode { set; get; }
        [CrmMapping(FieldName = "surveyorClientNumber", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_surveyor_client_number", Source = ENUMDataSource.srcCrm)]
        public String surveyorClientNumber { set; get; }
        [CrmMapping(FieldName = "surveyorName", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_surveyor_name", Source = ENUMDataSource.srcCrm)]
        public String surveyorName { set; get; }
        [CrmMapping(FieldName = "surveyorCompanyName", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_surveyor_company_name", Source = ENUMDataSource.srcCrm)]
        public String surveyorCompanyName { set; get; }
        [CrmMapping(FieldName = "surveyorCompanyMobile", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_surveyor_company_mobile", Source = ENUMDataSource.srcCrm)]
        public String surveyorCompanyMobile { set; get; }
        [CrmMapping(FieldName = "surveyorMobile", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "0", Source = ENUMDataSource.srcCrm)]
        public String surveyorMobile { set; get; }
        [CrmMapping(FieldName = "surveyorType", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_surveyor_type", Source = ENUMDataSource.srcCrm)]
        public String surveyorType { set; get; }
        [CrmMapping(FieldName = "surveyTeam", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_surveyor_team", Source = ENUMDataSource.srcCrm)]
        public String surveyTeam { set; get; }
    }
    public class LocusClaimsurvinformModel : BaseDataModel
    {
        [CrmMapping(FieldName = "deductibleFee", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_deductable_fee", Source = ENUMDataSource.srcCrm)]
        public int? deductibleFee { set; get; }
        [CrmMapping(FieldName = "excessFee", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_excess_fee", Source = ENUMDataSource.srcCrm)]
        public int? excessFee { set; get; }
        [CrmMapping(FieldName = "reportAccidentResultDate", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_accident_prilim_surveyor_report_date", Source = ENUMDataSource.srcCrm)]
        public DateTime? reportAccidentResultDate { set; get; }
        [CrmMapping(FieldName = "accidentLegalResult", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_accident_legal_result", Source = ENUMDataSource.srcCrm)]
        public String accidentLegalResult { set; get; }
        [CrmMapping(FieldName = "policeStation", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_police_station", Source = ENUMDataSource.srcCrm)]
        public String policeStation { set; get; }
        [CrmMapping(FieldName = "policeRecordId", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_police_record_id", Source = ENUMDataSource.srcCrm)]
        public String policeRecordId { set; get; }
        [CrmMapping(FieldName = "policeRecordDate", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_police_record_date", Source = ENUMDataSource.srcCrm)]
        public DateTime? policeRecordDate { set; get; }
        [CrmMapping(FieldName = "policeBailFlag", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_police_bail_flag", Source = ENUMDataSource.srcCrm)]
        public String policeBailFlag { set; get; }
        [CrmMapping(FieldName = "damageOfPolicyOwnerCar", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_demage_of_policy_owner_car", Source = ENUMDataSource.srcCrm)]
        public String damageOfPolicyOwnerCar { set; get; }
        [CrmMapping(FieldName = "numOfTowTruck", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_num_of_tow_truck", Source = ENUMDataSource.srcCrm)]
        public int? numOfTowTruck { set; get; }
        [CrmMapping(FieldName = "nameOfTowCompany", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_name_of_tow_company", Source = ENUMDataSource.srcCrm)]
        public String nameOfTowCompany { set; get; }
        [CrmMapping(FieldName = "detailOfTowEvent", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_detail_of_tow_event", Source = ENUMDataSource.srcCrm)]
        public String detailOfTowEvent { set; get; }
        [CrmMapping(FieldName = "numOfAccidentInjury", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_num_of_accident_injuries", Source = ENUMDataSource.srcCrm)]
        public int? numOfAccidentInjury { set; get; }
        [CrmMapping(FieldName = "detailOfAccidentInjury", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_detail_of_accident_injury", Source = ENUMDataSource.srcCrm)]
        public String detailOfAccidentInjury { set; get; }
        [CrmMapping(FieldName = "numOfDeath", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_num_of_death", Source = ENUMDataSource.srcCrm)]
        public int? numOfDeath { set; get; }
        [CrmMapping(FieldName = "detailOfDeath", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_detail_of_death", Source = ENUMDataSource.srcCrm)]
        public String detailOfDeath { set; get; }
        [CrmMapping(FieldName = "caseOwnerCode", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_accident_prilim_surveyor_report_by", Source = ENUMDataSource.srcCrm)]
        public String caseOwnerCode { set; get; }
        [CrmMapping(FieldName = "caseOwnerFullName", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_accident_prilim_surveyor_report_by", Source = ENUMDataSource.srcCrm)]
        public String caseOwnerFullName { set; get; }
    }
    public class LocusAccidentpartyinfoModel : BaseDataModel
    {
        [CrmMapping(FieldName = "accidentPartyFullname", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_parties_fullname", Source = ENUMDataSource.srcCrm)]
        public String accidentPartyFullname { set; get; }
        [CrmMapping(FieldName = "accidentPartyPhone", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_phoneno", Source = ENUMDataSource.srcCrm)]
        public String accidentPartyPhone { set; get; }
        [CrmMapping(FieldName = "accidentPartyCarPlateNumber", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_licence_no", Source = ENUMDataSource.srcCrm)]
        public String accidentPartyCarPlateNumber { set; get; }
        [CrmMapping(FieldName = "accidentPartyCarModel", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_brand", Source = ENUMDataSource.srcCrm)]
        public String accidentPartyCarModel { set; get; }
        [CrmMapping(FieldName = "accidentPartyInsuredFlag", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_accident_party_insured_flag", Source = ENUMDataSource.srcCrm)]
        public String accidentPartyInsuredFlag { set; get; }
        [CrmMapping(FieldName = "accidentPartyInsuranceCompany", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_insurance_name", Source = ENUMDataSource.srcCrm)]
        public String accidentPartyInsuranceCompany { set; get; }
        [CrmMapping(FieldName = "accidentPartyPolicyType", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_policy_type", Source = ENUMDataSource.srcCrm)]
        public String accidentPartyPolicyType { set; get; }
        [CrmMapping(FieldName = "accidentPartyPolicyNumber", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_policyno", Source = ENUMDataSource.srcCrm)]
        public String accidentPartyPolicyNumber { set; get; }
        [CrmMapping(FieldName = "accidentPartyPolicyExpdate", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_accident_party_policy_expdate", Source = ENUMDataSource.srcCrm)]
        public DateTime? accidentPartyPolicyExpdate { set; get; }
        [CrmMapping(FieldName = "demageOfPartyCar", Source = ENUMDataSource.srcSQL)]
        [CrmMapping(FieldName = "pfc_demage_of_party_car", Source = ENUMDataSource.srcCrm)]
        public String demageOfPartyCar { set; get; }


    }

}