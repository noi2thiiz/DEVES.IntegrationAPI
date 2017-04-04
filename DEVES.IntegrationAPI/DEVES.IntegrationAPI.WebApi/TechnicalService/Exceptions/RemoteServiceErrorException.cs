namespace DEVES.IntegrationAPI.WebApi.Services.Core.Exceptions
{
    /// <summary>
    ///
    /// </summary>
    public class RemoteServiceErrorException:ServiceFailException
    {
        public RemoteServiceErrorException(ServiceFailResult r)
        {
            r.setHeaderProperty("code","0");
            this.Result = r;
        }



    }
}