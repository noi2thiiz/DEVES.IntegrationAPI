using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.UpdateCompliantStatus;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class UpdateCompliantStatusController : BaseApiController
    {

        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzUpdateCompliantStatus, UpdateCompliantStatusInputModel>(value, "UpdateCompliantStatusInputModel.json");
        }
        

    }
}