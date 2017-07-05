using DEVES.IntegrationAPI.Model.RegCase;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegCase : BaseApiController
    {

        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzRegCase, RegCaseInputModel>(value, "RegCase_Input_Schema.json");
        }


    }
}