using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.CRM
{
    public class CreateAssessmentFromLocusInputModel:BaseDataModel
    {
        public string requestId {get; set;}
    }

    public class CreateAssessmentFromLocusOutputModel : BaseContentJsonProxyOutputModel
    {
        public CreateAssessmentFromLocusOutputDataModel data { set; get; } = new CreateAssessmentFromLocusOutputDataModel();
    }
    public class CreateAssessmentFromLocusOutputDataModel : BaseDataModel
    {
        public bool status { get; set; }
        public int totalRecord { get; set; }
    }
    

}
