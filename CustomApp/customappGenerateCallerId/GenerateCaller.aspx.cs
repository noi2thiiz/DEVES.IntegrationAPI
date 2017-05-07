using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using devescustomGenerateCallerId;

public partial class _Default : System.Web.UI.Page
{
    private const string posturl = "posturl";
    #region [parameter]
    private string guid;
    private string ticketno;
    private string currentuser;
    private string claimnotinumber;
    #endregion
    private QueryInfo QryInfo = new QueryInfo();
    private System.Data.DataTable dt = new System.Data.DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        dt = new System.Data.DataTable();
        
        if (!Page.IsPostBack)
        {
            guid = Request.QueryString["guid"];
            ticketno = Request.QueryString["ticketno"];
            currentuser = Request.QueryString["currentuser"];
            dt = QryInfo.Queryinfo_CallerId(guid, ticketno, currentuser);
            claimnotinumber = dt.Rows[0]["@resultDesc"].ToString();
            sendbackClaim(claimnotinumber);
        }
    }
    private void sendbackClaim(string claimnoti)
    {
        if (claimnoti != null)
        {
            string strRet = @"var RetObj = new Object();
                             RetObj.claimnotinumber = '{0}';
                             window.opener.postMessage(RetObj,'{1}');";
            string strRes = string.Format(strRet, claimnoti, System.Configuration.ConfigurationManager.AppSettings[posturl].ToString());
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
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "alert('กรุณากรอกข้อมูลให้ครบถ้วน');", true);
        }
    }
}