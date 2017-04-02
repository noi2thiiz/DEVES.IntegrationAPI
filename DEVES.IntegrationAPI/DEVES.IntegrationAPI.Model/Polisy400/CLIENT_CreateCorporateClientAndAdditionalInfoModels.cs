using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model.EWI;

namespace DEVES.IntegrationAPI.Model.Polisy400
{
    public class CLIENTCreateCorporateClientAndAdditionalInfoInputModel : BaseDataModel
    {
        //"telephones2": "0811111111",
        public string telephones2 { get; set; }

        //"remark": "ทดสอบผ่าน Service Proxy Fulfill Create Corporate",
        public string remark { get; set; }
        //"address1": "12/34 ม.7",
        public string address1 { get; set; }
        //"address2": "เขตท่าพระอาทืตย์",
        public string address2 { get; set; }
        //"specialIndicator": "01",
        public string specialIndicator { get; set; }
        //"add  public string xxx { get; set; }ress3": "แขวงกรุงทัย",
        public string ress3 { get; set; }
        //"capital": "กรุงเทพมหานคร",
        public string capital { get; set; }
        //"hospitalFlag": "",
        public string hospitalFlag { get; set; }
        //"facebook": "https://www.facebook.com/youknowme999",
        public string facebook { get; set; }
        //"solicitorBlackListFlag": "",
        public string solicitorBlackListFlag { get; set; }
        //"solicitorDelistFlag": "",
        public string solicitorDelistFlag { get; set; }
        //"corporateStaffNo": "69",
        public string corporateStaffNo { get; set; }
        //"vipStatus": "Y",
        public string vipStatus { get; set; }
        //"passportId": "4447889211",
        public string passportId { get; set; }
        //"emailAddress": "youknowme@live.com",
        public string emailAddress { get; set; }
        //"fao": "022222222",
        public string fao { get; set; }
        //"telegram": "022222222",
        public string telegram { get; set; }
        //"sTax": "022222222",
        public string sTax { get; set; }
        //"country": "กรุงเทพมหานคร",
        public string country { get; set; }
        //"repairerTerminateDate": "",
        public string repairerTerminateDate { get; set; }
        //"solicitorOregNum": "",
        public string solicitorOregNum { get; set; }
        //"taxId": "559897",
        public string taxId { get; set; }
        //"assessorDelistFlag": "",
        public string assessorDelistFlag { get; set; }
        //"directMail": "isaidyouknowme@hotmail.com",
        public string directMail { get; set; }
        //"longtitude": "144770.0",
        public string longtitude { get; set; }
        //"language": "T",
        public string language { get; set; }
        //"latitude": "1.3554787",
        public string latitude { get; set; }
        //"corporateName2": "Corporate Proxy",
        public string corporateName2 { get; set; }
        //"corporateName1": "Case 1 Corporate JSON",
        public string corporateName1 { get; set; }
        //"dateInCorporate": "20160303",
        public string dateInCorporate { get; set; }
        //"mailing": "isaidyouknowme@hotmail.com",
        public string mailing { get; set; }
        //"riskLevel": "R1",
        public string riskLevel { get; set; }
        //"assessorOregNum": "",
        public string assessorOregNum { get; set; }
        //"telex": "022222222",
        public string telex { get; set; }
        //"telephones": "022222222",
        public string telephones { get; set; }
        //"repairerFlag": "",
        public string repairerFlag { get; set; }
        //"assessorTerminateDate": "",
        public DateTime assessorTerminateDate { get; set; }
        //"repairerOregNum": "",
        public string repairerOregNum { get; set; }
        //"repai rerDelistFlag": "",
        public string rerDelistFlag { get; set; }
        //"countryOrigin": "N",
        public string countryOrigin { get; set; }
        //"lineId": "789410",
        public string lineId { get; set; }
        //"assessorBlackListFlag": "",
        public string assessorBlackListFlag { get; set; }
        //"idCard": "1101500773000",
        public string idCard { get; set; }
        //"facsimile": "10000",
        public string facsimile { get; set; }
        //"clientStatus": "Y",
        public string clientStatus { get; set; }
        //"postCode": "10240",
        public string postCode { get; set; }
        //"repairerBlackListFlag": "",
        public string repairerBlackListFlag { get; set; }
        //"econActivity": "Y",
        public string econActivity { get; set; }
        //"cleansingId": "CLS-33333",
        public string cleansingId { get; set; }
        //"address5": "10404",
        public string address5 { get; set; }
        //"solicitorFlag": "",
        public string solicitorFlag { get; set; }
        //"address4": "ประเทศไทย",
        public string address4 { get; set; }
        //"alientId": "-",
        public string alientId { get; set; }
        //"taxInNumber": "69",
        public string taxInNumber { get; set; }
        //"driverlicense": "69",
        public string driverlicense { get; set; }
        //"assessorFlag": "",
        public string assessorFlag { get; set; }
        //"solicitorTerminateDate": ""
        public DateTime solicitorTerminateDate { get; set; }
    }
    public class CLIENTCreateCorporateClientAndAdditionalInfoOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public CLIENTCreateCorporateClientAndAdditionalInfoContentModel content { set; get; }
    }

    public class CLIENTCreateCorporateClientAndAdditionalInfoContentModel : BaseContentJsonServiceOutputModel
    {

        //"riskLevel_Driver1": "",
        public string riskLevel_Driver1 { get; set; }

       // "riskLevel_Customer": "",
         public string riskLevel_Customer { get; set; }
        //"clientID_Customer": "",
         public string clientID_Customer { get; set; }
       // "clientID": "16960785",
         public string clientID { get; set; }
        //"riskLevel_Driver2": "",
         public string riskLevel_Driver2 { get; set; }
        //"clientID_Driver1": "",
         public string clientID_Driver1 { get; set; }
        //"clientID_Driver2": ""
         public string clientID_Driver2 { get; set; }
    }

}
