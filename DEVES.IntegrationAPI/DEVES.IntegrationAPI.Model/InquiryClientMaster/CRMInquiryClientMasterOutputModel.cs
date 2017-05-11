using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.InquiryClientMaster
{

    public class CRMInquiryClientContentOutputModel : BaseContentJsonProxyOutputModel
    {
        [JsonProperty(Order=20)]
        public List<CRMInquiryClientOutputDataModel> data { set; get; }
    }

    public class CRMInquiryClientOutputDataModel : BaseDataModel
    {
        public CRMInquiryClientGeneralHeaderModel generalHeader { set; get; }
        public CRMInquiryClientProfileInfoModel profileInfo { set; get; }
        public CRMInquiryClientContactInfoModel contactInfo { set; get; }
        public CRMInquiryClientAddressInfoModel addressInfo { set; get; }
        public CRMInquiryClientAsrhHeaderModel asrhHeader { set; get; }
        
    }
    public class CRMInquiryClientGeneralHeaderModel : BaseDataModel
    {
        public String clientType { set; get; } = "";
        public String roleCode { set; get; } = "";
        public String cleansingId { set; get; } = "";
        public String polisyClientId { set; get; } = "";
        public String crmClientId { set; get; } = "";
        public String emcsMemHeadId { set; get; } = "";
        public String emcsMemId { set; get; } = "";
        public String clientAdditionalExistFlag { set; get; } = "";
        public String sourceData { set; get; } = "";
    }
    public class CRMInquiryClientProfileInfoModel : BaseDataModel
    {
        public String name1 { set; get; } = "";
        public String name2 { set; get; } = "";
        public String fullName { set; get; } = "";
        public String salutationText { set; get; } = "";
        public String sex { set; get; } = "";
        public String idCard { set; get; } = "";
        public String idPassport { set; get; } = "";
        public String idAlien { set; get; } = "";
        public String idDriving { set; get; } = "";
        public String idTax { set; get; } = "";
        public String corporateBranch { set; get; } = "";
        public DateTime? dateOfBirth { set; get; }
        public DateTime? dateOfDeath { set; get; }
        public String natioanalityText { set; get; } = "";
        public String marriedText { set; get; } = "";
        public String occupationText { set; get; } = "";
        public String econActivityText { set; get; } = "";
        public String countryOriginText { set; get; } = "";
        public String riskLevelText { set; get; } = "";
        public String language { set; get; } = "";
        public String vipStatus { set; get; } = "";
        public String clientStatus { set; get; } = "";
        public String remark { set; get; } = "";
    }
    public class CRMInquiryClientContactInfoModel : BaseDataModel
    {
        public String telephone1 { set; get; } = "";
        public String telephone2 { set; get; } = "";
        public String telephone3 { set; get; } = "";

        public String mobilePhone { set; get; } = "";

        public String fax { set; get; } = "";

        public String contactNumber { set; get; } = "";
        public String emailAddress { set; get; } = "";
        public String lineID { set; get; } = "";
        public String facebook { set; get; } = "";
    }
    public class CRMInquiryClientAddressInfoModel : BaseDataModel
    {
        public String address { set; get; } = "";







        public String countryText { set; get; } = "";
        public String addressTypeText { set; get; } = "";
        public String latitude { set; get; } = "";
        public String longtitude { set; get; } = "";
    }
    public class CRMInquiryClientAsrhHeaderModel : BaseDataModel
    {
        public String assessorFlag { set; get; } = "";
        public String solicitorFlag { set; get; } = "";
        public String repairerFlag { set; get; } = "";
        public String hospitalFlag { set; get; } = "";
    }


}
