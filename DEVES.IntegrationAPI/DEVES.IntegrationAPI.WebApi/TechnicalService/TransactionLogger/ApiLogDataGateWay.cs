using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Configuration;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter;
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
              

                apiLogEntry.ServiceName = apiLogEntry.ServiceName.Replace("OutputModel", "");
                Console.WriteLine("MachineName:"+ System.Environment.MachineName);
                if (System.Environment.MachineName == AppConst.QA_SERVER_NAME 
                    || System.Environment.MachineName == AppConst.PRO1_SERVER_NAME 
                    || System.Environment.MachineName == AppConst.PRO2_SERVER_NAME)
                {
                   Console.WriteLine("ExecuteSql");
                   ExecuteSql(apiLogEntry);

                }
                if (System.Environment.MachineName== "DESKTOP-Q30CAGJ")
                {
                    ExecuteSql(apiLogEntry); 
                }
                else
                {
                    Console.WriteLine("CallWebService");
                    //ExecuteSql(apiLogEntry);
                    CallWebService(apiLogEntry);
                }



            }
            catch (Exception e)
            {
                Console.WriteLine("CANNOT SAVE: transactionLog"+e.Message );

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


            // var configurationString = CrmConfigurationSettings.AppSettings.Get("settings.CRM_CUSTOM_DB");
            using (SqlConnection openCon = new SqlConnection(configurationString))
            {
                string saveStaff = @"INSERT into
                                            transactionLog (
                                            [Controller],
                                            [ServiceName],
                                            [Activity],
                                            [TransactionID],
                                            [Application],
                                            [User],
                                            Machine,
                                            RequestIpAddress,
                                            RequestContentType,
                                            RequestContentBody,
                                            RequestUri,
                                            RequestMethod,
                                            RequestRouteTemplate,RequestRouteData,RequestHeaders,RequestTimestamp,
                                            ResponseContentType,ResponseContentBody,ResponseStatusCode,ResponseHeaders,ResponseTimestamp)
                                            VALUES (@Controller,
                                                    @ServiceName,
                                                    @Activity,
                                                    @TransactionID,@Application,@User,@Machine,@RequestIpAddress,
                                                    @RequestContentType,
                                                    @RequestContentBody,@RequestUri,@RequestMethod,
                                                    @RequestRouteTemplate,@RequestRouteData,@RequestHeaders,@RequestTimestamp,
                                                    @ResponseContentType,@ResponseContentBody,@ResponseStatusCode,@ResponseHeaders,
                                                    @ResponseTimestamp)";



                using (SqlCommand querySaveStaff = new SqlCommand(saveStaff))
                {


                    querySaveStaff.Connection = openCon;

                    querySaveStaff.Parameters.Add("@Controller", SqlDbType.NVarChar).Value = apiLogEntry.Controller;
                    querySaveStaff.Parameters.Add("@ServiceName", SqlDbType.NVarChar).Value = apiLogEntry.ServiceName;
                    querySaveStaff.Parameters.Add("@Activity", SqlDbType.NVarChar).Value = apiLogEntry.Activity;
                    querySaveStaff.Parameters.Add("@TransactionID", SqlDbType.NVarChar).Value = apiLogEntry.TransactionID;




                    querySaveStaff.Parameters.Add("@Application",SqlDbType.NVarChar).Value =""+apiLogEntry.Application;
                    querySaveStaff.Parameters.Add("@User", SqlDbType.NVarChar).Value = "" + apiLogEntry.User;
                    querySaveStaff.Parameters.Add("@Machine", SqlDbType.NVarChar).Value = "" + apiLogEntry.Machine;
                    querySaveStaff.Parameters.Add("@RequestIpAddress", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestIpAddress;
                    querySaveStaff.Parameters.Add("@RequestContentType", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestContentType;
                    querySaveStaff.Parameters.Add("@RequestContentBody", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestContentBody;
                    querySaveStaff.Parameters.Add("@RequestUri", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestUri;
                    querySaveStaff.Parameters.Add("@RequestMethod", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestMethod;
                    querySaveStaff.Parameters.Add("@RequestRouteTemplate", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestRouteTemplate;
                    querySaveStaff.Parameters.Add("@RequestRouteData", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestRouteData;
                    querySaveStaff.Parameters.Add("@RequestHeaders", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestHeaders;

                    querySaveStaff.Parameters.Add("@RequestTimestamp", SqlDbType.DateTime).Value = apiLogEntry.ResponseTimestamp;
                    querySaveStaff.Parameters.Add("@ResponseContentType", SqlDbType.NVarChar).Value = "" + apiLogEntry.ResponseContentType;
                    querySaveStaff.Parameters.Add("@ResponseContentBody", SqlDbType.NVarChar).Value = "" + apiLogEntry.ResponseContentBody;

                    querySaveStaff.Parameters.Add("@ResponseStatusCode", SqlDbType.NVarChar).Value = "" + apiLogEntry.ResponseStatusCode;
                    querySaveStaff.Parameters.Add("@ResponseHeaders", SqlDbType.NVarChar).Value = "" + apiLogEntry.RequestHeaders;
                    querySaveStaff.Parameters.Add("@ResponseTimestamp", SqlDbType.DateTime).Value = apiLogEntry.ResponseTimestamp;






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

