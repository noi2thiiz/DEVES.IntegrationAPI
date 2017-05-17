using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class BaseProxyService
    {
        public string serviceName { get; set; }

        private string GetEwiUsername()
        {

            return AppConfig.Instance.Get("EWI_USERNAME") ?? "sysdynamic";
        }
        private string GetEwiPassword()
        {

            return AppConfig.Instance.Get("EWI_PASSWORD") ?? "SzokRk43cEM=";
        }

        private string GetEwiUid()
        {

            return AppConfig.Instance.Get("EWI_UID") ?? "DevesClaim";
        }
        private string GetEwiGid()
        {

            return AppConfig.Instance.Get("EWI_GID") ?? "DevesClaim";
        }

        public RESTClientResult SendRequest(object JSON,string endpoint)
        {
            EWIRequest reqModel = new EWIRequest()
            {
                //user & password must be switch to get from calling k.Ton's API rather than fixed values.
                username = GetEwiUsername(),
                password = GetEwiPassword(),
                uid = GetEwiUid(),
                gid = GetEwiGid(),
                token = "",
                content = JSON

            };
            endpoint = AppConfig.Instance.GetProxyEnpoint() + endpoint;
            Console.WriteLine("====Request====");
            Console.WriteLine(endpoint);
            Console.WriteLine(reqModel.ToJson());

            var client = new RESTClient(endpoint);
            var result = client.Execute(reqModel);
            //InMemoryLogData.Instance.LogRequest(serviceName,result.Request, result.Response);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("====result.Message====");
                Console.WriteLine(result.Message);  
          
                throw new InternalErrorException(result.Message);
            }
            Console.WriteLine("====result.Content====");
            Console.WriteLine(result.Content);
            return result;
        }

        protected void LogAsync(HttpWebRequest req, HttpWebResponse res = null)
        {

            // Request
            // var reqContext = ((HttpContextBase)req.Properties["MS_HttpContext"]);

            // Map Request vaule to global Variables (Request)
            //user = reqContext.User.Identity.Name;
            //ip = reqContext.Request.UserHostAddress;
            //reqContentType = reqContext.Request.ContentType;
            var uri = req.RequestUri.ToString();

            var reqMethod = req.Method.ToString();
            // routeTemplate = req.GetRouteData().Route.RouteTemplate;
            var reqHeader = req.Headers.ToString();

            try
            {
                // requestRouteData = req.GetRouteData().ToJson();
            }
            catch (Exception e)
            {
                // do nothing
            }


            // Map Request vaule to global Variables (Response)
            var resContentType = "";
            // resBody = res.Content.Headers.ToString();
            var resStatus = "";
            var resHeader = "";
            if (res != null)
            {
                resHeader = res.Headers.ToString();
            }


            var apiLogEntry = new ApiLogEntry
            {
                Application = "XrmAPI",
                TransactionID = "",
                Controller = "",
                ServiceName = serviceName,
                Activity = "consume",
                User = "",
                Machine = Environment.MachineName,
                RequestIpAddress = "",
                RequestContentType = "",
                //RequestContentBody = req?.Con,
                RequestUri = uri,
                RequestMethod = reqMethod,
                RequestRouteTemplate = "",
                RequestRouteData = "",
                ResponseStatusCode = res?.StatusCode.ToString(),
                RequestHeaders = reqHeader,
                RequestTimestamp = DateTime.Now,
                ResponseContentType = resContentType,
               // ResponseContentBody = res?.Content?.ToJson(),
                // ResponseStatusCode = resStatus,
                ResponseHeaders = resHeader,
                ResponseTimestamp = DateTime.Now
            };
            InMemoryLogData.Instance.AddLogEntry(apiLogEntry);

           
        }

        public string GetTransactionId(HttpRequestMessage Request)
        {

            if (string.IsNullOrEmpty(Request.Properties["TransactionID"].ToStringOrEmpty()))
            {

                Request.Properties["TransactionID"] = Guid.NewGuid().ToString();
            }

            return Request.Properties["TransactionID"].ToString();

        }
    }
}