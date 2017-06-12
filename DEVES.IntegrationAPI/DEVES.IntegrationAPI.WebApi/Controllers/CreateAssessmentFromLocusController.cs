using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Model.CRM;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{

    public class CreateAssessmentFromLocusController:BaseApiController
    {
     
        public object Get([FromUri]object value)
        {

            return ProcessRequest<buzCreateAssessmentFromLocus, CreateAssessmentFromLocusInputModel>
                (value, "CreateAssessmentFromLocus_Input_Schema.json");


        }
    }
}