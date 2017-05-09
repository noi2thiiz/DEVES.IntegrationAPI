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
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegPayeeCorporateController : BaseApiController
    {
        public object Post([FromBody]object value)
        {
            //var data = File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/TEST_Response_RegPayeeCorporate.json"));
            //var contentOutput = JsonConvert.DeserializeObject(data);
            //return Request.CreateResponse(contentOutput);

            return ProcessRequest<buzCRMRegPayeeCorporate, RegPayeeCorporateInputModel>
                (value, "RegPayeeCorporate_Input_Schema.json");

            
            

        }
    }
}
