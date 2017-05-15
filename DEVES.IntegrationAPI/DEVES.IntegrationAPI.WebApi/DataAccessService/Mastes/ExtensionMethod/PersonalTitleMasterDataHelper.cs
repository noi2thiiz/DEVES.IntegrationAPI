using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.Mastes.ExtensionMethod
{
    public static class PersonalTitleMasterDataHelper
    {
        public static string ToPersonalTitleName(this string code)
        {
            if (string.IsNullOrEmpty(code)) return "";
            var resutl = PersonalTitleMasterData.Instance.FindByCode(code);
            return resutl != null ? resutl.Name : "";
        }

        public static string ToPersonalTitleForSap(this string code)
        {
            if (string.IsNullOrEmpty(code)) return "";
            var resutl = PersonalTitleMasterData.Instance.FindByCode(code);
            return resutl != null ? resutl.RefSap : "";
        }

        public static string ToPersonalTitlePolisyCode(this string code)
        {
            if (string.IsNullOrEmpty(code)) return "";
            var resutl = PersonalTitleMasterData.Instance.FindByCode(code);
            return resutl != null ? resutl.PolisyCode : "0001";
        }
        

    }
}