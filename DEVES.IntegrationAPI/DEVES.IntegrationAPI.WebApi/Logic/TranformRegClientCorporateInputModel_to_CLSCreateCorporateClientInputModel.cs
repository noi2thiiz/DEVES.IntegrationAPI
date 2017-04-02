using System;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformRegClientCorporateInputModel_to_CLSCreateCorporateClientInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegClientPersonalInputModel src = (RegClientPersonalInputModel)input;
            CLSCreateCorporateClientInputModel trgt = (CLSCreateCorporateClientInputModel)output;

            if (src == null)
            {
                return trgt;
            }
            if (trgt==null)
            {
                trgt = new CLSCreateCorporateClientInputModel();
            }
            if (src.generalHeader==null){src.generalHeader=new GeneralHeaderModel();}
            if (src.profileInfo == null) { src.profileInfo = new ProfileInfoModel();}
            if (src.addressInfo == null) { src.addressInfo = new AddressInfoModel();}
            if (src.contactInfo == null) { src.contactInfo = new ContactInfoModel();}

            trgt.roleCode = src.generalHeader.roleCode;
            trgt.clientId = src.generalHeader.polisyClientId;
            trgt.crmPersonId = src.generalHeader.crmClientId;

           // trgt.corporateName1 = src.profile


            return trgt;

        }
    }
}