using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devesCustomCTI
{
    public class QueryInfo
    {

        const string _TOP_QUERY_Key = "TOP_QUERY";

        public System.Data.DataTable QueryInfo_Contact(string Type,string Value)
        {
            string strSql = @"SELECT  TOP " + System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key].ToString() + @"
                                      c.ContactId ,
                                      c.MobilePhone AS [PhoneNumber],
                                      c.FullName AS [FullNameThai] ,                                     
                                      c.pfc_citizen_id AS [CitizenID] ,
                                      c.pfc_crm_person_id AS [PersonID],
                                      c.pfc_polisy_client_id AS [PolisyClientID],
                                      c.pfc_polisy_secuity_num AS [PolisySecurityNum],
                                      c.pfc_customer_vip AS [VIP],
                                      dbo.f_GetPicklistName('contact','pfc_customer_sensitive_level',c.pfc_customer_sensitive_level) AS [CustomerSensitive],  
                                      dbo.f_GetPicklistName('contact','pfc_customer_privilege_level',c.pfc_customer_privilege_level) AS [CustomerPrivilege],
                                      dbo.f_GetPicklistName('contact','pfc_language',c.pfc_language) AS [Language]
                              FROM    ContactBase c WITH ( NOLOCK )
                              WHERE   c.StateCode = '0' AND {0}
                              ORDER BY [FullNameThai] ";
            switch (Type)
            {
                case "CZID":
                    strSql = string.Format(strSql, "c.pfc_citizen_id = N'" + Value + "'");
                    break;
                case "TNUM":
                    if (Value.Length == 10)
                //        //strSql = string.Format(strSql, @"( new_crm_mobile_no = N'" + Value + "' OR new_Additional_Mobile_Phone1 = N'" + Value + "' OR new_Additional_Mobile_Phone2 = N'" + Value + "' OR new_Additional_Mobile_Phone3 = N'" + Value + "' )");
                        strSql = string.Format(strSql, @"( c.MobilePhone = N'" + Value + "' OR c.pfc_MobilePhone1 = N'" + Value + "' OR c.pfc_MobilePhone2 = N'" + Value + "' )");
                    else
                //        //strSql = string.Format(strSql, @"( new_crm_business_phone = N'" + Value + "' OR new_crm_home_phone = N'" + Value + "' OR new_Additional_Phone = N'" + Value + "' )");
                        strSql = string.Format(strSql, @"( c.MobilePhone = N'" + Value + "' OR c.Telephone1 = N'" + Value + "' OR c.Telephone2 = N'" + Value + "' OR c.Telephone3 = N'" + Value + "' OR c.pfc_HomePhone = N'" + Value + "' )");
                    break;
                case "FULLNAME_TH":
                    strSql = string.Format(strSql, @"( c.FullName LIKE N'%" + Value + "%' )");
                    break;
                //case "FULLNAME_ENG":
                //    strSql = string.Format(strSql, @"( c.new_name_e_bki LIKE N'%" + Value + "%' )");
                //    break;
                case "FIRSTNAME_TH":
                    strSql = string.Format(strSql, @"( c.FirstName LIKE N'" + Value + "%' )");
                    break;
                case "LASTNAME_TH":
                    strSql = string.Format(strSql, @"( c.LastName LIKE N'" + Value + "%' )");
                    break;
                //case "FIRSTNAME_ENG":
                //    strSql = string.Format(strSql, @"( c.new_First_Name_ENG LIKE N'" + Value + "%' )");
                //    break;
                //case "LASTNAME_ENG":
                //    strSql = string.Format(strSql, @"( c.new_Last_Name_ENG LIKE N'" + Value + "%' )");
                //    break;
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
            da.Fill(dt);
            return dt;
        }

        public System.Data.DataTable QueryInfo_Policy(string Type, string Value, string ValueProd_GRP, Boolean ValueIsExpired)
        {
            string strCondition = "";
            string strSql = @"SELECT  TOP " + System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key].ToString() + @"
                                      p.pfc_policyId ,
                                      p.pfc_customerId AS [CustomerId],
                                      p.pfc_cus_fullname AS [CustomerName],
                                      p.pfc_customerIdIdType AS [CustomerType],
                                      p.pfc_policy_number AS [PolicyNumber],
                                      p.pfc_cus_fullname AS [InsuredName],
                                      p.pfc_prod_group As [ProdGroup],
                                      p.pfc_prod_type As [ProdType] ,
                                      b.pfc_policy_additionalId As [policyadd],
                                      b.pfc_policy_additional_name As [policyaddname],
                                      b.pfc_chassis_num As [ChassisNumber],
                                      b.pfc_reg_num As [RegNumber],
                                      b.pfc_reg_num_prov As [RegProvince],
                                      b.pfc_deduct As [Deduct],
                                      b.pfc_rsk_num As [RiskNumber],
                                      c.pfc_claim_number As [ClaimNumber],
			                          CONVERT(NVARCHAR(50), DATEADD(hour,7, c.pfc_date_rep),103) As [ClaimOpenDate],
			                          CONVERT(NVARCHAR(50), DATEADD(hour,7, c.pfc_date_occ),103) As [DateofLoss],
			                          c.pfc_zrepclmno As [ClaimNotiNo],
                                      CONVERT(NVARCHAR(50), DATEADD(hour,7, p.pfc_issue_date), 103) AS [StartDate] ,
                                      CONVERT(NVARCHAR(50), DATEADD(hour,7, p.pfc_expiry_date), 103) AS [EndDate],
									  p.pfc_customerId As [OwnerPolicy],
                                      p.pfc_cus_fullname As [OwnerPolicyName]                 
                                      FROM    pfc_policyBase p with (nolock)
                                      LEFT JOIN pfc_policy_additionalBase b
                                      ON p.pfc_policyId = b.pfc_policyId
                                      LEFT JOIN pfc_claimBase c
                                      ON p.pfc_policyId = c.pfc_parent_policyId
                                      WHERE {0}                                   
                                      ORDER BY p.pfc_expiry_date DESC , [PolicyNumber]";
            if (Convert.ToInt32(ValueProd_GRP) < 99) strCondition = "p.pfc_prod_group = '" + ValueProd_GRP + "' and ";
            if (!ValueIsExpired) strCondition += "(p.pfc_expiry_date = 0 or p.pfc_expiry_date is null) and ";
            switch (Type)
            {
                case "PNUM":
                    strSql = string.Format(strSql, strCondition + "p.pfc_policy_number LIKE N'%" + Value + "%'");
                    break;
                case "PLATE_NO":
                    strSql = string.Format(strSql, strCondition + "b.pfc_reg_num LIKE N'%" + Value + "%'");
                    break;
                case "CHASSIS_NO":
                    strSql = string.Format(strSql, strCondition + "b.pfc_chassis_num LIKE N'%" + Value + "%'");
                    break;
                case "CLAIM_NO":
                    strSql = string.Format(strSql, strCondition + "c.pfc_claim_number LIKE N'%" + Value + "%'");
                    break;
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
            da.Fill(dt);
            return dt;
        }
        public System.Data.DataTable QueryInfo_Case(string Type, string Value)
        {
            string strCondition = "";
            string strSql = @"SELECT  TOP " + System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key].ToString() + @"
                                       c.IncidentId ,                                     
                                      c.TicketNumber AS [CaseNumber] ,
                                      c.pfc_claim_noti_number As [ClaimNoti],
                                      c.Title AS [CaseTitle] ,
                                      c.casetypecode ,
									  c.pfc_categoryId,
                                      b.pfc_category_name As [Category],
                                      c.pfc_sub_categoryId ,
									  s.pfc_sub_category_name As [SubCategory],
                                      c.pfc_case_vip As [CaseVIP],
                                      CONVERT(NVARCHAR(50), DATEADD(hour,7, c.pfc_notification_date), 103) AS [NotiDate],	  				   
									  tb.pfc_calltype_name AS [CaseType]
									  FROM    IncidentBase c with (nolock)
									  INNER JOIN pfc_calltypeBase tb
									  ON c.CaseTypeCode = tb.pfc_calltype_code
									  INNER JOIN pfc_categoryBase b
									  ON c.pfc_categoryId = b.pfc_categoryId
									  INNER JOIN pfc_sub_categoryBase s
									  ON c.pfc_sub_categoryId = s.pfc_sub_categoryId
                                      WHERE {0}
									  ORDER BY [NotiDate] DESC , [CaseNumber]";
            switch (Type)
            {
                case "CASE_NO":
                    strSql = string.Format(strSql, "c.TicketNumber = N'" + Value + "'");
                    break;
                case "NOTI_DATE":
                    strSql = string.Format(strSql, "c.pfc_notification_date = N'" + Value + "'");
                    break;
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
            da.Fill(dt);
            return dt;
        }
    }
}