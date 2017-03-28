using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegClientCorporateController : ApiController
    {
        public object Post([FromBody]object value)
        {
            var data = File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/TEST_Response_RegClientCorporate.json"));
            var contentOutput = JsonConvert.DeserializeObject(data);
            return Request.CreateResponse(contentOutput);
        }
    }
}
