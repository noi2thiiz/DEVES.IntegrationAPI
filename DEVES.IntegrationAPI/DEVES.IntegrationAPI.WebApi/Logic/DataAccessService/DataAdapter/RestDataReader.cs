using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter
{
    public class RestDataReader: DataReaderBase,IDataReader
    {
        private string db="";

        public RestDataReader(string dbName="")
        {
            this.db= dbName;
        }

        public string ConnectionString { get; set; }
        public DbResult Execute(DbRequest req)
        {

            var dbResult = new DbResult();

            try
            {
                var endpoint = "https://crmappqa.deves.co.th/internal-service/api";
                try
                {
                    if (false ==string.IsNullOrEmpty(System.Configuration.ConfigurationManager
                        .AppSettings["API_ENDPOINT_INTERNAL_SERVICE"]))
                    {
                        endpoint = System.Configuration.ConfigurationManager
                            .AppSettings["API_ENDPOINT_INTERNAL_SERVICE"].ToString();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message+":"+e.StackTrace);
                    //do not thing
                }
                
              
               
                
                if (req.StoreName == "sp_Query_AppConfig"
                       || req.StoreName == "sp_Insert_TransactionLog" 
                       || req.StoreName == "sp_Get_MaxTransactionID"
                       || req.StoreName == "sp_Insert_XrmApiTransactionLog")
                {
                    endpoint += "/StoreService/ext";
                }
                else if (db != "")
                {
                    endpoint += "/StoreService/"+ db;
                }
                else
                {
                    endpoint += "/StoreService/crm";
                }

                

                //Console.WriteLine("Load  Data Store : " + endpoint);
               // Console.WriteLine(req.ToJson());
                var client = new RESTClient(endpoint);
                var result = client.Execute(req);
              
               //  Console.WriteLine(result.Content);
                JavaScriptSerializer serializer1 = new JavaScriptSerializer();
                StoreOutput obje = serializer1.Deserialize<StoreOutput>(result.Content);

                dbResult.Count = Int32.Parse(obje.total);
                dbResult.Data = obje.data;
                dbResult.Success = true;
               // Console.WriteLine("Load  Data Store Result : " + dbResult?.ToJson());
                return dbResult;
            }
            catch (Exception e)
            {
                dbResult.Count = 0;
               // dbResult.Data = ;
                dbResult.Success = false;
                dbResult.Message = e.Message;
                Console.WriteLine(e.Message+"|"+e.StackTrace);

                return dbResult;
            }
           
        }
        public DbResult Execute<T>(DbRequest req)
        {
            
            DbResult result = Execute(req);
            if (result.Success)
            {
                if (result.Count > 0)
                {
                    var sources = result.Data.ToList();
                    result.Data.Clear();
                    foreach (var item in sources)
                    {
                     
                        result.Data.Add(Tranform<T>(item));
                    }


                    
                }

            }
            return result;
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