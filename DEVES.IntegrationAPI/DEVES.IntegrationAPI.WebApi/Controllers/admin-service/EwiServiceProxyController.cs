using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.ClaimRegistration;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.Model.SMS;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Swashbuckle.Swagger.Annotations;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    [RoutePrefix("api/ewi-service-proxy")]
    public class EwiServiceProxyController : ApiController
    {
        [HttpPost]
        [Route("{endPointKey}")]
        [ResponseType(typeof(DbResult))]
        public IHttpActionResult Post(string endPointKey,[FromBody]object value)
        {
            var endpoint = AppConfig.Instance.Get("EWI_ENDPOINT_" + endPointKey);
            Console.WriteLine(endpoint);
            var client = new RESTClient(endpoint);
            var result = client.Execute(value.ToString());
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new InternalErrorException(result.Message);
            }
            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<object>(result.Content);
            return Ok(contentObj);

        }

        [HttpPost]
        [Route("CLS_InquiryCLSPersonalClient")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(EWIResCLSInquiryPersonalClient))]
       
        public IHttpActionResult CLSInquiryCLSPersonalClient([FromBody]CLSInquiryPersonalClientInputModel value)
        {
            return proxyRequest("EWI_ENDPOINT_CLSInquiryPersonalClient", value);
        }

        [HttpPost]
        [Route("CLS_InquiryCLSCorporateClient")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(EWIResCLSInquiryCorporateClient))]

        public IHttpActionResult CLS_InquiryCLSCorporateClient([FromBody]CLSInquiryCorporateClientInputModel value)
        {
            return proxyRequest("EWI_ENDPOINT_CLSInquiryCLSCorporateClient", value);
        }

        [HttpPost]
        [Route("CLS_CreatePersonalClient")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CLSCreatePersonalClientOutputModel))]

        public IHttpActionResult CLS_CreatePersonalClient([FromBody]CLSCreatePersonalClientInputModel value)
        {
            return proxyRequest("EWI_ENDPOINT_CLSCreatePersonalClient", value);
        }

        [HttpPost]
        [Route("CLS_CreateCorporateClient")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CLSCreateCorporateClientInputModel))]

        public IHttpActionResult CLS_CreateCorporateClient([FromBody]CLSCreateCorporateClientOutputModel value)
        {
            return proxyRequest("EWI_ENDPOINT_CLSCreateCorporateClient", value);
        }

        
        [HttpPost]
        [Route("LOCUS_ClaimRegistration")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ClaimRegistrationContentOutputModel))]
        public IHttpActionResult LOCUS_ClaimRegistration([FromBody]LocusClaimRegistrationInputModel value)
        {
            return proxyRequest("EWI_ENDPOINT_ClaimRegistration", value);
        }


        [HttpPost]
        [Route("COMP_InquiryClientMaster")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(COMPInquiryClientMasterOutputModel))]
        public IHttpActionResult COMP_InquiryClientMaster([FromBody]COMPInquiryClientMasterInputModel value)
        {
            return proxyRequest("EWI_ENDPOINT_COMPInquiryClientMaster", value);
        }


        [HttpPost]
        [Route("MOTOR_InquiryMasterASRH")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(InquiryMasterASRHOutputModel))]
        public IHttpActionResult MOTOR_InquiryMasterASRH([FromBody]InquiryMasterASRHDataInputModel value)
        {
            return proxyRequest("EWI_ENDPOINT_MOTORInquiryMasterASRH", value);
        }


        [HttpPost]
        [Route("MOTOR_InquiryAPARPayeeList")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(InquiryAPARPayeeOutputModel))]
        public IHttpActionResult MOTOR_InquiryAPARPayeeList([FromBody]InquiryAPARPayeeListInputModel value)
        {
            return proxyRequest("EWI_ENDPOINT_APARInquiryPayeeList", value);
        }

        [HttpPost]
        [Route("COMP_SAPInquiryVendor")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SAPInquiryVendorOutputModel))]
        public IHttpActionResult EWI_ENDPOINT_SAPInquiryVendor([FromBody]SAPInquiryVendorInputModel value)
        {
            
            return proxyRequest("EWI_ENDPOINT_SAPInquiryVendor", value);
        }


        [HttpPost]
        [Route("COMP_SAPCreateVendor")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SAPCreateVendorOutputModel))]
        public IHttpActionResult COMP_SAPCreateVendor([FromBody]SAPCreateVendorInputModel value)
        {
            return proxyRequest("EWI_ENDPOINT_SAPCreateVendor", value);
        }

        [HttpPost]
        [Route("CLIENT_CreatePersonalClientAndAdditionalInfo")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CLIENTCreatePersonalClientAndAdditionalInfoOutputModel))]
        public IHttpActionResult CLIENT_CreatePersonalClientAndAdditionalInfo([FromBody] CLIENTCreatePersonalClientAndAdditionalInfoInputModel value)
        {
            return proxyRequest("EWI_ENDPOINT_CLIENTCreatePersonalClient", value);
        }


        [HttpPost]
        [Route("CLIENT_CreateCorporateClientAndAdditionalInfo")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CLIENTCreateCorporateClientAndAdditionalInfoOutputModel))]
        public IHttpActionResult CLIENT_CreateCorporateClientAndAdditionalInfo([FromBody] CLIENTCreateCorporateClientAndAdditionalInfoInputModel value)
        {
            return proxyRequest("EWI_ENDPOINT_CLIENTCreateCorporateClient", value);
        }


        [HttpPost]
        [Route("CLIENT_UpdateCorporateClientAndAdditionalInfo")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel))]
        public IHttpActionResult CLIENT_UpdateCorporateClientAndAdditionalInfo([FromBody] CLIENTUpdateCorporateClientAndAdditionalInfoInputModel value)
        {
            return proxyRequest("EWI_ENDPOINT_CLIENTUpdateCorporateClient", value);
        }



        [HttpPost]
        [Route("COMP_SendSMS")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SendSMSOutputModel))]
        public IHttpActionResult COMP_SendSMS([FromBody]  SendSMSInputModel value)
        {
            return proxyRequest("EWI_ENDPOINT_COMPSendSMS", value);
        }

        [HttpPost]
        [Route("COMP_InquiryClientNoByCleansingID")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(COMPInquiryClientNoByCleansingIdOutputModel))]
        public IHttpActionResult COMP_InquiryClientNoByCleansingID([FromBody] COMPInquiryClientNoByCleansingIdInputModel value)
        {
            return proxyRequest("EWI_ENDPOINT_COMPInquiryClientNoByCleansingID", value);
        }


        protected IHttpActionResult proxyRequest(string endpointKey, object value)
        {
            var endpoint = AppConfig.Instance.Get(endpointKey);
            var proxy = new BaseProxyService();
            var result = proxy.SendRequest(value, endpoint);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new InternalErrorException(result.Message);
            }
            else
            {
                var jss = new JavaScriptSerializer();
                var contentObj = jss.Deserialize<object>(result.Content);
                return Ok(contentObj);
            }
        }
    }

}