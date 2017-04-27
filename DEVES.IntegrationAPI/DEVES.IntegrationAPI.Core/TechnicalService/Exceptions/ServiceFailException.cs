using DEVES.IntegrationAPI.Model;
using System;

namespace DEVES.IntegrationAPI.WebApi.Core.Exceptions
{
    public abstract class ServiceFailException :Exception
    {


        /// <summary>
        ///
        /// </summary>
        public IServiceResult Result { get; set; }
    }
}