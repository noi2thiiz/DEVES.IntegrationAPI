using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.AccidentPrilimSurveyorReport;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.Xrm.Sdk.Client;
using System.Configuration;
using Microsoft.Xrm.Tooling.Connector;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using System.Linq;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Logic;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class AccidentPrilimSurveyorReportController : BaseApiController
    {
        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzAccidentPrilimSurveyorReport, AccidentPrilimSurveyorReportInputModel>(value, "AccidentPrilimSurveyorReport_Input_Schema.json");
        }

    }
}