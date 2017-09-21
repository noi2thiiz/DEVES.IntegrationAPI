using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.TechnicalService.Envelonment;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public class BaseApiController:ApiController
    {
       

        public string GetTransactionId()
        {
            try
            {
                if (string.IsNullOrEmpty(Request?.Properties["TransactionID"]?.ToStringOrEmpty()))
                {

                    Request.Properties["TransactionID"] = GlobalTransactionIdGenerator.Instance.GetNewGuid();
                }
                HttpContext.Current.Items["GlobalTransactionID"] = Request.Properties["TransactionID"];
                return Request.Properties["TransactionID"].ToString();
            }
            catch (Exception)
            {
                var newId = GlobalTransactionIdGenerator.Instance.GetNewGuid();

                try
                {
                    HttpContext.Current.Items["GlobalTransactionID"] = newId;
                }
                catch (Exception e)
                {
                    //do nothing
                }
                
                return newId;
            }
            
            
        }

        protected HttpResponseMessage ProcessRequest<TCommand, TInput>(object value,string schemaFileName)
            where TCommand : new()
            where TInput : class

        {
            // Console.WriteLine(value.ToJson());

            #region Initiate Command and Output Model
            TraceDebugLogger.Instance.AddLog("ProcessRequest", value);

            var type = typeof(TCommand);
            object instance = Activator.CreateInstance(type);
            BaseCommand cmd = (BaseCommand)instance;
            var outputFail = new OutputModelFail();
          
            cmd.TransactionId = GetTransactionId();
            try
            {
                //.Headers.Allow
                var config = GlobalConfiguration.Configuration;
                var controllerSelector = new DefaultHttpControllerSelector(config);
                var descriptor = controllerSelector.SelectController(Request);
               
                cmd.ControllerName = "" + descriptor.ControllerName;
                cmd.ApplicationName = AppEnvironment.Instance.GetApplicationName();
                cmd.SiteName = AppEnvironment.Instance.GetSiteName();
                

            }
            catch (Exception e)
            {
                TraceDebugLogger.Instance.AddLog("CreateInstance BuzCommand Exception" + e.Message, e.StackTrace);
               // throw;
            }

            

            #endregion


            string contentText = value?.ToString();
            //.Replace("=", ":"); ;
            
            //TInput contentModel;
            try
            {
                Console.WriteLine("try  Deserialize Object");
                //try  Deserialize Object
                JsonConvert.DeserializeObject<TInput>(contentText);
            }
            catch (Exception e)
            {
                //output model
                TraceDebugLogger.Instance.AddLog("Cannot parse JSON" + e.Message+ contentText, e.StackTrace);
                outputFail.code = AppConst.CODE_INVALID_INPUT;
                outputFail.message = AppConst.MESSAGE_INVALID_INPUT;
                outputFail.description = "Cannot parse JSON:"+e.Message+ contentText;
                outputFail.transactionId = GetTransactionId();
                outputFail.transactionDateTime = DateTime.Now;

                return Request.CreateResponse(outputFail);
            }
          

            string outvalidate;
            var filePath = AppEnvironment.Instance.GetJsonSchemaPhysicalPath(schemaFileName);
            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("Json Schema file not found");

            }
            
            Console.WriteLine("JSON Schema Path : "+filePath);

               TraceDebugLogger.Instance.AddLog("schema filePath", filePath);
           

            if (!JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                TraceDebugLogger.Instance.AddLog("TryValidateJson1", outvalidate);
                try
                {
                    var validationErrors = JsonHelper.GetErrorMessageOutputModel();
                   
                    
                    if (validationErrors.fieldErrors.Any() )
                    {
                        outputFail.AddOutputModelFailData(validationErrors);
                    }
                    

                    //output model
                    outputFail.code = AppConst.CODE_INVALID_INPUT;
                    outputFail.message = AppConst.MESSAGE_INVALID_INPUT;
                    outputFail.description = AppConst.DESC_INVALID_INPUT;
                    outputFail.transactionId = GetTransactionId();
                    outputFail.transactionDateTime = DateTime.Now;
                    outputFail.stackTrace = "";

                    return Request.CreateResponse(outputFail);
                }
                catch (Exception e)
                {
                    //output model
                    TraceDebugLogger.Instance.AddLog("Cannot parse JSON Exception" + e.Message + contentText, e.StackTrace);
                    outputFail.code = AppConst.CODE_INVALID_INPUT;
                    outputFail.message = AppConst.MESSAGE_INVALID_INPUT;
                    outputFail.description = "Cannot parse JSON" + outvalidate;
                    outputFail.transactionId = GetTransactionId();
                    outputFail.transactionDateTime = DateTime.Now;
                    outputFail.stackTrace = e.StackTrace;

                    return Request.CreateResponse(outputFail);
                }
            }
            else
            {
                var content = cmd.Execute(cmd.DeserializeJson<TInput>(value.ToString()));
                return Request.CreateResponse(content);
            }
            

        }

   

    }
}