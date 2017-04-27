using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace DEVES
{

   /*
    [RoutePrefix("api/update-config")]
    public class updateConfigController : ApiController
    {
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(object dataConfig)
        {

            try
            {
                CrmConfigurationService.AppConfig.updateConfig(dataConfig.ToString());
                return Ok(dataConfig);
            }
            catch (System.Exception e)
            {
                return InternalServerError(e);
            }
        }

    }
    */
    public class CrmConfigurationService
    {


        public static string ConfigPath = @"C:\CRM_App_Data\Config\config.json";
        public static string ConfigApiEndpoint = @"";
        public static string ConfigFolder = @"C:\CRM_App_Data\Config\\";
        public static string ConfigFile = "config.json";

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static CrmConfigurationService()
        {
        }

        public int X { get; set; }

        public int GetX()
        {
            return X;
        }

        public static CrmConfigurationService AppConfig { get; } = new CrmConfigurationService();

        public static CrmConfigurationService AppSettings => AppConfig;


        protected dynamic Config { get; set; }
        protected Dictionary<string, dynamic> ExtraConfig { get; set; }


        private void OnChanged(object source, FileSystemEventArgs e)
        {

            var json = File.ReadAllText(ConfigPath);
            var jss = new JavaScriptSerializer();

            this.Config = jss.Deserialize<dynamic>(json);
        }

        private void watch(string path)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.*";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }


        public void updateConfig(string jsonConfig)
        {
            var jss = new JavaScriptSerializer();
            this.Config = jss.Deserialize<dynamic>(jsonConfig);

        }

        public void Cache(string jsonConfig)
        {
            try
            {
                string json = JsonConvert.SerializeObject(jsonConfig, Formatting.Indented);
                File.WriteAllText(ConfigFile, json);

            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void updateFromServer()
        {
            var json = new WebClient().DownloadString(ConfigApiEndpoint);

            var jss = new JavaScriptSerializer();
            Config = jss.Deserialize<dynamic>(json);

        }
        public void updateFromFile()
        {
            var json = File.ReadAllText(ConfigPath);
            var jss = new JavaScriptSerializer();

            Config = jss.Deserialize<dynamic>(json);

        }

        private CrmConfigurationService()
        {
            Init(@"C:\CRM_App_Data\Config\\");
           
        }

        public void Init(string configFolder)
        {
            ConfigFolder = configFolder;
            updateFromFile();
            watch(configFolder);

           // var proxy = "https://crmappdev.deves.co.th/proxy/xml.ashx?";

           // var SMSServiceEnpoint = proxy + "https://192.168.3.194/ServiceProxy/Common/jsonservice/COMP_SendSMS";
           // var CTIMakeCallEnpoint = proxy + "https://10.10.0.22:8443/webdialer/services/WebdialerSoapService";

          //  var LERTFinishEndpoint = "http://ilerturest.cloudapp.net/v1/thirdparty/lert/finish";
            ExtraConfig = new Dictionary<string, dynamic>
            {
                {
                    "SMSRequestDefault", new
                    {
                        username = "sysdynamic",
                        password = "REZOJUNtN04",
                        token = "",
                        gid = "SendSMS",
                        uid = "CRM_Claim",
                        content = new
                        {
                            message = "",
                            mobileNumber = "",
                        }
                    }
                }
            };

           // var properties = config.settings.GetType().GetProperties();
            foreach (var item in (Dictionary<string,dynamic>)Config["settings"])
            {
                
                ExtraConfig.Add(item.Key, item.Value);
            }


            //this.config = ObjectConverter.Convert(ol);
        }

        public dynamic GetConfig()
        {
            return this.Config;
        }

        public dynamic Get(string key)
        {
            if (ExtraConfig.ContainsKey(key))
            {
               
                return ExtraConfig[key];
            }

            
           

            try
            {
                Console.WriteLine(key);

                List<string> listKey = key.Split('.').ToList<string>();
                dynamic returnValue = Config;
                foreach (var sp in listKey)
                {
                    returnValue = returnValue[sp];
                }
                return returnValue;
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Key [" + key + "] Not Found");
            }

        }
    }


}