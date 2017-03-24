using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.InquiryClientMaster
{
    public class InquiryClientMasterInputModel : BaseDataModel
    {
        ConditionHeaderModel conditionHeader;
        ConditionDetailModel conditionDetail;
    }
    public class ConditionHeaderModel : BaseDataModel
    {
        public String clientType { set; get; }
        public String roleCode { set; get; }
    }
    public class ConditionDetailModel : BaseDataModel
    {
        public String cleansingId { set; get; }
        public String polisyClientId { set; get; }
        public String crmClientId { set; get; }
        public String clientName1 { set; get; }
        public String clientName2 { set; get; }
        public String clientFullname { set; get; }
        public String idCard { set; get; }
        public String corporateBranch { set; get; }
        public string emcsCode { set; get; }
    }

}
