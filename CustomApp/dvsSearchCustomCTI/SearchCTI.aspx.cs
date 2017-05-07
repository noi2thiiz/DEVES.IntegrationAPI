using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using devesCustomCTI;
using devesCustomCTI.BusinessEntities;
using devesCustomCTI.BusinessLogic;
using devesCustomCTI.CRUDphonecall;

public partial class SearchCTI : System.Web.UI.Page
{
    public class ParamsType
    {
        public static string Simple = "Simple";
        public static string CZID = "CZID";
        public static string PNUM = "PNUM";
        public static string TNUM = "TNUM";
        public static string CSNUM = "CSNUM";
        public static string CLNUM = "CLNUM";
        public static string FirstNameTH = "FIRSTNAME_TH";
        public static string LastNameTH = "LASTNAME_TH";
        public static string FullNameTH = "FULLNAME_TH";
        public static string FirstNameENG = "FIRSTNAME_ENG";
        public static string LastNameENG = "LASTNAME_ENG";
        public static string FullNameENG = "FULLNAME_ENG";
        public static string PlateNo = "PLATE_NO";
        public static string ChassisNo = "CHASSIS_NO";
        public static string CaseNo = "CASE_NO";
        public static string Notidate = "NOTI_DATE";
        public static string ClaimNo = "CLAIM_NO";
        public static string MODE_CTI_CISCO = "1";
        public static string MODE_CTI_GENESY = "2";
        public static string MODE_SEARCH = "3";
        public static string MODE_MANAUL = "4";
    }
    public class ObjectTypeCode
    {
        public static int Account = 1;
        public static int Contact = 2;
        public static int Policy = 10002;
        public static int Case = 112;
    }
    private string MODE;
    private string MODELOG;
    private string citCallerId;
    private string handSetCode;
    private string telephoneNum;
    private string gettelephoneNum;
    private string voiceurl;
    private string ivr;
    private string CLID;
    private string CZID;
    private string CSNUM;
    private string PNUM;
    private string TNUM;
    const int grid_Page = 10;

    #region[web.config parameter]
    const string _NEW_SOURCE_Key = "NEW_SOURCE";
    const string _TOP_QUERY_Key = "TOP_QUERY";
    const string _SERVER_Key = "SERVER";
    const string _ORGANIZES_Key = "ORGANIZES";
    const string _URL_CONTACT_Key = "URL_CONTACT";
    const string _URL_PHONECALL_Key = "URL_PHONECALL";
    const string _PARAMS_Key = "PARAMS";
    const string _PARAMS_Empty = " - ";
    const string _URL_PARAMS_PHONECALL_Key = "URL_PARAMS_PHONECALL";
    const string _URL_PARAMS_PHONECALL_TMP_Key = "URL_PARAMS_PHONECALL_TMP";
    const string _USER_UNKNOWN_CALLER_Key = "USER_UNKNOWN_CALLER";
    const string _PRESSNUMBER_Key = "PRESSNUMBER";
    const string _URL_CUS_REGIS = "URL_CUSREGIS";
    #endregion

    private QueryInfo QryInfo = new QueryInfo();
    private System.Data.DataTable dt = new System.Data.DataTable();
    private void GetParams()
    {
        #region [Step I - Get Parameter]
        string[] Params = System.Configuration.ConfigurationManager.AppSettings[_PARAMS_Key].ToString().Split(':');
        //CLID = string.IsNullOrEmpty(Request.Params[Params[0]]) ? _PARAMS_Empty : Request.Params[Params[0]];
        CZID = string.IsNullOrEmpty(Request.Params[Params[1]]) ? _PARAMS_Empty : Request.Params[Params[1]];
        PNUM = string.IsNullOrEmpty(Request.Params[Params[2]]) ? _PARAMS_Empty : Request.Params[Params[2]];
        //TNUM = string.IsNullOrEmpty(Request.Params[Params[3]]) ? _PARAMS_Empty : ChkDigitTNUM(Request.Params[Params[3]]);
        MODE = string.IsNullOrEmpty(Request.Params[Params[4]]) ? _PARAMS_Empty : Request.Params[Params[4]];
        MODELOG = string.IsNullOrEmpty(Request.Params["ModeLog"]) ? MODE : Request.Params["ModeLog"];
        #endregion
        //lbCZID.Text = CZID;
    }
    private void ManaulSearch()
    {
        int typeSearch = Convert.ToInt32(ddlSearchView.SelectedIndex);
        dt = new System.Data.DataTable();
        if (!string.IsNullOrEmpty(textSearch.Text.Trim()))
        {
            switch (typeSearch)
            {
                case 0: dt = QryInfo.QueryInfo_Contact(ParamsType.CZID, textSearch.Text.Trim()); break;//Citizen Id
                case 1: dt = QryInfo.QueryInfo_Contact(ParamsType.FirstNameTH, textSearch.Text.Trim()); break;
                case 2: dt = QryInfo.QueryInfo_Contact(ParamsType.LastNameTH, textSearch.Text.Trim()); break;
                case 3: dt = QryInfo.QueryInfo_Contact(ParamsType.FullNameTH, textSearch.Text.Trim()); break;
                case 4: dt = QryInfo.QueryInfo_Contact(ParamsType.TNUM, textSearch.Text.Trim()); break;
                case 5: dt = QryInfo.QueryInfo_Policy(ParamsType.PNUM, textSearch.Text.Trim(),ddlSearchProdGroup.SelectedValue,chkExpired.Checked); break;//FullNameTH
                case 6: dt = QryInfo.QueryInfo_Policy(ParamsType.PlateNo, textSearch.Text.Trim(), ddlSearchProdGroup.SelectedValue, chkExpired.Checked); break;//FullNameTH
                case 7: dt = QryInfo.QueryInfo_Policy(ParamsType.ChassisNo, textSearch.Text.Trim(), ddlSearchProdGroup.SelectedValue, chkExpired.Checked); break;//FullNameTH                                                                                                                                                //case 10: dt = QryInfo.QueryInfo_Policy(ParamsType.ChassisNo, textSearch.Text.Trim(), ddlSearchPRD_GRP.SelectedValue, chkbIsExpired.Checked); break;//Chassis Number
                case 8: dt = QryInfo.QueryInfo_Case(ParamsType.CaseNo, textSearch.Text.Trim()); break;
                case 9: dt = QryInfo.QueryInfo_Case(ParamsType.Notidate, textSearch.Text.Trim()); break;
                case 10: dt = QryInfo.QueryInfo_Policy(ParamsType.ClaimNo, textSearch.Text.Trim(), ddlSearchProdGroup.SelectedValue, chkExpired.Checked); break;
            }//-- switch (TypeSearch) -
            if (typeSearch <= 4 && typeSearch <= 7)
            {
                showDataGrid(gvCZID, ParamsType.CZID, dt);
            }
            else if(typeSearch > 4 && typeSearch <= 7)
            {
                showDataGrid(gvPNUM, ParamsType.PNUM, dt);
            }
            else if(typeSearch >= 7 && typeSearch < 10)
            {
                showDataGrid(gvCase, ParamsType.CSNUM, dt);
            } 
            else
            {
                showDataGrid(gvClaim, ParamsType.CLNUM, dt);
            }
        }
        else
        {
            if (typeSearch <= 4)
                showDataGrid(gvCZID, ParamsType.CZID, dt);
            else
                showDataGrid(gvPNUM, ParamsType.PNUM, dt);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MODE = Request.QueryString["mode"];
            citCallerId = Request.QueryString["citCallerId"];
            handSetCode = Request.QueryString["handSetCode"];
            ivr = Request.QueryString["ivr"];
            voiceurl = Server.UrlDecode(Request.QueryString["VoiceURL"]);
            telephoneNum = Request.QueryString["telephoneNum"].Substring(1);
            hftelnum.Value = telephoneNum;
            hfivrlink.Value = ivr;
            hfhandcode.Value = handSetCode;
            hfvoiceurl.Value = voiceurl;
            //telephoneNum = Request.QueryString["telephoneNum"];
            
            if (telephoneNum != null)
            {
                dt = QryInfo.QueryInfo_Contact(ParamsType.TNUM,telephoneNum);
                int countrow = dt.Rows.Count;
                if (countrow == 1)
                {
                    string callerid = citCallerId;
                    string phonenumber = telephoneNum;
                    string callername = dt.Rows[0]["FullNameThai"].ToString();
                    string callercrmguid = dt.Rows[0]["ContactId"].ToString();
                    string czidnumber = dt.Rows[0]["CitizenId"].ToString();
                    fncreatePhonecalltemp(0, callercrmguid,callername,hftelnum.Value, hfvoiceurl.Value, hfhandcode.Value,"tnum");
                }
                else
                {
                    showDataGrid(gvCZID, ParamsType.CZID, dt);
                }
            }
        }
    }
    protected void onBtn_Ok_Click(object sender, EventArgs e)
    {
        #region [Varibles]
        GridView GVTmp = new GridView();
        string _ChkBox = "";
        string _hfOwnerPolicy = "";
        string _hfAccountPolicy = "";
        string _lbPhonenum = "";
        string _lbPolicyName = "";
        string _lbPolicyNumber = "";
        string _lbFullname = "";
        string _lbProducerCode = "";
        string _lbczid = "";
        string _lbcasenum = "";
        string _lbcasetitle = "";
        string _lbcasetype = "";
        string _lbcategory = "";
        string _lbsubcategory = "";
        string _lbclaimnoti = "";
        string _lbcasevip = "";
        string _lbnotidate = "";
        string _lbcusguid = "";
        string _lbcustype = "";
        string _lbpolicyadd = "";
        string _lbpolicyaddname = "";
        int _ObjTypeCode = 0;
        #endregion
        //CRUD_Phonecall_Temp createRecord = new CRUD_Phonecall_Temp();
        //createRecord.CreatePhonecallTemp();
       
        if (gvCZID.Visible)
        {
            GVTmp = gvCZID;
            _ChkBox = "checkboxContact";
            _ObjTypeCode = ObjectTypeCode.Contact;
            _lbFullname = "lbCZIDFullnameTH";
            _lbczid = "lbCZIDcitizenId";
            _lbPhonenum = "lbCZIDPhoneNumber";
            //_lbProducerCode = "lbCZIDProducerCode";

        }
        else if (gvPNUM.Visible)
        {
            GVTmp = gvPNUM;
            _ChkBox = "checkboxPolicy";
            _ObjTypeCode = ObjectTypeCode.Policy;
            _hfOwnerPolicy = "hfPNUMOwnerPolicy";
            _hfAccountPolicy = "hfPNUMAccountPolicy";
            _lbPolicyName = "lbPNUMpolicynumber";
            _lbPolicyNumber = "lbPNUMPolicyNumber";
            _lbFullname = "lbPNUMcontactname"; 
             _lbcusguid = "lbPNUMcontactid";
            _lbcustype = "lbPNUMcontacttype";
            _lbpolicyadd = "lbPNUMpolicyadd";
            _lbpolicyaddname = "lbPNUMpolicyaddname";
        }
        //else if (gvTNUM.Visible) { GVTmp = gvTNUM; _ChkBox = "chkTNUMAuthorizedPerson"; _ObjTypeCode = ObjectTypeCode.Contact; _lbFullname = "lbTNUMFullnameTH"; }
        else if (gvPlateNum.Visible)
        {
            GVTmp = gvPlateNum;
            _ChkBox = "checkboxPlatenum";
            _ObjTypeCode = ObjectTypeCode.Policy;
            _hfOwnerPolicy = "hfPlateNUMOwnerPolicy";
            _hfAccountPolicy = "hfPlateNUMAccountPolicy";
            _lbPolicyName = "lbPlateNUMInsuredName";
            _lbPolicyNumber = "lbPlateNUMPolicyNumber";
        }
        else if (gvCase.Visible)
        {
            GVTmp = gvCase;
            _ChkBox = "checkboxCase";
            _ObjTypeCode = ObjectTypeCode.Case;
            _lbcasenum = "lbCASEcasenumber";
            _lbcasetitle = "lbCASEcasetitle";
            _lbcasetype = "lbCASEcasetype";
            _lbcasevip = "lbCASEcasevip";
            _lbcategory = "lbCASEcategory";
            _lbsubcategory = "lbCASEsubcategory";
        }
        else if (gvClaim.Visible)
        {
            GVTmp = gvClaim;
            _ChkBox = "checkboxClaim";
            _ObjTypeCode = ObjectTypeCode.Policy;
            _hfOwnerPolicy = "hfPNUMOwnerPolicy";
            _hfAccountPolicy = "hfPNUMAccountPolicy";
            _lbPolicyName = "lbPNUMInsuredName";
            _lbPolicyNumber = "lbPNUMPolicyNumber";
        }

        foreach (GridViewRow GV in GVTmp.Rows)
        {
            CheckBox chkSelected = (CheckBox)GV.FindControl(_ChkBox);
            if (chkSelected != null)
            {
                if (chkSelected.Checked)
                {
                    string Primarykey = GVTmp.DataKeys[GV.RowIndex].Value.ToString();
                    if (MODE == ParamsType.MODE_SEARCH)
                    {
                        openPageCRM(_ObjTypeCode, Primarykey);

                    }
                    else
                    {
                        if (_ObjTypeCode == ObjectTypeCode.Policy)
                        {
                            HiddenField hfOwnerPolicy = (HiddenField)GV.FindControl(_hfOwnerPolicy);
                            HiddenField hfAccountPolicy = (HiddenField)GV.FindControl(_hfAccountPolicy);
                            Label lbPolicyName = (Label)GV.FindControl(_lbPolicyName);
                            Label lbPolicyNumber = (Label)GV.FindControl(_lbPolicyNumber);
                            Label lbcustomerid = (Label)GV.FindControl(_lbcusguid);
                            Label lbcustomername = (Label)GV.FindControl(_lbFullname);
                            Label lbcustomertype = (Label)GV.FindControl(_lbcustype);
                            Label lbpolicyadd = (Label)GV.FindControl(_lbpolicyadd);
                            Label lbpolicyaddname = (Label)GV.FindControl(_lbpolicyaddname);
                            
                            string callername = string.Format("{0}|{1}|{2}",
                                string.IsNullOrEmpty(lbcustomerid.Text) ? "" : lbcustomerid.Text,
                                string.IsNullOrEmpty(lbcustomername.Text) ? "" : lbcustomername.Text,
                                string.IsNullOrEmpty(lbcustomertype.Text) ? "" : lbcustomertype.Text);

                            string Policy = string.Format("{0}|{1}|{2}", 
                                Primarykey, 
                                string.IsNullOrEmpty(lbPolicyName.Text) ? "" : lbPolicyName.Text, 
                                "pfc_policy");

                            string Policyadd = string.Format("{0}|{1}|{2}", 
                                string.IsNullOrEmpty(lbpolicyadd.Text) ? "" : lbpolicyadd.Text,
                                string.IsNullOrEmpty(lbpolicyaddname.Text) ? "" : lbpolicyaddname.Text,
                                "pfc_policy_additional");
                            
                            fncreatePhonecalltemp(_ObjTypeCode, Policy,callername,hftelnum.Value, hfhandcode.Value, Policyadd,"pnum");
                            //openPageCRM_Phonecall(_ObjTypeCode, Primarykey, Sender, hfOwnerPolicy.Value, hfAccountPolicy.Value, lbPolicyNumber.Text, Policy, "");
                            //openPageCRM_Phonecall(_ObjTypeCode, Primarykey, Sender, hfOwnerPolicy.Value, hfAccountPolicy.Value, Policy);
                            
                        }//-- if (_ObjTypeCode == ObjectTypeCode.Policy) --
                        else if(_ObjTypeCode == ObjectTypeCode.Contact)
                        {
                            Label lbFullname = (Label)GV.FindControl(_lbFullname);
                            Label lbczidnumber = (Label)GV.FindControl(_lbczid);
                            Label lbphonenumber = (Label)GV.FindControl(_lbPhonenum);
                            string strlbProducerCode = string.IsNullOrEmpty(_lbProducerCode) ? "" : ((Label)GV.FindControl(_lbProducerCode)).Text;
                            //openPageCRM_Phonecall(_ObjTypeCode, Primarykey
                            //    , string.Format("{0}|{1}|{2}", Primarykey, string.IsNullOrEmpty(lbFullname.Text) ? "" : lbFullname.Text, "contact")
                            //, "||", "||", "", "||", strlbProducerCode);
                            string callerczid = string.Format("{0}|{1}|{2}", Primarykey, string.IsNullOrEmpty(lbFullname.Text) ? "" : lbFullname.Text, "contact");
                            fncreatePhonecalltemp(_ObjTypeCode, Primarykey, callerczid,hftelnum.Value, hfvoiceurl.Value,hfhandcode.Value, "czid");
                        }//-- else (_ObjTypeCode == ObjectTypeCode.Policy) --
                    }
                }
            }
        }
    }
    private void openPageCRM_Phonecall(int etc, string ObjID, string sender, string OwnerPolicy, string AccountPolicy, string Policy)
    {
        #region [Mapping New_source / MODE]
        string New_source = "";
        if (MODE == ParamsType.MODE_CTI_CISCO || MODE == ParamsType.MODE_CTI_GENESY || MODE == ParamsType.MODE_MANAUL)
        {
            string[] Params = System.Configuration.ConfigurationManager.AppSettings[_NEW_SOURCE_Key].ToString().Split(':');
            if (MODE == ParamsType.MODE_MANAUL) MODE = (Convert.ToInt32(MODE) - 1).ToString();
            New_source = Params[Convert.ToInt32(MODE) - 1];
        }//-- if (MODE == ParamsType.MODE_CTI_CISCO || MODE == ParamsType.MODE_CTI_GENESY) --
        #endregion
    
        #region [Create Phonecall Temp]
        BusCrm_PhoneCall_Temp prxy = new BusCrm_PhoneCall_Temp();
        prxy.pfc_source = New_source;
        //prxy.pfc_caller_id = "";
        //prxy.pfc_czid = lbCZID.Text != _PARAMS_Empty ? lbCZID.Text : "";
        prxy.pfc_relate_customerid = "";
        prxy.pfc_related_companyid = "";
        prxy.pfc_policyid = Policy.Split('|')[0];
        prxy.pfc_policyidname = Policy.Split('|')[1];
        prxy.pfc_policyidtype = Policy.Split('|')[2];
        prxy.pfc_form = sender.Split('|')[0];
        prxy.pfc_formName = sender.Split('|')[1];
        prxy.pfc_formType = sender.Split('|')[2];
        #endregion

        daResult ret = new BusCrm_PhoneCall_Temp().pfc_PhoneCall_TempCreateRecord(prxy);

        #region [Set Open Phone Call]
        string Url_ParamsPhoneCall = "";
        if (ret.ResultCode == daResult.EnumResultCode.OK)
        {
            Url_ParamsPhoneCall = string.Format(System.Configuration.ConfigurationManager.AppSettings[_URL_PARAMS_PHONECALL_TMP_Key].ToString()
                 , "" //contactInfo
                 , ret.ResultDescription//new_ctiparams_lookup
                 );
        }
        else
        {
            #region [Set Open Phone Call - OLD CODE]
            /* 
        string Url_ParamsPhoneCall = string.Format(System.Configuration.ConfigurationManager.AppSettings[_URL_PARAMS_PHONECALL_Key].ToString()
                , New_source //New_source
                , lbCLID.Text != _PARAMS_Empty ? lbCLID.Text : ""//new_caller_id
                , lbCZID.Text != _PARAMS_Empty ? lbCZID.Text : ""//new_czid
                , lbPNUM.Text != _PARAMS_Empty ? lbPNUM.Text : ""//new_pnum
                , lbTNUM.Text != _PARAMS_Empty ? TNUM : ""//phonenumber
                , lbTNUM.Text != _PARAMS_Empty ? System.Configuration.ConfigurationManager.AppSettings[_PRESSNUMBER_Key].ToString() + TNUM : ""//new_incoming_number
                , lbTNUM.Text != _PARAMS_Empty ? System.Configuration.ConfigurationManager.AppSettings[_PRESSNUMBER_Key].ToString() + TNUM : ""//new_outgoing_number
                , PolicyNumber
                , string.Format("{0}|{1}|{2}|{3}"
                    , sender.Split('|')[0] + "|" + HttpUtility.UrlEncodeUnicode(sender.Split('|')[1]) + "|" + sender.Split('|')[2]//sender
                    , OwnerPolicy.Split('|')[0] + "|" + HttpUtility.UrlEncodeUnicode(OwnerPolicy.Split('|')[1]) + "|" + OwnerPolicy.Split('|')[2]//OwnerPolicy
                    , AccountPolicy.Split('|')[0] + "|" + HttpUtility.UrlEncodeUnicode(AccountPolicy.Split('|')[1]) + "|" + AccountPolicy.Split('|')[2]//AccountPolicy
                    , Policy.Split('|')[0] + "|" + HttpUtility.UrlEncodeUnicode(Policy.Split('|')[1]) + "|" + Policy.Split('|')[2]//Policy
                    )//form(sender), new_relate_customerid(OwnerPolicy), new_related_companyid(AccountPolicy), new_policyid(Policy)
            );//-- string Url_ParamsPhoneCall = ... --

        string Url_PhoneCall = string.Format(System.Configuration.ConfigurationManager.AppSettings[_URL_PHONECALL_Key].ToString()
                , "", Url_ParamsPhoneCall);
        */
            #endregion

            Url_ParamsPhoneCall = string.Format(System.Configuration.ConfigurationManager.AppSettings[_URL_PARAMS_PHONECALL_Key].ToString()
                 , "" //3fcontactInfo
                 , New_source //New_source          
                 , ""              
                 //, lbCZID.Text != _PARAMS_Empty ? lbCZID.Text : ""
                 , ""
                 , ""
                 , ""
                 , ""
                 , string.Format("{0}|{1}|{2}|{3}"
                     , sender.Split('|')[0] + "|" + HttpUtility.UrlEncodeUnicode(sender.Split('|')[1]) + "|" + sender.Split('|')[2]//sender
                     , "||" //OwnerPolicy
                     , "||" //AccountPolicy
                     , Policy.Split('|')[0] + "|" + HttpUtility.UrlEncodeUnicode(Policy.Split('|')[1]) + "|" + Policy.Split('|')[2]//Policy
                     )//form(sender), new_relate_customerid(OwnerPolicy), new_related_companyid(AccountPolicy), new_policyid(Policy)
             );//-- string Url_ParamsPhoneCall = ... --
        }
        string Url_PhoneCall = string.Format(System.Configuration.ConfigurationManager.AppSettings[_URL_PHONECALL_Key].ToString(), Url_ParamsPhoneCall);
        #endregion

        #region [Set Close Window]
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        #region [Set Server Value]

        string CrossServer = Request.Url.Authority;
        switch (Request.Url.Authority)
        {
            //case "crm.bki.co.th": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "crm.bangkokinsurance.com": CrossServer = "crm.bki.co.th"; break;
            //case "172.16.2.44": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "172.16.2.45": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "172.16.2.48": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "172.16.2.65": CrossServer = "crm01-dev.bki.co.th"; break;
            //case "crm01-dev.bki.co.th": CrossServer = "172.16.2.65"; break;
            //case "crm01-umt.bki.co.th": CrossServer = "172.16.2.68"; break;
            //case "172.16.2.68": CrossServer = "crm01-umt.bki.co.th"; break;
            case "192.168.8.28": CrossServer = "crm.deves.co.th"; break;
            case "192.168.8.121": CrossServer = "crmqa.deves.co.th"; break;
            case "192.168.8.27": CrossServer = "adfs.deves.co.th"; break;
            case "localhost:51873": CrossServer = "192.168.8.122"; break;
            default: CrossServer = Request.Url.Authority; break;
        }

        string Server = System.Configuration.ConfigurationManager.AppSettings[_SERVER_Key].ToString();
        Server = string.Format(Server, CrossServer, System.Configuration.ConfigurationManager.AppSettings[_ORGANIZES_Key].ToString());
        #endregion

        sb.Append(@"var _Server = '" + Server + "';");
        sb.Append(@"_Server += '" + Url_PhoneCall.Replace("|", "%7C").Replace("'", "%27").Replace("\"", "%22").Replace("#", "%23") + "';");
        sb.Append(@"window.open(_Server,'_blank','height=' + ( screen.height - (screen.height*(10/100))) + ',width=' + screen.width + ',top=0,left=0,resizable=yes,status=yes');");

        //if (IsClosed == true)//Cancelled 30-Jan-2013
        //{
        sb.Append(@"    var ie7 = (document.all && !window.opera && window.XMLHttpRequest) ? true : false;
                                if (ie7) {window.open('','_parent',''); window.close(); }
                                else { this.focus(); self.opener = this; self.close(); }
                           ");
        //}//-- if (IsClosed == true) --
        #endregion

        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "myScript", sb.ToString(), true);
    }
    private void openPageCRM(int etc, string ObjID)
    {
        string Server = System.Configuration.ConfigurationManager.AppSettings[_SERVER_Key].ToString();
        //Server = string.Format(Server, Request.Url.Authority, System.Configuration.ConfigurationManager.AppSettings[_ORGANIZES_Key].ToString());
        Server = "https://internalcrmdev.deves.co.th/CRMDEV/";
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        string Url_Contact = string.Format(System.Configuration.ConfigurationManager.AppSettings[_URL_CONTACT_Key].ToString(), etc.ToString(), ObjID);
        sb.Append("window.open('" + Server + Url_Contact + "','_blank','height=' + ( screen.height - (screen.height*(10/100))) + ',width=' + screen.width + ',top=0,left=0,resizable=yes,status=yes');");
        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "myScript", sb.ToString(), true);
    }
    protected void onBtn_Search_Click(object sender, EventArgs e)
    {
        DataTable dtLookup = (DataTable)Session["dtLookup"];
        if (dtLookup != null)
        {
            String Type = (string)Session["ParamsType"];
            GridView gvBind = new GridView();
            switch (Type)
            {
                //case "Simple": gvBind = gvContact; break;
                case "CZID": gvBind = gvCZID; break;
                case "PNUM": gvBind = gvPNUM; break;
                    //case "TNUM": gvBind = gvTNUM; break;
                case "PLATE_NO": gvBind = gvPlateNum; break;
                case "CSNUM": gvBind = gvCase; break;
                case "CLNUM": gvBind = gvClaim; break;
            }
            gvBind.SelectedIndex = -1;
            gvBind.PageIndex = 0;
            gvBind.DataSource = dtLookup;
            gvBind.DataBind();
            lbGrid_PageIndex.Text = Convert.ToString(1);
            lbTotal.Text = string.Format("{0} - {1} of {2} (1 select)", "1", dtLookup.Rows.Count < 10 ? Convert.ToString(dtLookup.Rows.Count) : "10", hfTotal.Value);
        }
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
                                                                 new DataColumn("CitizenID"),
                                                                 new DataColumn("PhoneNumber")});
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
                dtLookup.Columns.AddRange(new DataColumn[] { new DataColumn("pfc_policyId",typeof(string)), new DataColumn("enabled",typeof(bool)),
                                                                 new DataColumn("CustomerId"),new DataColumn("CustomerName"),new DataColumn("CustomerType"),
                                                                 new DataColumn("policyadd"),new DataColumn("policyaddname"),
                                                                 new DataColumn("PolicyNumber"), new DataColumn("InsuredName"),
                                                                 new DataColumn("ProdType"), new DataColumn("ProdGroup"),
                                                                 new DataColumn("RegProvince"),new DataColumn("RegNumber"),
                                                                 new DataColumn("StartDate"),new DataColumn("EndDate"),
                                                                 new DataColumn("OwnerPolicy"),
                                                                 new DataColumn("RiskNumber"),new DataColumn("Deduct")});
                break;
            case "TNUM":
                dtLookup.Columns.AddRange(new DataColumn[] { new DataColumn("contactid",typeof(string)), new DataColumn("enabled",typeof(bool)) ,
                                                                 new DataColumn("FullNameThai"), new DataColumn("FullNameEng"),
                                                                 new DataColumn("HomePhone"),new DataColumn("BusinessPhone"),new DataColumn("MobilePhone") });
                break;
            case "PLATE_NO":
                dtLookup.Columns.AddRange(new DataColumn[] { new DataColumn("pfc_policyId",typeof(string)), new DataColumn("enabled",typeof(bool)),
                                                                 new DataColumn("PolicyNumber"), new DataColumn("InsuredName"),
                                                                 new DataColumn("Regnumber"),new DataColumn("RegProvince"),
                                                                 new DataColumn("StartDate"),new DataColumn("EndDate"),
                                                                 new DataColumn("OwnerPolicy"),new DataColumn("OwnerPolicyName"),
                                                                 new DataColumn("RiskNumber"),new DataColumn("Deduct")});
                break;
            case "CSNUM":
                dtLookup.Columns.AddRange(new DataColumn[] { new DataColumn("IncidentId",typeof(string)), new DataColumn("enabled",typeof(bool)) ,
                                                                 new DataColumn("CaseNumber"),
                                                                 new DataColumn("ClaimNoti"), 
                                                                 new DataColumn("CaseTitle"),
                                                                 new DataColumn("CaseType"),
                                                                 new DataColumn("Category"),
                                                                 new DataColumn("SubCategory"),
                                                                 new DataColumn("CaseVIP"),
                                                                 new DataColumn("NotiDate")});
                break;

            case "CLNUM":
                dtLookup.Columns.AddRange(new DataColumn[] { new DataColumn("pfc_policyId",typeof(string)), new DataColumn("enabled",typeof(bool)),
                                                                 new DataColumn("PolicyNumber"), new DataColumn("InsuredName"),
                                                                 new DataColumn("ProdType"), new DataColumn("ProdGroup"),
                                                                 new DataColumn("RegProvince"),new DataColumn("RegNumber"),
                                                                 new DataColumn("StartDate"),new DataColumn("EndDate"),
                                                                 new DataColumn("OwnerPolicy"),new DataColumn("ClaimNumber"),
                                                                 new DataColumn("ClaimOpenDate"),
                                                                 new DataColumn("DateofLoss")});
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
                                                      dt.Rows[i]["CitizenID"].ToString(),
                                                      dt.Rows[i]["PhoneNumber"].ToString()};
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
                    dr.ItemArray = new object[] { dt.Rows[i]["pfc_policyId"].ToString(),  true,
                                                      dt.Rows[i]["CustomerId"].ToString(),
                                                      dt.Rows[i]["CustomerName"].ToString(),
                                                      dt.Rows[i]["CustomerType"].ToString(),
                                                      dt.Rows[i]["policyadd"].ToString(),
                                                      dt.Rows[i]["policyaddname"].ToString(),
                                                      dt.Rows[i]["PolicyNumber"].ToString(),
                                                      dt.Rows[i]["InsuredName"].ToString(),
                                                      dt.Rows[i]["ProdType"].ToString(),
                                                      dt.Rows[i]["ProdGroup"].ToString(),
                                                      dt.Rows[i]["RegProvince"].ToString(),
                                                      dt.Rows[i]["RegNumber"].ToString(),
                                                      dt.Rows[i]["Startdate"].ToString(),
                                                      dt.Rows[i]["Enddate"].ToString(),
                                                      string.Format("{0}|{1}|{2}", dt.Rows[i]["OwnerPolicy"].ToString(), 
                                                      dt.Rows[i]["OwnerPolicyName"].ToString(), "contact"),                                                                               
                                                      dt.Rows[i]["RiskNumber"].ToString(),
                                                      dt.Rows[i]["Deduct"].ToString()};

                    //string.Format("{0}|{1}|{2}", dt.Rows[i]["new_AccountPolicy"].ToString(), dt.Rows[i]["new_AccountPolicyName"].ToString(), "account"), 
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
                    dr.ItemArray = new object[] { dt.Rows[i]["pfc_policyId"].ToString(),  true,
                                                      dt.Rows[i]["PolicyNumber"].ToString(),
                                                      dt.Rows[i]["InsuredName"].ToString(),
                                                      dt.Rows[i]["RegNumber"].ToString(),
                                                      dt.Rows[i]["RegProvince"].ToString(),
                                                      dt.Rows[i]["StartDate"].ToString(),
                                                      dt.Rows[i]["EndDate"].ToString(),
                                                      string.Format("{0}|{1}|{2}", dt.Rows[i]["OwnerPolicy"].ToString(),dt.Rows[i]["OwnerPolicyName"].ToString(), "contact"),                                                 
                                                      dt.Rows[i]["RiskNumber"].ToString(),
                                                      dt.Rows[i]["Deduct"].ToString()};
                    break;
                case "CSNUM":
                    dr.ItemArray = new object[] { dt.Rows[i]["IncidentId"].ToString(), true,
                                                     dt.Rows[i]["CaseNumber"].ToString(),
                                                     dt.Rows[i]["ClaimNoti"].ToString(),
                                                      dt.Rows[i]["CaseTitle"].ToString(),
                                                      dt.Rows[i]["CaseType"].ToString(),
                                                      dt.Rows[i]["Category"].ToString(),
                                                      dt.Rows[i]["SubCategory"].ToString(),
                                                      dt.Rows[i]["CaseVIP"].ToString(),
                                                      dt.Rows[i]["NotiDate"].ToString()};
                    break;
                case "CLNUM":
                    dr.ItemArray = new object[] { dt.Rows[i]["pfc_policyId"].ToString(),  true,                                
                                                      dt.Rows[i]["PolicyNumber"].ToString(),
                                                      dt.Rows[i]["InsuredName"].ToString(),
                                                      dt.Rows[i]["ProdType"].ToString(),
                                                      dt.Rows[i]["ProdGroup"].ToString(),
                                                      dt.Rows[i]["RegProvince"].ToString(),
                                                      dt.Rows[i]["RegNumber"].ToString(),
                                                      dt.Rows[i]["Startdate"].ToString(),
                                                      dt.Rows[i]["Enddate"].ToString(),
                                                      string.Format("{0}|{1}|{2}", dt.Rows[i]["OwnerPolicy"].ToString(),
                                                      dt.Rows[i]["OwnerPolicyName"].ToString(), "contact"),
                                                      dt.Rows[i]["ClaimNumber"].ToString(),
                                                      dt.Rows[i]["ClaimOpenDate"].ToString(),
                                                      dt.Rows[i]["DateofLoss"].ToString()};
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
                    case "CZID": dr.ItemArray = new object[] { "", false, "", "", "","","","","","","" }; break;
                    case "PNUM":
                        /*Cancelled 17-FEB-2013*/
                        /*dr.ItemArray = new object[] { "", false, "", "", "", "", "", "", "", "", "" }; */
                        dr.ItemArray = new object[] { "", false, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""};
                        break;
                    case "TNUM": dr.ItemArray = new object[] { "", false, "", "", "", "", "" }; break;
                    case "PLATE_NO": dr.ItemArray = new object[] { "", false, "", "", "", "", "", "", "", "", "", "" }; break;
                    case "CSNUM": dr.ItemArray = new object[] { "", false, "", "", "", "", "", "", "",""}; break;
                    case "CLNUM": dr.ItemArray = new object[] { "", false, "", "", "", "", "", "", "", "", "", "","" ,""}; break;
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
        gvPNUM.Visible = type.Equals(ParamsType.PNUM) ? true : false;
        //gvTNUM.Visible = type.Equals(ParamsType.TNUM) ? true : false;
        gvPlateNum.Visible = type.Equals(ParamsType.PlateNo) ? true : false;
        //btnNew.Visible = MODE.Equals(ParamsType.MODE_SEARCH) ? false : true;
        gvCase.Visible = type.Equals(ParamsType.CSNUM) ? true : false;
        gvClaim.Visible = type.Equals(ParamsType.CLNUM) ? true : false;
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
    #region[img page evt]
    protected void imgGrid_FirstPage_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtLookup = (DataTable)Session["dtLookup"];
        String Type = (string)Session["ParamsType"];
        Decimal pageTotal = Math.Ceiling(Convert.ToDecimal(dtLookup.Rows.Count) / grid_Page) - 1;

        GridView gvBind = new GridView();
        switch (Type)
        {
            //case "Simple": gvBind = gvContact; break;
            case "CZID": gvBind = gvCZID; break;
            case "PNUM": gvBind = gvPNUM; break;
                //case "TNUM": gvBind = gvTNUM; break;
            case "PLATE_NO": gvBind = gvPlateNum; break;
            case "CSNUM": gvBind = gvCase; break;
            case "CLNUM": gvBind = gvClaim; break;
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
        lbTotal.Text = string.Format("{0} - {1} of {2} (1 select)", "1", dtLookup.Rows.Count < 10 ? Convert.ToString(dtLookup.Rows.Count) : "10", hfTotal.Value);
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
            case "CZID": gvBind = gvCZID; break;
            case "PNUM": gvBind = gvPNUM; break;
                //case "TNUM": gvBind = gvTNUM; break;
            case "PLATE_NO": gvBind = gvPlateNum; break;
            case "CSNUM": gvBind = gvCase; break;
            case "CLNUM": gvBind = gvClaim; break;
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

        lbTotal.Text = string.Format("{0} - {1} of {2} (1 select)", _from, _to, hfTotal.Value);
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
            case "CZID": gvBind = gvCZID; break;
            case "PNUM": gvBind = gvPNUM; break;
                //case "TNUM": gvBind = gvTNUM; break;
            case "PLATE_NO": gvBind = gvPlateNum; break;
            case "CSNUM": gvBind = gvCase; break;
            case "CLNUM": gvBind = gvClaim; break;
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

        lbTotal.Text = string.Format("{0} - {1} of {2} (1 select)", _from, _to, hfTotal.Value);
    }
    #endregion
    protected void onBtn_New_Click(object sender, EventArgs e)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        var Url_cusregis = System.Configuration.ConfigurationManager.AppSettings[_URL_CUS_REGIS].ToString();
        #region [Set Server Value]

        string CrossServer = Request.Url.Authority;
        switch (Request.Url.Authority)
        {
            //case "crm.bki.co.th": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "crm.bangkokinsurance.com": CrossServer = "crm.bki.co.th"; break;
            //case "172.16.2.44": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "172.16.2.45": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "172.16.2.48": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "172.16.2.65": CrossServer = "crm01-dev.bki.co.th"; break;
            //case "crm01-dev.bki.co.th": CrossServer = "172.16.2.65"; break;
            //case "crm01-umt.bki.co.th": CrossServer = "172.16.2.68"; break;
            //case "172.16.2.68": CrossServer = "crm01-umt.bki.co.th"; break;
            case "192.168.8.28": CrossServer = "crm.deves.co.th"; break;
            case "192.168.8.121": CrossServer = "crmqa.deves.co.th"; break;
            case "192.168.8.27": CrossServer = "adfs.deves.co.th"; break;
            case "localhost:51873": CrossServer = "crmdev.deves.co.th"; break;
            case "crmappdev.deves.co.th": CrossServer = "crmdev.deves.co.th"; break;
            case "crmappqa.deves.co.th": CrossServer = "crmqa.deves.co.th"; break;
            default: CrossServer = Request.Url.Authority; break;
        }

        string Server = System.Configuration.ConfigurationManager.AppSettings[_SERVER_Key].ToString();
        //Server = string.Format(Server, CrossServer, System.Configuration.ConfigurationManager.AppSettings[_ORGANIZES_Key].ToString());
        Server = string.Format(Server, CrossServer);
        #endregion

        sb.Append(@"var _Server = '" + Server + "';");
        sb.Append(@"_Server += '" + Url_cusregis.Replace("|", "%7C").Replace("'", "%27").Replace("\"", "%22").Replace("#", "%23") + "';");
        sb.Append(@"window.open(_Server,'_blank','height=' + ( screen.height - (screen.height*(10/100))) + ',width=' + screen.width + ',top=0,left=0,resizable=yes,status=yes');");

        //if (IsClosed == true)//Cancelled 30-Jan-2013
        //{
        sb.Append(@"    var ie7 = (document.all && !window.opera && window.XMLHttpRequest) ? true : false;
                                console.log('codewindow');
                                if (ie7) {window.open('','_parent',''); window.close(); }
                                else { this.focus(); self.opener = this; self.close(); }
                            
                   ");
        //}//-- if (IsClosed == true) --
        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "myScript", sb.ToString(), true);
    }
    protected void onBtn_CreateRecord_Click(object sender, EventArgs e)
    {
        #region[prxy]
        CRUD_Phonecall_Temp prxy = new CRUD_Phonecall_Temp();
        prxy.pfc_firstname = textCreateName.Text.Trim();
        prxy.pfc_lastname = textCreateLastName.Text.Trim();
        prxy.pfc_phonenumber = numberCreatePhonenumber.Text.Trim();
        prxy.pfc_czid = numberCreateCitizenID.Text.Trim();
        #endregion
        var createbyprx = new CRUD_Phonecall_Temp();
        var createid = createbyprx.CreateIfnotfound(prxy);
    }
    protected void onBtn_CancleCreate_Click(object sender, EventArgs e)
    {
        CreateSection.Visible = false;
    }
    protected void ddlSearchView_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowDisplay_pnlSearchPolicyOther();
    }//--  protected void ddlSearchView_SelectedIndexChanged(object sender, EventArgs e) --

    private void ShowDisplay_pnlSearchPolicyOther()
    {
        Boolean blRes = int.Parse(ddlSearchView.SelectedItem.Value) > 5 && int.Parse(ddlSearchView.SelectedItem.Value) <= 7;
        ddlSearchProdGroup.Enabled = blRes;
        chkExpired.Enabled = blRes;

        ddlSearchProdGroup.SelectedValue = "99";
        if (!chkExpired.Checked) chkExpired.Checked = true;
    }

    private void fncreatePhonecalltemp(int etc, string ObjId, string caller, string phonenum,string voiceurl,string otherobject,string Type)
    {
        #region [Mapping New_source / MODE]
        string New_source = "";
        if (MODE == ParamsType.MODE_CTI_CISCO || MODE == ParamsType.MODE_CTI_GENESY || MODE == ParamsType.MODE_MANAUL)
        {
            string[] Params = System.Configuration.ConfigurationManager.AppSettings[_NEW_SOURCE_Key].ToString().Split(':');
            if (MODE == ParamsType.MODE_MANAUL) MODE = (Convert.ToInt32(MODE) - 1).ToString();
            New_source = Params[Convert.ToInt32(MODE) - 1];
        }//-- if (MODE == ParamsType.MODE_CTI_CISCO || MODE == ParamsType.MODE_CTI_GENESY) --
        #endregion
        CRUD_Phonecall_Temp prxy = new CRUD_Phonecall_Temp();
        switch (Type) {
        case "czid":
        #region[create phonecall temp czid]    
        prxy.pfc_source = New_source;
        prxy.pfc_form = ObjId;
        prxy.pfc_formentitytype = "Contact";
        prxy.pfc_formname = caller.Split('|')[1];
        prxy.pfc_phonenumber = phonenum;
        prxy.pfc_name = "phonecalltemp" + "คุณ" + caller.Split('|')[1];
        prxy.pfc_callerid = citCallerId;
        prxy.pfc_url = voiceurl;
        prxy.pfc_handcode = string.IsNullOrEmpty(hfhandcode.Value) ? "" : hfhandcode.Value;
        prxy.pfc_ivr = string.IsNullOrEmpty(hfivrlink.Value) ? "" :  hfivrlink.Value;
        #endregion
        break;
        case "tnum":
        #region [create phonecall temp TNUM]
        prxy.pfc_source = New_source;
        prxy.pfc_czid = "";
        prxy.pfc_form = ObjId;
        prxy.pfc_formentitytype = "Contact";
        prxy.pfc_formname = caller;
        prxy.pfc_phonenumber = phonenum;
        prxy.pfc_name = "phonecalltemp" + "คุณ"  + caller;
        prxy.pfc_callerid = citCallerId;
        prxy.pfc_url = voiceurl;
                prxy.pfc_handcode = string.IsNullOrEmpty(hfhandcode.Value) ? "" : hfhandcode.Value;
                prxy.pfc_ivr = string.IsNullOrEmpty(hfivrlink.Value) ? "" : hfivrlink.Value;
                #endregion
                break;
        case "pnum":
        #region [Create PNUM temp]
        prxy.pfc_form = caller.Split('|')[0];
        prxy.pfc_formname = caller.Split('|')[1];
        prxy.pfc_formentitytype = caller.Split('|')[2];
        prxy.pfc_policyid = ObjId.Split('|')[0];
        prxy.pfc_policyname = ObjId.Split('|')[1];
        prxy.pfc_policy_type = ObjId.Split('|')[2];
        prxy.pfc_policyaddid = otherobject.Split('|')[0];
        prxy.pfc_policyaddname = otherobject.Split('|')[1];
        prxy.pfc_policyadd_type = otherobject.Split('|')[2];
        prxy.pfc_callerid = citCallerId;
                prxy.pfc_name = "phonecalltemp คุณ" + caller.Split('|')[1];
                prxy.pfc_phonenumber = phonenum;
                prxy.pfc_url = hfvoiceurl.Value;
                prxy.pfc_handcode = string.IsNullOrEmpty(hfhandcode.Value) ? "" : hfhandcode.Value;
                prxy.pfc_ivr = string.IsNullOrEmpty(hfivrlink.Value) ? "" :  hfivrlink.Value;
                #endregion
                break;
            case "unknow":
                prxy.pfc_form = caller.Split('|')[0];
                prxy.pfc_formname = caller.Split('|')[1];
                prxy.pfc_formentitytype = caller.Split('|')[2];
                prxy.pfc_name = "phonecalltemp คุณ" + caller.Split('|')[1];
                prxy.pfc_callerid = citCallerId;
                prxy.pfc_url = hfvoiceurl.Value;
                prxy.pfc_handcode = string.IsNullOrEmpty(hfhandcode.Value) ? "" : hfhandcode.Value;
                prxy.pfc_ivr = string.IsNullOrEmpty(hfivrlink.Value) ? "" : hfivrlink.Value;
                break;   
        }
        var Createprxy = new CRUD_Phonecall_Temp();
        var phtid = Createprxy.CreatePhonecallTemp(prxy);
        string phtidstring = phtid.ToString();
        string Url_Params_Phonecall = string.Format(System.Configuration.ConfigurationManager.AppSettings[_URL_PARAMS_PHONECALL_Key].ToString(),phtidstring);
        string Url_PhoneCall = string.Format(System.Configuration.ConfigurationManager.AppSettings[_URL_PHONECALL_Key].ToString(), Url_Params_Phonecall);
        #region [Set Close Window]
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        #region [Set Server Value]

        string CrossServer = Request.Url.Authority;
        switch (Request.Url.Authority)
        {
            //case "crm.bki.co.th": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "crm.bangkokinsurance.com": CrossServer = "crm.bki.co.th"; break;
            //case "172.16.2.44": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "172.16.2.45": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "172.16.2.48": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "172.16.2.65": CrossServer = "crm01-dev.bki.co.th"; break;
            //case "crm01-dev.bki.co.th": CrossServer = "172.16.2.65"; break;
            //case "crm01-umt.bki.co.th": CrossServer = "172.16.2.68"; break;
            //case "172.16.2.68": CrossServer = "crm01-umt.bki.co.th"; break;
            case "192.168.8.28": CrossServer = "crm.deves.co.th"; break;
            case "192.168.8.121": CrossServer = "crmqa.deves.co.th"; break;
            case "192.168.8.27": CrossServer = "adfs.deves.co.th"; break;
            case "localhost:51873": CrossServer = "crmdev.deves.co.th"; break;
            case "crmappdev.deves.co.th": CrossServer = "crmdev.deves.co.th"; break;
            case "crmappqa.deves.co.th": CrossServer = "crmqa.deves.co.th"; break;
            default: CrossServer = Request.Url.Authority; break;
        }

        string Server = System.Configuration.ConfigurationManager.AppSettings[_SERVER_Key].ToString();
        //Server = string.Format(Server, CrossServer, System.Configuration.ConfigurationManager.AppSettings[_ORGANIZES_Key].ToString());
        Server = string.Format(Server, CrossServer);
        #endregion

        sb.Append(@"var _Server = '" + Server + "';");
        sb.Append(@"_Server += '" + Url_PhoneCall.Replace("|", "%7C").Replace("'", "%27").Replace("\"", "%22").Replace("#", "%23") + "';");
        sb.Append(@"window.open(_Server,'_blank','height=' + ( screen.height - (screen.height*(10/100))) + ',width=' + screen.width + ',top=0,left=0,resizable=yes,status=yes');");

        //if (IsClosed == true)//Cancelled 30-Jan-2013
        //{
        sb.Append(@"    var ie7 = (document.all && !window.opera && window.XMLHttpRequest) ? true : false;
                                console.log('codewindow');
                                if (ie7) {window.open('','_parent',''); window.close(); }
                                else { this.focus(); self.opener = this; self.close(); }
                            
                   ");
        //}//-- if (IsClosed == true) --
        #endregion

        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "myScript", sb.ToString(), true);
    }
    protected void onBtn_Clear_Click(object sender, EventArgs e)
    {
        textSearch.Text = "";
    }
    protected void onBtn_unknown_Click(object sender,EventArgs e)
    {
        string serverCreateUnknow = Request.Url.Authority;
        switch (Request.Url.Authority)
        {
            //case "crm.bki.co.th": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "crm.bangkokinsurance.com": CrossServer = "crm.bki.co.th"; break;
            //case "172.16.2.44": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "172.16.2.45": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "172.16.2.48": CrossServer = "crm.bangkokinsurance.com"; break;
            //case "172.16.2.65": CrossServer = "crm01-dev.bki.co.th"; break;
            //case "crm01-dev.bki.co.th": CrossServer = "172.16.2.65"; break;
            //case "crm01-umt.bki.co.th": CrossServer = "172.16.2.68"; break;
            //case "172.16.2.68": CrossServer = "crm01-umt.bki.co.th"; break;
            case "192.168.8.28": serverCreateUnknow = "crm.deves.co.th"; break;
            case "192.168.8.121": serverCreateUnknow = "crmqa.deves.co.th"; break;
            case "192.168.8.27": serverCreateUnknow = "adfs.deves.co.th"; break;
            case "localhost:51873": serverCreateUnknow = "crmdev.deves.co.th"; break;
            case "crmappdev.deves.co.th": serverCreateUnknow = "crmdev.deves.co.th"; break;
            case "crmappqa.deves.co.th": serverCreateUnknow = "crmqa.deves.co.th"; break;
            default: serverCreateUnknow = Request.Url.Authority; break;
        }
        if(serverCreateUnknow == "crmdev.deves.co.th")
        {
            string customer = string.Format("{0}|{1}|{2}", "83EE2007-282A-E711-80D6-0050568D1874", "ไม่ระบุ", "contact");
            fncreatePhonecalltemp(0, "", customer, "", "", "", "unknow");
        } else if (serverCreateUnknow == "crmqa.deves.co.th")
        {
            string customer = string.Format("{0}|{1}|{2}", "E9564956-2A2A-E711-80D4-0050568D615F", "ไม่ระบุ", "contact");
            fncreatePhonecalltemp(0, "", customer, "", "", "", "unknow");
        }

    }
}