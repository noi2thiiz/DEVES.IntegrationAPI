using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegClientCorporate;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformRegClientCorporateInputModel_to_CLSCreateCorporateClientInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegClientCorporateInputModel src = (RegClientCorporateInputModel) input;
            CLSCreateCorporateClientInputModel trgt = (CLSCreateCorporateClientInputModel) output;

            if (src == null)
            {
                return trgt;
            }
            // prevent null input
            if (trgt == null)
            {
                trgt = new CLSCreateCorporateClientInputModel();
            }
            if (src.generalHeader == null)
            {
                src.generalHeader = new GeneralHeaderModel();
            }
            if (src.profileHeader == null)
            {
                src.profileHeader = new ProfileHeaderModel();
            }
            if (src.contactHeader == null)
            {
                src.contactHeader = new ContactHeaderModel();
            }
            if (src.addressHeader == null)
            {
                src.addressHeader = new AddressHeaderModel();
            }

            // Tranform

            //generalHeader
            trgt.roleCode = src.generalHeader.roleCode;
            trgt.clientId = src.generalHeader.polisyClientId;
            trgt.crmPersonId = src.generalHeader.crmClientId;

            //profileHeader
            trgt.corporateName1 = src.profileHeader.corporateName1;
            trgt.corporateName2 = src.profileHeader.corporateName2;
            trgt.contactPerson = src.profileHeader.contactPerson;
            trgt.idRegCorp = src.profileHeader.idRegCorp;
            trgt.idTax = src.profileHeader.idTax;
            trgt.dateInCorporate = src.profileHeader.dateInCorporate;
            trgt.corporateBranch = src.profileHeader.corporateBranch;
            trgt.econActivity = src.profileHeader.econActivity;
            trgt.language = src.profileHeader.language;
            trgt.vipStatus = src.profileHeader.vipStatus;

            //contactHeader
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

            //address header

            trgt.address1 = src.addressHeader.address1;
            trgt.address2 = src.addressHeader.address2;
            trgt.address3 = src.addressHeader.address3;
            trgt.subDistrictCode = src.addressHeader.subDistrictCode;
            trgt.districtCode = src.addressHeader.districtCode;
            trgt.provinceCode = src.addressHeader.provinceCode;
            trgt.addressType = src.addressHeader.addressType;
            trgt.latitude = src.addressHeader.latitude;
            trgt.longigude = src.addressHeader.longtitude;


            return trgt;
        }
    }
}