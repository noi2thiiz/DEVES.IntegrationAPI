﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegClientPersonal
{
    class RegClientPersonalOutputModel
    {
    }
    public class RegClientPersonalContentOutputModel : BaseContentJsonProxyOutputModel
    {
        public List<RegClientPersonalDataOutputModel> data { get; set; }
    }

    public abstract class RegClientPersonalDataOutputModel : BaseDataModel
    {
    }

    public class RegClientPersonalOutputModel_Pass
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public List<RegClientPersonalDataOutputModel_Pass> data { get; set; }
    }

    public class RegClientPersonalDataOutputModel_Pass : RegClientPersonalDataOutputModel
    {
        public string cleansingId { get; set; }
        public string polisyClientId { get; set; }
        public string crmClientId { get; set; }
        public string personalName { get; set; }
        public string personalSurname { get; set; }

        public RegClientPersonalDataOutputModel_Pass()
        {
            cleansingId = "";
            polisyClientId = "";
            crmClientId = "";
            personalName = "";
            personalSurname = "";
        }
    }

    public class RegClientPersonalOutputModel_Fail
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public List<RegClientPersonalDataOutputModel_Fail> data { get; set; }
    }

    public class RegClientPersonalDataOutputModel_Fail: RegClientPersonalDataOutputModel
    {
        public string fieldErrors { get; set; }
        public string message { get; set; }
        public string name { get; set; }

    }
}