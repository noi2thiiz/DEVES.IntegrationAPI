using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public class FieldValidationException : Exception
    {
        public string fieldError = "";
        public string message = "";

        public FieldValidationException(string fieldError, string message)
        {
            this.fieldError = fieldError;
            this.message = message;
        }
    }
}