﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegPayeePersonal
{
    public class RegPayeePersonalInputModel
    {
        public GeneralHeaderModel generalHeader { get; set; }
        public ProfileInfoModel profileInfo { get; set; }
        public ContactInfoModel contactInfo { get; set; }
        public AddressInfoModel addressInfo { get; set; }
        public SapVendorInfoModel sapVendorInfo { get; set; }
    }

    public class GeneralHeaderModel
    {
        public string roleCode { get; set; }
        public string cleansingId { get; set; }
        public string polisyClientId { get; set; }
        public string crmClientId { get; set; }
    }

    public class ProfileInfoModel
    {
        public string salutation { get; set; }
        public string personalName { get; set; }
        public string personalSurname { get; set; }
        public string sex { get; set; }
        public string idCitizen { get; set; }
        public string idPassport { get; set; }
        public string idAlien { get; set; }
        public string idDriving { get; set; }
        public string birthDate { get; set; }
        public string nationality { get; set; }
        public string language { get; set; }
        public string married { get; set; }
        public string occupation { get; set; }
        public string riskLevel { get; set; }
        public string vipStatus { get; set; }
        public string remark { get; set; }
    }

    public class ContactInfoModel
    {
        public string telephone1 { get; set; }
        public string telephone1Ext { get; set; }
        public string telephone2 { get; set; }
        public string telephone2Ext { get; set; }
        public string telephone3 { get; set; }
        public string telephone3Ext { get; set; }
        public string mobilePhone { get; set; }
        public string fax { get; set; }
        public string emailAddress { get; set; }
        public string lineID { get; set; }
        public string facebook { get; set; }
    }

    public class AddressInfoModel
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string subDistrictCode { get; set; }
        public string districtCode { get; set; }
        public string provinceCode { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
        public string addressType { get; set; }
        public string latitude { get; set; }
        public string longtitude { get; set; }
    }

    public class SapVendorInfoModel
    {
        public string sapVendorGroupCode { get; set; }
        public BankInfoModel bankInfo { get; set; }
        public WithHoldingTaxInfoModel withHoldingTaxInfo { get; set; }
    }

    public class BankInfoModel
    {
        public string bankCountryCode { get; set; }
        public string bankCode { get; set; }
        public string bankBranchCode { get; set; }
        public string bankAccount { get; set; }
        public string accountHolder { get; set; }
        public string paymentMethods { get; set; }
    }

    public class WithHoldingTaxInfoModel
    {
        public string whtTaxCode { get; set; }
        public string receiptType { get; set; }
    }
}