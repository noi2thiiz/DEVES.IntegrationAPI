using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.WebApi.Core.Exceptions;
using System.Collections;
using DEVES.IntegrationAPI.Core.TechnicalService.Exceptions;
using DEVES.IntegrationAPI.WebApi.Core.ExtensionMethods;

namespace DEVES.IntegrationAPI.WebApi.Core.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        public new HttpRequestMessage Request => (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];

        protected string GetCurrentDateTime()
        {
            var usaCulture = new CultureInfo("en-US");
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", usaCulture);
        }


        protected string GetTransactionId()
        {
            return Request.Properties["TransactionID"].ToString();
        }

        protected void _validateModel(object model)
        {
            if (model == null)
            {
              
                var r = new ServiceFailResult();
                r.AddRequest(null);
                var error = new ErrorData();

                // build fieldErrors
                var fieldErrors = FieldErrorsParser.FieldErrorsParser.Parse(ModelState);
                error.AddFieldErrors(fieldErrors);


                error.type = "ParseError";
                error.message = "An error occured with your request. Reset your inputs and try again";
                r.AddBodyData(error);


                throw new NotEmptyModelException(r);
            }

            if (ModelState.IsValid) return;
            {
                //Console.WriteLine("==================! ModelState.IsValid===================");
                var r = new ServiceBadRequestResult();
                r.AddRequest(model);
                var error = new ErrorData();

                // build fieldErrors
                var fieldErrors = FieldErrorsParser.ValidationErrorParser.Parse(ModelState);
                error.AddFieldErrors(fieldErrors);


                error.type = "ValidationError";
                error.message = "An error occured with your request. Reset your inputs and try again";
                r.AddBodyData(error);


                throw new InvalidModelException(r);
            }
        }

        protected IHttpActionResult ProcessRequest<TCommand,TOutput>(object model)
            where TCommand : new() 
            where TOutput : class

        {
            return _processRequest<TCommand, TOutput>("Execute", model);
        }

        protected IHttpActionResult _processRequest<TCommand, TOutput>(string methodName, object model)
            where TCommand : new()
            where TOutput : class

        {
            try
            {
                var typeOutputContent = typeof(TOutput);
                Activator.CreateInstance(typeOutputContent);


                // Type typeOutputContentData = dataProp.PropertyType;
                IServiceResult output;
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




                var methodArgs = new object[1];
                methodArgs[0] = model;
                _validateModel(model);


       
                var type = typeof(TCommand);
                object instance = Activator.CreateInstance(type);
                var method = type.GetMethod(methodName);
                Console.WriteLine("======Invoke========");
                var outputContentInstance = (TOutput) method.Invoke(instance, methodArgs);

                Console.WriteLine("======Get Data========");
                var dataProp = typeOutputContent.GetProperty("Data");
                if (dataProp == null) return SuccessResponse(output);
                var outputContentData = dataProp.GetValue(outputContentInstance);

                output.AddBodyData(outputContentData);
                //  IServiceResult result = (IServiceResult)method.Invoke(manager, new [model]);
                Console.WriteLine("======SuccessResponse========");
                return SuccessResponse(output);
            }
            catch (DEVES.IntegrationAPI.Core.TechnicalService.Exceptions.BuzInValidBusinessConditionException e)
            {
                Console.WriteLine("BuzInValidBusinessConditionException");
                var errorMessage = e.ErrorMessage;
                //Console.WriteLine(response.StatusCode + " " + response.StatusDescription);

                var r = new ServiceFailResult();
                r.setHeaderProperty("code", errorMessage.code);
                r.setHeaderProperty("message", errorMessage.message);
                r.setHeaderProperty("description", errorMessage.description.ToString());

                return CreatedResponse(r);
            }
            catch (BuzInternalErrorException e)
            {
              
                var errorMessage = e.ErrorMessage;
              

                var r = new ServiceFailResult();
                r.setHeaderProperty("code", errorMessage.code);
                r.setHeaderProperty("message", errorMessage.message);
                r.setHeaderProperty("description", errorMessage.description.ToString());

                return CreatedResponse(r);
            }


            catch (RemoteServiceErrorException e)
            {
                return CreatedResponse(e.Result);
            }

            catch (ServiceFailException e)
            {

                return CreatedResponse(e.Result);
            }

            catch (WebException e)
            {
             
                var response = (HttpWebResponse) e.Response;
                //Console.WriteLine(response.StatusCode + " " + response.StatusDescription);

                var r = new ServiceFailResult();
                r.setHeaderProperty("code", response.StatusCode.ToString());
                r.setHeaderProperty("message", "Web Service error");
                r.setHeaderProperty("description", response.StatusDescription);


                return CreatedResponse(r);
            }


            catch (TargetInvocationException e)
            {
               // Console.WriteLine("catch System.Reflection.TargetInvocationException ");
               // Console.WriteLine(e.InnerException.GetType().ToString());
                if (e.InnerException != null)
                {
                    switch (e.InnerException.GetType().ToString())
                    {
                        case "DEVES.IntegrationAPI.WebApi.Services.Core.Exceptions.CannotConnetServiceException":
                            {

                                var ex = (ServiceFailException)e.InnerException;

                                return CreatedResponse(ex.Result);
                            }
                        case "DEVES.IntegrationAPI.WebApi.Core.Exceptions.RemoteServiceErrorException":
                            {
                                var ex = (ServiceFailException)e.InnerException;


                                return CreatedResponse(ex.Result);
                            }
                        case "DEVES.IntegrationAPI.Core.TechnicalService.Exceptions.BuzInternalErrorException":
                            {
                           
                                var ex = (BuzInternalErrorException)e.InnerException;
                                var errorMessage = ex.ErrorMessage;
                                //Console.WriteLine(response.StatusCode + " " + response.StatusDescription);

                                var r = new ServiceFailResult();
                                r.setHeaderProperty("code", errorMessage.code);
                                r.setHeaderProperty("message", errorMessage.message);
                                r.setHeaderProperty("description", errorMessage.description);


                                return CreatedResponse(r);
                            }
                        case "DEVES.IntegrationAPI.Core.TechnicalService.Exceptions.BuzInValidBusinessConditionException":
                            {
  
                   
                                var ex = (BuzInValidBusinessConditionException)e.InnerException;
                                var errorMessage = ex.ErrorMessage;
                                //Console.WriteLine(response.StatusCode + " " + response.StatusDescription);

                                var r = new ServiceFailResult();
                                r.setHeaderProperty("code", errorMessage.code);
                                r.setHeaderProperty("message", errorMessage.message);
                                r.setHeaderProperty("description", errorMessage.description);


                                return CreatedResponse(r);
                            }

                        default:
                            break;
                    }
                }
            }

            catch (Exception e)
            {
   
                var r = new ServiceResultHeaderOnly();

                //Console.WriteLine(r);
                r.setHeaderProperty("code", "500");
                r.setHeaderProperty("message", "Internal error occurred");
                r.setHeaderProperty("transactionDateTime", GetCurrentDateTime());
                r.setHeaderProperty("transactionId", GetTransactionId());


              //  var s = new StackTrace(e);
              //  var thisasm = Assembly.GetExecutingAssembly();
              //  var methodname = s.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;

                // r.setHeaderProperty("methodname",methodname);
                //r.stackTrace = e;

                return InternalServerError(e);
                //  _InternalServerError(r);
            }
            Console.WriteLine("Cannot Catch");
            return CreatedResponse(new ServiceFailResult());
        }

        protected IHttpActionResult SuccessResponse(dynamic output)
        {

            output.setHeaderProperty("code","200");
            output.setHeaderProperty("message","Success");
            output.setHeaderProperty("description", "SuccessResponse:The server successfully processed the request");
            output.setHeaderProperty("transactionDateTime", GetCurrentDateTime());
            output.setHeaderProperty("transactionId", GetTransactionId());


            return new OkResult<Object>(output, this);
        }

        protected IHttpActionResult CreatedResponse(IServiceResult output)
        {
            Console.WriteLine(output.ToJSON());
         
            if (output.code == null)
            {
                output.setHeaderProperty("code", "500");
                output.setHeaderProperty("message", "Internal error occurred");
                output.setHeaderProperty("description", output.message +","+ output.description);
            }
            
           
           
            output.setHeaderProperty("transactionDateTime", GetCurrentDateTime());
            output.setHeaderProperty("transactionId", GetTransactionId());


            return new OkResult<object>(output, this);
        }

    }
}