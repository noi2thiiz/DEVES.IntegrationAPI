using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.InquiryCRMPayeeList
{
    public class InquiryCrmPayeeListInputContentModel: BaseDataModel
    {
        public string clientType { set; get; }
        public string roleCode { set; get; }
        public string polisyClientId { set; get; }
        public string sapVendorCode { set; get; }
        public string fullname { set; get; }
        public string taxNo { set; get; }
        public string taxBranchCode { set; get; }
        public string requester { set; get; }
        public string emcsCode { set; get; }
    }

    public class CRMInquiryPayeeContentOutputModel : BaseContentJsonProxyOutputModel
    {
        [JsonProperty(Order = 20)]
        public List<InquiryCrmPayeeListDataModel> data { set; get; }

    }

    public class InquiryCrmPayeeListDataModel : BaseDataModel
    {
        public string sourceData { set; get; } = "";

        public string cleansingId { set; get; }

        public string polisyClientId { set; get; } = "";
        public string sapVendorCode { set; get; } = "";
        public string sapVendorGroupCode { set; get; } = "";
        public string emcsMemHeadId { set; get; } = "";
        public string emcsMemId { set; get; } = "";
        public string companyCode { set; get; } = "";
        public string title { set; get; } = "";
        public string name1 { set; get; } = "";
        public string name2 { set; get; } = "";
        public string fullName { set; get; } = "";
        public string street1 { set; get; } = "";
        public string street2 { set; get; } = "";
        public string district { set; get; } = "";
        public string city { set; get; } = "";
        public string postalCode { set; get; } = "";
        public string countryCode { set; get; } = "";
        public string countryCodeDesc { set; get; } = "";
        public string address {set; get;} = "";
        public string telephone1 { set; get; } = "";
        public string telephone2 { set; get; } = "";
        public string faxNo { set; get; } = "";
        public string contactNumber {set; get;} = "";
        public string taxNo { set; get; } = "";
        public string taxBranchCode { set; get; } = "";
        public string paymentTerm { set; get; } = "";
        public string paymentTermDesc { set; get; } = "";
        public string paymentMethods { set; get; } = "";
        public string inactive { set; get; } = "";
        public string assessorFlag { set; get; } = "";
        public string solicitorFlag { set; get; } = "";
        public string repairerFlag { set; get; } = "";
        public string hospitalFlag { set; get; } = "";

        [JsonProperty(Order = 30)]
        public List<bankInfoModel> bankInfo { set; get; } = new List<bankInfoModel>();
        [JsonProperty(Order = 31)]
        public List<withHoldingTaxInfoModel> withHoldingTaxInfo { set; get; } = new List<withHoldingTaxInfoModel>();

        public string sapVendorAccountCode { get; set; }
        public string sapVendorPayterm { get; set; }
    } 
    public class bankInfoModel : BaseDataModel
    {
        public string bankCountryCode { set; get; } = "";
        public string bankCode { set; get; } = "";
        public string bankName { set; get; } = "";
        public string bankBranchCode { set; get; } = "";
        public string bankBranchDesc { set; get; } = "";
        public string bankAccount { set; get; } = "";
        public string accountHolder { set; get; } = "";
    }
    public class withHoldingTaxInfoModel : BaseDataModel
    {
        public string whtCountryCode { set; get; } = "";
        public string whtTaxType { set; get; } = "";
        public string whtTaxTypeDecs { set; get; } = "";
        public string whtTaxCode { set; get; } = "";
        public string whtTaxCodeDesc { set; get; } = "";
        public string receiptType { set; get; } = "";
        public string receiptTypeDesc { set; get; } = "";
    }

}
