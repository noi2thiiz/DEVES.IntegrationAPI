using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.QuerySQLAdapter
{
    abstract class QuerySQLStretagy
    {
        public abstract void GetQuery(string databaseName, string sqlCommand);
    }
}