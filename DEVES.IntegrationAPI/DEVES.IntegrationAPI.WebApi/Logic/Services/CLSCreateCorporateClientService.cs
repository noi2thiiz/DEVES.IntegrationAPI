using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class CLSCreateCorporateClientService : BaseEwiServiceProxy
    {
        public CLSCreateCorporateClientService(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "CLS_CreateCorporateClient";
            systemName = "CLS";
            serviceEndpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyCLSCreateCorporateClient);

        }

        public CLSCreateCorporateClientContentOutputModel Execute(CLSCreateCorporateClientInputModel input)
        {

            var result = SendRequest(input, serviceEndpoint);


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<CLSCreateCorporateClientOutputModel>(result.Content);
            return contentObj?.content;
        }

        public CLSCreateCorporateClientContentOutputModel Execute(RegPayeeCorporateInputModel input)
        {

            BaseDataModel clsCreateCorporateIn =
                DataModelFactory.GetModel(typeof(CLSCreateCorporateClientInputModel));
            clsCreateCorporateIn =
                TransformerFactory.TransformModel(input, clsCreateCorporateIn);

            return Execute((CLSCreateCorporateClientInputModel) clsCreateCorporateIn);
        }
    }
}