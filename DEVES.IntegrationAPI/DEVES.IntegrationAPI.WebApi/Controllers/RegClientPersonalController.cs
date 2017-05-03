using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.WebApi.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegClientPersonalController : BaseApiController
    {

       // private string _logImportantMessage;
       // private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(RegClientPersonalController));

        public object Post([FromBody]object value)
        {

           
            return ProcessRequest<buzCRMRegClientPersonal, RegClientPersonalInputModel>(value, "RegClientPersonal_Input_Schema.json");


        }
     


    }
}