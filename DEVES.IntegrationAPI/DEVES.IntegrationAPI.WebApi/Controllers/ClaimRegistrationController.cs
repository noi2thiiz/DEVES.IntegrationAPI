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

        private LocusClaimRegistrationInputModel Mapping(string caseNo)
        {
            /*
            dt = new System.Data.DataTable();
            // DECLARE @ticketNo AS NVARCHAR(20) = 'CAS201702-00003';
            // DECLARE @uniqueID AS UNIQUEIDENTIFIER = 'D246086E-C1EE-E611-80D4-0050568D1874';
            dt = q.Queryinfo_CallerId("CAS201702-00003", null);


            // claimHeader
            cr.claimHeader.ticketNumber = dt.Rows[0]["ticketNunber"].ToString();
            cr.claimHeader.claimNotiNo = dt.Rows[0]["claimNotiNo"].ToString();
            cr.claimHeader.claimNotiRefer = dt.Rows[0]["claimNotiRefer"].ToString();
            cr.claimHeader.policyNo = dt.Rows[0]["policyNo"].ToString();
            cr.claimHeader.fleetCarNo = Convert.ToInt32(dt.Rows[0]["fleetCarNo"]);
            if (dt.Rows[0]["policySeqNo"] == null)
            {
                cr.claimHeader.policySeqNo = null;
            } 
            else
            {
                cr.claimHeader.policySeqNo = Convert.ToInt32(dt.Rows[0]["policySeqNo"]);
            }
            if (dt.Rows[0]["renewalNo"] == null)
            {
                cr.claimHeader.renewalNo = null;
            }
            else
            {
                cr.claimHeader.renewalNo = Convert.ToInt32(dt.Rows[0]["renewalNo"]);
            }
            if (dt.Rows[0]["barcode"] == null)
            {
                cr.claimHeader.barcode = null;
            }
            else
            {
                cr.claimHeader.barcode = dt.Rows[0]["barcode"].ToString();
            }
            if (dt.Rows[0]["insureCardNo"] == null)
            {
                cr.claimHeader.insureCardNo = null;
            }
            else
            {
                cr.claimHeader.insureCardNo = dt.Rows[0]["insureCardNo"].ToString(); ;
            }
            cr.claimHeader.policyIssueDate = dt.Rows[0]["policyIssueDate"].ToString();
            cr.claimHeader.policyEffectiveDate = dt.Rows[0]["policyEffectiveDate"].ToString();
            cr.claimHeader.policyExpiryDate = dt.Rows[0]["policyExpiryDate"].ToString();
            cr.claimHeader.policyProductTypeCode = dt.Rows[0]["policyProductTypeCode"].ToString();
            cr.claimHeader.policyProductTypeName = dt.Rows[0]["policyProductTypeName"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyPaymentStatus = dt.Rows[0]["policyPaymentStatus"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            cr.claimHeader.policyGarageFlag = dt.Rows[0]["policyGarageFlag"].ToString();
            */

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
