using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData
{


    public class SubDistrictMasterData : InMemoryDataStorageBase<SubDistrictEntity>
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
}

 