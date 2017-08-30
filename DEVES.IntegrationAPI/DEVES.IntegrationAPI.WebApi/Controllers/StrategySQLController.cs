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

        public object Get([FromBody]object value)
        {
            return GetLocalIPAddress();
        }

        public object Post([FromBody]object value)
        {
            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<QuerySQLInputModel>(contentText);

            string devesIP = "192.168.78";

            if(GetLocalIPAddress().Contains(devesIP))
            {
                string a = "abc";
                // 
            }

            if (GetLocalIPAddress().Equals(devesIP))
            {
                QuerySQLInternal sql = new QuerySQLInternal();
                sql.GetQuery(contentModel.databaseName, contentModel.sqlCommand);

                return "INTERNAL NETWORK (INTERNAL METHOD)";
            }
            else
            {
                QuerySQLOnline sql = new QuerySQLOnline();
                sql.GetQuery(contentModel.databaseName, contentModel.sqlCommand);

                return "ONLINE NETWORK (ONLINE METHOD)";
            }
        }
    }
}