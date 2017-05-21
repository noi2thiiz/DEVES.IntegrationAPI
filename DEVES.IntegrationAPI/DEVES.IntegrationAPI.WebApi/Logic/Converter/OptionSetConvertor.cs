using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.WebApi.Logic.Converter
{
    public static class OptionSetConvertor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="riskLevel"></param>
        /// <returns></returns>
        public static OptionSetValue GetRiskLevelOptionSetValue(string riskLevel)
        {

            switch (riskLevel)
            {
                case "A": return new OptionSetValue(100000001);
                case "B": return new OptionSetValue(100000002); 
                case "C1": return new OptionSetValue(100000003); 
                case "C2": return new OptionSetValue(100000004);
                case "R1": return new OptionSetValue(100000005); 
                case "R2": return new OptionSetValue(100000006); 
                case "R3": return new OptionSetValue(100000007); 
                case "R4": return new OptionSetValue(100000008); 
                case "RL1": return new OptionSetValue(100000009);
                case "RL2": return new OptionSetValue(100000010); 
                case "RL3": return new OptionSetValue(100000011); 
                case "U": return new OptionSetValue(100000012); 
                case "X": return new OptionSetValue(100000013); 
                default: return new OptionSetValue();
            }
        }


        public static OptionSetValue GetLanguageOptionSetValue(string language)
        {
            switch (language)
            {
                case "J": return new OptionSetValue(100000002); 
                case "T": return new OptionSetValue(100000003); 
                case "E": return new OptionSetValue(100000001); 
                default: return new OptionSetValue(100000004);
            }
        }

        public static OptionSetValue GetIntegrationSourceDataOptionSetValue()
        {
           return  new OptionSetValue(100000003);
        }

        public static OptionSetValue GetOptionsetValue(string val)
        {
            string opVal = "";

            if (val.Length == 1)
            {
                opVal = "10000000" + val;
            }
            else if (val.Length == 2)
            {
                opVal = "1000000" + val;
            }
            else
            {
                opVal = "100000" + val;
            }


            try
            {
                return new OptionSetValue(Int32.Parse(opVal));
            }
            catch (Exception)
            {
                return new OptionSetValue();
            }
           
           
        }
    }
}