using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using System.Globalization;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;
using DEVES.IntegrationAPI.Model.Polisy400;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformRegPayeeCorporateInputModel_to_CLIENTCreateCorporateClientAndAdditionalInfoInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegPayeeCorporateInputModel src = (RegPayeeCorporateInputModel)input;
            CLIENTCreateCorporateClientAndAdditionalInfoInputModel trgt = (CLIENTCreateCorporateClientAndAdditionalInfoInputModel)output;

            if (src == null)
            {
                return trgt;
            }

            if (src.generalHeader != null)
            {
                trgt.cleansingId = src.generalHeader.cleansingId;
                trgt.assessorFlag = src.generalHeader.roleCode == "A" ? "Y" : "N";
                trgt.solicitorFlag = src.generalHeader.roleCode == "S" ? "Y" : "N";
                trgt.repairerFlag = src.generalHeader.roleCode == "R" ? "Y" : "N";
                trgt.hospitalFlag = src.generalHeader.roleCode == "H" ? "Y" : "N";
            }
            if (src.profileHeader != null)
            {
                trgt.corporateName1 = src.profileHeader.corporateName1;
                trgt.corporateName2 = src.profileHeader.corporateName2;
                trgt.remark = src.profileHeader.contactPerson;
                trgt.idCard = src.profileHeader.idRegCorp;
                trgt.taxId = src.profileHeader.idTax;
                //{
                //    CultureInfo usaCulture = new CultureInfo("en-US");
                //    var dateString = src.profileHeader.dateInCorporate.ToString("yyyyMMdd", usaCulture);
                //    trgt.dateInCorporate = dateString;
                //}
                trgt.dateInCorporate = src.profileHeader.dateInCorporate;
                trgt.corporateStaffNo = src.profileHeader.corporateBranch;
                trgt.econActivity = src.profileHeader.econActivity;

                trgt.countryOrigin = src.profileHeader.countryOrigin ?? ""; // buzCountry.Instant.CountryList.FirstOrDefault(x => x.Code == src.profileHeader.countryOrigin)?.ctryPolisy??"";

                trgt.language = src.profileHeader.language;
                trgt.riskLevel = src.profileHeader.riskLevel;
                trgt.vipStatus = src.profileHeader.vipStatus;

            }
            if (src.contactHeader != null)
            {
                trgt.telephones = src.contactHeader.telephone1;
                trgt.telephone2 = src.contactHeader.telephone2;
                trgt.telex = src.contactHeader.telephone3;
                trgt.telegram = src.contactHeader.mobilePhone;
                trgt.facsimile = src.contactHeader.fax;
                trgt.emailAddress = src.contactHeader.emailAddress;
                trgt.lineId = src.contactHeader.lineID;
                trgt.facebook = src.contactHeader.facebook;
            }
            if (src.addressHeader != null)
            {
                trgt.address1 = src.addressHeader.address1;
                trgt.address2 = src.addressHeader.address2;
                trgt.address3 = src.addressHeader.address3;
                trgt.address4 = src.addressHeader.subDistrictCode;
                trgt.address5 = src.addressHeader.provinceCode;
                trgt.postCode = src.addressHeader.postalCode;

                trgt.country = src.addressHeader.country ?? ""; // buzCountry.Instant.CountryList.FirstOrDefault( x => x.Code == src.addressHeader.country)?.ctryPolisy ?? "";

                trgt.latitude = src.addressHeader.latitude;
                trgt.longtitude = src.addressHeader.longtitude;
            }

            trgt.fao = "";
            trgt.clientStatus = "";
            trgt.sTax = "";
            trgt.capital = "";
            trgt.mailing = "";
            trgt.directMail = "";
            trgt.taxInNumber = "";
            trgt.specialIndicator = "";

            trgt.passportId = "";
            trgt.alientId = "";
            trgt.driverlicense = "";

            trgt.solicitorBlackListFlag = "";
            trgt.solicitorDelistFlag = "";
            trgt.repairerTerminateDate = "";
            trgt.solicitorTerminateDate = CommonConstant.GetDevesAPINullDate();
            trgt.assessorTerminateDate = CommonConstant.GetDevesAPINullDate();
            trgt.solicitorOregNum = "";
            trgt.assessorDelistFlag = "";
            trgt.assessorOregNum = "";
            trgt.repairerOregNum = "";
            trgt.rerDelistFlag = "";
            trgt.assessorBlackListFlag = "";
            trgt.repairerBlackListFlag = "";

            return trgt;
        }
    }
}