using DEVES.IntegrationAPI.WebApi.DataAccessService.XrmEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class PfcMotorAccidentDataGateway: BaseCrmSdkTableGateWay<PfcMortorAccident>
    {
        public PfcMotorAccidentDataGateway()
        {
            this.EntityName = "pfc_motor_accident";
        }
    }
}