using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;

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
                var fullname = (src.profileHeader.corporateName1 + " " + src.profileHeader.corporateName2).Trim();

                if (fullname.Length > 40)
                {
                    trgt.NAME1 = fullname.Substring(0, 40);
                    trgt.NAME2 = fullname.Substring(40, fullname.Length - 40);
                }
                else
                {
                    trgt.NAME1 = fullname;
                }
               
                trgt.TAX3 = src.profileHeader.idTax;
                trgt.TAX4 = src.profileHeader.corporateBranch??"";
            }

            if (src.addressHeader != null)
            {
                string districtName = "";
                string subDistrictName = "";
                if (!string.IsNullOrEmpty(src.addressHeader.districtCode))
                {
                    var district = DistricMasterData.Instance.FindByCode(src.addressHeader.districtCode);
                    if (district != null)
                    {
                        districtName = DistricMasterData.Instance.GetNameWithPrefix(district, "full");
                    }

                }

                trgt.DISTRICT = ("" + subDistrictName + " " + districtName).Trim();

                if (!string.IsNullOrEmpty(src.addressHeader?.subDistrictCode))
                {
                    var subDistrict = SubDistrictMasterData.Instance.FindByCode(src.addressHeader.subDistrictCode);
                    if (subDistrict != null)
                    {
                        subDistrictName = SubDistrictMasterData.Instance.GetNameWithPrefix(subDistrict, "full");
                    }
                }



                trgt.DISTRICT = (subDistrictName + " " + districtName).Trim();
                //provinceCode    String	2	O จังหวัด
                if (!string.IsNullOrEmpty(src.addressHeader?.provinceCode))
                {
                    var province = ProvinceMasterData.Instance.FindByCode(src.addressHeader.provinceCode);
                    if (province != null)
                    {
                        trgt.CITY = ProvinceMasterData.Instance.GetNameWithPrefix(province,"full");
                    }
                }

                trgt.STREET1 = (src.addressHeader.address1 + " " + src.addressHeader.address2).Trim();
                trgt.STREET2 = src.addressHeader.address3 ?? "";

                // trgt.CITY = src.addressInfo.provinceCode ?? "";
                trgt.POSTCODE = src.addressHeader.postalCode ?? "";
                trgt.COUNTRY = buzMasterCountry.Instant.CountryList.FirstOrDefault(c => c.ctryPolisy == src.addressHeader.country).ctrySAP ?? "";

               
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
                if (string.IsNullOrEmpty(src.contactHeader.telephone1Ext))
                {
                    trgt.TEL1 = src.contactHeader.telephone1 ?? "";
                }
                else
                {
                    trgt.TEL1 = src.contactHeader.telephone1 + src.contactHeader.telephone1Ext ?? "";
                }

                if (string.IsNullOrEmpty(src.contactHeader.telephone2Ext))
                {
                    trgt.TEL2 = src.contactHeader.telephone2 ?? "";
                }
                else
                {
                    trgt.TEL2 = src.contactHeader.telephone2 + src.contactHeader.telephone2Ext ?? "";
                }

                if (!string.IsNullOrEmpty(src.contactHeader.mobilePhone))
                {
                    trgt.TEL2 = src.contactHeader.mobilePhone;
                }
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