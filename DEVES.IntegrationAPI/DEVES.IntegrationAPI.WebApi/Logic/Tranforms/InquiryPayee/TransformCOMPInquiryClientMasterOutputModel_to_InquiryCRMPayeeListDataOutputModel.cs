﻿using System;
using System.Collections.Generic;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class
        TransformEWIResCOMPInquiryClientMasterContentModel_to_InquiryCRMPayeeListDataOutputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            // CLS.CLSInquiryPersonalClientContentOutputModel srcContent = (CLS.CLSInquiryPersonalClientContentOutputModel)input;
            EWIResCOMPInquiryClientMasterContentModel srcContent = (EWIResCOMPInquiryClientMasterContentModel) input;
            CRMInquiryPayeeContentOutputModel trgtContent = (CRMInquiryPayeeContentOutputModel) output;


    

            trgtContent.data = new List<InquiryCrmPayeeListDataModel>();



            foreach (var compData in srcContent.clientListCollection)
            {
                var dataItem = new InquiryCrmPayeeListDataModel
                {

                    sourceData = CommonConstant.CONST_SYSTEM_POLISY400,
                    cleansingId = compData.clientList.cleansingId,
                    polisyClientId = compData.clientList.clientNumber,
                    sapVendorCode = "",
                    fullName = compData.clientList.fullName,
                    taxNo = compData.clientList.taxId,
                    // taxBranchCode = compData.clientList.,
                    emcsMemHeadId = "",
                    emcsMemId = ""

                };
               // dataItem.AddDebugInfo("COMP JSON Source", compData);
               // dataItem.AddDebugInfo("Transformer", "TransformEWIResCOMPInquiryClientMasterContentModel_to_InquiryCRMPayeeListDataOutputModel");

                trgtContent.data.Add(dataItem);

            }


            return trgtContent;
        }
    }
}