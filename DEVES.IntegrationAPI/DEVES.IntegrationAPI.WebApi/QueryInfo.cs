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

        /*
        public static string SQL_RequestSurveyor = @"DECLARE @TicketNumber AS NVARCHAR(20) = 'CAS201702-00003';
                                                    DECLARE @CurrentUserCode AS NVARCHAR(20) = 'G001';
                                                    EXEC [dbo].[sp_CustomApp_RequestSurveyor_Incident] @TicketNumber, @CurrentUserCode"
        */
        public static string SQL_RequestSurveyor = @"DECLARE @IncidentId AS UNIQUEIDENTIFIER = '{0}';
                                                     DECLARE @CurrentUserId AS UNIQUEIDENTIFIER = '{1}';
                                                     EXEC [dbo].[sp_CustomApp_RequestSurveyor_Incident] @IncidentId, @CurrentUserId;";

        public static string SQL_InquiryPolicyMotorList = @"DECLARE @policyNo AS NVARCHAR(100) = '{0}'
                                                    DECLARE @chassisNo AS NVARCHAR(100) = '{1}'
                                                    DECLARE @carRegisNo NVARCHAR(100) = '{2}'
                                                    DECLARE @carRegisProve NVARCHAR(100) = '{3}'
                                                    EXEC [dbo].[sp_API_InquiryPolicyMotorList] @policyNo,@chassisNo,@carRegisNo,@carRegisProve;";

        // DataTable
        public static string SQL_searchPersonal = @"DECLARE @fullname nvarchar(50) = '{0}'
                                                    DECLARE @czid nvarchar(13) = '{1}'
                                                    DECLARE @phoneno nvarchar(10) = '{2}'
                                                    DECLARE @crmid nvarchar(20) = '{3}'
                                                    DECLARE @clsid nvarchar(20) = '{4}'
                                                    DECLARE @email nvarchar(50) = '{5}'
                                                    EXEC[dbo].[sp_Query_APIpersonal] @fullname,@czid,@phoneno,@crmid,@clsid,@email";


        public System.Data.DataTable Queryinfo_CallerId(string ticketNo, string uniqueID)
        {
            string strSql = string.Format(SQL_01, ticketNo);
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDB"].ToString());
            da.Fill(dt);
            return dt;
        }
        public System.Data.DataTable Queryinfo_searchPerson(string value)
        {
            string strSql = string.Format(SQL_searchPersonal, value.Split('|')[0], value.Split('|')[1], value.Split('|')[2], value.Split('|')[4], value.Split('|')[3], value.Split('|')[5]);
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDB"].ToString());
            da.Fill(dt);
            return dt;
        }
        /// <summary>
        /// Query data in [dbo].[sp_CustomApp_RequestSurveyor_Incident]
        /// Input = TicketNumber and CurrentUserCode (G001)
        /// </summary>
        /// <param name="ticketNo"></param>
        /// <param name="uniqueID"></param>
        /// <returns>
        /// DataTable of @TicketNumber= 'CAS201702-00003' and @CurrentUserCode = 'G001'
        /// </returns>
        public System.Data.DataTable Queryinfo_RequestSurveyor(string IncidentId, string CurrentUserId)
        {
            string strSql = string.Format(SQL_RequestSurveyor, IncidentId, CurrentUserId);
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDB"].ToString());
            da.Fill(dt);
            return dt;
        }

        public System.Data.DataTable Queryinfo_InquiryPolicyMotorList(string policyNo, string carChassisNo, string carRegisNo, string carRegisProv)
        {
            string strSql = string.Format(SQL_InquiryPolicyMotorList, policyNo, carChassisNo, carRegisNo, carRegisProv);
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDB"].ToString());
            da.Fill(dt);
            return dt;
        }

    }
}