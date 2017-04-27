namespace DEVES.IntegrationAPI.WebApi.Core.Exceptions
{
    public class NullResponseException : ServiceFailException
    {


        public NullResponseException(ServiceFailResult r)
        {
            r.setHeaderProperty("code", "500");
            r.setHeaderProperty("message", "Response was not returned or is null");
            r.setHeaderProperty("description", "Request ended with HTTP status 500.0");

            this.Result = r;
        }

        public NullResponseException()
        {
            var r = new ServiceFailResult();
            r.setHeaderProperty("code", "500");
            r.setHeaderProperty("message", "Response was not returned or is null");
            r.setHeaderProperty("description", "Request ended with HTTP status 500.0");

            this.Result = r;
        }
    }
}