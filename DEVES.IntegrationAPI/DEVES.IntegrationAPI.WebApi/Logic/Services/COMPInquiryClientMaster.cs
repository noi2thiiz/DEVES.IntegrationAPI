using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.Templates;
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
            serviceName = "COMPInquiryClientMaster";
        }

        public COMPInquiryClientMaster(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "COMPInquiryClientMaster";
        }

        public EWIResCOMPInquiryClientMasterContentModel Execute(COMPInquiryClientMasterInputModel input)
        {
            input.backDay = AppConst.COMM_BACK_DAY;

            string endpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyCOMPInquiryClient);

            var result = SendRequest(input, endpoint);

            if (result.StatusCode != HttpStatusCode.OK)
            {

                throw new InternalErrorException(result.Message);
            }


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<COMPInquiryClientMasterOutputModel>(result.Content);

            if (true != contentObj.success)
            {
                throw new BuzErrorException(
                    contentObj.responseCode,
                    $"COMP Error:{contentObj.responseMessage}",
                    "Error on execute 'COMP_InquiryClientMaster'",
                    "COMP",
                    GlobalTransactionID);


            }
            



            return contentObj?.content;
        }
    }
}