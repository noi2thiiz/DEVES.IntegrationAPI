using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi
{
    public class AppErrorException : Exception
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        

        public AppErrorException(string code, string message, string description ="")
        {
            Code = code;
            Message = message;
            Description = description;
           
        }
    }
}