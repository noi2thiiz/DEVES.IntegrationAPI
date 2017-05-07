﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;
using DEVES.IntegrationAPI.Model.SAP;

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
                trgt.VCODE = src.generalHeader.polisyClientId ?? "";
            }
            if (src.profileHeader != null)
            {
                trgt.NAME1 = src.profileHeader.corporateName1??"";
                trgt.NAME2 = src.profileHeader.corporateName2 ?? "";
                trgt.TAX3 = src.profileHeader.idTax;
                trgt.TAX4 = src.profileHeader.corporateBranch??"";
            }

            if (src.addressHeader != null)
            {
                trgt.STREET1 = src.addressHeader.address1 ?? "";
                trgt.STREET2 = src.addressHeader.address3 ?? "";
                trgt.DISTRICT = src.addressHeader.subDistrictCode ?? "";
                trgt.CITY = src.addressHeader.provinceCode ?? "";
                trgt.POSTCODE = src.addressHeader.postalCode ?? "";
                trgt.COUNTRY = buzMasterCountry.Instant.CountryList.FirstOrDefault(x => x.ctryPolisy == src.addressHeader.country)?.ctrySAP ?? "";
            }
            if (src.sapVendorInfo != null)
            {
                trgt.VGROUP = src.sapVendorInfo.sapVendorGroupCode ?? "";
            }
            if (src.sapVendorInfo != null && src.sapVendorInfo.bankInfo != null)
            {
                trgt.CTRY = src.sapVendorInfo.bankInfo.bankCountryCode??"";
                trgt.BANKCODE = src.sapVendorInfo.bankInfo.bankCode??"";
                trgt.BANKBRANCH = src.sapVendorInfo.bankInfo.bankBranchCode ?? "";
                trgt.BANKACC = src.sapVendorInfo.bankInfo.bankAccount ?? "";
                trgt.ACCTHOLDER = src.sapVendorInfo.bankInfo.accountHolder ?? "";
                trgt.PAYMETHOD = src.sapVendorInfo.bankInfo.paymentMethods ?? "";

               
            }
            if (src.sapVendorInfo != null && src.sapVendorInfo.withHoldingTaxInfo != null)
            {
                trgt.WHTCODE = src.sapVendorInfo.withHoldingTaxInfo.whtTaxCode ?? "";
                trgt.RECPTYPE = src.sapVendorInfo.withHoldingTaxInfo.receiptType ?? "";
               
              
              }
            if (src.contactHeader != null )
            {
                trgt.TEL1 = src.contactHeader.telephone1 ?? "";
                trgt.TEL2 = src.contactHeader.telephone2 ?? "";
                trgt.FAX = src.contactHeader.fax ?? "";
            }

            trgt.COMPANY = AppConst.DEFAULT_CORPORATE_SAP_VENDOR_COMPANY; //fix TH
            trgt.SEARCH = src.profileHeader.corporateName1 ?? "";
            trgt.TAX1 = "";
            trgt.TAX2 = "";
            //trgt.CTRY = "TH";
           
            trgt.WHTCTRY = "TH";
            
            trgt.TITLE = "";

            if (string.IsNullOrEmpty(trgt.RECPTYPE))
            {
                trgt.RECPTYPE = AppConst.DEFAULT_CORPORATE_RECPTYPE; // default for corporate
            }

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