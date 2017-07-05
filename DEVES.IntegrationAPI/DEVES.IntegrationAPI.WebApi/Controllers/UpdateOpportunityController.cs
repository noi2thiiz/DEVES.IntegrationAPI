using DEVES.IntegrationAPI.Model.UpdateOpportunity;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class UpdateOpportunityController : BaseApiController
    {

        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzUpdateOpportunity, UpdateOpportunityInputModel>(value, "UpdateOpportunity_Input_Schema.json");
        }


    }
}