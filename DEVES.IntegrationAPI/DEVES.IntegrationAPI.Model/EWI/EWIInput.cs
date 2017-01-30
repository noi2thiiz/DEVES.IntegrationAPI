using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.EWI
{
    public class EWIInput
    {
        public string username { get; set; }
        public string token { get; set; }
        public bool success { get; set; }
        public string responseCode {
            get {
                string re = string.Empty;
                switch (responseCode_ENUM)
                {
                    case null:
                        break;
                    case EWIResponseCode.ETC:
                        re = EWIResponseCode.ETC.ToString() + ".";
                        break;
                    default:
                        re = EWIResponseCode.ETC.ToString();
                        re = re.Insert(3, "-");
                        break;
                }
                return re;
            }
            set {
                string temp = value;
                temp = temp.Replace("-", "").Replace(".", "");
                EWIResponseCode mycode;
                if(System.Enum.TryParse("", out mycode))
                {
                    responseCode_ENUM = mycode;
                }
            }
        }
        [JsonIgnore]
        private EWIResponseCode? responseCode_ENUM = null;
        public string responseMessage { get; set; }
        public string hostscreen { get; set; }
        public object content { get; set; }
    }
}
