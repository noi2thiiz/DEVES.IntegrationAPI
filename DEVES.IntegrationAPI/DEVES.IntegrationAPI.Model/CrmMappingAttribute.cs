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
        public const string CONST_DATE_NULL_POLISY400 = "99999999";
        [JsonIgnore]
        public const string CONST_DATE_NULL_CLS = "1900-01-01 00:00:00";

        [JsonIgnore]
        internal const string CONST_FORMAT_DATE_POLISY400 = "ddMMyyyy";
        [JsonIgnore]
        public string DateTimeCustomFormat = "yyyy-MM-dd HH:mm:ss";

        [JsonProperty(Order = 1000)]
        public List<DataModelDebugInfo> _debugInfo { get; set; }

        public void AddDebugInfo(string message , dynamic info)
        {
            if (_debugInfo == null)
            {
                _debugInfo = new List<DataModelDebugInfo>();
                _debugInfo.Add(new DataModelDebugInfo
                {
                    message = "warning",
                    info="_debugInfo will be remove on production!!"
                });
            }
            _debugInfo.Add(new DataModelDebugInfo
            {
                message = message,
                info = info
            });
        }

        public void AddListDebugInfo(List<DataModelDebugInfo> debugInfo)
        {
            if (!debugInfo.Any())
            {
                return;
            }
            if (_debugInfo == null)
            {
                _debugInfo = new List<DataModelDebugInfo>();
                _debugInfo.Add(new DataModelDebugInfo
                {
                    message = "warning",
                    info = "_debugInfo will be remove on production!!"
                });
            }
            _debugInfo.AddRange(debugInfo);
        }
    }

    public class DebugInfoDataModel : BaseDataModel
    {
        
    }
    public class DataModelDebugInfo
    {
        public string message { get; set; }
        public dynamic info { get; set; }
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
