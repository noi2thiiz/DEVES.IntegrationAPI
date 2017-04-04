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
using System.Collections;

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

        protected IHttpActionResult ProcessRequest<COMMAND,OUTPUT>(object model)
            where COMMAND : new() 
            where OUTPUT : class

        {
            return _processRequest<COMMAND, OUTPUT>("Execute", model);
        }

        protected IHttpActionResult _processRequest<T_COMMAND, T_OUTPUT>(string methodName, object model)
            where T_COMMAND : new()
            where T_OUTPUT : class

        {
            IServiceResult output;
            try
            {
                Type typeOutputContent = typeof(T_OUTPUT);
                object outputContent = Activator.CreateInstance(typeOutputContent);
              
               
               // Type typeOutputContentData = dataProp.PropertyType;
                if (typeof(IEnumerable).IsAssignableFrom(typeOutputContent))
                {
                    output = new ServiceResult<dynamic>();
                   
                }
                else
                {
                    output = new ServiceResultSingleData<dynamic>();

                }

                output.setHeaderProperty("transactionDateTime", GetCurrentDateTime());
                output.setHeaderProperty("transactionId", GetTransactionId());




                object[] methodArgs = new object[1];
                methodArgs[0] = model;


                _validateModel(model);


                var manager = new T_COMMAND();
                Type type = typeof(T_COMMAND);
                object instance = Activator.CreateInstance(type);
                MethodInfo method = type.GetMethod(methodName);
                var outputContentInstance = (T_OUTPUT) method.Invoke(instance, methodArgs);
                PropertyInfo dataProp = typeOutputContent.GetProperty("data");
                var outputContentData = dataProp.GetValue(outputContentInstance);
               
                output.AddBodyData(outputContentData);
                //  IServiceResult result = (IServiceResult)method.Invoke(manager, new [model]);
                return SuccessResponse(output);
            }

            catch (RemoteServiceErrorException e)
            {
                //Console.WriteLine("RemoteServiceErrorException");
                return CreatedResponse(e.Result);
            }

            catch (ServiceFailException e)
            {
                //Console.WriteLine("ServiceFailException");
                return CreatedResponse(e.Result);
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
                return CreatedResponse(r);
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

                    return CreatedResponse(ex.Result);
                }
                else if (e.InnerException != null && e.InnerException.GetType().ToString() ==
                         "DEVES.IntegrationAPI.WebApi.Services.Core.Exceptions.RemoteServiceErrorException")
                {
                    ServiceFailException ex = (ServiceFailException) e.InnerException;
                    //Console.WriteLine(ex.StackTrace);

                    return CreatedResponse(ex.Result);
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
                r.setHeaderProperty("transactionDateTime", GetCurrentDateTime());
                r.setHeaderProperty("transactionId", GetTransactionId());


                var s = new StackTrace(e);
                var thisasm = Assembly.GetExecutingAssembly();
                var methodname = s.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;

                // r.setHeaderProperty("methodname",methodname);
                //r.stackTrace = e;

                return InternalServerError(e);
                //  _InternalServerError(r);
            }

            ServiceFailResult x = new ServiceFailResult();
            return CreatedResponse(x);
        }

        protected IHttpActionResult SuccessResponse(dynamic output)
        {

            output.setHeaderProperty("code","200");
            output.setHeaderProperty("message","Success");
            output.setHeaderProperty("description","The server successfully processed the request");
            output.setHeaderProperty("transactionDateTime", GetCurrentDateTime());
            output.setHeaderProperty("transactionId", GetTransactionId());


            return new OkResult<Object>(output, this);
        }

        protected IHttpActionResult CreatedResponse(dynamic output)
        {

          
            output.setHeaderProperty("transactionDateTime", GetCurrentDateTime());
            output.setHeaderProperty("transactionId", GetTransactionId());


            return new OkResult<Object>(output, this);
        }

    }
}