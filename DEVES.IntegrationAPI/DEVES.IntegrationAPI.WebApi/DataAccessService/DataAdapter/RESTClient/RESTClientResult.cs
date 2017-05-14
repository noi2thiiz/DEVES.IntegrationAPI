using System.Net;

namespace DEVES.IntegrationAPI.WebApi.Core.DataAdepter
{
    public class RESTClientResult
    {
        public string Content { get; set; }
        public dynamic Params { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public HttpWebResponse Response { get; set; }

    }
}