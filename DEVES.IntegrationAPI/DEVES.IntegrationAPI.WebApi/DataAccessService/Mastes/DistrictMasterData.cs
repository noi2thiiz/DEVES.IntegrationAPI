using System;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData
{
    public class DistricMasterData: InMemoryDataStorageBase<DistrictEntity>
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
 
    }

    public class DistrictEntity
    {
        public Guid Id { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameEng { get; set; }
      
    }
}