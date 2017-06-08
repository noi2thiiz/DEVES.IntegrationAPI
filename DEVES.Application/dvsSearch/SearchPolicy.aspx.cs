using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using devesSearch;

public partial class SearchPolicy : System.Web.UI.Page
{
    public class ParamsType
    {
        public static string PolicyNo = "POLICY_NO";
        public static string CustomerName = "CUSTOMER_NAME";
        public static string CLAIMNOA = "CLAIMNOA";
        public static string PlateNo = "PLATE_NO";
        public static string Province = "PROVINCE";
        public static string ChassisNo = "CHASSIS_NO";
        public static string Barcode = "BARCODE";
        public static string InsuranceCard = "INSURANCE_CARD";
        public static string EffectiveDate = "EFFECTIVE_DATE";
        public static string ExpireDate = "EXPIRE_DATE";
        public static string Driver1 = "DRIVER_1";
        public static string Mobile = "MOBILE";
        public static string CitezenId = "CITIZEN_ID";
    }
    public class ObjectTypeCode
    {
        public static int Account = 1;
        public static int Contact = 2;
        public static int Policy = 10002;
    }
    private string customercleansingIdvalue;
    private string CLAIMNOA;
    const int grid_Page = 10;
    const string _TOP_QUERY_Key = "TOP_QUERY";
    const string _SERVER_Key = "SERVER";
    const string _PARAMS_Key = "PARAMS";
    const string _PARAMS_Empty = " - ";
    const string _ORGANIZES_Key = "ORGANIZES";
    const string _URL_POLICY_KEY = "URL_POLICY";
    const string _URL_CONTACT_Key = "URL_CONTACT";
    const string _URL_PHONECALL_Key = "URL_PHONECALL";


    private QueryInfo QryInfo = new QueryInfo();
    private System.Data.DataTable dt = new System.Data.DataTable();

    private void GetParams()
    {
        #region [Step I - Get Parameter]
        string[] Params = System.Configuration.ConfigurationManager.AppSettings[_PARAMS_Key].ToString().Split(':');
        //CLID = string.IsNullOrEmpty(Request.Params[Params[0]]) ? _PARAMS_Empty : Request.Params[Params[0]];
        CLAIMNOA = string.IsNullOrEmpty(Request.Params[Params[1]]) ? _PARAMS_Empty : Request.Params[Params[1]];
        //PNUM = string.IsNullOrEmpty(Request.Params[Params[2]]) ? _PARAMS_Empty : Request.Params[Params[2]];
        //TNUM = string.IsNullOrEmpty(Request.Params[Params[3]]) ? _PARAMS_Empty : ChkDigitTNUM(Request.Params[Params[3]]);
        #endregion
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        #region [Get URL String]
        if (!Page.IsPostBack)
        {

            customercleansingIdvalue = Request.QueryString["customerid"];
            int MotorType = Convert.ToInt32(motorTypeValue.SelectedValue);
            //textSearch.Text = customercleansingIdvalue;
            if (customercleansingIdvalue != null)
            {
                //ddlSearchView.Text = Server.HtmlEncode(customercleansingIdvalue);
                dt = new System.Data.DataTable();
                dt = QryInfo.QueryInfo_Policy(MotorType, 5, customercleansingIdvalue);
                showDataGrid(gvCZID, ParamsType.CLAIMNOA, dt);
            }

        }
        #endregion
    }

    private void ManaulSearch()
    {
        try
        {
            int typeSearch = Convert.ToInt32(ddlSearchView.SelectedValue);
            int MotorType = Convert.ToInt32(motorTypeValue.SelectedValue);
            dt = new System.Data.DataTable();
            if (!string.IsNullOrEmpty(textSearch.Text.Trim()))
            {
                //switch (typeSearch)
                //{

                //    case 0: dt = QryInfo.QueryInfo_Policy(ParamsType.PolicyNo, string.Format("{0}|{1}|{2}|{3}", textSearch.Text.Trim(), "", "", "")); break;//FullNameTH
                //    case 1: dt = QryInfo.QueryInfo_Policy(ParamsType.PlateNo, string.Format("{0}|{1}|{2}|{3}", "", textSearch.Text.Trim(), "", "")); break;//FullNameTH
                //    case 2: dt = QryInfo.QueryInfo_Policy(ParamsType.ChassisNo, string.Format("{0}|{1}|{2}|{3}", "", "", textSearch.Text.Trim(), "")); break;//FullNameTH

                //}
                dt = QryInfo.QueryInfo_Policy(MotorType, typeSearch, textSearch.Text.Trim());
                showDataGrid(gvCZID, ParamsType.CLAIMNOA, dt);
            }
        }
        catch (SqlException e)
        {
            string script = "alert(\"การค้นหาใช้ความยาวเกินไป กรุณาระบุคำค้นหาให้ชัดเจน!\");";
            ScriptManager.RegisterStartupScript(this, GetType(),
                                  "ServerControlScript", script, true);
        }
        catch (Exception e)
        {
            string script = "alert(\"something happen!\");";
            ScriptManager.RegisterStartupScript(this, GetType(),
                                  "ServerControlScript", script, true);
        }
    }
    private void openPageCRM(string PolicyId, string PolicyNo)
    {
        //string Server = System.Configuration.ConfigurationManager.AppSettings[_SERVER_Key].ToString();
        //Server = string.Format(Server, System.Configuration.ConfigurationManager.AppSettings[_ORGANIZES_Key].ToString());
        //System.Text.StringBuilder sb = new System.Text.StringBuilder();

        //string Url_Policy = string.Format(System.Configuration.ConfigurationManager.AppSettings[_URL_POLICY_KEY].ToString(), ObjID);
        //sb.Append("window.open('" + Server + Url_Policy + "','_blank','height=' + ( screen.height - (screen.height*(10/100))) + ',width=' + screen.width + ',top=0,left=0,resizable=yes,status=yes');");
        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "myScript", sb.ToString(), true);
        #region [-- stamp value to crm form --]
        string strRet = @"  var ResObj = new Object();
                                var PolicyArray = new Array();
                                PolicyArray[0] = '{0}';
                                PolicyArray[1] = '{1}';
                                PolicyArray[2] = 'pfc_policy_additional';

                                ResObj.new_policyid = PolicyArray; 
                                // ส่งค่าไปให้ indow.opener  โดย function รับ message จะอยู่ใน Web Resource: Function_Addcalltype
                                window.opener.postMessage(ResObj,'{2}');                  
                                window.returnValue = ResObj;                                                                                 
                             ";
        #endregion

        string strRes = string.Format(strRet
                                     , PolicyId
                                     , PolicyNo
                                     , System.Configuration.ConfigurationManager.AppSettings[_ORGANIZES_Key].ToString()
                                     );

        strRes += @" var ie7 = (document.all && !window.opera && window.XMLHttpRequest) ? true : false;
                           
                             var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);     
                                if (ie7) { window.open('','_parent',''); window.close(); }
                                else {  
                                        this.focus(); 
                                        self.opener = this; 
                                        self.close(); 
                                }";
        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "myScript", strRes, true);

    }
    protected void onBtn_Ok_Click(object sender, EventArgs e)
    {
        #region [Varibles]
        GridView GVTmp = new GridView();
        string _ChkBox = "";
        int _ObjTypeCode = 0;
        string _lbCZIDPolicyAddId = "";
        string _lbCZIDPolicyAddName = "";
        #endregion
        if (gvCZID.Visible)
        {
            GVTmp = gvCZID; _ChkBox = "checkboxPolicy"; _lbCZIDPolicyAddId = "lbPNUMpolicyadd"; _lbCZIDPolicyAddName = "lbPNUMpolicyaddname";
            _ObjTypeCode = ObjectTypeCode.Contact;
        }
        string PolicyId = "";
        string PolicyNo = "";

        foreach (GridViewRow GV in GVTmp.Rows)
        {
            CheckBox chkSelected = (CheckBox)GV.FindControl(_ChkBox);
            if (chkSelected != null)
            {
                if (chkSelected.Checked)
                {
                    System.Web.UI.WebControls.Label lbPolicyId = (System.Web.UI.WebControls.Label)GV.FindControl(_lbCZIDPolicyAddId);
                    PolicyId = lbPolicyId.Text.ToString();
                    System.Web.UI.WebControls.Label lbClaimDateTimeFull = (System.Web.UI.WebControls.Label)GV.FindControl(_lbCZIDPolicyAddName);
                    PolicyNo = lbClaimDateTimeFull.Text.ToString();
                    openPageCRM(PolicyId, PolicyNo);


                }
            }
        }
    }

    
    protected void onBtn_Search_Click(object sender, EventArgs e)
    {

        ManaulSearch();
    }
    private void showDataGrid(GridView GV, string Type, System.Data.DataTable dt)
    {
        System.Data.DataTable dtLookup = new System.Data.DataTable();
        switch (Type)
        {
            case "CLAIMNOA":
                dtLookup.Columns.AddRange(new DataColumn[] { new DataColumn("pfc_policy_additionalId",typeof(string)), new DataColumn("enabled",typeof(bool)),
                                                                 new DataColumn("pfc_policy_additional_name"),new DataColumn("insuredId"),
                                                                 new DataColumn("insuredIdIdType"),new DataColumn("insuredFullName"),
                                                                 new DataColumn("policyCarRegisterNo"),new DataColumn("policyNo"),
                                                                 new DataColumn("policyEffectiveDate"),new DataColumn("policyExpiryDate"),
                                                                 new DataColumn("policyContractType"), new DataColumn("Campaign"),
                                                                 new DataColumn("handleDept"), new DataColumn("handleFullName"),
                                                                 new DataColumn("Source"),new DataColumn("Agent"),
                                                                 new DataColumn("motorAddOn"),new DataColumn("Status"),
                                                                 new DataColumn("pfc_driver_firstId"),
                                                                 new DataColumn("pfc_vehicle_driver_first_fullname"),new DataColumn("pfc_driver_secondId"), new DataColumn("pfc_vehicle_driver_second_fullname"),
                                                                 new DataColumn("pfc_policy_statusName"),
                                                                 new DataColumn("pfc_policy_status"),
                                                                 new DataColumn("pfc_chdr_num"),
                                                                 new DataColumn("pfc_zren_num"),
                                                                 new DataColumn("pfc_tran_num"),
                                                                 new DataColumn("pfc_rsk_num")});
                break;
        }
        /*Step 2. Add Data*/
        int originalRow = dt.Rows.Count;
        //btnOK.Enabled = originalRow > 0 ? true : false;
        for (int i = 0; i < originalRow; i++)
        {
            System.Data.DataRow dr = dtLookup.NewRow();
            switch (Type)
            {
                case "CLAIMNOA":
                    dr.ItemArray = new object[] { dt.Rows[i]["pfc_policy_additionalId"].ToString(), true,
                                                      dt.Rows[i]["pfc_policy_additional_name"].ToString(),
                                                      dt.Rows[i]["insuredId"].ToString(),
                                                      dt.Rows[i]["insuredIdIdType"].ToString(),
                                                      dt.Rows[i]["insuredFullName"].ToString(),
                                                      dt.Rows[i]["policyCarRegisterNo"].ToString(),
                                                      dt.Rows[i]["policyNo"].ToString(),
                                                      dt.Rows[i]["policyEffectiveDate"].ToString(),
                                                      dt.Rows[i]["policyExpiryDate"].ToString(),
                                                      dt.Rows[i]["policyContractType"].ToString(),
                                                      dt.Rows[i]["Campaign"].ToString(),
                                                      dt.Rows[i]["handleDept"].ToString(),
                                                      dt.Rows[i]["handleFullName"].ToString(),
                                                      dt.Rows[i]["Source"].ToString(),
                                                      dt.Rows[i]["Agent"].ToString(),
                                                      dt.Rows[i]["motorAddOn"].ToString(),
                                                      dt.Rows[i]["Status"].ToString(),
                                                      dt.Rows[i]["pfc_driver_firstId"].ToString(),
                                                      dt.Rows[i]["pfc_vehicle_driver_first_fullname"].ToString(),
                                                      dt.Rows[i]["pfc_driver_secondId"].ToString(),
                                                      dt.Rows[i]["pfc_vehicle_driver_second_fullname"].ToString(),
                                                      dt.Rows[i]["pfc_policy_statusName"].ToString(),
                                                      dt.Rows[i]["pfc_policy_status"].ToString(),
                                                      dt.Rows[i]["pfc_chdr_num"].ToString(),
                                                      dt.Rows[i]["pfc_zren_num"].ToString(),
                                                      dt.Rows[i]["pfc_tran_num"].ToString(),
                                                      dt.Rows[i]["pfc_rsk_num"].ToString()};
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

        //lbTotal.Text = string.Format("{0} - {1} of {2}", "1", dtLookup.Rows.Count < 10 ? originalRow.ToString() : "10", originalRow == TopQuery ? hfTotal.Value : hfTotal.Value);

        if ((originalRow % 10) > 0)
        {
            for (int i = 0; i < (10 - originalRow % 10); i++)
            {
                DataRow dr = dtLookup.NewRow();
                switch (Type)
                {
                    //case "CZID": dr.ItemArray = new object[] { "", false, "", "", "", "", "", "", "", "", "", "" }; break;
                    case "CLAIMNOA": dr.ItemArray = new object[] { "", false, "", "", "", "", "", "", "" }; break;
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

        //GV.Columns[11].Visible = false;
        //GV.Columns[12].Visible = false;
        GV.DataSource = dtLookup;
        GV.DataBind();
        Session["dtLookup"] = dtLookup;
        Session["ParamsType"] = Type;

        lbGrid_PageIndex.Text = Convert.ToString(GV.PageIndex + 1);

        Decimal pageTotal = Math.Ceiling(Convert.ToDecimal(dtLookup.Rows.Count) / grid_Page) - 1;

        switch (Type)
        {
            //case "Simple": gvBind = gvContact; break;
            case "CLAIMNOA": GV = gvCZID; break;
                //case "PNUM": gvBind = gvPNUM; break;
                //case "TNUM": gvBind = gvTNUM; break;
                //case "PLATE_NO": gvBind = gvPlateNUM; break;
        }

        if (pageTotal == 0)
        {
            imgGrid_FirstPage.ImageUrl = "~/_imgs/page_FL0.gif";
            imgGrid_Previous.ImageUrl = "~/_imgs/page_L0.gif";
            imgGrid_Next.ImageUrl = "~/_imgs/page_R0.gif";

            return;
        }
        else if (GV.PageIndex == pageTotal)
        {
            imgGrid_FirstPage.ImageUrl = "~/_imgs/page_FL1.gif";
            imgGrid_Previous.ImageUrl = "~/_imgs/page_L1.gif";
            imgGrid_Next.ImageUrl = "~/_imgs/page_R0.gif";

        }
        else if (GV.PageIndex + 1 == pageTotal)
        {
            imgGrid_FirstPage.ImageUrl = "~/_imgs/page_FL1.gif";
            imgGrid_Previous.ImageUrl = "~/_imgs/page_L1.gif";
            imgGrid_Next.ImageUrl = "~/_imgs/page_R1.gif";
        }
        else
        {
            imgGrid_FirstPage.ImageUrl = "~/_imgs/page_FL1.gif";
            imgGrid_Previous.ImageUrl = "~/_imgs/page_L1.gif";
            imgGrid_Next.ImageUrl = "~/_imgs/page_R1.gif";
        }

        int _from = Int32.Parse(lbGrid_PageIndex.Text) * 10 - 9;
        int _to = (Int32.Parse(lbGrid_PageIndex.Text) * 10) - Int32.Parse(hfTotal.Value) > 0 ? Int32.Parse(hfTotal.Value) : Int32.Parse(lbGrid_PageIndex.Text) * 10;

        lbTotal.Text = string.Format("{0} - {1} of {2} ", _from, _to, hfTotal.Value);



    }
    private void Display(string type)
    {
        //gvContact.Visible = type.Equals(ParamsType.Simple) ? true : false;
        gvCZID.Visible = type.Equals(ParamsType.CLAIMNOA) ? true : false;
        //gvPNUM.Visible = type.Equals(ParamsType.PNUM) ? true : false;
        //gvTNUM.Visible = type.Equals(ParamsType.TNUM) ? true : false;
        //gvPlateNUM.Visible = type.Equals(ParamsType.PlateNo) ? true : false;
        //btnNew.Visible = MODE.Equals(ParamsType.MODE_SEARCH) ? false : true;
    }
    protected void gvCZID_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "pfc_policy_additionalId").ToString() != "")
            {
                e.Row.Attributes.Add("onMouseOver", "this.style.cursor='pointer';");
            }
            else
            {
                e.Row.Cells[0].Enabled = false;
            }
        }
    }
    protected void imgGrid_FirstPage_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtLookup = (DataTable)Session["dtLookup"];
        String Type = (string)Session["ParamsType"];
        Decimal pageTotal = Math.Ceiling(Convert.ToDecimal(dtLookup.Rows.Count) / grid_Page) - 1;

        GridView gvBind = new GridView();
        switch (Type)
        {
            //case "Simple": gvBind = gvContact; break;
            case "CLAIMNOA": gvBind = gvCZID; break;
                //case "PNUM": gvBind = gvPNUM; break;
                //case "TNUM": gvBind = gvTNUM; break;
                //case "PLATE_NO": gvBind = gvPlateNUM; break;
        }

        if (pageTotal == 0)
        {
            imgGrid_FirstPage.ImageUrl = "~/_imgs/page_FL0.gif";
            imgGrid_Previous.ImageUrl = "~/_imgs/page_L0.gif";
            imgGrid_Next.ImageUrl = "~/_imgs/page_R0.gif";

            return;
        }
        else if (pageTotal > 0)
        {
            imgGrid_FirstPage.ImageUrl = "~/_imgs/page_FL0.gif";
            imgGrid_Previous.ImageUrl = "~/_imgs/page_L0.gif";
            imgGrid_Next.ImageUrl = "~/_imgs/page_R1.gif";
        }

        gvBind.SelectedIndex = -1;
        gvBind.PageIndex = 0;

        gvBind.DataSource = dtLookup;
        gvBind.DataBind();

        lbGrid_PageIndex.Text = Convert.ToString(1);
        lbTotal.Text = string.Format("{0} - {1} of {2} ", "1", dtLookup.Rows.Count < 10 ? Convert.ToString(dtLookup.Rows.Count) : "10", hfTotal.Value);
    }//-- protected void imgGrid_FirstPage_Click(object sender, ImageClickEventArgs e) --

    protected void imgGrid_Previous_Click(object sender, ImageClickEventArgs e)
    {

        DataTable dtLookup = (DataTable)Session["dtLookup"];
        String Type = (string)Session["ParamsType"];
        Decimal pageTotal = Math.Ceiling(Convert.ToDecimal(dtLookup.Rows.Count) / grid_Page) - 1;

        GridView gvBind = new GridView();
        switch (Type)
        {
            //case "Simple": gvBind = gvContact; break;
            case "CLAIMNOA": gvBind = gvCZID; break;
                //case "PNUM": gvBind = gvPNUM; break;
                //case "TNUM": gvBind = gvTNUM; break;
                //case "PLATE_NO": gvBind = gvPlateNUM; break;
        }
        if (pageTotal == 0)
        {
            imgGrid_FirstPage.ImageUrl = "~/_imgs/page_FL0.gif";
            imgGrid_Previous.ImageUrl = "~/_imgs/page_L0.gif";
            imgGrid_Next.ImageUrl = "~/_imgs/page_R0.gif";

            return;
        }
        else if (gvBind.PageIndex == 0)
        {
            imgGrid_FirstPage.ImageUrl = "~/_imgs/page_FL0.gif";
            imgGrid_Previous.ImageUrl = "~/_imgs/page_L0.gif";
            imgGrid_Next.ImageUrl = "~/_imgs/page_R1.gif";

            return;
        }
        else if (gvBind.PageIndex - 1 == 0)
        {
            imgGrid_FirstPage.ImageUrl = "~/_imgs/page_FL0.gif";
            imgGrid_Previous.ImageUrl = "~/_imgs/page_L0.gif";
            imgGrid_Next.ImageUrl = "~/_imgs/page_R1.gif";
        }
        else
        {
            imgGrid_FirstPage.ImageUrl = "~/_imgs/page_FL1.gif";
            imgGrid_Previous.ImageUrl = "~/_imgs/page_L1.gif";
            imgGrid_Next.ImageUrl = "~/_imgs/page_R1.gif";
        }

        gvBind.SelectedIndex = -1;
        gvBind.PageIndex -= 1;

        gvBind.DataSource = dtLookup;
        gvBind.DataBind();

        lbGrid_PageIndex.Text = Convert.ToString(gvBind.PageIndex + 1);

        int _from = Int32.Parse(lbGrid_PageIndex.Text) * 10 - 9;
        int _to = (Int32.Parse(lbGrid_PageIndex.Text) * 10);

        lbTotal.Text = string.Format("{0} - {1} of {2} ", _from, _to, hfTotal.Value);
    }//-- protected void imgGrid_Previous_Click(object sender, ImageClickEventArgs e) --

    protected void imgGrid_Next_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtLookup = (DataTable)Session["dtLookup"];
        String Type = (string)Session["ParamsType"];
        Decimal pageTotal = Math.Ceiling(Convert.ToDecimal(dtLookup.Rows.Count) / grid_Page) - 1;

        GridView gvBind = new GridView();
        switch (Type)
        {
            //case "Simple": gvBind = gvContact; break;
            case "CLAIMNOA": gvBind = gvCZID; break;
                //case "PNUM": gvBind = gvPNUM; break;
                //case "TNUM": gvBind = gvTNUM; break;
                //case "PLATE_NO": gvBind = gvPlateNUM; break;
        }
        if (pageTotal == 0)
        {
            imgGrid_FirstPage.ImageUrl = "~/_imgs/page_FL0.gif";
            imgGrid_Previous.ImageUrl = "~/_imgs/page_L0.gif";
            imgGrid_Next.ImageUrl = "~/_imgs/page_R0.gif";

            return;
        }
        else if (gvBind.PageIndex == pageTotal)
        {
            imgGrid_FirstPage.ImageUrl = "~/_imgs/page_FL1.gif";
            imgGrid_Previous.ImageUrl = "~/_imgs/page_L1.gif";
            imgGrid_Next.ImageUrl = "~/_imgs/page_R0.gif";

            return;
        }
        else if (gvBind.PageIndex + 1 == pageTotal)
        {
            imgGrid_FirstPage.ImageUrl = "~/_imgs/page_FL1.gif";
            imgGrid_Previous.ImageUrl = "~/_imgs/page_L1.gif";
            imgGrid_Next.ImageUrl = "~/_imgs/page_R0.gif";
        }
        else
        {
            imgGrid_FirstPage.ImageUrl = "~/_imgs/page_FL1.gif";
            imgGrid_Previous.ImageUrl = "~/_imgs/page_L1.gif";
            imgGrid_Next.ImageUrl = "~/_imgs/page_R1.gif";
        }

        gvBind.SelectedIndex = -1;
        gvBind.PageIndex += 1;

        gvBind.DataSource = dtLookup;
        gvBind.DataBind();

        lbGrid_PageIndex.Text = Convert.ToString(gvBind.PageIndex + 1);

        int _from = Int32.Parse(lbGrid_PageIndex.Text) * 10 - 9;
        int _to = (Int32.Parse(lbGrid_PageIndex.Text) * 10) - Int32.Parse(hfTotal.Value) > 0 ? Int32.Parse(hfTotal.Value) : Int32.Parse(lbGrid_PageIndex.Text) * 10;

        lbTotal.Text = string.Format("{0} - {1} of {2} ", _from, _to, hfTotal.Value);
    }//-- protected void imgGrid_Next_Click(object sender, ImageClickEventArgs e) --

    protected void ChangeRowColor(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Get hold of the row and then the DataRow that it's being bound to
            GridViewRow row = e.Row;
            var value = row.FindControl("lbPNUMpolicystatuscode") as Label;
            // Look at the value and set the colour accordingly
            //switch (data.Field<string>("Type"))
            //{
            //    case "Assignment":
            //        row.BackColor = System.Drawing.Color.FromName("Blue");
            //        break;
            //    case "Exam":
            //        row.BackColor = System.Drawing.Color.FromName("Red");
            //        break;
            //}
            if (value.Text == "True")
            {
                //row.BackColor = System.Drawing.Color.FromName("Green");
                row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#008000");
            }
            if (value.Text == "False")
            {
                //row.BackColor = System.Drawing.Color.FromName("Red");
                row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            }
        }
    }


    protected void gvCZID_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void changMotorTypeauto (object sender, EventArgs e)
    {
        customercleansingIdvalue = Request.QueryString["customerid"];
        var txtValue = textSearch;

        if (customercleansingIdvalue != null && (string.IsNullOrWhiteSpace(txtValue.Text))) {
                int MotorType = Convert.ToInt32(motorTypeValue.SelectedValue);
                //textSearch.Text = customercleansingIdvalue;
                //ddlSearchView.Text = Server.HtmlEncode(customercleansingIdvalue);
                dt = new System.Data.DataTable();
                dt = QryInfo.QueryInfo_Policy(MotorType, 5, customercleansingIdvalue);
                showDataGrid(gvCZID, ParamsType.CLAIMNOA, dt);

        } else {
            ManaulSearch();
        }
        
    }

    protected void ProductsGridView_RowCommand(Object sender, GridViewCommandEventArgs e)
    {


        int index = Convert.ToInt32(e.CommandArgument);
        //GridViewRow gvrow = gvCZID.Rows[index];
        //Label Policy = (Label)gvCZID.Rows[index].Cells[3].FindControl("lbPNUMpolicynumber");
        //string PolicyS = Policy.Text.ToString();
        Label PolicyadditionalIdS = (Label)gvCZID.Rows[index].Cells[3].FindControl("lbPNUMAdditionalId");
        string PolicyadditionalId = PolicyadditionalIdS.Text.ToString();
        Label chdrnumS = (Label)gvCZID.Rows[index].Cells[3].FindControl("lbPNUMChdrnum");
        string chdrnum = chdrnumS.Text.ToString();
        Label zrennumS = (Label)gvCZID.Rows[index].Cells[3].FindControl("lbPNUMZrennum");
        string zrennum = zrennumS.Text.ToString();
        Label trannoS = (Label)gvCZID.Rows[index].Cells[3].FindControl("lbPNUMTrannum");
        string tranno = trannoS.Text.ToString();
        Label rsknoS = (Label)gvCZID.Rows[index].Cells[3].FindControl("lbPNUMRsknum");
        string rskno = rsknoS.Text.ToString();

        //Label 

        string queryString =

            "http://localhost:64804/PolicyDetail.aspx?chdrnum="

            + chdrnum + "&zrennum=" + zrennum + "&tranno=" + tranno + "&rskno=" + rskno + "&PolicyadditionalId=" + PolicyadditionalId;

        string newWin =

            "window.open('" + queryString + "');";

        ClientScript.RegisterStartupScript

            (this.GetType(), "pop", newWin, true);

    }

    protected string CallGenPage(object chdr,object zren ,object tran , object rsk)
    {

        string strSql = @"http://{0}/Production/documents/checkriskgenpage.asp?polno={1}&zrenno={2}&rskno={3}";

        strSql = string.Format(strSql
                ,"192.168.10.4"
                , chdr
                , zren
                , rsk            
                );

        return strSql;



    }

    protected void gvCZID_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }
}


