using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using Microsoft.Crm.Sdk.Messages;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public class FieldValidationException : Exception
    {
        public OutputModelFailData fieldErrorData { get; set; } = new OutputModelFailData();

        public string fieldError { get; set; } = "";
    
        public string message  {get; set; } = "";
        public string description { get; set; } = "";
        public string fieldMessage { get; set; } = "";

     

        public FieldValidationException()
        {
        }

        public FieldValidationException(string message)
        {
            this.message = message;
        }

        public FieldValidationException(string fieldError, string fieldMessage)
        {
            this.fieldError = fieldError;
            this.fieldMessage = fieldMessage;
            this.message = fieldMessage;
          
        }
        public FieldValidationException(string fieldError, string fieldMessage,string message)
        {
            this.fieldError = fieldError;
            this.fieldMessage = fieldMessage;
            this.message = message;
           
        }

        public FieldValidationException(OutputModelFailData fieldErrorData)
        {
            this.fieldErrorData = fieldErrorData;
        }

        public FieldValidationException(OutputModelFailDataFieldErrors fieldErrorData, string message)
        {
            this.fieldErrorData.fieldErrors.Add(fieldErrorData);
            this.message = message;
        }

        public FieldValidationException(IEnumerable<OutputModelFailDataFieldErrors> fieldErrorData, string message,string description)
        {
            foreach (OutputModelFailDataFieldErrors  field in fieldErrorData )
            {
                this.fieldErrorData.fieldErrors.Add(field);
            }


          
            this.message = message;
            this.description = description;
        }
        public FieldValidationException(IEnumerable<OutputModelFailDataFieldErrors> fieldErrorData, string message)
        {
            foreach (OutputModelFailDataFieldErrors field in fieldErrorData)
            {
                this.fieldErrorData.fieldErrors.Add(field);
            }



            this.message = message;
        }


    }
}