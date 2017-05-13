using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Model.SMS;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    [RoutePrefix("api/SMS")]
    public class SendSMSController : BaseApiController
    {

        [HttpPost]
        [Route("Send")]
        public object Post([FromBody]object value)
        {


            try
            {
                var result =  ProcessRequest<buzSendSMS, SendSMSInputModel>
                    (value, "SendSMS_Input_Schema.json");

                return result;

            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

        }

       
    }
}