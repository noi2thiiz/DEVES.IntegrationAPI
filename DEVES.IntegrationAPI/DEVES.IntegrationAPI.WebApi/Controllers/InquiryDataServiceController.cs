using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter;
using Swashbuckle.Swagger.Annotations;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    [RoutePrefix("api/InquiryDataService")]
    public class InquiryDataServiceController : ApiController
    {
        [HttpPost]
        [Route("PolicyMotorList")]
       
        public IHttpActionResult PolicyMotorList([FromUri]InquiryPolicyMotorListInputModel filter)
        {
            var connectionString = AppConfig.Instance.GetCRMDBConfigurationString();
            IDataReader reader = new RestDataReader();
            var req = new DbRequest();
            req.StoreName = "sp_API_InquiryPolicyMotorList";
            req.AddParam("policyNo", filter?.policyNo??"");
            req.AddParam("chassisNo", filter?.chassisNo ?? "");
            req.AddParam("carRegisNo", filter?.carRegisNo ?? "");
            req.AddParam("carRegisProve", filter?.carRegisProve ?? "");
            switch (filter.polisyStatus)
            {
                case PolisyStatus.Active: req.AddParam("policyStatus", "0"); break;
                case PolisyStatus.InActive: req.AddParam("policyStatus", "1"); break;
            }
            
            var result = reader.Execute(req);
            return Ok(result);
        }

        public class InquiryPolicyMotorListInputModel
        {
            public string policyNo { get; set; }
            public string chassisNo { get; set; }
            public string carRegisNo { get; set; }
            public string carRegisProve { get; set; }
            public PolisyStatus polisyStatus { get; set; }


        }
        public enum PolisyStatus 
        {
            Active,InActive,All
        }
    }
}