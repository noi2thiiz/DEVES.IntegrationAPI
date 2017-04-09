using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.EWI
{
    public class BaseEWIResponseModel
    {
        [JsonProperty(Order = 1)]
        public string username { get; set; }

        [JsonProperty(Order = 2)]
        public string uid { get; set; }

        [JsonProperty(Order = 3)]
        public string gid { get; set; }

        [JsonProperty(Order = 4)]
        public string token { get; set; }

        [JsonProperty(Order = 5)]
        public bool success { get; set; }

        [JsonProperty(Order = 6)]
        public string responseCode { set; get; }

        [JsonProperty(Order = 7)]
        public string responseMessage { get; set; }

        [JsonProperty(Order = 8)]
        public string hostscreen { get; set; }
    }

    public class EWIResponse: BaseEWIResponseModel
    {
        //public string gid { get; set; }
        //public string uid { get; set; }
        //public string username { get; set; }
        //public string token { get; set; }
        //public bool success { get; set; }
        //public string responseCode { set; get; }
        //    //get {
        //        //string re = string.Empty;
        //        //switch (responseCode_ENUM)
        //        //{
        //        //    case null:
        //        //        break;
        //        //    case EWIResponseCode.ETC:
        //        //        re = EWIResponseCode.ETC.ToString() + ".";
        //        //        break;
        //        //    default:
        //        //        re = responseCode_ENUM.ToString();
        //        //        re = re.Insert(3, "-");
        //        //        break;
        //        //}
        //        //return re;
        //    //}
        //    //set {
        //        //string temp = value;
        //        //temp = temp.Replace("-", "").Replace(".", "");
        //        //EWIResponseCode mycode;
        //        //if(System.Enum.TryParse(temp, out mycode))
        //        //{
        //        //    responseCode_ENUM = mycode;
        //        //}
        ////    }
        ////}

        //[JsonIgnore]
        //private EWIResponseCode? responseCode_ENUM = null;
        //public string responseMessage { get; set; }
        //public string hostscreen { get; set; }
        public EWIResponseContent content { get; set; }
        //public BaseDataModel content { get; set; }
    }


    public class EWIResponseContent : BaseContentJsonProxyOutputModel
    {
        //public string code { get; set; }
        //public string message { get; set; }
        //public string description { get; set; }
        //public string transactionId { get; set; }
        //public DateTime transactionDateTime { get; set; }
        public EWIResponseContentData data { get; set; }
    }

    public class EWIResponseContentData
    {
        public string claimId { get; set; }
        public string claimNo { get; set; }
        public string ticketNumber { get; set; }

        public string EventID { get; set; }
    }

    public class EWIResponse_ReqSur
    {
        public string gid { get; set; }
        public bool success { get; set; }
        public string responseMessage { get; set; }
        // public string hostscreen { get; set; }
        public object content { get; set; }
        public string username { get; set; }
        public string token { get; set; }
        public string responseCode
        {
            get
            {
                string re = string.Empty;
                switch (responseCode_ENUM)
                {
                    case null:
                        break;
                    case EWIResponseCode.ETC:
                        re = EWIResponseCode.ETC.ToString() + ".";
                        break;
                    default:
                        re = responseCode_ENUM.ToString();
                        re = re.Insert(3, "-");
                        break;
                }
                return re;
            }
            set
            {
                string temp = value;
                temp = temp.Replace("-", "").Replace(".", "");
                EWIResponseCode mycode;
                if (System.Enum.TryParse(temp, out mycode))
                {
                    responseCode_ENUM = mycode;
                }
            }
        }
        [JsonIgnore]
        private EWIResponseCode? responseCode_ENUM = null;
    }

    public class EWIResponseContent_ReqSur
    {
        public string eventid { get; set; }
        public string errorMessage { get; set; }
    }


}
