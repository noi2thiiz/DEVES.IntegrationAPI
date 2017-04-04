using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.WebApi.Services.Core.Exceptions;
using ExtensionMethods;


namespace DEVES.IntegrationAPI.WebApi.Services.Core.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        public HttpRequestMessage Request => (HttpRequestMessage) HttpContext.Current.Items["MS_HttpRequestMessage"];

        protected string GetCurrentDateTime()
        {
            CultureInfo UsaCulture = new CultureInfo("en-US");
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture);
        }


        protected string GetTransactionId()
        {
            return Request.Properties["TransactionID"].ToString();
        }

        protected void _validateModel(object model)
        {
            if (model == null)
            {
                //Console.WriteLine("==================model== == null===================");
                ServiceFailResult r = new ServiceFailResult();
                r.AddRequest(model);
                // r.AddModelState(ModelState);
                ErrorData error = new ErrorData();

                // build fieldErrors
                var fieldErrors = FieldErrorsParser.FieldErrorsParser.Parse(ModelState);
                error.AddFieldErrors(fieldErrors);


                error.type = "ParseError";
                error.message = "An error occured with your request. Reset your inputs and try again";
                r.AddBodyData(error);


                throw new NotEmptyModelException(r);
            }

            if (!ModelState.IsValid)
            {
                //Console.WriteLine("==================! ModelState.IsValid===================");
                ServiceBadRequestResult r = new ServiceBadRequestResult();
                r.AddRequest(model);
                //r.AddModelState(ModelState);
                ErrorData error = new ErrorData();

                // build fieldErrors
                var fieldErrors = FieldErrorsParser.ValidationErrorParser.Parse(ModelState);
                error.AddFieldErrors(fieldErrors);


                error.type = "ValidationError";
                error.message = "An error occured with your request. Reset your inputs and try again";
                r.AddBodyData(error);


                throw new InvalidModelException(r);
            }
        }

        protected IHttpActionResult _processRequest<T>(object model)
            where T : new()

        {
            return _processRequest<T>("Excecute", model);
        }

        protected IHttpActionResult _processRequest<T>(string methodName, object model)
            where T : new()

        {
            try
            {
                object[] methodArgs = new object[1];
                methodArgs[0] = model;


                _validateModel(model);


                var manager = new T();

                // MethodInfo method = typeof(T).GetMethod(methodName);
                Type type = typeof(T);
                object instance = Activator.CreateInstance(type);
                MethodInfo method = type.GetMethod(methodName);
                IServiceResult result = (IServiceResult) method.Invoke(instance, methodArgs);


                //  IServiceResult result = (IServiceResult)method.Invoke(manager, new [model]);
                return _SuccessResponse(result);
            }

            catch (DEVES.IntegrationAPI.WebApi.Services.Core.Exceptions.RemoteServiceErrorException e)
            {
                //Console.WriteLine("RemoteServiceErrorException");
                return _RemoteFailResponse(e.Result);
            }

            catch (ServiceFailException e)
            {
                //Console.WriteLine("ServiceFailException");
                return _FailResponse(e.Result);
            }

            catch (System.Net.WebException e)
            {
                //Console.WriteLine("WebException");
                HttpWebResponse response = (HttpWebResponse) e.Response;
                //Console.WriteLine(response.StatusCode + " " + response.StatusDescription);

                var r = new ServiceFailResult();
                r.setHeaderProperty("code", response.StatusCode.ToString());
                r.setHeaderProperty("message", "Web Service error");
                r.setHeaderProperty("description", response.StatusDescription);
                return _WebServiceErrorResponse(r);
            }

            catch (System.Reflection.TargetInvocationException e)
            {
                //Console.WriteLine("catch System.Reflection.TargetInvocationException ");
                //Console.WriteLine(e.StackTrace);
                if (e.InnerException != null && e.InnerException.GetType().ToString() ==
                    "DEVES.IntegrationAPI.WebApi.Services.Core.Exceptions.CannotConnetServiceException")
                {
                    ServiceFailException ex = (ServiceFailException) e.InnerException;
                    //Console.WriteLine(ex.StackTrace);

                    return _RemoteFailResponse(ex.Result);
                }
                else if (e.InnerException != null && e.InnerException.GetType().ToString() ==
                         "DEVES.IntegrationAPI.WebApi.Services.Core.Exceptions.RemoteServiceErrorException")
                {
                    ServiceFailException ex = (ServiceFailException) e.InnerException;
                    //Console.WriteLine(ex.StackTrace);

                    return _RemoteFailResponse(ex.Result);
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(" catch (Exception e)");

               // Console.WriteLine(e);


                ServiceResultHeaderOnly r = new ServiceResultHeaderOnly();

                //Console.WriteLine(r);
                r.setHeaderProperty("code", "500");
                r.setHeaderProperty("message", "Internal error occurred");


                var s = new StackTrace(e);
                var thisasm = Assembly.GetExecutingAssembly();
                var methodname = s.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;

                // r.setHeaderProperty("methodname",methodname);
                //r.stackTrace = e;

                return InternalServerError(e);
                //  _InternalServerError(r);
            }

            ServiceFailResult x = new ServiceFailResult();
            return _InternalServerError(x);
        }

        protected IHttpActionResult _SuccessResponse(IServiceResult model)
        {

            // model.setHeaderProperty("code","200");
            // model.setHeaderProperty("message","Success");
            //model.setHeaderProperty("description","The server successfully processed the request");
            //model.setHeaderProperty("transactionDateTime",_getCurrentDateTime());
            // model.setHeaderProperty("transactionId",_getTransactionId());

            return new OkResult<Object>(model, this);
        }

        protected IHttpActionResult _CreatedResponse(IServiceResult model)
        {

            // model.setHeaderProperty("code","200");
            // model.setHeaderProperty("message","Success");
            // model.setHeaderProperty("description","Created");
            // model.setHeaderProperty("transactionDateTime",_getCurrentDateTime());
            //  model.setHeaderProperty("transactionId",_getTransactionId());

            return new OkResult<Object>(model, this);
        }

        protected IHttpActionResult _FailResponse(IServiceResult model)
        {
            // model.setHeaderProperty("transactionDateTime",_getCurrentDateTime());
            // model.setHeaderProperty("transactionId",_getTransactionId());
            return new BadRequestResult<Object>(model, this);
        }

        protected IHttpActionResult _WebServiceErrorResponse(IServiceResult model)
        {
            // model.setHeaderProperty("transactionDateTime",_getCurrentDateTime());
            // model.setHeaderProperty("transactionId",_getTransactionId());
            return new ErrorResult<Object>(model, this);
        }

        protected IHttpActionResult _InternalServerError(IServiceResult model)
        {
            // model.setHeaderProperty("transactionDateTime",_getCurrentDateTime());
            // model.setHeaderProperty("transactionId",_getTransactionId());
            return new ErrorResult<Object>(model, this);
        }

        protected IHttpActionResult _RemoteFailResponse(IServiceResult model)
        {
            // model.setHeaderProperty("transactionDateTime",_getCurrentDateTime());
            // model.setHeaderProperty("transactionId",_getTransactionId());


            return new ErrorResult<Object>(model, this);
        }
    }
}