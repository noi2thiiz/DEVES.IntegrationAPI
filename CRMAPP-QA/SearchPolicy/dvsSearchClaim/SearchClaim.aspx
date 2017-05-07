using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using devesCustomCTI;

public partial class SearchCTI : System.Web.UI.Page
{
    public class ParamsType
    {

        public static string Simple = "Simple";
        public static string CZID = "CZID";
        public static string PNUM = "PNUM";
        public static string TNUM = "TNUM";
        public static string FirstNameTH = "FIRSTNAME_TH";
        public static string LastNameTH = "LASTNAME_TH";
        public static string FullNameTH = "FULLNAME_TH";
        public static string FirstNameENG = "FIRSTNAME_ENG";
        public static string LastNameENG = "LASTNAME_ENG";
        public static string FullNameENG = "FULLNAME_ENG";
        public static string PlateNo = "PLATE_NO";
        public static string ChassisNo = "CHASSIS_NO";
    }
    public class ObjectTypeCode
    {
        public static int Account = 1;
        public static int Contact = 2;
        public static int Policy = 10002;
    }
    const int grid_Page = 10;
    const string _TOP_QUERY_Key = "TOP_QUERY";
    const string _SERVER_Key = "SERVER";
    const string _ORGANIZES_Key = "ORGANIZES";
    const string _URL_CONTACT_Key = "URL_CONTACT";
    const string _URL_PHONECALL_Key = "URL_PHONECALL";
    private QueryInfo QryInfo = new QueryInfo();
    private System.Data.DataTable dt = new System.Data.DataTable();
    private void ManaulSearch()
    {
        int typeSearch = Convert.ToInt32(ddlSearchView.SelectedIndex);
        dt = new System.Data.DataTable();
        if (!string.IsNullOrEmpty(textSearch.Text.Trim()))
        {
            switch (typeSearch)
            {
                case 0: dt = QryInfo.QueryInfo_Contact(ParamsType.CZID, textSearch.Text.Trim()); break;//Citizen Id
                case 1: dt = QryInfo.QueryInfo_Contact(ParamsType.FirstNameTH, textSearch.Text.Trim()); break;//FirstNameTH
                case 2: dt = QryInfo.QueryInfo_Contact(ParamsType.LastNameTH, textSearch.Text.Trim()); break;//LasteNameTH
                case 3: dt = QryInfo.QueryInfo_Contact(ParamsType.FullNameTH, textSearch.Text.Trim()); break;//FullNameTH
                case 4: dt = QryInfo.QueryInfo_Contact(ParamsType.FirstNameENG, textSearch.Text.Trim()); break;//FirstNameENG
                case 5: dt = QryInfo.QueryInfo_Contact(ParamsType.LastNameENG, textSearch.Text.Trim()); break;//LasteNameENG
                case 6: dt = QryInfo.QueryInfo_Contact(ParamsType.FullNameENG, textSearch.Text.Trim()); break;//FullNameENG
                case 7: dt = QryInfo.QueryInfo_Contact(ParamsType.TNUM, textSearch.Text.Trim()); break;//Telephone
                                                                                                       //case 8: dt = QryInfo.QueryInfo_Policy(ParamsType.PNUM, textSearch.Text.Trim(), ddlSearchPRD_GRP.SelectedValue, chkbIsExpired.Checked); break;//Policy Number
                                                                                                       //case 9: dt = QryInfo.QueryInfo_Policy(ParamsType.PlateNo,textSearch.Text.Trim(), ddlSearchPRD_GRP.SelectedValue, chkbIsExpired.Checked); break;//Plate Number
                                                                                                       //case 10: dt = QryInfo.QueryInfo_Policy(ParamsType.ChassisNo, textSearch.Text.Trim(), ddlSearchPRD_GRP.SelectedValue, chkbIsExpired.Checked); break;//Chassis Number

            }//-- switch (TypeSearch) -
            if (typeSearch <= 7)
                showDataGrid(gvCZID, ParamsType.CZID, dt);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void onBtn_Ok_Click(object sender,EventArgs e)
    {
        #region [Varibles]
        GridView GVTmp = new GridView();
        string _ChkBox = "";
        string _hfOwnerPolicy = "";
        string _hfAccountPolicy = "";
        string _lbPolicyName = "";
        string _lbPolicyNumber = "";
        string _lbFullname = "";
        string _lbProducerCode = "";
        int _ObjTypeCode = 0;
        #endregion
            if (gvCZID.Visible) {
            GVTmp = gvCZID; _ChkBox = "chkCZIDAuthorizedPerson"; 
            _ObjTypeCode = ObjectTypeCode.Contact;
            _lbFullname = "lbCZIDFullnameTH";
            _lbProducerCode = "lbCZIDProducerCode";            
        }
            foreach (GridViewRow GV in GVTmp.Rows)
        {
            CheckBox chkSelected = (CheckBox)GV.FindControl(_ChkBox);
            if(chkSelected != null)
            {
                if (chkSelected.Checked)
                {
                    string Primarykey = GVTmp.DataKeys[GV.RowIndex].Value.ToString();



                }
            }
        }
    }
    private void openPageCRM_Phonecall(int etc,string ObjID, string sender,string OwnerPolicy,string AccountPolicy,string Policy,string ProducerCode)
    {

    }
    private void openPageCRM(int etc,string ObjID)
    {
        string Server = System.Configuration.ConfigurationManager.AppSettings[_SERVER_Key].ToString();
        Server = string.Format(Server, Request.Url.Authority, System.Configuration.ConfigurationManager.AppSettings[_ORGANIZES_Key].ToString());
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        string Url_Contact = string.Format(System.Configuration.ConfigurationManager.AppSettings[_URL_CONTACT_Key].ToString(), etc.ToString(), ObjID);
        sb.Append("window.open('" + Server + Url_Contact + "','_blank','height=' + ( screen.height - (screen.height*(10/100))) + ',width=' + screen.width + ',top=0,left=0,resizable=yes,status=yes');");
        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "myScript", sb.ToString(), true);
    }
    //protected void ddlSearchView_SelectedIndexChanged(object Sender,EventArgs e)
    //{

    //}
    protected void onBtn_Search_Click(object sender, EventArgs e)
    {
        ManaulSearch();
    }
    private void showDataGrid(GridView GV, string Type, System.Data.DataTable dt)
    {
        System.Data.DataTable dtLookup = new System.Data.DataTable();
        switch (Type)
        {
            case "CZID":
                dtLookup.Columns.AddRange(new DataColumn[] { new DataColumn("ContactId",typeof(string)), new DataColumn("enabled",typeof(bool)) ,
                                                                 new DataColumn("PersonId"),                                                              
                                                                 new DataColumn("FullNameThai"), new DataColumn("VIP"),
                                                                 new DataColumn("Language"),
                                                                 new DataColumn("CustomerSensitive"),
                                                                 new DataColumn("PolisyClientID"),
                                                                 new DataColumn("PolisySecurityNum"),
                                                                 new DataColumn("CustomerPrivilege"),
                                                                 new DataColumn("CitizenID")});
                break;
            case "PNUM":
                /*Cancelled 17-FEB-2013
                /*dtLookup.Columns.AddRange(new DataColumn[] { new DataColumn("new_policyid",typeof(string)), new DataColumn("enabled",typeof(bool)), 
                                                             new DataColumn("PolicyNumber"), new DataColumn("InsuredName"),
                                                             new DataColumn("ProductGroup"),new DataColumn("ProductType"),
                                                             new DataColumn("ChassisNo"),
                                                             new DataColumn("PlateNo"),new DataColumn("PlateJW"),
                                                             new DataColumn("StartDate"),new DataColumn("EndDate"),
                                                             new DataColumn("OwnerPolicy"),new DataColumn("AccountPolicy")});*/
                dtLookup.Columns.AddRange(new DataColumn[] { new DataColumn("new_policyid",typeof(string)), new DataColumn("enabled",typeof(bool)),
                                                                 new DataColumn("PolicyNumber"), new DataColumn("InsuredName"),
                                                                 new DataColumn("ProductType"),
                                                                 new DataColumn("PlateJW"),new DataColumn("PlateNo"),
                                                                 new DataColumn("StartDate"),new DataColumn("EndDate"),
                                                                 new DataColumn("OwnerPolicy"),new DataColumn("AccountPolicy"),
                                                                 new DataColumn("RiskText"),new DataColumn("Deduct")});
                break;
            case "TNUM":
                dtLookup.Columns.AddRange(new DataColumn[] { new DataColumn("contactid",typeof(string)), new DataColumn("enabled",typeof(bool)) ,
                                                                 new DataColumn("FullNameThai"), new DataColumn("FullNameEng"),
                                                                 new DataColumn("HomePhone"),new DataColumn("BusinessPhone"),new DataColumn("MobilePhone") });
                break;
            case "PLATE_NO":
                dtLookup.Columns.AddRange(new DataColumn[] { new DataColumn("new_policyid",typeof(string)), new DataColumn("enabled",typeof(bool)),
                                                                 new DataColumn("PolicyNumber"), new DataColumn("InsuredName"),
                                                                 new DataColumn("PlateNo"),new DataColumn("PlateJW"),
                                                                 new DataColumn("StartDate"),new DataColumn("EndDate"),
                                                                 new DataColumn("OwnerPolicy"),new DataColumn("AccountPolicy"),
                                                                 new DataColumn("RiskText"),new DataColumn("Deduct")});
                break;

        } //-- switch (Type) --
          /*Step 2. Add Data*/
        int originalRow = dt.Rows.Count;
        //btnOK.Enabled = originalRow > 0 ? true : false;
        for (int i = 0; i < originalRow; i++)
        {
            System.Data.DataRow dr = dtLookup.NewRow();
            switch (Type)
            {
                case "CZID":
                    dr.ItemArray = new object[] { dt.Rows[i]["ContactId"].ToString(), true,
                                                      dt.Rows[i]["PersonId"].ToString(),
                                                      dt.Rows[i]["FullNameThai"].ToString(),
                                                      dt.Rows[i]["VIP"].ToString(),
                                                      dt.Rows[i]["Language"].ToString(),
                                                      dt.Rows[i]["PolisyClientID"].ToString(),
                                                      dt.Rows[i]["PolisySecurityNum"].ToString(),
                                                      dt.Rows[i]["CustomerSensitive"].ToString(),
                                                      dt.Rows[i]["CustomerPrivilege"].ToString(),
                                                      dt.Rows[i]["CitizenID"].ToString()};
                    break;
                case "PNUM":
                    /*Cancelled 17-FEB-2013*/
                    /*dr.ItemArray = new object[] { dt.Rows[i]["new_policyId"].ToString(),  true,
                                                  dt.Rows[i]["Policy Number"].ToString(), 
                                                  dt.Rows[i]["Insured Name"].ToString(),
                                                  dt.Rows[i]["new_Product_GroupName"].ToString(), 
                                                  dt.Rows[i]["new_Product_TypeName"].ToString(),
                                                  dt.Rows[i]["new_chassis"].ToString(), 
                                                  dt.Rows[i]["new_plate_no"].ToString(), 
                                                  dt.Rows[i]["new_plate_jw"].ToString(),
                                                  dt.Rows[i]["new_Policy_Start_Date"].ToString(),
                                                  dt.Rows[i]["new_Policy_End_Date"].ToString(),
                                                  string.Format("{0}|{1}|{2}", dt.Rows[i]["new_Owner_policy"].ToString(), dt.Rows[i]["new_Owner_policyName"].ToString(), "contact"),
                                                  string.Format("{0}|{1}|{2}",dt.Rows[i]["new_AccountPolicy"].ToString(), dt.Rows[i]["new_AccountPolicyName"].ToString(), "account")};*/
                    dr.ItemArray = new object[] { dt.Rows[i]["new_policyId"].ToString(),  true,
                                                      dt.Rows[i]["Policy Number"].ToString(),
                                                      dt.Rows[i]["Insured Name"].ToString(),
                                                      dt.Rows[i]["new_Product_TypeName"].ToString(),
                                                      dt.Rows[i]["new_plate_jw"].ToString(),
                                                      dt.Rows[i]["new_plate_no"].ToString(),
                                                      dt.Rows[i]["new_Policy_Start_Date"].ToString(),
                                                      dt.Rows[i]["new_Policy_End_Date"].ToString(),
                                                      string.Format("{0}|{1}|{2}", dt.Rows[i]["new_Owner_policy"].ToString(), dt.Rows[i]["new_Owner_policyName"].ToString(), "contact"),
                                                      string.Format("{0}|{1}|{2}",dt.Rows[i]["new_AccountPolicy"].ToString(), dt.Rows[i]["new_AccountPolicyName"].ToString(), "account"),
                                                      dt.Rows[i]["new_risk_text"].ToString(),
                                                      dt.Rows[i]["new_deduct"].ToString()};
                    break;
                case "TNUM":
                    dr.ItemArray = new object[] { dt.Rows[i]["ContactId"].ToString(), true,
                                                     dt.Rows[i]["Full Name Thai"].ToString(),
                                                     dt.Rows[i]["Full Name Eng"].ToString(),
                                                      dt.Rows[i]["Home Phone"].ToString(),
                                                      dt.Rows[i]["Business Phone"].ToString(),
                                                      dt.Rows[i]["Mobile Phone"].ToString()};
                    break;
                case "PLATE_NO":
                    dr.ItemArray = new object[] { dt.Rows[i]["new_policyId"].ToString(),  true,
                                                      dt.Rows[i]["Policy Number"].ToString(),
                                                      dt.Rows[i]["Insured Name"].ToString(),
                                                      dt.Rows[i]["new_plate_no"].ToString(),
                                                      dt.Rows[i]["new_plate_jw"].ToString(),
                                                      dt.Rows[i]["new_Policy_Start_Date"].ToString(),
                                                      dt.Rows[i]["new_Policy_End_Date"].ToString(),
                                                      string.Format("{0}|{1}|{2}", dt.Rows[i]["new_Owner_policy"].ToString(),dt.Rows[i]["new_Owner_policyName"].ToString(), "contact"),
                                                      string.Format("{0}|{1}|{2}",dt.Rows[i]["new_AccountPolicy"].ToString(), dt.Rows[i]["new_AccountPolicyName"].ToString(), "account"),
                                                      dt.Rows[i]["new_risk_text"].ToString(),
                                                      dt.Rows[i]["new_deduct"].ToString()};
                    break;
            } //-- switch (Type) --}
            dtLookup.Rows.Add(dr);
        } //-- for (int i = 0; i < originalRow; i++) --
        Display(Type);

        /*Step 3. Set Display Gridview*/
        hfTotal.Value = originalRow.ToString();
        ////1-50 of 500+ (1 select)
        int TopQuery = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key]);
        lbTotal.Text = string.Format("{0} - {1} of {2} (1 select)", "1", dtLookup.Rows.Count < 10 ? originalRow.ToString() : "10", originalRow == TopQuery ? hfTotal.Value + '+' : hfTotal.Value);

        if ((originalRow % 10) > 0)
        {
            for (int i = 0; i < (10 - originalRow % 10); i++)
            {
                DataRow dr = dtLookup.NewRow();
                switch (Type)
                {
                    //case "CZID": dr.ItemArray = new object[] { "", false, "", "", "", "", "", "", "", "", "", "" }; break;
                    case "CZID": dr.ItemArray = new object[] { "", false, "", "" ,"" }; break;
                    case "PNUM":
                        /*Cancelled 17-FEB-2013*/
                        /*dr.ItemArray = new object[] { "", false, "", "", "", "", "", "", "", "", "" }; */
                        dr.ItemArray = new object[] { "", false, "", "", "", "", "", "", "", "", "", "", "" };
                        break;
                    case "TNUM": dr.ItemArray = new object[] { "", false, "", "", "", "", "" }; break;
                    case "PLATE_NO": dr.ItemArray = new object[] { "", false, "", "", "", "", "", "", "", "", "", "" }; break;
                }
                //dr["enabled"] = false;
                dtLookup.Rows.Add(dr);
            }
        }
        //-- if ((originalRow % 10) > 0) --

        GV.DataSource = dtLookup;
        GV.DataBind();
        Session["dtLookup"] = dtLookup;
        Session["ParamsType"] = Type;
        

    }
    private void Display(string type)
    {
        //gvContact.Visible = type.Equals(ParamsType.Simple) ? true : false;
        gvCZID.Visible = type.Equals(ParamsType.CZID) ? true : false;
        //gvPNUM.Visible = type.Equals(ParamsType.PNUM) ? true : false;
        //gvTNUM.Visible = type.Equals(ParamsType.TNUM) ? true : false;
        //gvPlateNUM.Visible = type.Equals(ParamsType.PlateNo) ? true : false;
        //btnNew.Visible = MODE.Equals(ParamsType.MODE_SEARCH) ? false : true;
    }
    protected void gvCZID_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "contactid").ToString() != "")
            {
                e.Row.Attributes.Add("onMouseOver", "this.style.cursor='pointer';");
            }
            else
            {
                e.Row.Cells[0].Enabled = false;
            }
        }
    }



}