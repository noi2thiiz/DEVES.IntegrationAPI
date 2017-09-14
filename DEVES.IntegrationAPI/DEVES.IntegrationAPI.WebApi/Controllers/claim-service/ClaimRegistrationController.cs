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
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class ClaimRegistrationController : BaseApiController
    {
        /* 
         * เป็น Method ที่รับค่าที่ยิงมาจาก Postman หรือ Web Service ครับ
         **/
        public object Post([FromBody]object value)
        {

            return ProcessRequest<BuzClaimRegistrationCommand, ClaimRegistrationInputModel>(value, "LOCUS_Integration_Input_Schema.json");

        }

    }
}
