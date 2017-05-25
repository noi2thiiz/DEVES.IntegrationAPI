using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.SAP;
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

        private SAPInquiryVendor()
        {

        }

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

        public EWIResSAPInquiryVendorContentModel Execute(SAPInquiryVendorInputModel input)
        {
           


            string endpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyAPARInquiryPayeeList);

            var result = SendRequest(input, endpoint);

            if (result.StatusCode != HttpStatusCode.OK)
            {

                throw new InternalErrorException(result.Message);
            }


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<SAPInquiryVendorOutputModel>(result.Content);
            if (false == contentObj.success)
            {
                throw new Exception($"SAP Error {contentObj.responseCode}: {contentObj.responseMessage}");
            }

            return contentObj?.content;
        }
    }
}