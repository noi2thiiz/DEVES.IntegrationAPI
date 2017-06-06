using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using devesSearch;


public partial class SearchCase : System.Web.UI.Page
{
    public class ParamsType
    {
        public static string CASENOA = "CASENOA";
        public static string CaseNo = "CASE_NO";
        public static string CustomerName = "CUSTOMER_NAME";
        public static string PolicyNo = "POLICY_NO";
        public static string PlateNo = "PLATE_NO";
        public static string Province = "PROVINCE";
        public static string ClaimNo = "CLAIM_NO";
        public static string DriverName = "DRIVER_NAME";
        public static string LostDate = "LOSTDATE";

    }
    public class ObjectTypeCode
    {
        public static int Account = 1;
        public static int Contact = 2;
        public static int Policy = 10002;
    }
    private string MODE;
    private string MODELOG;
    private string CZID;
    private string CASENOA;
    const int grid_Page = 10;

    #region[web.config parameter]
    const string _NEW_SOURCE_Key = "NEW_SOURCE";
    const string _TOP_QUERY_Key = "TOP_QUERY";
    const string _SERVER_Key = "SERVER";
    const string _ORGANIZES_Key = "ORGANIZES";
    const string _URL_CASE_KEY = "URL_CASE";
    const string _PARAMS_Key = "PARAMS";
    const string _PARAMS_Empty = " - ";

    const string _PRESSNUMBER_Key = "PRESSNUMBER";
    #endregion
    public static bool _check;
    private QueryInfo QryInfo = new QueryInfo();
    private System.Data.DataTable dt = new System.Data.DataTable();
    private void GetParams()
    {
        #region [Step I - Get Parameter]
        string[] Params = System.Configuration.ConfigurationManager.AppSettings[_PARAMS_Key].ToString().Split(':');
        //CLID = string.IsNullOrEmpty(Request.Params[Params[0]]) ? _PARAMS_Empty : Request.Params[Params[0]];
        CASENOA = string.IsNullOrEmpty(Request.Params[Params[1]]) ? _PARAMS_Empty : Request.Params[Params[1]];
        //PNUM = string.IsNullOrEmpty(Request.Params[Params[2]]) ? _PARAMS_Empty : Request.Params[Params[2]];
        //TNUM = string.IsNullOrEmpty(Request.Params[Params[3]]) ? _PARAMS_Empty : ChkDigitTNUM(Request.Params[Params[3]]);
        MODE = string.IsNullOrEmpty(Request.Params[Params[4]]) ? _PARAMS_Empty : Request.Params[Params[4]];
        MODELOG = string.IsNullOrEmpty(Request.Params["ModeLog"]) ? MODE : Request.Params["ModeLog"];
        #endregion
    }
    private void ManaulSearch()
    {
        int typeSearch = Convert.ToInt32(ddlSearchView.SelectedIndex);
        dt = new System.Data.DataTable();
        if (!string.IsNullOrEmpty(textSearch.Text.Trim()))
        {
            switch (typeSearch)
            {
                case 0: dt = QryInfo.QueryInfo_Case(ParamsType.CaseNo, textSearch.Text.Trim()); break;//Claim Number
                case 1: dt = QryInfo.QueryInfo_Case(ParamsType.CustomerName, textSearch.Text.Trim()); break;//Customer Name
                case 2: dt = QryInfo.QueryInfo_Case(ParamsType.PolicyNo, textSearch.Text.Trim()); break;//Plate Number
                case 3: dt = QryInfo.QueryInfo_Case(ParamsType.PlateNo, textSearch.Text.Trim()); break;//Province
                case 4: dt = QryInfo.QueryInfo_Case(ParamsType.Province, textSearch.Text.Trim()); break;//Claim Number
                case 5: dt = QryInfo.QueryInfo_Case(ParamsType.ClaimNo, textSearch.Text.Trim()); break;//Policy Number
                case 6: dt = QryInfo.QueryInfo_Case(ParamsType.DriverName, textSearch.Text.Trim()); break;//Driver Name
            }
            //DataTable dtSortDate =  dt;
            showDataGrid(gvCZID, ParamsType.CASENOA, dt);
        }
    }
    private void openPageCRM(int etc, string ObjID)
    {
        //OrganizationServiceProxy _serviceProxy;
        //string connectionstring = @"ServiceUri=https://internalcrmdev.deves.co.th/CRMDEV;Domain=dvs;Username=dvs\crmappadmin;Password=crmapp@2490";
        //CrmServiceClient client = new CrmServiceClient(connectionstring);
        //_serviceProxy = client.OrganizationServiceProxy;

        //if (_serviceProxy == null)
        //{
        //    Console.WriteLine("Cannot connect to CRM, connectionString");
        //}
        //else
        //{
        string Server = System.Configuration.ConfigurationManager.AppSettings[_SERVER_Key].ToString();
        Server = string.Format(Server, System.Configuration.ConfigurationManager.AppSettings[_ORGANIZES_Key].ToString());
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        string Url_Case = string.Format(System.Configuration.ConfigurationManager.AppSettings[_URL_CASE_KEY].ToString(), ObjID);
        sb.Append("window.open('" + Server + Url_Case + "','_blank','height=' + ( screen.height - (screen.height*(10/100))) + ',width=' + screen.width + ',top=0,left=0,resizable=yes,status=yes');");
        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "myScript", sb.ToString(), true);
        //}
    }
    protected void onBtn_Ok_Click(object sender, EventArgs e)
    {
        #region [Varibles]
        GridView GVTmp = new GridView();
        string _ChkBox = "";
        int _ObjTypeCode = 0;
        #endregion
        if (gvCZID.Visible)
        {
            GVTmp = gvCZID; _ChkBox = "chkCZIDAuthorizedPerson";
            _ObjTypeCode = ObjectTypeCode.Contact;
        }
        foreach (GridViewRow GV in GVTmp.Rows)
        {
            CheckBox chkSelected = (CheckBox)GV.FindControl(_ChkBox);
            if (chkSelected != null)
            {
                if (chkSelected.Checked)
                {
                    string Primarykey = GVTmp.DataKeys[GV.RowIndex].Value.ToString();
                    openPageCRM(_ObjTypeCode, Primarykey);


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
            case "CASENOA":
                dtLookup.Columns.AddRange(new DataColumn[] { new DataColumn("IncidentId",typeof(string)), new DataColumn("enabled",typeof(bool)) ,
                                                                 new DataColumn("CaseNo"),
                                                                 new DataColumn("CustomerName"),
                                                                 new DataColumn("ClaimNotiNo"),
                                                                 new DataColumn("PolicyNo"),
                                                                 new DataColumn("PlateNo"),
                                                                 new DataColumn("Province"),
                                                                 new DataColumn("ClaimNo"),
                                                                 new DataColumn("DriverName"),
                                                                 new DataColumn("LostDate")});
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
                case "CASENOA":
                    dr.ItemArray = new object[] { dt.Rows[i]["IncidentId"].ToString(), true,
                                                      dt.Rows[i]["CaseNo"].ToString(),
                                                      dt.Rows[i]["CustomerName"].ToString(),
                                                      dt.Rows[i]["ClaimNotiNo"].ToString(),
                                                      dt.Rows[i]["PolicyNo"].ToString(),
                                                      dt.Rows[i]["PlateNo"].ToString(),
                                                      dt.Rows[i]["Province"].ToString(),
                                                      dt.Rows[i]["ClaimNo"].ToString(),
                                                      dt.Rows[i]["DriverName"].ToString(),
                                                      dt.Rows[i]["LostDate"]};

                    break;
            } //-- switch (Type) --}

            dtLookup.Rows.Add(dr);

        } //-- for (int i = 0; i < originalRow; i++) --
        Display(Type);

        /*Step 3. Set Display Gridview*/
        hfTotal.Value = originalRow.ToString();
        ////1-50 of 500+ (1 select)
        int TopQuery = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key]);
        lbTotal.Text = string.Format("{0} - {1} of {2}", "1", dtLookup.Rows.Count < 10 ? originalRow.ToString() : "10", originalRow == TopQuery ? hfTotal.Value : hfTotal.Value);


        if ((originalRow % 10) > 0)
        {
            for (int i = 0; i < (10 - originalRow % 10); i++)
            {
                DataRow dr = dtLookup.NewRow();
                switch (Type)
                {
                    //case "CZID": dr.ItemArray = new object[] { "", false, "", "", "", "", "", "", "", "", "", "" }; break;
                    case "CASENOA": dr.ItemArray = new object[] { "", false, "", "", "", "", "", "" }; break;
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

        //Session["dtLookupSort"] = dtLookupSort;
        //condition = true;
        //DataView dvSortedView = new DataView(dt);
        //dvSortedView.Sort = "ClaimOpenDateFull DESC";
        //Session["dt2"] = dt;


    }
    private void Display(string type)
    {
        //gvContact.Visible = type.Equals(ParamsType.Simple) ? true : false;
        gvCZID.Visible = type.Equals(ParamsType.CASENOA) ? true : false;
        //gvPNUM.Visible = type.Equals(ParamsType.PNUM) ? true : false;
        //gvTNUM.Visible = type.Equals(ParamsType.TNUM) ? true : false;
        //gvPlateNUM.Visible = type.Equals(ParamsType.PlateNo) ? true : false;
        //btnNew.Visible = MODE.Equals(ParamsType.MODE_SEARCH) ? false : true;
    }
    protected void gvCZID_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "IncidentId").ToString() != "")
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
            case "CASENOA": gvBind = gvCZID; break;
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
        lbTotal.Text = string.Format("{0} - {1} of {2}", "1", dtLookup.Rows.Count < 10 ? Convert.ToString(dtLookup.Rows.Count) : "10", hfTotal.Value);
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
            case "CASENOA": gvBind = gvCZID; break;
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

        lbTotal.Text = string.Format("{0} - {1} of {2}", _from, _to, hfTotal.Value);
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
            case "CASENOA": gvBind = gvCZID; break;
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


}


