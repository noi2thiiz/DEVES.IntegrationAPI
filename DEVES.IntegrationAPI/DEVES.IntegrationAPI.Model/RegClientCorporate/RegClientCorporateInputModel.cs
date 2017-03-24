﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegClientCorporate
{
    public class RegClientCorporateInputModel
    {
        public GeneralHeaderModel generalHeader { get; set; }
        public ProfileHeaderModel profileHeader { get; set; }
        public ContactHeaderModel contactHeader { get; set; }
        public AddressHeaderModel addressHeader { get; set; }
        public AsrhHeaderModel asrhHeader { get; set; }
    }

    public class GeneralHeaderModel
    {
        public string roleCode { get; set; }
        public string cleansingId { get; set; }
        public string polisyClientId { get; set; }
        public string crmClientId { get; set; }
        public string clientAdditionalExistFlag { get; set; }
    }


    public class ProfileHeaderModel
    {
        public string corporateName1 { get; set; }
        public string corporateName2 { get; set; }
        public string contactPerson { get; set; }
        public string idRegCorp { get; set; }
        public string idTax { get; set; }
        public string dateInCorporate { get; set; }
        public string corporateBranch { get; set; }
        public string econActivity { get; set; }
        public string countryOrigin { get; set; }
        public string language { get; set; }
        public string riskLevel { get; set; }
        public string vipStatus { get; set; }
    }

    public class ContactHeaderModel
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

    public class AddressHeaderModel
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

    public class AsrhHeaderModel
    {
        public string assessorOregNum { get; set; }
        public string assessorDelistFlag { get; set; }
        public string assessorBlackListFlag { get; set; }
        public string assessorTerminateDate { get; set; }
        public string solicitorOregNum { get; set; }
        public string solicitorDelistFlag { get; set; }
        public string solicitorBlackListFlag { get; set; }
        public string solicitorTerminateDate { get; set; }
        public string repairerOregNum { get; set; }
        public string repairerDelistFlag { get; set; }
        public string repairerBlackListFlag { get; set; }
        public string repairerTerminateDate { get; set; }
    }

}
