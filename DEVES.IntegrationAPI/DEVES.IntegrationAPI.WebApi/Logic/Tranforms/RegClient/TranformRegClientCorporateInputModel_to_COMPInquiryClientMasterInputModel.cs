using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegClientCorporate;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformRegClientCorporateInputModel_to_COMPInquiryClientMasterInputModel
  : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegClientCorporateInputModel src = (RegClientCorporateInputModel)input;
            COMPInquiryClientMasterInputModel trgt = (COMPInquiryClientMasterInputModel)output;            

            if (src == null)
            {
                return trgt;
            }

            trgt.cltType = "C";
            //trgt.asrType = src.generalHeader.roleCode;
           
            trgt.clntnum = src.generalHeader.polisyClientId ?? "";
            trgt.backDay = "30";

            return trgt;

        }
    }
}