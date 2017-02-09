﻿using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.InquiryConsultingHistory;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class InquiryConsultingHistoryController : ApiController
    {
        //To use with log
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(InquiryConsultingHistoryController));

        public object Post([FromBody]object value)
        {
            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var output = new InquiryConsultingHistoryOutputModel();
            if (value == null)
            {
                output.code = 500;
                output.message = "innput is null";
                return Request.CreateResponse<InquiryConsultingHistoryOutputModel>(output);
            }
            var valueText = value.ToString();
            _logImportantMessage = "Username: {0}, Token: {1}, ";
            var contentModel = JsonConvert.DeserializeObject<InquiryConsultingHistoryInputModel>(valueText);
            string outvalidate = string.Empty;            
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/InquiryConsultingHistoryList_Input_Schema.json");

            if (JsonHelper.TryValidateJson(valueText, filePath, out outvalidate))
            {
                _logImportantMessage += "Code: " + contentModel.claimNo;
                output = HandleMessage(valueText, contentModel);
            }
            else
            {
                output = new InquiryConsultingHistoryOutputModel()
                {
                };
                _log.Error(_logImportantMessage);
                //_log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.responseCode, Environment.NewLine, output.responseMessage);
            }
            return Request.CreateResponse<InquiryConsultingHistoryOutputModel>(output);
        }

        private InquiryConsultingHistoryOutputModel HandleMessage(string valueText, InquiryConsultingHistoryInputModel content)
        {
            //TODO: Do what you want
            var output = new InquiryConsultingHistoryOutputModel();
            _log.Info("HandleMessage");
            try
            {
                //TODO: Do something

                output = new InquiryConsultingHistoryOutputModel()
                {
                    code = 200,
                    message = "Success",
                    description = "InquiryConsultingHistory success",
                    transactionDateTime = DateTime.Now ,
                    transactionId = "1234567",
                    data = new InquiryConsultingHistoryDataOutputModel()
                };
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

                output = new InquiryConsultingHistoryOutputModel()
                {
                    code = 505,
                    message = string.Format( "error {0}", e.Message ),
                    description = "",
                    transactionDateTime = DateTime.Now,
                    transactionId = "",
                    data = null
                };
            }

            return output;
        }
    }
}