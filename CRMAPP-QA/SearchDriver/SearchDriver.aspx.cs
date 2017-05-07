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
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Discovery;
using devesSearchDriver;


public partial class SearchDriver : System.Web.UI.Page
{

    public class ParamsType
    {
        public static string CLAIMNOA = "CLAIMNOA";
        public static string CustomerClientNo = "CUSTOMER_CLIENT_NO";
        public static string CustomerName = "CUSTOMER_NAME";
        public static string CitizenID = "CITIZEN_ID";




    }
    public class ObjectTypeCode
    {
        public static int Account = 1;
        public static int Contact = 2;
        public static int Policy = 10002;
    }
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
    private void ManaulSearch()
    {
        int typeSearch = Convert.ToInt32(ddlSearchView.SelectedIndex);
        dt = new System.Data.DataTable();
        if (!string.IsNullOrEmpty(textSearch.Text.Trim()))
        {
            switch (typeSearch)
            {
                case 0: dt = QryInfo.QueryInfo_Contact(ParamsType.CustomerName, textSearch.Text.Trim()); break;//Informer Name
                case 1: dt = QryInfo.QueryInfo_Contact(ParamsType.CustomerClientNo, textSearch.Text.Trim()); break;//Informer Number
                case 2: dt = QryInfo.QueryInfo_Contact(ParamsType.CitizenID, textSearch.Text.Trim()); break;//Case Number

            }
            //DataTable dtSortDate =  dt;
            checkDataSearch(gvCZID, ParamsType.CLAIMNOA, dt);


        }
    }
    private void openPageCRM(string CustomerName, string Primarykey)
    {
        //OrganizationServiceProxy _serviceProxy;
        //string connectionstring = @"ServiceUri=https://internalcrmdev.deves.co.th/CRMDEV;Domain=dvs;Username=dvs\crmappadmin;Password=crmapp@2490";
        //CrmServiceClient client = new CrmServiceClient(connectionstring);
        //_serviceProxy = client.OrganizationServiceProxy;

        ////string Server = System.Configuration.ConfiguUrl_Claim + "','_blank','height=' + ( screen.height - (screen.height*(10/100))) + ',width=' + screen.width + ',top=0,left=0,resizable=yes,status=yes');");
        ////ScriptManager.RegisterStartupScript(thisrationManager.AppSettings[_SERVER_Key].ToString();
        ////Server = string.Format(Server, System.Configuration.ConfigurationManager.AppSettings[_ORGANIZES_Key].ToString());
        ////System.Text.StringBuilder sb = new System.Text.StringBuilder();

        ////string Url_Claim = string.Format(System.Configuration.ConfigurationManager.AppSettings[_URL_CLAIM_KEY].ToString(), ObjID);
        ////sb.Append("window.open('" + Server + .Page, typeof(Page), "myScript", sb.ToString(), true);

        #region [-- stamp value to crm form --]
        string strRet = @"  var ResObj = new Object();
                                var DriverArray = new Array();
                                DriverArray[0] = '{0}';
                                DriverArray[1] = '{1}';
                                DriverArray[2] = 'contact';
                                

                                ResObj.new_driverid = DriverArray; 
                                // ส่งค่าไปให้ indow.opener  โดย function รับ message จะอยู่ใน Web Resource: Function_Addcalltype
                                window.opener.postMessage(ResObj,'{2}');                  
                                window.returnValue = ResObj;                                                                                 
                             ";
        #endregion

        string strRes = string.Format(strRet
                                     , Primarykey
                                     , CustomerName
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
        string _lbCZIDCustomerName = "";
        #endregion
        if (gvCZID.Visible)
        {
            GVTmp = gvCZID; _ChkBox = "chkCZIDAuthorizedPerson"; _lbCZIDCustomerName = "lbCZIDCustomerName";
            _ObjTypeCode = ObjectTypeCode.Contact;
        }
        string CustomerName = "";


        foreach (GridViewRow GV in GVTmp.Rows)
        {
            CheckBox chkSelected = (CheckBox)GV.FindControl(_ChkBox);
            if (chkSelected != null)
            {
                if (chkSelected.Checked)
                {
                    System.Web.UI.WebControls.Label lbIncustomerName = (System.Web.UI.WebControls.Label)GV.FindControl(_lbCZIDCustomerName);
                    CustomerName = lbIncustomerName.Text.ToString();
                    string Primarykey = GVTmp.DataKeys[GV.RowIndex].Value.ToString();

                    //ClaimNo = GV.Cells[(int)ENUM_COL.colClaimNo].Text;
                    //ClaimNo = GVTmp.SelectedRow.RowIndex.ToString();
                    //DataRowView drv = (DataRowView)GV.DataItem;
                    //ClaimNo = drv["ClaimNo"].ToString();
                    openPageCRM(CustomerName, Primarykey);
                }
            }
        }

    }

    protected void onBtn_Search_Click(object sender, EventArgs e)
    {

        ManaulSearch();
    }
    //private void checkDataSearch(GridView GV, string Type, System.Data.DataTable dt)
    //{
    //    if (dt.Rows.Count != 0)
    //    {
    //        showDataGrid(gvCZID, ParamsType.CLAIMNOA, dt);
    //    }

    //    else if (dt.Rows.Count == 0)
    //    {
    //        //wait to call claim service.
    //    }
    //}
    private void checkDataSearch(GridView GV, string Type, System.Data.DataTable dt)
    {
        if (dt.Rows.Count != 0)
        {
            showDataGrid(gvCZID, ParamsType.CLAIMNOA, dt);
        }

        else if (dt.Rows.Count == 0)
        {
            //wait to call claim service.
        }
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


        dtLookup.Columns.AddRange(new DataColumn[] { new DataColumn("ContactId",typeof(string)), new DataColumn("enabled",typeof(bool)) ,
                                                                 new DataColumn("CustomerName"),
                                                                 new DataColumn("CustomerClientNo"),
                                                                 new DataColumn("CitizenID"),
                                                                 new DataColumn("VIP"),
                                                                 new DataColumn("Privilege"),
                                                                 new DataColumn("Sensitive")});

        /*Step 2. Add Data*/
        int originalRow = dt.Rows.Count;
        //btnOK.Enabled = originalRow > 0 ? true : false;
        for (int i = 0; i < originalRow; i++)
        {
            System.Data.DataRow dr = dtLookup.NewRow();

            dr.ItemArray = new object[] { dt.Rows[i]["ContactId"].ToString(), true,
                                                      dt.Rows[i]["CustomerName"].ToString(),
                                                      dt.Rows[i]["CustomerClientNo"].ToString(),
                                                      dt.Rows[i]["CitizenID"].ToString(),
                                                      dt.Rows[i]["VIP"].ToString(),
                                                      dt.Rows[i]["Privilege"].ToString(),
                                                      dt.Rows[i]["Sensitive"].ToString()};
            //-- switch (Type) --}
            
            dtLookup.Rows.Add(dr);
            //-- transfer value optionset Gridview --}
            foreach (DataRow row in dtLookup.Rows)
            {
                {
                    #region [Check Privilege]
                    if (row["Privilege"].ToString() == null)
                    {
                        row["Privilege"] = "-";
                    }
                    else if (row["Privilege"].ToString() == "100000001")
                    {
                        row["Privilege"] = "Normal";
                    }
                    else if (row["Privilege"].ToString() == "100000002")
                    {
                        row["Privilege"] = "Silver";
                    }
                    else if (row["Privilege"].ToString() == "100000003")
                    {
                        row["Privilege"] = "Gold";
                    }
                    else if (row["Privilege"].ToString() == "100000004")
                    {
                        row["Privilege"] = "Diamond";
                    }
                    #endregion
                }
                    {
                        #region [Check VIP]
                        if (row["VIP"].ToString() == null)
                        {
                            row["VIP"] = "-";
                        }
                        else if (row["VIP"].ToString() == "False")
                        {
                            row["VIP"] = "Normal";
                        }
                        else if (row["VIP"].ToString() == "True")
                        {
                            row["VIP"] = "VIP";
                        }

                        #endregion
                    }
                {
                    #region [Check Sensitive]
                    if (row["Sensitive"].ToString() == null)
                    {
                        row["Sensitive"] = "-";
                    }
                    else if (row["Sensitive"].ToString() == "100000000")
                    {
                        row["Sensitive"] = "Low";
                    }
                    else if (row["Sensitive"].ToString() == "100000001")
                    {
                        row["Sensitive"] = "Medium";
                    }
                    else if (row["Sensitive"].ToString() == "100000002")
                    {
                        row["Sensitive"] = "High";
                    }
                    #endregion
                }
            }
        } //-- for (int i = 0; i < originalRow; i++) --
        Display(Type);

        /*Step 3. Set Display Gridview*/
        hfTotal.Value = originalRow.ToString();
        ////1-50 of 500+ (1 select)
        int TopQuery = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[_TOP_QUERY_Key]);
        lbTotal.Text = string.Format("{0} - {1} of {2} ", "1", dtLookup.Rows.Count < 10 ? originalRow.ToString() : "10", originalRow == TopQuery ? hfTotal.Value  : hfTotal.Value);


        if ((originalRow % 10) > 0)
        {
            for (int i = 0; i < (10 - originalRow % 10); i++)
            {
                DataRow dr = dtLookup.NewRow();

                dr.ItemArray = new object[] { "", false, "", "", "", "", "", "" };

                //dr["enabled"] = false;
                dtLookup.Rows.Add(dr);
            }
        }
        //-- if ((originalRow % 10) > 0) --


        GV.Columns[3].Visible = false;
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
            if (DataBinder.Eval(e.Row.DataItem, "ContactId").ToString() != "")
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

        lbTotal.Text = string.Format("{0} - {1} of {2} ", _from, _to, hfTotal.Value);
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

        lbTotal.Text = string.Format("{0} - {1} of {2} ", _from, _to, hfTotal.Value);
    }//-- protected void imgGrid_Next_Click(object sender, ImageClickEventArgs e) --



    protected void gvCZID_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}


