using System;
using DEVES.IntegrationAPI.Model.EWI;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.CLS
{
    public class CLSCreatePersonalClientInputModel : BaseDataModel
    {
      
        //roleCode String	5	M G : General Client
        public string roleCode { get; set; }
        //clientId String	8	O
        public string clientId { get; set; }
        //crmPersonId String	20	O
        public string crmPersonId { get; set; }

        //salutation  String	8	M คำนำหน้าชื่อ
        public string salutation { get; set; }
        //personalName String	60	M ชื่อ
        public string personalName { get; set; }
        //personalSurname String	60	M นามสกุล
        public string personalSurname { get; set; }
        //sex String	1	M เพศลูกค้า
        public string sex { get; set; }
        //idCitizen String	24	O หมายเลขบัตรประจำตัวประชาชน
        public string idCitizen { get; set; }
        //idPassport String	20	O หมายเลขบัตรหนังสือเดินทาง
        public string idPassport { get; set; }
        //idAlien String	20	O หมายเลขบัตรต่างด้าว
        public string idAlien { get; set; }
        //idDriving String	20	O หมายเลขบัตรใบขับขี่
        public string idDriving { get; set; }
        //birthDate String	20	O วันเดือนปีเกิด
        public DateTime birthDate { get; set; }
        //natioanality String	3	O Nationality
        public string natioanality { get; set; }
        //language String	1	O ภาษา
        public string language { get; set; }
        //married String	1	O สถานะการสมรส
        public string married { get; set; }
        //occupation String	3	O อาชีพลูกค้า
        public string occupation { get; set; }

        //vipStatus String	1	O VIP
        public string vipStatus { get; set; }

        //telephone1 String	10	O เบอร์ติดต่อที่สะดวก(Contact Number)
        public string telephone1 { get; set; }
        //telephone1Ext String	5	O
        public string telephone1Ext { get; set; }
        //telephone2  String	10	O โทรศัพท์ลูกค้า(Office)
        public string telephone2 { get; set; }
        //telephone2Ext String	5	O
        public string telephone2Ext { get; set; }
        //telNo   String	10	O DID Tel No
        public string telNo { get; set; }
        //telNoExt String	5	O
        public string telNoExt { get; set; }
        //mobilePhone String	16	O
        public string mobilePhone { get; set; }
        //fax String	16	O
        public string fax { get; set; }
        //emailAddress    String	50	O อีเมล์
        public string emailAddress { get; set; }
        //lineID String	50	O Line ID
        public string lineID { get; set; }
        //facebook    String	100	O Facebook
        public string facebook { get; set; }


        //address1 String	30	O ที่อยู่ บรรทัดที่ 1
        public string address1 { get; set; }
        //address2 String	30	O ที่อยู่ บรรทัดที่ 2
        public string address2 { get; set; }
        //address3 String	30	O ที่อยู่ บรรทัดที่ 3
        public string address3 { get; set; }
        //subDistrictCode String	6	O ตำบล / แขวง
        public string subDistrictCode { get; set; }
        //districtCode    String	4	O อำเภอ / เขต
        public string districtCode { get; set; }
        //provinceCode    String	2	O จังหวัด
        public string provinceCode { get; set; }
        //postalCode String	10	O ที่อยู่ -> รหัสไปรษณีย์
        public string postalCode { get; set; }
        //country String	3	O ที่อยู่ -> ประเทศ
        public string country { get; set; }
        //addressType String	1	O
        public string addressType { get; set; }
        //latitude    String	20	O
        public string latitude { get; set; }
        //longtitude  String	20	O
        public string longtitude { get; set; }

    }

    public class CLSCreatePersonalClientOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public CLSCreatePersonalClientContentOutputModel content { set; get; }
    }

    public class CLSCreatePersonalClientContentOutputModel : BaseContentJsonProxyOutputModel
    {
        [JsonProperty(Order = 16)]
        public bool success { set; get; }

        [JsonProperty(Order = 17)]
        public CLSCreatePersonalClientDataOutputModel data { set; get; }
    }
    public class CLSCreatePersonalClientDataOutputModel : BaseDataModel
    {
        //"cleansingId": "C2017-003324212",
        public string cleansingId { get; set; }
        //"salutation": "0028",
        public string salutation { get; set; }
        //"roleCode": "G",
        public string roleCode { get; set; }
        //"personalName": "อนันต์",
        public string personalName { get; set; }
        //"personalSurname": "กล",
        public string personalSurname { get; set; }
        //"personalFullName": "",
        public string personalFullName { get; set; }
        //"sex": "M",
        public string sex { get; set; }
        //"idCitizen": "1426992878232",
        public string idCitizen { get; set; }
        //"idPassport": "A00920322",
        public string idPassport { get; set; }
        //"idAlien": "",
        public string idAlien { get; set; }
        //"idDriving": "348347232",
        public string idDriving { get; set; }
        //"birthDate": "2017-07-02",
        public DateTime birthDate { get; set; }
        //"natioanality": "764",
        public string natioanality { get; set; }
        //"language": "T",
        public string language { get; set; }
        //"married": "M",
        public string married { get; set; }
        //"occupation": "",
        public string occupation { get; set; }
        //"vipStatus": "",
        public string vipStatus { get; set; }
        //"cltPhone01": "",
        public string cltPhone01 { get; set; }
        //"cltPhone02": "",
        public string cltPhone02 { get; set; }
        //"fax": "",
        public string fax { get; set; }
        //"emailAddress": "",
        public string emailAddress { get; set; }
        //"telephone1": "043763483",
        public string telephone1 { get; set; }
        //"telephone1Ext": "634",
        public string telephone1Ext { get; set; }
        //"telephone2": "",
        public string telephone2 { get; set; }
        //"telNo": "",
        public string telNo { get; set; }
        //"telNoExt": "",
        public string telNoExt { get; set; }
        //"mobilePhone": "",
        public string mobilePhone { get; set; }
        //"lineID": "",
        public string lineID { get; set; }
        //"facebook": "",
        public string facebook { get; set; }
        //"address1": "",
        public string address1 { get; set; }
        //"address2": "",
        public string address2 { get; set; }
        //"address3": "",
        public string address3 { get; set; }
        //"subDistrictCode": "",
        public string subDistrictCode { get; set; }
        //"districtCode": "",
        public string districtCode { get; set; }
        // "provinceCode": "",
        public string provinceCode { get; set; }
        //"postalCode": "",
        public string postalCode { get; set; }
        //"country": "",
        public string country { get; set; }
        //"addressType": "",
        public string addressType { get; set; }
        //"latitude": ""
        public string latitude { get; set; }
    }
}
