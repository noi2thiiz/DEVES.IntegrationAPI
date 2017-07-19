using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using Microsoft.Owin;
using Owin;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

[assembly: OwinStartup(typeof(DEVES.IntegrationAPI.WebApi.Startup))]

namespace DEVES.IntegrationAPI.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string licenseKey = WebConfigurationManager.AppSettings.Get("NEWTONSOFT_LICENSES");
            Newtonsoft.Json.Schema.License.RegisterLicense(licenseKey);

            ConfigureAuth(app);

        }
    }
}
