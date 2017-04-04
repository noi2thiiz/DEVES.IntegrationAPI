using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model
{

    public abstract class BaseDataModel
    {
        [JsonIgnore]
        internal const string CONST_FORMAT_DATE_POLISY400 = "ddMMyyyy";
        [JsonIgnore]
        public string DateTimeCustomFormat = "yyyy-MM-dd HH:mm:ss";
    }

    public abstract class BaseContentJsonServiceOutputModel : BaseDataModel
    {

    }

    public abstract class BaseContentJsonProxyOutputModel : BaseDataModel
    {
        [JsonProperty(Order = 11)]
        public string code { get; set; }

        [JsonProperty(Order = 12)]
        public string message { get; set; }

        [JsonProperty(Order = 13)]
        public string description { get; set; }

        [JsonProperty(Order = 14)]
        public string transactionId { get; set; }

        [JsonProperty(Order = 15)]
        public DateTime transactionDateTime { get; set; }

        //public abstract Object data { get; set; }
    }

    public enum ENUMDataSource
    {
        undefined,
        srcCrm,
        srcSQL,
        srcEWI        
    }

    [System.AttributeUsage(System.AttributeTargets.Class) ]
    public class JSONFormatDateTimeAttribute : System.Attribute
    {
        public string format = "yyyy-MM-dd hh:mm:ss";
    }

    [System.AttributeUsage(System.AttributeTargets.Field |
                           System.AttributeTargets.Property,
                           AllowMultiple = true)  
    ]
    public class CrmMappingAttribute : System.Attribute
    {
        public ENUMDataSource Source = ENUMDataSource.undefined;
        public string FieldName=null;
    }

    public class CrmClassToMapDataAttribute : System.Attribute
    {
        
    }

}
