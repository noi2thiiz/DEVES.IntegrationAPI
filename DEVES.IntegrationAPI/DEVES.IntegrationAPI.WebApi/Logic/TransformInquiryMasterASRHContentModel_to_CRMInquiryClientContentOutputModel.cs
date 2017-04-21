using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.Templates;
using System;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformInquiryMasterASRHContentModel_to_CRMInquiryClientContentOutputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryMasterASRHContentModel src = (InquiryMasterASRHContentModel)input;
            CRMInquiryClientContentOutputModel trgt = (CRMInquiryClientContentOutputModel)output;

            foreach (var asrh in src.ASRHListCollection)
            {
                CRMInquiryClientOutputDataModel data = new CRMInquiryClientOutputDataModel();
                data.generalHeader = new CRMInquiryClientGeneralHeaderModel();
                data.generalHeader.polisyClientId = asrh.ASRHList.polisyClntnum;
                data.generalHeader.emcsMemHeadId = asrh.ASRHList.emcsMemHeadId;
                data.generalHeader.emcsMemId = asrh.ASRHList.emcsMemId;

                data.profileInfo = new CRMInquiryClientProfileInfoModel();
                data.profileInfo.fullName = asrh.ASRHList.fullName;
                data.profileInfo.idTax = asrh.ASRHList.taxNo;
                data.profileInfo.corporateBranch = asrh.ASRHList.taxBranchCode;

                data.contactInfo = new CRMInquiryClientContactInfoModel();
                data.contactInfo.contactNumber = asrh.ASRHList.contactNumber;

                data.addressInfo = new CRMInquiryClientAddressInfoModel();
                data.addressInfo.address = asrh.ASRHList.address;

                data.asrhHeader = new CRMInquiryClientAsrhHeaderModel();
                data.asrhHeader.assessorFlag = asrh.ASRHList.assessorFlag;
                data.asrhHeader.solicitorFlag = asrh.ASRHList.solicitorFlag;
                data.asrhHeader.repairerFlag = asrh.ASRHList.repairerFlag;
                data.asrhHeader.hospitalFlag = asrh.ASRHList.businessType.ToUpper() == "H" ? "Y" : "N";


                trgt.data.Add(data);
            }

            return trgt;
        }
    }
}