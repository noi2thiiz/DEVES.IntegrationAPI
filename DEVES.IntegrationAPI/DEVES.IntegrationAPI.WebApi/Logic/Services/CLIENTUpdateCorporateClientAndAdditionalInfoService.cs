using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class CLIENTUpdateCorporateClientAndAdditionalInfoService : BaseEwiServiceProxy
    {

        public CLIENTUpdateCorporateClientAndAdditionalInfoService(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "CLIENT_UpdateCorporateClientAndAdditionalInfo";
            systemName = "COMP";
            serviceEndpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyCLIENTUpdateCorporateClient);
        }
        public CLIENTUpdateCorporateClientAndAdditionalInfoContentModel Execute(CLIENTUpdateCorporateClientAndAdditionalInfoInputModel input)
        {

            var result = SendRequest(input, serviceEndpoint);

            if (result.StatusCode != HttpStatusCode.OK)
            {

                throw new InternalErrorException(result.Message);
            }


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel>(result.Content);
            return contentObj?.content;
        }



        public CLIENTUpdateCorporateClientAndAdditionalInfoContentModel Execute(RegPayeeCorporateInputModel input)
        {
            BaseDataModel polUpdateCorporateIn =
                DataModelFactory.GetModel(typeof(CLIENTUpdateCorporateClientAndAdditionalInfoInputModel));

            polUpdateCorporateIn = TransformerFactory.TransformModel(input, polUpdateCorporateIn);
            return Execute((CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)polUpdateCorporateIn);
        }

    }
}