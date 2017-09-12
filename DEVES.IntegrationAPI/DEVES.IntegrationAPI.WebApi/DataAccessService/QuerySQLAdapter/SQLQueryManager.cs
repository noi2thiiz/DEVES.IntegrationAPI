using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model.QuerySQL;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.QuerySQLAdapter
{
    public class QuerySqlService
    {
        public QuerySQLOutputModel GetQuery(string databaseName, string sqlCommand)
        {
            var tpyeStrategy = "INTERNAL";//
            QuerySQLOutputModel output = new QuerySQLOutputModel();
            if (tpyeStrategy.Equals("INTERNAL"))
            {
               // QuerySQLInternal sql = new QuerySQLInternal();
               // output = sql.GetQuery(contentModel.databaseName, contentModel.sqlCommand);
               //return output;
            }
            else if (tpyeStrategy.Equals("ONLINE"))
            {
               //  QuerySQLOnline sql = new QuerySQLOnline();
               // output = sql.GetQuery(contentModel.databaseName, contentModel.sqlCommand);
               // return output;
            }
            else
            {
                throw  new Exception("NOT IN BOTH CONDITION");
               // return "NOT IN BOTH CONDITION";
            }

            return output;
        }
    }
}