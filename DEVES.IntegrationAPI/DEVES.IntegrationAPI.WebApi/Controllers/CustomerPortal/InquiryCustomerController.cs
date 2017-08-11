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
using Swashbuckle.Swagger.Annotations;
using System.Net;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    //[RoutePrefix("api/personSearch")]
    public class InquiryCustomerController : BaseApiController

    {

    //    [HttpPost]
    //    [Route("")]
       public object Post([FromBody]object value)
        {
            return ProcessRequest<buzpersonSearch, personSearchInputModel>(value, "personSearch_Input_Schema.json");
        }
    //    [HttpPost]
    //    [SwaggerResponse(HttpStatusCode.OK, Type = typeof(personSearchOutputModel))]
    //    [Route("Test")]
    //    public IHttpActionResult Test([FromBody] personSearchInputModel value)
    //    {

    //        var result =  ProcessRequest<buzpersonSearch, personSearchInputModel>(value, "personSearch_Input_Schema.json");
    //        return Ok(result);
    //    }
    }
}