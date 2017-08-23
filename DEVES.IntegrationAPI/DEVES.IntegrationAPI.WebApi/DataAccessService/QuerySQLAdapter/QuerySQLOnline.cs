using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.QuerySQLAdapter
{
    class QuerySQLOnline : QuerySQLStretagy
    {
        public override void GetQuery(string databaseName, string sqlCommand)
        {
            throw new NotImplementedException();
        }
    }
}