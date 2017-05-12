using System;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData
{
    public class ProvinceMasterData:InMemoryDataStorageBase<ProvinceEntity, ProvinceEntityFields>
    {

        private static ProvinceMasterData _instance;
        private ProvinceMasterData() { }

        public static ProvinceMasterData Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new ProvinceMasterData();
                _instance.Load("sp_Query_Province", "ProvinceCode");
                return _instance;
            }
        }

        

        public string GetNameWithPrefix(ProvinceEntity province)
        {
            

            if (string.IsNullOrEmpty(province?.ProvinceCode))
            {
                return "";
            }
            if (province?.ProvinceCode=="00")
            {
                return "";
            }

            if (province?.ProvinceCode == "10")
            {
                return province.ProvinceName;
            }
            else 
            {
                return "จ." + province?.ProvinceName;
            }
          
        }
    }
    public class ProvinceEntity
    {
        public Guid Id { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceNameEng { get; set; }
        public int SortIndex { get; set; }

    }

    public enum ProvinceEntityFields
    {
        Id, ProvinceCode, ProvinceName, ProvinceNameEng, SortIndex
    }

}