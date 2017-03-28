using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.ClaimRegistration;
using DEVES.IntegrationAPI.Model.EWI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi;
using System.Globalization;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.Model;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class ClaimRegistrationController : ApiController
    {
        //To use with log
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ClaimRegistrationController));

        private QueryInfo q = new QueryInfo();
        private System.Data.DataTable dt = new System.Data.DataTable();

        /*
         * isStringNull กับ isIntNull ทำหน้าที่คล้าย ๆ กันครับ
         * 
         * เมื่อรับค่ามาจาก Store ก็จะมีการเช็ค value ก่อนว่าเป็น null ไหม 
         * จริง ๆ ไม่มีอะไรครับ แค่เขียนแยกเพื่อจำได้ไม่ต้องเขียนหลาย ๆ รอบใน Code method Mapping
         *
         **/

        private DateTime? isDateNull(string a)
        {
            DateTime? d = null;
            try
            {
                d= (DateTime)dt.Rows[0][a];
            }
            catch(Exception e) {
            }
            return d;
        }

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
        private int isIntNull(string a)
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

        /* 
         * Method นี้เป็นการ Map ค่าจาก Model ที่ชื่อว่า "LocusClaimRegistrationInputModel" กับข้อมูลใน Store ครับ
         * ปัญหาของตอนนี้คือแปลง object หรือ string value ไปเป็น datetime format ครับ
         **/



        private LocusClaimRegistrationInputModel Mapping(string caseNo)
        {
            throw new NotImplementedException();

            //string format = "yyyy-MM-dd HH:mm:ss";
            //CultureInfo provider = new CultureInfo("th-TH");

            //dt = new System.Data.DataTable();
            //// DECLARE @ticketNo AS NVARCHAR(20) = 'CAS201702-00003';
            //// DECLARE @uniqueID AS UNIQUEIDENTIFIER = 'D246086E-C1EE-E611-80D4-0050568D1874';
            //dt = q.Queryinfo_CallerId(caseNo, null);

            //LocusClaimRegistrationInputModel cr = new LocusClaimRegistrationInputModel();

            //cr.claimHeader = new LocusClaimheaderModel();
            //// claimHeader
            //// Recently Add 
            //cr.claimHeader.premiumClass = isStringNull("premiumClass");
            //cr.claimHeader.teamCd = "";
            ////cr.claimHeader.claimStatus = "";
            //// Recently Add 
            //cr.claimHeader.ticketNumber = isStringNull("ticketNumber");
            //cr.claimHeader.claimNotiNo = isStringNull("claimNotiNo");
            //cr.claimHeader.claimNotiRefer = isStringNull("claimNotiRefer");
            //cr.claimHeader.policyNo = isStringNull("policyNo");
            //cr.claimHeader.fleetCarNo = isIntNull("fleetCarNo");
            //cr.claimHeader.policySeqNo = isIntNull("policySeqNo");
            //cr.claimHeader.renewalNo = isIntNull("renewalNo");
            //cr.claimHeader.barcode = isStringNull("barcode");
            //cr.claimHeader.insureCardNo = isStringNull("insureCardNo");
            //cr.claimHeader.policyIssueDate = isDateNull("policyIssueDate");
            //cr.claimHeader.policyEffectiveDate = isDateNull("policyEffectiveDate");
            //cr.claimHeader.policyExpiryDate = isDateNull("policyExpiryDate");
            //cr.claimHeader.policyProductTypeCode = isStringNull("policyProductTypeCode");
            //cr.claimHeader.policyProductTypeName = isStringNull("policyProductTypeName");
            //cr.claimHeader.policyGarageFlag = isStringNull("policyGarageFlag");
            //cr.claimHeader.policyPaymentStatus = isStringNull("policyPaymentStatus");
            //cr.claimHeader.policyCarRegisterNo = isStringNull("policyCarRegisterNo");
            //cr.claimHeader.policyCarRegisterProv = isStringNull("policyCarRegisterProv");
            //cr.claimHeader.carChassisNo = isStringNull("carChassisNo");
            //cr.claimHeader.carVehicleType = isStringNull("carVehicleType");
            //cr.claimHeader.carVehicleModel = isStringNull("carVehicleModel");
            //cr.claimHeader.carVehicleYear = isStringNull("carVehicleYear");
            //cr.claimHeader.carVehicleBody = isStringNull("carVehicleBody");
            //cr.claimHeader.carVehicleSize = isStringNull("carVehicleSize");
            //cr.claimHeader.policyDeduct = isIntNull("policyDeduct");
            //cr.claimHeader.vipCaseFlag = isStringNull("vipCaseFlag");
            //cr.claimHeader.privilegeLevel = isStringNull("privilegeLevel");
            //cr.claimHeader.highLossCaseFlag = isStringNull("highLossCaseFlag");
            //cr.claimHeader.legalCaseFlag = isStringNull("legalCaseFlag");
            //cr.claimHeader.claimNotiRemark = isStringNull("claimNotiRemark");
            //cr.claimHeader.claimType = isStringNull("claimType");

            //cr.claimInform = new LocusClaiminformModel();
            //// claimInform
            //// Recently Add 
            //cr.claimInform.accidentDesc = "";
            //// Recently Add 
            //cr.claimInform.informerClientId = isStringNull("informerClientId");
            //cr.claimInform.informerFullName = isStringNull("informerFullName");
            //cr.claimInform.informerMobile = isStringNull("informerMobile");
            //cr.claimInform.informerPhoneNo = isStringNull("informerPhoneNo");
            //cr.claimInform.driverClientId = isStringNull("driverClientId");
            //cr.claimInform.driverFullName = isStringNull("driverFullName");
            //cr.claimInform.driverMobile = isStringNull("driverMobile");
            //cr.claimInform.driverPhoneNo = isStringNull("driverPhoneNo");
            //cr.claimInform.insuredClientId = isStringNull("insuredClientId");
            //cr.claimInform.insuredFullName = isStringNull("insuredFullName");
            //cr.claimInform.insuredMobile = isStringNull("insuredMobile");
            //cr.claimInform.insuredPhoneNo = isStringNull("insuredPhoneNo");
            //cr.claimInform.relationshipWithInsurer = isStringNull("relationshipWithInsurer");
            //cr.claimInform.currentCarRegisterNo = isStringNull("currentCarRegisterNo");
            //cr.claimInform.currentCarRegisterProv = isStringNull("currentCarRegisterProv");
            //cr.claimInform.informerOn = isDateNull("informerOn");
            //cr.claimInform.accidentOn = isDateNull("accidentOn");
            //cr.claimInform.accidentDescCode = isStringNull("accidentDescCode");
            //cr.claimInform.numOfExpectInjury = isIntNull("numOfExpectInjury");
            //cr.claimInform.accidentPlace = isStringNull("accidentPlace");
            //cr.claimInform.accidentLatitude = isStringNull("accidentLatitude");
            //cr.claimInform.accidentLongitude = isStringNull("accidentLongitude");
            //cr.claimInform.accidentProvn = isStringNull("accidentProvn");
            //cr.claimInform.accidentDist = isStringNull("accidentDist");
            //cr.claimInform.sendOutSurveyorCode = isStringNull("sendOutSurveyorCode");

            //cr.claimAssignSurv = new LocusClaimassignsurvModel();
            //// claimAssignSurv
            //cr.claimAssignSurv.surveyorCode = isStringNull("surveyorCode");
            //cr.claimAssignSurv.surveyorClientNumber = isStringNull("surveyorClientNumber");
            //cr.claimAssignSurv.surveyorName = isStringNull("surveyorName");
            //cr.claimAssignSurv.surveyorCompanyName = isStringNull("surveyorCompanyName");
            //cr.claimAssignSurv.surveyorCompanyMobile = isStringNull("surveyorCompanyMobile");
            //cr.claimAssignSurv.surveyorMobile = isStringNull("surveyorMobile");
            //cr.claimAssignSurv.surveyorType = isStringNull("surveyorType");
            ///*
            // * just comment
            //cr.claimAssignSurv.reportAccidentResultDate = isDateNull("reportAccidentResultDate"); //DateTime.ParseExact(dt.Rows[0]["reportAccidentResultDate"].ToString(), "yyyy-MM-dd HH:mm tt", null);
            //// cr.claimAssignSurv.reportAccidentResultDate = DateTime.ParseExact(dt.Rows[0]["reportAccidentResultDate"], format, provider);
            //// Recently Add 
            //cr.claimAssignSurv.branchSurvey = "";
            //cr.claimAssignSurv.latitudeLongitude = "";
            //cr.claimAssignSurv.location = "";
            //cr.claimAssignSurv.createBy = "";
            //cr.claimAssignSurv.createDate = null;
            //cr.claimAssignSurv.updateBy = "";
            //cr.claimAssignSurv.updateDate = null;
            //// Recently Add 
            //*/

            //cr.claimSurvInform = new LocusClaimsurvinformModel();
            //// claimSurvInform
            //// Recently Add 
            //cr.claimSurvInform.excessFee = 0; // int
            //cr.claimSurvInform.deductibleFee = 0; // int
            //cr.claimSurvInform.reportAccidentResultDate = isDateNull("reportAccidentResultDate"); // datetime
            //// Recently Add 
            //cr.claimSurvInform.accidentLegalResult = isStringNull("accidentLegalResult");
            //cr.claimSurvInform.policeStation = isStringNull("policeStation");
            //cr.claimSurvInform.policeRecordId = isStringNull("policeRecordId");
            //cr.claimSurvInform.policeRecordDate = null; //DateTime.ParseExact(dt.Rows[0]["policeRecordDate"].ToString(), "yyyy-MM-dd HH:mm tt", null);
            //cr.claimSurvInform.policeBailFlag = isStringNull("policeBailFlag");
            //cr.claimSurvInform.damageOfPolicyOwnerCar = isStringNull("damageOfPolicyOwnerCar");
            //cr.claimSurvInform.numOfTowTruck = 0; //isIntNull("numOfTowTruck"); check type of this vaule in sqlserver
            //cr.claimSurvInform.nameOfTowCompany = isStringNull("nameOfTowCompany");
            //cr.claimSurvInform.detailOfTowEvent = isStringNull("detailOfTowEvent");
            //cr.claimSurvInform.numOfAccidentInjury = isIntNull("numOfAccidentInjury");
            //cr.claimSurvInform.detailOfAccidentInjury = isStringNull("detailOfAccidentInjury");
            //cr.claimSurvInform.numOfDeath = isIntNull("numOfDeath");
            //cr.claimSurvInform.detailOfDeath = isStringNull("detailOfDeath");
            //cr.claimSurvInform.caseOwnerCode = isStringNull("caseOwnerCode");
            //cr.claimSurvInform.caseOwnerFullName = isStringNull("caseOwnerFullName");
            //// LocusAccidentpartyinfo[] accidentPartyInfo 
            ///*
            //cr.claimSurvInform.accidentPartyInfo[0] = null;
            //cr.claimSurvInform.accidentPartyInfo[1] = null;
            //cr.claimSurvInform.accidentPartyInfo[2] = null;
            //cr.claimSurvInform.accidentPartyInfo[3] = null;
            //cr.claimSurvInform.accidentPartyInfo[4] = null;
            //cr.claimSurvInform.accidentPartyInfo[5] = null;
            //cr.claimSurvInform.accidentPartyInfo[6] = null;
            //cr.claimSurvInform.accidentPartyInfo[7] = null;
            //cr.claimSurvInform.accidentPartyInfo[8] = null;
            //cr.claimSurvInform.accidentPartyInfo[9] = null;
            //*/

            //return cr;
        }

        /* 
         * เป็น Method ที่รับค่าที่ยิงมาจาก Postman หรือ Web Service ครับ
         **/
        public object Post([FromBody]object value)
        {

            #region ReleasedCandidate_1

            /*

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: Get", CommonHelper.GetIpAddress());

            var output = new ClaimRegistrationOutputModel();

            if (value==null)
            {
                return Request.CreateResponse<ClaimRegistrationOutputModel>(output);
            }

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<ClaimRegistrationInputModel>(contentText);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/LOCUS_Integration_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                _logImportantMessage += "ticketNo: " + contentModel.caseNo;
                output = HandleMessage(contentText, contentModel);
            }
            else
            {
                _logImportantMessage = "Error: Input is not valid.";
                output.claimID = _logImportantMessage;
                _log.Error(_logImportantMessage);
//                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.responseCode, Environment.NewLine, output.responseMessage);
            }
            return Request.CreateResponse<ClaimRegistrationOutputModel>(output);
            */
            #endregion ReleasedCandidate_1

            
            BuzClaimRegistrationCommand buzCommand = new BuzClaimRegistrationCommand();
            BaseDataModel output = buzCommand.Execute(value);
            return Request.CreateResponse<ClaimRegistrationOutputModel>((ClaimRegistrationOutputModel)output);

        }

        /*
         * หลังจากเรามี input header ที่ชื่อว่า content แล้ว 
         * Method นี้จะ wrap เข้ากับ header ของ EWI อีกทีครีบ
         **/
        
        /*
        private EWIResponseContent RegisterClaimOnLocus(string caseNo)
        {

            ///PFC:: Change fixed values to be the configurable values
            ///

            
            LocusClaimRegistrationInputModel locusInputModel = Mapping(caseNo);
            EWIRequest reqModel = new EWIRequest()
            {
                username = "ClaimMotor",
                password = "1234",
                gid = "ClaimMotor",
                token = "",
                content = locusInputModel
            };
            string jsonReqModel = JsonConvert.SerializeObject(reqModel , Formatting.Indented , new EWIDatetimeConverter() );
            
            HttpClient client = new HttpClient();

            // URL
            client.BaseAddress = new Uri("http://192.168.3.194/ServiceProxy/ClaimMotor/jsonproxy/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Accept-Encoding", "utf-8");

            // + ENDPOINT
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "LOCUS_ClaimRegistration");
            request.Content = new StringContent(jsonReqModel, System.Text.Encoding.UTF8, "application/json");
            // request.Content = new StringContent(Dummy_Input(), System.Text.Encoding.UTF8, "application/json");

            // เช็ค check reponse 
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();

            EWIResponse ewiRes = response.Content.ReadAsAsync<EWIResponse>().Result;
            EWIResponseContent locusOutput = ewiRes.content;
            return locusOutput;

            //string sResContent = response.Content.ReadAsStringAsync().Result;
            //return new LocusClaimRegistrationOutputModel();

        }
        */
        /*
         * Method นี้คือการ Return ผลลัพธ์ครับ (claimID) 
         * การทำงานคือจะต้องสร้าง object สำหรับเก็บค่า output มาก่อนแล้ว map value จาก json response เข้ากับ output ครับ
         * หลังจากนั้นก็ return ค่า
         **/

        /*
        private ClaimRegistrationOutputModel HandleMessage(string valueText, ClaimRegistrationInputModel content)
        {
            //TODO: Do what you want
            var output = new ClaimRegistrationOutputModel();
            EWIResponseContent locusClaimRegOutput = new EWIResponseContent();
            _log.Info("HandleMessage");
            try
            {
                
                //TODO: Do something
                locusClaimRegOutput = RegisterClaimOnLocus(content.caseNo);
                LocusClaimRegistrationDataOutputModel regOutputData = new LocusClaimRegistrationDataOutputModel(locusClaimRegOutput.data);
                output.claimID = regOutputData.claimId;
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

                output.claimID = errorMessage;
            }

            return output;
        }

        // อันนี้เป็น Hard Json ไว้เทสตอนยิงครับ เป็น Json ที่ยิงได้ code 200 บน service proxy
        private string Dummy_Input()
        {
            string a = "{\"username\":\"ClaimMotor\",\"password\":\"1234\",\"token\":\"\",\"content\":{\"claimHeader\":{\"ticketNumber\":\"CAS201702-00003\",\"claimNotiNo\":\"201702-00005\",\"claimNotiRefer\":\"\",\"policyNo\":\"V0284555\",\"fleetCarNo\":1,\"policySeqNo\":9,\"renewalNo\":7,\"barcode\":\"2080004701062\",\"insureCardNo\":\"\",\"policyIssueDate\":\"20151102\",\"policyEffectiveDate\":\"20151115\",\"policyExpiryDate\":\"20170210\",\"policyProductTypeCode\":\"TP\",\"policyProductTypeName\":\"ประเภท 3\",\"policyGarageFlag\":\"N\",\"policyPaymentStatus\":\"N\",\"policyCarRegisterNo\":\"กร1170\",\"policyCarRegisterProv\": \"\",\"carChassisNo\":\"10801620088706\",\"carVehicleType\":\"110\",\"carVehicleModel\":\"MERCEDES BENZ S280\",\"carVehicleYear\":\"1972\",\"carVehicleBody\":\"SEDAN\",\"carVehicleSize\":\"5/2954.00\",\"policyDeduct\":0,\"vipCaseFlag\":\"N\",\"highLossCaseFlag\":\"N\",\"legalCaseFlag\":\"N\",\"claimNotiRemark\":\"-\",\"claimType\":\"I\"},\"claimInform\":{\"informerClientId\":\"11608685\",\"informerFullName\":\"ธีระเดช เลขาชินบุตร\",\"informerMobile\":\"0812345678\",\"informerPhoneNo\":\"\",\"driverClientId\":\"11608685\",\"driverFullName\":\"ธีระเดช เลขาชินบุตร\",\"driverMobile\":\"0812345678\",\"driverPhoneNo\":\"\",\"insuredClientId\":\"11608685\",\"insuredFullName\":\"\",\"insuredMobile\":\"\",\"insuredPhoneNo\":\"\",\"relationshipWithInsurer\":\"00\",\"currentCarRegisterNo\":\"กร1170\",\"currentCarRegisterProv\":\"\",\"informerOn\":\"09/02/2017 20:30:00\",\"accidentOn\":\"09/02/2017 20:00:00\",\"accidentDescCode\":\"\",\"numOfExpectInjury\":0,\"accidentPlace\":\"นวลจันทร์ เขต คลองเตย กรุงเทพมหานคร ประเทศไทย\",\"accidentLatitude\":\"13.742097\",\"accidentLongitude\":\"100.552975\",\"accidentProvn\":\"\",\"accidentDist\":\"\",\"sendOutSurveyorCode\":\"01\"},\"claimAssignSurv\":{\"surveyorCode\":\"\",\"surveyorClientNumber\":\"\",\"surveyorName\":\"สมชาย\",\"surveyorCompanyName\":\"บริษัท บริลเลี่ยน เซอร์เวย์ จำกัด (บางนา)\",\"surveyorCompanyMobile\":\"02-150-8844\",\"surveyorMobile\":\"\",\"surveyorType\":\"O\",\"reportAccidentResultDate\":null},\"claimSurvInform\":{\"accidentLegalResult\":\"\",n\"policeStation\":\"\",\"policeRecordId\":\"\",\"policeRecordDate\":null,\"policeBailFlag\": \"\",\"damageOfPolicyOwnerCar\":\"\",\"numOfTowTruck\":0,\"nameOfTowCompany\":\"\",\"detailOfTowEvent\":\"\",\"numOfAccidentInjury\":0,\"detailOfAccidentInjury\":\"\",\"numOfDeath\":0,\"detailOfDeath\":\"\",\"caseOwnerCode\":\"\",\"caseOwnerFullName\":\"\",\"accidentPartyInfo\":null}}}";
            return a;
        }
        */
    }
}
