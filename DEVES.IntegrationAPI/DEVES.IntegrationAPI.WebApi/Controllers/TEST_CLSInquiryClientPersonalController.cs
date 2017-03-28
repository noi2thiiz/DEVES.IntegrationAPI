using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;
using DEVES.IntegrationAPI.Model.CLS;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class TEST_CLSInquiryClientPersonalController : ApiController
    {
        public object Post([FromBody]object value)
        {
            var data = File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/TEST_Response_CLSInquiryClientPersonal.txt"));
            CLSInquiryPersonalClientContentOutputModel contentOutput = JsonConvert.DeserializeObject< CLSInquiryPersonalClientContentOutputModel>( data );
            return Request.CreateResponse<TEST_CLSInquiryClientPersonalOutput>(new TEST_CLSInquiryClientPersonalOutput(contentOutput));
        }

        public class TEST_CLSInquiryClientPersonalOutput
        {
            public string uid { set; get; }
            public string gid { set; get; }
            public bool success { set; get; }
            public string responseCode { set; get; }
            public string responseMessage { set; get; }
            public object content { set; get; }

            public TEST_CLSInquiryClientPersonalOutput(CLSInquiryPersonalClientContentOutputModel contentOutput)
            {
                this.uid = "DevesClaim";
                this.gid = "DevesClaim";
                this.success = true;
                this.responseCode = "EWI-0000I";
                this.responseMessage = "Your request for the container has been executed successfully.";
                this.content = contentOutput;
            }
        }
    }

}
