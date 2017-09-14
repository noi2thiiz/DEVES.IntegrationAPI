using DEVES.IntegrationAPI.Model.QuerySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.QuerySQLAdapter
{
	public interface QuerySQLStretagy
	{
        QuerySQLOutputModel GetQuery(string databaseName, string sqlCommand);
    }
}