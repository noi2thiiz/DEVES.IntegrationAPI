using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData
{
   

    public class AddressTypeMasterData : InMemoryDataStorageBase<AddressTypeEntity, AddressTypeEntityFields>
    {
        private static AddressTypeMasterData _instance;

        private AddressTypeMasterData()
        {
        }

        public static AddressTypeMasterData Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new AddressTypeMasterData();
                _instance.Load("sp_Query_AddressType", "Code");
                return _instance;
            }
        }

    }

    public class AddressTypeEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }

        public string PolisyCode { get; set; }

       
        }
    public enum AddressTypeEntityFields
    {

        Id, Code, PolisyCode, Name, NameEng
    }
}
