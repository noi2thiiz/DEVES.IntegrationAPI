using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformCLIENTCreatePersonalClientAndAdditionalInfoOutputModel_to_RegClientPersonalInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLIENTCreatePersonalClientAndAdditionalInfoContentModel src = (CLIENTCreatePersonalClientAndAdditionalInfoContentModel)input;
            RegClientPersonalInputModel trgt = (RegClientPersonalInputModel)output;

       

                if (string.IsNullOrEmpty(src.clientID)) trgt.generalHeader.polisyClientId = src.clientID;
         
            return trgt;

        }

    }
}