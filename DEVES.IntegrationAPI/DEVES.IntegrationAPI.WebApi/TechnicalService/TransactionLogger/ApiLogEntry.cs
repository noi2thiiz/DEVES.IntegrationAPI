﻿using System;

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
        public int? ResponseStatusCode { get; set; } = 0;       // The response status code.
        public string ResponseHeaders { get; set; } = "";       // The response headers.
        public DateTime? ResponseTimestamp { get; set; } = DateTime.Now;  // The response timestamp.
        public string Controller { get; set; } = "";
        public string ServiceName { get; set; } = "";

        public bool IsPersisted { get; set; } = false;
    }
}