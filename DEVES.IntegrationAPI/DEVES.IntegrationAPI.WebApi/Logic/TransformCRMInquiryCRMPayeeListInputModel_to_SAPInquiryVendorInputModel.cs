using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCRMInquiryCRMPayeeListInputModel_to_SAPInquiryVendorInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryCRMPayeeListInputModel src = (InquiryCRMPayeeListInputModel) input;
            SAPInquiryVendorInputModel trgt = (SAPInquiryVendorInputModel) output;

            trgt.PREVACC = src.polisyClientId;
            trgt.TAX3 = src.taxNo;
            trgt.TAX4 = src.taxBranchCode;
            trgt.VCODE = src.sapVendorCode;

            return output;
        }
    }
}