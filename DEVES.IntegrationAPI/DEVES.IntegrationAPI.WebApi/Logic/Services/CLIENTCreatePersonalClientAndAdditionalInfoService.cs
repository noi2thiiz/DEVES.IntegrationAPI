using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class CLIENTCreatePersonalClientAndAdditionalInfoService : BaseEwiServiceProxy
    {

        public CLIENTCreatePersonalClientAndAdditionalInfoService(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "CLIENT_CreatePersonalClientAndAdditionalInfo";
            systemName = "COMP";
            serviceEndpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyCLIENTCreatePersonalClient);
        }
        public CLIENTCreatePersonalClientAndAdditionalInfoContentModel Execute(CLIENTCreatePersonalClientAndAdditionalInfoInputModel input)
        {

            var result = SendRequest(input, serviceEndpoint);

            if (result.StatusCode != HttpStatusCode.OK)
            {

                throw new InternalErrorException(result.Message);
            }


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<CLIENTCreatePersonalClientAndAdditionalInfoOutputModel>(result.Content);
            return contentObj?.content;
        }



        public CLIENTCreatePersonalClientAndAdditionalInfoContentModel Execute(RegPayeePersonalInputModel input)
        {
            BaseDataModel polCreatePersonalIn =
                DataModelFactory.GetModel(typeof(CLIENTCreatePersonalClientAndAdditionalInfoInputModel));

            polCreatePersonalIn = TransformerFactory.TransformModel(input, polCreatePersonalIn);
            return Execute((CLIENTCreatePersonalClientAndAdditionalInfoInputModel)polCreatePersonalIn);
        }

    }
}