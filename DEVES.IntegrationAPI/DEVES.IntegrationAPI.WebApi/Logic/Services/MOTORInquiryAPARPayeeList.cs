﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class MotorInquiryAparPayeeList: BaseEwiServiceProxy
    {
        #region Singleton
        private static MotorInquiryAparPayeeList _instance;

        public MotorInquiryAparPayeeList()
        {
            serviceName = "MotorInquiryAparPayeeList";
        }

        public static MotorInquiryAparPayeeList Instance
        {
            get
            {
                //ยกเลิก Singleton
               
                if (_instance != null) return _instance;
                _instance = new MotorInquiryAparPayeeList();

                return _instance;
            }
        }


        #endregion

        public InquiryAPARPayeeContentModel Execute(InquiryAPARPayeeListInputModel input)
        {
            switch (input.requester)
            {
                case "MC": input.requester = "MotorClaim"; break;
                default: input.requester = "MotorClaim"; break;
            }


            string endpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyAPARInquiryPayeeList);
          
            var result = SendRequest(input, endpoint);
        
            if (result.StatusCode != HttpStatusCode.OK)
            {

                throw new InternalErrorException(result.Message);
            }

        
            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<InquiryAPARPayeeOutputModel>(result.Content);
            if (false == contentObj.success)
            {
                throw new Exception($"EWI-APAR Error {contentObj.responseCode}: {contentObj.responseMessage}");
            }

          

            return contentObj?.content;
        }
    }
}