using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Xrm.Client;
using devesCustomCTI.BusinessEntities;
using DVSXRM;
/// <summary>
/// Summary description for BusCrm_PhoneCall_Temp
/// </summary>
namespace devesCustomCTI.BusinessLogic
{
    public class BusCrm_PhoneCall_Temp
    {
        #region [variables]
        private string _Colname;
        private string _pfc_source;
        private string _pfc_name;
        private string _pfc_czid;
        private string _pfc_policyid;
        private string _pfc_form;
        private string _pfc_formname;
        private string _pfc_formtype;
        private string _pfc_relate_customerid;
        private string _pfc_related_companyid;
        private string _pfc_policyidname;
        private string _pfc_policyidtype;
        private string _pfc_caller_id;
        #endregion

        #region [pfc_name]
        public virtual string pfc_name
        {
            get
            {
                _Colname = "pfc_name";
                return _pfc_name;
            }
            set
            {
                this._pfc_name = value;
            }
        }
        #endregion
        #region [pfc_source]
        public virtual string pfc_source
        {
            get
            {
                _Colname = "pfc_source";
                return _pfc_source;
            }
            set
            {
                this._pfc_source = value;
            }
        }
        #endregion
        #region [pfc_czid]
        public virtual string pfc_czid
        {
            get
            {
                _Colname = "pfc_czid";
                return _pfc_czid;
            }
            set
            {
                this._pfc_czid = value;
            }
        }
        #endregion
        #region [pfc_policyid]
        public virtual string pfc_policyid
        {
            get
            {
                _Colname = "pfc_policyId";
                return _pfc_policyid;
            }
            set
            {
                this._pfc_policyid = value;
            }
        }
        #endregion
        #region [pfc_form]
        public virtual string pfc_form
        {
            get
            {
                _Colname = "pfc_Form";
                return _pfc_form;
            }
            set
            {
                this._pfc_form = value;
            }
        }
        #endregion
        #region [pfc_formName]
        public virtual string pfc_formName
        {
            get
            {
                _Colname = "pfc_formName";
                return _pfc_formname;
            }
            set
            {
                this._pfc_formname = value;
            }
        }
        #endregion
        #region [pfc_formType]
        public virtual string pfc_formType
        {
            get
            {
                _Colname = "pfc_formType";
                return _pfc_formtype;
            }
            set
            {
                this._pfc_formtype = value;
            }
        }
        #endregion
        #region [pfc_relate_customerid]
        public virtual string pfc_relate_customerid
        {
            get
            {
                _Colname = "pfc_relate_customerid";
                return _pfc_relate_customerid;
            }
            set
            {
                this._pfc_relate_customerid = value;
            }
        }
        #endregion
        #region [pfc_related_companyid]
        public virtual string pfc_related_companyid
        {
            get
            {
                _Colname = "pfc_related_companyid";
                return _pfc_related_companyid;
            }
            set
            {
                this._pfc_related_companyid = value;
            }
        }
        #endregion
        #region [pfc_policyidname]
        public virtual string pfc_policyidname
        {
            get
            {
                _Colname = "pfc_Form";
                return _pfc_form;
            }
            set
            {
                this._pfc_form = value;
            }
        }
        #endregion
        #region [pfc_policyidtype]
        public virtual string pfc_policyidtype
        {
            get
            {
                _Colname = "pfc_Form";
                return _pfc_form;
            }
            set
            {
                this._pfc_form = value;
            }
        }
        #endregion
        #region [pfc_caller_id]
        public virtual string pfc_caller_id
        {
            get
            {
                _Colname = "pfc_caller_id";
                return _pfc_caller_id;
            }
            set
            {
                this._pfc_caller_id = value;
            }
        }
        #endregion
        public daResult pfc_PhoneCall_TempCreateRecord(BusCrm_PhoneCall_Temp prxyCrm)
        {
            daResult ret = new daResult();
            ret.ResultCode = daResult.EnumResultCode.Error;
            try
            {

                var xrmConn = new CrmConnection("DBCRM");
                using (ServiceContext ctx = new ServiceContext(new Microsoft.Xrm.Client.Services.OrganizationService(xrmConn)))
                {
                    #region [Mapped]
                    pfc_phonecall_temp etn = new pfc_phonecall_temp();
                    etn.pfc_source = prxyCrm.pfc_source;
                    if (!string.IsNullOrEmpty(prxyCrm.pfc_caller_id)) etn.pfc_caller_id = prxyCrm.pfc_caller_id;
                    if (!string.IsNullOrEmpty(prxyCrm.pfc_czid)) etn.pfc_czid = prxyCrm.pfc_czid;
                    //if (!string.IsNullOrEmpty(prxyCrm.new_pnum)) etn.new_pnum = prxyCrm.new_pnum;
                    //if (!string.IsNullOrEmpty(prxyCrm.new_phonenumber)) etn.new_phonenumber = prxyCrm.new_phonenumber;
                    //if (!string.IsNullOrEmpty(prxyCrm.new_incoming_number)) etn.new_incoming_number = prxyCrm.new_incoming_number;
                    //if (!string.IsNullOrEmpty(prxyCrm.new_policy_number)) etn.new_policy_number = prxyCrm.new_policy_number;
                    //if (!string.IsNullOrEmpty(prxyCrm.new_underwrite_id)) etn.new_underwrite_id = prxyCrm.new_underwrite_id;
                    //if (!string.IsNullOrEmpty(prxyCrm.new_underwrite_name)) etn.new_underwrite_name = prxyCrm.new_underwrite_name;
                    //if (!string.IsNullOrEmpty(prxyCrm.new_underwrite_team)) etn.new_underwrite_team = prxyCrm.new_underwrite_team;
                    //if (!string.IsNullOrEmpty(prxyCrm.new_underwrite_ext_no)) etn.new_underwrite_ext_no = prxyCrm.new_underwrite_ext_no;

                    if (!string.IsNullOrEmpty(prxyCrm.pfc_form))
                    {
                        etn.pfc_form = prxyCrm.pfc_form.ToUpper();
                        etn.pfc_formName = prxyCrm.pfc_formName;
                        etn.pfc_formType = prxyCrm.pfc_formType.ToLower();
                    }
                    if (!string.IsNullOrEmpty(prxyCrm.pfc_relate_customerid)) { }
                    if (!string.IsNullOrEmpty(prxyCrm.pfc_related_companyid)) { }
                    if (!string.IsNullOrEmpty(prxyCrm.pfc_policyid))
                    {
                        etn.pfc_policyId = prxyCrm.pfc_policyid.ToUpper();
                        etn.pfc_policyIdName = prxyCrm.pfc_policyidname;
                        etn.pfc_policyIdType = prxyCrm.pfc_policyidtype.ToLower();
                    }
                    #endregion

                    ctx.AddObject(etn);
                    ctx.SaveChanges();
                    ret.ResultDescription = etn.Id.ToString().ToUpper();
                    ret.ResultCode = daResult.EnumResultCode.OK;
                }//-- using (xrmServiceContext ctx = new xrmServiceContext(new Microsoft.Xrm.Client.Services.OrganizationService(xrmConn))) --
            }
            catch (Exception ex)
            {
                ret.ResultCode = daResult.EnumResultCode.Error;
                ret.ResultDescription = ex.InnerException.Message;

            }
            return ret;
        }
    }
}