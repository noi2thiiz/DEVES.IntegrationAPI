using System.Globalization;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegClientCorporate;
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

                trgt.cleansingId = src.generalHeader.cleansingId;
                trgt.assessorFlag = src.generalHeader.assessorFlag;
                trgt.solicitorFlag = src.generalHeader.solicitorFlag;
                trgt.repairerFlag = src.generalHeader.repairerFlag;
                trgt.hospitalFlag = src.generalHeader.hospitalFlag;

            }

            if (src.profileHeader != null)
            {
                
                trgt.corporateName1 = src.profileHeader.corporateName1;
                trgt.corporateName2 = src.profileHeader.corporateName2;
                trgt.remark = src.profileHeader.contactPerson;
                trgt.idCard = src.profileHeader.idRegCorp;
                trgt.taxId = src.profileHeader.idTax;
               
                {
                    // to 20160303
                    CultureInfo usaCulture = new CultureInfo("en-US");
                    var dateString = src.profileHeader.dateInCorporate.ToString("ddMMyyy", usaCulture);
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
                trgt.country = src.addressHeader.country;

                trgt.latitude = src.addressHeader.latitude;
                trgt.longtitude = src.addressHeader.longtitude;

            }

            return trgt;

        }
    }
}