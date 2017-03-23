using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.InquiryClientMaster
{
    public class InquiryClientMasterInputModel
    {
        public ConditionHeaderModel conditionHeader { get; set; }
        public ConditionDetailModel conditionDetail { get; set; }
    }

    public class ConditionHeaderModel
    {
        public string clientType { get; set; }
        public string roleCode { get; set; }
    }

    public class ConditionDetailModel
    {
        public string cleansingId { get; set; }
        public string polisyClientId { get; set; }
        public string crmClientId { get; set; }
        public string clientName1 { get; set; }
        public string clientName2 { get; set; }
        public string clientFullname { get; set; }
        public string idCard { get; set; }
        public string corporateBranch { get; set; }
        public string emcsCode { get; set; }
    }
}
