using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter;
using DEVES.IntegrationAPI.WebApi.Templates;
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
            Console.WriteLine(filter);
            var connectionString = AppConfig.Instance.GetCRMDBConfigurationString();
            IDataReader reader = new RestDataReader();
            var req = new DbRequest();
            req.StoreName = "sp_API_InquiryPolicyMotorListForRVP";
            req.AddParam("policyNo", filter?.policyNo??"");
            req.AddParam("chassisNo", filter?.chassisNo ?? "");
            req.AddParam("carRegisNo", filter?.carRegisNo ?? "");
            req.AddParam("carRegisProve", filter?.carRegisProve ?? "");
           // req.AddParam("renewalNo", filter?.carRegisProve ?? "0");
            if (filter?.accidentOn != null)
            {  //2016-01-04 00:00:00
                req.AddParam("accidentOn", filter?.accidentOn.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en-US")));
            }
            
            Console.WriteLine(req.ToJson());

            var result = reader.Execute(req);
            return Ok(result);
        }

        public class InquiryPolicyMotorListInputModel
        {
            public string policyNo { get; set; }
            public string renewalNo { get; set; }
            public string fleetCarNo { get; set; }
            
            public string chassisNo { get; set; }
            public string carRegisNo { get; set; }
            public string carRegisProve { get; set; }
            public DateTime accidentOn { get; set; }

            

        }
        public enum PolisyStatus 
        {
            Active,InActive,All
        }
    }
}