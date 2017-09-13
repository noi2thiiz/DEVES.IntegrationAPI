using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model.QuerySQL;
using System.Net;

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
                return _instance;
            }
        }
        #endregion

        public void Start()
        {
            // string devesIP = "192.168.78.*";
            string devesIP = "192.168.78";

            if (GetLocalIPAddress().Contains(devesIP))
            {
                sql = new QuerySQLInternal();
            }
            else
            {
                sql = new QuerySQLOnline();
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
            QuerySQLOutputModel output = new QuerySQLOutputModel();
            output = sql.GetQuery(databaseName, sqlCommand);
            return output;
        }
    }
}