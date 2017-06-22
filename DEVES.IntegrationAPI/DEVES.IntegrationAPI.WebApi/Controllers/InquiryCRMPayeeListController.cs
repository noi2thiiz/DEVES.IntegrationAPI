using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
           
            System.Diagnostics.Stopwatch timer = new Stopwatch();
            timer.Start();

            var output =   ProcessRequest<buzInquiryCRMPayeeListNew, InquiryCRMPayeeListInputModel>(value, "InquiryCRMPayeeList_Input_Schema.json");

            timer.Stop();
            TimeSpan t = timer.Elapsed;
            System.Diagnostics.Debug.WriteLine("Post Execute =" + t.TotalMilliseconds);

            return output;
        }


    }

   
}