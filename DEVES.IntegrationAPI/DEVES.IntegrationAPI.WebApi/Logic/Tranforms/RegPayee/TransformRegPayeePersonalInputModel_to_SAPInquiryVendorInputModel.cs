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
                trgt.VCODE = src.generalHeader.cleansingId;
            }


            // =====Dont Finish ======


            return trgt;
        }
    }
}