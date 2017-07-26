using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.WebApi;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Logic;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using DEVES.IntegrationAPI.Model.InquiryCaseModel;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class inquiryCaseController :BaseApiController
    {
        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzinquiryCase, inquiryCaseInputModel>(value, "inquiryCase_Input_Schema.json");
        }
    }
}