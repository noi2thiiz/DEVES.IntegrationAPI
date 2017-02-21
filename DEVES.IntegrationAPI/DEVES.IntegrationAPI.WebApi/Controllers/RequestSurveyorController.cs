using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Model.RequestSurveyor;
using DEVES.IntegrationAPI.Model.EWI;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using DEVES.IntegrationAPI.Core.Helper;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RequestSurveyorController : ApiController
    {
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ClaimRegistrationController));

        private QueryInfo q = new QueryInfo();
        private System.Data.DataTable dt = new System.Data.DataTable();

        private RequestSurveyorInputModel Mapping(string caseNo, string userCode)
        {
            RequestSurveyorInputModel rsModel = new RequestSurveyorInputModel();
            return rsModel;
        }

        private RequestSurveyorOutputModel RequestSurveyorOniSurvey(string caseNo, string userCode)
        {

            ///PFC:: Change fixed values to be the configurable values
            ///


            RequestSurveyorInputModel iSurveyInputModel = Mapping(caseNo, userCode);
            EWIRequest reqModel = new EWIRequest()
            {
                username = "ClaimMotor",
                password = "1234",
                gid = "ClaimMotor",
                token = "",
                content = iSurveyInputModel
            };

            /* 
             * json format 
             * username = "ClaimMotor",
                password = "1234",
                token = "",
                content = locusInputModel
             * */
            string jsonReqModel = JsonConvert.SerializeObject(reqModel, Formatting.Indented, new EWIDatetimeConverter());

            HttpClient client = new HttpClient();

            // URL
            client.BaseAddress = new Uri("http://192.168.3.194/ServiceProxy/ClaimMotor/jsonproxy/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // + ENDPOINT
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "LOCUS_ClaimRegistration");
            request.Content = new StringContent(jsonReqModel, System.Text.Encoding.UTF8, "application/json");
            // request.Content = new StringContent(Dummy_Input(), System.Text.Encoding.UTF8, "application/json");

            // เช็ค check reponse 
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            RequestSurveyorOutputModel locusOutput = response.Content.ReadAsAsync<RequestSurveyorOutputModel>().Result;

            return locusOutput;

        }

        // For testing on Postman
        public object Post([FromBody]object value)
        {
            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: Get", CommonHelper.GetIpAddress());

            var output = new RequestSurveyorDataOutputModel();
            /*
            if (value==null)
            {
                return Request.CreateResponse<RequestSurveyorOutputModel>(output);
            }
            */
            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<RequestSurveyorInputModel>(contentText);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/RequestSurveyor_Input_Schema.json");
            // var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/LOCUS_Integration_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                _logImportantMessage += "ticketNo: " + contentModel.EventCode;
                output = HandleMessage(contentText, contentModel);
            }
            else
            {
                _logImportantMessage = "Error: Input is not valid.";
                output.EventID = _logImportantMessage;
                _log.Error(_logImportantMessage);
//                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.responseCode, Environment.NewLine, output.responseMessage);
            }
            return Request.CreateResponse<RequestSurveyorDataOutputModel>(output);
        }

        private RequestSurveyorDataOutputModel HandleMessage(string valueText, RequestSurveyorInputModel content)
        {
            //TODO: Do what you want
            var output = new RequestSurveyorDataOutputModel();
            RequestSurveyorOutputModel locusClaimRegOutput = new RequestSurveyorOutputModel();
            _log.Info("HandleMessage");
            try
            {

                //TODO: Do something
                // locusClaimRegOutput = RegisterClaimOnLocus(content.caseNo);
                output.EventID = "PASS";
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

                output.EventID = errorMessage;
            }

            return output;
        }

    }
}