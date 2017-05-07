using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter
{
    public class RestDataReader: IDataReader
    {
        public string ConnectionString { get; set; }
        public DbResult Execute(DbRequest req)
        {

            var dbResult = new DbResult();

            try
            {   //https://crmappqa.deves.co.th/internal-service/api/StoreService/crm/
                var endpoint = System.Configuration.ConfigurationManager.AppSettings["API_ENDPOINT_INTERNAL_SERVICE"].ToString();
                endpoint += "/StoreService/ext";
                var client = new RESTClient(endpoint);
                var result = client.Execute(req);
              
                //  Console.WriteLine(result.Content);
                JavaScriptSerializer serializer1 = new JavaScriptSerializer();
                StoreOutput obje = serializer1.Deserialize<StoreOutput>(result.Content);

                dbResult.Count = Int32.Parse(obje.total);
                dbResult.Data = obje.data;
                dbResult.Success = true;

                return dbResult;
            }
            catch (Exception e)
            {
                dbResult.Count = 0;
               // dbResult.Data = ;
                dbResult.Success = false;

                return dbResult;
            }
           
        }


        
    }

   
    public class StoreOutput
    {
     
        public string code { get; set; }


        public string message { get; set; }


        public string total { get; set; }


        public List<object> data { get; set; }
    }
    
 
}