using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformInquiryCRMPayeeListInputModel_to_COMPInquiryClientMasterInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryCRMPayeeListInputModel src = (InquiryCRMPayeeListInputModel)input;
            COMPInquiryClientMasterInputModel trgt = (COMPInquiryClientMasterInputModel)output;

            trgt.cltType = src.clientType;
            trgt.asrType = src.roleCode;
            trgt.clntnum = src.polisyClientId;
            trgt.fullName = src.fullname;

            trgt.idcard= src.taxNo;
            trgt.branchCode= src.taxBranchCode;
            
            //trgt.backDay =

            // src.sapVendorCode;
            // src.requester;
            // src.emcsCode;

            return trgt;

        }

    }
}