using DEVES.IntegrationAPI.WebApi.DataAccessService.XrmEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class PfcMotorAccidentPartiesDataGateWay : BaseCrmSdkTableGateWay<PfcMotorAccidentParties>
    {
        public PfcMotorAccidentPartiesDataGateWay()
        {
            this.EntityName = "pfc_motor_accident_parties";
        }
        
    }
}