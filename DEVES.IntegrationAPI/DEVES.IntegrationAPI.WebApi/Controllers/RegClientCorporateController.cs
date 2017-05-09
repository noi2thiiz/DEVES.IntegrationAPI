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
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegClientCorporateController : BaseApiController
    {
        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzCRMRegClientCorporate, RegClientCorporateInputModel>
                (value, "RegClientCorporate_Input_Schema.json");
            
        }

    
    }
}
