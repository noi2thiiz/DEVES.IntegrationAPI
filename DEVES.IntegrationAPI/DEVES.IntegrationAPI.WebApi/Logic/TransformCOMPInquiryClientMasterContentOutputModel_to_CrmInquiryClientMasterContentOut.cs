using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.Templates;


namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCOMPInquiryClientMasterContentOutputModel_to_CrmInquiryClientMasterContentOut : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            /*
             * ToDo:    Correct the case that there are many records input
             *          1. Loop through the input, 
             *          2. create a new CRMInquiryClientOutputDataModel, 
             *          3. transfer data from input to CRMInquiryClientOutputDataModel
            */

            COMPInquiryClientMasterContentOutputModel srcContent = (COMPInquiryClientMasterContentOutputModel)input;
            COMPInquiryClientMasterClientListModel src = srcContent.clientListCollection.clientList.First<COMPInquiryClientMasterClientListModel>();
            CRMInquiryClientContentOutputModel trgtContent = (CRMInquiryClientContentOutputModel)output;
            CRMInquiryClientOutputDataModel trgt = trgtContent.data.First<CRMInquiryClientOutputDataModel>();

            trgt.generalHeader.clientType = src.clientType;
            trgt.generalHeader.cleansingId = src.cleansingId;
            trgt.generalHeader.polisyClientId = src.clientNumber;
            trgt.generalHeader.clientAdditionalExistFlag = src.additionalExistFlag;

            trgt.profileInfo.name1 = src.name1;
            trgt.profileInfo.name2 = src.name2;
            trgt.profileInfo.fullName = src.fullName;
            trgt.profileInfo.salutationText = src.salutationText;
            trgt.profileInfo.sex = src.sex;
            trgt.profileInfo.idCard = src.idCard;
            trgt.profileInfo.idPassport = src.passportId;
            trgt.profileInfo.idAlien = src.alientId;
            trgt.profileInfo.idDriving = src.driverlicense;
            trgt.profileInfo.idTax = src.taxId;
            trgt.profileInfo.corporateBranch = src.corporateStaffNo;
            trgt.profileInfo.dateOfBirth = src.dateOfBirth;
            trgt.profileInfo.dateOfDeath = src.dateOfDeath;
            trgt.profileInfo.natioanalityText = src.natioanalityText;
            trgt.profileInfo.marriedText = src.marriedText;
            trgt.profileInfo.occupationText = src.occupationText;
            trgt.profileInfo.econActivityText = src.econActivityText;
            trgt.profileInfo.countryOriginText = src.countryOriginText;
            trgt.profileInfo.riskLevelText = src.riskLevelText;
            trgt.profileInfo.language = src.language;
            trgt.profileInfo.vipStatus = src.vipStatus;
            trgt.profileInfo.clientStatus = src.clientStatus;
            trgt.profileInfo.remark = src.remark;

            trgt.contactInfo.telephone1 = src.telephone1;
            trgt.contactInfo.telephone2 = src.telephone2;
            trgt.contactInfo.telephone3 = src.telex;
            trgt.contactInfo.mobilePhone = src.telegram;
            trgt.contactInfo.fax = src.facsimile;
            trgt.contactInfo.emailAddress = src.emailAddress;
            trgt.contactInfo.lineID = src.lineId;
            trgt.contactInfo.facebook = src.facebook;

            trgt.addressInfo.address = string.Join(CONST_CONCAT, src.address1
                                                                , src.address2
                                                                , src.address3
                                                                , src.address4
                                                                , src.address5
                                                                , src.postCode);
            trgt.addressInfo.countryText = src.countryText;
            trgt.addressInfo.addressTypeText = src.busResText;
            trgt.addressInfo.latitude = src.latitude;
            trgt.addressInfo.longtitude = src.longtitude;

            trgt.asrhHeader.assessorFlag = src.assessorFlag;
            trgt.asrhHeader.solicitorFlag = src.solicitorFlag;
            trgt.asrhHeader.repairerFlag = src.repairerFlag;
            trgt.asrhHeader.hospitalFlag = src.hospitalFlag;

            return trgtContent;
        }
    }
}