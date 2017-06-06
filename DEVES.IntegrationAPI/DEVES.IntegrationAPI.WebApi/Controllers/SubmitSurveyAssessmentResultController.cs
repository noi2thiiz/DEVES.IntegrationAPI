using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.SubmitSurveyAssessmentResult;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class SubmitSurveyAssessmentResultController : BaseApiController
    {

        public object Post([FromBody]object value)
        {
            buzSubmitSurveyAssessmentResult buzCommand = new buzSubmitSurveyAssessmentResult();
            BaseDataModel output = buzCommand.Execute(value);
            SubmitSurveyAssessmentResultOutputModel_Pass outputData = (SubmitSurveyAssessmentResultOutputModel_Pass)output;

            // return Request.CreateResponse<SubmitSurveyAssessmentResultOutputModel_Pass>(outputData);
            return ProcessRequest<buzSubmitSurveyAssessmentResult, SubmitSurveyAssessmentResultInputModel>
                (value, "SubmitSurveyAssessmentResult_Input_Schema.json");
        }
    }
}