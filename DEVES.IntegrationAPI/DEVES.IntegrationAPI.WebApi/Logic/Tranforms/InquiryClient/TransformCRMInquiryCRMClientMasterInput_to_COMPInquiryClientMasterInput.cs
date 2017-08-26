using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.Polisy400;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCRMInquiryCRMClientMasterInput_to_COMPInquiryClientMasterInput : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryClientMasterInputModel src = (InquiryClientMasterInputModel)input;
            COMPInquiryClientMasterInputModel trgt = (COMPInquiryClientMasterInputModel)output;

            trgt.cltType = src.conditionHeader.clientType??"";
            trgt.asrType = src.conditionHeader.roleCode??"";
            trgt.clntnum = src.conditionDetail.polisyClientId??"";
            trgt.fullName = src.conditionDetail.clientFullname ?? "";
            trgt.idcard = src.conditionDetail.idCard ?? "";
            trgt.branchCode = src.conditionDetail.corporateBranch ?? "";
            trgt.cleansingId = "" + src.conditionDetail.cleansingId?.Trim() ?? "";
            trgt.backDay = AppConst.COMM_BACK_DAY.ToString();

            if (string.IsNullOrEmpty(trgt.fullName))
            {
                trgt.fullName = src.conditionDetail.clientName1?.Trim() ?? "" + " " + src.conditionDetail.clientName2?.Trim() ?? "";
            }

            return trgt;
        }
    }
}