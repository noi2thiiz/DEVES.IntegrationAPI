﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.SMS;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class SendSmsService: BaseProxyService
    {
        #region Singleton
        private static SendSmsService _instance;

        private SendSmsService()
        {
            this.serviceName = "COMPSendSMS";
        }

        public static SendSmsService Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new SendSmsService();

                return _instance;
            }
        }


        #endregion
        public BaseEWIResponseModel SendMessage(string message, string mobileNumber)
        {
            var input = new SendSMSInputModel
            {
                message = message,
                mobileNumber = mobileNumber
            };
           


            string endpoint = AppConfig.Instance.Get("EWI_ENDPOINT_COMPSendSMS");
            
           
            var result = SendRequest(input, endpoint);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new InternalErrorException(result.Message);
            }
            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<BaseEWIResponseModel>(result.Content);
            return contentObj;

            //เลือกรายการที่มีค่า clientNumber มากที่สุด

        }
    }
}