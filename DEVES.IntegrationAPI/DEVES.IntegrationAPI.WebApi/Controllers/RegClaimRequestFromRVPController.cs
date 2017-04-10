using System;
using System.Web.Http;
using DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP;
using DEVES.IntegrationAPI.WebApi.Core.Controllers;
using DEVES.IntegrationAPI.WebApi.Logic.RVP;
using DEVES.IntegrationAPI.Core.TechnicalService.Exceptions;

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
            catch (BuzInValidBusinessConditionException e)
            {
                return InternalServerError(e);
            }

            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}