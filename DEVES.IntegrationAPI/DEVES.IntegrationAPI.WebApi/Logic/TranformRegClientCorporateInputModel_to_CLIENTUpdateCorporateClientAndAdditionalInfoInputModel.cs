using System;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegClientCorporate;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformRegClientCorporateInputModel_to_CLIENTUpdateCorporateClientAndAdditionalInfoInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegClientCorporateInputModel src = (RegClientCorporateInputModel)input;
            CLIENTUpdateCorporateClientAndAdditionalInfoInputModel trgt = (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)output;

            if (src == null)
            {
                return trgt;
            }

            if (src.generalHeader != null)
            {

                trgt.cleansingId = src.generalHeader.cleansingId ?? "";
                trgt.assessorFlag = src.generalHeader.roleCode == "A" ? "Y" : "N";
                trgt.solicitorFlag = src.generalHeader.roleCode == "S" ? "Y" : "N";
                trgt.repairerFlag = src.generalHeader.roleCode == "R" ? "Y" : "N";
                trgt.hospitalFlag = src.generalHeader.roleCode == "H" ? "Y" : "N";

            }

            if (src.profileHeader != null)
            {

                trgt.corporateName1 = src.profileHeader.corporateName1 ?? "";
                trgt.corporateName2 = src.profileHeader.corporateName2 ?? "";
                trgt.remark = src.profileHeader.contactPerson ?? "";
                trgt.idCard = src.profileHeader.idRegCorp ?? "";
                trgt.taxId = src.profileHeader.idTax ?? "";

                trgt.dateInCorporateDate = src.profileHeader.dateInCorporate;

                trgt.corporateStaffNo = src.profileHeader.corporateBranch ?? "";
                trgt.econActivity = src.profileHeader.econActivity ?? "";
                trgt.countryOrigin = src.profileHeader.countryOrigin ?? "";
                trgt.language = src.profileHeader.language ?? "";
                trgt.riskLevel = src.profileHeader.riskLevel ?? "";
                trgt.vipStatus = src.profileHeader.vipStatus ?? "";

            }
            if (src.contactHeader != null)
            {
                trgt.telephones = src.contactHeader.telephone1 ?? "";

                trgt.telephone2 = src.contactHeader.telephone2 ?? "";

                trgt.telex = src.contactHeader.telephone3 ?? "";

                trgt.telegram = src.contactHeader.mobilePhone ?? "";
                trgt.facsimile = src.contactHeader.fax ?? "";
                trgt.emailAddress = src.contactHeader.emailAddress ?? "";
                trgt.lineId = src.contactHeader.lineID ?? "";
                trgt.facebook = src.contactHeader.facebook ?? "";

            }

            if (src.addressHeader != null)
            {
                trgt.address1 = src.addressHeader.address1 ?? "";
                trgt.address2 = src.addressHeader.address2 ?? "";
                trgt.address3 = src.addressHeader.address3 ?? "";
                trgt.address4 = src.addressHeader.districtCode ?? "" + " " + src.addressHeader.subDistrictCode ?? "";

                trgt.address5 = src.addressHeader.provinceCode ?? "";
                trgt.postCode = src.addressHeader.postalCode ?? "";
                trgt.country = src.addressHeader.country ?? "";

                trgt.latitude = src.addressHeader.latitude ?? "";
                trgt.longtitude = src.addressHeader.longtitude ?? "";

            }

            if (src.asrhHeader != null)
            {
                trgt.assessorOregNum = src.asrhHeader.assessorOregNum ?? "";
                trgt.assessorBlackListFlag = src.asrhHeader.assessorBlackListFlag ?? "";
                trgt.assessorDelistFlag = src.asrhHeader.assessorDelistFlag ?? "";
                trgt.assessorTerminateDate = src.asrhHeader.assessorTerminateDate ?? "";

                trgt.repairerTerminateDate = src.asrhHeader.repairerTerminateDate ?? "";
                trgt.repairerBlackListFlag = src.asrhHeader.repairerBlackListFlag ?? "";
                trgt.repairerOregNum = src.asrhHeader.repairerOregNum ?? "";
                trgt.repairerDelistFlag = src.asrhHeader.repairerDelistFlag ?? "";

                trgt.solicitorOregNum = src.asrhHeader.solicitorOregNum ?? "";
                trgt.solicitorBlackListFlag = src.asrhHeader.solicitorBlackListFlag ?? "";
                trgt.solicitorDelistFlag = src.asrhHeader.solicitorDelistFlag ?? "";
                trgt.solicitorTerminateDate = src.asrhHeader.solicitorTerminateDate ?? "";

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

            trgt.clientNumber = src.generalHeader.polisyClientId;

            return trgt;
        }
    }
}