using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;
using DEVES.IntegrationAPI.Model.SAP;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformRegPayeePersonalInputModel_to_SAPCreateVendorInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegPayeePersonalInputModel src = (RegPayeePersonalInputModel)input;
            SAPCreateVendorInputModel trgt = (SAPCreateVendorInputModel)output;

            if (src == null)
            {
                return trgt;
            }

            if (src.generalHeader != null)
            {
                trgt.VCODE = src.generalHeader.polisyClientId;
            }
            if (src.profileInfo != null)
            {
                trgt.TITLE = src.profileInfo.salutation;
                trgt.NAME1 = src.profileInfo.personalName;
                trgt.NAME2 = src.profileInfo.personalSurname;
                trgt.TAX3 = src.profileInfo.idCitizen;
            }
            if (src.contactInfo != null)
            {
                trgt.TEL1 = src.contactInfo.telephone1;
                trgt.TEL2 = src.contactInfo.telephone2;
                trgt.FAX = src.contactInfo.fax;
            }
            if (src.addressInfo != null)
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
            
            if (src.sapVendorInfo != null && src.sapVendorInfo.bankInfo != null)
            {
                trgt.CITY = src.sapVendorInfo.bankInfo.bankCountryCode;
                trgt.BANKCODE = src.sapVendorInfo.bankInfo.bankCode;
                trgt.BANKBRANCH = src.sapVendorInfo.bankInfo.bankBranchCode;
                trgt.ACCTHOLDER = src.sapVendorInfo.bankInfo.accountHolder;
                trgt.PAYMETHOD = src.sapVendorInfo.bankInfo.paymentMethods;
            }
            if (src.sapVendorInfo != null && src.sapVendorInfo.withHoldingTaxInfo != null)
            {
                trgt.WHTCODE = src.sapVendorInfo.withHoldingTaxInfo.whtTaxCode;
                trgt.RECPTYPE = src.sapVendorInfo.withHoldingTaxInfo.receiptType;
            }
      
            return trgt;
        }
    }
}