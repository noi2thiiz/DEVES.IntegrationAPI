using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.RegClientPersonal
{
    class RegClientPersonalOutputModel
    {
    }
    public class RegClientPersonalContentOutputModel : BaseContentJsonProxyOutputModel
    {
        [JsonProperty(Order = 10)]
        public List<RegClientPersonalDataOutputModel> data { get; set; }
    }

    public abstract class RegClientPersonalDataOutputModel : BaseDataModel
    {
    }

    public class RegClientPersonalOutputModel_Pass
    {
        [JsonProperty(Order = 1)]
        public string code { get; set; }
        [JsonProperty(Order = 2)]
        public string message { get; set; }
        [JsonProperty(Order = 3)]
        public string description { get; set; }
        [JsonProperty(Order = 4)]
        public string transactionId { get; set; }
        [JsonProperty(Order = 5)]
        public string transactionDateTime { get; set; }
        [JsonProperty(Order = 6)]
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
        [JsonProperty(Order = 1)]
        public string code { get; set; }
        [JsonProperty(Order = 2)]
        public string message { get; set; }
        [JsonProperty(Order = 3)]
        public string description { get; set; }
        [JsonProperty(Order = 4)]
        public string transactionId { get; set; }
        [JsonProperty(Order = 5)]
        public string transactionDateTime { get; set; }
        [JsonProperty(Order = 6)]
        public RegClientPersonalDataOutputModel_Fail data { get; set; }
    }

    public class RegClientPersonalDataOutputModel_Fail: RegClientPersonalDataOutputModel
    {
        public List<RegClientPersonalFieldErrors> fieldErrors { get; set; }
    }

    public class RegClientPersonalFieldErrors
    {
        public string name { get; set; }
        public string message { get; set; }

        public RegClientPersonalFieldErrors(string m, string n)
        {
            name = n;
            message = m;
        }
    }
}