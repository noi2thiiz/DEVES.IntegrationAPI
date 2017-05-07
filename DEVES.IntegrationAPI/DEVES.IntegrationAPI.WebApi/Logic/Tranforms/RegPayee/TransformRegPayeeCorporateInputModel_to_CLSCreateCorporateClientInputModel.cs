﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using System.Globalization;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;
using DEVES.IntegrationAPI.Model.CLS;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformRegPayeeCorporateInputModel_to_CLSCreateCorporateClientInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegPayeeCorporateInputModel src = (RegPayeeCorporateInputModel)input;
            CLSCreateCorporateClientInputModel trgt = (CLSCreateCorporateClientInputModel)output;

            if (src == null)
            {
                return trgt;
            }

            if (src.generalHeader != null)
            {
                trgt.roleCode = src.generalHeader.roleCode;
                trgt.clientId = src.generalHeader.polisyClientId;
                trgt.crmPersonId = src.generalHeader.crmClientId;
                trgt.cleansingId = src.generalHeader.cleansingId;
            }
            if (src.profileHeader != null)
            {
                trgt.corporateName1 = src.profileHeader.corporateName1;
                trgt.corporateName2 = src.profileHeader.corporateName2;
                trgt.contactPerson = src.profileHeader.contactPerson;
                trgt.idRegCorp = src.profileHeader.idRegCorp;
                trgt.idTax = src.profileHeader.idTax;
                //{
                //    CultureInfo usaCulture = new CultureInfo("en-US");
                //    var dateString = src.profileHeader.dateInCorporate.ToString("yyyyMMdd", usaCulture);
                //    trgt.dateInCorporate = dateString;
                //}
                trgt.dateInCorporate = src.profileHeader.dateInCorporate;
                trgt.corporateStaffNo = src.profileHeader.corporateBranch;
                trgt.corporateBranch = src.profileHeader.corporateBranch;
                trgt.econActivity = src.profileHeader.econActivity;
                trgt.language = src.profileHeader.language;
                trgt.vipStatus = src.profileHeader.vipStatus;

            }
            if  (src.contactHeader != null)
            {
                trgt.telephone1 = src.contactHeader.telephone1;
                trgt.telephone1Ext = src.contactHeader.telephone1Ext;
                trgt.telephone2 = src.contactHeader.telephone2;
                trgt.telephone2Ext = src.contactHeader.telephone2Ext;
                trgt.telNo = src.contactHeader.telephone3;
                trgt.telNoExt = src.contactHeader.telephone3Ext;
                trgt.mobilePhone = src.contactHeader.mobilePhone;
                trgt.fax = src.contactHeader.fax;
                trgt.emailAddress = src.contactHeader.emailAddress;
                trgt.lineID = src.contactHeader.lineID;
                trgt.facebook = src.contactHeader.facebook;
            }
            if (src.addressHeader != null)
            {
                trgt.address1 = src.addressHeader.address1;
                trgt.address2 = src.addressHeader.address2;
                trgt.address3 = src.addressHeader.address3;
                trgt.subDistrictCode = src.addressHeader.subDistrictCode;
                trgt.districtCode = src.addressHeader.districtCode;
                trgt.provinceCode = src.addressHeader.provinceCode;
                trgt.postalCode = src.addressHeader.postalCode;
                trgt.country = src.addressHeader.country;
                trgt.addressType = src.addressHeader.addressType;
                trgt.latitude = src.addressHeader.latitude;
                trgt.longigude = src.addressHeader.longtitude;
            }
            trgt.isPayee = "Y";
            trgt.OregNum = "";
            trgt.DelistFlag = "";
            trgt.BlackListFlag = "";
            trgt.TerminateDate = CommonConstant.GetDevesAPINullDate();
            return trgt;
        }
    }
}