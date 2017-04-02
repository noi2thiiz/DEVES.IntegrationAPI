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
    public class InquiryCRMPayeeListController : ApiController
    {

        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(InquiryCRMPayeeListController));

        public object Post([FromBody]object value)
        {
            Console.WriteLine(value.ToJson());
            buzInquiryCRMPayeeList cmd = new buzInquiryCRMPayeeList();
            cmd.TransactionId = Request.Properties["TransactionID"].ToString();
           
            var content = cmd.Execute(cmd.DeserializeJson<InquiryCRMPayeeListInputModel>(value.ToString()));
           
            return Request.CreateResponse(content);
        }

        public object Put([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new InquiryCRMPayeeListOutputModel_Pass();
            var outputFail = new InquiryCRMPayeeListOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<InquiryCRMPayeeListInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/InquiryCRMPayeeList_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new InquiryCRMPayeeListOutputModel_Pass();
                _logImportantMessage += "polisyClientId: " + contentModel.polisyClientId;
                outputPass = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<InquiryCRMPayeeListOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new InquiryCRMPayeeListOutputModel_Fail();
                outputFail.data = new InquiryCRMPayeeListDataOutputModel_Fail();
                outputFail.data.fieldErrors = new List<InquiryCRMPayeeListFieldErrors>();
                outputFail.data.fieldErrors.Add(new InquiryCRMPayeeListFieldErrors());
                //List<string> errorMessage = JsonHelper.getReturnError();

                outputFail.message = "Invalid Input(s)";
                outputFail.description = "Some of your input is invalid. Please recheck again.";

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", outputFail.code, Environment.NewLine, outputFail.description);
                return Request.CreateResponse<InquiryCRMPayeeListOutputModel_Fail>(outputFail);
            }
        }

        private InquiryCRMPayeeListOutputModel_Pass HandleMessage(string valueText, InquiryCRMPayeeListInputModel content)
        {
            //TODO: Do what you want
            var output = new InquiryCRMPayeeListOutputModel_Pass();

            _log.Info("HandleMessage");

            try
            {
                var InquiryCRMPayeeListOutput = new List<InquiryCRMPayeeListDataOutputModel_Pass>();

                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "InquiryCRMPayeeList is done!";
                //output.transactionId = content.ticketNo;
                output.transactionId = "InquiryCRMPayeeList";
                output.transactionDateTime = DateTime.Now.ToString();
                //InquiryCRMPayeeListOutput.ticketNo = "ticketNo: " + content.ticketNo;
                //InquiryCRMPayeeListOutput. = "{1} ticketNo need to be added from stored";
                //InquiryCRMPayeeListOutput.claimNotiNo = "{1} claimNotiNo need to be added from stored";
                output.data = new List<InquiryCRMPayeeListDataOutputModel_Pass>();
                output.data.Add(new InquiryCRMPayeeListDataOutputModel_Pass());
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