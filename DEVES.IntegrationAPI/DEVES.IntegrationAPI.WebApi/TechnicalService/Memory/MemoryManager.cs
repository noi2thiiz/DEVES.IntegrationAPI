using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// HttpContext.Current.Session["userMessages"] = null; //  HttpContext.Current.Session["userMessages"] = new UserMessage(); 
namespace DEVES.IntegrationAPI.WebApi.TechnicalService
{
    public class MemoryManager
    {
        private static readonly MemoryManager Instant = new MemoryManager();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static MemoryManager()
        {
           
        }

       
        public static MemoryManager Memory
        {
            get { return Instant; }
        }
        protected  Dictionary<string, dynamic> Store { get; set; } = new Dictionary<string, dynamic>();

        public void SetItem(string key, dynamic value) {

            Store.Add(key, value);

        }

        public dynamic GetItem (string key)
        {
            return Store[key];

        }

        public bool ContainsKey(string key)
        {
            return Store.ContainsKey(key);
        }

    }
}
