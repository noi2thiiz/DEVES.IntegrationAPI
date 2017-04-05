using System.Net;

namespace DEVES.IntegrationAPI.WebApi.Core.DataAdepter
{
    public class RESTClientResult
    {
        public string content { get; set; }
        public string message { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public HttpWebResponse response { get; set; }

    }
}