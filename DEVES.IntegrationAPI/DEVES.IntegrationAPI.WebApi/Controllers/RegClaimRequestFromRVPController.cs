using System;
using System.Web.Http;
using DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Core.Controllers;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RegClaimRequestFromRVPController:BaseApiController
    {

        [HttpPost]
        public IHttpActionResult Post([FromBody] CRMregClaimRequestFromRVPInputModel model)
        {
            try
            {
                return ProcessRequest<BuzRegClaimRequestFromRVPCommand, CrmregClaimRequestFromRVPContentOutputModel>(model);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}