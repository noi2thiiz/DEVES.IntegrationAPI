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
    public class CLSInquiryCLSPersonalClientService : BaseEwiServiceProxy
    {
        #region Singleton
        private static CLSInquiryCLSPersonalClientService _instance;

        

        public static CLSInquiryCLSPersonalClientService Instance
        {
            get
            {

                if (_instance != null) return _instance;
                _instance = new CLSInquiryCLSPersonalClientService();

                return _instance;
            }
        }

        #endregion
        public CLSInquiryCLSPersonalClientService():base("")
        {
            serviceName = "CLS_InquiryCLSPersonal";
            systemName = "CLS";
        }

        public CLSInquiryCLSPersonalClientService(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "CLS_InquiryCLSPersonal";
            systemName = "CLS";
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
        private BaseTransformer _clsPersonalInputTransform = new TransformCRMInquiryCRMClientMasterInput_to_CLSInquiryCLSPersonalClientInput();
        public CLSInquiryPersonalClientContentOutputModel Execute(InquiryClientMasterInputModel searchCondition)
        {
            var clssearchCondition = (CLSInquiryPersonalClientInputModel)
                _clsPersonalInputTransform.TransformModel(searchCondition, new CLSInquiryPersonalClientInputModel());

            return Execute(clssearchCondition);

        }
    }
}