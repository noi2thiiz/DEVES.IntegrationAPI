using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DEVES.IntegrationAPI.Model.EWI;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.CLS
{
    class CLSCreateCorporateClientInputModel : BaseDataModel
    {
    }

    public class CLSCreateCorporateClientOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public CLSCreateCorporateClientContentOutputModel content { set; get; }
    }

    public class CLSCreateCorporateClientContentOutputModel : BaseContentJsonProxyOutputModel
    {
        [JsonProperty(Order = 16)]
        public bool success { set; get; }

        [JsonProperty(Order = 17)]
        public List<CLSCreateCorporateClientDataOutputModel> data { set; get; }
    }
    public class CLSCreateCorporateClientDataOutputModel : BaseDataModel
    {

    }
}
