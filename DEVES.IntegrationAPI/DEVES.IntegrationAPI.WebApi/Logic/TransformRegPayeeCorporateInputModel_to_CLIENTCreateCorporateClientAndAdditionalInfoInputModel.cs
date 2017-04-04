using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;

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
            }
            if (src.profileHeader != null)
            {
                trgt.corporateName1 = src.profileHeader.corporateName1;
                trgt.corporateName2 = src.profileHeader.corporateName2;
                trgt.remark = src.profileHeader.contactPerson;
                trgt.idCard = src.profileHeader.idRegCorp;
                trgt.taxId = src.profileHeader.idTax;
                {
                    CultureInfo usaCulture = new CultureInfo("en-US");
                    var dateString = src.profileHeader.dateInCorporate.ToString("yyyyMMdd", usaCulture);
                    trgt.dateInCorporate = dateString;
                }
                trgt.corporateStaffNo = src.profileHeader.corporateBranch;
                trgt.econActivity = src.profileHeader.econActivity;
                trgt.countryOrigin = src.profileHeader.countryOrigin;
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
                trgt.country = src.addressHeader.country;
                trgt.latitude = src.addressHeader.latitude;
                trgt.longtituge = src.addressHeader.longtitude;
            }
            if (src.data != null)
            {
                trgt.clientID = src.data.policyClientId;
            }
            

            return trgt;
        }
    }
}