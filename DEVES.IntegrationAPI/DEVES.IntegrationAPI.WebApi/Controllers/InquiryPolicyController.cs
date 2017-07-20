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
using DEVES.IntegrationAPI.Model.InquiryPolicyModel;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class InquiryPolicyController : BaseApiController
    {
        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzinquiryPolicy,inquiryPolicyInputModel>(value, "inquiryPolicy_Input_Schema.json");
            
        }
    }
}