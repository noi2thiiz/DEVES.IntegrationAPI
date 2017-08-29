using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCRMInquiryCRMClientMasterInput_to_CLSInquiryCorporateClientInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryClientMasterInputModel src = (InquiryClientMasterInputModel)input;
            CLSInquiryCorporateClientInputModel trgt = (CLSInquiryCorporateClientInputModel)output;

            TraceDebugLogger.Instance.AddLog("TransformCRMInquiryCRMClientMasterInput_to_CLSInquiryCorporateClientInputModel", input);

            trgt.roleCode = src?.conditionHeader?.roleCode?.Trim() ?? "";
            trgt.clientId = src?.conditionDetail?.polisyClientId?.Trim() ?? "";
            trgt.corporateFullName = src?.conditionDetail?.clientFullname?.Trim() ?? "";
            trgt.taxNo = src?.conditionDetail?.idCard?.Trim() ?? "";
            //trgt.corporateBranch = "" + src.conditionDetail.corporateBranch;
            //trgt.taxBranch = "" + src.conditionDetail.corporateBranch;
            trgt.corporateStaffNo = "" + src.conditionDetail.corporateBranch?.Trim() ?? "";
            trgt.cleansingId      = "" + src.conditionDetail.cleansingId?.Trim() ?? "";
            trgt.telephone = ""; 
            trgt.emailAddress = "";
            trgt.backDay = AppConst.COMM_BACK_DAY.ToString();
            if (string.IsNullOrEmpty(trgt.corporateFullName))
            {
                if (string.IsNullOrEmpty(src.conditionDetail.clientName1))
                {
                    trgt.corporateFullName = src.conditionDetail.clientName2;
                }
                else 
                {

                    trgt.corporateFullName = src.conditionDetail.clientName1;
                }
                
            }

            trgt.corporateFullName.ReplaceMultiplSpacesWithSingleSpace();

            return trgt;
        }
    }
}