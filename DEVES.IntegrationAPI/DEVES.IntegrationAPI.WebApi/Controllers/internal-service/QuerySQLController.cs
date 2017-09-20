using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.QuerySQL;
using DEVES.IntegrationAPI.WebApi.DataAccessService.QuerySQLAdapter;
using DEVES.IntegrationAPI.WebApi.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class QuerySQLController : ApiController
    {

        public object Post([FromBody]object value)
        {
            /*
            // Preparation Output
            QuerySQLOutputModel output = new QuerySQLOutputModel();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<QuerySQLInputModel>(contentText);

            string dbName = "";
            if(contentModel.databaseName.Equals("CRMQA_MSCRM")) // STORED_QA
            {
                dbName = "CRMDB";
            }
            else if(contentModel.databaseName.Equals("CRM_CUSTOM_APP")) // LOG_QA
            {
                dbName = "CRM_CUSTOMAPP_DB";
            }
            else if (contentModel.databaseName.Equals("CRM_MSCRM")) // STORED_PRODUCTION
            {
                dbName = "CRMDB_PRO";
            }
            else if (contentModel.databaseName.Equals("CRM_CUSTOM_APP_PRO")) // LOG_QA
            {
                dbName = "CRM_CUSTOMAPP_DB_PRO";
            }
            else
            {
                output.databaseName = contentModel.databaseName;
                output.sqlCommand = contentModel.sqlCommand;
                output.message = "Fail: can't find db from databaseName variable";

                return Request.CreateResponse<QuerySQLOutputModel>(output);
            }

            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(contentModel.sqlCommand, System.Configuration.ConfigurationManager.AppSettings[dbName].ToString());
            da.Fill(dt);
            
            // return output if program work propery
            output.databaseName = contentModel.databaseName;
            output.sqlCommand = contentModel.sqlCommand;
            output.message = "Pass";
            output.dt = dt;
            */

            buzQuerySQL query = new buzQuerySQL();
            BaseDataModel output = query.Execute(value);

            return Request.CreateResponse<BaseDataModel>(output);

        }
    }

}