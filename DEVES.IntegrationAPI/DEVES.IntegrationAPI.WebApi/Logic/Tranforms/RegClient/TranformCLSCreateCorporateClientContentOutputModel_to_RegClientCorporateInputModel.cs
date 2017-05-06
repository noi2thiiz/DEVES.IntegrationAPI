using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegClientCorporate;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformCLSCreateCorporateClientContentOutputModel_to_RegClientCorporateInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLSCreateCorporateClientContentOutputModel src = (CLSCreateCorporateClientContentOutputModel)input;
            RegClientCorporateInputModel trgt = (RegClientCorporateInputModel)output;

            if (src.data != null)
            {
                if (!string.IsNullOrEmpty(src.data.cleansingId))
                {
                    trgt.generalHeader.cleansingId = src.data.cleansingId;
                }
            }

            return trgt;

        }

    }
}