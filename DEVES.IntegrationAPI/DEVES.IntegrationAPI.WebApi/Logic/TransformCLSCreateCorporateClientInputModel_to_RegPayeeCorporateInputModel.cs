using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCLSCreateCorporateClientInputModel_to_RegPayeeCorporateInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLSCreateCorporateClientInputModel src = (CLSCreateCorporateClientInputModel)input;
            RegPayeeCorporateInputModel trgt = (RegPayeeCorporateInputModel)output;

            if (src == null)
            {
                return trgt;
            }
            if (trgt.generalHeader == null)
            {
                trgt.generalHeader = new GeneralHeaderModel();
            }
            if (trgt.profileHeader == null)
            {
                trgt.profileHeader = new ProfileHeaderModel();
            }
            if (trgt.contactHeader == null)
            {
                trgt.contactHeader = new ContactHeaderModel();
            }
            if (trgt.sapVendorInfo ==null)
            {
                trgt.sapVendorInfo = new SapVendorInfoModel();
            }


            //====generalHeader====

            trgt.generalHeader.roleCode = src.roleCode;
            trgt.generalHeader.polisyClientId = src.clientId;
            trgt.generalHeader.crmClientId = src.crmPersonId;

            //====profileHeader====

            trgt.profileHeader.corporateName1 = src.corporateName1;
            trgt.profileHeader.corporateName2 = src.corporateName2;
            trgt.profileHeader.contactPerson = src.contactPerson;
            trgt.profileHeader.idRegCorp = src.idRegCorp;
            trgt.profileHeader.idTax = src.idTax;
            trgt.profileHeader.dateInCorporate = src.dateInCorporate;
            trgt.profileHeader.corporateBranch = src.corporateStaffNo;
            trgt.profileHeader.econActivity = src.econActivity;
            trgt.profileHeader.language = src.language;
            trgt.profileHeader.vipStatus = src.vipStatus;


            //====contactHeader=====

            trgt.contactHeader.telephone1 = src.telephone1;
            trgt.contactHeader.telephone1Ext = src.telephone1Ext;
            trgt.contactHeader.telephone2 = src.telephone2;
            trgt.contactHeader.telephone2Ext = src.telephone2Ext;
            trgt.contactHeader.telephone3 = src.telNo;
            trgt.contactHeader.telephone3Ext = src.telNoExt;
            trgt.contactHeader.mobilePhone = src.mobilePhone;
            trgt.contactHeader.fax = src.fax;
            trgt.contactHeader.emailAddress = src.emailAddress;
            trgt.contactHeader.lineID = src.lineID;
            trgt.contactHeader.facebook = src.facebook;

            //====addressHeade=====

            trgt.addressHeader.address1 = src.address1;
            trgt.addressHeader.address2 = src.address2;
            trgt.addressHeader.address3 = src.address3;
            trgt.addressHeader.subDistrictCode = src.subDistrictCode;
            trgt.addressHeader.districtCode = src.districtCode;
            trgt.addressHeader.provinceCode = src.provinceCode;
            trgt.addressHeader.postalCode = src.postalCode;
            trgt.addressHeader.country = src.country;
            trgt.addressHeader.addressType = src.addressType;
            trgt.addressHeader.latitude = src.latitude;
            trgt.addressHeader.longtitude = src.longigude;





            return trgt;
        }
    }
}