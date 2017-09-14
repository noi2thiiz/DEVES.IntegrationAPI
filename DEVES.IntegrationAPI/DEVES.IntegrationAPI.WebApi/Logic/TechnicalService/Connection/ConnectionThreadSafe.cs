using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.ConnectCRM
{
    public static class ConnectionThreadSafe
    {
      //  private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ConnectionThreadSafe));
        private static ServerConnection.Configuration config;

        public static OrganizationServiceProxy GetOrganizationProxy()
        {
            //_log.Info("Call GetOrganizationProxy Method.");
            if (config == null)
            {
               // _log.Info("Use New ServerConnection.Configuration.");
                ServerConnection serverConnect = new ServerConnection();
                config = serverConnect.GetServerConfiguration();
            }
            else if(config.OrganizationName != GetAppConfig.OrganizationName)
            {
               //_log.Info("Use New ServerConnection.Configuration.");
                ServerConnection serverConnect = new ServerConnection();
                config = serverConnect.GetServerConfiguration();
            }
            else
            {
               // _log.Info("Use Existing ServerConnection.Configuration.");
            }
            return ServerConnection.GetOrganizationProxy(config);
        }
    }

    internal static class GetAppConfig
    {
        public static string ServerAddress { get { return ConfigurationManager.AppSettings["XRM_ServerAddress"]; } }
        public static string SSL { get { return ConfigurationManager.AppSettings["XRM_SSL"]; } }
        public static string IsO365Org { get { return ConfigurationManager.AppSettings["XRM_IsO365Org"]; } }
        public static string OrganizationName { get { return ConfigurationManager.AppSettings["XRM_OrganizationName"]; } }
        public static string User { get { return ConfigurationManager.AppSettings["XRM_User"]; } }
        public static string Password { get { return ConfigurationManager.AppSettings["XRM_Password"]; } }
    }
}
