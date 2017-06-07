using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using devesSearch;

public partial class PolicyDetail : System.Web.UI.Page
{
    public class ParamsType
    {
        public static string CLAIMNOA = "CLAIMNOA";
    }

    private string chdrnum;
    private string zrennum;
    private string tranno;
    private string rskno;
    private string PolicyAdditionalId;

    const string _TOP_QUERY_Key = "TOP_QUERY";

    private QueryInfo QryInfo = new QueryInfo();
    private System.Data.DataTable dt = new System.Data.DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        #region [Get URL String]
        if (!Page.IsPostBack)
        {

            chdrnum = Request.QueryString["chdrnum"];
            zrennum = Request.QueryString["zrennum"];
            tranno = Request.QueryString["tranno"];
            rskno = Request.QueryString["rskno"];
            PolicyAdditionalId = Request.QueryString["PolicyAdditionalId"];

            //int MotorType = Convert.ToInt32(motorTypeValue.SelectedValue);
            if (chdrnum != null)
            {
                //ddlSearchView.Text = Server.HtmlEncode(customercleansingIdvalue);
                dt = new System.Data.DataTable();
                dt = QryInfo.QueryInfo_Policy(0, 5, "สมาน");
                showDataGrid(gvCZID, ParamsType.CLAIMNOA, dt);
                //getmorePolicyDetail( chdrnum,  zrennum, tranno, rskno, PolicyAdditionalId);
            }

        }
        #endregion
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

    private void getmorePolicyDetail (string chdr , string zren , string tran , string rskno , string policyGUID)
    {
       //---------------------------------//


        return;
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

    }
}