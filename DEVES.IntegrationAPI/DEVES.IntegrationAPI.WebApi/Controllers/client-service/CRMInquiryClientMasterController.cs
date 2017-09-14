using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Newtonsoft.Json;

using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using System.Web;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.Templates;
using Swashbuckle.Swagger.Annotations;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class CRMInquiryClientMasterController : BaseApiController

    {
        
        public object Post([FromBody]object value)
        {
            //remove redundant validate json schema code
            //+ Deserialize Input
            //InquiryClientMasterInputModel contentModel = DeserializeJson<InquiryClientMasterInputModel>(input.ToString());
            return ProcessRequest<buzCrmInquiryClientMaster, InquiryClientMasterInputModel>
                (value, "InquiryClientMaster_Input_Schema.json");
          
        }
    }
}
