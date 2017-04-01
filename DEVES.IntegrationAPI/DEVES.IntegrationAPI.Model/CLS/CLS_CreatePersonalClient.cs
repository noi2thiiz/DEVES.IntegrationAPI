using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DEVES.IntegrationAPI.Model.EWI;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.CLS
{
    class CLSCreatePersonalClientInputModel : BaseDataModel
    {
    }

    public class CLSCreatePersonalClientOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public CLSCreatePersonalClientContentOutputModel content { set; get; }
    }

    public class CLSCreatePersonalClientContentOutputModel : BaseContentJsonProxyOutputModel
    {
        [JsonProperty(Order = 16)]
        public bool success { set; get; }

        [JsonProperty(Order = 17)]
        public List<CLSCreatePersonalClientDataOutputModel> data { set; get; }
    }
    public class CLSCreatePersonalClientDataOutputModel : BaseDataModel
    {

    }
}
