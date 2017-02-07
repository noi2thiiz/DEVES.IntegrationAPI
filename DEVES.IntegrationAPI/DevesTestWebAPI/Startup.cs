using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DevesTestWebAPI.Startup))]
namespace DevesTestWebAPI
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
