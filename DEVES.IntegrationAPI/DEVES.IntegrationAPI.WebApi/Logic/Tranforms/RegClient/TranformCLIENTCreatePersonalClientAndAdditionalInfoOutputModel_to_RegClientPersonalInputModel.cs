using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformCLIENTCreatePersonalClientAndAdditionalInfoOutputModel_to_RegClientPersonalInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLIENTCreatePersonalClientAndAdditionalInfoContentModel src = (CLIENTCreatePersonalClientAndAdditionalInfoContentModel)input;
            RegClientPersonalInputModel trgt = (RegClientPersonalInputModel)output;

            if (src != null)

            {
                if (!string.IsNullOrEmpty(src.clientID)) trgt.generalHeader.polisyClientId = src.clientID;

                /* dont found filed to map
                {
                    if (trgt.generalHeader == null)
                    {
                        trgt.generalHeader = new GeneralHeaderModel();
                    }
                    if (trgt.profileInfo == null)
                    {
                        trgt.profileInfo = new ProfileInfoModel();
                    }
                    if (trgt.contactInfo == null)
                    {
                        trgt.contactInfo = new ContactInfoModel();
                    }
                    if (trgt.addressInfo == null)
                    {
                        trgt.addressInfo = new AddressInfoModel();
                    }


                    //====generalHeader====

                    trgt.generalHeader.polisyClientId = src.clientID;

                    //====profileInfo====

                    trgt.profileInfo.salutation = src.salutation;
                    trgt.profileInfo.personalName = src.personalName;
                    trgt.profileInfo.personalName = src.personalSurname;
                    trgt.profileInfo.sex = src.sex;
                    trgt.profileInfo.idCitizen = src.idCitizen;
                    trgt.profileInfo.idPassport = src.idPassport;
                    trgt.profileInfo.idAlien = src.idAlien;
                    trgt.profileInfo.idDriving = src.idDriving;
                    trgt.profileInfo.birthDate = src.birthDate;
                    trgt.profileInfo.nationality = src.natioanality;
                    trgt.profileInfo.language = src.language;
                    trgt.profileInfo.married = src.married;
                    trgt.profileInfo.occupation = src.occupation;
                    trgt.profileInfo.vipStatus = src.vipStatus;
                }
                */

            }

            return trgt;

        }

    }
}