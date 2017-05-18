using System;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.RegClientPersonal
{
    public class RegClientPersonalInputModel : BaseDataModel
    {
        public GeneralHeaderModel generalHeader { get; set; } = new GeneralHeaderModel();
        public ProfileInfoModel profileInfo { get; set; } = new ProfileInfoModel();
        public ContactInfoModel contactInfo { get; set; } = new ContactInfoModel();
        public AddressInfoModel addressInfo { get; set; } = new AddressInfoModel();

     
    }

    public class GeneralHeaderModel : BaseDataModel
    {
        public string roleCode { get; set; } = "";
        public string cleansingId { get; set; } = "";
        public string polisyClientId { get; set; } = "";
        public string crmClientId { get; set; } = "";
        public string clientAdditionalExistFlag { get; set; } = "";
        //ถ้า = Y  ไม่ต้องไปสร้าง  Create Polisy  (จะส่งมาจากหน้าจอ CRM)
        public string notCreatePolisyClientFlag { get; set; } = "N";
    }

    public class ProfileInfoModel : BaseDataModel
    {
        public string salutation { get; set; } = "";
        public string personalName { get; set; } = "";
        public string personalSurname { get; set; } = "";
        public string sex { get; set; } = "U";
        public string idCitizen { get; set; } = "";
        public string idPassport { get; set; } = "";
        public string idAlien { get; set; } = "";
        public string idDriving { get; set; } = "";
        public DateTime? birthDate { get; set; }

        public string nationality { get; set; } = "";
        public string language { get; set; } = "";
        public string married { get; set; } = "";
        public string occupation { get; set; } = "";
        public string riskLevel { get; set; } = "";
        public string vipStatus { get; set; } = "";
        public string remark { get; set; } = "";
    }

    public class ContactInfoModel : BaseDataModel
    {
        public string telephone1 { get; set; } = "";
        public string telephone1Ext { get; set; } = "";
        public string telephone2 { get; set; } = "";
        public string telephone2Ext { get; set; } = "";
        public string telephone3 { get; set; } = "";
        public string telephone3Ext { get; set; } = "";
        public string mobilePhone { get; set; } = "";
        public string fax { get; set; } = "";
        public string emailAddress { get; set; } = "";
        public string lineID { get; set; } = "";
        public string facebook { get; set; } = "";
    }

    public class AddressInfoModel : BaseDataModel
    {
        public string address1 { get; set; } = "";
        public string address2 { get; set; } = "";
        public string address3 { get; set; } = "";
        public string subDistrictCode { get; set; } = "";
        public string districtCode { get; set; } = "";
        public string provinceCode { get; set; } = "";
        public string postalCode { get; set; } = "";
        public string country { get; set; } = "";
        public string addressType { get; set; } = "";
        public string latitude { get; set; } = "";
        public string longtitude { get; set; } = "";
    }
}