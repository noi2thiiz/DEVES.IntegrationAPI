using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model.EWI;

namespace DEVES.IntegrationAPI.Model.Polisy400
{
    public class CLIENTCreatePersonalClientAndAdditionalInfoInputModel : BaseDataModel
    {
        //"username": "sysdynamic",
        public string username { get; set; }
        //"password": "REZOJUNtN04=",
        public string password { get; set; }
        //"gid": "CRMClaim",
        public string gid { get; set; }
        //"uid": "CRMClaim",
        public string uid { get; set; }
        // "token": "",
        public string token { get; set; }
        // "content": {
        public string content { get; set; }
        // "telephone1": "0public 29999999",
        public string telephone1 { get; set; }
        //"telephone2": "029999999",
        public string telephone2 { get; set; }
        // "remark": "ทดสอบ Service",
        public string remark { get; set; }
        // "address1": "ตำบลเมาะตะแมะ",
        public string address1 { get; set; }
        // "address2": "อำเภอชัยบาดาล",
        public string address2 { get; set; }
        //"specialIndicator": "01",
        public string specialIndicator { get; set; }
        //"address3": "จังหวัดยะลา",
        public string address3 { get; set; }
        //"married": "D",
        public string married { get; set; }
        //"staffFlag": "Y",
        public string staffFlag { get; set; }
        //"facebook": "148976652",
        public string facebook { get; set; }
        // "birthDate": "",
        public string birthDate { get; set; }
        //"telNo": "0999999999",
        public string telNo { get; set; }
        //"natioanality": "T",
        public string natioanality { get; set; }
        //"occupation": "000",
        public string occupation { get; set; }
        // "fax": "029999999",
        public string fax { get; set; }
        // "vipStatus": "Y",
        public string vipStatus { get; set; }
        // "passportId": "1447889522108",
        public string passportId { get; set; }
        // "emailAddress": "createpersonal@hotmail.com",
        public string emailAddress { get; set; }
        //"nameFormat": "2",
        public string nameFormat { get; set; }
        //"sTax": "Y",
        public string sTax { get; set; }
        //"country": "T",
        public string country { get; set; }
        //"soe": "2624623",
        public string soe { get; set; }
        //"mobilePhone": "0999999999",
        public string mobilePhone { get; set; }
        //"taxId": "114744",
        public string taxId { get; set; }
        // "personalName": "Case 2 Create Personal Proxy",
        public string personalName { get; set; }
        // "directMail": "Y",
        public string directMail { get; set; }
        //"longtitude": "1444.000",
        public string longtitude { get; set; }
        //"language": "T",
        public string language { get; set; }
        //"latitude": "10.24487865",
        public string latitude { get; set; }
        //"companyDoctor": "N",
        public string companyDoctor { get; set; }
        // "mailing": "Y",
        public string mailing { get; set; }
        //"sex": "M",
        public string sex { get; set; }
        //"riskLevel": "R1",
        public string riskLevel { get; set; }
        //"birthPlace": "Th Hospital",
        public string birthPlace { get; set; }
        //"deathDate": "",
        public DateTime deathDate { get; set; }
        //"documentNo": "024567",
        public string documentNo { get; set; }
        // "oldIDNumber": "",
        public string oldIDNumber { get; set; }
        //"lineId": "011474",
        public string lineId { get; set; }
        //"pager": "-",
        public string pager { get; set; }
        // "idCard": "1101500772000",
        public string idCard { get; set; }
        // "clientStatus": "AC",
        public string clientStatus { get; set; }
        // "postCode": "32368",
        public string postCode { get; set; }
        // "busRes": "R",
        public string busRes { get; set; }
        // "cleansingId": "CLS-1147",
        public string cleansingId { get; set; }
        // "address5": "ติดชายแดน",
        public string address5 { get; set; }
        // "address4": "ประเทศไทย",
        public string address4 { get; set; }
        // "alientId": "-",
        public string alientId { get; set; }
        //"taxInNumber": "1190400292259",
        public string taxInNumber { get; set; }
        // "driverlicense": "119040",
        public string driverlicense { get; set; }
        // "personalSurname": "JSON",
        public string personalSurname { get; set; }
        //"salutation": "0023"
        public string salutation { get; set; }
    }

    public class CLIENTCreatePersonalClientAndAdditionalInfoOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public CLIENTCreatePersonalClientAndAdditionalInfoContentModel content { set; get; }
    }

    public class CLIENTCreatePersonalClientAndAdditionalInfoContentModel : BaseContentJsonServiceOutputModel
    {

        // "riskLevel_Driver1": "",
        public string riskLevel_Driver1 { get; set; }

        //"riskLevel_Customer": "",
        public string riskLevel_Customer { get; set; }

        //"clientID_Customer": "",
        public string clientID_Customer { get; set; }

        //"clientID": "16960784",
        public string clientID { get; set; }

        //"riskLevel_Driver2": "",
        public string riskLevel_Driver2 { get; set; }

        //"clientID_Driver1": "",
        public string clientID_Driver1 { get; set; }

        //"clientID_Driver2": ""
        public string clientID_Driver2 { get; set; }
    }


}
