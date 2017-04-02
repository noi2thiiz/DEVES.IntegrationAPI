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

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegClientPersonalController : ApiController
    {

        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(RegClientPersonalController));

        public object Post([FromBody]object value)
        {
            buzCRMRegClientPersonal cmdCrmRegClientPersonal = new buzCRMRegClientPersonal();
            cmdCrmRegClientPersonal.TransactionId = Request.Properties["TransactionID"].ToString();
            var content = cmdCrmRegClientPersonal.Execute(cmdCrmRegClientPersonal.DeserializeJson<RegClientPersonalInputModel>( value.ToString()) );
            return Request.CreateResponse(content);
        }

        public object Put([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new RegClientPersonalOutputModel_Pass();
            var outputFail = new RegClientPersonalOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<RegClientPersonalInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/RegClientPersonal_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new RegClientPersonalOutputModel_Pass();
                _logImportantMessage += "cleansingId: " + contentModel.generalHeader.cleansingId;
                outputPass = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<RegClientPersonalOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new RegClientPersonalOutputModel_Fail();
                outputFail.data = new List<RegClientPersonalDataOutputModel_Fail>();
                outputFail.data.Add(new RegClientPersonalDataOutputModel_Fail());
                //List<string> errorMessage = JsonHelper.getReturnError();

                outputFail.message = "Invalid Input(s)";
                outputFail.description = "Some of your input is invalid. Please recheck again.";

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", outputFail.code, Environment.NewLine, outputFail.description);
                return Request.CreateResponse<RegClientPersonalOutputModel_Fail>(outputFail);
            }
        }

        private RegClientPersonalOutputModel_Pass HandleMessage(string valueText, RegClientPersonalInputModel content)
        {
            //TODO: Do what you want
            var output = new RegClientPersonalOutputModel_Pass();

            _log.Info("HandleMessage");

            try
            {
                var RegClientPersonalOutput = new List<RegClientPersonalDataOutputModel_Pass>();

                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "RegClientPersonal is done!";
                //output.transactionId = content.ticketNo;
                output.transactionId = "RegClientPersonal";
                output.transactionDateTime = DateTime.Now.ToString();
                //RegClientPersonalOutput.ticketNo = "ticketNo: " + content.ticketNo;
                //RegClientPersonalOutput. = "{1} ticketNo need to be added from stored";
                //RegClientPersonalOutput.claimNotiNo = "{1} claimNotiNo need to be added from stored";
                output.data = new List<RegClientPersonalDataOutputModel_Pass>();
                output.data.Add(new RegClientPersonalDataOutputModel_Pass());
                //output.data.cleansingId = content.generalHeader[0].cleansingId;
                //output.data.polisyClientId = content.generalHeader[0].polisyClientId;
                //output.data.crmPersonId = content.generalHeader[0].crmPersonId;
                //output.data.personalName = "Napat";
                //output.data.personalSurname = "Akka";
            }
            catch (Exception e)
            {

                var errorMessage = e.GetType().FullName + ": " + e.Message + Environment.NewLine;
                errorMessage += "StackTrace: " + e.StackTrace;

                if (e.InnerException != null)
                {
                    errorMessage += Environment.NewLine + "(InnerException)" + e.InnerException.GetType().FullName + " - " + e.InnerException.Message + Environment.NewLine;
                    errorMessage += "StackStrace: " + e.InnerException.StackTrace;
                }
                _log.Error("RequestId - " + _logImportantMessage);
                _log.Error(errorMessage);
                output.description = "Mapping Error";

            }

            return output;
        }

    }
}