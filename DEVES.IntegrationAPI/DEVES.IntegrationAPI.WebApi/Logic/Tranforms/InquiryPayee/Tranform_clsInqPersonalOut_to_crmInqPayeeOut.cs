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
    public class Tranform_clsInqPersonalOut_to_crmInqPayeeOut : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLSInquiryPersonalClientContentOutputModel clsInqPersClient = (CLSInquiryPersonalClientContentOutputModel)input;
            CRMInquiryPayeeContentOutputModel crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)output

                                                               ?? (CRMInquiryPayeeContentOutputModel)DataModelFactory.GetModel(typeof(CRMInquiryPayeeContentOutputModel));
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
                string street1Text = "";
                string street2Text = "";
                string districtText = "";
                string provinceText = "";
                string postalCode = "";
                string countryCode = "";
                string countryText = "";
                string fullAddressText = "";

                var validator = new MasterDataValidator();

                int lastSeq = 0;

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
                            countryCode = CountryMasterData.Instance.FindByPolisyCode(clsAddress?.ctrycode)?.CountryCode??"";
                            countryText = clsAddress.cls_ctrycode_text;
                            fullAddressText = clsAddress.full_original_address;

                            break;
                        }
                    }
                }
                var model = new InquiryCrmPayeeListDataModel
                {

                    sourceData = "CLS",
                    cleansingId = clsData?.cleansing_id,
                    polisyClientId = clsData?.clntnum,
                    sapVendorCode = "",
                    sapVendorGroupCode = "",

                    // emcsMemHeadId = ,
                    // emcsMemId ="",
                    // companyCode ="",
                    title = validator.TryConvertSalutationCode("clsData.salutl", clsData?.salutl),
                    name1 = clsData?.lgivname,
                    name2 = clsData?.lsurname,
                    fullName = clsData?.cls_full_name,
                    street1 = street1Text,
                    street2 = street2Text,
                    district = districtText,
                    city = provinceText,
                    postalCode = postalCode,
                    countryCode = countryCode,
                    countryCodeDesc = countryText,
                    address = fullAddressText,
                    telephone1 = clsData?.cltphone01,
                    telephone2 = clsData?.cltphone02,
                    faxNo = clsData?.cls_fax,
                    contactNumber = clsData?.cls_display_phone,
                    //taxNo = ,
                    //taxBranchCode ="",
                    //paymentTerm="",
                    //paymentTermDesc ="",
                    //paymentMethods = ,
                    //inactive = ,
                    //assessorFlag =,
                    //solicitorFlag = ,
                    //repairerFlag = ,
                    //hospitalFlag =

                };
                model.AddDebugInfo("CLS JSON", clsData);
                crmInqPayeeOut.data.Add(model);

            }

            crmInqPayeeOut.AddDebugInfo("Tranformer", "Tranform_clsInqPersonalOut_to_crmInqPayeeOut");
            return crmInqPayeeOut;

        }

    }
}