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

        private string isStringNull(string a)
        {
            if (dt.Rows[0][a] == null)
            {
                return null;
            }
            else
            {
                return dt.Rows[0][a].ToString();
            }
        }
        private int? isIntNull(string a)
        {
            if (dt.Rows[0][a] == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(dt.Rows[0][a]);
            }
        }
        private double isDoubleNull(string a)
        {
            if(string.IsNullOrEmpty(dt.Rows[0][a].ToString()))
            {
                return Double.NaN;
            }
            else
            {
                return Convert.ToDouble(dt.Rows[0][a]);
            }
        }

        private RequestSurveyorInputModel Mapping(string caseNo, string userCode)
        {
            dt = new System.Data.DataTable();
            dt = q.Queryinfo_RequestSurveyor(caseNo, userCode);

            RequestSurveyorInputModel rsModel = new RequestSurveyorInputModel();

            rsModel.claimNotiNo = isStringNull("EventCode");
            rsModel.claimNotirefer = isStringNull("claimnotirefer");
            rsModel.insureID = isStringNull("InsureID");
            rsModel.rskNo = isStringNull("RSKNo");
            rsModel.tranNo = isStringNull("TranNo");
            rsModel.notifyName = isStringNull("NotifyName");
            rsModel.mobile = isStringNull("Mobile");
            rsModel.driver = isStringNull("Driver");
            rsModel.driverTel = isStringNull("DriverTel");
            rsModel.currentVehicleLicence = isStringNull("current_VehicleLicence");
            rsModel.currentProvince = isStringNull("current_Province");
            rsModel.eventDate = isStringNull("EventDate");
            rsModel.activityDate = isStringNull("ActivityDate");
            rsModel.eventDetail = isStringNull("EventDetail");
            rsModel.isCasualty = isIntNull("isCasualty");
            rsModel.eventLocation = isStringNull("EventLocation");
            rsModel.accidentLocation = isStringNull("accidentLocation");
            rsModel.accidentLat = isDoubleNull("accidentLat");
            rsModel.accidentLng = isDoubleNull("accidentLng");
            rsModel.ISVIP = isStringNull("IsVIP");
            rsModel.remark = isStringNull("Remark");
            rsModel.claimTypeID = isIntNull("ClameTypeID");
            rsModel.subClaimTypeID = isIntNull("SubClameTypeID");
            rsModel.empCode = isStringNull("informBy");
            rsModel.appointLat = isDoubleNull("appointLat");
            rsModel.appointLong = isDoubleNull("appointLong");
            rsModel.appointLocation = isStringNull("appointLocation");
            rsModel.appointDate = isStringNull("appointDate");
            rsModel.appointName = isStringNull("appointName");
            rsModel.appointPhone = isStringNull("appointPhone");
            rsModel.contractName = isStringNull("contractName");
            rsModel.contractPhone = isStringNull("contractPhone");

            return rsModel;
        }

        private ISurvey_RequestSurveyorDataOutputModel RequestSurveyorOniSurvey(string caseNo, string userCode)
        {

            ///PFC:: Change fixed values to be the configurable values
            ///


            RequestSurveyorInputModel iSurveyInputModel = Mapping(caseNo, userCode);
            EWIRequest reqModel = new EWIRequest()
            {
                username = "ClaimMotor",
                password = "1234",
                token = "",
                gid = "",
                content = iSurveyInputModel
            };

            string jsonReqModel = JsonConvert.SerializeObject(reqModel, Formatting.Indented, new EWIDatetimeConverter());

            HttpClient client = new HttpClient();

            // URL
            client.BaseAddress = new Uri("http://192.168.3.194/ServiceProxy/ClaimMotor/jsonservice/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // + ENDPOINT
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "MOTOR_RequestSurveyor");
            request.Content = new StringContent(jsonReqModel, System.Text.Encoding.UTF8, "application/json");
            //request.Content = new StringContent(Dummy_Input(), System.Text.Encoding.UTF8, "application/json");

            // check reponse 
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            ISurvey_RequestSurveyorDataOutputModel iSurveyOutput = response.Content.ReadAsAsync<ISurvey_RequestSurveyorDataOutputModel>().Result;

            return iSurveyOutput;

        }

        // For testing
        public object Post([FromBody]object value)
        {
            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: Post", CommonHelper.GetIpAddress());

            //RequestSurveyorInputModel iSurveyInputModel = Mapping("CAS201702-00003", "");

            var output = new ISurvey_RequestSurveyorDataOutputModel();
            
            if (value==null)
            {
                return Request.CreateResponse<ISurvey_RequestSurveyorDataOutputModel>(output);
            }
            
            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<RequestSurveyorInputModel_WebService>(contentText);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/iSurvey_Integration_RequestSurveyor_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                _logImportantMessage += "caseNo: " + contentModel.caseNo;
                output = HandleMessage(contentText, contentModel);
            }
            else
            {
                _logImportantMessage = "Error: Input is not valid.";
                output.responseMessage = _logImportantMessage;
                _log.Error(_logImportantMessage);
//                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.responseCode, Environment.NewLine, output.responseMessage);
            }
            return Request.CreateResponse<ISurvey_RequestSurveyorDataOutputModel>(output);
        }

        private ISurvey_RequestSurveyorDataOutputModel HandleMessage(string valueText, RequestSurveyorInputModel_WebService content)
        {
            //TODO: Do what you want
            // var output = new RequestSurveyorDataOutputModel();
            /*
            RequestSurveyorOutputModel locusClaimRegOutput = new RequestSurveyorOutputModel();
            locusClaimRegOutput.data = new RequestSurveyorDataOutputModel();
            var output = locusClaimRegOutput.data;
            */
            ISurvey_RequestSurveyorDataOutputModel iSurveyOutput = new ISurvey_RequestSurveyorDataOutputModel();
            _log.Info("HandleMessage");
            try
            {
                //content.caseNo
                iSurveyOutput = RequestSurveyorOniSurvey(content.caseNo, content.userCode);
                //locusClaimRegOutput = RequestSurveyorOniSurvey("CAS201702-00003", "G001");
                //output.EventID = "EventID";

                /*
                //TODO: Do something
                // locusClaimRegOutput = RegisterClaimOnLocus(content.caseNo);
                locusClaimRegOutput.code = 200;
                locusClaimRegOutput.message = "Success";
                locusClaimRegOutput.description = "Success";
                locusClaimRegOutput.transactionId = "transactionId";
                locusClaimRegOutput.transactionDateTime = DateTime.Now;
                // locusClaimRegOutput.data = new RequestSurveyorDataOutputModel(); ;
                output.EventID = "EventID";
                */
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

                iSurveyOutput.responseMessage = errorMessage;
            }

            return iSurveyOutput;
        }

        private string Dummy_Input()
        {
            string a = "{\"username\": \"ClaimMotor\",\"password\": \"1234\",\"token\": \"\",\"gid\":\"\",\"content\":{\"claimNotiNo\":\"1702-00005\",\"claimNotiRefer\":\"\",\"CaseID\":\"1234\",\"currentVehicleLicense\":\"ชท5422\",\"currentProvince\":\"กท\",\"insureID\":\"V0734802\",\"rskNo\":\"1\",\"tranNo\":\"1\",\"accidentLocation\":\"เทเวศประกันภัย กรุงเทพมหานคร ประเทศไทย\",\"accidentLng\":\"100.504466\",\"accidentLat\":\"13.756553\",\"eventLocation\":\"เทเวศประกันภัย กรุงเทพมหานคร ประเทศไทย\",\"eventDetail\":\"ประกันชนคู่กรณี\",\"eventDate\":\"2017-02-22 09:00:01\",\"subClaimtypeID\":\"1\",\"claimTypeID\":\"0\",\"notifyName\":\"แจ้งเหตุ ว่าอย่างไร\",\"mobile\":\"0883344322\",\"driver\":\"คนขับ รับส่งของ\",\"driverTel\":\"0992334455\",\"activityDate\":\"2017-02-22 09:00:01\",\"isCasualty\":\"0\",\"ISVIP\":\"N\",\"remark\":\"ทดสอบระบบ\",\"empCode\":\"48105\",\"appointDate\":\"\",\"appointName\":\"\",\"appointPhone\":\"\",\"appointLocation\":\"\",\"appointLat\":\"\",\"appointLong\":\"\",\"contractName\":\"\",\"contractPhone\":\"\"}}";
            return a;
        }

    }
}