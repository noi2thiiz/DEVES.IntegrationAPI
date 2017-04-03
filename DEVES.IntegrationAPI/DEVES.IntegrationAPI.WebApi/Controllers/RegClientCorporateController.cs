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
using DEVES.IntegrationAPI.Model.RegClientCorporate;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegClientCorporateController : ApiController
    {
        public object Post([FromBody]object value)
        {
            // var data = File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/TEST_Response_RegClientCorporate.json"));
            buzCRMRegClientCorporate cmdCrmRegClientCorporate = new buzCRMRegClientCorporate();
            cmdCrmRegClientCorporate.TransactionId = Request.Properties["TransactionID"].ToString();
            var contentOutput = cmdCrmRegClientCorporate.Execute(cmdCrmRegClientCorporate.DeserializeJson<RegClientCorporateInputModel>(value.ToString()));
            return Request.CreateResponse(contentOutput);
        }

        public object Put([FromBody]object value)
        {
            buzCreateCrmClientCorporate cmd = new buzCreateCrmClientCorporate();
            var content = cmd.Execute(value);
            return Request.CreateResponse(content);
        }
    }
}
