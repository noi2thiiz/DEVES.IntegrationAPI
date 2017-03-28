using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class CRMInquiryClientMasterController : ApiController
    {
        public object Post([FromBody]object value)
        {
            buzCrmInquiryClientMaster inqClientCmd = new buzCrmInquiryClientMaster();
            var content = inqClientCmd.Execute(value);
            return Request.CreateResponse(content);
        }
    }
}
