using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devesCustomCTI
{
    public class QueryInfo
    {

        const string _TOP_QUERY_Key = "TOP_QUERY";

        public System.Data.DataTable QueryInfo_Contact(string Type, string Value)
        {
            string strSql = @"SELECT  TOP " + System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key].ToString() + @"
                                      c.ContactId ,
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
                //case "TNUM":
                //    if (Value.Length == 10)
                //        //strSql = string.Format(strSql, @"( new_crm_mobile_no = N'" + Value + "' OR new_Additional_Mobile_Phone1 = N'" + Value + "' OR new_Additional_Mobile_Phone2 = N'" + Value + "' OR new_Additional_Mobile_Phone3 = N'" + Value + "' )");
                //        strSql = string.Format(strSql, @"( new_crm_mobile_no = N'" + Value + "' OR new_Additional_Mobile_Phone1 = N'" + Value + "' OR new_Additional_Mobile_Phone2 = N'" + Value + "' )");
                //    else
                //        //strSql = string.Format(strSql, @"( new_crm_business_phone = N'" + Value + "' OR new_crm_home_phone = N'" + Value + "' OR new_Additional_Phone = N'" + Value + "' )");
                //        strSql = string.Format(strSql, @"( new_crm_business_phone = N'" + Value + "' OR new_crm_home_phone = N'" + Value + "' OR new_Additional_Phone = N'" + Value + "' OR new_Additional_Mobile_Phone3 = N'" + Value + "' )");
                //    break;
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
                                      p.new_policyId ,
                                      p.new_Policy_Id AS [Policy Number] ,
                                      p.new_policy_name AS [Insured Name] ,
                                      p.new_Product_GroupName ,
                                      p.new_Product_TypeName ,
                                      p.new_chassis ,
                                      p.new_plate_no ,
                                      p.new_plate_jw ,
                                      CONVERT(NVARCHAR(50), DATEADD(hour,7, p.new_Policy_Start_Date), 103) AS [new_Policy_Start_Date] ,
                                      CONVERT(NVARCHAR(50), DATEADD(hour,7, p.new_Policy_End_Date), 103) AS [new_Policy_End_Date] ,
                                      p.new_Owner_policy , p.new_Owner_policyName ,
                                      p.new_AccountPolicy , p.new_AccountPolicyName ,
                                      p.new_risk_text , p.new_deduct 
                              FROM    new_policy p WITH ( NOLOCK )
                              WHERE   p.StateCode = 0 AND {0} 
                              ORDER BY p.new_Policy_End_Date DESC , [Policy Number]";
            if (Convert.ToInt32(ValueProd_GRP) < 99) strCondition = "p.new_product_group_code = '" + ValueProd_GRP + "' and ";
            if (!ValueIsExpired) strCondition += "(p.new_is_expired = 0 or p.new_is_expired is null) and ";
            switch (Type)
            {
                case "PNUM":
                    strSql = string.Format(strSql, strCondition + "p.new_Policy_Id = N'" + Value + "'");
                    break;
                case "PLATE_NO":
                    strSql = string.Format(strSql, strCondition + "p.new_plate_no = N'" + Value + "'");
                    break;
                case "CHASSIS_NO":
                    strSql = string.Format(strSql, strCondition + "p.new_chassis = N'" + Value + "'");
                    break;
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSql, System.Configuration.ConfigurationManager.AppSettings["CRMDATA"].ToString());
            da.Fill(dt);
            return dt;
        }
    }
}