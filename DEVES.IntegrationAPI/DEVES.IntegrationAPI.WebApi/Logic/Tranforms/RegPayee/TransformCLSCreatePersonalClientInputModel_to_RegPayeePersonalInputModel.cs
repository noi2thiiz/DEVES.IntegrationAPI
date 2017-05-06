using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCLSCreatePersonalClientInputModel_to_RegPayeePersonalInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLSCreatePersonalClientInputModel src = (CLSCreatePersonalClientInputModel)input;
            RegPayeePersonalInputModel trgt = (RegPayeePersonalInputModel)output;


            if (src == null)
            {
                return trgt;
            }
            if (trgt.generalHeader == null)
            {
                trgt.generalHeader = new GeneralHeaderModel();
            }
            if (trgt.profileInfo == null)
            {
                trgt.profileInfo = new ProfileInfoModel();
            }
            if (trgt.contactInfo == null)
            {
                trgt.contactInfo = new ContactInfoModel();
            }
            if (trgt.sapVendorInfo == null)
            {
                trgt.sapVendorInfo = new SapVendorInfoModel();
            }


            //====generalHeader====

            trgt.generalHeader.roleCode = src.roleCode;
            trgt.generalHeader.polisyClientId = src.clientId;
            trgt.generalHeader.crmPersonId = src.crmPersonId;

            //====profileInfo====

            trgt.profileInfo.salutation = src.salutation;
            trgt.profileInfo.personalName = src.personalName;
            trgt.profileInfo.personalName = src.personalSurname;
            trgt.profileInfo.sex = src.sex;
            trgt.profileInfo.idCitizen = src.idCitizen;
            trgt.profileInfo.idPassport = src.idPassport;
            trgt.profileInfo.idAlien = src.idAlien;
            trgt.profileInfo.idDriving = src.idDriving; 
            trgt.profileInfo.birthDate = src.dtBirthDate;
            trgt.profileInfo.nationality = src.natioanality;
            trgt.profileInfo.language = src.language;
            trgt.profileInfo.married = src.married;
            trgt.profileInfo.occupation = src.occupation;
            trgt.profileInfo.vipStatus = src.vipStatus;



            //====contactInfo=====

            trgt.contactInfo.telephone1 = src.telephone1;
            trgt.contactInfo.telephone1Ext = src.telephone1Ext;
            trgt.contactInfo.telephone2 = src.telephone2;
            trgt.contactInfo.telephone2Ext = src.telephone2Ext;
            trgt.contactInfo.telephone3 = src.telNo;
            trgt.contactInfo.telephone3Ext = src.telNoExt;
            trgt.contactInfo.mobilePhone = src.mobilePhone;
            trgt.contactInfo.fax = src.fax;
            trgt.contactInfo.emailAddress = src.emailAddress;
            trgt.contactInfo.lineID = src.lineID;
            trgt.contactInfo.facebook = src.facebook;

            //====addressInfo=====

            trgt.addressInfo.address1 = src.address1;
            trgt.addressInfo.address2 = src.address2;
            trgt.addressInfo.address3 = src.address3;
            trgt.addressInfo.subDistrictCode = src.subDistrictCode;
            trgt.addressInfo.districtCode = src.districtCode;
            trgt.addressInfo.provinceCode = src.provinceCode;
            trgt.addressInfo.postalCode = src.postalCode;
            trgt.addressInfo.country = src.country;
            trgt.addressInfo.addressType = src.addressType;
            trgt.addressInfo.latitude = src.latitude;
            trgt.addressInfo.longtitude = src.longtitude;

            return trgt;
        }
    }
}