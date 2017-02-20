using System.Web.Http;
using System.Web.Http.Description;
using DEVES.IntegrationAPI.WebApi.Models.WebDialer;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    [RoutePrefix("api/WebDialer")]

    public class WebDialerController : ApiController
    {



        // POST api/WebDialer/makeCall>
        /// <summary>
        /// {
	    ///       "UserName":"agenttest1",
	    ///      "DestinationNumber":"90865557013"
        ///}
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("MakeCall")]
        [ResponseType(typeof(MakeCallResultModel))]
        public IHttpActionResult Post([FromBody]MakeCallRequestModel req)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string endPoint = "https://10.10.0.22:8443";
                IServiceRequestAble<MakeCallRequestModel> requester = new Models.WebDialer.WebDialerRequester(endPoint);
                IServiceResult result = requester.Execute(req);

                return Ok(result);
            }
            catch (System.Exception e)
            {
                return InternalServerError();
            }
           

        }

    } 
   
}