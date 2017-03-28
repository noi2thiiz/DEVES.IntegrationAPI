using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model
{

    public abstract class BaseDataModel
    { }

    public abstract class BaseContentOutputModel : BaseDataModel
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
