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
            return System.Web.Hosting.HostingEnvironment.ApplicationHost.GetVirtualPath()?.Replace("/", "");
        }

        public string GetSiteName()
        {
            return System.Web.Hosting.HostingEnvironment.ApplicationHost.GetSiteName();
        }

        public string GetVirtualPath()
        {
            return System.Web.Hosting.HostingEnvironment.ApplicationHost.GetVirtualPath();
        }
        public string GetPhysicalPath()
        {
            return System.Web.Hosting.HostingEnvironment.ApplicationHost.GetPhysicalPath();
        }
    }
}
    
