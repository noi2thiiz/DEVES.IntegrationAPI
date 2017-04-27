using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter
{
    public class DbRequest
    {
        public DbRequest()
        {
            StoreParams = new List<StoreParams>();
        }

        public void AddParam(string key, string value, string type)
        {
            StoreParams.Add(new StoreParams(key, value, type));
        }
        public void AddParam(string key, string value)
        {
            StoreParams.Add(new StoreParams(key, value));
        }

        public string StoreName { get; set; }
        public List<StoreParams> StoreParams { get; set; }
        // public Dictionary<string, object> inputs = new Dictionary<string, object>();
    }

    public class StoreParams
    {


        public StoreParams(string key, string value, string type)
        {
            this.Key = key;
            this.Value = value;
            this.Type = type;
        }
        public StoreParams(string key, string value)
        {
            this.Key = key;
            this.Value = value;

        }

        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
    public class DataAdepterParams : DbRequest
    {
    }
}