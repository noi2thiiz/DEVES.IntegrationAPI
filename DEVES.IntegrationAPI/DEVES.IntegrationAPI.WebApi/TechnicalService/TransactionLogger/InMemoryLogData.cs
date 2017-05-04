using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger
{
    public class InMemoryLogData
    {
        public List<ApiLogEntry> LogData { get; set; }
        private static InMemoryLogData _instance;

        private InMemoryLogData()
        {
            LogData = new List<ApiLogEntry>();
        }

        public static InMemoryLogData Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new InMemoryLogData();

              
                return _instance;
            }
        }

     

        public void AddLogEntry(ApiLogEntry log)
        {
            LogData.Add(log);
        }

        
      
    }
    
}
