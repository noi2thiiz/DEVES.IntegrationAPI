using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Configuration;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter;
using DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService
{
    public class ApiLogDataGateWay
    {
        public static void Create(ApiLogEntry apiLogEntry)
        {
            try
            {
                if (string.IsNullOrEmpty(apiLogEntry.ServiceName))
                {
                    apiLogEntry.ServiceName = apiLogEntry.Controller;
                }


                apiLogEntry.BuildDebugLog();

                apiLogEntry.ServiceName = apiLogEntry.ServiceName.Replace("OutputModel", "");
                Console.WriteLine("MachineName:"+ System.Environment.MachineName);
                if (AppConst.IS_SERVER)
                {
                   Console.WriteLine("Execute Log By Sql");
                   ExecuteSql(apiLogEntry);

                }else
                if (System.Environment.MachineName== "DESKTOP-Q30CAGJ")
                {
                   // CallWebService(apiLogEntry);
                    ExecuteSql(apiLogEntry); 
                }
                else
                {
                    Console.WriteLine("CallWebService");
                    //ExecuteSql(apiLogEntry);
                    CallWebService(apiLogEntry);
                }

                //ลบข้อมูลออกจาก Memmory
                if (apiLogEntry.Activity == "provide")
                {
                    TraceDebugLogger.Instance.RemoveLog(apiLogEntry.GlobalTransactionID);
                    GlobalTransactionIdGenerator.Instance.ClearGlobalId(apiLogEntry.GlobalTransactionID);
                }
               

            }
            catch (Exception e)
            {
                Console.WriteLine("CANNOT SAVE: transactionLog"+e.Message );
                Console.WriteLine("CANNOT SAVE: transactionLog" + e.StackTrace);
              

            }

        }

        public static void ExecuteSql(ApiLogEntry apiLogEntry)
        {

            var configurationString = "";
         
                
            if (System.Environment.MachineName == "DESKTOP-Q30CAGJ")
            {
                configurationString = WebConfigurationManager.AppSettings["CRM_CUSTOMAPP_DB_TON"];
            }
            else
            {
                configurationString = WebConfigurationManager.AppSettings["CRM_CUSTOMAPP_DB_SERVER"];
            }
            var logTable = WebConfigurationManager.AppSettings["LOG_TABLE"];
           

            // var configurationString = CrmConfigurationSettings.AppSettings.Get("settings.CRM_CUSTOM_DB");
            using (SqlConnection openCon = new SqlConnection(configurationString))
            {
                
                var saveStaff = $"INSERT into {logTable} ";
                saveStaff += @"(
                                            [Controller],
                                            [ServiceName],
                                            [Activity],
                                            [TransactionID], [GlobalTransactionID],
                                            [Application],
                                            [User],
                                            Machine,
                                            RequestIpAddress,
                                            RequestContentType,
                                            RequestContentBody,
                                            RequestUri,
                                            RequestMethod,
                                            RequestRouteTemplate,RequestRouteData,RequestHeaders,RequestTimestamp,
                                            ResponseContentType,ResponseContentBody,ResponseStatusCode,ResponseHeaders,ResponseTimestamp,
                                            DebugLog,TotalRecord,ErrorLog,StackTrace,Remark,
                                            ContentCode,ContentMessage,ContentTransactionId,ContentTransactionDateTime,
                                            EWICode,EWIMessage,EWIToken,ResponseTime,ResponseTimeTotalMilliseconds)
                                            VALUES (@Controller,
                                                    @ServiceName,
                                                    @Activity,
                                                    @TransactionID,@GlobalTransactionID,
                                                    @Application,@User,@Machine,@RequestIpAddress,
                                                    @RequestContentType,
                                                    @RequestContentBody,@RequestUri,@RequestMethod,
                                                    @RequestRouteTemplate,@RequestRouteData,@RequestHeaders,@RequestTimestamp,
                                                    @ResponseContentType,@ResponseContentBody,@ResponseStatusCode,@ResponseHeaders,
                                                    @ResponseTimestamp,
                                                    @DebugLog,@TotalRecord,@ErrorLog,@StackTrace,
                                                    @Remark,@ContentCode,@ContentMessage,
                                                    @ContentTransactionId,@ContentTransactionDateTime,
                                                    @EWICode,@EWIMessage,@EWIToken,@ResponseTime,@ResponseTimeTotalMilliseconds)";



                using (SqlCommand querySaveStaff = new SqlCommand(saveStaff))
                {


                    querySaveStaff.Connection = openCon;

                    querySaveStaff.Parameters.Add("@Controller", SqlDbType.NVarChar).Value = apiLogEntry.Controller;
                    querySaveStaff.Parameters.Add("@ServiceName", SqlDbType.NVarChar).Value = apiLogEntry.ServiceName;
                    querySaveStaff.Parameters.Add("@Activity", SqlDbType.NVarChar).Value = apiLogEntry.Activity;
                    querySaveStaff.Parameters.Add("@TransactionID", SqlDbType.NVarChar).Value =
                        apiLogEntry.TransactionID;
                    querySaveStaff.Parameters.Add("@GlobalTransactionID", SqlDbType.NVarChar).Value = apiLogEntry?.GlobalTransactionID?? apiLogEntry?.TransactionID??"";




                    querySaveStaff.Parameters.Add("@Application",SqlDbType.NVarChar).Value =""+apiLogEntry.Application;
                    querySaveStaff.Parameters.Add("@User", SqlDbType.NVarChar).Value = "" + apiLogEntry.User;
                    querySaveStaff.Parameters.Add("@Machine", SqlDbType.NVarChar).Value = "" + apiLogEntry.Machine;
                    querySaveStaff.Parameters.Add("@RequestIpAddress", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestIpAddress;
                    querySaveStaff.Parameters.Add("@RequestContentType", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestContentType;
                    querySaveStaff.Parameters.Add("@RequestContentBody", SqlDbType.NText).Value = "" + apiLogEntry.RequestContentBody;
                    querySaveStaff.Parameters.Add("@RequestUri", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestUri;
                    querySaveStaff.Parameters.Add("@RequestMethod", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestMethod;
                    querySaveStaff.Parameters.Add("@RequestRouteTemplate", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestRouteTemplate;
                    querySaveStaff.Parameters.Add("@RequestRouteData", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestRouteData;
                    querySaveStaff.Parameters.Add("@RequestHeaders", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestHeaders;
                   
                    querySaveStaff.Parameters.Add("@RequestTimestamp", SqlDbType.DateTime).Value = apiLogEntry?.ResponseTimestamp??DateTime.Now;
                    querySaveStaff.Parameters.Add("@ResponseContentType", SqlDbType.NVarChar).Value = "" + apiLogEntry.ResponseContentType;
                    querySaveStaff.Parameters.Add("@ResponseContentBody", SqlDbType.NText).Value = "" + apiLogEntry.ResponseContentBody;

                    querySaveStaff.Parameters.Add("@ResponseStatusCode", SqlDbType.NVarChar).Value = "" + apiLogEntry.ResponseStatusCode;
                    querySaveStaff.Parameters.Add("@ResponseHeaders", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestHeaders;
                    querySaveStaff.Parameters.Add("@ResponseTimestamp", SqlDbType.DateTime).Value = apiLogEntry?.ResponseTimestamp??DateTime.Now;


                    querySaveStaff.Parameters.Add("@DebugLog", SqlDbType.NVarChar).Value = "" + apiLogEntry.DebugLog;
                    querySaveStaff.Parameters.Add("@TotalRecord", SqlDbType.Int).Value = "" + apiLogEntry.TotalRecord;
                    querySaveStaff.Parameters.Add("@ErrorLog", SqlDbType.NVarChar).Value = "" + apiLogEntry.ErrorLog;
                    querySaveStaff.Parameters.Add("@StackTrace", SqlDbType.NText).Value = "" + apiLogEntry.StackTrace;
                    
                    querySaveStaff.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = "" + apiLogEntry.Remark;

                    querySaveStaff.Parameters.Add("@ContentCode", SqlDbType.NVarChar).Value = "" + apiLogEntry.ContentCode;
                    querySaveStaff.Parameters.Add("@ContentMessage", SqlDbType.NVarChar).Value = "" + apiLogEntry.ContentMessage;

                    querySaveStaff.Parameters.Add("@ContentTransactionId", SqlDbType.NVarChar).Value = "" + apiLogEntry.ContentTransactionId;
                    querySaveStaff.Parameters.Add("@ContentTransactionDateTime", SqlDbType.NVarChar).Value = "" + apiLogEntry.ContentTransactionDateTime;

                    querySaveStaff.Parameters.Add("@EWICode", SqlDbType.NVarChar).Value = "" + apiLogEntry.EWICode;
                    querySaveStaff.Parameters.Add("@EWIMessage", SqlDbType.NVarChar).Value = "" + apiLogEntry.EWIMessage;
                    querySaveStaff.Parameters.Add("@EWIToken", SqlDbType.NVarChar).Value = "" + apiLogEntry.EWIToken;

                    querySaveStaff.Parameters.Add("@ResponseTime", SqlDbType.NVarChar).Value = "" + apiLogEntry.ResponseTime;
                    querySaveStaff.Parameters.Add("@ResponseTimeTotalMilliseconds", SqlDbType.Float).Value =  apiLogEntry.ResponseTimeTotalMilliseconds;

                    




                    openCon.Open();
                    var result = querySaveStaff.ExecuteReader();
                    Console.WriteLine("SAVE: transactionLog");
                }
            }
        }

        public static void CallWebService(ApiLogEntry apiLogEntry)
        {

            CultureInfo UsaCulture = new CultureInfo("en-US");
            var endpoint = WebConfigurationManager.AppSettings["API_ENDPOINT_INTERNAL_SERVICE"];

            Console.WriteLine(endpoint + "/StoreService/ext");
            var client = new RESTClient(endpoint+"/StoreService/ext");
            var req = new DbRequest {StoreName = "sp_Insert_TransactionLog"};
            req.AddParam("TransactionID",""+ apiLogEntry.TransactionID);
            
            req.AddParam("Application", "" + apiLogEntry.Application );
            req.AddParam("Controller", ""+apiLogEntry.Controller);
            req.AddParam("ServiceName", ""+ apiLogEntry.ServiceName);
            req.AddParam("Activity", ""+ apiLogEntry.Activity);
            req.AddParam("User", ""+ apiLogEntry.User);
            req.AddParam("Machine", ""+ apiLogEntry.Machine);
            req.AddParam("RequestIpAddress", ""+ apiLogEntry.RequestIpAddress);
            req.AddParam("RequestContentType", "" + apiLogEntry.RequestContentType);
            req.AddParam("RequestContentBody", "" + apiLogEntry.RequestContentBody);
            req.AddParam("RequestUri", "" + apiLogEntry.RequestUri);
            req.AddParam("RequestMethod", "" + apiLogEntry.RequestMethod);
            req.AddParam("RequestRouteTemplate", ""+ apiLogEntry.RequestRouteTemplate);
            req.AddParam("RequestRouteData", "" + apiLogEntry.RequestRouteData);
            req.AddParam("RequestHeaders", "" + apiLogEntry.RequestHeaders);
            req.AddParam("RequestTimestamp", ""+apiLogEntry.ResponseTimestamp?.ToString("yyyy-MM-dd HH:mm:ss",UsaCulture), "DateTime");
            req.AddParam("ResponseContentType", "" + apiLogEntry.ResponseContentType);
            req.AddParam("ResponseContentBody", "" + apiLogEntry.ResponseContentBody);
            req.AddParam("ResponseStatusCode", "" + apiLogEntry.ResponseStatusCode);
            req.AddParam("ResponseHeaders", "" + apiLogEntry.RequestHeaders);
            req.AddParam("ResponseTimestamp", ""+ apiLogEntry.ResponseTimestamp?.ToString("yyyy-MM-dd HH:mm:ss",UsaCulture), "DateTime");

            var result = client.Execute(req);
            var dbResult = new DbResult();
           // Console.WriteLine(dbResult.ToJson());
        }
    }
}

