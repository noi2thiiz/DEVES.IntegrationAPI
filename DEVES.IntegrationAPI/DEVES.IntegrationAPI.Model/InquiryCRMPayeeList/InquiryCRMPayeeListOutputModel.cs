using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.InquiryCRMPayeeList
{
    class InquiryCRMPayeeListOutputModel
    {
    }

    public class InquiryCRMPayeeListOutputModel_Pass
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public List<InquiryCRMPayeeListDataOutputModel_Pass> data { get; set; }
    }

    public class InquiryCRMPayeeListDataOutputModel_Pass
    {
        public string polisyClientId { get; set; } = "";
        public string sapVendorCode { get; set; } = "";
        public string sapVendorGroupCode { get; set; } = "";
        public string emcsMemHeadId { get; set; } = "";
        public string companyCode { get; set; } = "";
        public string title { get; set; } = "";
        public string name1 { get; set; } = "";
        public string name2 { get; set; } = "";
        public string fullName { get; set; } = "";
        public string street1 { get; set; } = "";
        public string street2 { get; set; } = "";
        public string district { get; set; } = "";
        public string city { get; set; } = "";
        public string postalCode { get; set; } = "";
        public string countryCode { get; set; } = "";
        public string countryCodeDesc { get; set; } = "";
        public string address { get; set; } = "";
        public string telephone1 { get; set; } = "";
        public string telephone2 { get; set; } = "";
        public string faxNo { get; set; } = "";
        public string contactNumber { get; set; } = "";
        public string taxNo { get; set; } = "";
        public string taxBranchCode { get; set; } = "";
        public string paymentTerm { get; set; } = "";
        public string paymentTermDesc { get; set; } = "";
        public string paymentMethods { get; set; } = "";
        public string inactive { get; set; } = "";
        public string assessorFlag { get; set; } = "";
        public string solicitorFlag { get; set; } = "";
        public string repairerFlag { get; set; } = "";
        public string hospitalFlag { get; set; } = "";

        public List<InquiryCRMPayeeListBankInfoOutputModel> bankInfo { get; set; }
        public List<InquiryCRMPayeeListWithHoldingTaxInfoOutputModel> withHoldingTaxInfo { get; set; }

        public InquiryCRMPayeeListDataOutputModel_Pass()
        {
            polisyClientId = "";
            sapVendorCode = "";
            sapVendorGroupCode = "";
            emcsMemHeadId = "";
            companyCode = "";
            title = "";
            name1 = "";
            name2 = "";
            fullName = "";
            street1 = "";
            street2 = "";
            district = "";
            city = "";
            postalCode = "";
            countryCode = "";
            countryCodeDesc = "";
            address = "";
            telephone1 = "";
            telephone2 = "";
            faxNo = "";
            contactNumber = "";
            taxNo = "";
            taxBranchCode = "";
            paymentTerm = "";
            paymentTermDesc = "";
            paymentMethods = "";
            inactive = "";
            assessorFlag = "";
            solicitorFlag = "";
            repairerFlag = "";
            hospitalFlag = "";
            polisyClientId = "";
            polisyClientId = "";

            bankInfo = new List<InquiryCRMPayeeListBankInfoOutputModel>();
            bankInfo.Add(new InquiryCRMPayeeListBankInfoOutputModel());

            withHoldingTaxInfo = new List<InquiryCRMPayeeListWithHoldingTaxInfoOutputModel>();
            withHoldingTaxInfo.Add(new InquiryCRMPayeeListWithHoldingTaxInfoOutputModel());
        }

    }

    public class InquiryCRMPayeeListBankInfoOutputModel
    {
        public string bankCountryCode { get; set; }
        public string bankCode { get; set; }
        public string bankName { get; set; }
        public string bankBranchCode { get; set; }
        public string bankBranchDesc { get; set; }
        public string bankAccount { get; set; }
        public string accountHolder { get; set; }

        public InquiryCRMPayeeListBankInfoOutputModel()
        {
            bankCountryCode = "";
            bankCode = "";
            bankName = "";
            bankBranchCode = "";
            bankBranchDesc = "";
            bankAccount = "";
            accountHolder = "";
        }
    }

    public class InquiryCRMPayeeListWithHoldingTaxInfoOutputModel
    {
        public string whtCountryCode { get; set; }
        public string whtTaxType { get; set; }
        public string whtTaxTypeDecs { get; set; }
        public string whtTaxCode { get; set; }
        public string whtTaxCodeDesc { get; set; }
        public string receiptType { get; set; }
        public string receiptTypeDesc { get; set; }

        public InquiryCRMPayeeListWithHoldingTaxInfoOutputModel()
        {
            whtCountryCode = "";
            whtTaxType = "";
            whtTaxTypeDecs = "";
            whtTaxCode = "";
            whtTaxCodeDesc = "";
            receiptType = "";
            receiptTypeDesc = "";
        }

    }


    public class InquiryCRMPayeeListOutputModel_Fail
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public InquiryCRMPayeeListDataOutputModel_Fail data { get; set; }
    }

    public class InquiryCRMPayeeListDataOutputModel_Fail
    {
        public List<InquiryCRMPayeeListFieldErrors> fieldErrors { get; set; }
    }

    public class InquiryCRMPayeeListFieldErrors
    {
        public string name { get; set; }
        public string message { get; set; }

        public InquiryCRMPayeeListFieldErrors()
        {
            name = "";
            message = "";
        }

        public InquiryCRMPayeeListFieldErrors(string n, string m)
        {
            name = n;
            message = m;
        }
    }

}