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

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class ClaimRegistrationController : ApiController
    {
        //To use with log
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ClaimRegistrationController));

        private QueryInfo q = new QueryInfo();
        private System.Data.DataTable dt = new System.Data.DataTable();
        private LocusClaimRegistrationInputModel cr = new LocusClaimRegistrationInputModel();

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
        private LocusClaimRegistrationInputModel Mapping(string caseNo)
        {

            dt = new System.Data.DataTable();
            // DECLARE @ticketNo AS NVARCHAR(20) = 'CAS201702-00003';
            // DECLARE @uniqueID AS UNIQUEIDENTIFIER = 'D246086E-C1EE-E611-80D4-0050568D1874';
            dt = q.Queryinfo_CallerId("CAS201702-00003", null);


            // claimHeader
            cr.claimHeader.ticketNo = isStringNull("ticketNunber");
            cr.claimHeader.claimNotiNo = isStringNull("claimNotiNo");
            cr.claimHeader.claimNotiRefer = isStringNull("claimNotiRefer");
            cr.claimHeader.policyNo = isStringNull("policyNo");
            cr.claimHeader.fleetCarNo = isIntNull("fleetCarNo");
            cr.claimHeader.policySeqNo = isIntNull("policySeqNo");
            cr.claimHeader.renewalNo = isIntNull("renewalNo");
            cr.claimHeader.barcode = isStringNull("barcode");
            cr.claimHeader.insureCardNo = isStringNull("insureCardNo");
            cr.claimHeader.policyIssueDate = isStringNull("policyIssueDate");
            cr.claimHeader.policyEffectiveDate = isStringNull("policyEffectiveDate");
            cr.claimHeader.policyExpiryDate = isStringNull("policyExpiryDate");
            cr.claimHeader.policyProductTypeCode = isStringNull("policyProductTypeCode");
            cr.claimHeader.policyProductTypeName = isStringNull("policyProductTypeName");
            cr.claimHeader.policyGarageFlag = isStringNull("policyGarageFlag");
            cr.claimHeader.policyPaymentStatus = isStringNull("policyPaymentStatus");
            cr.claimHeader.policyCarRegisterNo = isStringNull("policyCarRegisterNo");
            cr.claimHeader.policyCarRegisterProv = isStringNull("policyCarRegisterProv");
            cr.claimHeader.carChassisNo = isStringNull("carChassisNo");
            cr.claimHeader.carVehicleType = dt.Rows[0]["carVehicleType"].ToString();
            cr.claimHeader.carVehicleModel = dt.Rows[0]["carVehicleModel"].ToString();
            cr.claimHeader.carVehicleYear = dt.Rows[0]["carVehicleYear"].ToString();
            cr.claimHeader.carVehicleBody = dt.Rows[0]["carVehicleBody"].ToString();
            cr.claimHeader.carVehicleSize = dt.Rows[0]["carVehicleSize"].ToString();
            cr.claimHeader.policyDeduct = Convert.ToInt32(dt.Rows[0]["policyDeduct"]);
            cr.claimHeader.vipCaseFlag = dt.Rows[0]["vipCaseFlag"].ToString();
            cr.claimHeader.privilegeLevel = dt.Rows[0]["privilegeLevel"].ToString();
            cr.claimHeader.highLossCaseFlag = dt.Rows[0]["highLossCaseFlag"].ToString();
            cr.claimHeader.LegalCaseFlag = dt.Rows[0]["LegalCaseFlag"].ToString();
            cr.claimHeader.claimNotiRemark = dt.Rows[0]["claimNotiRemark"].ToString();
            cr.claimHeader.claimType = dt.Rows[0]["claimType"].ToString();

            // claimInform
            cr.claimType.informerClientId = dt.Rows[0]["informerClientId"].ToString();
            cr.claimType.informerFullName = dt.Rows[0]["informerFullName"].ToString();
            cr.claimType.informerMobile = dt.Rows[0]["informerMobile"].ToString();
            cr.claimType.informerPhoneNo = dt.Rows[0]["informerPhoneNo"].ToString();
            cr.claimType.driverClientId = dt.Rows[0]["driverClientId"].ToString();
            cr.claimType.driverFullName = dt.Rows[0]["driverFullName"].ToString();
            cr.claimType.driverMobile = dt.Rows[0]["driverMobile"].ToString();
            cr.claimType.driverPhoneNo = dt.Rows[0]["driverPhoneNo"].ToString();
            cr.claimType.insuredClientId = dt.Rows[0]["insuredClientId"].ToString();
            cr.claimType.insuredFullName = dt.Rows[0]["insuredFullName"].ToString();
            cr.claimType.insuredMobile = dt.Rows[0]["insuredMobile"].ToString();
            cr.claimType.insuredPhoneNo = dt.Rows[0]["insuredPhoneNo"].ToString();
            cr.claimType.relationshipWithInsurer = dt.Rows[0]["relationshipWithInsurer"].ToString();
            cr.claimType.currentCarRegisterNo = dt.Rows[0]["currentCarRegisterNo"].ToString();
            cr.claimType.currentCarRegisterProv = dt.Rows[0]["currentCarRegisterProv"].ToString();
            cr.claimType.informOn = dt.Rows[0]["informOn"].ToString();
            cr.claimType.accidentOn = dt.Rows[0]["accidentOn"].ToString();
            cr.claimType.accidentDescCode = dt.Rows[0]["accidentDescCode"].ToString();
            cr.claimType.numOfExpectInjury = Convert.ToInt32(dt.Rows[0]["numOfExpectInjury"]);
            cr.claimType.accidentPlace = dt.Rows[0]["accidentPlace"].ToString();
            cr.claimType.accidentLatitude = dt.Rows[0]["accidentLatitude"].ToString();
            cr.claimType.accidentLongitude = dt.Rows[0]["accidentLongitude"].ToString();
            cr.claimType.accidentProvn = dt.Rows[0]["accidentProvn"].ToString();
            cr.claimType.accidentDist = dt.Rows[0]["accidentDist"].ToString();
            cr.claimType.sendOutSurveyorCode = dt.Rows[0]["sendOutSurveyorCode"].ToString();

            // claimAssignSurv
            cr.claimAssignSurv.surveyorCode = dt.Rows[0]["surveyorCode"].ToString();
            cr.claimAssignSurv.surveyorClientNumber = dt.Rows[0]["surveyorClientNumber"].ToString();
            cr.claimAssignSurv.surveyorName = dt.Rows[0]["surveyorName"].ToString();
            cr.claimAssignSurv.surveyorCompanyName = dt.Rows[0]["surveyorCompanyName"].ToString();
            cr.claimAssignSurv.surveyorCompanyMobile = dt.Rows[0]["surveyorCompanyMobile"].ToString();
            cr.claimAssignSurv.surveyorMobile = dt.Rows[0]["surveyorMobile"].ToString();
            cr.claimAssignSurv.surveyorType = dt.Rows[0]["surveyorType"].ToString();
            cr.claimAssignSurv.reportAccidentResultDate = DateTime.ParseExact(dt.Rows[0]["reportAccidentResultDate"].ToString(), "yyyy-MM-dd HH:mm tt", null);

            // claimSurvInform
            cr.claimSurvInform.accidentLegalResult = dt.Rows[0]["accidentLegalResult"].ToString();
            cr.claimSurvInform.policeStation = dt.Rows[0]["policeStation"].ToString();
            cr.claimSurvInform.policeRecordId = dt.Rows[0]["policeRecordId"].ToString();
            cr.claimSurvInform.policeRecordDate = DateTime.ParseExact(dt.Rows[0]["policeRecordDate"].ToString(), "yyyy-MM-dd HH:mm tt", null);
            cr.claimSurvInform.policeBailFlag = dt.Rows[0]["policeBailFlag"].ToString();
            cr.claimSurvInform.demageOfPolicyOwnerCar = dt.Rows[0]["demageOfPolicyOwnerCar"].ToString();
            cr.claimSurvInform.numOfTowTruck = Convert.ToInt32(dt.Rows[0]["numOfTowTruck"]);
            cr.claimSurvInform.nameOfTowCompany = dt.Rows[0]["nameOfTowCompany"].ToString();
            cr.claimSurvInform.detailOfTowEvent = dt.Rows[0]["detailOfTowEvent"].ToString();
            cr.claimSurvInform.numOfAccidentInjury = Convert.ToInt32(dt.Rows[0]["numOfAccidentInjury"]);
            cr.claimSurvInform.detailOfAccidentInjury = dt.Rows[0]["detailOfAccidentInjury"].ToString();
            cr.claimSurvInform.numOfDeath = Convert.ToInt32(dt.Rows[0]["numOfDeath"]);
            cr.claimSurvInform.detailOfDeath = dt.Rows[0]["detailOfDeath"].ToString();
            cr.claimSurvInform.caseOwnerCode = dt.Rows[0]["caseOwnerCode"].ToString();
            cr.claimSurvInform.caseOwnerFullName = dt.Rows[0]["caseOwnerFullName"].ToString();
            // LocusAccidentpartyinfo[] accidentPartyInfo 
            cr.claimSurvInform.accidentPartyInfo[0] = null;
            cr.claimSurvInform.accidentPartyInfo[1] = null;
            cr.claimSurvInform.accidentPartyInfo[2] = null;
            cr.claimSurvInform.accidentPartyInfo[3] = null;
            cr.claimSurvInform.accidentPartyInfo[4] = null;
            cr.claimSurvInform.accidentPartyInfo[5] = null;
            cr.claimSurvInform.accidentPartyInfo[6] = null;
            cr.claimSurvInform.accidentPartyInfo[7] = null;
            cr.claimSurvInform.accidentPartyInfo[8] = null;
            cr.claimSurvInform.accidentPartyInfo[9] = null;

            return cr;
        }
        public object Post([FromBody]object value)
        {
            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: Get", CommonHelper.GetIpAddress());

            var output = new ClaimRegistrationOutputModel();
            if (value==null)
            {
                return Request.CreateResponse<ClaimRegistrationOutputModel>(output);
            }
            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<ClaimRegistrationInputModel>(contentText);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/InternalClaimRegistration_Input_Schema.json");

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
        }

        private LocusClaimRegistrationOutputModel RegisterClaimOnLocus(string caseNo)
        {

            ///PFC:: Change fixed values to be the configurable values
            ///

            LocusClaimRegistrationInputModel locusInputModel = Mapping(caseNo);
            EWIRequest reqModel = new EWIRequest()
            {
                username = "ClaimMotor",
                password = "1234",
                token = "",
                content = locusInputModel
            };

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://192.168.3.194/ServiceProxy/ClaimMotor/jsonproxy/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.PostAsJsonAsync("LOCUS_ClaimRegistration", reqModel).Result;
            response.EnsureSuccessStatusCode();
            LocusClaimRegistrationOutputModel locusOutput = response.Content.ReadAsAsync<LocusClaimRegistrationOutputModel>().Result;

            return locusOutput;

        }

        private ClaimRegistrationOutputModel HandleMessage(string valueText, ClaimRegistrationInputModel content)
        {
            //TODO: Do what you want
            var output = new ClaimRegistrationOutputModel();
            LocusClaimRegistrationOutputModel locusClaimRegOutput = new LocusClaimRegistrationOutputModel();
            _log.Info("HandleMessage");
            try
            {
                //TODO: Do something
                locusClaimRegOutput = RegisterClaimOnLocus(content.caseNo);
                output.claimID = locusClaimRegOutput.data.claimId;
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
    }
}
