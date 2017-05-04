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

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public List<OutputModelFailDataFieldErrors> ExtractSapCreateVendorFieldError<T>(string message, dynamic inputModel)
        {


            List<OutputModelFailDataFieldErrors> errorCorections = new List<OutputModelFailDataFieldErrors>();
            if (message == null)
            {
                return errorCorections;
            }
            //Ex: 'Please fill recipient type'
            if (message.Contains("recipient"))
            {
              
                errorCorections.Add(new OutputModelFailDataFieldErrors("withHoldingTaxInfo.receiptType", message));
            }else
            if (message.Contains("street"))
            {
               
                errorCorections.Add(new OutputModelFailDataFieldErrors("addressHeader.address1", message));
            }else if (message.Contains("postal"))
            {
              
                errorCorections.Add(new OutputModelFailDataFieldErrors("addressHeader.postalCode", message));
            }
            else if (message.Contains("tax number 3"))
            {

                errorCorections.Add(new OutputModelFailDataFieldErrors("addressHeader.idTax", message));
            }
            //Please fill bank info. (Bank country / Bank code / Bank branch / Bank acc.)
            else if (message.Contains("Please fill bank info."))
            {
                var bankInfo = inputModel?.sapVendorInfo?.bankInfo;

                if (bankInfo!=null)
                {
                    
                    if (string.IsNullOrEmpty(bankInfo?.bankCountryCode?.ToString()))
                    {
                        errorCorections.Add(new OutputModelFailDataFieldErrors("sapVendorInfo.bankInfo.bankCountryCode", "Please fill  bank country"));
                    }

                    if (string.IsNullOrEmpty(bankInfo?.bankCode?.ToString()))
                    {
                        errorCorections.Add(new OutputModelFailDataFieldErrors("sapVendorInfo.bankInfo.bankCode", "Please fill  bank code"));
                    }

                    if (string.IsNullOrEmpty(bankInfo?.bankBranchCode?.ToString()))
                    {
                        errorCorections.Add(new OutputModelFailDataFieldErrors("sapVendorInfo.bankInfo.bankBranchCode", "Please fill bank branch"));
                    }

                    if (string.IsNullOrEmpty(bankInfo?.bankAccount?.ToString()))
                    {
                        errorCorections.Add(new OutputModelFailDataFieldErrors("sapVendorInfo.bankInfo.bankAccount", "Please fill  bank acc"));
                    }

                }

         
            }
            //"For smart payment. Please fill info. (Tax number 4/Bank country/Bank code/Bank branch)"
            else if (message.Contains("For smart payment. Please fill info."))
            {
                var bankInfo = inputModel?.sapVendorInfo?.bankInfo;

                if (bankInfo != null)
                {

                    

                    //Tax number 4
                    if (string.IsNullOrEmpty(bankInfo?.bankCountryCode?.ToString()))
                    {
                        errorCorections.Add(new OutputModelFailDataFieldErrors("profileHeader.corporateBranch", "Please fill  corporate branch"));
                    }
                    //Bank code
                    if (string.IsNullOrEmpty(bankInfo?.bankCode?.ToString()))
                    {
                        errorCorections.Add(new OutputModelFailDataFieldErrors("sapVendorInfo.bankInfo.bankCode", "Please fill  bank code"));
                    }
                    //Bank branch
                    if (string.IsNullOrEmpty(bankInfo?.bankBranchCode?.ToString()))
                    {
                        errorCorections.Add(new OutputModelFailDataFieldErrors("sapVendorInfo.bankInfo.bankBranchCode", "Please fill bank branch"));
                    }
                    //Bank country
                    if (string.IsNullOrEmpty(bankInfo?.bankCountryCode?.ToString()))
                    {
                        errorCorections.Add(new OutputModelFailDataFieldErrors("sapVendorInfo.bankInfo.bankCountryCode", "Please fill  bank country"));
                    }



                }


            }




            else
            {
                errorCorections.Add(new OutputModelFailDataFieldErrors("unknown", message));
            }

            


            return errorCorections;

        }

       
    }
}