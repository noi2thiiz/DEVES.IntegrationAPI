using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Discovery;
using devesSearch;


public partial class SearchClaim : System.Web.UI.Page
{


    public class ParamsType
    {
        public bool sortCheck = true;
        public static string ClaimId = "CLAIM_ID";
        public static string CustomerName = "CUSTOMER_NAME";
        public static string PlateNo = "PLATE_NO";
        public static string Province = "PROVINCE";
        public static string ClaimNo = "CLAIM_NO";
        public static string CLAIMNOA = "CLAIMNOA";
        public static string PolicyNo = "POLICY_NO";
        public static string ClaimOpenDate = "CLAIM_OPEN_DATE";
        public static string ClaimOpenTime = "CLAIM_OPEN_TIME";

    }
    public class ObjectTypeCode
    {
        public static int Account = 1;
        public static int Contact = 2;
        public static int Policy = 10002;
    }
    private string policyIdValue;
    private string MODE;
    private string MODELOG;
    private string CLAIMNOA;
    const int grid_Page = 10;

    #region[web.config parameter]
    const string _NEW_SOURCE_Key = "NEW_SOURCE";
    const string _TOP_QUERY_Key = "TOP_QUERY";
    const string _SERVER_Key = "SERVER";
    const string _ORGANIZES_Key = "ORGANIZES";
    const string _URL_CLAIM_KEY = "URL_CLAIM";
    const string _URL_PHONECALL_Key = "URL_PHONECALL";
    const string _PARAMS_Key = "PARAMS";
    const string _PARAMS_Empty = " - ";
    const string _URL_PARAMS_PHONECALL_Key = "URL_PARAMS_PHONECALL";
    const string _URL_PARAMS_PHONECALL_TMP_Key = "URL_PARAMS_PHONECALL_TMP";
    const string _USER_UNKNOWN_CALLER_Key = "USER_UNKNOWN_CALLER";

    const string _PRESSNUMBER_Key = "PRESSNUMBER";
    #endregion

    //#region [Check ButtonSort]

    //protected bool condition = true;
    //#endregion
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
        MODE = string.IsNullOrEmpty(Request.Params[Params[4]]) ? _PARAMS_Empty : Request.Params[Params[4]];
        MODELOG = string.IsNullOrEmpty(Request.Params["ModeLog"]) ? MODE : Request.Params["ModeLog"];
        #endregion
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        #region [Get URL String]
        if (!Page.IsPostBack)
        {
            policyIdValue = Request.QueryString["policyid"];
            if (policyIdValue != null)
            {
                dt = new System.Data.DataTable();
                dt = QryInfo.QueryInfo_Claim(ParamsType.PolicyNo, policyIdValue);
                showDataGrid(gvCZID, ParamsType.CLAIMNOA, dt);
            }

        }
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
                case 0: dt = QryInfo.QueryInfo_Claim(ParamsType.ClaimNo, textSearch.Text.Trim()); break;//Claim No
                case 1: dt = QryInfo.QueryInfo_Claim(ParamsType.CustomerName, textSearch.Text.Trim()); break;//Fullname
                case 2: dt = QryInfo.QueryInfo_Claim(ParamsType.PlateNo, textSearch.Text.Trim()); break;//Plate Number
                case 3: dt = QryInfo.QueryInfo_Claim(ParamsType.Province, textSearch.Text.Trim()); break;//Province
                case 4: dt = QryInfo.QueryInfo_Claim(ParamsType.PolicyNo, textSearch.Text.Trim()); break;//PolicyNo
            }
            //DataTable dtSortDate =  dt;
            showDataGrid(gvCZID, ParamsType.CLAIMNOA, dt);


        }
    }
    private void openPageCRM(string ClaimNoOP, string ClaimIdOP, string PolicyAddName, string PolicyAddId, string ClaimDateTime)
    {
        //OrganizationServiceProxy _serviceProxy;
        //string connectionstring = @"ServiceUri=https://internalcrmdev.deves.co.th/CRMDEV;Domain=dvs;Username=dvs\crmappadmin;Password=crmapp@2490";
        //CrmServiceClient client = new CrmServiceClient(connectionstring);
        //_serviceProxy = client.OrganizationServiceProxy;

        ////string Server = System.Configuration.ConfigurationManager.AppSettings[_SERVER_Key].ToString();
        ////Server = string.Format(Server, System.Configuration.ConfigurationManager.AppSettings[_ORGANIZES_Key].ToString());
        ////System.Text.StringBuilder sb = new System.Text.StringBuilder();

        ////string Url_Claim = string.Format(System.Configuration.ConfigurationManager.AppSettings[_URL_CLAIM_KEY].ToString(), ObjID);
        ////sb.Append("window.open('" + Server + Url_Claim + "','_blank','height=' + ( screen.height - (screen.height*(10/100))) + ',width=' + screen.width + ',top=0,left=0,resizable=yes,status=yes');");
        ////ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "myScript", sb.ToString(), true);

        #region [-- stamp value to crm form --]
        string strRet = @"  var ResObj = new Object();
                                var ClaimArray = new Array();
                                var PolicyArray = new Array();
                                var ClaimDate = '{0}'; 
                                ClaimArray[0] = '{1}';
                                ClaimArray[1] = '{2}';
                                ClaimArray[2] = 'pfc_claim';
                                PolicyArray[0] = '{3}';
                                PolicyArray[1] = '{4}';
                                PolicyArray[2] = 'pfc_policy_additional';

                                ResObj.new_claimid = ClaimArray; 
                                ResObj.new_policyid = PolicyArray;
                                ResObj.new_datetime = ClaimDate;
                                // ส่งค่าไปให้ indow.opener  โดย function รับ message จะอยู่ใน Web Resource: Function_Addcalltype
                                window.opener.postMessage(ResObj,'{5}');                  
                                window.returnValue = ResObj;                                                                                 
                             ";
        #endregion

        string strRes = string.Format(strRet
                                     , ClaimDateTime
                                     , ClaimIdOP
                                     , ClaimNoOP
                                     , PolicyAddName
                                     , PolicyAddId
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
        string _ibClaimNo = "";
        string _ibClaimOpenDateFull = "";
        string _lbCZIDPolicyAddName = "";
        string _lbCZIDPolicyAddId = "";
        #endregion
        if (gvCZID.Visible)
        {
            GVTmp = gvCZID; _ChkBox = "chkCZIDAuthorizedPerson"; _ibClaimNo = "lbCZIDClaimNo"; _ibClaimOpenDateFull = "lbCZIDClaimOpenDateFull";
            _lbCZIDPolicyAddName = "lbCZIDPolicyAddName"; _lbCZIDPolicyAddId = "lbCZIDPolicyAddId"; _ObjTypeCode = ObjectTypeCode.Contact;
        }
        string ClaimId = "";
        string ClaimNo = "";
        string ClaimDateTime = "";
        string PolicyId = "";
        string PolicyName = "";


        foreach (GridViewRow GV in GVTmp.Rows)
        {
            CheckBox chkSelected = (CheckBox)GV.FindControl(_ChkBox);
            if (chkSelected != null)
            {
                if (chkSelected.Checked)
                {
                    ClaimId = GVTmp.DataKeys[GV.RowIndex].Value.ToString();
                    System.Web.UI.WebControls.Label lbClaimName = (System.Web.UI.WebControls.Label)GV.FindControl(_ibClaimNo);
                    ClaimNo = lbClaimName.Text.ToString();
                    System.Web.UI.WebControls.Label lbClaimDateTimeFull = (System.Web.UI.WebControls.Label)GV.FindControl(_ibClaimOpenDateFull);
                    ClaimDateTime = lbClaimDateTimeFull.Text.ToString();
                    System.Web.UI.WebControls.Label lbClaimPolicyName = (System.Web.UI.WebControls.Label)GV.FindControl(_lbCZIDPolicyAddName);
                    PolicyName = lbClaimPolicyName.Text.ToString();
                    System.Web.UI.WebControls.Label lbClaimPolicyId = (System.Web.UI.WebControls.Label)GV.FindControl(_lbCZIDPolicyAddId);
                    PolicyId = lbClaimPolicyId.Text.ToString();
                    //ClaimNo = GV.Cells[(int)ENUM_COL.colClaimNo].Text;
                    //ClaimNo = GVTmp.SelectedRow.RowIndex.ToString();
                    //DataRowView drv = (DataRowView)GV.DataItem;
                    //ClaimNo = drv["ClaimNo"].ToString();
                    openPageCRM(ClaimNo, ClaimId, PolicyId, PolicyName, ClaimDateTime);
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
        //DataTable dtLookupSort = dt.Copy();
        //DataView dvSortedView = new DataView(dtLookupSort);
        //dvSortedView.Sort = "ClaimOpenDateFull DESC";
        //GridView gvBind = new GridView();
        //DataTable dtSortDate = dvSortedView.ToTable();
        //gvBind.DataSource = dtSortDate;


        switch (Type)
        {
            case "CLAIMNOA":
                dtLookup.Columns.AddRange(new DataColumn[] { new DataColumn("pfc_claimId",typeof(string)), new DataColumn("enabled",typeof(bool)) ,
                                                                 new DataColumn("ClaimNo"),
                                                                 new DataColumn("CustomerName"),
                                                                 new DataColumn("PlateNo"),
                                                                 new DataColumn("Province"),
                                                                 new DataColumn("PolicyNo"),
                                                                 new DataColumn("ClaimOpenDate"),
                                                                 new DataColumn("ClaimOpenTime"),
                                                                 new DataColumn("PolicyAddName"),
                                                                 new DataColumn("PolicyAddId"),
                                                                 new DataColumn("ClaimOpenDateFull")});

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
                    dr.ItemArray = new object[] { dt.Rows[i]["pfc_claimId"].ToString(), true,
                                                      dt.Rows[i]["ClaimNo"].ToString(),
                                                      dt.Rows[i]["CustomerName"].ToString(),
                                                      dt.Rows[i]["PlateNo"].ToString(),
                                                      dt.Rows[i]["Province"].ToString(),
                                                      dt.Rows[i]["PolicyNo"].ToString(),
                                                      dt.Rows[i]["ClaimOpenDate"].ToString(),
                                                      dt.Rows[i]["ClaimOpenTime"].ToString(),
                                                      dt.Rows[i]["PolicyAddName"].ToString(),
                                                      dt.Rows[i]["PolicyAddId"].ToString(),
                                                      dt.Rows[i]["ClaimOpenDateFull"]};

                    break;
            } //-- switch (Type) --}

            dtLookup.Rows.Add(dr);

        } //-- for (int i = 0; i < originalRow; i++) --
        Display(Type);

        /*Step 3. Set Display Gridview*/
        hfTotal.Value = originalRow.ToString();
        ////1-50 of 500+ (1 select)
        int TopQuery = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key]);
        lbTotal.Text = string.Format("{0} - {1} of {2} ", "1", dtLookup.Rows.Count < 10 ? originalRow.ToString() : "10", originalRow == TopQuery ? hfTotal.Value + '+' : hfTotal.Value);


        if ((originalRow % 10) > 0)
        {
            for (int i = 0; i < (10 - originalRow % 10); i++)
            {
                DataRow dr = dtLookup.NewRow();
                switch (Type)
                {
                    //case "CZID": dr.ItemArray = new object[] { "", false, "", "", "", "", "", "", "", "", "", "" }; break;
                    case "CLAIMNOA": dr.ItemArray = new object[] { "", false, "", "", "", "", "", "" }; break;
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

        GV.Columns[8].Visible = false;
        GV.Columns[9].Visible = false;
        GV.Columns[10].Visible = false;
        GV.DataSource = dtLookup;
        GV.DataBind();
        Session["dtLookup"] = dtLookup;
        Session["ParamsType"] = Type;

        lbGrid_PageIndex.Text = Convert.ToString(GV.PageIndex + 1);

        Decimal pageTotal = Math.Ceiling(Convert.ToDecimal(dtLookup.Rows.Count) / grid_Page) - 1;

        GV = gvCZID;

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
            if (DataBinder.Eval(e.Row.DataItem, "pfc_claimId").ToString() != "")
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


        gvBind = gvCZID;

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

        gvBind = gvCZID;

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

        gvBind = gvCZID;

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

        lbTotal.Text = string.Format("{0} - {1} of {2}", _from, _to, hfTotal.Value);
    }//-- protected void imgGrid_Next_Click(object sender, ImageClickEventArgs e) --




    protected void gvCZID_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}


