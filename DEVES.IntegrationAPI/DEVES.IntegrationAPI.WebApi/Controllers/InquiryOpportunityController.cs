using DEVES.IntegrationAPI.Model.InquiryOpportunity;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class InquiryOpportunityController : BaseApiController
    {

        public object Post([FromBody]object value)
        {
            return ProcessRequest<buzInquiryOpportunity, InquiryOpportunityInputModel>(value, "InquiryOpportunity_Input_Schema.json");
        }


    }
}