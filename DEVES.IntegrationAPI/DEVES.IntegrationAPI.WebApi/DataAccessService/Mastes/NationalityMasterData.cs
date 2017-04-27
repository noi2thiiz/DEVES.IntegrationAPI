using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData
{
   
    public class NationalityMasterData : InMemoryDataStorageBase<NationalityEntity, NationalityEntityFields>
    {
        private static NationalityMasterData _instance;

        private NationalityMasterData()
        {
        }

        public static NationalityMasterData Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new NationalityMasterData();
                
                _instance.Load("sp_Query_Nationalities", "Code");
                return _instance;
            }
        }
    }

    public class NationalityEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Polisycode { get; set; }
    }

    public enum NationalityEntityFields
    {
        Id, Name, Code, Polisycode
    }
}