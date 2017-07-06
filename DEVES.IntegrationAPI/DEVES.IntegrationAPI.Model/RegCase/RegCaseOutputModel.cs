using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegCase
{
    public class RegCaseOutputModel : BaseContentJsonProxyOutputModel
    {
        public RegCaseDataModel data { get; set; }
    }

    public class RegCaseDataModel
    {
        public string caseNo { get; set; }
        public DateTime caseDate { get; set; }
    }
}
