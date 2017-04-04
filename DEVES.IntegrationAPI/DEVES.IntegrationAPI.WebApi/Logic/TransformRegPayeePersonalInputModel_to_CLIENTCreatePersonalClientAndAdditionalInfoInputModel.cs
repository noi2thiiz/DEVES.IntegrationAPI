using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;
using DEVES.IntegrationAPI.Model.Polisy400;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformRegPayeePersonalInputModel_to_CLIENTCreatePersonalClientAndAdditionalInfoInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegPayeePersonalInputModel src = (RegPayeePersonalInputModel)input;
            CLIENTCreatePersonalClientAndAdditionalInfoInputModel trgt = (CLIENTCreatePersonalClientAndAdditionalInfoInputModel)output;



            if (src == null)
            {
                return trgt;
            }
            /*
            if (src.generalHeader != null)
            {
                trgt.cleansingId = src.generalHeader.cleansingId;
            }
            if (src.profileInfo != null)
            {
                trgt.salutation = src.profileInfo.salutation;
                trgt.personalName = src.profileInfo.personalName;
                trgt.personalSurname = src.profileInfo.personalSurname;
                trgt.sex = src.profileinfo.sex;
                trgt.idCard = src.profileInfo.idCitizen;
                trgt.passportId = src.profileInfo.idPassport;
                trgt.alientId = src.profileInfo.idAlien;
                trgt.driverlicense = src.profileInfo.idDriving;
                trgt.birthDate = src.profileInfo.birthDate;
                trgt.natioanality = src.profileInfo.nationality;
                trgt.language = src.profileInfo.language;
                trgt.married = src.profileInfo.married;
                trgt.occupation = src.profileInfo.occupation;
                trgt.riskLevel = src.profileInfo.riskLevel;
                trgt.vipStatus = src.profileInfo.vipStatus;

            }
            if (src.contactInfo != null)
            {
                trgt.telephone1 = src.contactInfo.telephone1;
                trgt.telephone2 = src.contactInfo.telephone3;
                trgt.telNo = src.contactInfo.telephone3;
                trgt.mobilePhone = src.contactInfo.mobilePhone;
                trgt.fax = src.contactInfo.fax;
                trgt.emailAddress = src.contactInfo.emailAddress;
                trgt.lineId = src.contactInfo.lineID;
                trgt.facebook = src.contactInfo.facebook;
            }
            if (src.addressInfo != null)
            {
                trgt.address1 = src.addressInfo.address1;
                trgt.address2 = src.addressInfo.address2;
                trgt.address3 = src.addressInfo.address3;
                trgt.address4 = src.addressInfo.subDistrictCode;
                trgt.address5 = src.addressInfo.provinceCode;
                trgt.postCode = src.addressInfo.postalCode;
                trgt.country = src.addressInfo.country;
                trgt.busRes = src.addressInfo.addressType;
                trgt.latitude = src.addressInfo.latitude;
                trgt.longtituge = src.addressInfo.longtitude;
            }
            */
            return trgt;
        }

    }
}