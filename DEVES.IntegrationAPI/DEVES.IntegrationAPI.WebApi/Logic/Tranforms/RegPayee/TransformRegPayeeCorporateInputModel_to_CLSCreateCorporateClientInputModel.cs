using System;
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
                trgt.roleCode = src.generalHeader.roleCode?.Trim()??"";
                trgt.clientId = src.generalHeader.polisyClientId?.Trim() ?? "";
                trgt.crmPersonId = src.generalHeader.crmClientId?.Trim() ?? "";
                trgt.cleansingId = src.generalHeader.cleansingId?.Trim() ?? "";
            }
            if (src.profileHeader != null)
            {
                trgt.corporateName1 = src.profileHeader.corporateName1?.Trim() ?? "";
                trgt.corporateName2 = src.profileHeader.corporateName2?.Trim() ?? "";
                trgt.contactPerson = src.profileHeader.contactPerson?.Trim() ?? "";
                trgt.idRegCorp = src.profileHeader.idRegCorp?.Trim() ?? "";
                trgt.idTax = src.profileHeader.idTax?.Trim() ?? "";
                //{
                //    CultureInfo usaCulture = new CultureInfo("en-US");
                //    var dateString = src.profileHeader.dateInCorporate.ToString("yyyyMMdd", usaCulture);
                //    trgt.dateInCorporate = dateString;
                //}
                trgt.dateInCorporate = src.profileHeader.dateInCorporate;
                trgt.corporateStaffNo = src.profileHeader.corporateBranch?.Trim() ?? "";
                trgt.corporateBranch = src.profileHeader.corporateBranch?.Trim() ?? "";
                trgt.econActivity = src.profileHeader.econActivity?.Trim() ?? "";
                trgt.language = src.profileHeader.language?.Trim() ?? "";
                trgt.vipStatus = src.profileHeader.vipStatus?.Trim() ?? "";

            }
            if  (src.contactHeader != null)
            {
                trgt.telephone1 = src.contactHeader.telephone1?.Trim() ?? "";
                trgt.telephone1Ext = src.contactHeader.telephone1Ext?.Trim() ?? "";
                trgt.telephone2 = src.contactHeader.telephone2?.Trim() ?? "";
                trgt.telephone2Ext = src.contactHeader.telephone2Ext?.Trim() ?? "";
                trgt.telNo = src.contactHeader.telephone3?.Trim() ?? "";
                trgt.telNoExt = src.contactHeader.telephone3Ext?.Trim() ?? "";
                trgt.mobilePhone = src.contactHeader.mobilePhone?.Trim() ?? "";
                trgt.fax = src.contactHeader.fax?.Trim() ?? "";
                trgt.emailAddress = src.contactHeader.emailAddress?.Trim() ?? "";
                trgt.lineID = src.contactHeader.lineID?.Trim() ?? "";
                trgt.facebook = src.contactHeader.facebook?.Trim() ?? "";
            }
            if (src.addressHeader != null)
            {
                trgt.address1 = src.addressHeader.address1?.Trim() ?? "";
                trgt.address2 = src.addressHeader.address2?.Trim() ?? "";
                trgt.address3 = src.addressHeader.address3?.Trim() ?? "";
                trgt.subDistrictCode = src.addressHeader.subDistrictCode?.Trim() ?? "";
                trgt.districtCode = src.addressHeader.districtCode?.Trim() ?? "";
                trgt.provinceCode = src.addressHeader.provinceCode?.Trim() ?? "";
                trgt.postalCode = src.addressHeader.postalCode?.Trim() ?? "";
                trgt.country = src.addressHeader.country?.Trim() ?? "";
                trgt.addressType = src.addressHeader.addressType?.Trim() ?? "";
                trgt.latitude = src.addressHeader.latitude?.Trim() ?? "";
                trgt.longitude = src.addressHeader.longtitude?.Trim() ?? "";
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