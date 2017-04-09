using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class SpGetUserForRVPDataGateway
    {
        public DbResult Excecute()
        {
            try
            {
                var conectionString = CrmConfigurationSettings.AppConfig.Get("CRMDB");
                StoreDataReader reader = new StoreDataReader(conectionString);
                var req = new DbRequest();
                req.StoreName = "sp_GetUserForRVP";

                DbResult result = reader.Execute(req);
                Console.WriteLine(result.ToString());
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}