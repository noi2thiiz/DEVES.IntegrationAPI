using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService
{
    public class ApiLogDataGateWay
    {
        public static void Create(ApiLogEntry apiLogEntry)
        {
            try
            {
                var configurationString = WebConfigurationManager.AppSettings["CRM_CUSTOMAPP_DB"];
               // var configurationString = CrmConfigurationSettings.AppSettings.Get("settings.CRM_CUSTOM_DB");
                using(SqlConnection openCon=new SqlConnection(configurationString))
                {
                    string saveStaff = @"INSERT into
                                            transactionLog (
                                            [Controller],
                                            Activity,
                                            TransactionID,
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
                                                    @TransactionID,@Activity,@User,@Machine,@RequestIpAddress,
                                                    @RequestContentType,
                                                    @RequestContentBody,@RequestUri,@RequestMethod,
                                                    @RequestRouteTemplate,@RequestRouteData,@RequestHeaders,@RequestTimestamp,
                                                    @ResponseContentType,@ResponseContentBody,@ResponseStatusCode,@ResponseHeaders,
                                                    @ResponseTimestamp)";



                    using(SqlCommand querySaveStaff = new SqlCommand(saveStaff))
                    {


                        querySaveStaff.Connection=openCon;

                        querySaveStaff.Parameters.Add("@Controller", SqlDbType.NVarChar,100).Value =apiLogEntry.Controller;

                        querySaveStaff.Parameters.Add("@TransactionID",SqlDbType.NVarChar,100).Value=apiLogEntry.TransactionID;
                        querySaveStaff.Parameters.Add("@Activity",SqlDbType.NVarChar,100).Value=apiLogEntry.Activity;



                      //  querySaveStaff.Parameters.Add("@Application",SqlDbType.NVarChar).Value =""+apiLogEntry.Application;
                        querySaveStaff.Parameters.Add("@User", SqlDbType.NVarChar).Value =""+apiLogEntry.User;
                        querySaveStaff.Parameters.Add("@Machine", SqlDbType.NVarChar).Value =""+apiLogEntry.Machine;
                        querySaveStaff.Parameters.Add("@RequestIpAddress", SqlDbType.NVarChar).Value =""+apiLogEntry.RequestIpAddress;
                        querySaveStaff.Parameters.Add("@RequestContentType", SqlDbType.NVarChar).Value =""+apiLogEntry.RequestContentType;
                        querySaveStaff.Parameters.Add("@RequestContentBody", SqlDbType.NVarChar).Value =""+apiLogEntry.RequestContentBody;
                        querySaveStaff.Parameters.Add("@RequestUri", SqlDbType.NVarChar).Value =""+apiLogEntry.RequestUri;
                        querySaveStaff.Parameters.Add("@RequestMethod", SqlDbType.NVarChar).Value =""+apiLogEntry.RequestMethod;
                        querySaveStaff.Parameters.Add("@RequestRouteTemplate", SqlDbType.NVarChar).Value =""+apiLogEntry.RequestRouteTemplate;
                        querySaveStaff.Parameters.Add("@RequestRouteData", SqlDbType.NVarChar).Value =""+apiLogEntry.RequestRouteData;
                        querySaveStaff.Parameters.Add("@RequestHeaders", SqlDbType.NVarChar).Value =""+apiLogEntry.RequestHeaders;

                        querySaveStaff.Parameters.Add("@RequestTimestamp", SqlDbType.DateTime).Value =apiLogEntry.ResponseTimestamp;
                        querySaveStaff.Parameters.Add("@ResponseContentType", SqlDbType.NVarChar).Value =""+apiLogEntry.ResponseContentType;
                        querySaveStaff.Parameters.Add("@ResponseContentBody", SqlDbType.NVarChar).Value =""+apiLogEntry.ResponseContentBody;

                        querySaveStaff.Parameters.Add("@ResponseStatusCode", SqlDbType.NVarChar).Value =""+apiLogEntry.ResponseStatusCode;
                        querySaveStaff.Parameters.Add("@ResponseHeaders", SqlDbType.NVarChar).Value =""+apiLogEntry.RequestHeaders;
                        querySaveStaff.Parameters.Add("@ResponseTimestamp", SqlDbType.DateTime).Value =apiLogEntry.ResponseTimestamp;



                        Console.WriteLine("SAVE: transactionLog" );


                        openCon.Open();
                        var result = querySaveStaff.ExecuteReader();

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                
            }

        }
    }
}