using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
/// <summary>
/// Summary description for Class1
/// </summary>
namespace devescustomGenerateCallerId
{
    public class QueryInfo
    {
        #region [sql execute]
        public System.Data.DataTable Queryinfo_CallerId(string guid, string caseid, string ticketno)
        {
            string strSql = @"DECLARE @ticketNo AS NVARCHAR(20) = '" + ticketno + @"'
                      DECLARE @uniqueID AS UNIQUEIDENTIFIER = '" + guid + @"'
                      DECLARE @requestBy AS NVARCHAR(250) = '" + caseid + @"'
                      DECLARE @resultCode AS NVARCHAR(10)
                      DECLARE @resultDesc AS NVARCHAR(1000)
                      select @uniqueID = ISNULL(IncidentId, @uniqueID)
                      from dbo.IncidentBase a with(nolock)
                      where a.TicketNumber = @ticketNo
                      EXEC[dbo].[sp_ReqMotorClaimNotiNo] @uniqueID, @requestBy, @resultCode OUTPUT, @resultDesc OUTPUT
                      select @ticketNo as [@ticketNo],@uniqueID as [@uniqueID],@requestBy as [@requestBy],@resultCode as [@resultCode], @resultDesc as [@resultDesc]";
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
            da.Fill(dt);
            return dt;
        }
        #endregion

        #region [c# execute]
        //public void Queryinfo_CallerId(string guid, string caseid, string ticketno)
        //{
        //    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString()))
        //    {
        //        using(SqlCommand cmd = new SqlCommand("sp_ReqMotorClaimNotiNo", conn))
        //        {
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //        }
        //    }
        //}
        #endregion
    }
}