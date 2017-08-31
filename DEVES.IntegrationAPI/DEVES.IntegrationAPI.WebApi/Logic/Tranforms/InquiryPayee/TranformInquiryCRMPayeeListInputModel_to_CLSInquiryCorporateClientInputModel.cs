using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformInquiryCRMPayeeListInputModel_to_CLSInquiryCorporateClientInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryCRMPayeeListInputModel src = (InquiryCRMPayeeListInputModel)input;
            CLSInquiryCorporateClientInputModel trgt = (CLSInquiryCorporateClientInputModel)output;
            TraceDebugLogger.Instance.AddLog("TranformInquiryCRMPayeeListInputModel_to_CLSInquiryCorporateClientInputModel", input);
            trgt.clientId = src.polisyClientId?.Trim() ?? "";
            trgt.roleCode = src.roleCode?.Trim() ?? "";
            trgt.cleansingId = "" + src.cleansingId?.Trim() ?? "";
            trgt.corporateFullName = src.fullname?.Trim() ?? "";
            trgt.taxNo = src.taxNo?.Trim() ?? "";
            trgt.corporateStaffNo = src.taxBranchCode?.Trim() ?? "";
            
        //  trgt.taxBranch = "" + src.taxBranchCode;

        trgt.backDay = AppConst.COMM_BACK_DAY.ToString();
             trgt.telephone = "";
            trgt.emailAddress= "";

            return trgt;
        
        }

    }
}