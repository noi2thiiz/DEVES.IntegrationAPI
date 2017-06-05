using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger;
using DEVES.IntegrationAPI.WebApi.Templates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService
{
    public class ApiLogHandler : DelegatingHandler
    {
        System.Diagnostics.Stopwatch timer = new Stopwatch();
        string[] AllowMethod = { "POST", "GET", "PUT", "DELETE" };
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            timer = new Stopwatch();
            Debug.WriteLine("timer Start");
            timer.Start();

            // Gen TransactionID
            var transactionId = "";
           
            if (AllowMethod.Contains(request?.Method?.Method))
            {
                transactionId =  GlobalTransactionIdGenerator.Instance.GetNewGuid();
            }
            
           
            if (request != null && !request.Properties.ContainsKey("TransactionID"))
            {
                request.Properties["TransactionID"] = transactionId;
            }
            else
            {
                if (request != null) transactionId = request.Properties["TransactionID"].ToString();
            }
           

            return await ProcessLog(request, cancellationToken, transactionId);
        }

        private async Task<HttpResponseMessage> ProcessLog(HttpRequestMessage request, CancellationToken cancellationToken, string transactionId)
        {
            var apiLogEntry = CreateApiLogEntryWithRequestData(request);
            apiLogEntry.TransactionID = transactionId;
            apiLogEntry.GlobalTransactionID = transactionId;


            // get  controllerSelector
            apiLogEntry.Controller = "";
            try
            {
                var config = GlobalConfiguration.Configuration;
                var controllerSelector = new DefaultHttpControllerSelector(config);

                // descriptor here will contain information about the controller to which the request will be routed. If it's null (i.e. controller not found), it will throw an exception
                var descriptor = controllerSelector.SelectController(request);
                apiLogEntry.Controller = "" + descriptor.ControllerName;
              
            }
            catch (Exception e)
            {
                // continue
                Debug.WriteLine(e);
            }

           
            if (request.Content != null)
            {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task =>
                    {
                        apiLogEntry.RequestContentBody = task.Result;
                        TraceDebugLogger.Instance.AddLogEntry(apiLogEntry);
                    }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;

                    // Update the API log entry with response info

                    apiLogEntry.ResponseStatusCode = response?.StatusCode.ToString();
                    apiLogEntry.ResponseTimestamp = DateTime.Now;
                    apiLogEntry.Response = response;
                    apiLogEntry.Request = request;


                    timer.Stop();

                    TimeSpan t = timer.Elapsed;
                    apiLogEntry.ResponseTime  = $"{t.Hours:D2}h:{t.Minutes:D2}m:{t.Seconds:D2}s:{t.Milliseconds:D3}ms";

                    apiLogEntry.ResponseTimeTotalMilliseconds = (float)t.TotalMilliseconds;
                    Debug.WriteLine("timer Stop="+t.TotalMilliseconds);

                    if (AllowMethod.Contains(request?.Method?.Method))
                    {
                        InMemoryLogData.Instance.AddLogEntry(apiLogEntry);
                    }

                    try
                    {
                        TraceDebugLogger.Instance.RemoveLog(apiLogEntry.GlobalTransactionID);
                        GlobalTransactionIdGenerator.Instance.ClearGlobalId(apiLogEntry.GlobalTransactionID);

                    }
                    catch (Exception)
                    {
                        //donothing
                    }



                    return response;
                }, cancellationToken);
        }

        private ApiLogEntry CreateApiLogEntryWithRequestData(HttpRequestMessage request)
        {
            var requestRouteData = "";
            var routeTemplate = "";
            var context = ((HttpContextBase) request.Properties["MS_HttpContext"]);
            var routeData = request.GetRouteData();
            routeTemplate = routeData.Route.RouteTemplate;
            try
            {
                requestRouteData = routeData.ToJson();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return new ApiLogEntry
            {
                Application = "xrmAPI",
                Activity = "provide",
                User = context.User.Identity.Name,
                Machine = Environment.MachineName,
                RequestContentType = context.Request.ContentType,
                RequestRouteTemplate = routeTemplate,
                RequestRouteData = requestRouteData,
                RequestIpAddress = context?.Request?.UserHostAddress,
                RequestMethod = request?.Method?.Method,
                RequestHeaders = SerializeHeaders(request.Headers),
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()
            };
        }

        private string SerializeRouteData(IHttpRouteData routeData)
        {
            return JsonConvert.SerializeObject(routeData, Formatting.Indented);
        }

        private string SerializeHeaders(HttpHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value != null)
                {
                    var header = String.Empty;
                    foreach (var value in item.Value)
                    {
                        header += value + " ";
                    }

                    // Trim the trailing space and add item to the dictionary
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }

            return JsonConvert.SerializeObject(dict, Formatting.Indented);
        }
    }
}