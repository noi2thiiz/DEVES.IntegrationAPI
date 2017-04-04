using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http.Cors;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using System.Web.ModelBinding;
using DEVES.IntegrationAPI.WebApi.Core.Attributes;

namespace DEVES.IntegrationAPI.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            GlobalConfiguration.Configuration.MessageHandlers.Add(new ApiLogHandler());
         
            
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API configuration and services 
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );



            

            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(StringDateTimeAttribute), typeof(RegularExpressionAttributeAdapter));

            //config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new EWIDatetimeConverter());

        }
    }
}
