namespace DEVES.IntegrationAPI.WebApi.Core.Exceptions
{
    public class NotEmptyModelException:ServiceFailException
    {


        public NotEmptyModelException(ServiceFailResult r)
        {
            r.setHeaderProperty("code","400");
            r.setHeaderProperty("message","Bad Request");
            r.setHeaderProperty("description","The server cannot or will not process the request due to an apparent client error (e.g., malformed request syntax, too large size, invalid request message framing, or deceptive request routing)");


            this.Result = r;
        }


    }
}