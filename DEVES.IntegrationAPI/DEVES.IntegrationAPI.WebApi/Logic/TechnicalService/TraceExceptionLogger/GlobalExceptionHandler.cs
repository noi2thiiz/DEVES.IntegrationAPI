using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model;

using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionHandler _innerHandler;

        public GlobalExceptionHandler(IExceptionHandler innerHandler)
        {
            Console.WriteLine("GlobalExceptionHandler");
            if (innerHandler == null)
                throw new ArgumentNullException(nameof(innerHandler));

            _innerHandler = innerHandler;
        }

        public IExceptionHandler InnerHandler
        {
            get { return _innerHandler; }
        }

        public System.Threading.Tasks.Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            Handle(context);
            var regFail = new OutputModelFail
            {
                code = AppConst.CODE_FAILED,
                message = "An error occurred",
                description = context.Exception.Message
               
            };
            var m = new  JsonMediaTypeFormatter();
            var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new ObjectContent(typeof(OutputModelFail), regFail, m, "application/json"),
                ReasonPhrase = "An error occurred",
                StatusCode = HttpStatusCode.OK,
                
            };
            throw new HttpResponseException(resp);

            Console.WriteLine("GlobalExceptionHandler:HandleAsync");
            return  _innerHandler.HandleAsync(context, cancellationToken);
        }

        public void Handle(ExceptionHandlerContext context)
        {
            var regFail = new OutputModelFail
            {
                code = AppConst.CODE_FAILED,
                message = "An error occurred",
                description = context.Exception.Message,
                stackTrace = context.Exception.StackTrace,
            };
            context.Request.CreateResponse(regFail);
            context.Result = new OkResult(context.Request);
        }
    }
  
}