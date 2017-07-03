using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Model.personSearchModel;
using Newtonsoft.Json;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.WebApi;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Logic;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class personSearchController : BaseApiController

    {
        public object Post([FromBody]object value)
        {

            return ProcessRequest<buzpersonSearch, personSearchInputModel>(value, "personSearch_Input_Schema.json");
        }
    }
}