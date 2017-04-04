using System;

namespace DEVES.IntegrationAPI.WebApi.Services.Core.Exceptions
{
    public class CannotConnetServiceException : ServiceFailException
    {
        public CannotConnetServiceException(ServiceFailResult r)
        {
            Console.WriteLine("new CannotConnetServiceException 1");
             // r.setHeaderProperty("code", "0");
             // r.setHeaderProperty("message", "The external remote service could not be resolved");
            //r.setHeaderProperty("description", "ไม่สามารถติดต่อไปยัง web service ภายนอกได้");

            this.Result = r;
        }

        public CannotConnetServiceException()
        {
            Console.WriteLine("new CannotConnetServiceException 2");
            var r = new ServiceFailResult();
            r.setHeaderProperty("code", "0");
            r.setHeaderProperty("message", "The external remote service could not be resolved");
            r.setHeaderProperty("description", "ไม่สามารถติดต่อไปยัง web service ภายนอกได้");

            this.Result = r;
        }
    }
}