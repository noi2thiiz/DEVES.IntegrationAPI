using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CRM;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    [RoutePrefix("api/CreateAssessmentFromLocus")]
    public class CreateAssessmentFromLocusController:BaseApiController
    {
        [HttpPost]
        [Route("")]
        public object Post([FromBody]object value)
        {

            return ProcessRequest<buzCreateAssessmentFromLocus, CreateAssessmentFromLocusInputModel>
                (value, "CreateAssessmentFromLocus_Input_Schema.json");


        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var cmd = new buzCreateAssessmentFromLocus();
            cmd.TransactionId = GetTransactionId();
            var result = (BaseContentJsonProxyOutputModel)cmd.Execute(new CreateAssessmentFromLocusInputModel
            {
                requestId = "api"
            });

            return Ok(result);


        }

       
    }
}