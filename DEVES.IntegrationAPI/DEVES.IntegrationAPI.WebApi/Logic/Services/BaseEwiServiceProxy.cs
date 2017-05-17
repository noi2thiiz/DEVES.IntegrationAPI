using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class BaseEwiServiceProxy
    {
        protected const string CONST_CODE_SUCCESS = "200";
        protected const string CONST_MESSAGE_SUCCESS = "SUCCESS";
        protected const string CONST_CODE_FAILED = "500";
        protected const string CONST_MESSAGE_INTERNAL_ERROR = "An error has occurred";
        protected const string CONST_DEFAULT_UID = "uid";
        protected const string CONST_CODE_INVALID_INPUT = "400";
        protected const string CONST_MESSAGE_INVALID_INPUT = "Invalid input(s)";
        protected const string CONST_DESC_INVALID_INPUT = "Some of your input is invalid. Please recheck again";


        protected string TransactionId { get; set; } = "";

        protected OrganizationServiceProxy _serviceProxy;
        protected IOrganizationService _service;

        protected Guid CurrentUserId { get; set; }

        protected const string CONST_JSON_SCHEMA_FILE = "JSON_SCHEMA_{0}";

        // variables after this line will be contained in LOG
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


        public RESTClientResult SendRequest(BaseDataModel JSON, string endpoint)
        {
            var result = new RESTClientResult();
            EWIRequest reqModel = new EWIRequest()
            {
                //user & password must be switch to get from calling k.Ton's API rather than fixed values.
                username = AppConfig.GetEwiUsername(),
                password = AppConfig.GetEwiPassword(),
                uid = AppConfig.GetEwiUid(),
                gid = AppConfig.GetEwiGid(),
                token ="",
                content = JSON

            };


            jsonReqModel = JsonConvert.SerializeObject(reqModel, Formatting.Indented, new EWIDatetimeConverter(JSON.DateTimeCustomFormat));

            client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Accept-Encoding", "utf-8");

          
            // + ENDPOINT
            string EWIendpoint = CommonConstant.PROXY_ENDPOINT + endpoint;
            reqTime = DateTime.Now;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, EWIendpoint);
            Console.WriteLine("==========jsonReqModel========");
            Console.WriteLine(jsonReqModel.ToJson());
            request.Headers.Add("ContentType", "application/json; charset=UTF-8");
            request.Content = new StringContent(jsonReqModel, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.SendAsync(request).Result;
            resTime = DateTime.Now;

            LogAsync(request, response);

            response.EnsureSuccessStatusCode();
            Task<string> ewiRes = response.Content.ReadAsStringAsync();
            //T2 output = (T2)typeof(T1).GetProperty("content").GetValue(ewiRes);
           
            result.Content = ewiRes.Result;
            result.StatusCode = response.StatusCode;


            return result;
        }
      
        protected async Task<ApiLogEntry> LogAsync(HttpRequestMessage req, HttpResponseMessage res = null)
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
            if (res != null)
            {
                resHeader = res.Headers.ToString();
            }


            var apiLogEntry = new ApiLogEntry
            {
                Application = appName,
                TransactionID = TransactionId,
                Controller = "",
                ServiceName = serviceName,
                Activity = activity,
                User = user,
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
                ResponseContentType = resContentType,
                ResponseContentBody = resBody,
                // ResponseStatusCode = resStatus,
                ResponseHeaders = resHeader,
                ResponseTimestamp = resTime
            };
            InMemoryLogData.Instance.AddLogEntry(apiLogEntry);

            return null;
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

    }
}