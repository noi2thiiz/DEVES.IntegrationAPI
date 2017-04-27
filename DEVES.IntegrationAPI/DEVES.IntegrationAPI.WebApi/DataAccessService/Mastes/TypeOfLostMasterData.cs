using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData
{
    public class TypeOfLossMasterData : InMemoryDataStorageBase<TypeOfLostEntity>
    {
        private static TypeOfLossMasterData _instance;

        private TypeOfLossMasterData()
        {

        }

        public static TypeOfLossMasterData Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new TypeOfLossMasterData();
                _instance.Load("sp_Query_TypeOfLost", "Code");
                return _instance;
            }
        }
    }

    public class TypeOfLostEntity
    {
        public Guid Id { get; set; }
        public string SubDistrictName { get; set; }
        public string SubDistrictCode { get; set; }
        public string DistrictCode { get; set; }

        public string ProvinceCode { get; set; }

        public string PostalCode { get; set; }


    }
}