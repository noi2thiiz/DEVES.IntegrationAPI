using System;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegClientCorporate;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformRegClientCorporateInputModel_to_CLIENTUpdateCorporateClientAndAdditionalInfoInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegClientCorporateInputModel src = (RegClientCorporateInputModel)input;
            CLIENTUpdateCorporateClientAndAdditionalInfoInputModel trgt = (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)output;

            trgt.assessorFlag =  src.generalHeader.assessorFlag;
            trgt.solicitorFlag = src.generalHeader.solicitorFlag;
            trgt.repairerFlag = src.generalHeader.repairerFlag;
            trgt.hospitalFlag = src.generalHeader.hospitalFlag;
            
       
            return trgt;
        }
    }
}