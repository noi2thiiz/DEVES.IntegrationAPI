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
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
           
            timer.Start();

            // Gen TransactionID
            var transactionId = GlobalTransactionIdGenerator.Instance.GetNewGuid();
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

                    apiLogEntry.ResponseStatusCode = response?.StatusCode.ToString();
                    apiLogEntry.ResponseTimestamp = DateTime.Now;

                    if (response?.Content != null)
                    {
                        apiLogEntry.ResponseContentBody = response.Content?.ReadAsStringAsync().Result;
                        apiLogEntry.ResponseContentType = response.Content?.Headers.ContentType.MediaType;
                        apiLogEntry.ResponseHeaders = SerializeHeaders(response?.Content?.Headers);
                    }
                    try
                    {
                        var responseContent = JObject.Parse(apiLogEntry.ResponseContentBody);

                        var responseContentCode = "";
                        var responseContentMessage = "";
                        var responseContentDescription = "";
                        var responseContentId = "";
                        var responseContentDateTime = "";
                        if (responseContent != null)
                        {
                            apiLogEntry.ContentCode = responseContent["code"]?.ToString() ?? "";
                            apiLogEntry.ContentMessage = responseContent["message"]?.ToString() ?? "";
                            apiLogEntry.ContentDescription = responseContent["description"]?.ToString() ?? "";
                            apiLogEntry.ContentTransactionId = responseContent["transactionId"]?.ToString() ?? "";
                            apiLogEntry.ContentTransactionDateTime =
                                responseContent["transactionDateTime"]?.ToString() ?? "";

                            if ( !string.IsNullOrEmpty(responseContent["data"]?.ToString()))
                            {
                                
                                var token = JToken.Parse(responseContent["data"].ToString());

                                if (token is JArray)
                                {
                                    IEnumerable<object> items = token.ToObject<List<object>>();
                                    apiLogEntry.TotalRecord = items.Count();
                                }
                                else if (token is JObject)
                                {
                                    apiLogEntry.TotalRecord = 1;
                                }
                                else
                                {
                                    apiLogEntry.TotalRecord = 0;
                                }
                            }
                        }


                        

                        if (responseContent["_debugInfo"] != null)
                        {
                            apiLogEntry.DebugLog = responseContent["_debugInfo"].ToString();
                            responseContent["_debugInfo"] = null;
                        }

                        if (responseContent["stackTrace"] != null)
                        {
                            apiLogEntry.StackTrace = responseContent["stackTrace"].ToString();
                            responseContent["stackTrace"] = null;
                        }


                        //  var m = new JsonMediaTypeFormatter();
                        response.Content = new StringContent(responseContent.ToString(), System.Text.Encoding.UTF8,
                            "application/json");
                        // ApiLogDataGateWay.Create(apiLogEntry);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("142"+e.Message);
                    }
                    timer.Stop();

                    TimeSpan timeTaken = timer.Elapsed;
                    apiLogEntry.ResponseTime = timeTaken.ToString();
                    InMemoryLogData.Instance.AddLogEntry(apiLogEntry);
                    Console.WriteLine(response.Content);
                    GlobalTransactionIdGenerator.Instance.ClearGlobalId(apiLogEntry.TransactionID);

                    
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