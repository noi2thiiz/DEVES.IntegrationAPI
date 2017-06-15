﻿using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class COMPInquiryClientMaster : BaseEwiServiceProxy
    {
        #region Singleton
        private static COMPInquiryClientMaster _instance;

        

        public static COMPInquiryClientMaster Instance
        {
            get
            {
              
             
                if (_instance != null) return _instance;
                _instance = new COMPInquiryClientMaster();

                return _instance;
            }
        }

        #endregion

        public COMPInquiryClientMaster():base("")
        {
            serviceName = "COMP_InquiryClientMaster";
            systemName = "COMP";
        }

        public COMPInquiryClientMaster(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "COMP_InquiryClientMaster";
            systemName = "COMP";
        }

        public EWIResCOMPInquiryClientMasterContentModel Execute(COMPInquiryClientMasterInputModel input)
        {
            input.backDay = AppConst.COMM_BACK_DAY;

            string endpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyCOMPInquiryClient);

            var result = SendRequest(input, endpoint);


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<COMPInquiryClientMasterOutputModel>(result.Content);

            return contentObj?.content;
        }

        public BaseTransformer COMPInputTransform = new TransformCRMInquiryCRMClientMasterInput_to_COMPInquiryClientMasterInput();
        public EWIResCOMPInquiryClientMasterContentModel Execute(InquiryClientMasterInputModel searchCondition)
        {
            var compSearchCondition = (COMPInquiryClientMasterInputModel)
                COMPInputTransform.TransformModel(searchCondition, new COMPInquiryClientMasterInputModel());

            return Execute(compSearchCondition);

        }
    }
}