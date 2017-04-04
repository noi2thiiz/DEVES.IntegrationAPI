using System;
using System.Net;

namespace DEVES.IntegrationAPI.WebApi.Services
{
    public class WebServiceException:Exception
    {
        public HttpWebResponse response { get; set; }

        public WebServiceException(HttpWebResponse response, string msg)
        {
           // r.setHeaderProperty("code","400");
           // r.setHeaderProperty("message","Bad Request");
           // r.setHeaderProperty("description","The server cannot or will not process the request due to an apparent client error (e.g., malformed request syntax, too large size, invalid request message framing, or deceptive request routing)");

            this.response = response;
        }
    }
}