using DEVES.IntegrationAPI.Model.RegComplaint;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers 
{
    public class RegComplaintController : BaseApiController
    {
        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzRegComplaint, RegComplaintInputModel>(value, "RegComplaintInputModel.json");
        }
    }
}