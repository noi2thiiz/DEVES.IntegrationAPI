using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;


namespace System
{
    public class CrmConfigurationSettings
    {
        //"C:/CRM_App_Data/Config/config.json"
        private string configPath =
            @"D:\CRM_App_Data\Config\config.json";

        private string configFolder =
            @"D:\CRM_App_Data\Config\\";

        private string configFile = "config.json";


        private static readonly CrmConfigurationSettings Instant = new CrmConfigurationSettings();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static CrmConfigurationSettings()
        {
        }

        public static CrmConfigurationSettings AppConfig
        {
            get { return Instant; }
        }

        public static CrmConfigurationSettings AppSettings
        {
            get { return Instant; }
        }


        protected dynamic config { get; set; }


        private void OnChanged(object source, FileSystemEventArgs e)
        {

            var json = File.ReadAllText(configPath);
            var jss = new JavaScriptSerializer();

            this.config = jss.Deserialize<dynamic>(json);
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

        private CrmConfigurationSettings()
        {

            var json = File.ReadAllText(configPath);
            var jss = new JavaScriptSerializer();

            this.config = jss.Deserialize<dynamic>(json);


            this.watch(configFolder);

            //this.config = ObjectConverter.Convert(ol);
        }


        public dynamic GetConfig()
        {
            return this.config;
        }

        public dynamic Get(string key)
        {
            List<string> listKey = key.Split('.').ToList<string>();
            dynamic returnValue = config;
            foreach (var sp in listKey)
            {
                returnValue = returnValue[sp];
            }
            return returnValue;
        }
    }
}