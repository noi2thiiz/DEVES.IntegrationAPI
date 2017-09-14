using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.UpdateClaimNo;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class UpdateClaimNoController : BaseApiController
    {

        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzUpdateClaimNo, UpdateClaimNoInputModel>(value, "UpdateClaimNo_Input_Schema.json");
        }
    }
}