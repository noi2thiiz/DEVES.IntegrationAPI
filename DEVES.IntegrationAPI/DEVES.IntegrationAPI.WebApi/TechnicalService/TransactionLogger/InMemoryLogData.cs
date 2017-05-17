using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using DEVES.IntegrationAPI.Model.EWI;

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


        public void LogRequest(string serviceName, string endpoint, EWIRequest reqModel)
        {
            throw new NotImplementedException();
        }

        public void LogRequest(string serviceName, HttpWebRequest resultRequest, HttpWebResponse resultResponse)
        {
            var log = new ApiLogEntry();
            log.ServiceName = serviceName;
            log.RequestUri = resultRequest.Address.ToString();
            LogData.Add(log);
        }
    }
    
}
