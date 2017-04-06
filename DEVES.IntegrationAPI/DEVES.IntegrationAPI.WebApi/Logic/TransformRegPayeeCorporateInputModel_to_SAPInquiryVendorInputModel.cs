﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;
using DEVES.IntegrationAPI.Model.SAP;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformRegPayeeCorporateInputModel_to_SAPInquiryVendorInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {

            // =====Dont Finish ======

            RegPayeeCorporateInputModel src = (RegPayeeCorporateInputModel)input;
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