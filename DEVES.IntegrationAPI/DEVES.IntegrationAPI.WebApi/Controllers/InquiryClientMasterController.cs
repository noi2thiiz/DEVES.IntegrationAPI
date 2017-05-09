using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
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
    public class InquiryClientMasterController : BaseApiController
    {
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(InquiryClientMasterController));

        public object Post([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new InquiryClientMasterOutputModel_Pass();
            var outputFail = new InquiryClientMasterOutputModel_Fail();


            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<InquiryClientMasterInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/InquiryClientMaster_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new InquiryClientMasterOutputModel_Pass();
                _logImportantMessage += "cleansingId: " + contentModel.conditionDetail.cleansingId;
                outputPass = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<InquiryClientMasterOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new InquiryClientMasterOutputModel_Fail();
                outputFail.data = new InquiryClientMasterDataOutputModel_Fail();
                outputFail.data.fieldErrors = new List<InquiryClientMasterListFieldErrors>();
                outputFail.data.fieldErrors.Add(new InquiryClientMasterListFieldErrors());
                //List<string> errorMessage = JsonHelper.getReturnError();

                outputFail.message = "Invalid Input(s)";
                outputFail.description = "Some of your input is invalid. Please recheck again.";
                outputFail.transactionId = GetTransactionId();
                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", outputFail.code, Environment.NewLine, outputFail.description);
                return Request.CreateResponse<InquiryClientMasterOutputModel_Fail>(outputFail);
            }
        }

        private InquiryClientMasterOutputModel_Pass HandleMessage(string valueText, InquiryClientMasterInputModel content)
        {
            //TODO: Do what you want
            var output = new InquiryClientMasterOutputModel_Pass();

            _log.Info("HandleMessage");

            try
            {
                var InquiryClientMasterOutput = new List<InquiryClientMasterDataOutputModel_Pass>();

                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "InquiryClientMaster is done!";
                //output.transactionId = content.ticketNo;
                output.transactionId = GetTransactionId();
                output.transactionDateTime = DateTime.Now.ToString();
                //InquiryClientMasterOutput.ticketNo = "ticketNo: " + content.ticketNo;
                //InquiryClientMasterOutput. = "{1} ticketNo need to be added from stored";
                //InquiryClientMasterOutput.claimNotiNo = "{1} claimNotiNo need to be added from stored";
                output.data = new List<InquiryClientMasterDataOutputModel_Pass>();
                output.data.Add(new InquiryClientMasterDataOutputModel_Pass());
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