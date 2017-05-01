using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.RegPayeeCorporate
{
    public class RegPayeeCorporateInputModel:BaseDataModel
    {
        public GeneralHeaderModel generalHeader { get; set; }
        public ProfileHeaderModel profileHeader { get; set; }
        public ContactHeaderModel contactHeader { get; set; }
        public AddressHeaderModel addressHeader { get; set; }
        public SapVendorInfoModel sapVendorInfo { get; set; }
    }

    public class GeneralHeaderModel : BaseDataModel
    {
        public string roleCode { get; set; }
        public string cleansingId { get; set; }
        public string polisyClientId { get; set; }
        public string crmClientId { get; set; }
        public string clientAdditionalExistFlag { get; set; }
        //ถ้า = Y  ไม่ต้องไปสร้าง  Create Polisy  (จะส่งมาจากหน้าจอ CRM)
        public string notCreatePolisyClientFlag { get; set; } = "N";
    }

    public class ProfileHeaderModel : BaseDataModel
    {
        public string corporateName1 { get; set; }
        public string corporateName2 { get; set; }
        public string contactPerson { get; set; }
        public string idRegCorp { get; set; }
        public string idTax { get; set; }
        public DateTime? dateInCorporate { get; set; }
        public string corporateBranch { get; set; }
        public string econActivity { get; set; }
        public string countryOrigin { get; set; }
        public string language { get; set; }
        public string riskLevel { get; set; }
        public string vipStatus { get; set; }
    }

    public class ContactHeaderModel : BaseDataModel
    {
        public string telephone1 { get; set; }
        public string telephone1Ext { get; set; }
        public string telephone2 { get; set; }
        public string telephone2Ext { get; set; }
        public string telephone3 { get; set; }
        public string telephone3Ext { get; set; }
        public string mobilePhone { get; set; }
        public string fax { get; set; }
        public string emailAddress { get; set; }
        public string lineID { get; set; }
        public string facebook { get; set; }
    }

    public class AddressHeaderModel : BaseDataModel
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string subDistrictCode { get; set; }
        public string districtCode { get; set; }
        public string provinceCode { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
        public string addressType { get; set; }
        public string latitude { get; set; }
        public string longtitude { get; set; }
    }

    public class SapVendorInfoModel : BaseDataModel
    {
        public string sapVendorGroupCode { get; set; }
        public BankInfoModel bankInfo { get; set; }
        public WithHoldingTaxInfoModel withHoldingTaxInfo { get; set; }

        [JsonIgnore]
        public string sapVendorCode { get; set; }
    }

    public class BankInfoModel : BaseDataModel
    {
        public string bankCountryCode { get; set; }
        public string bankCode { get; set; }
        public string bankBranchCode { get; set; }
        public string bankAccount { get; set; }
        public string accountHolder { get; set; }
        public string paymentMethods { get; set; }
    }

    public class WithHoldingTaxInfoModel : BaseDataModel
    {
        public string whtTaxCode { get; set; }
        public string receiptType { get; set; }
    }

}
