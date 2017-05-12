using System.Globalization;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegClientCorporate;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
   public class TranformRegClientCorporateInputModel_to_CLIENTCreateCorporateClientAndAdditionalInfoInputModel
 : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegClientCorporateInputModel src = (RegClientCorporateInputModel)input;
            CLIENTCreateCorporateClientAndAdditionalInfoInputModel trgt = (CLIENTCreateCorporateClientAndAdditionalInfoInputModel)output;



            if (src == null)
            {
                return trgt;
            }

            if (src.generalHeader != null)
            {

                trgt.cleansingId = src.generalHeader.cleansingId??"";
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
                if (string.IsNullOrEmpty(src.contactHeader.telephone1Ext))
                {
                    trgt.telephones = src.contactHeader.telephone1 ?? "";
                }
                else
                {
                    trgt.telephones = src.contactHeader.telephone1 + "#" + src.contactHeader.telephone1Ext ?? "";
                }
                if (string.IsNullOrEmpty(src.contactHeader.telephone2Ext))
                {
                    trgt.telephone2 = src.contactHeader.telephone2 ?? "";
                }
                else
                {
                    trgt.telephone2 = src.contactHeader.telephone2 + "#" + src.contactHeader.telephone2Ext ?? "";
                }
                if (string.IsNullOrEmpty(src.contactHeader.telephone3Ext))
                {
                    trgt.telex = src.contactHeader.telephone3 ?? "";
                }
                else
                {
                    trgt.telex = src.contactHeader.telephone3 + "#" + src.contactHeader.telephone3Ext ?? "";
                }

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
                string districtName = "";
                string subDistrictName = "";
                if (!string.IsNullOrEmpty(src.addressHeader.districtCode))
                {
                    var district = DistricMasterData.Instance.FindByCode(src.addressHeader.districtCode);
                    if (district != null)
                    {
                        districtName = DistricMasterData.Instance.GetNameWithPrefix(district);
                    }

                }


                if (!string.IsNullOrEmpty(src.addressHeader?.subDistrictCode))
                {
                    var subDistrict = SubDistrictMasterData.Instance.FindByCode(src.addressHeader.subDistrictCode);
                    if (subDistrict != null)
                    {
                        subDistrictName = SubDistrictMasterData.Instance.GetNameWithPrefix(subDistrict) ;
                    }
                }

                trgt.address4 = ("" + subDistrictName + " " + districtName).Trim();
                

                //provinceCode    String	2	O จังหวัด
                if (!string.IsNullOrEmpty(src.addressHeader?.provinceCode))
                {
                    var province = ProvinceMasterData.Instance.FindByCode(src.addressHeader.provinceCode);
                    if (province != null)
                    {
                        trgt.address5 = ProvinceMasterData.Instance.GetNameWithPrefix(province);
                    }
                }
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


            return trgt;

        }
    }
}