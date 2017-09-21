using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService.Envelonment
{
    public class AppEnvironment
    {
        private static AppEnvironment _instance;

        private AppEnvironment()
        {
        }

        public static AppEnvironment Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new AppEnvironment();

              
                return _instance;
            }
        }

        public string GetApplicationName()
        {
            
                return System.Web.Hosting.HostingEnvironment.ApplicationHost?.GetVirtualPath()?.Replace("/", "")??"";
            
           
        }

        public string GetSiteName()
        {
            return System.Web.Hosting.HostingEnvironment.ApplicationHost?.GetSiteName() ?? "";
        }

        public string GetVirtualPath()
        {
            return System.Web.Hosting.HostingEnvironment.ApplicationHost?.GetVirtualPath() ?? "";
        }
        public string GetPhysicalPath()
        {
            return System.Web.Hosting.HostingEnvironment.ApplicationHost?.GetPhysicalPath() ?? "";
        }

        public string GetJsonSchemaPhysicalPath(string schemaFileName)
        {
            var filePath = "";
            try
            {

                 filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/"+ schemaFileName);
                Console.WriteLine("Steo1:");
                Console.WriteLine(filePath);
                 filePath = filePath?.Replace(@"Tests\bin\Release", "");
                 filePath = filePath?.Replace(@"Tests\bin\Debug", "");
                Console.WriteLine("Steo2:");
                Console.WriteLine(filePath);
            }
            catch (Exception)
            {
                //C:\Users\patiw\Source\Repos\Production1\DEVES.IntegrationAPI\DEVES.IntegrationAPI.WebApiTests1\bin\Release
                //C:\Users\patiw\Source\Repos\Production1\DEVES.IntegrationAPI\DEVES.IntegrationAPI.WebApiTests\bin\Release/App_Data/JsonSchema/RegClientCorporate_Input_Schema.json
                string startupPath = Environment.CurrentDirectory?.Replace(@"Tests\bin\Release", "");
                       startupPath = startupPath?.Replace(@"Tests\bin\Debug", "");

                filePath = startupPath + "/App_Data/JsonSchema/"+ schemaFileName;

                Console.WriteLine("Steo2:");
                Console.WriteLine(filePath);
            }
            return filePath;
        }
    }
}
    
