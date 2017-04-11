using DEVES.IntegrationAPI.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.DataAccessService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.DataGateway
{
    public class DevesCrmDataAdepter : CrmDataAdepter
    {
        public DevesCrmDataAdepter(string dataEntityName) 
        {
            dataEntityName = dataEntityName;
            ConnectionString = EnvironmentDataService.Instance.GetXrmConnectionString();
        }
    }
}