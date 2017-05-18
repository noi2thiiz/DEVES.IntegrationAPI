using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
   
    public class CrmApiTestController : ApiController
    {


        public object Post([FromBody]RegClientPersonalInputModel value)
        {
            var endpoint = AppConfig.Instance.Get("http://localhost:50076/api/RegClientPersonal");
            var client = new RESTClient(endpoint);
            var result = client.Execute(value.ToString());
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new InternalErrorException(result.Message);
            }
            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<object>(result.Content);
            return Request.CreateResponse(contentObj);

        }


    }
}