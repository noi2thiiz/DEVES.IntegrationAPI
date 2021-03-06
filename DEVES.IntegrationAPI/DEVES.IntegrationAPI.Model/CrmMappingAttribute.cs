﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.WebApi;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model
{

    public abstract class BaseDataModel
    {
        [JsonIgnore]
        public const string CONST_DATE_NULL_POLISY400 = "99999999";
        [JsonIgnore]
        public const string CONST_DATE_NULL_CLS = ""; //1900-01-01 00:00:00

        [JsonIgnore]
        internal const string CONST_FORMAT_DATE_POLISY400 = "ddMMyyyy";
        [JsonIgnore]
        public string DateTimeCustomFormat = "yyyy-MM-dd HH:mm:ss";

       

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented,
                new ModelDatetimeConverter(this?.DateTimeCustomFormat));

           
        }
    }

    public abstract class BaseEWIRequestContentModel : BaseDataModel
    {
        [JsonIgnore]
        public BaseEWIRequestContentTransactionHeaderModel transactionHeader { get; set; }

    }
  
    
    public class BaseEWIRequestContentTransactionHeaderModel : BaseDataModel
    {
        

        public string requestId { get; set; }
        public string requestDateTime { get; set; } 
        public string system { get; set; } = "CRM";
        public string application { get; set; } = "xrmAPI";
        public string service { get; set; } = "";

    }

    public class DebugInfoDataModel : BaseDataModel
    {
        
    }
    //StackTrace stackTrace = new StackTrace();

    // Get calling method name
   // Console.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);
    public class DataModelDebugInfo: BaseDataModel
    {
        public string message { get; set; }
    
        public string methodName { get; set; }
        public string className { get; set; }
        public int line { get; set; }
       
   
        public string timestamp { get; set; } = DateTime.Now.ToString();

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
