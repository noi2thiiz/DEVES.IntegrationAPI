using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
namespace DEVES.IntegrationAPI.Model.RegPayeePersonal
{
    //public class RegPersonalInputModel : BaseDataModel
    //{
    //    public GeneralHeaderModel generalHeader { get; set; }
    //    public ProfileInfoModel profileInfo { get; set; }
    //    public ContactInfoModel contactInfo { get; set; }
    //    public AddressInfoModel addressInfo { get; set; }
    //}

    public class RegPayeePersonalInputModel : BaseDataModel
    {
        public GeneralHeaderModel generalHeader { get; set; }
        public ProfileInfoModel profileInfo { get; set; }
        public ContactInfoModel contactInfo { get; set; }
        public AddressInfoModel addressInfo { get; set; }
        public SapVendorInfoModel sapVendorInfo { get; set; }
    }

    public class GeneralHeaderModel : BaseDataModel
    {
        public string roleCode { get; set; }
        public string cleansingId { get; set; }
        public string polisyClientId { get; set; }
        public string crmPersonId { get; set; }

        public string clientAdditionalExistFlag { get; set; }
    }

    public class ProfileInfoModel : BaseDataModel
    {
        public string salutation { get; set; }
        public string personalName { get; set; }
        public string personalSurname { get; set; }
        public string sex { get; set; }
        public string idCitizen { get; set; }
        public string idPassport { get; set; }
        public string idAlien { get; set; }
        public string idDriving { get; set; }
        public DateTime birthDate { get; set; }
        public string nationality { get; set; }
        public string language { get; set; }
        public string married { get; set; }
        public string occupation { get; set; }
        public string riskLevel { get; set; }
        public string vipStatus { get; set; }
        public string remark { get; set; }
    }

    public class ContactInfoModel : BaseDataModel
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

    public class AddressInfoModel : BaseDataModel
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
        public string sapVendorCode { set; get; }
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