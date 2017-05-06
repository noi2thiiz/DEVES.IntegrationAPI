using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class transformInquiryMasterASRHContentASRHListCollectionDataModel_to_InquiryCRMPayeeListInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryMasterASRHContentASRHListCollectionDataModel inp = (InquiryMasterASRHContentASRHListCollectionDataModel)input;
            InquiryCRMPayeeListInputModel oup;
            if (output == null)
                oup = new InquiryCRMPayeeListInputModel();
            else
                oup = (InquiryCRMPayeeListInputModel)output;
            
            oup.assessorFlag = inp.ASRHList.assessorFlag ?? "";
            oup.clientType = inp.ASRHList.polisyClntnum ?? "";
            oup.emcsCode = "";
            oup.emcsMemId = inp.ASRHList.emcsMemId ?? "";
            oup.emcsMemHeadId = inp.ASRHList.emcsMemHeadId ?? "";
            oup.fullname = inp.ASRHList.fullName ?? "";
            oup.hospitalFlag = inp.ASRHList.assessorFlag == "H" ? "Y" : "N";
            oup.polisyClientId = inp.ASRHList.polisyClntnum ?? "";
            oup.repairerFlag = inp.ASRHList.repairerFlag ?? "";
            oup.requester = "";
            oup.roleCode = "";
            oup.sapVendorCode = inp.ASRHList.vendorCode ?? "";
            oup.solicitorFlag = inp.ASRHList.solicitorFlag ?? "";
            oup.taxNo = inp.ASRHList.taxNo??"";
            oup.taxBranchCode = inp.ASRHList.taxBranchCode??"";

            return oup;
        }
    }
}