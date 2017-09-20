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

        public object Post([FromBody]object value)
        {
            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<QuerySQLInputModel>(contentText);

            QuerySQLOutputModel output = new QuerySQLOutputModel();
            QuerySqlService sql = QuerySqlService.Instance;
            output = sql.GetQuery(contentModel.databaseName, contentModel.sqlCommand);

            return output;
        }

    }
}