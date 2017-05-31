﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class SAPInquiryVendor: BaseEwiServiceProxy
    {
        #region Singleton
        private static SAPInquiryVendor _instance;
        private string v;

        public static SAPInquiryVendor Instance
        {
            get
            {
              
              
                if (_instance != null) return _instance;
                _instance = new SAPInquiryVendor();

                return _instance;
            }
        }


        #endregion
        public SAPInquiryVendor() : base("")
        {
            serviceName = "SAPInquiryVendor";

        }

        public SAPInquiryVendor(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "SAPInquiryVendor";
        }

        public EWIResSAPInquiryVendorContentModel Execute(SAPInquiryVendorInputModel input)
        {
           


            string endpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeySAPInquiryVendor);

            var result = SendRequest(input, endpoint);

            if (result.StatusCode != HttpStatusCode.OK)
            {

                throw new InternalErrorException(result.Message);
            }


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<SAPInquiryVendorOutputModel>(result.Content);
            if (true != contentObj.success)
            {
                throw new BuzErrorException(
                    contentObj.responseCode,
                    $"SAP Error:{contentObj.responseMessage}",
                    "Error on execute 'COMP_SAPInquiryVendor'",
                    "SAP",
                    GlobalTransactionID);
            }

            return contentObj?.content;
        }
    }
}