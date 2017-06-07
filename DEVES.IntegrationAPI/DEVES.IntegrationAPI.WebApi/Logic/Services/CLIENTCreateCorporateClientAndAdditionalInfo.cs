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
    public class CLIENTCreateCorporateClientAndAdditionalInfo: BaseEwiServiceProxy
    {
       
        public CLIENTCreateCorporateClientAndAdditionalInfo(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "CLIENT_CreateCorporateClientAndAdditionalInfo";
            systemName = "COMP";
            serviceEndpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyCLIENTCreateCorporateClient);
        }
        public CLIENTCreateCorporateClientAndAdditionalInfoContentModel Execute(CLIENTCreateCorporateClientAndAdditionalInfoInputModel input)
        {
          
            var result = SendRequest(input, serviceEndpoint);

            if (result.StatusCode != HttpStatusCode.OK)
            {

                throw new InternalErrorException(result.Message);
            }


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<CLIENTCreateCorporateClientAndAdditionalInfoOutputModel>(result.Content);
            return contentObj?.content;
        }

     

        public CLIENTCreateCorporateClientAndAdditionalInfoContentModel Execute(RegPayeeCorporateInputModel input)
        {
            BaseDataModel polCreateCorporateIn =
                DataModelFactory.GetModel(typeof(CLIENTCreateCorporateClientAndAdditionalInfoInputModel));

            polCreateCorporateIn = TransformerFactory.TransformModel(input, polCreateCorporateIn);
            return Execute((CLIENTCreateCorporateClientAndAdditionalInfoInputModel)polCreateCorporateIn);
        }
        
    }
}