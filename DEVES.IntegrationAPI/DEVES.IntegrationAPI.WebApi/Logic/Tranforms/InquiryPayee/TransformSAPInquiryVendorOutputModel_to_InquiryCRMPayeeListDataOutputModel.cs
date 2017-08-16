using System;
using System.Collections.Generic;
using System.Linq;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.Ajax.Utilities;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformSAPInquiryVendorOutputModel_to_InquiryCRMPayeeListDataOutputModel : BaseTransformer
    {
        private Dictionary<string, SAPInquiryVendorContentVendorInfoModel> _tmpSAPInquiryVendorContentModel;
        private Dictionary<string, InquiryCrmPayeeListDataModel> _tmpOutPutModel;
       
    
        public InquiryCrmPayeeListDataModel TransformDataModel(SAPInquiryVendorContentVendorInfoModel input,
            InquiryCrmPayeeListDataModel output)
        {
            output.sourceData = CommonConstant.CONST_SYSTEM_SAP;
            output.polisyClientId = input.PREVACC;
            output.sapVendorCode = input.VCODE;
            output.sapVendorAccountCode = input.PREVACC;
            output.sapVendorPayterm = input.PAYTERM;
            output.sapVendorGroupCode = input.VGROUP;
            //dataItrm.emcsMemHeadId = "";
            //dataItrm.emcsMemId = "";
            output.companyCode = input.COMPANY;
            output.title = input.TITLE;
            output.name1 = input.NAME1;
            output.name2 = input.NAME2; 
            if (string.IsNullOrEmpty(output.fullName ))
            {
                output.fullName = input.NAME1 + " " +input.NAME2;
            }

            // dataItrm.fullName = "";
            output.street1 = input.STREET1;
            output.street2 = input.STREET2;
            output.district = input.DISTRICT;
            

            output.city = input.CITY;
            output.postalCode = input.POSTCODE;
            output.countryCode = CountryMasterData.Instance.FindBySapCode(input.COUNTRY)?.CountryCode ?? "";
            output.countryCodeDesc = input.COUNTRY_DESC;
            //dataItrm.address = "";
            output.telephone1 = input.TEL1;
            output.telephone2 = input.TEL2;

            output.faxNo = input.FAX;
            //dataItrm.contactNumber = "";
            output.taxNo = input.TAX3;
            output.taxBranchCode = input.TAX4;


            output.paymentTerm = input.PAYTERM;
            output.paymentTermDesc = input.PAYTERM_DESC;
            output.paymentMethods = input.PAYMETHOD;
            output.inactive = input.INACTIVE;
            if (output.street1 == "ไม่แจ้งที่อยู่")
            {
                output.address = "ไม่แจ้งที่อยู่";
            }
            else
            {
                output.address = (output.street1 + " " + output.street2 + " " + " " + input.DISTRICT + " " + output.city + " "
                                  + output.postalCode ).ReplaceMultiplSpacesWithSingleSpace();
            }
            

            if (input.BankInfo != null)
            {
                output.bankInfo = new List<bankInfoModel>();
                foreach (var bankInfoItem in input.BankInfo)
                {
                    output.bankInfo.Add(new bankInfoModel
                    {
                        bankCountryCode = bankInfoItem.CTRY,
                        bankCode = bankInfoItem.BANKCODE,
                        bankName = "",
                        bankBranchCode = bankInfoItem.BANKBRANCH,
                        bankBranchDesc = "",
                        bankAccount = bankInfoItem.BANKACC,
                        accountHolder = bankInfoItem.ACCTHOLDER,
                    });
                }
            }

            if (input.WHTInfo != null)
            {
                output.withHoldingTaxInfo = new List<withHoldingTaxInfoModel>();
                foreach (var wHTInfo in input.WHTInfo)
                {
                    output.withHoldingTaxInfo.Add(new withHoldingTaxInfoModel
                    {
                        whtCountryCode = wHTInfo.WHTCTRY,
                        whtTaxType = wHTInfo.WHTTYPE,
                        whtTaxTypeDecs = wHTInfo.WHTTYPE_DESC,
                        whtTaxCode = wHTInfo.WHTCODE,
                        whtTaxCodeDesc = wHTInfo.WHTCODE_DESC,
                        receiptType = wHTInfo.RECPTYPE,
                        receiptTypeDesc = wHTInfo.RECPTYPE_DESC
                    });
                }
            }
           // output.AddDebugInfo("SAP JSON Source", input);
           // output.AddDebugInfo("Transformer", "TransformSAPInquiryVendorOutputModel_to_InquiryCRMPayeeListDataOutputModel");
            return output;
        }
        public bankInfoModel TransformSAPInquiryVendorBankInfoModelToBankInfoModel(SAPInquiryVendorBankInfoModel input,
            bankInfoModel output)
        {
            return output;
        }
      

        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            Console.WriteLine(" process :x TransformSAPInquiryVendorOutputModel_to_InquiryCRMPayeeListDataOutputModel");
            
            EWIResSAPInquiryVendorContentModel srcContent = (EWIResSAPInquiryVendorContentModel)input;
            CRMInquiryPayeeContentOutputModel outputContent;

            

            if (output != null)
            {
                outputContent = (CRMInquiryPayeeContentOutputModel)output;
                if (outputContent.data == null)
                    outputContent.data = new List<InquiryCrmPayeeListDataModel>();
            }
            else
            {
                outputContent = new CRMInquiryPayeeContentOutputModel();
                outputContent.data = new List<InquiryCrmPayeeListDataModel>();
            }
            

            foreach (var vendorInfo in srcContent.VendorInfo)
            {
                
                outputContent.data.Add(TransformDataModel(vendorInfo, new InquiryCrmPayeeListDataModel { sourceData = "SAP" }));
            }
            
           // outputContent.data = outputContent.data.DistinctBy(row => new { row.sourceData, row.sapVendorCode,row.polisyClientId, row.cleansingId }).ToList();
            /*
            List<Product> result = pr.GroupBy(g => new { g.Title, g.Price })
                .Select(g => g.First())
                .ToList();
                */
           // outputContent.AddDebugInfo("Transformer", "TransformSAPInquiryVendorOutputModel_to_InquiryCRMPayeeListDataOutputModel");
            return outputContent;
        }

    }
}