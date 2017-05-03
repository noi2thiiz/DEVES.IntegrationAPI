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
            Console.WriteLine("25: START:RegPayeePersonal");
            //var data = File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/TEST_Response_RegPayeeCorporate.json"));
            //var contentOutput = JsonConvert.DeserializeObject(data);
            //return Request.CreateResponse(contentOutput);

            buzCRMRegPayeePersonal cmdCrmRegPayee = new buzCRMRegPayeePersonal();
            cmdCrmRegPayee.TransactionId = GetTransactionId();

            return ProcessRequest<buzCRMRegPayeePersonal, RegPayeePersonalInputModel>(value, "RegPayeePersonal_Input_Schema.json");

           

        }

     
    }
}