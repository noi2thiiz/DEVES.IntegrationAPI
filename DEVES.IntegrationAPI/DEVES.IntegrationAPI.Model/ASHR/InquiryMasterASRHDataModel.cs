using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model.EWI;

namespace DEVES.IntegrationAPI.Model.ASHR
{

    /// <summary>
    /// End Point: http://192.168.3.194/ServiceProxy/ClaimMotor/jsonservice/MOTOR_InquiryMasterASRH
    /// </summary>
    public class InquiryMasterASRHDataModel : BaseDataModel
    {
        public string vendorCode { get; set; }
        public string taxNo { get; set; }
        public string taxBranchCode { get; set; }
        public string asrhType { get; set; }
        public string polisyClntnum { get; set; }
        public string fullName { get; set; }
        public string emcsCode { get; set; }

     
    }
    
    
    
    public class InquiryMasterASRHModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public InquiryMasterASRHContentOutputModel content { set; get; }
    }

   

    public class InquiryMasterASRHContentOutputModel : BaseContentJsonServiceOutputModel
    {
        [JsonProperty(Order = 21)]
        public InquiryMasterASRHContentASRHListCollectionModel ASRHListCollection { set; get; }
    }

    public class InquiryMasterASRHContentASRHListCollectionModel
    {
        public List<InquiryMasterASRHListOutputModel> ASRHList { set; get; }
    }
    public class InquiryMasterASRHListOutputModel : BaseDataModel
    {

        //"vendorCode": "10108819",
        public string vendorCode { get; set; }
        //"taxBranchCode": "",
        public string taxBranchCode { get; set; }
        //"emcsMemId": "3970",
        public string emcsMemId { get; set; }
        //"contactNumber": ""027433399,
        public string contactNumber { get; set; }
        //"emcsMemHeadId": "2921",
        public string emcsMemHeadId { get; set; }
        //"masterASRHCode": "ASRH-1703-000045",
        public string masterASRHCode { get; set; }
        //"repairerFlag": "N",
        public string repairerFlag { get; set; }
        //"businessType": "Repairer",
        public string businessType { get; set; }
        //"taxNo": "3649800117121",
        public string taxNo { get; set; }
        //"solicitorFlag": "N",
        public string solicitorFlag { get; set; }
        //"address": "357 ถ.รณชัยชาญยุทธ ในเมือง เมืองร้อยเอ็ด ร้อยเอ็ด 45000",
        public string address { get; set; }
        //"polisyClntnum": "10108819",
        public string polisyClntnum { get; set; }
        //"fullName": "เอกชัยบริการรถยก",
        public string fullName { get; set; }
       // "assessorFlag": "N"
        public string assessorFlag { get; set; }

    }
}