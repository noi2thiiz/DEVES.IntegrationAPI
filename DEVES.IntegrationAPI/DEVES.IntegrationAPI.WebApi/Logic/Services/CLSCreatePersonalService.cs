using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class CLSCreatePersonalService : BaseEwiServiceProxy
    {
        public CLSCreatePersonalService(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "CLS_CreatePersonalClient";
            systemName = "CLS";
            serviceEndpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyCLSCreatePersonalClient);

        }

        public CLSCreatePersonalClientContentOutputModel Execute(CLSCreatePersonalClientInputModel input)
        {

            var result = SendRequest(input, serviceEndpoint);


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<CLSCreatePersonalClientOutputModel>(result.Content);
            return contentObj?.content;
        }

        public CLSCreatePersonalClientContentOutputModel Execute(RegPayeePersonalInputModel input)
        {

            BaseDataModel clsCreatePersonalIn =
                DataModelFactory.GetModel(typeof(CLSCreatePersonalClientInputModel));
            clsCreatePersonalIn =
                TransformerFactory.TransformModel(input, clsCreatePersonalIn);

            return Execute((CLSCreatePersonalClientInputModel)clsCreatePersonalIn);
        }
    }
}