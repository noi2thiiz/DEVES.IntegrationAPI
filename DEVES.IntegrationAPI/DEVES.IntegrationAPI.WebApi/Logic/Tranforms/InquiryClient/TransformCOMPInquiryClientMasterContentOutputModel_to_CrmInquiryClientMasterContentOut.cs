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

            Console.WriteLine("  process:TransformCOMPInquiryClientMasterContentOutputModel_to_CrmInquiryClientMasterContentOut");
            /*
             * ToDo:    Correct the case that there are many records input
             *          1. Loop through the input, 
             *          2. create a new CRMInquiryClientOutputDataModel, 
             *          3. transfer data from input to CRMInquiryClientOutputDataModel
            */

            EWIResCOMPInquiryClientMasterContentModel srcContent = (EWIResCOMPInquiryClientMasterContentModel)input;
            CRMInquiryClientContentOutputModel trgtContent = (CRMInquiryClientContentOutputModel)output;
            if (trgtContent.data == null)
            {
                trgtContent.data = new List<CRMInquiryClientOutputDataModel>();
            }

            // Console.WriteLine(" >>>>>>>>>>>>>>>>>>>>>>srcContent>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            // Console.WriteLine(srcContent.ToJson());
            // Console.WriteLine("   >>>>>>>>>>>>>>>>>>trgtContent>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

            //Console.WriteLine(trgtContent.ToJson());
            if (srcContent.clientListCollection != null)
            {
                foreach (COMPInquiryClientMasterContentClientListModel src in srcContent.clientListCollection)
                {
                    CRMInquiryClientOutputDataModel trgt = new CRMInquiryClientOutputDataModel();
                    trgt.addressInfo = new CRMInquiryClientAddressInfoModel();
                    trgt.asrhHeader = new CRMInquiryClientAsrhHeaderModel();
                    trgt.contactInfo = new CRMInquiryClientContactInfoModel();
                    trgt.generalHeader = new CRMInquiryClientGeneralHeaderModel();
                    trgt.profileInfo = new CRMInquiryClientProfileInfoModel();

                    trgt.generalHeader.clientType = src.clientList.clientType;
                    trgt.generalHeader.cleansingId = src.clientList.cleansingId;
                    trgt.generalHeader.polisyClientId = src.clientList.clientNumber;
                    trgt.generalHeader.clientAdditionalExistFlag = src.clientList.additionalExistFlag;
                  
                    trgt.generalHeader.sourceData = CommonConstant.CONST_SYSTEM_POLISY400;


                    trgt.profileInfo.name1 = src.clientList.name1;
                    trgt.profileInfo.name2 = src.clientList.name2;
                    trgt.profileInfo.fullName = src.clientList.fullName;
                    trgt.profileInfo.salutationText = src.clientList.salutationText;
                    trgt.profileInfo.sex = src.clientList.sex;
                    trgt.profileInfo.idCard = src.clientList.idCard;
                    trgt.profileInfo.idPassport = src.clientList.passportId;
                    trgt.profileInfo.idAlien = src.clientList.alientId;
                    trgt.profileInfo.idDriving = src.clientList.driverlicense;
                    trgt.profileInfo.idTax = src.clientList.taxId;
                    trgt.profileInfo.corporateBranch = src.clientList.corporateStaffNo;
                    trgt.profileInfo.dateOfBirth = src.clientList.dateOfBirthDate;
                    trgt.profileInfo.dateOfDeath = src.clientList.dateOfDeathDate;
                    trgt.profileInfo.natioanalityText = src.clientList.natioanalityText;
                    trgt.profileInfo.marriedText = src.clientList.marriedText;
                    trgt.profileInfo.occupationText = src.clientList.occupationText;
                    trgt.profileInfo.econActivityText = src.clientList.econActivityText;
                    trgt.profileInfo.countryOriginText = src.clientList.countryOriginText;
                    trgt.profileInfo.riskLevelText = src.clientList.riskLevelText;
                    trgt.profileInfo.language = src.clientList.language;
                    trgt.profileInfo.vipStatus = src.clientList.vipStatus;
                    trgt.profileInfo.clientStatus = src.clientList.clientStatus;
                    trgt.profileInfo.remark = src.clientList.remark;

                    trgt.contactInfo.telephone1 = src.clientList.telephone1;
                    trgt.contactInfo.telephone2 = src.clientList.telephone2;
                    trgt.contactInfo.telephone3 = src.clientList.telex;
                    trgt.contactInfo.mobilePhone = src.clientList.telegram;
                    trgt.contactInfo.fax = src.clientList.facsimile;
                    trgt.contactInfo.emailAddress = src.clientList.emailAddress;
                    trgt.contactInfo.lineID = src.clientList.lineId;
                    trgt.contactInfo.facebook = src.clientList.facebook;

                    trgt.addressInfo.address = string.Join(CONST_CONCAT, src.clientList.address1
                                                                        , src.clientList.address2
                                                                        , src.clientList.address3
                                                                        , src.clientList.address4
                                                                        , src.clientList.address5
                                                                        , src.clientList.postCode);
                    trgt.addressInfo.countryText = src.clientList.countryText;
                    trgt.addressInfo.addressTypeText = src.clientList.busResText;
                    trgt.addressInfo.latitude = src.clientList.latitude;
                    trgt.addressInfo.longitude = src.clientList.longtitude;

                    trgt.asrhHeader.assessorFlag = src.clientList.assessorFlag;
                    trgt.asrhHeader.solicitorFlag = src.clientList.solicitorFlag;
                    trgt.asrhHeader.repairerFlag = src.clientList.repairerFlag;
                    trgt.asrhHeader.hospitalFlag = src.clientList.hospitalFlag?.ToUpper() == "Y" ? "Y" : "N";

                    if (trgt.generalHeader.clientType!="P")
                    {
                        trgt.profileInfo.salutationText = "";
                        trgt.profileInfo.sex = "";
                    }

                   

                    trgtContent.data.Add(trgt);
                }
            }
            return trgtContent;
        }
    }
}