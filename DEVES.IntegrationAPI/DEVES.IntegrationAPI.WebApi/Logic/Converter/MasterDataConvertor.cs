using System;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.Converter
{
    public class MasterDataConvertor
    {
        private static MasterDataConvertor _instance;
        private MasterDataConvertor() { }
        public static MasterDataConvertor Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new MasterDataConvertor();
             
                return _instance;
            }
        }

        public string ConvertSexOptionSetValueToMasterCode(string optionSetValue)
        {
            if (!string.IsNullOrEmpty(optionSetValue))
            {
                switch (optionSetValue)
                {
                    case "100000001":
                       return "M"; break;
                    case "100000002":
                        return "F"; break;
                    default:
                        return "U"; break;
                }

            }

            return "U";
        }

       
    }
}