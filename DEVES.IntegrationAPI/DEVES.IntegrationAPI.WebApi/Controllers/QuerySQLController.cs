using DEVES.IntegrationAPI.Model.QuerySQL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class QuerySQLController : ApiController
    {
        public object Post([FromBody]object value)
        {
            // Preparation Output
            QuerySQLOutputModel output = new QuerySQLOutputModel();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<QuerySQLInputModel>(contentText);

            string dbName = "";
            if(contentModel.databaseName.Equals("CRMQA_MSCRM")) 
            {
                dbName = "CRMDB";
            }
            else if(contentModel.databaseName.Equals("CRM_CUSTOMAPP"))
            {
                dbName = "CRM_CUSTOMAPP_DB_NORTH";
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

            return Request.CreateResponse<QuerySQLOutputModel>(output);
        }
    }

}