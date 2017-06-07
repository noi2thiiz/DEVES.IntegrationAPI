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
        #region [Query Customer] 
        public string sql_searchcustomer = @"DECLARE @type nvarchar(1) = '{0}' 
                                            DECLARE @fullname nvarchar(50) = '{1}'
                                            DECLARE @czid nvarchar(13) = '{2}'
                                            DECLARE @phoneno nvarchar(10) = '{3}'
                                            EXEC[dbo].[sp_Query_searchCustomerStore] @type,@fullname,@czid,@phoneno";

        public enum ENUM_CLIENT_TYPE
        {
            Personal,
            Corporate
        }
        #endregion

        const string _TOP_QUERY_Key = "TOP_QUERY";
        const string _TIME_OUT = "TIME_OUT";

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
            da.SelectCommand.CommandTimeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings[_TIME_OUT].ToString());
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

        public System.Data.DataTable QueryInfo_Customer(string Type, string Value , string chkType )
        {
            string type = "";
            string fullname = "";
            string czid = "";
            string phoneon = "";

            if(chkType.Equals("Corporate"))
            {
                type = "1";
            }
            else if(chkType.Equals("Personal"))
            {
                type = "2";
            }
            /**
             * 1.เขียนเงื่อนไข check ค่าจาก Type 
             **/
            switch (Type)
            {
                case "1": czid = Value; break;
                case "2": fullname = Value; break;
                case "3": phoneon = Value; break;
                default: break;
            }
        
            string strSql = string.Format(sql_searchcustomer, type, fullname, czid, phoneon);
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
            //System.Data.DataTable dtCloned = dt.Clone();
            da.SelectCommand.CommandTimeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings[_TIME_OUT].ToString());
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
        public System.Data.DataTable QueryInfo_Policy(int Motor_NonMotor, int Type, string Value)
        {
            string strSql = @"DECLARE @topRecords nvarchar(100) = '{0}';
                            DECLARE @Motor_NonMotor int ; --9: ALL, 0: Motor, 1: Non-Motor
                            DECLARE @policyNo nvarchar(20), @policyCarRegisterNo nvarchar(20), @ChassisNo nvarchar(20) 
	                            , @carCheckId nvarchar(20), @barCode nvarchar(50), @insuranceCard nvarchar(50)
	                            , @insuredFullName nvarchar(200), @driverFullName nvarchar(200)
                                , @insuredCleansingID nvarchar(20) 
                            ----------------------------------------------------
                            SET @Motor_NonMotor = {1};
                            SET @policyNo = {2};
                            SET @policyCarRegisterNo = {3};
                            SET @ChassisNo = {4};
                            SET @carCheckId = {5};
                            SET @barCode = {6};
                            SET @insuranceCard = {7};
                            SET @insuredFullName = {8};
                            SET @driverFullName  ={9};
                            SET @insuredCleansingID = {10};

                            EXEC [dbo].[sp_Query_Policy] @topRecords, @Motor_NonMotor, @policyNo, @policyCarRegisterNo
                                        ,@ChassisNo, @carCheckId, @barCode, @insuranceCard, @insuredFullName, @driverFullName 
                                        ,@insuredCleansingID";

            strSql = string.Format(strSql
                , "100" //index[0]
                , Motor_NonMotor.ToString()//index[1]
                , Type == 1 ? string.Format("'{0}'", Value) : "null"//index[2]
                , Type == 3 ? string.Format("'{0}'", Value) : "null"//index[3]
                , Type == 2 ? string.Format("'{0}'", Value) : "null"//index[4]
                , Type == 0 ? string.Format("'{0}'", Value) : "null"//index[5]
                , Type == 7 ? string.Format("'{0}'", Value) : "null"//index[6]
                , Type == 8 ? string.Format("'{0}'", Value) : "null"//index[7]
                , Type == 5 ? string.Format("'{0}'", Value) : "null"//index[8]
                , Type == 0 ? string.Format("'{0}'", Value) : "null"//index[9]
                , Type == 10 ? string.Format("'{0}'", Value) : "null"//index[10]
                );

            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
            da.SelectCommand.CommandTimeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings[_TIME_OUT].ToString());
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
            da.SelectCommand.CommandTimeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings[_TIME_OUT].ToString());
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
            da.SelectCommand.CommandTimeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings[_TIME_OUT].ToString());
            //System.Data.DataTable dtCloned = dt.Clone();
            da.Fill(dt);
            return dt;
        }
        #endregion
        //---------------------
    }

}
