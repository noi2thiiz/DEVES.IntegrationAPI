using System;

namespace DEVES.IntegrationAPI.WebApi.Services.Core.Exceptions
{
    public abstract class ServiceFailException :Exception
    {


        /// <summary>
        ///
        /// </summary>
        public IServiceResult Result { get; set; }
    }
}