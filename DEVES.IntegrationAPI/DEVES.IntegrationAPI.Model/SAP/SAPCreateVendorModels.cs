using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model.EWI;

namespace DEVES.IntegrationAPI.Model.SAP
{
    //class SAPCreateVendorModels
    public class SAPCreateVendorInputModel : BaseDataModel
    {

    }

    public class SAPCreateVendorOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public SAPCreateVendorContentOutputModel content { set; get; }
    }

    public class SAPCreateVendorContentOutputModel : BaseContentJsonServiceOutputModel
    {
        //[JsonProperty(Order = 21)]
        //public SAPCreateVendorContentStatusModel Status { set; get; }

        //[JsonProperty(Order = 22)]
        //public List<SAPCreateVendorContentVendorInfoModel> VendorInfo { set; get; }
    }


}
