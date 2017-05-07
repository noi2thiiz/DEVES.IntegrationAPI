using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devesSearch
{
    /// <summary>
    /// Summary description for QueryInfo
    /// </summary>
    public class QueryInfo
    {
        public enum ENUM_CLIENT_TYPE
        {
            Personal,
            Corporate
        }

        const string _TOP_QUERY_Key = "TOP_QUERY";
        //----Query Claim----
        #region [Query Claim]
        public System.Data.DataTable QueryInfo_Claim(string Type, string Value)
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
                    strSql = string.Format(strSql, @"(c.pfc_reg_no LIKE N'%" + Value + "%')");
                    break;
                case "PROVINCE":
                    strSql = string.Format(strSql, @"( c.pfc_reg_no_province LIKE N'%" + Value + "%')");
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
        #endregion
        //-------------------

        //----Query Case ----
        #region [Query Case]
        public System.Data.DataTable QueryInfo_Case(string Type, string Value)
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
        #endregion
        //-------------------

        //----Query Customer ----
        #region [Query Customer]

        public System.Data.DataTable QueryInfo_Customer(string Type, string Value, ENUM_CLIENT_TYPE clientType = ENUM_CLIENT_TYPE.Personal)
        {
            string strSql = "";
            switch (clientType)
            {
                case ENUM_CLIENT_TYPE.Personal:
                    strSql = @"SELECT  TOP " + System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key].ToString() + @"
                                        c.ContactId as ClientId,                                 
                                        c.FullName AS [CustomerName] ,
                                        c.pfc_polisy_client_id AS [CustomerClientNo],
                                        c.pfc_citizen_id AS [CitizenID/TaxNo],
                                        dbo.f_GetPicklistName('Contact','pfc_customer_vip', c.pfc_customer_vip) AS [VIP],
                                        dbo.f_GetPicklistName('Contact','pfc_customer_privilege_level', c.pfc_customer_privilege_level) AS [Privilege],
                                        dbo.f_GetPicklistName('Contact','pfc_customer_sensitive_level', c.pfc_customer_sensitive_level) AS [Sensitive],
                                        'Personal' AS [ClientType],
		                                c.pfc_cleansing_cusormer_profile_code as [CleansingId],
		                                c.pfc_crm_person_id as [CrmClientId]
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
                    break;
                case ENUM_CLIENT_TYPE.Corporate:
                    strSql = @"SELECT  TOP " + System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key].ToString() + @"
                                        a.AccountId as [ClientId],                                 
                                        a.name AS [CustomerName] ,
                                        a.pfc_polisy_client_id AS [CustomerClientNo],
                                        a.pfc_tax_no AS [CitizenID/TaxNo],
                                        dbo.f_GetPicklistName('account','pfc_customer_vip', a.pfc_customer_vip) AS [VIP],
                                        dbo.f_GetPicklistName('account','pfc_customer_privilege_level', a.pfc_customer_privilege_level) AS [Privilege],
                                        dbo.f_GetPicklistName('account','pfc_customer_sensitive_level', a.pfc_customer_sensitive_level) AS [Sensitive],
                                        'Corporate' AS [ClientType],
										a.pfc_cleansing_cusormer_profile_code as [CleansingId],
										a.accountnumber as [CrmClientId]
                                FROM    AccountBase a WITH ( NOLOCK )
                                WHERE   a.StateCode = '0' AND {0}
                                ORDER BY [Name] ";
                    switch (Type)
                    {
                        case "CUSTOMER_NAME":
                            strSql = string.Format(strSql, "a.Name LIKE N'%" + Value + "%'");
                            break;
                        case "CUSTOMER_CLIENT_NO":
                            strSql = string.Format(strSql, @"( a.pfc_polisy_client_id LIKE N'%" + Value + "%' )");
                            break;
                        case "CITIZEN_ID":
                            strSql = string.Format(strSql, @"( a.pfc_tax_no LIKE N'%" + Value + "%' )");
                            break;
                    }
                    break;
                default:
                    break;
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
            //System.Data.DataTable dtCloned = dt.Clone();
            da.Fill(dt);
            return dt;


        }

        //public System.Data.DataTable QueryInfo_Customer(string Type, string Value)
        //{
        //    string strSql = @"SELECT  TOP " + System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key].ToString() + @"
        //                              c.ContactId ,                                 
        //                              c.FullName AS [CustomerName] ,
        //                              c.pfc_polisy_client_id AS [CustomerClientNo],
        //                              c.pfc_citizen_id AS [CItizenID],
        //                              c.pfc_customer_vip AS [VIP],
        //                              c.pfc_customer_privilege_level AS [Privilege],
        //                              c.pfc_customer_sensitive_level AS [Sensitive]
        //                      FROM    ContactBase c WITH ( NOLOCK )
        //                      WHERE   c.StateCode = '0' AND {0}
        //                      ORDER BY [FullName] ";

        //    switch (Type)
        //    {
        //        case "CUSTOMER_NAME":
        //            strSql = string.Format(strSql, "c.FullName LIKE N'%" + Value + "%'");
        //            break;
        //        case "CUSTOMER_CLIENT_NO":
        //            strSql = string.Format(strSql, @"( c.pfc_polisy_client_id LIKE N'%" + Value + "%' )");
        //            break;
        //        case "CITIZEN_ID":
        //            strSql = string.Format(strSql, @"( c.pfc_citizen_id LIKE N'%" + Value + "%' )");
        //            break;



        //    }
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
        //    //System.Data.DataTable dtCloned = dt.Clone();
        //    da.Fill(dt);
        //    return dt;


        //}
        #endregion
        //-------------------


        //----Query Policy ---- (don't have Driver1 and Driver2 in sql)
        #region [Query Policy]
        public System.Data.DataTable QueryInfo_Policy(string Type, string Value)
        {
            string strSql = @"SELECT  TOP " + System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key].ToString() + @"
                                      pfc_policy_additionalId , 
                                      pfc_policy_additional_name AS [PolicyNo],
                                      pfc_fullname AS [CustomerName],
                                      pfc_reg_num As [PlateNo],
									  pfc_reg_num_prov AS [Province],
                                      pfc_chassis_num AS [ChassisNo],
                                      pfc_bar_code AS [BARCODE],
									  '-' AS [InsuranceCard],
									  '-' AS [Driver1],
									  '-' AS [Driver2],
                                      CONVERT(CHAR(11), pfc_effective_date, 106) AS [EffectiveDate],
                                      CONVERT(CHAR(11), pfc_expiry_date, 106) AS [ExpireDate],
                                      pfc_policy_additionalid AS [PolicyAddId],
                                      pfc_policy_additional_name  AS [PolicyAddName]                           
									  FROM dbo.vw_crmPolicyDetail a
                                      WHERE   StateCode = '0' AND {0}
                              ORDER BY [CustomerName] ";

            switch (Type)
            {
                case "POLICY_NO":
                    strSql = string.Format(strSql, @"( a.pfc_chdr_num LIKE N'%" + Value + "%' )");
                    break;
                case "CITIZEN_ID":
                    strSql = string.Format(strSql, @"( a.pfc_citizenid LIKE N'%" + Value + "%' )");
                    break;
                case "CUSTOMER_NAME":
                    strSql = string.Format(strSql, @"( a.pfc_fullname LIKE N'%" + Value + "%' )");
                    break;
                case "MOBILE":
                    strSql = string.Format(strSql, @"(    
				( a.insuredCustomerIdIdType = 1 and exists( select * from CRMQA_MSCRM.dbo.AccountBase b where b.AccountId = a.insuredCustomerId and b.pfc_moblie_phone1 LIKE N'%" + Value + @"%') )
				OR
				( a.insuredCustomerIdIdType = 2 and exists( select * from CRMQA_MSCRM.dbo.ContactBase c where c.ContactId = a.insuredCustomerId and c.MobilePhone LIKE N'%" + Value + @"%'))			
			)") ;
                    break;
                case "PLATE_NO":
                    strSql = string.Format(strSql, @"( a.pfc_reg_num LIKE N'%" + Value + "%' )");
                    break;
                case "CHASSIS_NO":
                    strSql = string.Format(strSql, @"( a.pfc_chassis_num LIKE N'%" + Value + "%' )");
                    break;
                case "BARCODE":
                    strSql = string.Format(strSql, @"( a.pfc_bar_code LIKE N'%" + Value + "%' )");
                    break;
                case "INSURANCE_CARD":
                    strSql = string.Format(strSql, @"( a.pfc_insurance_card LIKE N'%" + Value + "%' )");
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
        #endregion
        //-------------------

        //----Query Informer----
        #region [Query Informer]
        public System.Data.DataTable QueryInfo_Informer(string Type, string Value)
        {
            string strSql = @"SELECT  TOP " + System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key].ToString() + @"
                                      c.ContactId as ClientId,                                 
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
                    strSql = string.Format(strSql, @"( c.FullName LIKE N'%" + Value + "%' )");
                    break;
                case "CUSTOMER_CLIENT_NO":
                    strSql = string.Format(strSql, @"( c.pfc_polisy_client_id LIKE N'%" + Value + "%' )");
                    break;
                case "CITIZEN_ID":
                    strSql = string.Format(strSql, @"( c.pfc_citizen_id LIKE N'%" + Value + "%' )");
                    break;



            }
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
            //System.Data.DataTable dtCloned = dt.Clone();
            da.Fill(dt);
            return dt;


        }
        #endregion
        //--------------

        //-----Query Driver -----
        #region [Query Driver]
        public System.Data.DataTable QueryInfo_Driver(string Type, string Value)
        {
            string strSql = @"SELECT  TOP " + System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key].ToString() + @"
                                      c.ContactId as ClientId,                                 
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
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
            //System.Data.DataTable dtCloned = dt.Clone();
            da.Fill(dt);
            return dt;
        }
        #endregion
        //---------------------
    }
}
