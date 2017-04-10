using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.IO;
using Newtonsoft.Json;

using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegPayeeCorporateController : ApiController
    {
        public object Post([FromBody]object value)
        {
            //var data = File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/TEST_Response_RegPayeeCorporate.json"));
            //var contentOutput = JsonConvert.DeserializeObject(data);
            //return Request.CreateResponse(contentOutput);

            buzCRMRegPayeeCorporate cmdCrmRegPayeeCorporate = new buzCRMRegPayeeCorporate();
            // cmdCrmRegPayeeCorporate.TransactionId = Request.Properties["TransactionID"].ToString();
            var contentOutput = cmdCrmRegPayeeCorporate.Execute(cmdCrmRegPayeeCorporate.DeserializeJson<RegPayeeCorporateInputModel>(value.ToString()));
            return Request.CreateResponse(contentOutput);

        }
    }
}
