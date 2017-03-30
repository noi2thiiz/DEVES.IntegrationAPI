using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model.EWI;

namespace DEVES.IntegrationAPI.Model.Polisy400
{
    public class COMPInquiryClientMasterInputModel : BaseDataModel
    {
        public String cltType { set; get; }
        public String asrType { set; get; }
        public String clntnum { set; get; }
        public String fullName { set; get; }
        public String idcard { set; get; }
        public String branchCode { set; get; }

        public int? backDay { set; get; }
    }

    public class COMPInquiryClientMasterOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public EWIResCOMPInquiryClientMasterContentModel content { set; get; }
    }

    public class EWIResCOMPInquiryClientMasterContentModel : BaseContentJsonServiceOutputModel
    {

        [JsonProperty(Order = 21)]
        public List<COMPInquiryClientMasterContentClientListModel> clientListCollection { set; get; }
    }

    public class COMPInquiryClientMasterContentClientListModel
    {
        public COMPInquiryClientMasterClientModel clientList { set; get; }
    }

    public class COMPInquiryClientMasterClientModel : BaseDataModel
    {
        public String clientType { set; get; }

        public String cleansingId { set; get; }
        public String clientNumber { set; get; }
        public String additionalExistFlag { set; get; }
        public String name1 { set; get; }
        public String name2 { set; get; }
        public String fullName { set; get; }
        public String salutationText { set; get; }
        public String sex { set; get; }
        public String idCard { set; get; }
        public String passportId { set; get; }
        public String alientId { set; get; }
        public String driverlicense { set; get; }
        public String taxId { set; get; }
        public String corporateStaffNo { set; get; }
        public DateTime? dateOfBirth { set; get; }
        public DateTime? dateOfDeath { set; get; }
        public String natioanalityText { set; get; }
        public String marriedText { set; get; }
        public String occupationText { set; get; }
        public String econActivityText { set; get; }
        public String countryOriginText { set; get; }
        public String riskLevelText { set; get; }
        public String language { set; get; }
        public String vipStatus { set; get; }
        public String clientStatus { set; get; }
        public String remark { set; get; }
        public String telephone1 { set; get; }
        public String telephone2 { set; get; }
        public String telex { set; get; }
        public String telNo { set; get; }
        public String telegram { set; get; }
        public String mobilePhone { set; get; }
        public String facsimile { set; get; }
        public String fax { set; get; }

        public String emailAddress { set; get; }
        public String lineId { set; get; }
        public String facebook { set; get; }

        public String address1 { set; get; }
        public String address2 { set; get; }
        public String address3 { set; get; }
        public String address4 { set; get; }

        public String address5 { set; get; }
        public String postCode { set; get; }
        public String countryText { set; get; }
        public String busResText { set; get; }
        public String latitude { set; get; }
        public String longtitude { set; get; }
        public String assessorFlag { set; get; }
        public String solicitorFlag { set; get; }
        public String repairerFlag { set; get; }
        public String hospitalFlag { set; get; }

    }








}
