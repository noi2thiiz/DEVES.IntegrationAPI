using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService
{
    public class ApiLogEntry
    {
        public string TransactionID { get; set; } = "";
        public string Activity{ get; set; } = "";

        public long ApiLogEntryId { get; set; } = 0;              // The (database) ID for the API log entry.
        public string Application { get; set; } = "XrmAPI";            // The application that made the request.
        public string User { get; set; } = "";                  // The user that made the request.
        public string Machine { get; set; } = "";                // The machine that made the request.
        public string RequestIpAddress { get; set; } = "";       // The IP address that made the request.
        public string RequestContentType { get; set; } = "";     // The request content type.
        public string RequestContentBody { get; set; } = "";    // The request content body.
        public string RequestUri { get; set; } = "";             // The request URI.
        public string RequestMethod { get; set; } = "";           // The request method (GET, POST, etc).
        public string RequestRouteTemplate { get; set; } = "";    // The request route template.
        public string RequestRouteData { get; set; } = "";      // The request route data.
        public string RequestHeaders { get; set; } = "";      // The request headers.
        public DateTime? RequestTimestamp { get; set; } = DateTime.Now;  // The request timestamp.
        public string ResponseContentType { get; set; } = "";    // The response content type.
        public string ResponseContentBody { get; set; } = "";    // The response content body.
        public string ResponseStatusCode { get; set; } = "";       // The response status code.
        public string ResponseHeaders { get; set; } = "";       // The response headers.
        public DateTime? ResponseTimestamp { get; set; } = DateTime.Now;  // The response timestamp.
        public string Controller { get; set; } = "";
        public string ServiceName { get; set; } = "";

        public string PhysicalPath { get; set; } = "";
        public string SiteName { get; set; } = "";
        

        public bool IsPersisted { get; set; } = false;
        public string GlobalTransactionID { get; set; }

        public string DebugLog { get; set; } = "";
        public int TotalRecord { get; set; } = 0;
        public string ErrorLog { get; set; } = "";

        public string Remark { get; set; } = "";

        public string ContentCode { get; set; } = "";
        public string ContentMessage { get; set; } = "";
        public string ContentDescription { get; set; } = "";
        public string ContentTransactionId { get; set; } = "";
        public string ContentTransactionDateTime { get; set; } = "";
        public string EWICode { get; set; } = "";
        public string EWIMessage { get; set; } = "";
        public string EWIToken { get; set; } = "";
        public string StackTrace { get; set; } = "";
        public string ResponseTime { get; set; } = "";
        public float ResponseTimeTotalMilliseconds { get; set; } = 0;
        public HttpRequestMessage Request { get; set; }
        public HttpResponseMessage Response { get; set; }


        public List<DataModelDebugInfo> _debugInfo { get; set; }

        public void AddDebugInfo(string message, dynamic info)
        {
            if (_debugInfo == null)
            {
                _debugInfo = new List<DataModelDebugInfo>();
               
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

        public void BuildDebugLog()
        {
            if (_debugInfo!= null && _debugInfo.Any())
            {
                DebugLog = _debugInfo.ToJson();
            }
            
        }
    }
}