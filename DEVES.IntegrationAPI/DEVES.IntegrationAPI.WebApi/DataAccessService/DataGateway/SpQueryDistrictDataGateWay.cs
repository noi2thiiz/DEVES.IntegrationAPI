using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class SpQueryDistrictDataGateWay:IDataGateWay
    {
        public DbResult Find(string Id)
        {
            try
            {
                var conectionString = CrmConfigurationSettings.AppConfig.Get("CRMDB");
                StoreDataReader reader = new StoreDataReader(conectionString);
                var req = new DbRequest()
                {
                    StoreName = "sp_Query_District"

                };
                req.AddParam("Provincecode", Id);
                DbResult result = reader.Execute(req);
                Console.WriteLine(result.ToString());
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DbResult FindByDistrictCode(string DistrictCode)
        {
            try
            {
                var conectionString = CrmConfigurationSettings.AppConfig.Get("CRMDB");
                StoreDataReader reader = new StoreDataReader(conectionString);
                var req = new DbRequest()
                {
                    StoreName = "sp_Query_District"

                };
                req.AddParam("DistrictCode", DistrictCode);
                DbResult result = reader.Execute(req);
                Console.WriteLine(result.ToString());
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DbResult FetchAll()
        {
            var conectionString = CrmConfigurationSettings.AppConfig.Get("CRMDB");
            StoreDataReader reader = new StoreDataReader(conectionString);
            var req = new DbRequest()
            {
                StoreName = "sp_Query_District"

            };

            DbResult result = reader.Execute(req);
            Console.WriteLine(result.ToString());
            return result;
        }
    }
}