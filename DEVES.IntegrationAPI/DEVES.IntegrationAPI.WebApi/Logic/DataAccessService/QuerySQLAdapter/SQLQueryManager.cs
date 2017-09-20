using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model.QuerySQL;
using System.Net;
using System.Web.Configuration;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.QuerySQLAdapter
{
    public class QuerySqlService
    {
        private QuerySQLStretagy sql;

        #region Singleton
        private static QuerySqlService _instance;
        private QuerySqlService() {
            // do nothing
        }
        public static QuerySqlService Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new QuerySqlService();
                _instance.Start();
                return _instance;
            }
        }
        #endregion

        public void Start()
        {
            if (sql == null)
            {

                string userIP = GetLocalIPAddress();

                // string internalClientIP = "192.168.78";
                string internalClientIP = WebConfigurationManager.AppSettings["IPclientDeves"];
                string serverQAIP = WebConfigurationManager.AppSettings["IPserverQA"];
                string serverProd1IP = WebConfigurationManager.AppSettings["IPserverProduction1"];
                string serverProd2IP = WebConfigurationManager.AppSettings["IPserverProduction2"];


                if (userIP.Contains(internalClientIP) || userIP.Equals(serverQAIP) || userIP.Equals(serverProd1IP) || userIP.Equals(serverProd2IP))
                {
                    sql = new QuerySQLInternal();
                    Console.WriteLine("QuerySQLInternal");
                }
                else
                {
                    sql = new QuerySQLOnline();
                    Console.WriteLine("QuerySQLOnline");
                }

                /*
                if (userIP.Contains(internalClientIP))
                {
                    sql = new QuerySQLInternal();
                }
                else
                {
                    sql = new QuerySQLOnline();
                }
                */
            }
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public QuerySQLOutputModel GetQuery(string databaseName, string sqlCommand)
        {
            if (sql == null)
            {
                throw new Exception("QuerySQLStretagy Not Found");

            }
            QuerySQLOutputModel output = new QuerySQLOutputModel();
            output = sql.GetQuery(databaseName, sqlCommand);
            return output;
        }
    }
}