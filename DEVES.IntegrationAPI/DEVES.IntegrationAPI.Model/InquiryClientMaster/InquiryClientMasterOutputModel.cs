using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.InquiryClientMaster
{
    public class InquiryClientMasterOutputModel : BaseContentOutputModel
    {
    }

    public class InquiryClientMasterOutputModel_Pass: BaseDataModel
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public List<InquiryClientMasterDataOutputModel_Pass> data { get; set; }
    }

    public class InquiryClientMasterDataOutputModel_Pass: BaseDataModel
    {
        public GeneralHeaderModel generalHeader { get; set; }
        public ProfileInfoModel profileInfo { get; set; }
        public ContactHeaderModel contactHeader { get; set; }
        public AddressHeaderModel addressHeader { get; set; }
        public AsrhHeaderModel asrhHeader { get; set; }
    }

    public class GeneralHeaderModel: BaseDataModel
    {
        public string clientType { get; set; }
        public string roleCode { get; set; }
        public string cleansingId { get; set; }
        public string polisyClientId { get; set; }
        public string crmClientId { get; set; }
        public string emcsMemHeadId { get; set; }
        public string emcsMemId { get; set; }
        public string clientAdditionalExistFlag { get; set; }
        public string sourceData { get; set; }
    }

    public class ProfileInfoModel: BaseDataModel
    {
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string fullName { get; set; }
        public string salutationText { get; set; }
        public string sex { get; set; }
        public string idCard { get; set; }
        public string idPassport { get; set; }
        public string idAlien { get; set; }
        public string idTax { get; set; }
        public string corporateBranch { get; set; }
        public string dateOfBirth { get; set; }
        public string dateOfDeath { get; set; }
        public string natioanalityText { get; set; }
        public string marriedText { get; set; }
        public string occupationText { get; set; }
        public string econActivityText { get; set; }
        public string countryOriginText { get; set; }
        public string riskLevelText { get; set; }
        public string language { get; set; }
        public string vipStatus { get; set; }
        public string clientStatus { get; set; }
        public string remark { get; set; }
    }

    public class ContactHeaderModel
    {
        public string telephone1 { get; set; }
        public string telephone2 { get; set; }
        public string telephone3 { get; set; }
        public string mobilePhone { get; set; }
        public string fax { get; set; }
        public string contactNumber { get; set; }
        public string emailAddress { get; set; }
        public string lineID { get; set; }
        public string facebook { get; set; }
    }

    public class AddressHeaderModel
    {
        public string address1 { get; set; }
        public string countryText { get; set; }
        public string addressTypeText { get; set; }
        public string latitude { get; set; }
        public string longtitude { get; set; }
    }

    public class AsrhHeaderModel
    {
        public string assessorFlag { get; set; }
        public string solicitorFlag { get; set; }
        public string repairerFlag { get; set; }
        public string hospitalFlag { get; set; }
    }

    public class InquiryClientMasterOutputModel_Fail
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public InquiryClientMasterDataOutputModel_Fail data { get; set; }
    }

    public class InquiryClientMasterDataOutputModel_Fail
    {
        public List<InquiryClientMasterListFieldErrors> fieldErrors { get; set; }
    }

    public class InquiryClientMasterListFieldErrors
    {
        public string name { get; set; }
        public string message { get; set; }

        public InquiryClientMasterListFieldErrors()
        {
            name = "";
            message = "";
        }
    }
}
