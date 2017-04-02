using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformCLSCreatePersonalClientOutputModel_to_RegClientPersonalInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLSCreatePersonalClientContentOutputModel src = (CLSCreatePersonalClientContentOutputModel) input;
            RegClientPersonalInputModel trgt =(RegClientPersonalInputModel) output;

            if (src.data != null)
            {

                if (string.IsNullOrEmpty(src.data.cleansingId)) trgt.generalHeader.cleansingId = src.data.cleansingId;
                

            }
            
            return trgt;

        }

    }
}