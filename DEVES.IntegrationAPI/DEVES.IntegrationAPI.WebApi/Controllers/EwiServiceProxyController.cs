using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    //[RoutePrefix("api/ewi-service-proxy")]
    public class EwiServiceProxyController : ApiController
    {
        [HttpPost]
        [Route("{endPointKey}")]
        [ResponseType(typeof(DbResult))]
        public IHttpActionResult Post(string endPointKey,[FromBody]object value)
        {
            var endpoint = AppConfig.Instance.Get("EWI_ENDPOINT_" + endPointKey);
            Console.WriteLine(endpoint);
            var client = new RESTClient(endpoint);
            var result = client.Execute(value.ToString());
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new InternalErrorException(result.Message);
            }
            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<object>(result.Content);
            return Ok(contentObj);

        }
    }
}