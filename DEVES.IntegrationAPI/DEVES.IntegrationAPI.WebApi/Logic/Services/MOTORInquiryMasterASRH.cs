﻿using DEVES.IntegrationAPI.Model.MASTER;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class MOTORInquiryMasterASRH : BaseEwiServiceProxy
    {
        #region Singleton
        private static MOTORInquiryMasterASRH _instance;

        

        public static MOTORInquiryMasterASRH Instance
        {
            get
            {

                if (_instance != null) return _instance;
                _instance = new MOTORInquiryMasterASRH();

                return _instance;
            }
        }

        #endregion
        public MOTORInquiryMasterASRH() : base("")
        {
            serviceName = "MOTOR_InquiryMasterASRH";
            systemName = "ASRH";
        }

        public MOTORInquiryMasterASRH(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "MOTOR_InquiryMasterASRH";
            systemName = "ASRH";
            serviceEndpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyMOTORInquiryMasterASRH);
        }

        public InquiryMasterASRHContentModel Execute(InquiryMasterASRHDataInputModel input)
        {
            string endpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyMOTORInquiryMasterASRH);

            var result = SendRequest(input, endpoint);

            if (result.StatusCode != HttpStatusCode.OK)
            {

                throw new InternalErrorException(result.Message);
            }


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<InquiryMasterASRHOutputModel>(result.Content);
            

            return contentObj?.content;
        }
    }
}