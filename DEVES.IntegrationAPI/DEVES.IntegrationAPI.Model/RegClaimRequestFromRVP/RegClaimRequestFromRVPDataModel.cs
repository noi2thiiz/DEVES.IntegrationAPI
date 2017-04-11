using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System;

namespace DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP
{
    public class CRMregClaimRequestFromRVPInputModel:BaseDataModel
    {
        /// <summary>
        /// เลขเคลมของ RVP
        /// </summary>
        [Required]
        [StringLength(20)]
        public string rvpCliamNo { get; set; }
        [Required]
        public PolicyInfoModel policyInfo { get; set; }
        [Required]
        public PolicyDriverInfoModel policyDriverInfo { get; set; }
        [Required]
        public ClaimInformModel claimInform { get; set; }
        public List<AccidentPartyInfoModel> accidentPartyInfo { get; set; }
    }

    public class CrmregClaimRequestFromRVPContentOutputModel : BaseContentJsonProxyOutputModel
    {
        [JsonProperty(Order=20)]
        public CrmRegClaimRequestFromRVPDataOutputModel data { set; get; }
    }

    public class CrmRegClaimRequestFromRVPDataOutputModel:BaseDataModel
    {
        public string ticketNo { get; set; }
        public string claimNotiNo { get; set; }

    }

    public class PolicyInfoModel
    {

        /// <summary>
        /// Primary Key  Policy Detail ใน CRM
        /// </summary>
        [StringLength(40)]
        public string crmPolicyDetailId { get; set; }
        /// <summary>
        /// รหัส Policy Detail ใน CRM
        /// </summary>
        [StringLength(100)]
        public string crmPolicyDetailCode { get; set; }
        /// <summary>
        /// เลขกรมธรรม์ (ps/400.chdrnum)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string policyNo { get; set; }
        /// <summary>
        /// ครั้งที่ต่ออายุ 0 = 1st Year
        /// </summary>
        public string renewalNo { get; set; }
        /// <summary>
        /// RSKNO (คันที่)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string fleetCarNo { get; set; }
        /// <summary>
        /// เลข Barcode บนหน้าตาราง Policy 
        /// ขึ้นต้นด้วย(2010, 2020, 208) เป็นต้น
        /// </summary>
        [StringLength(50)]
        public string barcode { get; set; }
        public string carChassisNo { get; set; }
        [Required]
        public string currentCarRegisterNo { get; set; }
        public string currentCarRegisterProv { get; set; }
        [StringLength(20)]
        public string insuredCleansingId { get; set; }
        [StringLength(2)]
        public string insuredClientType { get; set; }
        [StringLength(120)]
        public string insuredFullName { get; set; }
        [StringLength(10)]
        public string insuredClientId { get; set; }
    }

    public class PolicyDriverInfoModel
    {
        public string driverCleansingId { get; set; }
        [Required]
        public string driverFullName { get; set; }
        [Required]
        public string driverClientId { get; set; }
        [StringLength(20)]
        public string driverMobile { get; set; }
        [StringLength(20)]
        public string driverPhoneNo { get; set; }
    }

    public class ClaimInformModel
    {
        [Required]
        public DateTime informerOn { get; set; }
        [Required]
        public DateTime accidentOn { get; set; }
        [Required]
        [StringLength(100)]
        public string accidentDescCode { get; set; }
        [StringLength(1000)]
        public string accidentDesc { get; set; }
        [Required]
        public int numOfExpectInjury { get; set; }
        [StringLength(20)]
        public string accidentLatitude { get; set; }
        [StringLength(20)]
        public string accidentLongitude { get; set; }
        [StringLength(2000)]
        public string accidentPlace { get; set; }
        [StringLength(2)]
        public string accidentProvn { get; set; }
        [StringLength(4)]
        public string accidentDist { get; set; }
        [Required]
        [StringLength(1)]
        public string claimType { get; set; }
        [Required]
        [StringLength(2)]
        public string sendOutSurveyorCode { get; set; }
        public DateTime reportAccidentResultDate { get; set; }
        [Required]
        [StringLength(20)]
        public string caseOwnerCode { get; set; }
        [Required]
        [StringLength(250)]
        public string caseOwnerFullName { get; set; }
        [Required]
        [StringLength(20)]
        public string informByCrmId { get; set; }
        [Required]
        [StringLength(200)]
        public string informByCrmName { get; set; }
        [Required]
        [StringLength(20)]
        public string submitByCrmId { get; set; }
        [Required]
        [StringLength(200)]
        public string submitByCrmName { get; set; }
        [Required]
        [StringLength(1)]
        public string accidentLegalResult { get; set; }
        [Required]
        public int numOfAccidentInjury { get; set; }
        public int numOfDeath { get; set; }
        public decimal excessFee { get; set; }
        public decimal deductibleFee { get; set; }
        [StringLength(2000)]
        public string accidentNatureDesc { get; set; }
        [StringLength(200)]
        public string policeStation { get; set; }
        [StringLength(200)]
        public string policeRecordId { get; set; }
        public DateTime policeRecordDate { get; set; }
        [StringLength(1)]
        public string policeBailFlag { get; set; }
        [Required]
        public int numOfAccidentParty { get; set; }
    }

    public class AccidentPartyInfoModel
    {
        public int rvpAccidentPartySeq { get; set; }
        [StringLength(200)]
        public string accidentPartyFullname { get; set; }
        [StringLength(50)]
        public string accidentPartyPhone { get; set; }
        [StringLength(50)]
        public string accidentPartyCarPlateNo { get; set; }
        [StringLength(50)]
        public string accidentPartyCarPlateProv { get; set; }
        [StringLength(200)]
        public string accidentPartyInsuranceCompany { get; set; }
        [StringLength(20)]
        public string accidentPartyPolicyNumber { get; set; }
        [StringLength(20)]
        public string accidentPartyPolicyType { get; set; }

    }




}