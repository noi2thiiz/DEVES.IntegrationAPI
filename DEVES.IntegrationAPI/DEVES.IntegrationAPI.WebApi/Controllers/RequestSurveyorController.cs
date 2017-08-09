using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Model.RequestSurveyor;
using DEVES.IntegrationAPI.Model.EWI;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.UpdateSurveyStatus;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger;
using DEVES.IntegrationAPI.WebApi.Templates;
using WebGrease.Activities;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RequestSurveyorController : BaseApiController
    {
        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzRequestSurveyor, RequestSurveyorInputModel>(value, "RequestSurveyor_Input_Schema.json");
        }

    }
}