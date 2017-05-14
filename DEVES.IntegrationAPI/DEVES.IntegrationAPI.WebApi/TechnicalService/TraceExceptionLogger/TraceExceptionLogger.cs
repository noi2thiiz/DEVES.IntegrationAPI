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
using DEVES.IntegrationAPI.WebApi.Templates;

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



            public string transactionDateTime { get; set; } ="";
   
 
        }
        public override void Log(ExceptionLoggerContext context)
        {
            var guid = GetTransactionId(context.ExceptionContext.Request) ?? Guid.NewGuid().ToString();
            var regFail = new FatalErrorModel
            {
                code = AppConst.CODE_FAILED,
                message = "An error occurred",
                transactionId = guid,
                transactionDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),//2017-05-15 02:32:18
                description = context?.Exception?.Message

            };

            try
            {
                string path = System.Configuration.ConfigurationManager.AppSettings["ERRORLOG_PATH"];
              
                path += "/" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "_" + guid + "_errorlog.log";
                //Console.WriteLine(path);
                // Create the file.

                

                string message = "";
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
                message += context?.RequestContext + Environment.NewLine;
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
            catch (Exception)
            {
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
            throw new HttpResponseException(resp);

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