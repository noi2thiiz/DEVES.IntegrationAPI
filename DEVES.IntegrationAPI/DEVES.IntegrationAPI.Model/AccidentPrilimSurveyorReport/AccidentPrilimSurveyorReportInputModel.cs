using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.AccidentPrilimSurveyorReport
{
    public class AccidentPrilimSurveyorReportInputModel : BaseDataModel
    {
        public string ticketNo { get; set; }
        public string claimNotiNo { get; set; }
        public string eventId { get; set; }
        public string caseOwnerCode { get; set; }
        public string caseOwnerFullName { get; set; }
        public string reportAccidentResultDate { get; set; }
        public EventDetailInfoModel eventDetailInfo { get; set; }
        public List<PartiesInfoModel> partiesInfo { get; set; }
        public List<ClaimDetailInfoModel> claimDetailInfo { get; set; }
        public List<ClaimDetailPartiesInfoModel> claimDetailPartiesInfo { get; set; }
    }

    public class EventDetailInfoModel
    {
        public string accidentOn { get; set; }
        public string accidentLatitude { get; set; }
        public string accidentLongitude { get; set; }
        public string accidentPlace { get; set; }
        public string accidentNatureDesc { get; set; }
        public string accidentRemark { get; set; }
        public string accidentLegalResult { get; set; }
        public string policeStation { get; set; }
        public string policeRecordId { get; set; }
        public string policeRecordDate { get; set; }
        public string policeBailFlag { get; set; }
        public int numOfTowTruck { get; set; }
        public int numOfAccidentInjury { get; set; }
        public int numOfDeath { get; set; }
        public int deductibleFee { get; set; }
        public int excessFee { get; set; }
        public string totalEvent { get; set; }
        public string iSurveyCreatedDate { get; set; }
        public string iSurveyModifiedDate { get; set; }
        public string iSurveyIsDeleted { get; set; }
        public string iSurveyIsDeletedDate { get; set; }
        public int numOfAccidentParty { get; set; }
    }

    public class PartiesInfoModel
    {
        public string partiesEventId { get; set; }
        public string partiesEventItem { get; set; }
        public string partiesId { get; set; }
        public string partiesFullname { get; set; }
        public int partiesType { get; set; }
        public string partiesCarPlateNo { get; set; }
        public string partiesCarPlateProv { get; set; }
        public string partiesCarBrand { get; set; }
        public string partiesCarModels { get; set; }
        public string partiesCarColor { get; set; }
        public string partiesPartyPhone { get; set; }
        public string partiesInsuranceCompany { get; set; }
        public string partiesPolicyNumber { get; set; }
        public string partiesPolicyType { get; set; }
        public string partiesCreatedDate { get; set; }
        public string partiesModifiedDate { get; set; }
        public string partiesIsDeleted { get; set; }
        public string partiesIsDeletedDate { get; set; }
    }

    public class ClaimDetailInfoModel
    {
        public string claimDetailEventId { get; set; }
        public string claimDetailItem { get; set; }
        public string claimDetailDetailid { get; set; }
        public string claimDetailDetail { get; set; }
        public string claimDetailLevels { get; set; }
        public string claimDetailIsRepair { get; set; }
        public string claimDetailRemark { get; set; }
        public string claimDetailCreatedDate { get; set; }
        public string claimDetailModifiedDate { get; set; }
        public string claimDetailIsDeleted { get; set; }

        public string claimDetailIsDeletedDate { get; set; }
    }
    
    public class ClaimDetailPartiesInfoModel
    {
        public string claimDetailPartiesPartiesId { get; set; }
        public string claimDetailPartiesItem { get; set; }
        public string claimDetailPartiesDetailId { get; set; }
        public string claimDetailPartiesDetail { get; set; }
        public string claimDetailPartiesLevels { get; set; }
        public string claimDetailPartieslIsRepair { get; set; }
        public string claimDetailPartiesRemark { get; set; }
        public string claimDetailPartiesCreatedDate { get; set; }
        public string claimDetailPartiesModifiedDate { get; set; }
        public string claimDetailPartiesIsDeleted { get; set; }
        public string claimDetailPartiesIsDeletedDate { get; set; }
    }

}
