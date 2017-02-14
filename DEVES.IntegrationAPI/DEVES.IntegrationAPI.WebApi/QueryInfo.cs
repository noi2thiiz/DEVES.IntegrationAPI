using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi
{
    public class QueryInfo
    {
        public static string SQL_01 = @"DECLARE @ticketNo AS NVARCHAR(20) = '{0}';
                          
                          EXEC[dbo].[sp_CustomApp_RegClaimInfo_Incident] null, @ticketNo;";
        // DataTable
        public System.Data.DataTable Queryinfo_CallerId(string ticketNo, string uniqueID)
        {
            string strSql = string.Format(SQL_01, ticketNo);
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDB"].ToString());
            da.Fill(dt);
            return dt;
        }

        // DataSet
    }
}