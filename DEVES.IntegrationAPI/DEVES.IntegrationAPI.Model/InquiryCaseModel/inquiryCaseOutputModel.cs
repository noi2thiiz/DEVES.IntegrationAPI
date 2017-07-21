using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.InquiryCaseModel
{
    public class inquiryCaseOutputModel : BaseContentJsonProxyOutputModel
    {
        public List<inquiryCaseDataOutput> data { get; set; }
    }
    public class inquiryCaseDataOutput : BaseDataModel
    {
        public string caseNo { get; set; }
        public string caseTitle { get; set; }
        public string caseType { get; set; }
        public string category { get; set; }
        public string subCategory { get; set; }
        public string priority { get; set; }
        public string origin { get; set; }
        public string description { get; set; }
        public DateTime dueDate { get; set; }
        public string policyAdditionalId { get; set; }
        public string policyNo { get; set; }
        public string policyAdditionalNo { get; set; }


    }
}
