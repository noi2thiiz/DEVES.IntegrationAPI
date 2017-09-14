using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model.EWI;

namespace DEVES.IntegrationAPI.Model.SAP
{
    //class SAPCreateVendorModels
    public class SAPCreateVendorInputModel : BaseEWIRequestContentModel
    {
           // "VCODE": "2111",
        public string VCODE { get; set; } = "";
        //"VGROUP": "ONET",
        public string VGROUP { get; set; } = "";
        // "COMPANY": "2020",
        public string COMPANY { get; set; } = "";
        // "TITLE": "",
        public string TITLE { get; set; } = "";
        //// "NAME1": "Kunatip Pewdee",
        public string NAME1 { get; set; } = "";
        // "NAME2": "j",
        public string NAME2 { get; set; } = "";
        // "SEARCH": "Astachai",
        public string SEARCH { get; set; } = "";
        // "STREET1": "Ladprao ROAD",
        public string STREET1 { get; set; } = "";
        // "STREET2": "Ratchada ROAD",
        public string STREET2 { get; set; } = "";
        // "DISTRICT": "MUANG",
        public string DISTRICT { get; set; } = "";
        // "CITY": "BANGKOK",
        public string CITY { get; set; } = "";
        // "POSTCODE": "10270",
        public string POSTCODE { get; set; } = "";
        // "COUNTRY": "TH",
        public string COUNTRY { get; set; } = "";
        // "TEL1": "089-6590100",
        public string TEL1 { get; set; } = "";
        // "TEL2": "089-6590100",
        public string TEL2 { get; set; } = "";
        // "FAX": "02-757-7097",
        public string FAX { get; set; } = "";
        // "TAX1": "1919900147338",
        public string TAX1 { get; set; } = "";
        // "TAX2": "1919900147338",
        public string TAX2 { get; set; } = "";
        // "TAX3": "1919900136441",
        public string TAX3 { get; set; } = "";
        // "TAX4": "1111",
        public string TAX4 { get; set; } = "";
        // "CTRY": "TH",
        public string CTRY { get; set; } = "";
        // "BANKCODE": "002",
        public string BANKCODE { get; set; } = "";
        // "BANKBRANCH": "1234",
        public string BANKBRANCH { get; set; } = "";
        //"BANKACC": "1234567890",
        public string BANKACC { get; set; } = "";
        //"ACCTHOLDER": "อนุพล ชัยศิริ",
        public string ACCTHOLDER { get; set; } = "";
        //"PAYMETHOD": "C",
        public string PAYMETHOD { get; set; } = "";
        // "WHTCTRY": "TH",
        public string WHTCTRY { get; set; } = "";
        // "WHTCODE": "13",
        public string WHTCODE { get; set; } = "";
        //"RECPTYPE": "03"
        public string RECPTYPE { get; set; } = "";

    }

    public class SAPCreateVendorOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public SAPCreateVendorContentOutputModel content { set; get; }
    }

    public class SAPCreateVendorContentOutputModel : BaseContentJsonServiceOutputModel
    {
        //"VCODE": "990027",
        public string VCODE { get; set; }
        //"Message": "Vendor 990027 was created in company code 2020"
        public string Message { get; set; }
         
         
    }


}
