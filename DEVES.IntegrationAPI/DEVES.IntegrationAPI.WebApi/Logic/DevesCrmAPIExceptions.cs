using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TooManySearchResultsException : System.Exception
    {
        const string _message = "Result of searching from system:{0} exceed {1} records";
        public TooManySearchResultsException(string system, int recordsLimit):base(string.Format(_message, system, recordsLimit.ToString()))
        {
        }
    }
}