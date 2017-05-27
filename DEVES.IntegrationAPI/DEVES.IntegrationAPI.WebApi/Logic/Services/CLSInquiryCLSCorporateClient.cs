using DEVES.IntegrationAPI.Model.CLS;
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
    public class CLSInquiryCLSCorporateClient : BaseEwiServiceProxy
    {
        #region Singleton
        private static CLSInquiryCLSCorporateClient _instance;

        

        public static CLSInquiryCLSCorporateClient Instance
        {
            get
            {
    
                if (_instance != null) return _instance;
                 _instance = new CLSInquiryCLSCorporateClient();

                return _instance;
            }
        }

        #endregion

        public CLSInquiryCLSCorporateClient():base("")
        {
            serviceName = "CLSInquiryCLSCorporateClient";
        }

        public CLSInquiryCLSCorporateClient(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "CLSInquiryCLSCorporateClient";
        }

        public CLSInquiryCorporateClientContentOutputModel Execute(CLSInquiryCorporateClientInputModel input)
        {
            input.backDay = AppConst.COMM_BACK_DAY;

            string endpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyCLSInquiryCorporateClient);

            var result = SendRequest(input, endpoint);

            if (result.StatusCode != HttpStatusCode.OK)
            {

                throw new InternalErrorException(result.Message);
            }


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<EWIResCLSInquiryCorporateClient>(result.Content);
            if (false == contentObj.success)
            {
                throw new Exception($"CLS Error {contentObj.responseCode}: {contentObj.responseMessage}");
            }



            return contentObj?.content;
        }

    }
}