using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCLSCreateCorporateClientInputModel_to_RegPayeeCorporateInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLSCreateCorporateClientInputModel src = (CLSCreateCorporateClientInputModel)input;
            RegPayeeCorporateInputModel trgt = (RegPayeeCorporateInputModel)output;

            if (src == null)
            {
                return trgt;
            }
            if (trgt.generalHeader == null)
            {
                trgt.generalHeader = new GeneralHeaderModel();
            }
            if (trgt.profileHeader == null)
            {
                trgt.profileHeader = new ProfileHeaderModel();
            }
            if (trgt.contactHeader == null)
            {
                trgt.contactHeader = new ContactHeaderModel();
            }
            if (trgt.sapVendorInfo ==null)
            {
                trgt.sapVendorInfo = new SapVendorInfoModel();
            }

           
           

            return trgt;
        }
    }
}