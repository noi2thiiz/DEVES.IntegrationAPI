﻿using System;
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
        public string SQL_RequestSurveyor = @"DECLARE @IncidentId AS UNIQUEIDENTIFIER = '{0}'; DECLARE @CurrentUserId AS UNIQUEIDENTIFIER = '{1}'; EXEC [dbo].[sp_CustomApp_RequestSurveyor_Incident] @IncidentId, @CurrentUserId;";

        public string SQL_ClaimRegistration = @"DECLARE @IncidentId AS UNIQUEIDENTIFIER = '{0}'; DECLARE @CurrentUserId AS UNIQUEIDENTIFIER = '{1}'; EXEC [dbo].[sp_CustomApp_RegClaimInfo_Incident] @IncidentId, @CurrentUserId;";

        public static string SQL_InquiryPolicyMotorList = @"DECLARE @policyNo AS NVARCHAR(100) = '{0}'
                                                    DECLARE @chassisNo AS NVARCHAR(100) = '{1}'
                                                    DECLARE @carRegisNo NVARCHAR(100) = '{2}'
                                                    DECLARE @carRegisProve NVARCHAR(100) = '{3}'
                                                    EXEC [dbo].[sp_API_InquiryPolicyMotorList] @policyNo,@chassisNo,@carRegisNo,@carRegisProve;";

        // DataTable
        public System.Data.DataTable Queryinfo_CallerId(string ticketNo, string uniqueID)
        {
            string strSql = string.Format(SQL_01, ticketNo);
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