using DEVES.IntegrationAPI.Model.RegLead;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegLeadController : BaseApiController
    {

        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzRegLead, RegLeadInputModel>(value, "RegLead_Input_Schema.json");
        }
        
    }
}