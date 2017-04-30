﻿using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCRMInquiryCRMClientMasterInput_to_InquiryMasterASRHDataInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryClientMasterInputModel src = (InquiryClientMasterInputModel)input;
            InquiryMasterASRHDataInputModel trgt = (InquiryMasterASRHDataInputModel)output;

            trgt.fullName = src.conditionDetail.clientFullname;
            trgt.polisyClntnum = src.conditionDetail.polisyClientId;
            trgt.asrhType = src.conditionHeader.roleCode;
            trgt.taxBranchCode = src.conditionDetail.corporateBranch;
            trgt.taxNo = src.conditionDetail.idCard;
            trgt.emcsCode = src.conditionDetail.emcsCode;

            return trgt;
        }

    }
}