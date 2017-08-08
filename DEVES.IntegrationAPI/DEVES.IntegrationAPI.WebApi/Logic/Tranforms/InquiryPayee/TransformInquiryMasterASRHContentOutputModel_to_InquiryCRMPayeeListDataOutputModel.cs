using System;
using System.Collections.Generic;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class
        TransformInquiryMasterASRHContentOutputModel_to_InquiryCRMPayeeListDataOutputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryMasterASRHContentModel srcContent = (InquiryMasterASRHContentModel)input;
            CRMInquiryPayeeContentOutputModel trgtContent = (CRMInquiryPayeeContentOutputModel)output;

            #region Prevent Null Reference

            if (srcContent == null)
            {
                return trgtContent;
            }
            if (srcContent?.ASRHListCollection == null)
            {
                return trgtContent;
            }

            if (trgtContent == null)
            {
                trgtContent = new CRMInquiryPayeeContentOutputModel();

            }
            trgtContent.data = new List<InquiryCrmPayeeListDataModel>();

            #endregion



            foreach (var ASRHListCollection in srcContent?.ASRHListCollection)
            {

                if (ASRHListCollection?.ASRHList != null)
                {

                    var ASRHList = ASRHListCollection?.ASRHList;
                    var dataItem = new InquiryCrmPayeeListDataModel
                    {
                        sourceData = CommonConstant.CONST_SYSTEM_MASTER_ASRH,

                        polisyClientId = ASRHList.polisyClntnum,
                        sapVendorCode = ASRHList.vendorCode,
                        fullName = ASRHList.fullName,
                        taxNo = ASRHList.taxNo,
                        taxBranchCode = ASRHList.taxBranchCode,
                        emcsMemHeadId = ASRHList.emcsMemHeadId,
                        emcsMemId = ASRHList.emcsMemId,
                        address = ASRHList.address,
                        contactNumber = ASRHList.contactNumber,
                        assessorFlag = ASRHList.assessorFlag,
                        solicitorFlag = ASRHList.solicitorFlag,
                        repairerFlag = ASRHList.repairerFlag
                        //  ASRHList "businessType": null,
                        //  ASRHList.masterASRHCode
                        // ASRHList.polisyClntnum
                    };
                    switch (ASRHList.businessType)
                    {
                        case "Hospital" :
                            dataItem.hospitalFlag = "Y"; break;
                    }

                   // dataItem.AddDebugInfo("MASTER_ASHR JSON Source", ASRHListCollection);
                   // dataItem.AddDebugInfo("Transformer", "TransformInquiryMasterASRHContentOutputModel_to_InquiryCRMPayeeListDataOutputModel");

                    trgtContent.data.Add(dataItem);
                }
            }


            return trgtContent;
        }
    }
}