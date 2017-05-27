﻿using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class CLSInquiryCLSPersonalClient : BaseEwiServiceProxy
    {
        #region Singleton
        private static CLSInquiryCLSPersonalClient _instance;

        

        public static CLSInquiryCLSPersonalClient Instance
        {
            get
            {

                if (_instance != null) return _instance;
                _instance = new CLSInquiryCLSPersonalClient();

                return _instance;
            }
        }

        #endregion
        public CLSInquiryCLSPersonalClient():base("")
        {
            serviceName = "CLSInquiryCLSPersonal";
        }

        public CLSInquiryCLSPersonalClient(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "CLSInquiryCLSPersonal";
        }

        public CLSInquiryPersonalClientContentOutputModel Execute(CLSInquiryPersonalClientInputModel input)
        {
            //input.backDay = AppConst.COMM_BACK_DAY;

            string endpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyCLSInquiryPersonalClient);

            var result = SendRequest(input, endpoint);

            if (result.StatusCode != HttpStatusCode.OK)
            {

                throw new InternalErrorException(result.Message);
            }


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<EWIResCLSInquiryPersonalClient>(result.Content);
            if (false == contentObj.success)
            {
                throw new Exception($"CLS Error {contentObj.responseCode}: {contentObj.responseMessage}");
            }



            return contentObj?.content;
        }
    }
}