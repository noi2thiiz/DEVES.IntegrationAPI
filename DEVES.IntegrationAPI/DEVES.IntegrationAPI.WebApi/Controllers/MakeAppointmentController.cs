using DEVES.IntegrationAPI.Model.MakeAppointment;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class MakeAppointmentController : BaseApiController
    {

        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzMakeAppointment, MakeAppointmentInputModel>(value, "MakeAppointment_Input_Schema.json");
        }


    }
}