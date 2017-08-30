using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Controllers;
using DEVES.IntegrationAPI.WebApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.QuerySQLAdapter
{
    public class QuerySQLInternal : QuerySQLStretagy
    {
        public void GetQuery(string databaseName, string sqlCommand)
        {

            QuerySQLController sql = new QuerySQLController();

            string content = "{\"databaseName\": " + "\"" + databaseName + "\"" + "," +
                "\"sqlCommand\": " + "\"" + sqlCommand + "\"" + "}"
                ;

            buzQuerySQL query = new buzQuerySQL();
            BaseDataModel output = query.Execute(content);

            /*
            string dbName = "";
            if (databaseName.Equals("CRMQA_MSCRM")) // STORED_QA
            {
                dbName = "CRMDB";
            }
            else if (databaseName.Equals("CRM_CUSTOM_APP")) // LOG_QA
            {
                dbName = "CRM_CUSTOMAPP_DB";
            }
            else if (databaseName.Equals("CRM_MSCRM")) // STORED_PRODUCTION
            {
                dbName = "CRMDB_PRO";
            }
            else if (databaseName.Equals("CRM_CUSTOM_APP_PRO")) // LOG_QA
            {
                dbName = "CRM_CUSTOMAPP_DB_PRO";
            }
            else
            {
                /* 
                output.databaseName = databaseName;
                output.sqlCommand = sqlCommand;
                output.message = "Fail: can't find db from databaseName variable";

                return Request.CreateResponse<QuerySQLOutputModel>(output);
                
            }

            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(sqlCommand, System.Configuration.ConfigurationManager.AppSettings[dbName].ToString());
            da.Fill(dt);

            */

        }
    }
}