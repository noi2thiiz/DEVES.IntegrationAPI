using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService
{
   
    public sealed class GlobalTransactionIdGenerator

    {
        private GlobalTransactionIdGenerator()
        {
            if (IsReady==false)
            {
                Init();
            }

        }

        private static readonly object padlock = new object();

        private static GlobalTransactionIdGenerator instance = null;
        public static GlobalTransactionIdGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GlobalTransactionIdGenerator();
                    }
                    return instance;
                }
            }
        }

        private int logCount = 0;
        private string appId = "00";

        
        private string logMonth = "";
        private string logDate = "";
        private Random rnd = new Random();

        public string GetNewGuid()
        {
            while (true)
            {
                var now = DateTime.Now.ToString("yyMMdd");
                var month = DateTime.Now.ToString("yyMM");
                var time = DateTime.Now.ToString("HHmmss");
                var millisecond = DateTime.Now.ToString("fff");
                if (logMonth == month)
                {
                    ++logCount;
                }
                else
                {
                    logCount = 0;

                    logMonth = month;
                }


                // int seed = rnd.Next(100, 999);
                var globalId = appId + "-" + now +""+ time + millisecond+ "-" + (logCount.ToString()).PadLeft(7, '0');
                if (false == globalIdList.ContainsKey(globalId))
                {
                    globalIdList.Add(globalId, 0);

                    try
                    {
                        HttpContext.Current.Items["GlobalTransactionID"] = globalId;
                    }
                    catch (Exception e)
                    {
                        //do nothing
                    }

                    return globalId;

                }
    
            }
        }

        Dictionary<string,int> globalIdList = new Dictionary<string, int>();
        public string GetNewGuid(string globalId)
        {
            try
            {

                if (globalIdList.ContainsKey(globalId))
                {
                    globalIdList[globalId]++;

                    var lastId = globalIdList[globalId];

                    return globalId + "-" + (lastId.ToString()).PadLeft(3, '0');

                }
                else
                {
                    return GetNewGuid();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error On GetNewGuid:"+ e.Message);
                return Guid.NewGuid().ToString();
            }
           
        }

        public void ClearGlobalId(string globalId)
        {
            if (globalIdList.ContainsKey(globalId))
            {
                globalIdList.Remove(globalId);
            }
        }
        
        public MaxTransactionEntity LoadMaxTransactionInfo()
        {
                IDataReader reader;

                //  var client = new RESTClient(endpoint + "/StoreService/ext");
                var req = new DbRequest
                {
                    StoreName = "sp_Get_MaxTransactionID"
                };
                req.AddParam("Machine",Environment.MachineName);
                var configurationString = "";

            
                if (AppConst.IS_SERVER)
                {
                    configurationString = WebConfigurationManager.AppSettings["CRM_CUSTOMAPP_DB_SERVER"];
                    reader = new StoreDataReader(configurationString);
                }
                else if (System.Environment.MachineName == "DESKTOP-Q30CAGJ")
                {
                    configurationString = WebConfigurationManager.AppSettings["CRM_CUSTOMAPP_DB_TON"];
                    reader = new StoreDataReader(configurationString);


                }
                else
                {
                reader = new RestDataReader("ext");
                }

              
               
               
            
            var result = reader.Execute<MaxTransactionEntity>(req);
            Console.WriteLine(result.ToJson());
            if (result.Success)
            {
                if (result.Count>0)
                {
                    var output = (MaxTransactionEntity)result.Data[0];

                    return output;
                }
              
            }


            return default(MaxTransactionEntity);
        }
        public bool IsReady = false;
        public void Init()
        {
            if (Environment.MachineName == AppConst.PRO1_SERVER_NAME)
            {
                appId = "01";
            }
            else if (Environment.MachineName == AppConst.PRO2_SERVER_NAME)
            {
                appId = "02";
            }
            else
            {
                appId = "00";
            }
           
            var maxTransactionInfo = LoadMaxTransactionInfo();
            if (maxTransactionInfo != null)
            {
                logDate = maxTransactionInfo.LogDate ?? "";
                logMonth = maxTransactionInfo.LogMonth ?? "";
                logCount = maxTransactionInfo.MaxTransactionID;
            }
            
            IsReady = true;
            //หา ลำดับล่าสุดจาก ฐานข้อมูล
        }

        public class MaxTransactionEntity
        {
            public int id { get; set; } = 0;
            public string Machine { get; set; } = "";
            public string EnvID { get; set; } = "00";
            public int MaxTransactionID { get; set; } = 0;

            public string LogMonth { get; set; } = "";
            public string LogDate { get; set; } = "";
            
                
        }
      

    }
}