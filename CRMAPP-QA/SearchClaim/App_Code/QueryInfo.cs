using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devesSearchClaim
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
                                      c.pfc_claimId ,
                                      c.pfc_claim_number AS [ClaimNo] ,                                     
                                      c.pfc_cus_fullname AS [CustomerName] ,
                                      c.pfc_reg_no AS [PlateNo],
                                      c.pfc_reg_no_province AS [Province],
                                      c.pfc_chdr_number AS [PolicyNo],
                                      CONVERT(CHAR(11), c.pfc_date_rep, 106) AS [ClaimOpenDate],
			                          RIGHT(c.pfc_date_rep, 8)  AS [ClaimOpenTime],
                                      c.pfc_date_rep AS [ClaimOpenDateFull],
                                      d.pfc_policy_additional_name AS [PolicyAddName],
                                      d.pfc_policy_additionalId AS [PolicyAddId]
                              FROM    pfc_claimBase c WITH ( NOLOCK ) 
                                      INNER JOIN pfc_policy_additionalBase d
                                      ON c.pfc_parent_policyId = d.pfc_policyId
                              WHERE   c.StateCode = '0' AND {0}
                              ORDER BY [pfc_cus_fullname] ";

            switch (Type)
            {
                case "CLAIM_NO":
                    strSql = string.Format(strSql, "c.pfc_claim_number LIKE N'%" + Value + "%'");
                    break;
                case "CUSTOMER_NAME":
                    strSql = string.Format(strSql, @"( c.pfc_cus_fullname LIKE N'%" + Value + "%' )");
                    break;
                case "PLATE_NO":
                    strSql = string.Format(strSql, @"(c.pfc_reg_no LIKE N'%" + Value + "%')" );
                    break;
                case "PROVINCE":
                    strSql = string.Format(strSql, @"( c.pfc_reg_no_province LIKE N'%" + Value + "%')" );
                    break;
                case "POLICY_NO":
                    strSql = string.Format(strSql, @"( d.pfc_policy_additional_name LIKE N'%" + Value + "%' )");
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
