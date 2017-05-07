using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using getWebconfig;

namespace devesSearchCustomer
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
                                      c.ContactId ,                                 
                                      c.FullName AS [CustomerName] ,
                                      c.pfc_polisy_client_id AS [CustomerClientNo],
                                      c.pfc_citizen_id AS [CItizenID],
                                      c.pfc_customer_vip AS [VIP],
                                      c.pfc_customer_privilege_level AS [Privilege],
                                      c.pfc_customer_sensitive_level AS [Sensitive]
                              FROM    ContactBase c WITH ( NOLOCK )
                              WHERE   c.StateCode = '0' AND {0}
                              ORDER BY [FullName] ";

            switch (Type)
            {
                case "CUSTOMER_NAME":
                    strSql = string.Format(strSql, "c.FullName LIKE N'%" + Value + "%'");
                    break;
                case "CUSTOMER_CLIENT_NO":
                    strSql = string.Format(strSql, @"( c.pfc_polisy_client_id LIKE N'%" + Value + "%' )");
                    break;
                case "CITIZEN_ID":
                    strSql = string.Format(strSql, @"( c.pfc_citizen_id LIKE N'%" + Value + "%' )");
                    break;



            }
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, getWebconfig.selectString.SqlConnect);
            //System.Data.DataTable dtCloned = dt.Clone();
            da.Fill(dt);
            return dt;


        }
    }
}
