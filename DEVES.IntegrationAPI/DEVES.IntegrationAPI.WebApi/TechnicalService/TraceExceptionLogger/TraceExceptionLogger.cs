using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger;
using DEVES.IntegrationAPI.WebApi.Templates;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService.TraceExceptionLogger
{
    public class TraceExceptionLogger : ExceptionLogger
    {
        public class FatalErrorModel
        {

            public string code { get; set; } = "";


            public string message { get; set; } = "";


            public string description { get; set; } = "";


            public string transactionId { get; set; } = "";



            public string transactionDateTime { get; set; } = "";
            public string stackTrace { get; set; } ="";

            


        }
        public override void Log(ExceptionLoggerContext context)
        {
            string message = "";
            var guid = GetTransactionId(context.ExceptionContext.Request) ?? Guid.NewGuid().ToString();
            var regFail = new FatalErrorModel
            {
                code = AppConst.CODE_FAILED,
                message = "An error occurred",
                transactionId = guid,
                transactionDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),//2017-05-15 02:32:18
                description = context?.Exception?.Message,
                stackTrace = ""+ context?.Exception?.InnerException


            };

            try
            {
                string dir = System.Configuration.ConfigurationManager.AppSettings["ERRORLOG_PATH"];

                dir += "/" + DateTime.Now.ToString("dd-MM-yyyy");
                string path = dir + "/" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")+"_"+ guid + "_log.log";
                //Console.WriteLine(path);
                // Create the file.
                if (!Directory.Exists(dir))
                {
                    DirectoryInfo di = Directory.CreateDirectory(dir);
                   
                }

                Task<string> ewiRes = context?.Request?.Content?.ReadAsStringAsync();
                string requestContextBody = ewiRes?.Result??"";



                    message += "TransactionId:" + guid;
                message += "" + Environment.NewLine;
                message += "MachineName:" + Environment.MachineName + Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "Date:" + DateTime.Now+Environment.NewLine ;
                message += "" + Environment.NewLine;
                message += "Message:" + context?.Exception?.Message + Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "Source:" + context?.Exception?.Source + Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "Url:" + context?.Request?.RequestUri + Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "Method:" + context?.Request?.Method + Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "------------------------------------" + Environment.NewLine;
                message += "Headers" + Environment.NewLine;
                message += "------------------------------------" + Environment.NewLine;
                message += context?.Request?.Headers.ToJson() + Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "------------------------------------" + Environment.NewLine;
                message += "Request Context" + Environment.NewLine;
                message += "------------------------------------" + Environment.NewLine;
                message += requestContextBody?.ToString()+ Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "------------------------------------" + Environment.NewLine;
                message += "Response Content" + Environment.NewLine;
                message += "------------------------------------" + Environment.NewLine;
                message += regFail?.ToJson() + Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "------------------------------------" + Environment.NewLine;
                message += "ExceptionContext" + Environment.NewLine;
                message += "------------------------------------" + Environment.NewLine;
                message += context.ExceptionContext.Exception + Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "------------------------------------" + Environment.NewLine;
                message += "Inner Exception" + Environment.NewLine;
                message += "------------------------------------" + Environment.NewLine;
                message += context?.Exception?.InnerException + Environment.NewLine;

                message += "" + Environment.NewLine;
                message += "" + Environment.NewLine;
                message += "------------------------------------" + Environment.NewLine;
                message += "Inner Exception" + Environment.NewLine;
                message += "------------------------------------" + Environment.NewLine;



                //File.Create(path);
                
                File.WriteAllText(path, message);
                

            }
            catch (Exception e)
            {
                message += "Error On Log " + e.Message + ":" + e.StackTrace;
                // ignored
            }
            context.Request.CreateResponse(regFail);
            var m = new JsonMediaTypeFormatter();
            var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new ObjectContent(typeof(FatalErrorModel), regFail, m, "application/json"),
                ReasonPhrase = "An error occurred",
                StatusCode = HttpStatusCode.OK,

            };

            // Log to DataBAse
            try
            {
                var log = TraceDebugLogger.Instance.GetLogEntry(guid);
                if (log != null)
                {
                    log.ErrorLog = log.StackTrace + Environment.NewLine+ message;
                    log.ResponseContentBody = regFail.ToString();
                    log.Remark = "Log By TraceExceptionLogger";
                   // InMemoryLogData.Instance.AddLogEntry(log);
                    //GlobalTransactionIdGenerator.Instance.ClearGlobalId(guid);
                }
            }
            catch (Exception e)
            {
                //do nothing
                Debug.WriteLine(e.Message+e.StackTrace);
            }
            throw new HttpResponseException(resp);

        }

        public string GetTransactionId(HttpRequestMessage Request)
        {
            try
            {
                if (string.IsNullOrEmpty(Request?.Properties["TransactionID"]?.ToStringOrEmpty()))
                {

                    Request.Properties["TransactionID"] = GlobalTransactionIdGenerator.Instance.GetNewGuid();
                }

                return Request.Properties["TransactionID"].ToString();
            }
            catch (Exception)
            {
                Request.Properties["TransactionID"] = GlobalTransactionIdGenerator.Instance.GetNewGuid();
                return Request.Properties["TransactionID"].ToString();
            }

            

        }
    }
}