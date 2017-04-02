using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model.EWI;

namespace DEVES.IntegrationAPI.Model.Polisy400
{
    //http://192.168.3.194/ServiceProxy/ClaimMotor/jsonservice/COMP_InquiryClientMaster
    /*{
          "username":"systelepro",
          "password":"WDdDbXokJVo=",
          "gid":"cleansing",
          "uid":"cleansing",
          "token":"",
          "content":{
                   "cltType":"P",
                   "asrType":"",
                   "branchCode":"",
                   "idcard":"3101100240156",
                   "clntnum":"",
                   "fullName":""    , 
                   "backDay":""                
          }
}*/
    public class COMPInquiryClientMasterInputModel : BaseDataModel
    {
        public String cltType { set; get; }
        public String asrType { set; get; }
        public String clntnum { set; get; }
        public String fullName { set; get; }
        public String idcard { set; get; }
        public String branchCode { set; get; }

        public int? backDay { set; get; }
    }

    public class COMPInquiryClientMasterOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public EWIResCOMPInquiryClientMasterContentModel content { set; get; }
    }

    public class EWIResCOMPInquiryClientMasterContentModel : BaseContentJsonServiceOutputModel
    {

        [JsonProperty(Order = 21)]
        public List<COMPInquiryClientMasterContentClientListModel> clientListCollection { set; get; }
    }

    public class COMPInquiryClientMasterContentClientListModel
    {
        public COMPInquiryClientMasterClientModel clientList { set; get; }
    }

    public class COMPInquiryClientMasterClientModel : BaseDataModel
    {
        public string clientType { set; get; }

        public string cleansingId { set; get; }
        public string clientNumber { set; get; }
        public string additionalExistFlag { set; get; }
        public string name1 { set; get; }
        public string name2 { set; get; }
        public string fullName { set; get; }
        public string salutationText { set; get; }
        public string sex { set; get; }
        public string idCard { set; get; }
        public string passportId { set; get; }
        public string alientId { set; get; }
        public string driverlicense { set; get; }
        public string taxId { set; get; }
        public string corporateStaffNo { set; get; }
        public DateTime? dateOfBirth { set; get; }
        public DateTime? dateOfDeath { set; get; }
        public string natioanalityText { set; get; }
        public string marriedText { set; get; }
        public string occupationText { set; get; }
        public string econActivityText { set; get; }
        public string countryOriginText { set; get; }
        public string riskLevelText { set; get; }
        public string language { set; get; }
        public string vipStatus { set; get; }
        public string clientStatus { set; get; }
        public string remark { set; get; }
        public string telephone1 { set; get; }
        public string telephone2 { set; get; }
        public string telex { set; get; }
        public string telNo { set; get; }
        public string telegram { set; get; }
        public string mobilePhone { set; get; }
        public string facsimile { set; get; }
        public string fax { set; get; }

        public string emailAddress { set; get; }
        public string lineId { set; get; }
        public string facebook { set; get; }

        public string address1 { set; get; }
        public string address2 { set; get; }
        public string address3 { set; get; }
        public string address4 { set; get; }

        public string address5 { set; get; }
        public string postCode { set; get; }
        public string countryText { set; get; }
        public string busResText { set; get; }
        public string latitude { set; get; }
        public string longtitude { set; get; }
        public string assessorFlag { set; get; }
        public string solicitorFlag { set; get; }
        public string repairerFlag { set; get; }
        public string hospitalFlag { set; get; }


        public string fao { set; get; }
        public string sTax { set; get; }
        public string nameFormat { set; get; }
        public string companyDoctor { set; get; }
        public string birthPlace { set; get; }
        public string soe { set; get; }
        public string documentNo { set; get; }
        public string capital { set; get; }
        public string mailing { set; get; }
        public string directMail { set; get; }
        public string staffFlag { set; get; }
        public string pager { set; get; }
        public string taxIdNumber { set; get; }
        public string specialIndicator { set; get; }
        public string oldIdNumber { set; get; }
        public string busRes { set; get; }
        public string country { set; get; }
        public string natioanality { set; get; }
        public string married { set; get; }
        public string econActivity { set; get; }
        public string countryOrigin { set; get; }
        public string riskLevel { set; get; }
        public string occupation { set; get; }
        public string assessorOregNum { set; get; }
        public string assessorDelistFlag { set; get; }
        public string assessorBlackListFlag { set; get; }
        public string salutation { set; get; }
        public string assessorTerminateDate { set; get; }
        public string solicitorOregNum { set; get; }
        public string solicitorDelistFlag { set; get; }
        public string solicitorBlackListFlag { set; get; }
        public string solicitorTerminateDate { set; get; }
        public string repairerOregNum { set; get; }
        public string repairerDelistFlag { set; get; }
        public string repairerBlackListFlag { set; get; }
        public string repairerTerminateDate { set; get; }



    }








}
