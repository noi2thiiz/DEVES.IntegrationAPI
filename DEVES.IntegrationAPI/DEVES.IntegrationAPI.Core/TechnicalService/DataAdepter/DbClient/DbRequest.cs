using System.Collections.Generic;

namespace DEVES.IntegrationAPI.WebApi.Core.DataAdepter
{
    public class DbRequest
    {
        public string storeName { get; set; }
        public List<StoreParams> storeParams { get; set; }
       // public Dictionary<string, object> inputs = new Dictionary<string, object>();
    }

    public class StoreParams
    {
        public string key { get; set; }
        public string value { get; set; }
        public string type { get; set; }
    }
}