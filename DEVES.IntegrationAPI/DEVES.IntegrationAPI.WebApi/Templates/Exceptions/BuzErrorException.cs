using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;

namespace DEVES.IntegrationAPI.WebApi.Templates.Exceptions
{
    public class BuzErrorException:Exception
     
    {
        private RegPayeeCorporateContentOutputModel regPayeeCorporateOutput;

        public string Code { get; set; }
        public override string Message { get;  }
        public string Description { get; set; }
        public string SourceError { get; set; }

        public object SourceData { get; set; }
        public string TransactionId { get; set; }
        

        public object OutputModel { get; set; }
        public BuzErrorException(string code, string message, string description, string sourceError)
        {
            Code = code;
            Message = message;
            Description = description;
            SourceError = sourceError;
        }
        public BuzErrorException(string code, string message, string description)
        {
            Code = code;
            Message = message;
            Description = description;
         
        }

        public BuzErrorException(string code, string message, string description,object sourceData, string sourceError)
        {
            Code = code;
            Message = message;
            Description = description;
            SourceError = sourceError;
            SourceData = sourceData;
          

        }

         public BuzErrorException(string code, string message, string description, string sourceError, string transactionId)
        {
            Code = code;
            Message = message;
            Description = description;
            SourceError = sourceError;
          
            TransactionId = transactionId;

        }

        public BuzErrorException(object regPayeeCorporateOutput, object sourceData)
        {
            SourceData = sourceData;
            OutputModel = regPayeeCorporateOutput;
        }
    }
}