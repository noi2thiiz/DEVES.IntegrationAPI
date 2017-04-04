using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Http.ModelBinding;
using DEVES.IntegrationAPI.WebApi.Core.FieldErrorsParser;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.WebApi.Core
{
    public interface IServiceResult
    {
        string code { get; set; }

        /// <summary>
        /// Service’s Response Description
        /// </summary>
        string message { get; set; }

        string description { get; set; }
        string transactionId { get; set; }

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        string transactionDateTime { get; set; }


        //ServiceResultHeader Header { get; set; }
        // object Request { get; set;}

        void AddRequest(object request);

        //void AddHeader(ServiceResultHeader header);
        void setHeaderProperty(string key, string value);

        void AddBodyData(dynamic data);

        RESTClientResult GetResponse();
        void SetResponse(RESTClientResult response);
    }


    public class ServiceBadRequestResult : ServiceResultSingleData<ErrorData>
    {
        // public string type {  set; }
        // public string methodname {  set; }
        // public object stackTrace {  set; }

        private object modelState { set; get; }
    }


    public class ServiceFailResult : ServiceResultSingleData<object>
    {
        // public string type { get; set; }
        // public string methodname { get; set; }
        // public object stackTrace { get; set; }

        private object ModelState { get; set; }

        public void AddModelState(ModelStateDictionary modelState)
        {
            //this.ModelState = modelState;
        }
    }


    public class FieldError
    {
        [JsonProperty(Order = 1)]
        public string name { get; set; }

        private string errorType { get; set; }

        [JsonProperty(Order = 2)]
        public string message { get; set; }

        [JsonProperty(Order = 3)]
        public List<dynamic> errors;

        public FieldError()
        {
            name = "";
            message = "";
            this.errors = new List<dynamic>();
        }

        public void AddErrorDetail(ErrorBase error)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = error.getFieldName();
            }

            if (message.Equals("Null object cannot be converted to a value type."))
            {
                message = $"The {name} field is required.";
            }
            this.errors.Add(error);
        }

        private List<dynamic> data { get; set; }
    }

    public class ErrorItem
    {
        public string message { get; set; }
        public string type { get; set; }
        public string x { get; set; }

        public ErrorItem(ModelError error)
        {
            this.x = "x";
            this.message = error.ErrorMessage;
            this.type = error.GetType().Name;

        }

    }
    public class ErrorData
    {
        public ErrorData()
        {
            this.message = "The input was invalid";
            this.type = "UnknownError";
            this.fieldErrors = new List<dynamic>();
        }



        public string message { get; set; }
        public string type { get; set; }
        public List<dynamic> fieldErrors { get; set; }

        public void AddFieldErrors(List<dynamic> fieldErrors)
        {
            if (fieldErrors != null) this.fieldErrors = fieldErrors;
        }
    }

    public class FieldErrorData
    {
        public string type { get; set; }

        public string message { get; set; }

       // public string format { get; set; }

        public FieldErrorData(ModelError error)
        {
            if (error.Exception != null)
            {
                this.type = error.Exception.InnerException.GetType().Name;
                this.message = error.Exception.InnerException.Message;
            }
            else
            {
                this.type = error.GetType().Name;
                this.message = error.ErrorMessage;
            }

            this.type = GetErrorType(this.message);

            //this.error = error;
        }

        public string GetErrorType(string message)
        {
            Regex pattern = new Regex(
                @"The (?<field>\S+) field is required.");
            Match match = pattern.Match( message);
            if (match.Success)
            {
                return "NotNullOrEmpty";
            }


             pattern = new Regex(
                @"The field (?<field>\S+) must be a string with datetime format 'yyyy-mm-dd HH:mm:ss' ");
             match = pattern.Match( message);
            if (match.Success)
            {
               // this.format = "yyyy-MM-dd HH:mm:ss";
                return "InvalidDateTime";
            }


            pattern = new Regex(
                @"The field (?<field>\S+) is invalid.");
            match = pattern.Match( message);
            if (match.Success)
            {
                // this.format = "yyyy-MM-dd HH:mm:ss";
                return "InvalidValue";
            }




            return "ModelError";
        }

        public FieldErrorData(Exception error)
        {
            this.type = error.GetType().Name;
            this.message = error.Message.ToString();
        }


        //public ModelError error { get; set; }
    }

    public class ServiceResultHeader
    {
        /// <summary>
        /// Service’s Response Code
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// Service’s Response Description
        /// </summary>
        public string message { get; set; }

        public string description { get; set; }
        public string transactionId { get; set; }

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string transactionDateTime { get; set; }
    }

    public class ServiceResultBodyList<T>
    {
        public object request { get; set; }

        public ServiceResultBodyList()
        {
            data = new List<T>();
        }

        private List<T> _dataList;

        public List<T> data
        {
            get { return _dataList; }
            set { _dataList = value; }
        }
    }


    public class ServiceResultBodyObject<T>
    {
        public object request { get; set; }
        public T data { get; set; }

        public void Add(T d)
        {
            data = d;
        }
    }
}