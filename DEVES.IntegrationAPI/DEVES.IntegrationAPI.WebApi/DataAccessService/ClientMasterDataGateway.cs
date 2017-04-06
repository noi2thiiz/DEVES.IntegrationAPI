using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using System.Collections.Generic;
namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class ClientMasterDataGateway:IDataGateWay
    {
        //
        //DECLARE @clientType NVARCHAR(1) = '1'
        // DECLARE @clientId NVARCHAR(100) = 'CRM5555'
        // EXEC [dbo].[sp_API_CustomerClient] @clientType,@clientId
        //
        public DbResult FetchAll(DataRequest req)
        {

      
            var conectionString = CrmConfigurationSettings.AppConfig.Get("CRMDB");
            StoreDataReader reader = new StoreDataReader(conectionString);

            req.StoreName = "sp_API_CustomerClient";

            DbResult result = reader.Execute(req);


            return result;
        }

      
    }
}