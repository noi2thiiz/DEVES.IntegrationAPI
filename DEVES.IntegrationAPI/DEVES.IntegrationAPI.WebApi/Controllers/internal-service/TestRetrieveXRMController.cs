using DEVES.IntegrationAPI.Model.TestRetrieveXRM;
using DEVES.IntegrationAPI.WebApi.ConnectCRM;
using Microsoft.Xrm.Sdk.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers.internal_service
{
    public class TestRetrieveXRMController : ApiController
    {
        public object Post([FromBody]object value)
        {
            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<TestRetrieveXRMInputModel>(contentText);

            // Preparation Linq query to CRM
            ServiceContext svcContext;
            var _serviceProxy = GetOrganizationServiceProxy(out svcContext);

            var queryCase = from c in svcContext.IncidentSet
                            where c.pfc_claim_noti_number == contentModel.claimNotiNo
                            select c;

            TestRetrieveXRMOutputModel output = new TestRetrieveXRMOutputModel();

            if (queryCase.FirstOrDefault<Incident>() == null)
            {
                output.ticketNo = null;
                output.description = null;
                output.message = "claimNotiNo is not found";
            }
            else
            {
                Incident incident = queryCase.FirstOrDefault<Incident>();

                output.ticketNo = incident.TicketNumber;
                output.description = incident.pfc_accident_desc;
                output.message = "claimNotiNo is found";
            }
            
            return Request.CreateResponse<TestRetrieveXRMOutputModel>(output);
        }

        private object GetOrganizationServiceProxy(out ServiceContext svcContext)
        {
            OrganizationServiceProxy _serviceProxy = ConnectionThreadSafe.GetOrganizationProxy();
            svcContext = new ServiceContext(_serviceProxy);
            _serviceProxy.EnableProxyTypes();

            return _serviceProxy;
        }
    }
}