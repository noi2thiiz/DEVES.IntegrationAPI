using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService
{
    public class TraceDebugLogger
    {
        private static TraceDebugLogger _instance;

        private TraceDebugLogger()
        {
            LogData = new Dictionary<string, ApiLogEntry>();
        }

        public static TraceDebugLogger Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new TraceDebugLogger();


                return _instance;
            }
        }

        public Dictionary<string, ApiLogEntry> LogData { get; set; }

        public void AddLogEntry(string globalTransactionID, ApiLogEntry log)
        {
            LogData.Add(globalTransactionID, log);
        }
        public void AddLogEntry(ApiLogEntry log)
        {
            LogData.Add(log.GlobalTransactionID, log);
        }

        public ApiLogEntry GetLogEntry(string globalTransactionID)
        {
            if (LogData.ContainsKey(globalTransactionID))
            {
                return LogData[globalTransactionID];
            }
            return null;
        }

        public void RemoveLog(string globalTransactionID)
        {
            if (LogData.ContainsKey(globalTransactionID))
            {
                 LogData.Remove(globalTransactionID);
            }
        }

        
        public void AddDebugLogInfo(string globalTransactionID, string message, dynamic info, string memberName, string sourceFilePath,int sourceLineNumber)
        {
            if (LogData.ContainsKey(globalTransactionID))
            {
               
                var log = LogData[globalTransactionID];
                log.AddDebugInfo(message, info, memberName, sourceFilePath, sourceLineNumber);
            }
        }

    }
}