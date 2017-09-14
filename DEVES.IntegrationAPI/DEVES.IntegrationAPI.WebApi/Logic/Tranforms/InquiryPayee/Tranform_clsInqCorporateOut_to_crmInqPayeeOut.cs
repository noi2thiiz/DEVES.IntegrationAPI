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

            //retCLSInqCorpClient.AddDebugInfo("tranformer", "Tranform_clsInqCorporateOut_to_crmInqPayeeOut");
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
                            street1Text = clsAddress.address_1?.Trim() ?? "";
                            street2Text = clsAddress.address_2?.Trim() ?? "";
                            districtText = clsAddress.district_text?.Trim() ?? "";
                            provinceText = clsAddress.province_text?.Trim() ?? "";
                            postalCode = clsAddress.postal_code?.Trim() ?? "";
                            countryText = clsAddress.cls_ctrycode_text?.Trim() ?? "";
                            ctrymastercode = CountryMasterData.Instance.FindByPolisyCode(clsAddress?.ctrycode)?.CountryCode ?? "";
                            fullAddressText = clsAddress.full_original_address;

                            break;
                        }
                    }
                }

                var model = new InquiryCrmPayeeListDataModel
                {

                    sourceData = CommonConstant.CONST_SYSTEM_CLS,
                    cleansingId = clsData.cleansing_id?.Trim() ?? "",
                    clientType = clsData.clientType,
                    polisyClientId = clsData.clntnum?.Trim() ?? "",
                    sapVendorCode = "",
                    title = "",
                    name1 = clsData.lgivname?.Trim() ?? "",
                    name2 = clsData.lsurname?.Trim() ?? "",
                    // countryCode = clsData.c
                    telephone1 = clsData?.cltphone01?.Trim() ?? "",
                    telephone2 = clsData?.cltphone02?.Trim() ?? "",
                    contactNumber = clsData?.cls_display_phone?.Trim() ?? "",


                    fullName = clsData.cls_full_name?.Trim() ?? "",
                    taxNo = clsData.cls_tax_no_new?.Trim() ?? "",
                    //taxBranchCode = clsData.crm_ref_code1,
                    clientStatus =  clsData?.cltstat?.Trim().ToUpper() ?? "",
                    taxBranchCode = clsData.corporate_staff_no?.Trim() ?? "",



                    emcsMemHeadId = "",
                    emcsMemId = "",
                    street1 = street1Text?.Trim() ?? "",
                    street2 = street2Text?.Trim() ?? "",
                    district = districtText?.Trim() ?? "",
                    city = provinceText?.Trim() ?? "",

                    postalCode = postalCode?.Trim() ?? "",
                    countryCode = ctrymastercode?.Trim() ?? "",
                    countryCodeDesc = countryText?.Trim() ?? "",
                    address = fullAddressText?.Trim() ?? "",


                };
                //model.AddDebugInfo("CLS Source", clsData);
                //model.AddDebugInfo("tranformer", "Tranform_clsInqCorporateOut_to_crmInqPayeeOut");
                crmInqPayeeOut.data.Add(model);

            }

           // crmInqPayeeOut.AddDebugInfo("Tranformer", "Tranform_clsInqPersonalOut_to_crmInqPayeeOut");
            return crmInqPayeeOut;

        }

    }
}