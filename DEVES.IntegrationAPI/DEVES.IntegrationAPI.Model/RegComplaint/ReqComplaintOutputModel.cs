using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegComplaint
{
    public class ReqComplaintOutputModel : BaseDataModel
    {
        private EWI.EWIResponseContentData _data;

        /*
        public ReqComplaintOutputModel()
        {
            comp_id = null;
            case_no = null;
        }
        
        public ReqComplaintOutputModel(EWI.EWIResponseContentData data)
        {
            this.comp_id = data.comp_id;
            this.case_no = data.case_no;
        }
        */

        public string comp_id { get; set; }
        public string case_no { get; set; }
        public string errorMessage { get; set; }
    }
}
