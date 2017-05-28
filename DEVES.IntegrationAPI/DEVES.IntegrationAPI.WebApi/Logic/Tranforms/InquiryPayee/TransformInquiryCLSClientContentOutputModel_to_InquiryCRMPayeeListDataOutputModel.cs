using System;
using System.Collections.Generic;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class
        TransformCLSInquiryPersonalClientContentOutputModel_to_InquiryCRMPayeeListDataOutputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            // CLS.CLSInquiryPersonalClientContentOutputModel srcContent = (CLS.CLSInquiryPersonalClientContentOutputModel)input;
            CLSInquiryPersonalClientContentOutputModel srcContent = (CLSInquiryPersonalClientContentOutputModel)input;
            CRMInquiryPayeeContentOutputModel trgtContent = (CRMInquiryPayeeContentOutputModel)output;




            trgtContent.data = new List<InquiryCrmPayeeListDataModel>();
            trgtContent.AddDebugInfo("transformer", "TransformCLSInquiryPersonalClientContentOutputModel_to_InquiryCRMPayeeListDataOutputModel");



            foreach (var clsData in srcContent.data)
            {
                var model = new InquiryCrmPayeeListDataModel
                {

                    sourceData = "CLS",
                    cleansingId = clsData.cleansing_id,
                    polisyClientId = clsData.clntnum,
                    sapVendorCode = "",
                    fullName = clsData.cls_full_name,
                    taxNo = clsData.cls_fax,
                    taxBranchCode = "",
                    emcsMemHeadId = "",
                    emcsMemId = ""


                };
               // model.AddDebugInfo("transformer", "TransformCLSInquiryPersonalClientContentOutputModel_to_InquiryCRMPayeeListDataOutputModel");
               // model.AddDebugInfo("CLS Source", clsData);
                trgtContent.data.Add(model);

            }


            return trgtContent;
        }
    }
}