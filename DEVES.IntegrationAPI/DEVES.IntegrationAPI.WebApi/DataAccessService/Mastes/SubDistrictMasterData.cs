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

      


        public string GetNameWithPrefix(SubDistrictEntity entity, string prefixType = "")
        {
            
            

            if (string.IsNullOrEmpty(entity?.ProvinceCode))
            {
               return  "";
            }
            if (entity?.SubDistrictCode == "000000")
            {
                return "";
            }
            if (entity?.ProvinceCode == "10")
            {
                
                    return "แขวง" + entity?.SubDistrictName;
                
                
               
            }
            else
            {
                if (prefixType == "full")
                {
                    return "ตำบล" + entity?.SubDistrictName;
                }
                else
                {
                    return "ต." + entity?.SubDistrictName;
                }
               
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

 