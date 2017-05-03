using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Templates.Exceptions
{
    public class IntegrationServicesErrorException:Exception
    {
        public string ErrorCode { get; set; } 
        public string ErrorMessage { get; set; } 
        public string ErrorDescription { get; set; } 

        public string ServiceName { get; set; } 

        public IntegrationServicesErrorException( string errorCode, string errorMessage,  string errorDescription, string serviceName)
        {
            ErrorCode = errorCode;
            ErrorMessage =  "The "+serviceName + " Services Error or connection timeout";
            ServiceName = serviceName;
            ErrorDescription = errorDescription;


        }
    }
}