using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;
using DEVES.IntegrationAPI.Model.CLS;
using System.Globalization;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformRegPayeePersonalInputModel_to_CLSCreatePersonalClientInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegPayeePersonalInputModel src = (RegPayeePersonalInputModel)input;
            CLSCreatePersonalClientInputModel trgt = (CLSCreatePersonalClientInputModel)output;



            if (src == null)
            {
                return trgt;
            }
            
            if (src.generalHeader != null)
            {

                trgt.roleCode = src.generalHeader.roleCode;
                trgt.clientId = src.generalHeader.polisyClientId;
                trgt.crmPersonId = src.generalHeader.crmPersonId;
            }

            if (src.profileInfo != null)
            {

                trgt.salutation = src.profileInfo.salutation;
                trgt.personalName = src.profileInfo.personalName;
                trgt.personalSurname = src.profileInfo.personalSurname;
                trgt.sex = src.profileInfo.sex;
                trgt.idCitizen = src.profileInfo.idCitizen;
                trgt.idPassport = src.profileInfo.idPassport;
                trgt.idAlien = src.profileInfo.idAlien;
                trgt.idDriving = src.profileInfo.idDriving;
                trgt.dtBirthDate = src.profileInfo.birthDate;
                trgt.natioanality = src.profileInfo.nationality;
                trgt.language = src.profileInfo.language;
                trgt.married = src.profileInfo.married;
                trgt.occupation = src.profileInfo.occupation;
                trgt.vipStatus = src.profileInfo.vipStatus;

            }
            if (src.contactInfo != null)
            {
                trgt.telephone1 = src.contactInfo.telephone1;
                trgt.telephone1Ext = src.contactInfo.telephone1Ext;
                trgt.telephone2 = src.contactInfo.telephone2;
                trgt.telephone2Ext = src.contactInfo.telephone2Ext;
                trgt.telNo = src.contactInfo.telephone3;
                trgt.telNoExt = src.contactInfo.telephone3Ext;

                trgt.mobilePhone = src.contactInfo.mobilePhone;
                trgt.fax = src.contactInfo.fax;
                trgt.emailAddress = src.contactInfo.emailAddress;
                trgt.lineID = src.contactInfo.lineID;
                trgt.facebook = src.contactInfo.facebook;


            }

            if (src.addressInfo != null)
            {
                trgt.address1 = src.addressInfo.address1;
                trgt.address2 = src.addressInfo.address2;
                trgt.address3 = src.addressInfo.address3;
                trgt.subDistrictCode = src.addressInfo.subDistrictCode;
                trgt.districtCode = src.addressInfo.districtCode;

                trgt.provinceCode = src.addressInfo.provinceCode;
                trgt.postalCode = src.addressInfo.postalCode;
                trgt.country = src.addressInfo.country;

                trgt.addressType = src.addressInfo.addressType;

                trgt.latitude = src.addressInfo.latitude;
                trgt.longtitude = src.addressInfo.longtitude;

            }

            trgt.isPayee = "Y";
            return trgt;

        }
    }
}