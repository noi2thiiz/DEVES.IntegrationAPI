using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devesSearchPolicy
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
                                      c.pfc_policyId , 
                                      c.pfc_policy_number AS [PolicyNo],
                                      c.pfc_cus_fullname AS [CustomerName],
                                      d.pfc_reg_num As [PlateNo],
									  d.pfc_reg_num_prov AS [Province],
                                      d.pfc_chassis_num AS [ChassisNo],
                                      d.pfc_bar_code AS [BARCODE],
									  '-' AS [InsuranceCard],
									  '-' AS [Driver1],
									  '-' AS [Driver2],
                                      CONVERT(CHAR(11), c.pfc_effective_date, 106) AS [EffectiveDate],
                                      CONVERT(CHAR(11), c.pfc_expiry_date, 106) AS [ExpireDate],
                                      d.pfc_policy_additionalid AS [PolicyAddId],
                                      d.pfc_policy_additional_name  AS [PolicyAddName]                           
                              FROM    pfc_policyBase c WITH ( NOLOCK )
                                      INNER JOIN pfc_policy_additionalBase d
                                      ON c.pfc_policyId = d.pfc_policyId
                                      WHERE   c.StateCode = '0' AND {0}
                              ORDER BY [pfc_cus_fullname] ";

            switch (Type)
            {
                case "POLICY_NO":
                    strSql = string.Format(strSql, @"( c.pfc_chdr_num LIKE N'%" + Value + "%' )");
                    break;
                case "CUSTOMER_NAME":
                    strSql = string.Format(strSql, @"( c.pfc_cus_fullname LIKE N'%" + Value + "%' )");
                    break;
                case "PLATE_NO":
                    strSql = string.Format(strSql, @"( d.pfc_reg_num LIKE N'%" + Value + "%' )");
                    break;
                case "PROVINCE":
                    strSql = string.Format(strSql, @"( d.pfc_reg_num_prov LIKE N'%" + Value + "%' )");
                    break;
                case "CHASSIS_NO":
                    strSql = string.Format(strSql, @"( d.pfc_chassis_num LIKE N'%" + Value + "%' )");
                    break;
                case "BARCODE":
                    strSql = string.Format(strSql, @"( d.pfc_bar_code LIKE N'%" + Value + "%' )");
                    break;
                case "INSURANCE_CARD":
                    strSql = string.Format(strSql, @"( d.pfc_insurance_card LIKE N'%" + Value + "%' )");
                    break;
                    //case "PLATE_NO":
                    //    strSql = string.Format(strSql, "c.pfc_reg_num = N'" + Value + "'");
                    //    break;
                    //case "PROVINCE":
                    //    strSql = string.Format(strSql, @"( c.pfc_reg_num_prov LIKE N'" + Value + "'");
                    //    break;
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
            da.Fill(dt);
            return dt;
        }
        public System.Data.DataTable QueryInfo_ContactSort(string Type, string Value)
        {
            string strSql = @"SELECT  TOP " + System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key].ToString() + @"
                                      c.pfc_policyId , 
                                      c.pfc_policy_number AS [PolicyNo],
                                      c.pfc_cus_fullname AS [CustomerName],
                                      d.pfc_reg_num As [PlateNo],
									  d.pfc_reg_num_prov AS [Province],
                                      d.pfc_chassis_num AS [ChassisNo],
                                      d.pfc_bar_code AS [BARCODE],
									  d.pfc_insurance_card AS [InsuranceCard],
									  c.pfc_ndrv1_name AS [Driver1],
									  c.pfc_ndrv2_name AS [Driver2],
                                      CONVERT(CHAR(11), c.pfc_effective_date, 106) AS [EffectiveDate],
                                      CONVERT(CHAR(11), c.pfc_expiry_date, 106) AS [ExpireDate],
                                      d.pfc_policy_additionalid AS [PolicyAddId],
                                      d.pfc_policy_additional_name  AS [PolicyAddName]                           
                              FROM    pfc_policyBase c WITH ( NOLOCK )
                                      INNER JOIN pfc_policy_additionalBase d
                                      ON c.pfc_policyId = d.pfc_policyId
                                      WHERE   c.StateCode = '0' AND {0}
                              ORDER BY [pfc_cus_fullname] ";

            switch (Type)
            {
                case "POLICY_NO":
                    strSql = string.Format(strSql, @"( c.pfc_chdr_num LIKE N'%" + Value + "%' )");
                    break;
                case "CUSTOMER_NAME":
                    strSql = string.Format(strSql, @"( c.pfc_cus_fullname LIKE N'%" + Value + "%' )");
                    break;
                case "PLATE_NO":
                    strSql = string.Format(strSql, @"( d.pfc_reg_num LIKE N'%" + Value + "%' )");
                    break;
                case "PROVINCE":
                    strSql = string.Format(strSql, @"( d.pfc_reg_num_prov LIKE N'%" + Value + "%' )");
                    break;
                case "CHASSIS_NO":
                    strSql = string.Format(strSql, @"( d.pfc_chassis_num LIKE N'%" + Value + "%' )");
                    break;
                case "BARCODE":
                    strSql = string.Format(strSql, @"( d.pfc_bar_code LIKE N'%" + Value + "%' )");
                    break;
                case "INSURANCE_CARD":
                    strSql = string.Format(strSql, @"( d.pfc_insurance_card LIKE N'%" + Value + "%' )");
                    break;
                    //case "PLATE_NO":
                    //    strSql = string.Format(strSql, "c.pfc_reg_num = N'" + Value + "'");
                    //    break;
                    //case "PROVINCE":
                    //    strSql = string.Format(strSql, @"( c.pfc_reg_num_prov LIKE N'" + Value + "'");
                    //    break;
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
            da.Fill(dt);
            return dt;
        }
    }
}

