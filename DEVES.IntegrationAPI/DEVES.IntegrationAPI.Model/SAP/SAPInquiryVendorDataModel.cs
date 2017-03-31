using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model.EWI;

namespace DEVES.IntegrationAPI.Model.SAP
{
    /// <summary>
    /// end point: http://192.168.3.194/ServiceProxy/ClaimMotor/jsonproxy/COMP_SAPInquiryVendor
    /// </summary>
    public class SAPInquiryVendorInputModel : BaseDataModel
    {
        // "TAX3": "123456789012",
        public string TAX3 { get; set; }

        //"TAX4": "00000",

        public string TAX4 { get; set; }

        //"PREVACC": "",
        public string PREVACC { get; set; }

        //"VCODE": ""
        public string VCODE { get; set; }
    }

    public class SAPInquiryVendorOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public EWIResSAPInquiryVendorContentModel content { set; get; }
    }

    public class EWIResSAPInquiryVendorContentModel : BaseContentJsonServiceOutputModel
    {
        [JsonProperty(Order = 21)]
        public SAPInquiryVendorContentStatusModel Status { set; get; }

        [JsonProperty(Order = 22)]
        public List<SAPInquiryVendorContentVendorInfoModel> VendorInfo { set; get; }
    }


    public class SAPInquiryVendorContentStatusModel : BaseDataModel
    {
        // "Status": "S",
        public string Status { get; set; }

        // "Message": ""
        public string Message { get; set; }
    }

    public class SAPInquiryVendorContentVendorInfoModel : BaseDataModel
    {
        // "VCODE": "2104",
        public string VCODE { get; set; }

        // "VGROUP": "REIL",
        public string VGROUP { get; set; }

        // "COMPANY": "2020",
        public string COMPANY { get; set; }

        // "TITLE": "",
        public string TITLE { get; set; }

        // "NAME1": "อนุพล XXX",
        public string NAME1 { get; set; }

        // "NAME2": "อนุพล จริงๆนะ",
        public string NAME2 { get; set; }

        // "SEARCH": "อนุพล",
        public string SEARCH { get; set; }

        // "STREET1": "37 หมู่บ้านกลางเมือง",
        public string STREET1 { get; set; }

        //  "STREET2": "ลาดพร้าว 23",
        public string STREET2 { get; set; }

        // "DISTRICT": "จันทรเกษม",
        public string DISTRICT { get; set; }

        // "CITY": "กทม.",
        public string CITY { get; set; }

        //"POSTCODE": "10900",
        public string POSTCODE { get; set; }

        // "COUNTRY": "TH",
        public string COUNTRY { get; set; }

        //"COUNTRY_DESC": "Thailand",
        public string COUNTRY_DESC { get; set; }

        // "TEL1": "099999999",
        public string TEL1 { get; set; }

        // "TEL2": "0999999999",
        public string TEL2 { get; set; }

        // "FAX": "",
        public string FAX { get; set; }

        // "TAX1": "",
        public string TAX1 { get; set; }

        // "TAX2": "",
        public string TAX2 { get; set; }

        // "TAX3": "123456789012",
        public string TAX3 { get; set; }

        // "TAX4": "00000",
        public string TAX4 { get; set; }

        //"RECONV": "210100",
        public string RECONV { get; set; }

        //"SORTKEY": "",
        public string SORTKEY { get; set; }

        //"SORTKEY_DESC": "",
        public string SORTKEY_DESC { get; set; }

        //"PREVACC": "2104",
        public string PREVACC { get; set; }

        //"PAYTERM": "NT00",
        public string PAYTERM { get; set; }

        //"PAYTERM_DESC": "",
        public string PAYTERM_DESC { get; set; }

        // "PAYMETHOD": "C",
        public string PAYMETHOD { get; set; }

        // "INACTIVE": "",
        public string INACTIVE { get; set; }
        // "BankInfo": [

        public List<SAPInquiryVendorBankInfoModel> BankInfo { get; set; }
        public List<SAPInquiryVendorWHTInfoModel> WHTInfo { get; set; }
    }

    public class SAPInquiryVendorBankInfoModel : BaseDataModel
    {
        //  "CTRY": "TH",
        public string CTRY { get; set; }

        // "BANKCODE": "002",
        public string BANKCODE { get; set; }

        // "BANKBRANCH": "1234",
        public string BANKBRANCH { get; set; }

        // "BANKACC": "0123456789",
        public string BANKACC { get; set; }
        // "ACCTHOLDER": "Test"

        public string ACCTHOLDER { get; set; }
    }

    public class SAPInquiryVendorWHTInfoModel : BaseDataModel
    {
        public string WHTCTRY { get; set; }
        //"WHTCTRY": "TH",

        public string WHTTYPE { get; set; }
        //"WHTTYPE": "P2",

        public string WHTTYPE_DESC { get; set; }
        //"WHTTYPE_DESC": "ภงด 3 Rate 1 : AT Paymnt.&Cert no.",

        public string WHTCODE { get; set; }
        //"WHTCODE": "13",

        public string WHTCODE_DESC { get; set; }
       // "WHTCODE_DESC": "1% ค่าขนส่ง",

        public string RECPTYPE { get; set; }
        //"RECPTYPE": "03",

        public string RECPTYPE_DESC { get; set; }
       // "RECPTYPE_DESC": "Phaw.Ngaw.Daw. 03"
    }

}