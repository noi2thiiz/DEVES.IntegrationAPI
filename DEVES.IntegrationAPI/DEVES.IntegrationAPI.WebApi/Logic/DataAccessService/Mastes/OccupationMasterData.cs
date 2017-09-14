using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData
{
    public class OccupationMasterData : InMemoryDataStorageBase<OccupationEntity, OccupationEntityFields>
    {
        private static OccupationMasterData _instance;
        private OccupationMasterData() { }
        public static OccupationMasterData Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new OccupationMasterData();
            _instance.Load("sp_Query_Occupation", "Code");
                return _instance;
            }
        }

    }

    public class OccupationEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string PolisyCode { get; set; }

    }

    public enum OccupationEntityFields
    {
        Id, Name, Code, Polisycode
    }
}