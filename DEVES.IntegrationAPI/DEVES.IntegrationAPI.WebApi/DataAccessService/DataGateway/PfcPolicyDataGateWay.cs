using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Core.DataAdepter;
using DEVES.IntegrationAPI.Core.DataGateWay;
using System.Data;
using DEVES.IntegrationAPI.WebApi.DataAccessService.Helper;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.DataGateway
{
    public class PfcPolicyDataGateWay: BaseDataGateWay
    {
       public PfcPolicyDataGateWay()
        {
            SetDataAdepter(new DevesCrmDataAdepter("pfc_policy"));
        }

    }
}