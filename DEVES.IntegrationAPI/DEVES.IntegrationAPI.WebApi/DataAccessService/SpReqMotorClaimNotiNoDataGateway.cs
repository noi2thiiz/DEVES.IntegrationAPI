using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class SpReqMotorClaimNotiNoDataGateway:IDataGateWay
    {


        public DbResult Excecute(string incidentGuid,string requestByName)
        {/*
            DECLARE @ticketNo AS NVARCHAR(20) = 'CAS-00012-L1X2C8';
            DECLARE @uniqueID AS UNIQUEIDENTIFIER = 'E7E760A9-F0C1-E611-80CB-0050568D1874';
            DECLARE @requestByName AS NVARCHAR(250) = 'ADMIN TEST';
            DECLARE @resultCode AS NVARCHAR(10);
            DECLARE @resultDesc as NVARCHAR(1000)
            ----EXEC[dbo].[sp_ReqMotorClaimNotiNo] @uniqueID, @requestByName, @resultCode OUTPUT, @resultDesc;
            */
            try
            {
                DataRequest req = new DataRequest();
            
                req.AddParam("uniqueID", incidentGuid);
                req.AddParam("requestByName", requestByName);
               // req.AddParam("resultCode", null);
               // req.AddParam("resultCode", null);
             
                var conectionString = CrmConfigurationSettings.AppConfig.Get("CRMDB");
                StoreDataReader reader = new StoreDataReader(conectionString);

                req.StoreName = "sp_ReqMotorClaimNotiNoTesT";

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