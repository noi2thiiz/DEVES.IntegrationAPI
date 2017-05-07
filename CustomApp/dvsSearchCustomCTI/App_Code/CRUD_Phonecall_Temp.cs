using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Tooling.Connector;
using devesCustomCTI.BusinessEntities;
using DVSXRM;
/// <summary>
/// Summary description for CRUD_Phonecall_Temp
/// </summary>
namespace devesCustomCTI.CRUDphonecall
{
    public class CRUD_Phonecall_Temp
    {
        #region [Variable]
        private string _Colname;
        private string _pfc_source;
        private string _pfc_name;
        private string _pfc_czid;
        private string _pfc_phonenumber;
        private string _contactid;
        private string _pfc_form;
        private string _pfc_formentitytype;
        private string _pfc_formname;
        private string _pfc_firstname;
        private string _pfc_lastname;
        private string _pfc_callerid;
        private string _pfc_policyid;
        private string _pfc_policyname;
        private string _pfc_policy_type;
        private string _pfc_policyaddid;
        private string _pfc_policyaddname;
        private string _pfc_policyadd_type;
        private string _pfc_url;
        private string _pfc_handcode;
        private string _pfc_ivr;
        #endregion
        #region [get set string]
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
        public virtual string pfc_phonenumber
        {
            get
            {
                _Colname = "pfc_phonenumber";
                return _pfc_phonenumber;
            }
            set
            {
                this._pfc_phonenumber = value;
            }
        }
        public virtual string pfc_form
        {
            get
            {
                _Colname = "pfc_form";
                return _pfc_form;
            }
            set
            {
                this._pfc_form = value;
            }
        }
        public virtual string pfc_formentitytype
        {
            get
            {
                _Colname = "pfc_formentitytype";
                return _pfc_formentitytype;
            }
            set
            {
                this._pfc_formentitytype = value;
            }
        }
        public virtual string pfc_formname
        
        {
            get
            {
                _Colname = "pfc_formname";
                return _pfc_formname;
            }
            set
            {
                this._pfc_formname = value;
            }
        }
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
        public virtual string pfc_firstname

        {
            get
            {
                _Colname = "pfc_firstname";
                return _pfc_firstname;
            }
            set
            {
                this._pfc_firstname = value;
            }
        }
        public virtual string pfc_lastname
        {
            get
            {
                _Colname = "pfc_lastname";
                return _pfc_lastname;
            }
            set
            {
                this._pfc_lastname = value;
            }
        }
        public virtual string pfc_callerid
        {
            get
            {
                _Colname = "pfc_callerid";
                return _pfc_callerid;
            }
            set
            {
                this._pfc_callerid = value;
            }
        }
        public virtual string pfc_policyid
        {
            get
            {
                _Colname = "pfc_policyid";
                return _pfc_policyid;
            }
            set
            {
                this._pfc_policyid = value;
            }
        }
        public virtual string pfc_policyname
        {
            get
            {
                _Colname = "pfc_policyname";
                return _pfc_policyname;
            }
            set
            {
                this._pfc_policyname = value;
            }
        }
        public virtual string pfc_policy_type
        {
            get
            {
                _Colname = "pfc_policy_type";
                return _pfc_policy_type;
            }
            set
            {
                this._pfc_policy_type = value;
            }
        }
        public virtual string pfc_policyaddid
        {
            get
            {
                _Colname = "pfc_policyaddid";
                return _pfc_policyaddid;
            }
            set
            {
                this._pfc_policyaddid = value;
            }
        }
        public virtual string pfc_policyaddname
        {
            get
            {
                _Colname = "pfc_policyaddname";
                return _pfc_policyaddname;
            }
            set
            {
                this._pfc_policyaddname = value;
            }
        }
        public virtual string pfc_policyadd_type
        {
            get
            {
                _Colname = "pfc_policyadd_type";
                return _pfc_policyadd_type;
            }
            set
            {
                this._pfc_policyadd_type = value;
            }
        }
        public virtual string pfc_url
        {
            get
            {
                _Colname = "pfc_url";
                return _pfc_url;
            }
            set
            {
                this._pfc_url = value;
            }
        }
        public virtual string pfc_handcode
        {
            get
            {
                _Colname = "pfc_handcode";
                return _pfc_handcode;
            }
            set
            {
                this._pfc_handcode = value;
            }
        }
        public virtual string pfc_ivr
        {
            get
            {
                _Colname = "pfc_ivr";
                return _pfc_ivr;
            }
            set
            {
                this._pfc_ivr = value;
            }
        }
        #endregion
        private IOrganizationService _service;
        public Guid CreatePhonecallTemp(CRUD_Phonecall_Temp prxycrm)
        {     
            #region [late-bound]
            OrganizationServiceProxy _serviceProxy;
            //string connectionstring = @"ServiceUri=https://crmdev.deves.co.th/CRMDEV;Domain=dvs;Username=crmtest1;Password=crm#01;";
            CrmServiceClient client = new CrmServiceClient(System.Configuration.ConfigurationManager.ConnectionStrings["DBCRM"].ConnectionString);
            _serviceProxy = client.OrganizationServiceProxy;          
                Entity PhonecallTemp = new Entity("pfc_phonecall_temp");                
            if(!string.IsNullOrEmpty(prxycrm.pfc_name)) 
                PhonecallTemp["pfc_name"] = prxycrm.pfc_name;

            if (!string.IsNullOrEmpty(prxycrm.pfc_czid))
                PhonecallTemp["pfc_czid"] = prxycrm.pfc_czid;

            if (!string.IsNullOrEmpty(prxycrm.pfc_phonenumber))
                PhonecallTemp["pfc_phonenumber"] = prxycrm.pfc_phonenumber;

            if (!string.IsNullOrEmpty(prxycrm.pfc_form))
                PhonecallTemp["pfc_form"] = prxycrm.pfc_form;

            if (!string.IsNullOrEmpty(prxycrm.pfc_formentitytype))
                PhonecallTemp["pfc_formtype"] = prxycrm.pfc_formentitytype;

            if (!string.IsNullOrEmpty(prxycrm.pfc_formname))
                PhonecallTemp["pfc_formname"] = prxycrm.pfc_formname;

            if (!string.IsNullOrEmpty(prxycrm.pfc_callerid))
                PhonecallTemp["pfc_caller_id"] = prxycrm.pfc_callerid;

            if (!string.IsNullOrEmpty(prxycrm.pfc_policyid))
                PhonecallTemp["pfc_policyid"] = prxycrm.pfc_policyid;

            if (!string.IsNullOrEmpty(prxycrm.pfc_policyname))
                PhonecallTemp["pfc_policyidname"] = prxycrm.pfc_policyname;

            if (!string.IsNullOrEmpty(prxycrm.pfc_policy_type))
                PhonecallTemp["pfc_policyidtype"] = prxycrm.pfc_policy_type;

            if (!string.IsNullOrEmpty(prxycrm.pfc_policyaddid))
                PhonecallTemp["pfc_policy_additionalid"] = prxycrm.pfc_policyaddid;

            if (!string.IsNullOrEmpty(prxycrm.pfc_policyaddname))
                PhonecallTemp["pfc_policy_additionalidname"] = prxycrm.pfc_policyaddname;

            if (!string.IsNullOrEmpty(prxycrm.pfc_policyadd_type))
                PhonecallTemp["pfc_policy_additionalidtype"] = prxycrm.pfc_policyadd_type;

            if (!string.IsNullOrEmpty(prxycrm.pfc_url))
                PhonecallTemp["pfc_url"] = prxycrm.pfc_url;

            if (!string.IsNullOrEmpty(prxycrm.pfc_handcode))
                PhonecallTemp["pfc_hand_code"] = prxycrm.pfc_handcode;

            if (!string.IsNullOrEmpty(prxycrm.pfc_ivr))
                PhonecallTemp["pfc_cti_ivr"] = prxycrm.pfc_ivr;

            Guid id_phonecalltemp = _serviceProxy.Create(PhonecallTemp);
            
            #endregion
            return id_phonecalltemp;
        }
        public Guid CreateIfnotfound(CRUD_Phonecall_Temp prxycrm)
        {
            OrganizationServiceProxy _serviceProxy;
            //string connectionstring = @"ServiceUri=https://internalcrmdev.deves.co.th/CRMDEV;Domain=dvs;Username=dvs\crmappadmin;Password=crmapp@2490";
            CrmServiceClient client = new CrmServiceClient(System.Configuration.ConfigurationManager.ConnectionStrings["DBCRM"].ConnectionString);
            _serviceProxy = client.OrganizationServiceProxy;
            Entity Contact = new Entity("contact");

            if (!string.IsNullOrEmpty(prxycrm.pfc_firstname))
                Contact["FirstName"] = prxycrm.pfc_firstname;

            if (!string.IsNullOrEmpty(prxycrm.pfc_lastname))
                Contact["LastName"] = prxycrm.pfc_lastname;

            if (!string.IsNullOrEmpty(prxycrm.pfc_czid))
                Contact["pfc_citizen_id"] = prxycrm.pfc_czid;

            if (!string.IsNullOrEmpty(prxycrm.pfc_phonenumber))
                Contact["pfc_MobilePhone1"] = prxycrm.pfc_phonenumber;
            Guid id_phonecalltemp = _serviceProxy.Create(Contact);
            return id_phonecalltemp;
        }
    }
}