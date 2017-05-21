using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.WebApi.Controllers;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using TableDependency;
using TableDependency.SqlClient;


//http://www.c-sharpcorner.com/UploadFile/87b416/working-with-sql-notification/
//http://www.c-sharpcorner.com/blogs/the-sql-server-service-broker-for-the-current-database-is-not-enabled-and-as-a-result-query-notifications-are-not-supported-please-enable-the-service-broker-for-this-database-if-you-wish-to-use-not1
namespace DEVES.IntegrationAPI.WebApi
{
    public class AppConfig
    {
        private static AppConfig _instance;

        private AppConfig()
        {
            try
            {
                //ConnectionString = System.Configuration.ConfigurationManager.AppSettings["CRM_CUSTOMAPP_DB"].ToString();
                //Console.WriteLine(ConnectionString);
                //var mapper = new ModelToTableMapper<AppConfigEntity>();
                // mapper.AddMapping(model => model.Key, "Key");

                // dependency = new SqlTableDependency<AppConfigEntity>(ConnectionString, "AppConfig", mapper);
              
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                //do not thing
                //updateConfig();
            }
           
        }
        public static AppConfig Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new AppConfig();
                return _instance;
            }
        }
        private delegate void RateChangeNotification(DataTable table);
        //private TableDependency.SqlClient.SqlTableDependency dependency;
        //System.Configuration.ConfigurationManager.AppSettings['CRM_CUSTOMAPP_DB'].ToString();
        //"Data Source=DESKTOP-Q30CAGJ;Initial Catalog=CRM_CUSTOM_APP;User ID=sa;Password=patiwat";


        private string ConnectionString;


        private readonly SqlTableDependency<AppConfigEntity> dependency;
        public void Startup()
        {
            try
            {
                
                ConnectionString = System.Configuration.ConfigurationManager.AppSettings["CRM_CUSTOMAPP_DB"].ToString();
                
               // Console.WriteLine("Start auto update config");


               // dependency.OnChanged += dependency_OnChanged; //new OnChangeEventHandler(OnChange);
               // dependency.OnError += dependency_OnError;
                // dependency.OnError += OnError;
                updateConfig();
               // dependency.Start();
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot  Start auto update config");
               // updateConfig();
            }
           
        }
        private bool isStartTed = false;
        public void StartupForUnitTest()
        {
            if (!isStartTed)
            {
                updateConfig();
            }
        }

        private void updateConfig()
        {
            string env = System.Configuration.ConfigurationManager.AppSettings["ENVIRONMENT"] ?? "QA";
            Console.WriteLine("UpdateConfig Change");
            RestDataReader rest = new RestDataReader();
            var request = new DbRequest
            {
                StoreName = "sp_Query_AppConfig",

            };
            request.AddParam("Environment", env.ToUpperIgnoreNull());
            var result = rest.Execute(request);
            if (result.Success)
            {  
                Console.WriteLine(result.ToJson());
                foreach (Dictionary<string, dynamic> item in result.Data)
                {

                    var key = item["Key"];
                    var value = item["Value"];
                    if (!Config.ContainsKey(key))
                    {
                        Config.Add(key, value);
                    }
                    else
                    {
                        Config[key] = value;
                    }
                }
                
                Console.WriteLine("update config success");
            }
            else
            {
                Console.WriteLine("update config fail");
            }
           
            //Read config form Database 
        }

        private void dependency_OnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<AppConfigEntity> e)
        {
            Console.WriteLine("Config Change");
            updateConfig();
        }

        private void dependency_OnError(object sender, TableDependency.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine("Config error" + e.Message+e.Error.ToString());

            Startup();
        }
        /*
        private void OnChange(object sender, SqlNotificationEventArgs e)
        {
            Console.WriteLine("Config Change");
        }*/
       
     
        public void StopNotification()
        {
            SqlDependency.Stop(this.ConnectionString, "AppConfigChangeMessage");
        }
        protected Dictionary<string, dynamic> Config { get; set; } = new Dictionary<string, dynamic>();

        public dynamic Get(string key)
        {
            if (Config.ContainsKey(key))
            {
                return Config[key];
            }

            if (System.Configuration.ConfigurationManager.AppSettings[key] != null)
            {
                return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
            }
          
                return null;
            

        }

        public static string GetEwiUsername()
        {

            return AppConfig.Instance.Get("EWI_USERNAME") ?? "sysdynamic";
        }
        public static string GetEwiPassword()
        {

            return AppConfig.Instance.Get("EWI_PASSWORD") ?? "SzokRk43cEM=";
        }

        public static string GetEwiUid()
        {

            return AppConfig.Instance.Get("EWI_UID") ?? "CRM_CLNT";
        }
        public static string GetEwiGid()
        {

            return AppConfig.Instance.Get("EWI_GID") ?? "CRM_CLNT";
        }

        public void Reload()
        {
            updateConfig();
        }

        public object GetConfig()
        {
            return Config;
        }

        public string GetCRMDBConfigurationString()
        {
            var connection = System.Configuration.ConfigurationManager.AppSettings["CRMDB"];
            if (!string.IsNullOrEmpty(connection))
            {
                return connection;
            }
            else
            {
                throw new AppErrorException("500", "Fatal Error:CRM ConnectionString is empty");
            }
          
           
        }

        public void UpdateEwiConfig(EwiConfig ewiConfig)
        {
            Config["EWI_GID"] = !string.IsNullOrEmpty(ewiConfig.EWI_GID)?ewiConfig.EWI_GID:Config["EWI_GID"];
            Config["EWI_UID"] = !string.IsNullOrEmpty(ewiConfig.EWI_UID)?ewiConfig.EWI_UID:Config["EWI_UID"];
            Config["EWI_USERNAME"] = !string.IsNullOrEmpty(ewiConfig.EWI_USERNAME)?ewiConfig.EWI_USERNAME:Config["EWI_USERNAME"];
            Config["EWI_PASSWORD"] = !string.IsNullOrEmpty(ewiConfig.EWI_PASSWORD)?ewiConfig.EWI_PASSWORD:Config["EWI_PASSWORD"];
            Config["EWI_HOSTNAME"] = !string.IsNullOrEmpty(ewiConfig.EWI_HOSTNAME)?ewiConfig.EWI_PASSWORD:Config["EWI_HOSTNAME"];
           
        }

        public string GetProxyEnpoint()
        {
            
           return System.Configuration.ConfigurationManager.AppSettings["API_ENDPOINT_CRMPROXY"];
            
        }
    }

    public class AppConfigEntity
    {
        public string Key { get; set; }
        public string Enveronment { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
}