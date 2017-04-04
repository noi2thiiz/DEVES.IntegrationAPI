using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http.ModelBinding;
using Microsoft.Ajax.Utilities;


namespace DEVES.IntegrationAPI.WebApi.Services.FieldErrorsParser
{

    public class ValidationErrorParser
    {
        public static List<dynamic> Parse(ModelStateDictionary modelState)
        {

            var fieldErrors = new List<dynamic>();

            foreach (string k in modelState.Keys)
            {

                var modelStateVal = modelState[k];
                var countError = modelStateVal.Errors.Count;

                var f = new FieldError
                {
                    name = string.Join(".", (k.ToString()).Split('.').Skip(1) ),
                    message = "Invalid field format"
                };
                Console.WriteLine("fieldErrors: {0}",f.name);
                try
                {
                    if (countError == 1)
                    {
                        var error = modelStateVal.Errors[0];

                        if (error.Exception != null)
                        {
                            //Console.WriteLine(error.Exception.InnerException.ToJSON());

                            try
                            {
                                f.message = error.Exception.Message.Equals("Null object cannot be converted to a value type.")  ?
                                    $"The {f.name} field is required." : error.Exception.InnerException.Message;
                                //    Console.WriteLine("ErrorMessage:" + error.Exception.Message);
                                //f.errorType = error.Exception.InnerException.GetType().Name;
                            }
                            catch (Exception e)
                            {
                                //Required property 'requestChanel' not found in JSON. Path '', line 43, position 1."
                                string text = error.Exception.Message;
                                Regex pattern = new Regex(
                                    @"Required property '(?<field>\S+)' not found in JSON. Path '', line (?<line>\d+), position (?<position>\d+).");
                                Match match = pattern.Match(text);
                                f.name = match.Groups["field"].Value;
                                if (match.Success)
                                {
                                    f.message = $"The {f.name} field is required.";
                                    modelStateVal.Errors.Add(new ModelError(text));
                                }

                                pattern = new Regex(
                                    @"Unexpected token Undefined when parsing enum. Path '(?<field>\S+)', line (?<line>\d+), position (?<position>\d+).");
                                match = pattern.Match(text);
                                f.name = match.Groups["field"].Value;
                                if (match.Success)
                                {
                                    f.message = $"The {f.name} field is required.";
                                    modelStateVal.Errors.Add(new ModelError(text));
                                }

                                pattern = new Regex(
                                    @"Could not convert string to integer: (?<value>\S+). Path '(?<field>\S+)', line (?<line>\d+), position (?<position>\d+).");
                                match = pattern.Match(text);
                                f.name = match.Groups["field"].Value;
                                if (match.Success)
                                {

                                    f.message = $"The {f.name} field is not valid integer.";
                                    modelStateVal.Errors.Add(new ModelError(text));

                                }


                                pattern = new Regex(
                                    @"Null object cannot be converted to a value type.");
                                match = pattern.Match(text);

                                if (match.Success)
                                {

                                    f.message = $"The {f.name} field is required.";
                                    modelStateVal.Errors.Add(new ModelError(text));

                                }



                                Console.WriteLine("Message1 :" + e.Message);
                            }


                            //}
                            //catch (Exception e)
                            //{
                            // f.type = error.Exception.InnerException.GetType().Name;
                            //f.message = error.Exception.InnerException.Message;
                            //}

                        }
                        else
                        {
                            try
                            {
                                Console.WriteLine(
                                    "=========  ! if (error.Exception != null)==========");
                                Console.WriteLine(error.ToString());
                                //f.errorType = error.GetType().Name;

                                f.message = error.ErrorMessage;
                               // f.AddErrorDetail( new ErrorItem(error));

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Message2 :" + e.Message);
                            }

                        }
                    }



                    foreach (var error in modelStateVal.Errors)
                    {

                        try
                        {

                            f.AddErrorDetail(GetErrorType(f.name,error));
                        }
                        catch (Exception e)
                        {
                          //  f.AddErrorDetail(new FieldErrorData(e));
                           Console.WriteLine("Message3 :"+e.Message);
                           Console.WriteLine("StackTrace3 :"+e.StackTrace);
                        }

                    }

                    fieldErrors.Add(f);
                }
                catch (Exception e)
                {
                    Console.WriteLine("==============Exception 2================================");
                    Console.WriteLine(modelState);
                    //f.errors.Add(new FieldErrorData(e));
                    Console.WriteLine(e);

                }
            }
            return fieldErrors;
        }

        private static ErrorBase GetErrorType(string fieldName, ModelError error)
        {

            var message = "";
            if (error.Exception != null)
            {
                //this.type = error.Exception.InnerException.GetType().Name;
                message = error.Exception.InnerException.Message;
            }
            else
            {
                //this.type = error.GetType().Name;
                message= error.ErrorMessage;
            }

            Regex pattern = new Regex(
                @"The (?<field>\S+) field is required.");
            Match match = pattern.Match( message);
            if (match.Success)
            {
                if (string.IsNullOrEmpty(fieldName))
                {
                    fieldName =  match.Groups["field"].Value;
                }
                return new  NotNullOrEmptyError(fieldName,message);
            }

             pattern = new Regex(
                @"Null object cannot be converted to a value type.");
             match = pattern.Match( message);
            if (match.Success)
            {
                if (string.IsNullOrEmpty(fieldName))
                {
                   // fieldName =  match.Groups["field"].Value;
                }
                return new  NotNullOrEmptyError(fieldName,message);
            }



             pattern = new Regex(
                @"Required property '(?<field>\S+)' not found in JSON. Path '', line (?<line>\d+), position (?<position>\d+).");
             match = pattern.Match( message);
            if (match.Success)
            {
                if (string.IsNullOrEmpty(fieldName))
                {
                    fieldName =  match.Groups["field"].Value;
                }
                return new  NotNullOrEmptyError(fieldName,message);
            }




            pattern = new Regex(
                @"The field (?<field>\S+) is not valid datetime");
            match = pattern.Match( message);
            if (match.Success)
            {
                if (string.IsNullOrEmpty(fieldName))
                {
                    fieldName =  match.Groups["field"].Value;
                }
                return new InvalidDateTimeError(fieldName,message);
            }





            pattern = new Regex(
                @"The field (?<field>\S+) is invalid.");
            match = pattern.Match( message);
            if (match.Success)
            {
                if (string.IsNullOrEmpty(fieldName))
                {
                    fieldName =  match.Groups["field"].Value;
                }
                return new InvalidParameterError(fieldName,message);
            }



            pattern = new Regex(
                @"Requested value '(?<value>\S+)' was not found.");
            match = pattern.Match( message);
            if (match.Success)
            {

                    var value =  match.Groups["value"].Value;

                    return new InvalidEnumError(fieldName,$"The Value '{value}' of {fieldName} is not defined in enum members.");
            }


            pattern = new Regex(
                @"he field (?<field>\S+) must be a string with a maximum length of (?<length>\S+).");
            match = pattern.Match( message);
            if (match.Success)
            {

                if (string.IsNullOrEmpty(fieldName))
                {
                    fieldName =  match.Groups["field"].Value;
                }

                return new InvalidStringLengthError(fieldName,message);
            }





            return new UnknownError(fieldName,message);

        }



        public object modelState {  set; get; }
        public void AddModelState(ModelStateDictionary modelState)
        {

            this.modelState = modelState;

        }
    }
}