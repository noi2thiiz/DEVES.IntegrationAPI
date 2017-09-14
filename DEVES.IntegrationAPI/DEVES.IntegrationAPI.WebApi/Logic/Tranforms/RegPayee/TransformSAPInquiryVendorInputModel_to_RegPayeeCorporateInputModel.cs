using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformSAPInquiryVendorInputModel_to_RegPayeeCorporateInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            //cant, map field
            SAPInquiryVendorInputModel src = (SAPInquiryVendorInputModel)input;
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