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
            InquiryMasterASRHContentModel srcContent = (InquiryMasterASRHContentModel) input;
            CRMInquiryPayeeContentOutputModel trgtContent = (CRMInquiryPayeeContentOutputModel) output;

            trgtContent.data = new List<InquiryCrmPayeeListDataModel>();


            foreach (var ASRHListCollection in srcContent.ASRHListCollection)
            {
                if (ASRHListCollection.ASRHList != null)
                {
                    var ASRHList = ASRHListCollection.ASRHList;
                    trgtContent.data.Add(new InquiryCrmPayeeListDataModel
                    {
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
                    });
                }
            }


            return trgtContent;
        }
    }
}