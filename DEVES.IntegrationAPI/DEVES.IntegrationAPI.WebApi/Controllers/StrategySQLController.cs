using DEVES.IntegrationAPI.Model.QuerySQL;
using DEVES.IntegrationAPI.WebApi.DataAccessService.QuerySQLAdapter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class StrategySQLController : ApiController
    {

        private static string tpyeStrategy;

        public static string GetLocalIPAddress()
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

        public object Get()
        {
            return GetLocalIPAddress();
        }

        public object Post([FromBody]object value)
        {
            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<QuerySQLInputModel>(contentText);

            // string devesIP = "192.168.78.*";
            string devesIP = "192.168.78";

            if (String.IsNullOrEmpty(tpyeStrategy)) 
            {
                if(GetLocalIPAddress().Contains(devesIP))
                {
                    tpyeStrategy = "INTERNAL";
                }
                else
                {
                    tpyeStrategy = "ONLINE";
                }
            }


            QuerySQLOutputModel output = new QuerySQLOutputModel();

            if (tpyeStrategy.Equals("INTERNAL"))
            {
                QuerySQLInternal sql = new QuerySQLInternal();
                output = sql.GetQuery(contentModel.databaseName, contentModel.sqlCommand);

                return output;
            }
            else if(tpyeStrategy.Equals("ONLINE"))
            {
                QuerySQLOnline sql = new QuerySQLOnline();
                output = sql.GetQuery(contentModel.databaseName, contentModel.sqlCommand);

                return output;
            }
            else
            {
                return "NOT IN BOTH CONDITION";
            }
        }

    }
}