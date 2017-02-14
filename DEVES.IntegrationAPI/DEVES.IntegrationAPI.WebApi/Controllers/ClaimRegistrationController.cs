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
            dt = q.Queryinfo_CallerId(caseNo, null);


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
            cr.claimHeader.carVehicleType = isStringNull("carVehicleType");
            cr.claimHeader.carVehicleModel = isStringNull("carVehicleModel");
            cr.claimHeader.carVehicleYear = isStringNull("carVehicleYear");
            cr.claimHeader.carVehicleBody = isStringNull("carVehicleBody");
            cr.claimHeader.carVehicleSize = isStringNull("carVehicleSize");
            cr.claimHeader.policyDeduct = isIntNull("policyDeduct");
            cr.claimHeader.vipCaseFlag = isStringNull("vipCaseFlag");
            cr.claimHeader.privilegeLevel = isStringNull("privilegeLevel");
            cr.claimHeader.highLossCaseFlag = isStringNull("highLossCaseFlag");
            cr.claimHeader.LegalCaseFlag = isStringNull("LegalCaseFlag");
            cr.claimHeader.claimNotiRemark = isStringNull("claimNotiRemark");
            cr.claimHeader.claimType = isStringNull("claimType");
            
            // claimInform
            cr.claimType.informerClientId = isStringNull("informerClientId");
            cr.claimType.informerFullName = isStringNull("informerFullName");
            cr.claimType.informerMobile = isStringNull("informerMobile");
            cr.claimType.informerPhoneNo = isStringNull("informerPhoneNo");
            cr.claimType.driverClientId = isStringNull("driverClientId");
            cr.claimType.driverFullName = isStringNull("driverFullName");
            cr.claimType.driverMobile = isStringNull("driverMobile");
            cr.claimType.driverPhoneNo = isStringNull("driverPhoneNo");
            cr.claimType.insuredClientId = isStringNull("insuredClientId");
            cr.claimType.insuredFullName = isStringNull("insuredFullName");
            cr.claimType.insuredMobile = isStringNull("insuredMobile");
            cr.claimType.insuredPhoneNo = isStringNull("insuredPhoneNo");
            cr.claimType.relationshipWithInsurer = isStringNull("relationshipWithInsurer");
            cr.claimType.currentCarRegisterNo = isStringNull("currentCarRegisterNo");
            cr.claimType.currentCarRegisterProv = isStringNull("currentCarRegisterProv");
            cr.claimType.informOn = isStringNull("informOn");
            cr.claimType.accidentOn = isStringNull("accidentOn");
            cr.claimType.accidentDescCode = isStringNull("accidentDescCode");
            cr.claimType.numOfExpectInjury = isIntNull("numOfExpectInjury");
            cr.claimType.accidentPlace = isStringNull("accidentPlace");
            cr.claimType.accidentLatitude = isStringNull("accidentLatitude");
            cr.claimType.accidentLongitude = isStringNull("accidentLongitude");
            cr.claimType.accidentProvn = isStringNull("accidentProvn");
            cr.claimType.accidentDist = isStringNull("accidentDist");
            cr.claimType.sendOutSurveyorCode = isStringNull("sendOutSurveyorCode");

            // claimAssignSurv
            cr.claimAssignSurv.surveyorCode = isStringNull("surveyorCode");
            cr.claimAssignSurv.surveyorClientNumber = isStringNull("surveyorClientNumber");
            cr.claimAssignSurv.surveyorName = isStringNull("surveyorName");
            cr.claimAssignSurv.surveyorCompanyName = isStringNull("surveyorCompanyName");
            cr.claimAssignSurv.surveyorCompanyMobile = isStringNull("surveyorCompanyMobile");
            cr.claimAssignSurv.surveyorMobile = isStringNull("surveyorMobile");
            cr.claimAssignSurv.surveyorType = isStringNull("surveyorType");
            cr.claimAssignSurv.reportAccidentResultDate = DateTime.ParseExact(dt.Rows[0]["reportAccidentResultDate"].ToString(), "yyyy-MM-dd HH:mm tt", null);

            // claimSurvInform
            cr.claimSurvInform.accidentLegalResult = isStringNull("accidentLegalResult");
            cr.claimSurvInform.policeStation = isStringNull("policeStation");
            cr.claimSurvInform.policeRecordId = isStringNull("policeRecordId");
            cr.claimSurvInform.policeRecordDate = DateTime.ParseExact(dt.Rows[0]["policeRecordDate"].ToString(), "yyyy-MM-dd HH:mm tt", null);
            cr.claimSurvInform.policeBailFlag = isStringNull("policeBailFlag");
            cr.claimSurvInform.demageOfPolicyOwnerCar = isStringNull("demageOfPolicyOwnerCar");
            cr.claimSurvInform.numOfTowTruck = isIntNull("numOfTowTruck");
            cr.claimSurvInform.nameOfTowCompany = isStringNull("nameOfTowCompany");
            cr.claimSurvInform.detailOfTowEvent = isStringNull("detailOfTowEvent");
            cr.claimSurvInform.numOfAccidentInjury = isIntNull("numOfAccidentInjury");
            cr.claimSurvInform.detailOfAccidentInjury = isStringNull("detailOfAccidentInjury");
            cr.claimSurvInform.numOfDeath = isIntNull("numOfDeath");
            cr.claimSurvInform.detailOfDeath = isStringNull("detailOfDeath");
            cr.claimSurvInform.caseOwnerCode = isStringNull("caseOwnerCode");
            cr.claimSurvInform.caseOwnerFullName = isStringNull("caseOwnerFullName");
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
