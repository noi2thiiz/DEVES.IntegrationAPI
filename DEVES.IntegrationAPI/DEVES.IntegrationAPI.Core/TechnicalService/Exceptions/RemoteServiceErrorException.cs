using System;

namespace DEVES.IntegrationAPI.WebApi.Core.Exceptions
{
    /// <summary>
    ///
    /// </summary>
    public class RemoteServiceErrorException:ServiceFailException
    {
        public RemoteServiceErrorException(IServiceResult r)
        {
            Console.WriteLine("new RemoteServiceErrorException"+ r.code +r.message);
            // r.setHeaderProperty("code","500");
            this.Result = r;
        }



    }
}