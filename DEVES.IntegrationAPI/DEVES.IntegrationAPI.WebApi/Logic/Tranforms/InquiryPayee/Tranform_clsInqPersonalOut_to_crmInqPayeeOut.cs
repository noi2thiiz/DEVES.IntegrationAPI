using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class Tranform_clsInqPersonalOut_to_crmInqPayeeOut : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLSInquiryPersonalClientContentOutputModel clsInqPersClient = (CLSInquiryPersonalClientContentOutputModel)input;
            CRMInquiryPayeeContentOutputModel crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)output

                                                        ?? (CRMInquiryPayeeContentOutputModel) DataModelFactory.GetModel(typeof(CRMInquiryPayeeContentOutputModel));
            #region prevent null reference
            if (clsInqPersClient?.data == null)
            {
                return crmInqPayeeOut;
            }
            if (crmInqPayeeOut.data == null)
            {
                crmInqPayeeOut.data = new List<InquiryCrmPayeeListDataModel>();
            }
            #endregion

            foreach (var clsData in clsInqPersClient.data)
            {
                crmInqPayeeOut.data.Add(new InquiryCrmPayeeListDataModel
                {

                    sourceData = "CLS",
                    cleansingId = clsData?.cleansing_id,
                    polisyClientId = clsData?.clntnum,
                    sapVendorCode = "",
                    sapVendorGroupCode = "",
                    
                    // emcsMemHeadId = ,
                    // emcsMemId ="",
                    //   companyCode ="",
                   // title = ,
                    name1 = clsData?.lgivname,
                    name2 = clsData?.lgivname,
                    fullName = clsData?.cls_full_name,
                    // street1="",
                    // street2 ="",
                    // district ="",
                    //city = client?.,
                   // postalCode = clsData?.cl,
                    //countryCode = clsData?.,
                  //  countryCodeDesc = clsData?.,
                    // address ="",
                    telephone1 = clsData?.cls_display_phone,
                  //  telephone2 = ,
                    faxNo = clsData?.cls_fax,
                    //  contactNumber ="",
                   // taxNo = ,
                    //taxBranchCode ="",
                    //paymentTerm="",
                    //paymentTermDesc ="",
                    //paymentMethods = ,
                    //inactive = ,
                    //assessorFlag =,
                    //solicitorFlag = ,
                    //repairerFlag = ,
                    //hospitalFlag = 

                });
            }
           


            return crmInqPayeeOut;

        }

    }
}