using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.ClaimRegistration;
using DEVES.IntegrationAPI.Model.CLS;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class LOCUSClaimRegistrationService : BaseEwiServiceProxy
    {
        public LOCUSClaimRegistrationService(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "LOCUS_ClaimRegistration";
            systemName = "LOCUS";
            serviceEndpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyLOCUSClaimRegistration);
        }

        public LocusClaimRegistrationContentOutputModel Execute(LocusClaimRegistrationInputModel input)
        {

            var result = SendRequest(input, serviceEndpoint);


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<LocusClaimRegistrationOutputModel>(result.Content);
            return contentObj?.content;
        }

        internal LocusClaimRegistrationContentOutputModel Execute(BaseDataModel inputData)
        {
           return Execute((LocusClaimRegistrationInputModel)inputData);
        }
    }
}