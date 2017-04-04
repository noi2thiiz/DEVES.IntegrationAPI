namespace DEVES.IntegrationAPI.WebApi.Services.Core.Exceptions
{
    public class NotFoundResponseException : ServiceFailException
    {
        public NotFoundResponseException(ServiceFailResult r)
        {
            r.setHeaderProperty("code", "404");
            r.setHeaderProperty("message", "Not Found");
            r.setHeaderProperty("description", "The remote server returned an error: (404) Not Found.");

            this.Result = r;
        }

        public NotFoundResponseException()
        {
            var r = new ServiceFailResult();
            r.setHeaderProperty("code", "404");
            r.setHeaderProperty("message", "Not Found");
            r.setHeaderProperty("description", "The remote server returned an error: (404) Not Found.");

            this.Result = r;
        }
    }
}