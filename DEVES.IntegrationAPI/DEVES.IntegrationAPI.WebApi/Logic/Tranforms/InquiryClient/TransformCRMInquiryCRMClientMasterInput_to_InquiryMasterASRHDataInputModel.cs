﻿using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCRMInquiryCRMClientMasterInput_to_InquiryMasterASRHDataInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            
            InquiryClientMasterInputModel src = (InquiryClientMasterInputModel)input;
            InquiryMasterASRHDataInputModel trgt = (InquiryMasterASRHDataInputModel)output;

            TraceDebugLogger.Instance.AddLog("TransformCRMInquiryCRMClientMasterInput_to_InquiryMasterASRHDataInputModel", input);

            trgt.fullName = src.conditionDetail.clientFullname ?? "";
            if (string.IsNullOrEmpty(trgt.fullName))
            {
                trgt.fullName = src.conditionDetail.clientName1 +" "+ src.conditionDetail.clientName2;

            }
           
            trgt.fullName.ReplaceMultiplSpacesWithSingleSpace();

            trgt.polisyClntnum = src.conditionDetail.polisyClientId ?? "";
            trgt.asrhType = src.conditionHeader.roleCode ?? "";
            trgt.taxBranchCode = src.conditionDetail.corporateBranch ?? "";
            trgt.taxNo = src.conditionDetail.idCard ?? "";
            trgt.emcsCode = src.conditionDetail.emcsCode ?? "";
            trgt.cleansingId = "" + src.conditionDetail.cleansingId?.Trim() ?? "";

          
            return trgt;
        }

    }
}