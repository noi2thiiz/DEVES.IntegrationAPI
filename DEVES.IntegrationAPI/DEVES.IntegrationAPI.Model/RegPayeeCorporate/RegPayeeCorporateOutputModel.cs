﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegPayeeCorporate
{
    class RegPayeeCorporateOutputModel
    {
    }

    public class RegPayeeCorporateOutputModel_Pass
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public List<RegPayeeCorporateDataOutputModel_Pass> data { get; set; }
    }

    public class RegPayeeCorporateDataOutputModel_Pass
    {
        public string polisyClientId { get; set; }
        public string sapVendorCode { get; set; }
        public string sapVendorGroupCode { get; set; }
        public string corporateName1 { get; set; }
        public string corporateName2 { get; set; }
        public string corporateBranch { get; set; }
    }

    public class RegPayeeCorporateOutputModel_Fail
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public List<RegPayeeCorporateDataOutputModel_Fail> data { get; set; }
    }

    public class RegPayeeCorporateDataOutputModel_Fail
    {
        public string fieldErrors { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }
}