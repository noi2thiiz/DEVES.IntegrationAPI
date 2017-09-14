using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.Mastes.ExtensionMethod
{
    /// <summary>
    /// String helper methods
    /// </summary>
    public static class ProvinceMasterDataHelper
    {
        public static string ToProvinceName(this string provinceCode)
        {
            if (string.IsNullOrEmpty(provinceCode)) return provinceCode;
            var province = ProvinceMasterData.Instance.FindByCode(provinceCode);
            if (province != null)
            {
                return province.ProvinceName;
            }
            return "";
        }

        public static string ToProvinceNameWithShortPrefix(this string provinceCode)
        {
            if (string.IsNullOrEmpty(provinceCode)) return provinceCode;
            var province = ProvinceMasterData.Instance.FindByCode(provinceCode);
            if (province != null)
            {
                return ProvinceMasterData.Instance.GetNameWithPrefix(province, "short");
            }
            return "";
        }
        public static string ToProvinceNameWithLongPrefix(this string provinceCode)
        {
            if (string.IsNullOrEmpty(provinceCode)) return provinceCode;
            var province = ProvinceMasterData.Instance.FindByCode(provinceCode);
            if (province != null)
            {
                return ProvinceMasterData.Instance.GetNameWithPrefix(province, "full");
            }
            return "";
        }



    }
}