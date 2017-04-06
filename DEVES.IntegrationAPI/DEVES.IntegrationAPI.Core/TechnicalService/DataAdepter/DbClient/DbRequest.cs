using System.Collections.Generic;

namespace DEVES.IntegrationAPI.WebApi.Core.DataAdepter
{
    public class DbRequest
    {
        public DbRequest()
        {
            StoreParams = new List<StoreParams>();
        }

        public void AddParam(string key, string value, string type)
        {
            StoreParams.Add(new StoreParams(key, value,type));
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
            this.key = key;
            this.value = value;
            this.type = type;
        }
        public StoreParams(string key, string value)
        {
            this.key = key;
            this.value = value;
        
        }

        public string key { get; set; }
        public string value { get; set; }
        public string type { get; set; }
    }
    public class DataAdepterParams : DbRequest
    {
    }

}