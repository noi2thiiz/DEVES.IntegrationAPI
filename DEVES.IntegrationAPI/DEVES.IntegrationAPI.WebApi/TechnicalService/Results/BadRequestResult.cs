using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace DEVES.IntegrationAPI.WebApi.Services
{
    public class BadRequestResult<T> : OkNegotiatedContentResult<T>
    {

        public BadRequestResult(T content, ApiController controller)
            : base(content, controller) {}
        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contentNegotiator"></param>
        /// <param name="request"></param>
        /// <param name="formatters"></param>
        public BadRequestResult(T content, IContentNegotiator contentNegotiator, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
            : base(content, contentNegotiator, request, formatters) { }



        public string ETagValue { get; set; }



        public override async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.ExecuteAsync(cancellationToken);

           // response.Headers.Add("X-offset", "2");
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }
    }
}