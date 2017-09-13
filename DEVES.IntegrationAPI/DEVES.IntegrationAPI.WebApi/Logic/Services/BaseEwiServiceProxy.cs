using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using System.Windows;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.TechnicalService.Envelonment;
using DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;
using Microsoft.Ajax.Utilities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{

   
    public class BaseEwiServiceProxy
    {
      



        protected const string CONST_JSON_SCHEMA_FILE = "JSON_SCHEMA_{0}";

        // variables after this line will be contained in LOG
        protected HttpClient client = new HttpClient();

        protected string appName = "xrmAPI"; // Application

        protected string siteName = ""; // Application
        protected string physicalPath = ""; // Application

        protected string serviceName = "";
        protected string systemName = "";
        protected string serviceEndpoint = "";

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

        protected string GlobalTransactionID = "";
        private string TransactionID { get; set; }
        protected string ControllerName = "";

        public void AddDebugInfo(string message, dynamic info,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
           // StackTrace stackTrace = new StackTrace();
            TraceDebugLogger.Instance.AddDebugLogInfo(GlobalTransactionID, message, info, memberName, sourceFilePath, sourceLineNumber);
            // debugInfo.AddDebugInfo(message, info);
        }
        public void AddDebugInfo(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
           // StackTrace stackTrace = new StackTrace();
            TraceDebugLogger.Instance.AddDebugLogInfo(GlobalTransactionID, message, message,  memberName, sourceFilePath, sourceLineNumber);
            // debugInfo.AddDebugInfo(message, message);
        }

        public BaseEwiServiceProxy(string globalTransactionID, string controllerName="")
        {
            SetGlobalTransactionID(globalTransactionID);
            SetControllerName(controllerName);
            appName = AppEnvironment.Instance.GetApplicationName();
            siteName = AppEnvironment.Instance.GetSiteName();
            physicalPath = AppEnvironment.Instance.GetPhysicalPath();
            
        }

        public void SetGlobalTransactionID(string id)
        {
            GlobalTransactionID = id;
        }

        public void SetControllerName(string ctrlName)
        {
            ControllerName = ctrlName;
        }

        public RESTClientResult SendRequest(BaseEWIRequestContentModel JSON, string endpoint)
        {
            TransactionID = GetTransactionId();
            AddDebugInfo("SendRequest:" + serviceName, JSON);
            System.Diagnostics.Stopwatch timer = new Stopwatch();
            timer.Start();
            resTime = DateTime.Now;
            var result = new RESTClientResult();
            var sv = endpoint.Split('/');

            serviceName = sv[sv.Length - 1];
            JSON.transactionHeader = new BaseEWIRequestContentTransactionHeaderModel();
            JSON.transactionHeader.requestDateTime = DateTime.Now.ToString(AppConst.TRANSACTION_DATE_TIME_CUSTOM_FORMAT, new CultureInfo("en-US"));
            JSON.transactionHeader.requestId = TransactionID;
            JSON.transactionHeader.application = appName;
            JSON.transactionHeader.service = ControllerName;
          
            EWIRequest reqModel = new EWIRequest()
            {
                //user & password must be switch to get from calling k.Ton's API rather than fixed values.
                username = AppConfig.GetEwiUsername(),
                password = AppConfig.GetEwiPassword(),
                uid = AppConfig.GetEwiUid(),
                gid = AppConfig.GetEwiGid(),
                token = "",
                content = JSON

            };


            jsonReqModel = JsonConvert.SerializeObject(reqModel, Formatting.Indented,
                new EWIDatetimeConverter(JSON.DateTimeCustomFormat));

            client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            var media = new MediaTypeWithQualityHeaderValue("application/json") { CharSet = "utf-8" };
            client.DefaultRequestHeaders.Accept.Add(media);
            //  client.DefaultRequestHeaders.Add("Accept-Encoding", "utf-8");
            var mediaType = new JsonMediaTypeFormatter();

            // + ENDPOINT

            string EWIendpoint = CommonConstant.PROXY_ENDPOINT + endpoint;
            reqTime = DateTime.Now;
            Console.WriteLine("==========EWIendpoint========");
            Console.WriteLine(EWIendpoint);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, EWIendpoint);
            Console.WriteLine("==========jsonReqModel========");
            Console.WriteLine(jsonReqModel);

            // request.Headers.Add("ContentType", "application/json; charset=UTF-8");
            request.Content = new StringContent(jsonReqModel, System.Text.Encoding.UTF8, "application/json");

            var resendCount = 1;
            var LimitResend = 3;
           // while (resendCount <= LimitResend)
            //{
                ++resendCount;
                try
                {


                    HttpResponseMessage response = client.SendAsync(request).Result;
                    resTime = DateTime.Now;



                    response.EnsureSuccessStatusCode();
                    timer.Stop();

                    TimeSpan timeTaken = timer.Elapsed;

                    Task<string> ewiRes = response.Content.ReadAsStringAsync();
                    //T2 output = (T2)typeof(T1).GetProperty("content").GetValue(ewiRes);
                    resBody = ewiRes.Result;
                    resTime = DateTime.Now;
                    // Console.WriteLine(resBody);


                    AddDebugInfo("SendRequest Success:", response);
                    LogRequest(request, response, timeTaken);

                    result.Content = ewiRes.Result;
                    result.StatusCode = response.StatusCode;

                    if (result.StatusCode != HttpStatusCode.OK)
                    {
                        //if (resendCount > LimitResend)
                        //{
                            throw new BuzErrorException(
                                "500",
                                $"{systemName} Error: Error on execute '{serviceName}',The request failed or the service did not respond",
                                $"Error on execute '{serviceName}',The request failed or the service did not respond",
                                systemName,
                                GlobalTransactionID);
                        //}
                        
                        
                    }

                    var responseContent = JObject.Parse(ewiRes.Result);
                    if (responseContent["responseCode"] != null)
                    {
                        if (responseContent["responseCode"].ToString() != "EWI-0000I")
                        {
                           // if (responseContent["responseCode"].ToString() == "EWI-1000E")
                           // {

                           // }
                            //else
                            //{
                                //if (resendCount > LimitResend)
                                //{
                                    throw new BuzErrorException(
                                        responseContent["responseCode"].ToString(),
                                        $"{systemName} Error:{responseContent["responseMessage"]}",
                                        $"Error on execute '{serviceName}'",
                                        systemName,
                                        GlobalTransactionID);
                                //}
                                    
                           // }
                        }




                    }
                    else
                    {
                        throw new BuzErrorException(
                            "500",
                            $"{systemName} Error: Error on execute '{serviceName}',The request failed or the service did not respond",
                            $"Error on execute '{serviceName}',The request failed or the service did not respond",
                            systemName,
                            GlobalTransactionID);
                    }

                    return result;
                }
                catch (BuzErrorException e)
                {
                    AddDebugInfo("SendRequest BuzErrorException:" + e.Message, e.StackTrace);
                    throw;
                }
                catch (Exception e)
                {
                    AddDebugInfo("SendRequest Exception:" + e.Message, e.StackTrace);
                    LogRequest(request);

                    throw new BuzErrorException(
                        "500",
                        $"{systemName} Error: Error on  execute {serviceName}, the request failed or the service did not respond",
                        $"Error on execute '{serviceName}', {e.Message}",
                        systemName,
                        GlobalTransactionID);

                }
            //}

            throw new BuzErrorException(
                "500",
                $"{systemName} Error: Error on execute '{serviceName}',The request failed or the service did not respond",
                $"Error on execute '{serviceName}',The request failed or the service did not respond",
                systemName,
                GlobalTransactionID);
        }
      
        protected void LogRequest(HttpRequestMessage req, HttpResponseMessage res, TimeSpan t)
        {
            var responseTime = "";
            float responseTimeTotalMilliseconds=0 ;
            try
            {
                responseTime = $"{t.Hours:D2}h:{t.Minutes:D2}m:{t.Seconds:D2}s:{t.Milliseconds:D3}ms";
                responseTimeTotalMilliseconds = (float)t.TotalMilliseconds;
            }
            catch (Exception)
            {
                //do nothing
            }


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
                    TransactionID = TransactionID??GetTransactionId(req),
                    GlobalTransactionID = GlobalTransactionID,
                    Controller = ControllerName,
                    ServiceName = serviceName,
                    Activity = "consume",
                    User = user,
                    SiteName = siteName,
                    PhysicalPath = physicalPath,
                    Machine = machineName,
                    RequestIpAddress = ip,
                    RequestContentType = req.Headers.Accept.ToString(),/*reqHeader*/
                    RequestContentBody = jsonReqModel,
                    RequestUri = uri,
                    RequestMethod = reqMethod,
                    RequestRouteTemplate = routeTemplate,
                    RequestRouteData = requestRouteData,
                    RequestHeaders = req.Headers.ToJson(),
                    RequestTimestamp = reqTime,
                    ResponseContentType = resContentType,
                    ResponseContentBody = resBody,
                    ResponseStatusCode = res?.StatusCode.ToString(),
                    ResponseHeaders = res?.Headers.ToJson(),
                    ResponseTimestamp = resTime,
                   
                    ResponseTime = responseTime,
                    ResponseTimeTotalMilliseconds= responseTimeTotalMilliseconds,
                    Remark = "used BaseEwiServiceProxy"



                };

               
                InMemoryLogData.Instance.AddLogEntry(apiLogEntry);


            }
            catch (Exception)
            {
                //donothing
            }
        }

        protected void LogRequest(HttpRequestMessage req)
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




                // Map Request vaule to global Variables (Response)
                resContentType = "";
                // resBody = res.Content.Headers.ToString();
                resStatus = "";



                var apiLogEntry = new ApiLogEntry
                {
                    Application = appName,
                    TransactionID = TransactionID??GetTransactionId(req),
                    Controller = ControllerName,
                    ServiceName = serviceName,
                    Activity = "consume:Send request",
                    User = user,
                    SiteName = siteName,
                    PhysicalPath = physicalPath,
                    Machine = machineName,
                    RequestIpAddress = ip,
                    RequestContentType = client.DefaultRequestHeaders?.Accept.ToString(),
                    RequestContentBody = jsonReqModel,

                    RequestUri = uri,
                    RequestMethod = reqMethod,
                    RequestRouteTemplate = routeTemplate,
                    RequestRouteData = requestRouteData,
                    RequestHeaders = reqHeader,
                    RequestTimestamp = reqTime,

                    ResponseTimestamp = resTime,
                    Remark = "used BaseEwiServiceProxy"

                };
                InMemoryLogData.Instance.AddLogEntry(apiLogEntry);
            }
            catch (Exception)
            {
                //donothing
            }

           
        }


        //internal string CallEWIService(string EWIendpointKey, BaseDataModel JSON)
        //{
        //    string UID = "ClaimMotor";
        //    EWIRequest reqModel = new EWIRequest()
        //    {
        //        //user & password must be switch to get from calling k.Ton's API rather than fixed values.
        //        username = "sysdynamic",
        //        password = "REZOJUNtN04=",
        //        uid = UID,
        //        gid = UID,
        //        token = GetLatestToken(),
        //        content = JSON
        //    };

        //    string jsonReqModel = JsonConvert.SerializeObject(reqModel, Formatting.Indented, new EWIDatetimeConverter());

        //    HttpClient client = new HttpClient();

        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    client.DefaultRequestHeaders.Add("Accept-Encoding", "utf-8");

        //    // + ENDPOINT
        //    string EWIendpoint = GetEWIEndpoint(EWIendpointKey);
        //    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, EWIendpoint);
        //    request.Content = new StringContent(jsonReqModel, System.Text.Encoding.UTF8, "application/json");

        //    // เช็ค check reponse 
        //    HttpResponseMessage response = client.SendAsync(request).Result;
        //    response.EnsureSuccessStatusCode();

        //    string strResult = response.Content.ReadAsStringAsync().Result;
        //    return strResult;
        //}

        internal T DeserializeJson<T>(string contentText)
        {
            Console.WriteLine(contentText.ToString());
            T contentModel;
            string validateResult = string.Empty;
            Type objType = typeof(T);
            string key = string.Format(CONST_JSON_SCHEMA_FILE, objType.Name);
            var filePath = HttpContext.Current.Server.MapPath(GetAppConfigurationSetting(key));
            Console.WriteLine(filePath);
            if (JsonHelper.TryValidateJson(contentText, filePath, out validateResult))
            {
                Console.WriteLine("DeserializeObject");
                contentModel = JsonConvert.DeserializeObject<T>(contentText);
                return contentModel;
            }
            else
            {
                Console.WriteLine("Validation Error!");
                Console.WriteLine(validateResult.ToJson());
                throw new JsonSerializationException("Validation Error!");
            }
        }

        internal string GetAppConfigurationSetting(string key)
        {
            // Console.WriteLine("==================key=================");
            // Console.WriteLine(key);
            // Console.WriteLine("==================End Key=================");
            return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
        }

        public string GetTransactionId(HttpRequestMessage Request=null)
        {
            
            return GlobalTransactionIdGenerator.Instance.GetNewGuid(GlobalTransactionID);

        }

        private static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }

    public class EWIJSONResponseModel
    {



        public string responseCode { set; get; } = "";
        public string responseMessage { get; set; } = "";

       // public BaseEWIJSONContent content { get; set; }
    }
    public  class BaseEWIJSONContent 
    {

        public string code { get; set; } = "";


        public string message { get; set; } = "";


        public string description { get; set; } = "";




    }
}