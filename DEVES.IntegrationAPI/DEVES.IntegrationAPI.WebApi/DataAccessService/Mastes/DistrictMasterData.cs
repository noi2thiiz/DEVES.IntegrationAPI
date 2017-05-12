using System;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData
{
    public class DistricMasterData: InMemoryDataStorageBase<DistrictEntity, DistrictEntityFields>
    {
        private static DistricMasterData _instance;
        private DistricMasterData() { }
        public static DistricMasterData Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new DistricMasterData();
                _instance.Load("sp_Query_District", "DistrictCode");
                return _instance;
            }
        }

        internal string GetNameWithPrefix(DistrictEntity entity)
        {
            var provinceCode = entity?.DistrictCode.Substring(0, 2);
            if (string.IsNullOrEmpty(entity?.DistrictCode))
            {
                return entity?.DistrictName ?? "";
            }
            if (provinceCode == "10")
            {
                return "เขต" + entity?.DistrictName;
            }
            else
            {
                return "อ." + entity?.DistrictName;
            }
        }
    }

    public class DistrictEntity
    {
        public Guid Id { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameEng { get; set; }
      
    }
    public enum DistrictEntityFields{
         Id,DistrictCode, DistrictName, DistrictNameEng
    }
}