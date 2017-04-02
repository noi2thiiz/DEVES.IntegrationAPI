using System.Collections.Generic;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformCRMRegClientPersonalOutputDataModel_to_CRMRegClientPersonalOutputDataModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CRMRegClientPersonalOutputModel src = (CRMRegClientPersonalOutputModel)input;
            CRMRegClientPersonalOutputModel trgt = (CRMRegClientPersonalOutputModel)output;

            trgt = src;

            return trgt;

        }

    }
}