using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegPayeePersonalController : ApiController
    {

        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(RegPayeePersonalController));

        public object Post([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new RegPayeePersonalOutputModel_Pass();
            var outputFail = new RegPayeePersonalOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<RegPayeePersonalInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/RegPayeePersonal_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new RegPayeePersonalOutputModel_Pass();
                _logImportantMessage += "cleansingId: " + contentModel.generalHeader[0].cleansingId;
                outputPass = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<RegPayeePersonalOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new RegPayeePersonalOutputModel_Fail();
                outputFail.data = new List<RegPayeePersonalDataOutputModel_Fail>();
                outputFail.data.Add(new RegPayeePersonalDataOutputModel_Fail());
                //outputFail.data.fieldErrors = "fieldErrors";
                //List<string> errorMessage = JsonHelper.getReturnError();

                outputFail.message = "Invalid Input(s)";
                outputFail.description = "Some of your input is invalid. Please recheck again.";

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", outputFail.code, Environment.NewLine, outputFail.description);
                return Request.CreateResponse<RegPayeePersonalOutputModel_Fail>(outputFail);
            }
        }

        private RegPayeePersonalOutputModel_Pass HandleMessage(string valueText, RegPayeePersonalInputModel content)
        {
            //TODO: Do what you want
            var output = new RegPayeePersonalOutputModel_Pass();

            _log.Info("HandleMessage");

            try
            {
                var RegPayeePersonalOutput = new List<RegPayeePersonalDataOutputModel_Pass>();

                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "RegPayeePersonal is done!";
                //output.transactionId = content.ticketNo;
                output.transactionId = "RegPayeePersonal";
                output.transactionDateTime = DateTime.Now.ToString();
                //RegPayeePersonalOutput.ticketNo = "ticketNo: " + content.ticketNo;
                //RegPayeePersonalOutput. = "{1} ticketNo need to be added from stored";
                //RegPayeePersonalOutput.claimNotiNo = "{1} claimNotiNo need to be added from stored";
                output.data = new List<RegPayeePersonalDataOutputModel_Pass>();
                output.data.Add(new RegPayeePersonalDataOutputModel_Pass());
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