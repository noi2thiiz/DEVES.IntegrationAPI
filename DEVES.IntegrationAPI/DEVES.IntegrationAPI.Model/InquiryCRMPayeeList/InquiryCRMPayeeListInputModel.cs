using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.InquiryCRMPayeeList
{
    public enum ENUM_SAP_SearchConditionType
    {
        invalid,
        sapVendorCode,
        polisyClientId,
        taxNo
    }
    public class InquiryCRMPayeeListInputModel : BaseDataModel
    {

        public string clientType { get; set; } = "";
        public string roleCode { get; set; } = "";
        public string polisyClientId { get; set; } = "";
        public string sapVendorCode { get; set; } = "";
        public string fullname { get; set; } = "";
        public string taxNo { get; set; } = "";
        public string taxBranchCode { get; set; } = "";
        public string requester { get; set; } = "";
        public string emcsCode { get; set; } = "";


        [JsonIgnore]
        public string emcsMemHeadId { get; set; } = "";
        [JsonIgnore]
        public string emcsMemId { get; set; } = "";
        [JsonIgnore]
        public string contactNumber { get; set; } = "";
        [JsonIgnore]
        public string assessorFlag { get; set; } = "";
        [JsonIgnore]
        public string solicitorFlag { get; set; } = "";
        [JsonIgnore]
        public string repairerFlag { get; set; } = "";
        [JsonIgnore]
        public string hospitalFlag { get; set; } = "";
        [JsonIgnore]
        public string cleansingId { get; set; } = "";

        [JsonIgnore]
        public ENUM_SAP_SearchConditionType SearchConditionType
        {
            get
            {
                ENUM_SAP_SearchConditionType r = ENUM_SAP_SearchConditionType.invalid;

                if (!string.IsNullOrEmpty(sapVendorCode))
                {
                    r = ENUM_SAP_SearchConditionType.sapVendorCode;
                }
                else if (!string.IsNullOrEmpty(polisyClientId))
                {
                    r = ENUM_SAP_SearchConditionType.polisyClientId;
                }
                else if (!(string.IsNullOrEmpty(taxNo) || !string.IsNullOrEmpty(taxBranchCode))  )
                {
                    r = ENUM_SAP_SearchConditionType.taxNo;
                }

                return r;
            }
        }
    }
}