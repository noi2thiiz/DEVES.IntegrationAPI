using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformRegPayeeCorporateInputModel_to_SAPCreateVendorInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegPayeeCorporateInputModel src = (RegPayeeCorporateInputModel)input;
            SAPCreateVendorInputModel trgt = (SAPCreateVendorInputModel)output;

            if (src == null)
            {
                return trgt;
            }

            if (src.generalHeader != null)
            {
                trgt.VCODE = src.generalHeader.policyClientId;
            }
            if (src.profileHeader != null)
            {
                trgt.TITLE = src.profileInfo.salutation;
                trgt.NAME1 = src.profileInfo.personalName;
                trgt.NAME2 = src.profileInfo.personalSurname;
                trgt.TAX3 = src.profileInfo.idCitizen;
                trgt.TAX4 = src.progileInfo.corporateBranch;
            }

            if (src.addressHeader != null)
            {
                trgt.STREET1 = src.addressInfo.address1;
                trgt.STREET2 = src.addressInfo.address3;
                trgt.DISTRICT = src.addressInfo.subDistrictCode;
                trgt.CITY = src.addressInfo.provinceCode;
                trgt.POSTCODE = src.addressInfo.postalCode;
                trgt.COUNTRY = src.addressInfo.country;
            }
            if (src.sapVendorInfo != null)
            {
                trgt.VGROUP = src.sapVendorInfo.sapVendorGroupCode;
            }
            if (src.bankInfo != null)
            {
                trgt.CITY = src.bankInfo.BankCountryCode;
                trgt.BANKCODE = src.bankInfo.bankCode;
                trgt.BANKBRANCH = src.bankInfo.bankBranchCode;
                trgt.BANKACC = src.bankInfo.bankAccount;
                trgt.ACCTHOLDER = src.bankInfo.accountHolder;
                trgt.PAYMETHOD = src.bankInfo.paymentMethods;
            }
            if (src.withHoldingTaxInfo != null)
            {
                trgt.WHTCODE = src.withHoldingTaxInfo.whtTaxCode;
                trgt.RECPTYPE = src.withHoldingTaxInfo.receiptType;
            }

            return trgt;
        }
    }
}