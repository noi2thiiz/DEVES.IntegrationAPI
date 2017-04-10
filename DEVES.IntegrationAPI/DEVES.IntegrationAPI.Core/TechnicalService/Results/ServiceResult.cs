using System;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;

namespace DEVES.IntegrationAPI.WebApi.Core
{


    public class InternalServerErrorResult : ServiceResultHeaderOnly
    {

    }
    public class InternalServerErrorModel : ServiceResultHeaderOnly
    {

    }

    public class BadRequestModel : ServiceBadRequestResult
    {

    }


    public class RemoteServiceFailResult : ServiceFailResult
    {

    }
public class ServiceResultHeaderOnly : IServiceResult
    {
    public string code { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string transactionId { get; set; }
    public string transactionDateTime { get; set; }



    public ServiceResultHeaderOnly()
    {

    }


    public void AddRequest(object request)
    {

    }


    public void setHeaderProperty(string key, string value)
    {
        Type type = this.GetType();
        PropertyInfo pi = type.GetProperty(key);
        pi.SetValue(this, value, null);
    }

    public void AddBodyData(object data)
    {

    }

    private RESTClientResult _response { get; set; }

    public RESTClientResult GetResponse()
    {
        return _response;
    }

    public void SetResponse(RESTClientResult response)
    {
        _response = response;
    }
}

    public class ServiceResult<MODEL_DATA_TYPE> : IServiceResult
        where MODEL_DATA_TYPE : class
    {
        [JsonProperty(Order = 1)]
        public string code { get; set; }
        [JsonProperty(Order = 2)]
        public string message { get; set; }
        [JsonProperty(Order = 3)]
        public string description { get; set; }
        [JsonProperty(Order = 4)]
        public string transactionId { get; set; }
        [JsonProperty(Order = 5)]
        public string transactionDateTime { get; set; }
        [JsonProperty(Order = 6)]
        private object request { get; set; }
        [JsonProperty(Order = 7)]
        public List<MODEL_DATA_TYPE> data { get; set; }

        public ServiceResult()
        {
            // Header = new ServiceResultHeader();
            data = new List<MODEL_DATA_TYPE>();
        }

        // public ServiceResultHeader Header { get; set; }

        // public ServiceResultBodyList<MODEL_DATA_TYPE>Body { get; set; }
        // public object Request { get; set; }

        public void AddRequest(object request)
        {
            this.request = request;
        }


        public void setHeaderProperty(string key, string value)
        {
            Type type = this.GetType();
            PropertyInfo pi = type.GetProperty(key);
            pi.SetValue(this, value, null);
        }

        public void AddBodyData(dynamic data)
        {
            this.data = (List<MODEL_DATA_TYPE>)data;
        }
        public void AddBodyData(List<MODEL_DATA_TYPE> data)
        {
            foreach (var item in data)
            {
                this.data.Add((MODEL_DATA_TYPE)item);
            }
           
        }


        protected RESTClientResult _response { get; set; }

        public RESTClientResult GetResponse()
        {
            return _response;
        }

        public void SetResponse(RESTClientResult response)
        {
            _response = response;
        }


    }


    public class ServiceResultSingleData<MODEL_DATA_TYPE> : IServiceResult
        where MODEL_DATA_TYPE : new()
    {
        [JsonProperty(Order = 1)]
        public string code { get; set; }
        [JsonProperty(Order = 2)]
        public string message { get; set; }
        [JsonProperty(Order = 3)]
        public string description { get; set; }
        [JsonProperty(Order = 4)]
        public string transactionId { get; set; }
        [JsonProperty(Order = 5)]
        public string transactionDateTime { get; set; }
        [JsonProperty(Order = 6)]
        public MODEL_DATA_TYPE data { get; set; }
       // [JsonProperty(Order = 7)]
       // private DebugInfo _debugInfo  { get; set; }


        public ServiceResultSingleData()
        {
            // Header = new ServiceResultHeader();
            data = new MODEL_DATA_TYPE();
            //_debugInfo = new DebugInfo();
        }

        //public ServiceResultHeader Header { get; set; }

        // public Object Request { get; set; }
        //public ServiceResultBodyObject<MODEL_DATA_TYPE> Body { get; set; }

        public void AddRequest(dynamic request)
        {
           // this._debugInfo.request = request;
        }

        private dynamic _content { get; set; }

        public void AddContent(string dataContent)
        {
           // this._debugInfo.responseContentString = dataContent;
        }


        public void setHeaderProperty(string key, string value)
        {
            Type type = this.GetType();
            PropertyInfo pi = type.GetProperty(key);
            pi.SetValue(this, value, null);
        }

        public void AddBodyData(dynamic data)
        {
            this.data = ((MODEL_DATA_TYPE) data);
        }


        private RESTClientResult _response;

        public RESTClientResult GetResponse()
        {
            return _response;
        }

        public void SetResponse(RESTClientResult response)
        {
            _response = response;
        }
    }

    public class DebugInfo{
        public string remark = "The _debugInfo will be removed on production!!";
        public object request { get; set; }
        public RESTClientResult response { get; set; }
        public object responseContentString { get; set; }
    }
}