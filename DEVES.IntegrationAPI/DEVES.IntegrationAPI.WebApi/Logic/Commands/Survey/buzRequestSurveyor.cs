using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RequestSurveyor;
using DEVES.IntegrationAPI.Model.EWI;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzRequestSurveyor : BuzCommand
    {
        private QueryInfo q = new QueryInfo();
        private System.Data.DataTable dt = new System.Data.DataTable();

        public override BaseDataModel ExecuteInput(object input)
        {
            // Deserialize Input
            RequestSurveyorInputModel_WebService contentInput = (RequestSurveyorInputModel_WebService)input;
            // Preparation Variable
            RequestSurveyorDataOutputModel output = new RequestSurveyorDataOutputModel();

            EWIResponseContent_ReqSur iSurveyOutput = new EWIResponseContent_ReqSur();
            try
            {
               
                iSurveyOutput = RequestSurveyorOniSurvey(contentInput.incidentId, contentInput.currentUserId);
                output.eventID = iSurveyOutput.eventid;
                output.errorMessage = iSurveyOutput.errorMessage;
            }
            catch (BuzErrorException e)
            {
                output.eventID = "";
                output.errorMessage = e.Message;
                if (e.Message.Contains("เลขรับแจ้งมีอยู่แล้วในระบบ"))
                {
                    iSurveyOutput.eventid = "";
                    iSurveyOutput.errorMessage = "ส่งข้อมูลเข้า i-Survey ไม่สำเร็จ: เลขรับแจ้งมีอยู่แล้วในระบบ";
                }
                else
                {
                    iSurveyOutput.eventid = "";
                    iSurveyOutput.errorMessage = "ส่งข้อมูลเข้า i-Survey ไม่สำเร็จ: " + e.Message;
                }
            }
            catch (Exception e)
            {
                output.eventID = "";
                output.errorMessage = e.Message;
            }
            

            return output;
        }

        private EWIResponseContent_ReqSur RequestSurveyorOniSurvey(string incidentId, string currentUserId)
        {
            RequestSurveyorInputModel iSurveyInputModel = Mapping(incidentId, currentUserId);
            var service = new MOTORRequestSurveyor(TransactionId, ControllerName);
            var ewiRes = service.ExecuteEWI(iSurveyInputModel);

            EWIResponseContent_ReqSur iSurveyOutput = new EWIResponseContent_ReqSur();

            if (ewiRes.content.ToString().Equals("{}"))
            {
                iSurveyOutput.eventid = "";
                iSurveyOutput.errorMessage = ewiRes.responseMessage;
            }
            else if (ewiRes.responseCode != "EWI-0000I")
            {
                iSurveyOutput.eventid = "";
                iSurveyOutput.errorMessage = "ส่งข้อมูลเข้า i-Survey ไม่สำเร็จ: " + ewiRes.responseMessage.ToString();
            }
            else
            {
                iSurveyOutput.eventid = ewiRes.content.eventId;
                iSurveyOutput.errorMessage = null;
            }
           
            return iSurveyOutput;
        }

        private EWIResponseContent_ReqSur RequestSurveyorOniSurveyOld(string incidentId, string currentUserId)
        {
            RequestSurveyorInputModel iSurveyInputModel = Mapping(incidentId, currentUserId);

            EWIRequest reqModel = new EWIRequest()
            {
                username = AppConfig.GetEwiUsername(),
                password = AppConfig.GetEwiPassword(),
                uid = AppConfig.GetEwiUid(),
                gid = AppConfig.GetEwiGid(),
                token = "",
                content = iSurveyInputModel
            };

            string jsonReqModel =
                JsonConvert.SerializeObject(reqModel, Formatting.Indented, new EWIDatetimeConverter());

            HttpClient client = new HttpClient();

            // URL
            // reqTime = DateTime.Now;
            var crmEndpoint = CommonConstant.PROXY_ENDPOINT;
            var ewiEndpoint = crmEndpoint +
                              System.Configuration.ConfigurationManager.AppSettings["API_ENDPOINT_EWIPROXY_SERVICE"] + "MOTOR_RequestSurveyor";
            Console.WriteLine(ewiEndpoint);


            Console.WriteLine(jsonReqModel);
            client.BaseAddress = new Uri(crmEndpoint + ewiEndpoint);
            client.DefaultRequestHeaders.Accept.Clear();
            var media = new MediaTypeWithQualityHeaderValue("application/json") { CharSet = "utf-8" };
            client.DefaultRequestHeaders.Accept.Add(media);
            client.DefaultRequestHeaders.Add("Accept-Encoding", "utf-8");

            // + ENDPOINT

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ewiEndpoint);
            request.Content = new StringContent(jsonReqModel, System.Text.Encoding.UTF8, "application/json");
            //request.Content = new StringContent(Dummy_Input(), System.Text.Encoding.UTF8, "application/json");
            // LogAsync(request, jsonReqModel);
            // check reponse 
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();

            EWIResponse_ReqSur ewiRes = response.Content.ReadAsAsync<EWIResponse_ReqSur>().Result;
            // EWIResponseContent_ReqSur iSurveyOutput = (EWIResponseContent_ReqSur)ewiRes.content;
            //resBody = ewiRes.ToJson();
            // resTime = DateTime.Now;

            EWIResponseContent_ReqSur iSurveyOutput = new EWIResponseContent_ReqSur();
            // iSurveyOutput.eventid = ewiRes.content.ToString();
            if (ewiRes.content.ToString().Equals("{}"))
            {
                iSurveyOutput.eventid = ewiRes.content.ToString();
                iSurveyOutput.errorMessage = ewiRes.responseMessage.ToString();
            }
            else if (ewiRes.responseCode != "EWI-0000I")
            {
                iSurveyOutput.eventid = "ส่งข้อมูลเข้า i-Survey ไม่สำเร็จ" + "\n" + "กรุณากดอีกครั้งหรือติดต่อแผนก IT";
                iSurveyOutput.errorMessage = "ส่งข้อมูลเข้า i-Survey ไม่สำเร็จ: " + ewiRes.responseMessage.ToString();
            }
            else
            {
                iSurveyOutput.eventid = ewiRes.content.ToString();
                iSurveyOutput.errorMessage = null;
            }
            LogAsync(request, response);
            return iSurveyOutput;
        }

        private RequestSurveyorInputModel Mapping(string incidentId, string currentUserId)
        {
            dt = new System.Data.DataTable();
            dt = q.Queryinfo_RequestSurveyor(incidentId, currentUserId);

            RequestSurveyorInputModel rsModel = new RequestSurveyorInputModel();

            rsModel.CaseID = isStringNull("ticketNunber");
            rsModel.claimNotiNo = isStringNull("EventCode");
            rsModel.claimNotirefer = isStringNull("claimnotirefer");
            rsModel.insureID = isStringNull("InsureID");
            rsModel.rskNo = isStringNull("RSKNo");
            rsModel.tranNo = isStringNull("TranNo");
            rsModel.notifyName = isStringNull("NotifyName");
            rsModel.mobile = isStringNull("Mobile");
            rsModel.driver = isStringNull("Driver");
            rsModel.driverTel = isStringNull("DriverTel");
            rsModel.currentVehicleLicense = isStringNull("current_VehicleLicence");
            rsModel.currentProvince = isStringNull("current_Province");
            rsModel.eventDate = isDateTimeNull(isStringNull("EventDate"));
            rsModel.activityDate = isDateTimeNull(isStringNull("ActivityDate"));
            rsModel.eventDetail = isStringNull("EventDetail");
            rsModel.isCasualty = isStringNull("isCasualty");
            if (rsModel.isCasualty.Equals(""))
            {
                rsModel.isCasualty = "0";
            }
            rsModel.eventLocation = isStringNull("EventLocation");
            rsModel.accidentLocation = isStringNull("accidentLocation");
            rsModel.accidentLat = isStringNull("accidentLat");
            if (rsModel.accidentLat.Equals(""))
            {
                rsModel.accidentLat = "0";
            }
            rsModel.accidentLng = isStringNull("accidentLng");
            if (rsModel.accidentLng.Equals(""))
            {
                rsModel.accidentLng = "0";
            }
            rsModel.ISVIP = isStringNull("IsVIP");
            rsModel.remark = isStringNull("Remark");
            rsModel.claimTypeID = isStringNull("ClameTypeID");
            rsModel.subClaimtypeID = isStringNull("SubClameTypeID");
            rsModel.empCode = isStringNull("informBy");
            rsModel.appointLat = isStringNull("appointLat");
            rsModel.appointLong = isStringNull("appointLong");
            rsModel.appointLocation = isStringNull("appointLocation");
            rsModel.appointDate = isDateTimeNull(isStringNull("appointDate"));
            rsModel.appointName = isStringNull("appointName");
            rsModel.appointPhone = isStringNull("appointPhone");
            rsModel.contractName = isStringNull("contractName");
            rsModel.contractPhone = isStringNull("contractPhone");
            rsModel.surveyTeam = isStringNull("surveyTeam");

            return rsModel;
        }

        protected string isStringNull(string a)
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
        protected int isIntNull(string a)
        {
            if (string.IsNullOrEmpty(dt.Rows[0][a].ToString()))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(dt.Rows[0][a]);
            }
        }
        protected double isDoubleNull(string a)
        {
            if (string.IsNullOrEmpty(dt.Rows[0][a].ToString()))
            {
                return 0;
            }
            else
            {
                return Convert.ToDouble(dt.Rows[0][a]);
            }
        }

        protected DateTime isDateTimeNull(string a)
        {
            string datetime = "";

            if (a.Equals(""))
            {
                datetime = "01/01/1900 0:00:00 AM";
            }
            else
            {
                datetime = a.ToString();
            }

            return Convert.ToDateTime(datetime);
        }

    }
}