using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCLIENTCreatePersonalClientAndAdditionalInfoInputModel_to_RegPayeePersonalInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLIENTCreatePersonalClientAndAdditionalInfoInputModel src = (CLIENTCreatePersonalClientAndAdditionalInfoInputModel)input;
            RegPayeePersonalInputModel trgt = (RegPayeePersonalInputModel)output;

            return trgt;
        }
    }
}