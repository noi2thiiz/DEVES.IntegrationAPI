using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace DEVES.IntegrationAPI.WebApi.TechnicalService
{
    public class ApiLogHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Gen TransactionID
            var transactionId = Guid.NewGuid().ToString();
            if (!request.Properties.ContainsKey("TransactionID"))
            {
                request.Properties["TransactionID"] = transactionId;
            }
            else
            {
                transactionId = request.Properties["TransactionID"].ToString();
            }
           

            return await ProcessLog(request, cancellationToken, transactionId);
        }

        private async Task<HttpResponseMessage> ProcessLog(HttpRequestMessage request, CancellationToken cancellationToken, string transactionId)
        {
            var apiLogEntry = CreateApiLogEntryWithRequestData(request);
            apiLogEntry.TransactionID = transactionId;


            // get  controllerSelector
            apiLogEntry.Controller = "";
            try
            {
                var config = GlobalConfiguration.Configuration;
                var controllerSelector = new DefaultHttpControllerSelector(config);

                // descriptor here will contain information about the controller to which the request will be routed. If it's null (i.e. controller not found), it will throw an exception
                var descriptor = controllerSelector.SelectController(request);
                apiLogEntry.Controller = "" + descriptor.ControllerName;
                Console.WriteLine("controllerSelector : " + apiLogEntry.Controller);
            }
            catch (Exception e)
            {
                // continue
                Console.WriteLine(e);
            }


            if (request.Content != null)
            {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task => { apiLogEntry.RequestContentBody = task.Result; }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;

                    // Update the API log entry with response info

                    apiLogEntry.ResponseStatusCode = (int)response.StatusCode;
                    apiLogEntry.ResponseTimestamp = DateTime.Now;

                    if (response?.Content != null)
                    {
                        apiLogEntry.ResponseContentBody = response.Content?.ReadAsStringAsync().Result;
                        apiLogEntry.ResponseContentType = response.Content?.Headers.ContentType.MediaType;
                        apiLogEntry.ResponseHeaders = SerializeHeaders(response?.Content?.Headers);
                    }

                    InMemoryLogData.Instance.AddLogEntry(apiLogEntry);

                   // ApiLogDataGateWay.Create(apiLogEntry);

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
                RequestIpAddress = context.Request.UserHostAddress,
                RequestMethod = request.Method.Method,
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