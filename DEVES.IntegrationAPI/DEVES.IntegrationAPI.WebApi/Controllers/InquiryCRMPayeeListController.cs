using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
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
    public class InquiryCRMPayeeListController : BaseApiController
    {

       // private string _logImportantMessage;
       // private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(InquiryCRMPayeeListController));

        public object Post([FromBody]object value)
        {
            //throw new Exception("XXXXXXXXXXXX");

            return  ProcessRequest<buzInquiryCRMPayeeList, InquiryCRMPayeeListInputModel>(value, "InquiryCRMPayeeList_Input_Schema.json");
            
        }

   
    }
}