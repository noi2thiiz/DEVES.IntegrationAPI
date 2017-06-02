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
using DEVES.IntegrationAPI.Model.UpdateSurveyStatus;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger;
using DEVES.IntegrationAPI.WebApi.Templates;
using WebGrease.Activities;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RequestSurveyorController : ApiBaseAdHocController
    {
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(RequestSurveyorController));

        private QueryInfo q = new QueryInfo();
        private System.Data.DataTable dt = new System.Data.DataTable();

        private Guid _transactionId = new Guid();

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

            if(a.Equals(""))
            {
                datetime = "01/01/1900 0:00:00 AM";
            }
            else
            {
                datetime = a.ToString();
            }

            return Convert.ToDateTime(datetime);
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
            if(rsModel.accidentLat.Equals(""))
            {
                rsModel.accidentLat = "0";
            }
            rsModel.accidentLng = isStringNull("accidentLng");
            if(rsModel.accidentLng.Equals(""))
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

        private EWIResponseContent_ReqSur RequestSurveyorOniSurvey(string incidentId, string currentUserId)
        {
            try
            {


                ///PFC:: Change fixed values to be the configurable values
                ///


                RequestSurveyorInputModel iSurveyInputModel = Mapping(incidentId, currentUserId);


                /*
                string iSurveyInputContent = iSurveyInputModel.ToString();
                var fileJsonPath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/RequestSurveyor_Input_Schema.json");
                string outvalidate = string.Empty;

                if (!JsonHelper.TryValidateJson(iSurveyInputContent, fileJsonPath, out outvalidate))
                {
                */
                /*
                 *  "CaseID":"", (เพิ่มเติม เลข Case ID)
                    "notifyName": "",
                    "mobile": "",
                    "driver": "",
                    "driverTel": "",
                    "currentVehicleLicence": "",
                    "currentProvince": "",
                    "claimTypeID": 0,
                    "subClaimTypeID": 0,
                    "empCode": "G001",
                 */

                /*
               EWIResponseContent_ReqSur error = new EWIResponseContent_ReqSur();
               error.eventid = "Required field is not follow schema";
               return error;
           }
            */
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
                reqTime = DateTime.Now;
                var crmEndpoint = CommonConstant.PROXY_ENDPOINT;
                var ewiEndpoint = crmEndpoint+
                    System.Configuration.ConfigurationManager.AppSettings["API_ENDPOINT_EWIPROXY_SERVICE"]+ "MOTOR_RequestSurveyor";
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
                LogAsync(request, jsonReqModel);
                // check reponse 
                HttpResponseMessage response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();

                EWIResponse_ReqSur ewiRes = response.Content.ReadAsAsync<EWIResponse_ReqSur>().Result;
                // EWIResponseContent_ReqSur iSurveyOutput = (EWIResponseContent_ReqSur)ewiRes.content;
                resBody = ewiRes.ToJson();
                resTime = DateTime.Now;
             
                EWIResponseContent_ReqSur iSurveyOutput = new EWIResponseContent_ReqSur();
                // iSurveyOutput.eventid = ewiRes.content.ToString();
                if (ewiRes.content.ToString().Equals("{}"))
                {
                    iSurveyOutput.eventid = ewiRes.content.ToString();
                    iSurveyOutput.errorMessage = ewiRes.responseMessage.ToString();
                }
                else if(ewiRes.responseCode != "EWI-0000I")
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
                /*
                ISurvey_RequestSurveyorDataOutputModel iSurveyOutput = response.Content.ReadAsAsync<ISurvey_RequestSurveyorDataOutputModel>().Result;

                return iSurveyOutput;
                */
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message+e.StackTrace);
                LogErrorException(e);
                throw;
            }
            catch (Exception e)
            {
                LogErrorException(e);
                throw;
            }


        }

        // For testing
        public object Post([FromBody]object value)
        {
            _transactionId = Guid.NewGuid();

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: Post", CommonHelper.GetIpAddress());

            //RequestSurveyorInputModel iSurveyInputModel = Mapping("CAS201702-00003", "");

            var output = new RequestSurveyorDataOutputModel();

            if (value == null)
            {
                _logImportantMessage = "Error: Json format is invalid.";
                output.eventID = _logImportantMessage;
                output.errorMessage = _logImportantMessage;
                _log.Error(_logImportantMessage);
                return Request.CreateResponse<RequestSurveyorDataOutputModel>(output);
            }

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<RequestSurveyorInputModel_WebService>(contentText);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/iSurvey_Integration_RequestSurveyor_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                _logImportantMessage += "IncidentId: " + contentModel.incidentId;
                output = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<RequestSurveyorDataOutputModel>(output);
            }
            else
            {
                var outputFail = new UpdateSurveyStatusOutputModel_Fail();
                outputFail.data = new UpdateSurveyStatusDataOutputModel_Fail();
                outputFail.data.fieldError = new List<UpdateSurveyStatusFieldErrorOutputModel_Fail>();

                List<string> errorMessage = JsonHelper.getReturnError();
                foreach (var text in errorMessage)
                {
                    string fieldMessage = "";
                    string fieldName = "";
                    if (text.Contains("Required properties"))
                    {
                        int indexEnd = 0;
                        for (int i = 0; i < text.Length - 1; i++)
                        {
                            if (text.Substring(i, 1).Equals(":"))
                            {
                                fieldMessage = text.Substring(0, i);
                                indexEnd = i + 1;
                            }
                            if (text.Substring(i, 1).Equals("."))
                            {
                                fieldName = text.Substring(indexEnd, i - indexEnd).Trim();
                                break;
                            }
                        }

                        string[] splitProp = fieldName.Split(',');
                        for (int i = 0; i < splitProp.Length; i++)
                        {
                            outputFail.data.fieldError.Add(new UpdateSurveyStatusFieldErrorOutputModel_Fail(splitProp[i].Trim(), fieldMessage));
                        }
                    }
                    else if (text.Contains("exceeds maximum length"))
                    {
                        bool isMessage = false;
                        int endMessage = 0;
                        int startName = 0;
                        int endName = 0;
                        for (int i = 0; i < text.Length - 4; i++)
                        {
                            if (text.Substring(i, 4).Equals("Path"))
                            {
                                fieldMessage = text.Substring(0, i - 1);
                                isMessage = true;
                                endMessage = i + "Path".Length;
                            }
                            if (isMessage)
                            {
                                if (text.Substring(i, 1).Equals("'"))
                                {
                                    if (startName == 0)
                                    {
                                        startName = i + 1;
                                    }
                                    else if (endName == 0)
                                    {
                                        endName = i - 1;
                                    }
                                }
                                if (startName != 0 && endName != 0)
                                {
                                    fieldName = text.Substring(startName, i - startName).Trim();
                                    break;
                                }
                            }
                        }
                    }
                    else if (text.Contains("minimum length"))
                    {
                        bool isMessage = false;
                        int startName = 0;
                        int endName = 0;
                        for (int i = 0; i < text.Length - 7; i++)
                        {
                            string check = text.Substring(i, 7);
                            if (text.Substring(i, 7).Equals("minimum"))
                            {
                                fieldMessage = "Required field must not be null";
                                isMessage = true;
                            }
                            if (isMessage)
                            {
                                if (text.Substring(i, 1).Equals("'"))
                                {
                                    if (startName == 0)
                                    {
                                        startName = i + 1;
                                    }
                                    else if (endName == 0)
                                    {
                                        endName = i - 1;
                                    }
                                }
                                if (startName != 0 && endName != 0)
                                {
                                    fieldName = text.Substring(startName, i - startName).Trim();
                                    break;
                                }
                            }
                        }
                    }
                    else if (text.Contains("Invalid type."))
                    {
                        int startIndex = "Invalid type.".Length;
                        int endMessage = 0;
                        int startName = 0;
                        int endName = 0;
                        for (int i = startIndex; i < text.Length - 1; i++)
                        {
                            if (text.Substring(i, 1).Equals("."))
                            {
                                fieldMessage = text.Substring(0, i);
                                endMessage = i + 1;
                            }
                            if (text.Substring(i, 1).Equals("'"))
                            {
                                if (startName == 0)
                                {
                                    startName = i + 1;
                                }
                                else if (endName == 0)
                                {
                                    endName = i - 1;
                                }
                            }
                            if (startName != 0 && endName != 0)
                            {
                                fieldName = text.Substring(startName, i - startName).Trim();
                                break;
                            }
                        }
                    }
                    else if (text.Contains("not defined in enum"))
                    {
                        int startName = 0;
                        int endName = 0;
                        bool isChecking = true;

                        for (int i = 0; i < text.Length - 1; i++)
                        {
                            if (text.Substring(i, 1).Equals(".") && isChecking)
                            {
                                fieldMessage = text.Substring(0, i);
                                isChecking = false;
                            }
                            if (text.Substring(i, 1).Equals("'"))
                            {
                                if (startName == 0)
                                {
                                    startName = i + 1;
                                }
                                else if (endName == 0)
                                {
                                    endName = i - 1;
                                }
                            }
                            if (startName != 0 && endName != 0)
                            {
                                fieldName = text.Substring(startName, i - startName).Trim();
                                break;
                            }
                        }
                    }

                    if (!text.Contains("Required properties"))
                    {
                        outputFail.data.fieldError.Add(new UpdateSurveyStatusFieldErrorOutputModel_Fail(fieldName, fieldMessage));
                    }

                }

                outputFail.code = "400";
                outputFail.message = "Invalid Input(s)";
                outputFail.description = "Some of your input is invalid. Please recheck again.";
                outputFail.transactionId = _transactionId.ToString();
                outputFail.transactionDateTime = DateTime.Now.ToString();

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", outputFail.code, Environment.NewLine, outputFail.description);
                _logImportantMessage = "Error: Input is not valid.";
                // output.eventID = "";
                output.errorMessage = _logImportantMessage;
                _log.Error(_logImportantMessage);
                //                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.responseCode, Environment.NewLine, output.responseMessage);
                return Request.CreateResponse<RequestSurveyorDataOutputModel>(output);
            }
            
        }

        private RequestSurveyorDataOutputModel HandleMessage(string valueText, RequestSurveyorInputModel_WebService content)
        {
            //TODO: Do what you want
            // var output = new RequestSurveyorDataOutputModel();
            /*
            RequestSurveyorOutputModel locusClaimRegOutput = new RequestSurveyorOutputModel();
            locusClaimRegOutput.data = new RequestSurveyorDataOutputModel();
            var output = locusClaimRegOutput.data;
            */
            RequestSurveyorDataOutputModel output = new RequestSurveyorDataOutputModel();
            try
            {
               
                EWIResponseContent_ReqSur iSurveyOutput = new EWIResponseContent_ReqSur();
               // _log.Info("HandleMessage");
                //content.caseNo
                // iSurveyOutput = RequestSurveyorOniSurvey(content.incidentId, content.currentUserId);
                iSurveyOutput = RequestSurveyorOniSurvey(content.incidentId, content.currentUserId);
                // ISurvey_RequestSurveyorContentDataOutputModel regOutputData = new ISurvey_RequestSurveyorContentDataOutputModel(iSurveyOutput.eventid);
                output.eventID = iSurveyOutput.eventid;
                output.errorMessage = iSurveyOutput.errorMessage;
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

                output.errorMessage = errorMessage;
                LogErrorException(e);
            }

            return output;
        }

        private string Dummy_Input()
        {
            string a = "{\"username\": \"ClaimMotor\",\"password\": \"1234\",\"token\": \"\",\"gid\":\"\",\"content\":{\"claimNotiNo\":\"1702-00005\",\"claimNotiRefer\":\"\",\"CaseID\":\"1234\",\"currentVehicleLicense\":\"ชท5422\",\"currentProvince\":\"กท\",\"insureID\":\"V0734802\",\"rskNo\":\"1\",\"tranNo\":\"1\",\"accidentLocation\":\"เทเวศประกันภัย กรุงเทพมหานคร ประเทศไทย\",\"accidentLng\":\"100.504466\",\"accidentLat\":\"13.756553\",\"eventLocation\":\"เทเวศประกันภัย กรุงเทพมหานคร ประเทศไทย\",\"eventDetail\":\"ประกันชนคู่กรณี\",\"eventDate\":\"2017-02-22 09:00:01\",\"subClaimtypeID\":\"1\",\"claimTypeID\":\"0\",\"notifyName\":\"แจ้งเหตุ ว่าอย่างไร\",\"mobile\":\"0883344322\",\"driver\":\"คนขับ รับส่งของ\",\"driverTel\":\"0992334455\",\"activityDate\":\"2017-02-22 09:00:01\",\"isCasualty\":\"0\",\"ISVIP\":\"N\",\"remark\":\"ทดสอบระบบ\",\"empCode\":\"48105\",\"appointDate\":\"\",\"appointName\":\"\",\"appointPhone\":\"\",\"appointLocation\":\"\",\"appointLat\":\"\",\"appointLong\":\"\",\"contractName\":\"\",\"contractPhone\":\"\"}}";
            return a;
        }

    }
}