using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformSAPInquiryVendorInputModel_to_RegPayeePersonalInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            //cant, map field
            SAPInquiryVendorInputModel src = (SAPInquiryVendorInputModel)input;
            RegPayeePersonalInputModel trgt = (RegPayeePersonalInputModel)output;

            if (src == null)
            {
                return trgt;
            }
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
            if (trgt.sapVendorInfo == null)
            {
                trgt.sapVendorInfo = new SapVendorInfoModel();
            }


            //====generalHeader====


            trgt.generalHeader.polisyClientId = src.PREVACC;
            trgt.generalHeader.cleansingId = src.VCODE;

            //====profileInfo====


            ///dont finish


            return trgt;
        }
    }
}