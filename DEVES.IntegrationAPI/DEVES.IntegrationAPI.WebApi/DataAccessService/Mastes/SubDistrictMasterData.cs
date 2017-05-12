using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData
{


    public class SubDistrictMasterData : InMemoryDataStorageBase<SubDistrictEntity, SubDistrictEntityFields>
    {
        private static SubDistrictMasterData _instance;

        private SubDistrictMasterData()
        {
        }

        public static SubDistrictMasterData Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new SubDistrictMasterData();
                _instance.Load("sp_Query_SubDistrict", "SubDistrictCode");
                return _instance;
            }
        }

        public string GetPrefix(SubDistrictEntity entity)
        {
            var code = entity.ProvinceCode;
            return code == "10" ? "แขวง" : "ตำบล";
        }


        public string GetNameWithPrefix(SubDistrictEntity entity)
        {
            if (string.IsNullOrEmpty(entity?.ProvinceCode))
            {
               return entity?.SubDistrictName ?? "";
            }
            if (entity?.ProvinceCode == "10")
            {
                return "แขวง"+ entity?.SubDistrictName;
            }
            else
            {
                return "ต." + entity?.SubDistrictName;
            }
          
            
        }
    }

    public class SubDistrictEntity
    {
        public Guid Id { get; set; }
        public string SubDistrictName { get; set; }
        public string SubDistrictCode { get; set; }
        public string DistrictCode { get; set; }

        public string ProvinceCode { get; set; }

        public string PostalCode { get; set; }


    }
    public enum SubDistrictEntityFields
    {

        Id, SubDistrictName, SubDistrictCode, DistrictCode, ProvinceCode, PostalCode
    }
}

 