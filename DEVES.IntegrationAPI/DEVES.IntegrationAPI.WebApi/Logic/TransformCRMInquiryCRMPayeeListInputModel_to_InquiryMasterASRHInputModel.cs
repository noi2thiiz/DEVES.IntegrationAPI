using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformInquiryCRMPayeeListInputModel_to_InquiryMasterASRHDataInputModel : BaseTransformer
    {

        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryCRMPayeeListInputModel src = (InquiryCRMPayeeListInputModel)input;
            InquiryMasterASRHDataInputModel trgt = (InquiryMasterASRHDataInputModel)output;

            trgt.fullName = src.fullname;
            trgt.polisyClntnum = src.polisyClientId;
            trgt.asrhType = src.roleCode;
            trgt.taxBranchCode = src.taxBranchCode;
            trgt.vendorCode = src.sapVendorCode;
            trgt.taxNo = src.taxNo;
            trgt.taxBranchCode  =  src.taxBranchCode;
            trgt.emcsCode = src.emcsCode;

            return output;
        }

    }
}