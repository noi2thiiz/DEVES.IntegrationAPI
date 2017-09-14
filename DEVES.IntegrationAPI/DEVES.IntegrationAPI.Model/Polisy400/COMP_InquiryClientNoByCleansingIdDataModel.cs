using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.EWI;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.Polisy400
{
    
        public class COMPInquiryClientNoByCleansingIdInputModel : BaseEWIRequestContentModel
    {

            //"topRecord":"10"
            public string topRecord { set; get; } = "5";
            //"cleansingId":"C2017-100000068",
            public string cleansingId { set; get; } = "";

        }

        public class COMPInquiryClientNoByCleansingIdOutputModel : BaseEWIResponseModel
        {
            [JsonProperty(Order = 10)]
            public COMPInquiryClientNoByCleansingIdContentModel content { set; get; }
        }

        public class COMPInquiryClientNoByCleansingIdContentModel : BaseContentJsonServiceOutputModel
        {

            [JsonProperty(Order = 21)]
            public List<COMPInquiryClientNoByCleansingIdContenitemListCollectionModel> itemListCollection { set; get; }
        }



        public class COMPInquiryClientNoByCleansingIdContenitemListCollectionModel : BaseContentJsonServiceOutputModel
        {
            public COMPInquiryClientNoByCleansingIdContenitemListCollectionitemListModel itemList { get; set; }
        }
        public class COMPInquiryClientNoByCleansingIdContenitemListCollectionitemListModel : BaseDataModel
    {

            [JsonProperty(Order = 21)]
            //"cleansingId": "C2017-100000068",
            public string cleansingId { set; get; }
            //"clntnum": "16961857",
            public string clntnum { set; get; }
            //"clientType": "P"
            public string clientType { set; get; }
        }

}
