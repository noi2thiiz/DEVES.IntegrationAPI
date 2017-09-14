using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData
{
    public class PersonalTitleMasterData : InMemoryDataStorageBase<PersonalTitleEntity, PersonalTitleEntityFields>
    {
        private static PersonalTitleMasterData _instance;

        private PersonalTitleMasterData()
        {
        }

        public static PersonalTitleMasterData Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new PersonalTitleMasterData();
                _instance.Load("sp_Query_PersonalTitle", "Code");
                return _instance;
            }
        }
    }

    public class PersonalTitleEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string PolisyCode { get; set; }
        public string RefSap { get; set; }
    }

    public enum PersonalTitleEntityFields
    {
        Id,Name,ContractDetail,PolisyCode, RefSap
    }
}