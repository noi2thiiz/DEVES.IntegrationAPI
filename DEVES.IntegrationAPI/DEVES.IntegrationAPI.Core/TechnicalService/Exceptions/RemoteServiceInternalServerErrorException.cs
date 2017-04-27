namespace DEVES.IntegrationAPI.WebApi.Core.Exceptions
{
    /// <summary>
    ///
    /// </summary>
    public class RemoteServiceInternalServerErrorException:ServiceFailException
    {
        public RemoteServiceInternalServerErrorException(ServiceFailResult r)
        {
            r.setHeaderProperty("code","0");
            this.Result = r;
        }



    }
}