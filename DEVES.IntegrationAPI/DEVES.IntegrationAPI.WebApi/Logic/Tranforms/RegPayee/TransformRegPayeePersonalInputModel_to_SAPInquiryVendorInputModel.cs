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
    public class TransformRegPayeePersonalInputModel_to_SAPInquiryVendorInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {

            // =====Dont Finish ======

            RegPayeePersonalInputModel src = (RegPayeePersonalInputModel)input;
            SAPInquiryVendorInputModel trgt = (SAPInquiryVendorInputModel)output;

            if (src == null)
            {
                return trgt;
            }

            if (src.generalHeader != null)
            {
                trgt.PREVACC = src.generalHeader.polisyClientId;
              //  trgt.TAX3 = src.sapVendorInfo.sapVendorCode;
            //    trgt.TAX4 = src.profileInfo.;
                //  SAPInqVendorIn.TAX3 = regPayeeCorporateInput.profileHeader.idTax ?? "";

                //  SAPInqVendorIn.TAX4 = regPayeeCorporateInput.profileHeader.corporateBranch ?? "";
                //  SAPInqVendorIn.PREVACC = regPayeeCorporateInput.generalHeader.polisyClientId ?? "";
                //  SAPInqVendorIn.VCODE = regPayeeCorporateInput.sapVendorInfo.sapVendorCode ?? "";
                trgt.VCODE = src.sapVendorInfo.sapVendorCode;
            }


            // =====Dont Finish ======


            return trgt;
        }
    }
}