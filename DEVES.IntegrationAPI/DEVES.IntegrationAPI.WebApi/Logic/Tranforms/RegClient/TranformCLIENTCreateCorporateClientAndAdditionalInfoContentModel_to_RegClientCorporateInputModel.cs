using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegClientCorporate;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformCLIENTCreateCorporateClientAndAdditionalInfoContentModel_to_RegClientCorporateInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLIENTCreateCorporateClientAndAdditionalInfoContentModel src = (CLIENTCreateCorporateClientAndAdditionalInfoContentModel)input;
            RegClientCorporateInputModel trgt = (RegClientCorporateInputModel)output;

            if (!string.IsNullOrEmpty(src.clientID)) trgt.generalHeader.polisyClientId = src.clientID;

            return trgt;

        }

    }
}