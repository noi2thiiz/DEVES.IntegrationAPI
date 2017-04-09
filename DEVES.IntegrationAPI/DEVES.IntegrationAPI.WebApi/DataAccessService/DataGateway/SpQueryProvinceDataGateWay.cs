using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class SpQueryProvinceDataGateWay:IDataGateWay
    {
        public DbResult Find(string Id)
        {
            try
            {
                var conectionString = CrmConfigurationSettings.AppConfig.Get("CRMDB");
                StoreDataReader reader = new StoreDataReader(conectionString);
                var req = new DbRequest()
                {
                    StoreName = "sp_Query_Province"
                    
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
        public DbResult FindByProvincecode(string Provincecode)
        {
            try
            {
                var conectionString = CrmConfigurationSettings.AppConfig.Get("CRMDB");
                StoreDataReader reader = new StoreDataReader(conectionString);
                var req = new DbRequest()
                {
                    StoreName = "sp_Query_Province"

                };
                req.AddParam("Provincecode", Provincecode);
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
                StoreName = "sp_Query_Province"

            };
         
            DbResult result = reader.Execute(req);
            Console.WriteLine(result.ToString());
            return result;
        } 
    }
}