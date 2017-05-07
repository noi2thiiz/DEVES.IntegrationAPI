using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dvsSearchCase
{
    /// <summary>
    /// Summary description for QueryInfo
    /// </summary>
    public class QueryInfo
    {
        const string _TOP_QUERY_Key = "TOP_QUERY";

        public System.Data.DataTable QueryInfo_Contact(string Type, string Value)
        {
            string strSql = @"SELECT  TOP " + System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key].ToString() + @"
                                      c.IncidentId ,
                                      c.TicketNumber AS [CaseNo] ,                                     
                                      c.CustomerIdYomiName [CustomerName] ,
                                      c.pfc_claim_noti_number AS [ClaimNotiNo],
                                      c.pfc_policy_number AS [PolicyNo],
                                      c.pfc_current_reg_num AS [PlateNo],
                                      c.pfc_current_reg_num_prov AS [Province],
                                      c.pfc_claim_number AS [CLaimNo],
                                      c.pfc_driver_nameName AS [DriverName],
                                      c.pfc_accident_on AS [AccidentOn],
                                      c.pfc_claim_loss_date AS [LostDate]
                              FROM    IncidentBase c WITH ( NOLOCK )
                                      WHERE   c.StateCode = '0' AND {0}
                              ORDER BY [CustomerIdYomiName] ";
            switch (Type)
            {
                case "CASE_NO":
                    strSql = string.Format(strSql, @"( c.TicketNumber LIKE N'%" + Value + "%' )");
                    break;
                case "CUSTOMER_NAME":
                    strSql = string.Format(strSql, @"( c.CustomerIdYomiName LIKE N'%" + Value + "%' )");
                    break;
                case "POLICY_NO":
                    strSql = string.Format(strSql, @"( c.pfc_policy_number LIKE N'%" + Value + "%' )");
                    break;
                case "PLATE_NO":
                    strSql = string.Format(strSql, @"( c.pfc_current_reg_num LIKE N'%" + Value + "%' )");
                    break;
                case "PROVINCE":
                    strSql = string.Format(strSql, @"( c.pfc_current_reg_num_prov LIKE N'%" + Value + "%' )");
                    break;
                case "CLAIM_NO":
                    strSql = string.Format(strSql, @"( c.pfc_claim_number LIKE N'%" + Value + "%' )");
                    break;
                case "DRIVER_NAME":
                    strSql = string.Format(strSql, @"( c.pfc_driver_nameName LIKE N'%" + Value + "%' )");
                    break;

            }
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
            //System.Data.DataTable dtCloned = dt.Clone();
            da.Fill(dt);
            return dt;


        }

        public System.Data.DataTable QueryInfo_ContactSort(string Type, string Value)
        {
            string strSql = @"SELECT  TOP " + System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key].ToString() + @"
                                      c.IncidentId ,
                                      c.TicketNumber AS [CaseNo] ,                                     
                                      c.CustomerIdYomiName [CustomerName] ,
                                      c.pfc_claim_noti_number AS [ClaimNotiNo],
                                      c.pfc_policy_number AS [PolicyNo],
                                      c.pfc_current_reg_num AS [PlateNo],
                                      c.pfc_current_reg_num_prov AS [Province],
                                      c.pfc_claim_number AS [CLaimNo],
                                      c.pfc_driver_nameName AS [DriverName],
                                      c.pfc_claim_loss_date AS [LostDate]
                              FROM    IncidentBase c WITH ( NOLOCK )
                                      WHERE   c.StateCode = '0' AND {0}
                              ORDER BY [pfc_claim_loss_date] ";
            switch (Type)
            {
                case "CASE_NO":
                    strSql = string.Format(strSql, @"( c.TicketNumber LIKE N'%" + Value + "%' )");
                    break;
                case "CUSTOMER_NAME":
                    strSql = string.Format(strSql, @"( c.CustomerIdYomiName LIKE N'%" + Value + "%' )");
                    break;
                case "POLICY_NO":
                    strSql = string.Format(strSql, @"( c.pfc_policy_number LIKE N'%" + Value + "%' )");
                    break;
                case "PLATE_NO":
                    strSql = string.Format(strSql, @"( c.pfc_current_reg_num LIKE N'%" + Value + "%' )");
                    break;
                case "PROVINCE":
                    strSql = string.Format(strSql, @"( c.pfc_current_reg_num_prov LIKE N'%" + Value + "%' )");
                    break;
                case "CLAIM_NO":
                    strSql = string.Format(strSql, @"( c.pfc_claim_number LIKE N'%" + Value + "%' )");
                    break;
                case "DRIVER_NAME":
                    strSql = string.Format(strSql, @"( c.pfc_driver_nameNAme LIKE N'%" + Value + "%' )");
                    break;

            }
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
            //System.Data.DataTable dtCloned = dt.Clone();
            da.Fill(dt);
            return dt;


        }

    }
}
