using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DEVES.IntegrationAPI.Model.EWI;
using Newtonsoft.Json;


namespace DEVES.IntegrationAPI.Model.CLS
{
    class CLSCreateCorporateClientInputModel : BaseDataModel
    {
                   // "cleansingId": "",
        public string cleansingId { get; set; }
        //  "roleCode":"G",
        public string roleCode { get; set; }
        //  "crmPersonId":"",
        public string crmPersonId { get; set; }
        // 	"isPayee":"",
        public string isPayee { get; set; }
        //"corporateName1": "Prime Factor",
        public string corporateName1 { get; set; }
        //"corporateName2": "",
        public string corporateName2 { get; set; }
        //"contactPerson": "อนัน พูล",
        public string contactPerson { get; set; }
        //"idRegCorp": "82368262",
        public string idRegCorp { get; set; }
        //"taxNo": "001122334456",
        public string taxNo { get; set; }
        //"dateInCorporate": "2017-06-02 00:00:00",
        public DateTime dateInCorporate { get; set; }
        //"corporateStaffNo": "152312",
        public string corporateStaffNo { get; set; }
        //"econActivity": "9823822",
        public string econActivity { get; set; }
        //"language": "E",
        public string language { get; set; }
        //"vipStatus": "Y",
        public string vipStatus { get; set; }
        //"telephone1": "043823712",
        public string telephone1 { get; set; }
        //"telephone1Ext": "20",
        public string telephone1Ext { get; set; }
        //"telephone2": "043823812",
        public string telephone2 { get; set; }
        //"telephone2Ext": "20",
        public string telephone2Ext { get; set; }
        //"telNo": "043347233",
        public string telNo { get; set; }
        //"telNoExt": "",
        public string telNoExt { get; set; }
        //"mobilePhone": "0892736235",
        public string mobilePhone { get; set; }
        //"fax": "043347233",
        public string fax { get; set; }
        //"emailAddress": "kan_anan@windowslive.com",
        public string emailAddress { get; set; }
        //"lineID": "58230834",
        public string lineID { get; set; }
        //"facebook": "kan_anan@windowslive.com",
        public string facebook { get; set; }
        //"address1": "152/3 ลาดพร้าว ช.122",
        public string address1 { get; set; }
        //"address2": "",
        public string address2 { get; set; }
        //"address3": "",
        public string address3 { get; set; }
        //"subDistrictCode": "104501",
        public string subDistrictCode { get; set; }
        //"districtCode": "1045",
        public string districtCode { get; set; }
        //"provinceCode": "10",
        public string provinceCode { get; set; }
        //"postalCode": "10310",
        public string postalCode { get; set; }
        //"country": "",
        public string country { get; set; }
        //"addressType": "01",
        public string addressType { get; set; }
        //"latitude": "",
        public string latitude { get; set; }
        //"longigude": "",
        public string longigude { get; set; }
        //"OregNum": "",
        public string OregNum { get; set; }
        //"DelistFlag": "",
        public string DelistFlag { get; set; }
        //"BlackListFlag": "",
        public string BlackListFlag { get; set; }
        //"TerminateDate": ""    
        public DateTime TerminateDate { get; set; }
    }

    public class CLSCreateCorporateClientOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public CLSCreateCorporateClientContentOutputModel content { set; get; }
    }

    public class CLSCreateCorporateClientContentOutputModel : BaseContentJsonProxyOutputModel
    {
        [JsonProperty(Order = 16)]
        public bool success { set; get; }

        [JsonProperty(Order = 17)]
        public CLSCreateCorporateClientDataOutputModel data { set; get; }
    }
    public class CLSCreateCorporateClientDataOutputModel : BaseDataModel
    {
        //"cleansingId": "C2017-003324220",
        public string cleansingId { set; get; }
        //"roleCode": "G",
        public string roleCode { set; get; }
        //"corporateName1": "Prime Factor",
        public string corporateName1 { set; get; }
        // "corporateName2": "",
        public string corporateName2 { set; get; }
        // "corporateFullName": "Prime Factor ",
        public string corporateFullName { set; get; }
        // "sex": "",
        public string sex { set; get; }
        //"contactPerson": "อนัน พูล",
        public string contactPerson { set; get; }
        //"idRegCorp": "82368262",
         public string idRegCorp { set; get; }
        //"idTax": "",
        public string idTax { set; get; }
        // "dateInCorporate": "2017-06-02 00:00:00",
        public string dateInCorporate { set; get; }
        // "corporateStaffNo": "152312",
        public string corporateStaffNo { set; get; }
        // "econActivity": "9823822",
        public string econActivity { set; get; }
        // "idPassport": "",
        public string idPassport { set; get; }
        // "idAlien": "",
        public string idAlien { set; get; }
        // "idDriving": "",
        public string idDriving { set; get; }
        // "birthDate": "",
        public DateTime birthDate { set; get; }
        // "natioanality": "",
        public string natioanality { set; get; }
        // "language": "E",
        public string language { set; get; }
        // "married": "",
        public string married { set; get; }
        // "vipStatus": "Y",
        public string vipStatus { set; get; }
        // "cltPhone01": "",
        public string cltPhone01 { set; get; }
        // "cltPhone02": "",
        public string cltPhone02 { set; get; }
        // "fax": "043347233",
        public string fax { set; get; }
        // "emailAddress": "kan_anan@windowslive.com",
        public string emailAddress { set; get; }
        // "telephone1": "043823712",
        public string telephone1 { set; get; }
        // "telephone1Ext": "20",
        public string telephone1Ext { set; get; }
        //"telephone2Ext": "20",
        public string telephone2Ext { set; get; }
        //"telNo": "043347233",
        public string telNo { set; get; }
        //"telNoExt": "",
        public string telNoExt { set; get; }
        //"mobilePhone": "0892736235",
        public string mobilePhone { set; get; }
        //"lineID": "58230834",
        public string lineID { set; get; }
        // "facebook": "kan_anan@windowslive.com",
        public string facebook { set; get; }
        // "address1": "152/3 ลาดพร้าว ช.122",
        public string address1 { set; get; }
        //"address2": "",
        public string address2 { set; get; }
        //"address3": "",
        public string address3 { set; get; }
        // "subDistrictCode": "104501",
        public string subDistrictCode { set; get; }
        //"districtCode": "1045",
        public string districtCode { set; get; }
        //"provinceCode": "10",
        public string provinceCode { set; get; }
        //"postalCode": "10310",
        public string postalCode { set; get; }
        // "country": "",
        public string country { set; get; }
        // "addressType": "01",
        public string addressType { set; get; }
        // "latitude": "",
        public string latitude { set; get; }
        // "longigude": "",
        public string longigude { set; get; }
        // "OregNum": "",
        public string OregNum { set; get; }
        // "DelistFlag": "",
        public string DelistFlag { set; get; }
        // "BlackListFlag": "",
        public string BlackListFlag { set; get; }
        // "TerminateDate": ""
        public DateTime TerminateDate { set; get; }

    }
}
