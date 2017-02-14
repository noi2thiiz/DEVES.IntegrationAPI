using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DEVES.IntegrationAPI.WebApi.Controllers;
using DEVES.IntegrationAPI;

namespace DEVES.IntegrationAPI.WebApi
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void button_send_Click(object sender, EventArgs e)
        {
            // Get GUID vaule from user
            string text_Guid = txt_guid.Text.ToString();
            // Get ClaimID vaule from user
            string text_ClaimID = txt_ClaimID.Text.ToString();

            // Show result of GUID
            label_guid.Text = "Guid: " + text_Guid;
            // Show result of ClaimID
            label_ClaimID.Text = "Claim ID: " + text_ClaimID;
        }

        protected void txt_guid_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}