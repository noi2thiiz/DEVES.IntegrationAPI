﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model
{
  
    public class OutputModelFail : BaseContentJsonProxyOutputModel
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
        public DateTime transactionDateTime { get; set; } = DateTime.Now;
        [JsonProperty(Order = 6)]
        public OutputModelFailData data { get; set; } = new OutputModelFailData();



        public void AddFieldError(string fieldName, string fieldMessage)
        {
            data.fieldErrors.Add(new OutputModelFailDataFieldErrors(fieldName, fieldMessage));
        }
    }

    public class OutputModelFailData : BaseDataModel
    {
        public List<OutputModelFailDataFieldErrors> fieldErrors { get; set; } = new List<OutputModelFailDataFieldErrors>();
    }

    public class OutputModelFailDataFieldErrors:BaseDataModel
    {
    
        public string name { get; set; }
        public string message { get; set; }

        public OutputModelFailDataFieldErrors(string n, string m)
        {
           
            name = n;
            message = m;
        }
    }
}
