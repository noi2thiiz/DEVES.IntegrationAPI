using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Summary description for connect
/// </summary>
/// 
namespace UpdateContactName
{
    public class connect
    {
        protected static OrganizationServiceProxy _serviceProxy;
        string connectString = ConfigurationManager.ConnectionStrings["DBCRM"].ConnectionString;

        public bool IsConnect { get { return _serviceProxy != null ? true : false; } }

        public void ConnectCRM()
        {
            CrmServiceClient client = new CrmServiceClient(connectString);
            _serviceProxy = client.OrganizationServiceProxy;
            if (_serviceProxy == null)
            {
                Console.WriteLine("Cannot connect to CRM, connectionString:\"" + connectString + "\"");
            }
            else
            {
                Console.WriteLine("CRM Connected.");
            }
        }
    }
}
