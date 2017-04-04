using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using RPS = DEVES.IntegrationAPI.Model.RPS;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCLIENTCreateCorporateClientAndAdditionalInfoInputModel_to_RegPayeeCorporateInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLIENTCreateCorporateClientAndAdditionalInfoInputModel srccontect = (CLIENTCreateCorporateClientAndAdditionalInfoInputModel)input;
            RegPayeeCorporateInputModel trgtcontent = (RegPayeeCorporateInputModel)output;


            if (src.data != null)
                foreach (RPS.CLIENTCreateCorporateClientAndAdditionalInfoInputModel src in srccontect.data)
                {

                    RegPayeeCorporateInputModel trgt = new RegPayeeCorporateInputModel();
                    trgt.generalHeader = new RegPayeeCorporateInputModel(); 
                    trgt.profileHeader = new RegPayeeCorporateInputModel();
                    trgt.contentHeader = new RegPayeeCorporateInputModel();
                    trgt.addressHeader = new RegPayeeCorporateInputModel();
                    trgt.data = new RegPayeeCorporateInputModel(); 
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
                    trgt.addressHeader.longtitude = src.longtituge;

                    //src.data 

                    trgt.data.policyClientId = src.clientID;

                    trgtContent.data.Add(trgt);
                }
            


            return trgt;
        }
    }
}