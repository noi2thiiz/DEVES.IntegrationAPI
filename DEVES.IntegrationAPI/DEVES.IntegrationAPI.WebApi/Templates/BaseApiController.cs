using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public class BaseApiController:ApiController
    {

        public string GetTransactionId()
        {
            try
            {
                if (string.IsNullOrEmpty(Request?.Properties["TransactionID"]?.ToStringOrEmpty()))
                {

                    Request.Properties["TransactionID"] = GlobalTransactionIdGenerator.Instance.GetNewGuid();
                }

                return Request.Properties["TransactionID"].ToString();
            }
            catch (Exception)
            {
                return  GlobalTransactionIdGenerator.Instance.GetNewGuid();
            }
            
            
        }

        protected HttpResponseMessage ProcessRequest<TCommand, TInput>(object value,string schemaFileName)
            where TCommand : new()
            where TInput : class

        {
            // Console.WriteLine(value.ToJson());

            #region Initiate Command and Output Model

            
            var type = typeof(TCommand);
            object instance = Activator.CreateInstance(type);
            BaseCommand cmd = (BaseCommand)instance;
            var outputFail = new OutputModelFail();

            cmd.TransactionId = GetTransactionId();

            #endregion


            string contentText = value?.ToString();
            //.Replace("=", ":"); ;
            
            //TInput contentModel;
            try
            {
                //try  Deserialize Object
                JsonConvert.DeserializeObject<TInput>(contentText);
            }
            catch (Exception e)
            {
                //output model
                outputFail.code = AppConst.CODE_INVALID_INPUT;
                outputFail.message = AppConst.MESSAGE_INVALID_INPUT;
                outputFail.description = "Cannot parse JSON:"+e.Message+ contentText;
                outputFail.transactionId = GetTransactionId();
                outputFail.transactionDateTime = DateTime.Now;

                return Request.CreateResponse(outputFail);
            }
          

            string outvalidate;
            var filePath = "";
            try
            {
                 filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/" + schemaFileName);
            }
            catch (Exception)
            {
                //C:\Users\patiw\Source\Repos\Production1\DEVES.IntegrationAPI\DEVES.IntegrationAPI.WebApiTests1\bin\Release
                string startupPath = Environment.CurrentDirectory?.Replace(@"Tests1\bin\Release","");
               
                filePath = startupPath+"/App_Data/JsonSchema/" +schemaFileName;
            }

           

            if (!JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
               
                try
                {
                 




                    List<string> errorMessage = JsonHelper.getReturnError();
                    foreach (var text in errorMessage)
                    {
                        string fieldMessage = "";
                        string fieldName = "";
                        if (text.Contains("Required properties"))
                        {
                            int indexEnd = 0;
                            for (int i = 0; i < text.Length - 1; i++)
                            {
                                if (text.Substring(i, 1).Equals(":"))
                                {
                                    fieldMessage = text.Substring(0, i);
                                    indexEnd = i + 1;
                                }
                                if (text.Substring(i, 1).Equals("."))
                                {
                                    fieldName = text.Substring(indexEnd, i - indexEnd).Trim();
                                    break;
                                }
                            }

                            string[] splitProp = fieldName.Split(',');
                            for (int i = 0; i < splitProp.Length; i++)
                            {
                                outputFail.AddFieldError(splitProp[i].Trim(), fieldMessage);
                            }
                        }
                        else if (text.Contains("exceeds maximum length"))
                        {
                            bool isMessage = false;
                            int endMessage = 0;
                            int startName = 0;
                            int endName = 0;
                            for (int i = 0; i < text.Length - 4; i++)
                            {
                                if (text.Substring(i, 4).Equals("Path"))
                                {
                                    fieldMessage = text.Substring(0, i - 1);
                                    isMessage = true;
                                    endMessage = i + "Path".Length;
                                }
                                if (isMessage)
                                {
                                    if (text.Substring(i, 1).Equals("'"))
                                    {
                                        if (startName == 0)
                                        {
                                            startName = i + 1;
                                        }
                                        else if (endName == 0)
                                        {
                                            endName = i - 1;
                                        }
                                    }
                                    if (startName != 0 && endName != 0)
                                    {
                                        fieldName = text.Substring(startName, i - startName).Trim();
                                        break;
                                    }
                                }
                            }
                        }
                        else if (text.Contains("minimum length"))
                        {
                            bool isMessage = false;
                            int startName = 0;
                            int endName = 0;
                            for (int i = 0; i < text.Length - 7; i++)
                            {
                                string check = text.Substring(i, 7);
                                if (text.Substring(i, 7).Equals("minimum"))
                                {
                                    fieldMessage = "Required field must not be null";
                                    isMessage = true;
                                }
                                if (isMessage)
                                {
                                    if (text.Substring(i, 1).Equals("'"))
                                    {
                                        if (startName == 0)
                                        {
                                            startName = i + 1;
                                        }
                                        else if (endName == 0)
                                        {
                                            endName = i - 1;
                                        }
                                    }
                                    if (startName != 0 && endName != 0)
                                    {
                                        fieldName = text.Substring(startName, i - startName).Trim();
                                        break;
                                    }
                                }
                            }
                        }
                        else if (text.Contains("exceeds maximum value"))
                        {
                            bool isMessage = false;
                            int endMessage = 0;
                            int startName = 0;
                            int endName = 0;
                            for (int i = 0; i < text.Length - 4; i++)
                            {
                                if (text.Substring(i, 4).Equals("Path"))
                                {
                                    fieldMessage = text.Substring(0, i - 1);
                                    isMessage = true;
                                    endMessage = i + "Path".Length;
                                }
                                if (isMessage)
                                {
                                    if (text.Substring(i, 1).Equals("'"))
                                    {
                                        if (startName == 0)
                                        {
                                            startName = i + 1;
                                        }
                                        else if (endName == 0)
                                        {
                                            endName = i - 1;
                                        }
                                    }
                                    if (startName != 0 && endName != 0)
                                    {
                                        fieldName = text.Substring(startName, i - startName).Trim();
                                        break;
                                    }
                                }
                            }
                        }
                        else if (text.Contains("minimum value"))
                        {
                            bool isMessage = false;
                            int startName = 0;
                            int endName = 0;
                            for (int i = 0; i < text.Length - 7; i++)
                            {
                                string check = text.Substring(i, 7);
                                if (text.Substring(i, 7).Equals("minimum"))
                                {
                                    fieldMessage = "The value is less than the minimum value";
                                    isMessage = true;
                                }
                                if (isMessage)
                                {
                                    if (text.Substring(i, 1).Equals("'"))
                                    {
                                        if (startName == 0)
                                        {
                                            startName = i + 1;
                                        }
                                        else if (endName == 0)
                                        {
                                            endName = i - 1;
                                        }
                                    }
                                    if (startName != 0 && endName != 0)
                                    {
                                        fieldName = text.Substring(startName, i - startName).Trim();
                                        break;
                                    }
                                }
                            }
                        }
                        else if (text.Contains("Invalid type."))
                        {
                            int startIndex = "Invalid type.".Length;
                            int endMessage = 0;
                            int startName = 0;
                            int endName = 0;
                            for (int i = startIndex; i < text.Length - 1; i++)
                            {
                                if (text.Substring(i, 1).Equals("."))
                                {
                                    fieldMessage = text.Substring(0, i);
                                    endMessage = i + 1;
                                }
                                if (text.Substring(i, 1).Equals("'"))
                                {
                                    if (startName == 0)
                                    {
                                        startName = i + 1;
                                    }
                                    else if (endName == 0)
                                    {
                                        endName = i - 1;
                                    }
                                }
                                if (startName != 0 && endName != 0)
                                {
                                    fieldName = text.Substring(startName, i - startName).Trim();
                                    break;
                                }
                            }
                        }
                        else if (text.Contains("not defined in enum"))
                        {
                            int startName = 0;
                            int endName = 0;
                            bool isChecking = true;

                            for (int i = 0; i < text.Length - 1; i++)
                            {
                                if (text.Substring(i, 1).Equals(".") && isChecking)
                                {
                                    fieldMessage = text.Substring(0, i);
                                    isChecking = false;
                                }
                                if (text.Substring(i, 1).Equals("'"))
                                {
                                    if (startName == 0)
                                    {
                                        startName = i + 1;
                                    }
                                    else if (endName == 0)
                                    {
                                        endName = i - 1;
                                    }
                                }
                                if (startName != 0 && endName != 0)
                                {
                                    fieldName = text.Substring(startName, i - startName).Trim();
                                    break;
                                }
                            }
                        }
                        else if (text.Contains("JSON does not match any schemas"))
                        {
                            int startIndex = "JSON does not match any schemas from 'anyOf'. Path '".Length;
                            int startName = startIndex;
                            int endName = 0;

                            for (int i = startIndex; i < text.Length - 1; i++)
                            {
                                if (text.Substring(i, 1).Equals("'"))
                                {
                                    endName = i;
                                    break;
                                }
                            }

                            fieldName = text.Substring(startName, endName - startName).Trim();
                            fieldMessage = "Incorrect input format";
                        }
                        else if (text.Contains("not a multiple of"))
                        {
                            int getSingle = 0;
                            bool isPath = false;
                            int startName = 0;
                            int endName = 0;

                            for (int i = 0; i < text.Length - 1; i++)
                            {
                                if(isPath)
                                {

                                }

                                if (text.Substring(i, 1).Equals("'"))
                                {
                                    if(getSingle == 0)
                                    {
                                        startName = i;
                                    }
                                    else if (getSingle == 1)
                                    {
                                        endName = i;
                                    }
                                    getSingle++;
                                }

                                if(getSingle == 2)
                                {
                                    break;
                                }
                            }

                            fieldName = text.Substring(startName, endName - startName).Trim();
                            fieldMessage = "Input floating point is invalid";
                        }

                        if (!text.Contains("Required properties"))
                        {
                            outputFail.AddFieldError(fieldName, fieldMessage);
                        }
                        
                    }
                    //output model
                    outputFail.code = AppConst.CODE_INVALID_INPUT;
                    outputFail.message = AppConst.MESSAGE_INVALID_INPUT;
                    outputFail.description = AppConst.DESC_INVALID_INPUT;
                    outputFail.transactionId = GetTransactionId();
                    outputFail.transactionDateTime = DateTime.Now;

                    return Request.CreateResponse(outputFail);
                }
                catch (Exception)
                {
                    //output model
                    outputFail.code = AppConst.CODE_INVALID_INPUT;
                    outputFail.message = AppConst.MESSAGE_INVALID_INPUT;
                    outputFail.description = "Cannot parse JSON";
                    outputFail.transactionId = GetTransactionId();
                    outputFail.transactionDateTime = DateTime.Now;

                    return Request.CreateResponse(outputFail);
                }
            }
            else
            {
                var content = cmd.Execute(cmd.DeserializeJson<TInput>(value.ToString()));
                return Request.CreateResponse(content);
            }

            
        }

   

    }
}