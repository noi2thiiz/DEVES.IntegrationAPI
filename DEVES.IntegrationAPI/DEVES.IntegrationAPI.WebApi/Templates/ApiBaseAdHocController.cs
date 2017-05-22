using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public class ApiBaseAdHocController:BaseApiController
    {
        protected HttpClient client = new HttpClient();

        protected const string appName = "xrmAPI"; // Application
        protected string serviceName = "";
        protected const string activity = "consume"; // Activity
        protected string user = ""; // User
        protected string machineName = Environment.MachineName; // Machine
        protected string ip = ""; // RequestIpAddress;
        protected string reqContentType = ""; // RequestContentType = context.Request.ContentType;
        protected string jsonReqModel = ""; // RequestContentBody
        protected string uri = ""; // RequestUri
        protected string reqMethod = ""; // RequestMethod
        protected string routeTemplate = ""; // RequestRouteTemplate
        protected string requestRouteData = ""; // RequestRouteData
        protected string reqHeader = ""; // RequestHeaders
        protected DateTime reqTime = new DateTime(); // RequestTimestamp
        protected string resContentType = ""; // ResponseContentType
        protected string resBody = ""; // ResponseContentBody
        protected string resStatus = ""; // ResponseStatusCode
        protected string resHeader = ""; // ResponseHeaders
        protected DateTime resTime = new DateTime(); // ResponseTimestamp

        public string GetTransactionId(HttpRequestMessage Request)
        {
            try
            {
                if (string.IsNullOrEmpty(Request.Properties["TransactionID"].ToStringOrEmpty()))
                {
                    Request.Properties["TransactionID"] = Guid.NewGuid().ToString();
                }

                return Request.Properties["TransactionID"].ToString();
            }
            catch (Exception)
            {
                Request.Properties["TransactionID"] = Guid.NewGuid().ToString();
                return Request.Properties["TransactionID"].ToString();
            }
        }

        protected void LogAsync(HttpRequestMessage req, HttpResponseMessage res)
        {
            try
            {
                // Request
                // var reqContext = ((HttpContextBase)req.Properties["MS_HttpContext"]);

                // Map Request vaule to global Variables (Request)
                //user = reqContext.User.Identity.Name;
                //ip = reqContext.Request.UserHostAddress;
                //reqContentType = reqContext.Request.ContentType;
                uri = req.RequestUri.ToString();

                reqMethod = req.Method.Method;
                // routeTemplate = req.GetRouteData().Route.RouteTemplate;
                reqHeader = req.Headers.ToString();

                resTime = DateTime.Now;
            
                var sv = uri.Split('/');

                serviceName = sv[sv.Length - 1];

               


                // Map Request vaule to global Variables (Response)
                resContentType = "";
                // resBody = res.Content.Headers.ToString();
                resStatus = "";
                if (res != null)
                {
                    resHeader = res.Headers.ToString();
                }


                var apiLogEntry = new ApiLogEntry
                {
                    Application = appName,
                    TransactionID = GetTransactionId(req),
                    Controller = "",
                    ServiceName = serviceName,
                    Activity = "consume:Receive response",
                    User = user,
                    Machine = machineName,
                    RequestIpAddress = ip,
                    RequestContentType = client.DefaultRequestHeaders?.Accept.ToString(),
                    RequestContentBody = jsonReqModel,
                    RequestUri = req.RequestUri.ToString(),
                    RequestMethod = "POST",
                    RequestRouteTemplate = routeTemplate,
                    RequestRouteData = requestRouteData,
                    RequestHeaders = reqHeader,
                    RequestTimestamp = DateTime.Now,
                    ResponseContentType = resContentType,
                    ResponseContentBody = resBody,
                    ResponseStatusCode = res?.StatusCode.ToString(),
                    ResponseHeaders = res?.Headers.ToJson(),
                    ResponseTimestamp = DateTime.Now
            };
                InMemoryLogData.Instance.AddLogEntry(apiLogEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + ":" + e.StackTrace);
                //TODO Do something
            }
        }

        protected void LogAsync(HttpRequestMessage req,string jsonReqModel)
        {
            try
            {

                // Request
                // var reqContext = ((HttpContextBase)req.Properties["MS_HttpContext"]);

                // Map Request vaule to global Variables (Request)
                //user = reqContext.User.Identity.Name;
                //ip = reqContext.Request.UserHostAddress;
                //reqContentType = reqContext.Request.ContentType;
                uri = req.RequestUri.ToString();

                reqMethod = req.Method.Method;
                // routeTemplate = req.GetRouteData().Route.RouteTemplate;
                reqHeader = req.Headers.ToString();

                var sv = uri.Split('/');

                serviceName = sv[sv.Length - 1];
                try
                {
                    // requestRouteData = req.GetRouteData().ToJson();
                }
                catch (Exception e)
                {
                    // do nothing
                }


                // Map Request vaule to global Variables (Response)
                resContentType = "";
                // resBody = res.Content.Headers.ToString();
                resStatus = "";


                var apiLogEntry = new ApiLogEntry
                {
                    Application = appName,
                    TransactionID = GetTransactionId(),
                    Controller = "",
                    ServiceName = serviceName,
                    Activity = "consume:Send request",
                    User = user,
                    Machine = machineName,
                    RequestIpAddress = ip,
                    RequestContentType = client.DefaultRequestHeaders?.Accept.ToString(),
                    RequestContentBody = jsonReqModel,

                    RequestUri = req.RequestUri.ToString(),
                    RequestMethod = "POST",
                    RequestRouteTemplate = routeTemplate,
                    RequestRouteData = requestRouteData,
                    RequestHeaders = reqHeader,
                    RequestTimestamp = DateTime.Now,

                    ResponseTimestamp = DateTime.Now
                };
                InMemoryLogData.Instance.AddLogEntry(apiLogEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message+":"+e.StackTrace);
                //TODO Do something
            }
        }

        protected void LogErrorException(Exception e)
        {
            try
            {
                var apiLogEntry = new ApiLogEntry
                {
                    Application = appName,
                    TransactionID = GetTransactionId(),
                    Controller = "",
                    ServiceName = serviceName,
                    Activity = "Error Exception",
                    User = user,
                    Machine = machineName,
                    RequestIpAddress = ip,
                    RequestContentType = client.DefaultRequestHeaders?.Accept.ToString(),
                    RequestContentBody = e.Message + e.StackTrace,

                    RequestUri = "",
                    RequestMethod = reqMethod,
                    RequestRouteTemplate = routeTemplate,
                    RequestRouteData = requestRouteData,
                    RequestHeaders = reqHeader,
                    RequestTimestamp = DateTime.Now,

                    ResponseTimestamp = DateTime.Now
                };
                InMemoryLogData.Instance.AddLogEntry(apiLogEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ":" + ex.StackTrace);
            }
        }
    }
}