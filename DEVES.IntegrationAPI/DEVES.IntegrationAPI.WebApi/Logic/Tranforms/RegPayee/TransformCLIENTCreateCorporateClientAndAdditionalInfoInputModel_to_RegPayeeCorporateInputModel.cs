using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCLIENTCreateCorporateClientAndAdditionalInfoInputModel_to_RegPayeeCorporateInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLIENTCreateCorporateClientAndAdditionalInfoInputModel src = (CLIENTCreateCorporateClientAndAdditionalInfoInputModel)input;
            RegPayeeCorporateInputModel trgt = (RegPayeeCorporateInputModel)output;




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
            if (trgt.addressHeader == null)
            {
                trgt.addressHeader = new AddressHeaderModel();
            }
                   
                    
                   
                    
                    //generalHeader 

                    trgt.generalHeader.cleansingId = src.cleansingId;

                    //profileHeader 

                    trgt.profileHeader.corporateName1 = src.corporateName1;
                    trgt.profileHeader.corporateName2 = src.corporateName2;
                    trgt.profileHeader.contactPerson = src.remark;
                    trgt.profileHeader.idRegCorp = src.idCard;
                    trgt.profileHeader.idTax = src.taxId;
                    trgt.profileHeader.corporateBranch = src.corporateStaffNo;
                    trgt.profileHeader.econActivity = src.econActivity;
                    trgt.profileHeader.countryOrigin = src.countryOrigin;
                    trgt.profileHeader.language = src.language;
                    trgt.profileHeader.riskLevel = src.riskLevel;
                    trgt.profileHeader.vipStatus = src.vipStatus;

                    //src.contactHeader 

                    trgt.contactHeader.telephone1 = src.telephones;
                    trgt.contactHeader.telephone2 = src.telephone2;
                    trgt.contactHeader.telephone3 = src.telex;
                    trgt.contactHeader.mobilePhone = src.telegram;
                    trgt.contactHeader.fax = src.facsimile;
                    trgt.contactHeader.lineID = src.lineId;
                    trgt.contactHeader.facebook = src.facebook;

                    //src.addressHeader 

                    trgt.addressHeader.address1 = src.address1;
                    trgt.addressHeader.address2 = src.address2;
                    trgt.addressHeader.address3 = src.address3;
                    trgt.addressHeader.subDistrictCode = src.address4;
                    trgt.addressHeader.provinceCode = src.address5;
                    trgt.addressHeader.postalCode = src.postCode;
                    trgt.addressHeader.country = src.country;
                    trgt.addressHeader.latitude = src.latitude;
                    trgt.addressHeader.longtitude = src.longtitude;
            
               

            

            return trgt;
        }
    }
}