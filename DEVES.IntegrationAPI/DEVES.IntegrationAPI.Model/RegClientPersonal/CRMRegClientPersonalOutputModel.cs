using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.RegClientPersonal
{
    public class CRMRegClientPersonalOutputModel : BaseContentJsonProxyOutputModel
    {
        public List<CRMRegClientPersonalOutputDataModel> data { set; get; }
    }

    public class CRMRegClientPersonalOutputDataModel : BaseDataModel
    {
        public string cleansingId { get; set; }
        public string polisyClientId { get; set; }
        public string crmClientId { get; set; }
        public string personalName { get; set; }
        public string personalSurname { get; set; }
    }
}
