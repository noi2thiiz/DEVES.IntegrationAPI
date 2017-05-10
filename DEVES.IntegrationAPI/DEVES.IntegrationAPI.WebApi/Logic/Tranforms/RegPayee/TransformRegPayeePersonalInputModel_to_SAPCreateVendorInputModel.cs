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
                trgt.VCODE = src.generalHeader.polisyClientId ?? "";
            }
            if (src.profileInfo != null)
            {
                trgt.TITLE = buzMasterSalutation.Instant.SalutationList.FirstOrDefault( t => t.titlePolisy == src.profileInfo.salutation).titleSAP ?? "";
                trgt.NAME1 = src.profileInfo.personalName ?? "";
                trgt.NAME2 = src.profileInfo.personalSurname ?? "";
                trgt.TAX3 = src.profileInfo.idCitizen ?? "";
                trgt.SEARCH = src.profileInfo.personalName ?? "";
            }
            if (src.contactInfo != null)
            {
                if (string.IsNullOrEmpty(src.contactInfo.telephone1Ext))
                {
                    trgt.TEL1 = src.contactInfo.telephone1 ?? "";
                }
                else
                {
                    trgt.TEL1 = src.contactInfo.telephone1 + src.contactInfo.telephone1Ext ?? "";
                }
                if (string.IsNullOrEmpty(src.contactInfo.telephone2Ext))
                {
                    trgt.TEL2 = src.contactInfo.telephone2 ?? "";
                }
                else
                {
                    trgt.TEL2 = src.contactInfo.telephone2 + src.contactInfo.telephone2Ext ?? "";
                }
                trgt.FAX = src.contactInfo.fax ?? "";
            }
            if (src.addressInfo != null)
            {
                trgt.STREET1 = src.addressInfo.address1 ?? "";
                trgt.STREET2 = src.addressInfo.address3 ?? "";
                trgt.DISTRICT = src.addressInfo.subDistrictCode ?? "";
                trgt.CITY = src.addressInfo.provinceCode ?? "";
                trgt.POSTCODE = src.addressInfo.postalCode ?? "";
                trgt.COUNTRY = buzMasterCountry.Instant.CountryList.FirstOrDefault( c => c.ctryPolisy== src.addressInfo.country).ctrySAP??"";
            }
            if (src.sapVendorInfo != null)
            {
                trgt.VGROUP = src.sapVendorInfo.sapVendorGroupCode;
            }
            
            if (src.sapVendorInfo != null && src.sapVendorInfo.bankInfo != null)
            {
                trgt.CTRY = src.sapVendorInfo.bankInfo.bankCountryCode ?? "";
                trgt.BANKCODE = src.sapVendorInfo.bankInfo.bankCode ?? "";
                trgt.BANKBRANCH = src.sapVendorInfo.bankInfo.bankBranchCode ?? "";
                trgt.ACCTHOLDER = src.sapVendorInfo.bankInfo.accountHolder ?? "";
                trgt.PAYMETHOD = src.sapVendorInfo.bankInfo.paymentMethods ?? "";
                trgt.BANKACC = src.sapVendorInfo.bankInfo.bankAccount ?? "";
               
            }
            if (src.sapVendorInfo != null && src.sapVendorInfo.withHoldingTaxInfo != null)
            {
                trgt.WHTCODE = src.sapVendorInfo.withHoldingTaxInfo.whtTaxCode ?? "";
                trgt.RECPTYPE = src.sapVendorInfo.withHoldingTaxInfo.receiptType ?? "";
              
            }

            trgt.COMPANY = "2020";
            trgt.TAX1 = "";
            trgt.TAX2 = "";
            trgt.TAX4 = "";
            trgt.WHTCTRY = "TH";

            if (string.IsNullOrEmpty(trgt.RECPTYPE))
            {
                trgt.RECPTYPE = AppConst.DEFAULT_PERSONAL_RECPTYPE; // default 03 for personal
            }
            //'Fix "TH" ก็ต่อเมื่อข้อมูลต่อไปนี้มีค่า  CTRY BANKCODE BANKBRANCH BANKACC
            if (!(string.IsNullOrEmpty(trgt.BANKCODE) &&
                 string.IsNullOrEmpty(trgt.BANKBRANCH) &&
                 string.IsNullOrEmpty(trgt.BANKACC)))
            {
                trgt.CTRY = "TH";
            }
         
            
                
            
                

            return trgt;
        }
    }
}