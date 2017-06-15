using DEVES.IntegrationAPI.Model.CLS;
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
            serviceName = "CLS_InquiryCLSCorporateClient";
            systemName = "CLS";
        }

        public CLSInquiryCLSCorporateClient(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "CLS_InquiryCLSCorporateClient";
            systemName = "CLS";
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
           
            if (true != contentObj?.content?.success && contentObj?.content?.code != AppConst.CODE_CLS_NOTFOUND)
            {
                throw new BuzErrorException(
                    contentObj?.content?.code,
                    $"CLS Error:{contentObj?.content?.message}",
                    contentObj?.content?.description,
                    "CLS",
                    GlobalTransactionID);

            }

            return contentObj?.content;
        }

        public BaseTransformer CLSCorporateInputTransform =
            new TransformCRMInquiryCRMClientMasterInput_to_CLSInquiryCorporateClientInputModel();
        public CLSInquiryCorporateClientContentOutputModel Execute(InquiryClientMasterInputModel searchCondition)
        {

            var clssearchCondition = (CLSInquiryCorporateClientInputModel)
                CLSCorporateInputTransform.TransformModel(searchCondition, new CLSInquiryCorporateClientInputModel());
            return Execute(clssearchCondition);
        }
    }
}