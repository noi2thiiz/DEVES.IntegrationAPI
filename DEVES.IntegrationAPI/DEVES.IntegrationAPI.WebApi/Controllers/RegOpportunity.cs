using DEVES.IntegrationAPI.Model.RegOpportunity;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegOpportunity : BaseApiController
    {
        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzRegOpportunity, RegOpportunityInputModel>(value, "RegOpportunity_Input_Schema.json");
        }

    }
}