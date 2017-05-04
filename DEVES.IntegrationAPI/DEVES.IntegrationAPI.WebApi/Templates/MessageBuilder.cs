using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DEVES.IntegrationAPI.Model;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public class MessageBuilder
    {
        private static MessageBuilder _instance;

        private MessageBuilder()
        {
        }

        public static MessageBuilder Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new MessageBuilder();

                return _instance;
            }
        }

        public string GetInvalidMasterMessage()
        {
            return "The value is not defined in master data";
        }

        public string GetInvalidMasterMessage(string masterName, string value)
        {
            return $"The {masterName.ToLower()} code '{value}' is not defined in master data";
        }

        private bool complete { get; set; } = false;

        public List<OutputModelFailDataFieldErrors> ExtractSapCreateVendorFieldError<T>(string message)
        {


            List<OutputModelFailDataFieldErrors> errorCorections = new List<OutputModelFailDataFieldErrors>();
            if (message == null)
            {
                return errorCorections;
            }
            //Ex: 'Please fill recipient type'
            if (message.Contains("recipient"))
            {
                complete = true;
                errorCorections.Add(new OutputModelFailDataFieldErrors("withHoldingTaxInfo.receiptType", message));
            }
            if(!complete)
            {
                errorCorections.Add(new OutputModelFailDataFieldErrors("", message));
            }

            return errorCorections;

        }

       
    }
}