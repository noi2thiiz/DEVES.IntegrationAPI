using DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP;
using Microsoft.Xrm.Sdk;
using System;
using System.Reflection;
using DEVES.IntegrationAPI.WebApi.Core.ExtensionMethods;
using DEVES.IntegrationAPI.WebApi.DataAccessService.XrmEntity;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class PfcIncidentDataGateWay : BaseCrmSdkTableGateWay<IncidentEntity>

    {
        public PfcIncidentDataGateWay() 
        {
            EntityName = "incident";
        }
    }

   

  
}