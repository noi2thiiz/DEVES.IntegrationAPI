using DEVES.IntegrationAPI.Model;

using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformInquiryCRMPayeeListInputModel_to_SAPInquiryVendorInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryCRMPayeeListInputModel src = (InquiryCRMPayeeListInputModel) input;
            SAPInquiryVendorInputModel trgt = (SAPInquiryVendorInputModel) output;

            trgt.VCODE = "";
            trgt.PREVACC = "";
            trgt.TAX3 = "";
            trgt.TAX4 = "";
            switch (src.SearchConditionType)
            {
                case ENUM_SAP_SearchConditionType.sapVendorCode:
                    trgt.VCODE = src.sapVendorCode;
                    break;
                case ENUM_SAP_SearchConditionType.polisyClientId:
                    trgt.PREVACC = src.polisyClientId;
                    break;
                case ENUM_SAP_SearchConditionType.taxNo:
                    trgt.TAX3 = src.taxNo;
                    trgt.TAX4 = src.taxBranchCode;
                    break;
                default:
                    break;
            }
            
            return trgt;
        }
    }
}