using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model.EWI;

namespace DEVES.IntegrationAPI.Model.Polisy400
{
    //    class CLIENTUpdateCorporateClientAndAdditionalInfoModels
    public class CLIENTUpdateCorporateClientAndAdditionalInfoInputModel : BaseDataModel
    {

    }
    public class CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public CLIENTUpdateCorporateClientAndAdditionalInfoContentModel content { set; get; }
    }

    public class CLIENTUpdateCorporateClientAndAdditionalInfoContentModel : BaseContentJsonServiceOutputModel
    {

        //[JsonProperty(Order = 21)]
        //public List<...> xxx { set; get; }
    }
}
