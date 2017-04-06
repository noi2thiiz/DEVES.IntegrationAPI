using System;
using System.Collections.Generic;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformSAPInquiryVendorOutputModel_to_InquiryCRMPayeeListDataOutputModel : BaseTransformer
    {
        private Dictionary<string, SAPInquiryVendorContentVendorInfoModel> _tmpSAPInquiryVendorContentModel;
       
        public BaseDataModel TransformModel2(BaseDataModel input, BaseDataModel output)
        {
            Console.WriteLine(" process : TransformSAPInquiryVendorOutputModel_to_InquiryCRMPayeeListDataOutputModel");
            EWIResSAPInquiryVendorContentModel srcContent = (EWIResSAPInquiryVendorContentModel) input;
            CRMInquiryPayeeContentOutputModel trgtContent = (CRMInquiryPayeeContentOutputModel) output;

            Console.WriteLine(" >>>>>>>>>>>>>>>>>>>>>>>>>>>srcContent>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            Console.WriteLine(srcContent.ToJson());
            Console.WriteLine(" >>>>>>>>>>>>>>>>>>>>>>>>trgtContent>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            Console.WriteLine(trgtContent.ToJson());
            Console.WriteLine(" >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

            CRMInquiryPayeeContentOutputModel outputContent = new CRMInquiryPayeeContentOutputModel();
            outputContent.data = new List<InquiryCrmPayeeListDataModel>();

            _tmpSAPInquiryVendorContentModel = new Dictionary<string, SAPInquiryVendorContentVendorInfoModel>();
            
            foreach (var vendorInfo in srcContent.VendorInfo)
            {
                if (vendorInfo.PREVACC != null)
                {
                    _tmpSAPInquiryVendorContentModel.Add(vendorInfo.PREVACC, vendorInfo);
                }
            }
            if (trgtContent == null)
            {
                trgtContent = new CRMInquiryPayeeContentOutputModel();
            }
            if (trgtContent.data == null)
            {
                trgtContent.data = new List<InquiryCrmPayeeListDataModel>();
            }

            foreach (var dataItrm in trgtContent.data)
            {
                if (dataItrm.polisyClientId != null &&
                    _tmpSAPInquiryVendorContentModel.ContainsKey(dataItrm.polisyClientId))
                {
                    var _srcContentData = _tmpSAPInquiryVendorContentModel[dataItrm.polisyClientId];


                    outputContent.data.Add(TransformDataModel(_srcContentData, dataItrm));
                    _tmpSAPInquiryVendorContentModel.Remove(dataItrm.polisyClientId);
                }
                else
                {
                    outputContent.data.Add(dataItrm);
                }
               
            }
            foreach (var vendorItrm in _tmpSAPInquiryVendorContentModel)
            {
                outputContent.data.Add(TransformDataModel(vendorItrm.Value, new InquiryCrmPayeeListDataModel()));
            }



            return outputContent;
        }

        public InquiryCrmPayeeListDataModel TransformDataModel(SAPInquiryVendorContentVendorInfoModel input,
            InquiryCrmPayeeListDataModel output)
        {
            output.polisyClientId = input.PREVACC;
            output.sapVendorCode = input.VCODE;
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
            output.countryCode = input.COUNTRY;
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
                        bankAccount = bankInfoItem.ACCTHOLDER,
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

            return output;
        }
        public bankInfoModel TransformSAPInquiryVendorBankInfoModelToBankInfoModel(SAPInquiryVendorBankInfoModel input,
            bankInfoModel output)
        {
            return output;
        }

        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            Console.WriteLine(" process : TransformSAPInquiryVendorOutputModel_to_InquiryCRMPayeeListDataOutputModel");
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
                outputContent.data.Add(TransformDataModel(vendorInfo, new InquiryCrmPayeeListDataModel()));
            }
            return outputContent;
        }
    }
}