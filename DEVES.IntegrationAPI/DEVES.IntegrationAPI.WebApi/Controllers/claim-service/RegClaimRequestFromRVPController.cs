using DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP;
using DEVES.IntegrationAPI.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using System.Net;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegClaimRequestFromRVPController : ApiController
    {

         
        public object Post([FromBody]object value)
        {
            var endpoint = "http://crmapi.deves.co.th/rvp-service/api/RegClaimRequestFromRVP";
                //'""AppConfig.Instance.Get("CRMAPI_ENDPOINT_RegClaimRequestFromRVP");
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