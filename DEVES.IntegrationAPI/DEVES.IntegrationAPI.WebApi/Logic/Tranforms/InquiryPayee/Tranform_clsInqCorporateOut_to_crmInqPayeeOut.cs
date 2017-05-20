using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Logic.Validator;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class Tranform_clsInqCorporateOut_to_crmInqPayeeOut : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLSInquiryCorporateClientContentOutputModel retCLSInqCorpClient = (CLSInquiryCorporateClientContentOutputModel)input;
            CRMInquiryPayeeContentOutputModel crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)output

                                                               ?? (CRMInquiryPayeeContentOutputModel)DataModelFactory.GetModel(typeof(CRMInquiryPayeeContentOutputModel));
            #region prevent null reference
            if (retCLSInqCorpClient?.data == null)
            {
                return crmInqPayeeOut;
            }
            if (crmInqPayeeOut.data == null)
            {
                crmInqPayeeOut.data = new List<InquiryCrmPayeeListDataModel>();
            }
            #endregion
            retCLSInqCorpClient.AddDebugInfo("tranformer", "Tranform_clsInqCorporateOut_to_crmInqPayeeOut");
            foreach (var clsData in retCLSInqCorpClient.data)
            {
                string street1Text = "";
                string street2Text = "";
                string districtText = "";
                string provinceText = "";
                string postalCode = "";
                string countryText = "";
                string fullAddressText = "";
                string ctrymastercode = "";

                int lastSeq = 0;

                //@TODO AdHoc Add CLS
                if (clsData.addressListsCollection != null)
                {
                    foreach (var clsAddress in clsData.addressListsCollection)
                    {
                        if (clsAddress.sequence_id > lastSeq)
                        {
                            lastSeq = clsAddress.sequence_id;
                        }
                    }

                    foreach (var clsAddress in clsData.addressListsCollection)
                    {
                        if (clsAddress.sequence_id == lastSeq)
                        {
                            street1Text = clsAddress.address_1;
                            street2Text = clsAddress.address_2;
                            districtText = clsAddress.district_text;
                            provinceText = clsAddress.province_text;
                            postalCode = clsAddress.postal_code;
                            countryText = clsAddress.cls_ctrycode_text;
                            ctrymastercode = CountryMasterData.Instance.FindByPolisyCode(clsAddress?.ctrycode)?.SapCode ?? "";
                            fullAddressText = clsAddress.full_original_address;

                            break;
                        }
                    }
                }

                var model = new InquiryCrmPayeeListDataModel
                {

                    sourceData = "CLS",
                    cleansingId = clsData.cleansing_id,
                    polisyClientId = clsData.clntnum,
                    sapVendorCode = "",
                    title = "",
                    name1 = clsData.lgivname,
                    name2 = clsData.lsurname,
                    // countryCode = clsData.c
                    telephone1 = clsData?.cltphone01,
                    telephone2 = clsData?.cltphone02,
                    contactNumber = clsData?.cls_display_phone,


                    fullName = clsData.cls_full_name,
                    taxNo = clsData.cls_tax_no_new,

                    emcsMemHeadId = "",
                    emcsMemId = "",
                    street1 = street1Text,
                    street2 = street2Text,
                    district = districtText,
                    city = provinceText,

                    postalCode = postalCode,
                    countryCode = ctrymastercode,
                    countryCodeDesc = countryText,
                    address = fullAddressText,


                };
                model.AddDebugInfo("CLS Source", clsData);
                model.AddDebugInfo("tranformer", "Tranform_clsInqCorporateOut_to_crmInqPayeeOut");
                crmInqPayeeOut.data.Add(model);

            }

            crmInqPayeeOut.AddDebugInfo("Tranformer", "Tranform_clsInqPersonalOut_to_crmInqPayeeOut");
            return crmInqPayeeOut;

        }

    }
}