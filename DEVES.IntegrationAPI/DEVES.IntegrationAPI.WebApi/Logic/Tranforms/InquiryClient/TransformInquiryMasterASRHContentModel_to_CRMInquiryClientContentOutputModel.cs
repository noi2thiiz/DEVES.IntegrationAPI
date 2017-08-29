using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformInquiryMasterASRHContentModel_to_CRMInquiryClientContentOutputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryMasterASRHContentModel src = (InquiryMasterASRHContentModel)input;
            CRMInquiryClientContentOutputModel trgt = (CRMInquiryClientContentOutputModel)output;

            if(trgt.data == null)
            {
                trgt.data=new List<CRMInquiryClientOutputDataModel>();
            }

            foreach (var asrh in src.ASRHListCollection)
            {
                CRMInquiryClientOutputDataModel data = new CRMInquiryClientOutputDataModel();
                data.generalHeader = new CRMInquiryClientGeneralHeaderModel();
                data.generalHeader.polisyClientId = asrh.ASRHList.polisyClntnum;
                data.generalHeader.emcsMemHeadId = asrh.ASRHList.emcsMemHeadId;
                data.generalHeader.emcsMemId = asrh.ASRHList.emcsMemId;
                data.generalHeader.sourceData = CommonConstant.CONST_SYSTEM_MASTER_ASRH;
                data.generalHeader.clientType = "C";

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


                //ตัด เบอร์โทร  "contactNumber": "T. 034845533 F. 034845533",

                //ตัด "fullName": "บริษัท พระราม 2 เซอร์เวย์ จำกัด",
                //ตัด "salutationText": "",

                trgt.data.Add(data);
            }

            return trgt;
        }
    }
}