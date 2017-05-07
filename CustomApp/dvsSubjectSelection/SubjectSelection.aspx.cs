using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Http.Cors;
using System.Web.Http;

namespace SubjectSelection
{

    public partial class SubjectSelection : System.Web.UI.Page
    {
        private const string posturl = "posturl";
        public static void Register(HttpConfiguration config)
        {
            var corsAttr = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsAttr);
        }
        List<Category> categories = new List<Category>();
        List<SubCategory> subCategories = new List<SubCategory>();
        List<SubjectInformation> subjectInformations = new List<SubjectInformation>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                #region[--Call Type--]
                string etc = Request.QueryString["etc"];
                string directioncode = Request.QueryString["directioncode"];
                hfetc.Value = etc;
                hfdrc.Value = directioncode;
                if(hfdrc.Value == "true")
                {
                    rbCaller.Items[0].Selected = true;
                } else
                {
                    rbCaller.Items[1].Selected = true;
                }
                if(hfetc.Value == "4210")
                {
                    this.rbCaller.Enabled = false;
                    this.rbCaller.Attributes.Remove("OnSelectedIndexChanged");
                }
                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + hfetc.Value + "');", true);
                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + hfurl.Value + "');", true);
                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert(ret);", true);
                string sqlCallType = @"select pfc_calltypeId, pfc_calltype_name
                                from pfc_calltypeBase a with (nolock)
                                where statecode=0 and statuscode=1 and pfc_do_not_show = 0 
								order by a.pfc_calltype_code";

                DataSet dsCallType = DAL.executeDataSet(sqlCallType);
                ViewState["CallType"] = dsCallType;
                lbCallType.DataSource = dsCallType;
                lbCallType.DataTextField = "pfc_calltype_name";
                lbCallType.DataValueField = "pfc_calltypeId";
                lbCallType.DataBind();

                #endregion

                #region[--Category--]
                string sqlCategory = @"select pfc_categoryId, pfc_category_name, pfc_calltypeId
                                from pfc_categoryBase a with (nolock)
                                where statecode=0 and statuscode=1 and pfc_do_not_show = 0
								order by a.pfc_category_code";
                DataSet dsCategory = DAL.executeDataSet(sqlCategory);
                foreach (DataRow dtr in dsCategory.Tables[0].Rows)
                {
                    Category cat = new Category();
                    cat.CategotyId = dtr["pfc_categoryId"].ToString();
                    cat.CategotyName = dtr["pfc_category_name"].ToString();
                    cat.CallType = dtr["pfc_calltypeId"].ToString();

                    categories.Add(cat);
                }

                ViewState["categories"] = categories;
                #endregion

                #region[--Sub Category--]
                string sqlSubCategory = @"select pfc_sub_categoryId, pfc_sub_category_name, pfc_categoryId
                                    from pfc_sub_categoryBase a with (nolock)
                                    where statecode=0 and statuscode=1 and pfc_do_not_show = 0
									order by a.pfc_sub_category_code";
                DataSet dsSubCategory = DAL.executeDataSet(sqlSubCategory);
                foreach (DataRow dtr in dsSubCategory.Tables[0].Rows)
                {
                    SubCategory subCat = new SubCategory();
                    subCat.SubCategotyId = dtr["pfc_sub_categoryId"].ToString();
                    subCat.SubCategotyName = dtr["pfc_sub_category_name"].ToString();
                    subCat.CategotyId = dtr["pfc_categoryId"].ToString();

                    subCategories.Add(subCat);
                }

                ViewState["subcategories"] = subCategories;
                #endregion

                #region[--Subject Information--]
                //string sqlSubjectInformation = @"select new_Scripting, new_Note, new_More_Information, new_Max_Working_Day, new_Call_TypeIDName, new_Categoryid, new_Sub_CategoryID
                //                       from new_subject_information
                //                       where statecode=0 and statuscode=1";
                //DataSet dsSubjectInformation = DAL.executeDataSet(sqlSubjectInformation);
                //foreach (DataRow dtr in dsSubjectInformation.Tables[0].Rows)
                //{
                //    SubjectInformation subjectInformation = new SubjectInformation();
                //    subjectInformation.Scripting = dtr["new_Scripting"].ToString();
                //    subjectInformation.Note = dtr["new_Note"].ToString();
                //    subjectInformation.MoreInformation = dtr["new_More_Information"].ToString();
                //    subjectInformation.MaxWorkingDay = dtr["new_Max_Working_Day"].ToString();
                //    subjectInformation.CallTypeIDName = dtr["new_Call_TypeIDName"].ToString();
                //    subjectInformation.CategoryId = dtr["new_Categoryid"].ToString();
                //    subjectInformation.SubCategoryId = dtr["new_Sub_CategoryID"].ToString();

                //    subjectInformations.Add(subjectInformation);
                //}

                //ViewState["subjectInformations"] = subjectInformations;
                #endregion

                clearData();
            }
        }

        protected void lbCallType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["categories"] != null)
            {
                List<Category> categories = (List<Category>)ViewState["categories"];
                lbCategory.DataSource = categories.Where(t => t.CallType == lbCallType.SelectedValue).ToList();
                lbCategory.DataTextField = "CategotyName";
                lbCategory.DataValueField = "CategotyId";
                lbCategory.DataBind();
            }

            //Clear Value
            lbSubCategory.Items.Clear();
            clearData();
        }

        protected void lbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["subcategories"] != null)
            {
                List<SubCategory> subcategories = (List<SubCategory>)ViewState["subcategories"];
                lbSubCategory.DataSource = subcategories.Where(t => t.CategotyId == lbCategory.SelectedValue).ToList();
                lbSubCategory.DataTextField = "SubCategotyName";
                lbSubCategory.DataValueField = "SubCategotyId";
                lbSubCategory.DataBind();
            }

            //Clear Vlaue
            clearData();
        }
        protected void callerOnCheck(object sender, EventArgs e)
        {
            string callerSelectedvalue = rbCaller.SelectedValue;
            hfdrc.Value = callerSelectedvalue;  

        }
        protected void lbSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["subjectInformations"] != null)
            {
                List<SubjectInformation> subjectInformations = (List<SubjectInformation>)ViewState["subjectInformations"];
                SubjectInformation subjectInformation = subjectInformations.Where(t => t.SubCategoryId == lbSubCategory.SelectedValue).FirstOrDefault();

                if (subjectInformation != null)
                {
                    //txtMoreInformation.Text = subjectInformation.MoreInformation;
                    if (subjectInformation.MoreInformation != "")
                    {
                        hplMoreInformation.Text = subjectInformation.MoreInformation;
                        hplMoreInformation.NavigateUrl = subjectInformation.MoreInformation;
                    }
                    else
                    {
                        hplMoreInformation.Text = "None";
                        hplMoreInformation.NavigateUrl = "";
                    }
                    txtScript.Text = subjectInformation.Scripting;
                    txtNote.Text = subjectInformation.Note;
                    hfMaxWoringDay.Value = string.IsNullOrEmpty(subjectInformation.MaxWorkingDay) ? "0" : subjectInformation.MaxWorkingDay;
                }

            }
        }

        public void clearData()
        {
            hplMoreInformation.Text = "None";
            hplMoreInformation.NavigateUrl = "";

            txtScript.Text = "";
            txtNote.Text = "";

        }

        private string CallType(string name, int etcValue)
        {
            string result = "";
            var namelower = name.ToLower();
            var namelowerreplace = namelower.Replace(" ", "_");
            switch (etcValue)
            {
                case 4210:
                    if (namelowerreplace == "inquiry")
                    {
                        result = "100000000";
                    }
                    else if (namelowerreplace == "service_request")
                    {
                        result = "100000001";
                    }
                    else if (namelowerreplace == "complaint_&_suggestion")
                    {
                        result = "100000002";
                    }
                    else if (namelowerreplace == "compliment")
                    {
                        result = "100000003";
                    }
                    else if (namelowerreplace == "sale_(outbound)")
                    {
                        result = "100000004";
                    }
                    else if (namelowerreplace == "satisfaction_evaluation")
                    {
                        result = "100000005";
                    }
                    break;

                case 112:
                    if (namelowerreplace == "inquiry")
                    {
                        result = "100000000";
                    }
                    else if (namelowerreplace == "service_request")
                    {
                        result = "100000001";
                    }
                    else if (namelowerreplace == "complaint_&_suggestion")
                    {
                        result = "100000002";
                    }
                    else if (namelowerreplace == "compliment")
                    {
                        result = "100000003";
                    }
                    else if (namelowerreplace == "sale_(outbound)")
                    {
                        result = "100000004";
                    }
                    else if (namelowerreplace == "satisfaction_evaluation")
                    {
                        result = "100000005";
                    }
                    break;
            }
            return result;
        }


        private bool validation()
        {
            bool result = false;

            if (lbCallType.SelectedValue != "" && lbCategory.SelectedValue != "" && lbSubCategory.SelectedValue != "")
            {
                result = true;
            }

            return result;
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                #region [-- stamp value to crm form --]
                string strRet = @"  var ResObj = new Object();
                                var CategoryArray = new Array();
                                var SubCategoryArray = new Array();
                                CategoryArray[0] = '{0}';
                                CategoryArray[1] = '{1}';
                                CategoryArray[2] = 'pfc_category';

                                SubCategoryArray[0] = '{2}';
                                SubCategoryArray[1] = '{3}';
                                SubCategoryArray[2] = 'pfc_sub_category';

                                ResObj.description = '{4}'; //description
                                ResObj.new_calltypeid = '{5}'; //new_calltypeid
                                ResObj.new_categoryid = CategoryArray; //new_categoryid
                                ResObj.new_sub_categoryid = SubCategoryArray; //new_sub_categoryid
                                ResObj.new_max_work_day = '{6}';
                                ResObj.directioncode = '{7}';
                                //alert(JSON.stringify(ResObj));
                                // ส่งค่าไปให้ indow.opener  โดย function รับ message จะอยู่ใน Web Resource: Function_Addcalltype
                                window.opener.postMessage(ResObj,'{8}');                  
                                window.returnValue = ResObj;                                                                                 
                             ";
                #endregion
                string strRes = string.Format(strRet
                                             , string.IsNullOrEmpty(lbCategory.SelectedValue) ? "" : lbCategory.SelectedValue, string.IsNullOrEmpty(lbCategory.SelectedItem.Text) ? "" : lbCategory.SelectedItem.Text
                                             , string.IsNullOrEmpty(lbSubCategory.SelectedValue) ? "" : lbSubCategory.SelectedValue, string.IsNullOrEmpty(lbSubCategory.SelectedItem.Text) ? "" : lbSubCategory.SelectedItem.Text
                                             , string.IsNullOrEmpty(txtNote.Text) ? "" : txtNote.Text.Replace("\n", "_-ln-_").Replace("\r", "_-lr-_")
                                             , string.IsNullOrEmpty(lbCallType.SelectedItem.Text) ? "" : CallType(lbCallType.SelectedItem.Text, int.Parse(hfetc.Value))
                                             , string.IsNullOrEmpty(hfMaxWoringDay.Value) ? "0" : hfMaxWoringDay.Value
                                             , string.IsNullOrEmpty(rbCaller.SelectedValue) ? "" : rbCaller.SelectedValue
                                             , System.Configuration.ConfigurationManager.AppSettings[posturl].ToString()
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
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "alert('กรุณากรอกข้อมูลให้ครบถ้วน');", true);
            }
        }
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {            
            string strRes = @"window.returnValue = null;";
            strRes += @" var ie7 = (document.all && !window.opera && window.XMLHttpRequest) ? true : false;
                                if (ie7) { window.open('','_parent',''); window.close(); }
                                else { this.focus(); self.opener = this; self.close(); }";
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "myScript", strRes, true);
        }
    }
}