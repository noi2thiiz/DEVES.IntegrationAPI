using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.WebApi.Templates;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger
{
    public class InMemoryLogData
    {
        public List<ApiLogEntry> LogData { get; set; }
        private static InMemoryLogData _instance;

        private InMemoryLogData()
        {
            LogData = new List<ApiLogEntry>();
        }

        public static InMemoryLogData Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new InMemoryLogData();

              
                return _instance;
            }
        }

     

        public void AddLogEntry(ApiLogEntry log)
        {
            var response = log.Response;
            if (response?.Content != null)
            {
                log.ResponseContentBody = response.Content?.ReadAsStringAsync().Result;
                log.ResponseContentType = response.Content?.Headers.ContentType.MediaType;
                log.ResponseHeaders = response?.Content?.Headers.ToJson();
            }
            try
            {
                var responseContent = JObject.Parse(log.ResponseContentBody);

                var responseContentCode = "";
                var responseContentMessage = "";
                var responseContentDescription = "";
                var responseContentId = "";
                var responseContentDateTime = "";
                if (responseContent != null)
                {
                    log.ContentCode = responseContent["code"]?.ToString() ?? "";
                    log.ContentMessage = responseContent["message"]?.ToString() ?? "";
                    log.ContentDescription = responseContent["description"]?.ToString() ?? "";
                    log.ContentTransactionId = responseContent["transactionId"]?.ToString() ?? "";
                    log.ContentTransactionDateTime =
                        responseContent["transactionDateTime"]?.ToString() ?? "";

                    if (!string.IsNullOrEmpty(responseContent["data"]?.ToString()))
                    {

                        var token = JToken.Parse(responseContent["data"].ToString());

                        if (token is JArray)
                        {
                            IEnumerable<object> items = token.ToObject<List<object>>();
                            log.TotalRecord = items.Count();
                        }
                        else if (token is JObject)
                        {
                            log.TotalRecord = 1;
                        }
                        else
                        {
                            log.TotalRecord = 0;
                        }
                    }
                }




                if (responseContent["_debugInfo"] != null)
                {
                    log.DebugLog = responseContent["_debugInfo"].ToString();
                  
                }

                if (responseContent["stackTrace"] != null)
                {
                    log.StackTrace = responseContent["stackTrace"].ToString();
                   
                }


                //  var m = new JsonMediaTypeFormatter();
               // response.Content = new StringContent(responseContent.ToString(), System.Text.Encoding.UTF8,
               //     "application/json");
                // ApiLogDataGateWay.Create(apiLogEntry);
            }
            catch (Exception e)
            {
                Debug.WriteLine("142" + e.Message);
            }
            LogData.Add(log);
        }


        public void LogRequest(string serviceName, string endpoint, EWIRequest reqModel)
        {
            throw new NotImplementedException();
        }

        public void LogRequest(string serviceName, HttpWebRequest resultRequest, HttpWebResponse resultResponse)
        {
            var log = new ApiLogEntry();
            log.ServiceName = serviceName;
            log.RequestUri = resultRequest.Address.ToString();
            LogData.Add(log);
        }
    }
    
}
