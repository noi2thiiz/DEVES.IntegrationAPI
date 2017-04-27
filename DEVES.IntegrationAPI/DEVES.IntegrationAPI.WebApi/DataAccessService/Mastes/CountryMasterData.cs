using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData
{
    public class CountryMasterData : InMemoryDataStorageBase<CountryEntity>
    {
        private static CountryMasterData _instance;
        private CountryMasterData() { }
        public static CountryMasterData Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new CountryMasterData();
                _instance.Load("sp_Query_Countries", "CountryCode");
                return _instance;
            }
        }

    }

    public class CountryEntity
    {
        public Guid Id { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string PolisyCode { get; set; }
        public string SapCode { get; set; }

    }
}