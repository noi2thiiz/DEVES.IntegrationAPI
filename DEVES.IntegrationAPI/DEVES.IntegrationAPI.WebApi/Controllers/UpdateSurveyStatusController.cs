using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.UpdateSurveyStatus;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Linq;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Logic;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class UpdateSurveyStatusController : BaseApiController
    {

        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzUpdateSurveyStatus, UpdateSurveyStatusInputModel>(value, "UpdateSurveyStatus_Input_Schema.json");
        }

    }
}