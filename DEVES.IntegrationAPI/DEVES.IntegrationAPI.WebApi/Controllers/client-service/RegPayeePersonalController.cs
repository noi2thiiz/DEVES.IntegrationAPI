using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegPayeePersonalController : BaseApiController
    {

      //  private string _logImportantMessage;
      //  private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(RegPayeePersonalController));


        public object Post([FromBody]object value)
        {
         

            return ProcessRequest<buzCRMRegPayeePersonal, RegPayeePersonalInputModel>
                (value, "RegPayeePersonal_Input_Schema.json");

           

        }

     
    }
}