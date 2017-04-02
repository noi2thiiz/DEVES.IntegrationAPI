using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model.EWI;

namespace DEVES.IntegrationAPI.Model.Polisy400
{
    public class CLIENTCreateCorporateClientAndAdditionalInfoInputModel : BaseDataModel
    {

    }
    public class CLIENTCreateCorporateClientAndAdditionalInfoOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public CLIENTCreateCorporateClientAndAdditionalInfoContentModel content { set; get; }
    }

    public class CLIENTCreateCorporateClientAndAdditionalInfoContentModel : BaseContentJsonServiceOutputModel
    {

        //[JsonProperty(Order = 21)]
        //public List<...> xxx { set; get; }
    }

}
