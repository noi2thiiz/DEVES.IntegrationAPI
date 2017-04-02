using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model.EWI;

namespace DEVES.IntegrationAPI.Model.Polisy400
{
    //    class CLIENTUpdateCorporateClientAndAdditionalInfoModels
    public class CLIENTUpdateCorporateClientAndAdditionalInfoInputModel : BaseDataModel
    {
      
        
        //"remark": "ทดสอบผ่าน Service Proxy Fulfill Create and Update Corporate",
        public string remark { get; set; }
        //"address1": "43/21 ม.7",
        public string address1 { get; set; }
        //"address2": "เขตท่าจันทร์",
        public string address2 { get; set; }
        //"specialIndicator": "",
        public string specialIndicator { get; set; }
        //"address3": "แขวงกรุงธน",
        public string address3 { get; set; }

        public string address4 { get; set; }

        //"capital": "สมุทรปราการ",
        public string capital { get; set; }
        //"hospitalFlag": "Y",
        public string hospitalFlag { get; set; }
        //"facebook": "https://www.facebook.com/iknowyou5678",
        public string facebook { get; set; }
        //"solicitorBlackListFlag": "Y",
        public string solicitorBlackListFlag { get; set; }
        //"solicitorDelistFlag": "Y",
        public string solicitorDelistFlag { get; set; }
        //"corporateStaffNo": "96",
        public string corporateStaffNo { get; set; }
        //"vipStatus": "Y",
        public string vipStatus { get; set; }
        //"passportId": "4447889211",
        public string passportId { get; set; }
        //"emailAddress": "iknowyou5678@gmail.com",
        public string emailAddress { get; set; }
        //"fao": "0233333333",
        public string fao { get; set; }
        //"telegram": "0233333333",
        public string telegram { get; set; }
        //"sTax": "0233333333",
        public string sTax { get; set; }
        //"country": "กรุงเทพมหานคร",
        public string country { get; set; }
        //"repairerTerminateDate": "Y",
        public string repairerTerminateDate { get; set; }
        //"solicitorOregNum": "96",
        public string solicitorOregNum { get; set; }
        //"taxId": "559897",
        public string taxId { get; set; }
        //"longtitude": "144770.0",
        public string longtitude { get; set; }
        //"assessorDelistFlag": "Y",
        public string assessorDelistFlag { get; set; }
        //"directMail": "isaidiknowyou5678@yahoo.com",
        public string directMail { get; set; }
        //"language": "T",
        public string language { get; set; }
        //"latitude": "1.3554787",
        public string latitude { get; set; }
        //"corporateName2": "Corporate Proxy",
        public string corporateName2 { get; set; }
        //"corporateName1": "Case 2 Create and Update Corporate JSON",
        public string corporateName1 { get; set; }
        //"dateInCorporate": "04042016",
        public string dateInCorporate { get; set; }
        //"mailing": "isaidyouknowme@hotmail.com",
        public string mailing { get; set; }
        //"riskLevel": "R3",
        public string riskLevel { get; set; }
        //"assessorOregNum": "96",
        public string assessorOregNum { get; set; }
        //"telex": "023333333",
        public string telex { get; set; }
        //"telephones": "023333333",
        public string telephones { get; set; }

        //"telephones2": "0833333333",
        public string telephones2 { get; set; }

        //"repairerFlag": "Y",
        public string repairerFlag { get; set; }
        //"clientNumber": "16960644",
        public string clientNumber { get; set; }
        //"assessorTerminateDate": "04042016",
        public string assessorTerminateDate { get; set; }
        //"repairerOregNum": "96",
        public string repairerOregNum { get; set; }
        //"repairerDelistFlag": "Y",
        public string repairerDelistFlag { get; set; }
        //"countryOrigin": "N",
        public string countryOrigin { get; set; }
        //"lineId": "789410",
        public string lineId { get; set; }
        //"assessorBlackListFlag": "Y",
        public string assessorBlackListFlag { get; set; }
        //"idCard": "1101500772509",
        public string idCard { get; set; }
        //"facsimile": "15000",
        public string facsimile { get; set; }
        //"clientStatus": "Y",
        public string clientStatus { get; set; }
        //"postCode": "10530",
        public string postCode { get; set; }
        //"repairerBlackListFlag": "Y",
        public string repairerBlackListFlag { get; set; }
        //"econActivity": "Y",
        public string econActivity { get; set; }
        //"checkFlag": "UPDATE",
        public string checkFlag { get; set; }
        //"cleansingId": "CLS-33333",
        public string cleansingId { get; set; }
        //"address5": "10530",
        public string address5 { get; set; }
        //"solicitorFlag": "Y",
        public string solicitorFlag { get; set; }
        //"address4": "ประเทศไทย",
        public string address4xxx { get; set; }
        //"alientId": "-",
        public string alientId { get; set; }
        //"taxInNumber": "96",
        public string taxInNumber { get; set; }
        //"driverlicense": "96",
        public string driverlicense { get; set; }
        //"assessorFlag": "Y",
        public string assessorFlag { get; set; }
        //"solicitorTerminateDate": "04042016"
        public string solicitorTerminateDate { get; set; }
    }
    public class CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public CLIENTUpdateCorporateClientAndAdditionalInfoContentModel content { set; get; }
    }

    public class CLIENTUpdateCorporateClientAndAdditionalInfoContentModel : BaseContentJsonServiceOutputModel
    {

        //"riskLevel_Driver1": "",
        public string riskLevel_Driver1 { get; set; }

        //"riskLevel_Customer": "",
        public string riskLevel_Customer { get; set; }
        //"clientID_Customer": "",
        public string clientID_Customer { get; set; }
        //"clientID": "16960644",
        public string clientID { get; set; }
        //"riskLevel_Driver2": "",
        public string riskLevel_Driver2 { get; set; }
        //"clientID_Driver1": "",
        public string clientID_Driver1 { get; set; }
        //"clientID_Driver2": ""
        public string clientID_Driver2 { get; set; }
    }
}
