using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class InquiryCRMPayeeListController : ApiController
    {

        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(InquiryCRMPayeeListController));

        public object Post([FromBody]object value)
        {
            Console.WriteLine(value.ToJson());
            buzInquiryCRMPayeeList cmd = new buzInquiryCRMPayeeList();
            cmd.TransactionId = Guid.NewGuid().ToString(); // Request.Properties["TransactionID"].ToString();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<InquiryCRMPayeeListInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/InquiryCRMPayeeList_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                var content = cmd.Execute(cmd.DeserializeJson<InquiryCRMPayeeListInputModel>(value.ToString()));
                return Request.CreateResponse(content);
            }
            else
            {
                var outputFail = new InquiryCRMPayeeListOutputModel_Fail();
                outputFail.data = new InquiryCRMPayeeListDataOutputModel_Fail();
                outputFail.data.fieldErrors = new List<InquiryCRMPayeeListFieldErrors>();

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

                        for (int i = 0; i < text.Length - 1; i++)
                        {
                            if (text.Substring(i, 1).Equals("."))
                            {
                                fieldMessage = text.Substring(0, i);
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

                    outputFail.data.fieldErrors.Add(new InquiryCRMPayeeListFieldErrors(fieldName, fieldMessage));
                }

                outputFail.code = "500";
                outputFail.message = "Invalid Input(s)";
                outputFail.description = "Some of your input is invalid. Please recheck again.";
                outputFail.transactionId = Request.Properties["TransactionID"].ToString();
                outputFail.transactionDateTime = DateTime.Now.ToString();

                return Request.CreateResponse<InquiryCRMPayeeListOutputModel_Fail>(outputFail);
            }

        }

        public object Put([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new InquiryCRMPayeeListOutputModel_Pass();
            var outputFail = new InquiryCRMPayeeListOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<InquiryCRMPayeeListInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/InquiryCRMPayeeList_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new InquiryCRMPayeeListOutputModel_Pass();
                _logImportantMessage += "polisyClientId: " + contentModel.polisyClientId;
                outputPass = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<InquiryCRMPayeeListOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new InquiryCRMPayeeListOutputModel_Fail();
                outputFail.data = new InquiryCRMPayeeListDataOutputModel_Fail();
                outputFail.data.fieldErrors = new List<InquiryCRMPayeeListFieldErrors>();
                outputFail.data.fieldErrors.Add(new InquiryCRMPayeeListFieldErrors());
                //List<string> errorMessage = JsonHelper.getReturnError();

                outputFail.message = "Invalid Input(s)";
                outputFail.description = "Some of your input is invalid. Please recheck again.";

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", outputFail.code, Environment.NewLine, outputFail.description);
                return Request.CreateResponse<InquiryCRMPayeeListOutputModel_Fail>(outputFail);
            }
        }

        private InquiryCRMPayeeListOutputModel_Pass HandleMessage(string valueText, InquiryCRMPayeeListInputModel content)
        {
            //TODO: Do what you want
            var output = new InquiryCRMPayeeListOutputModel_Pass();

            _log.Info("HandleMessage");

            try
            {
                var InquiryCRMPayeeListOutput = new List<InquiryCRMPayeeListDataOutputModel_Pass>();

                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "InquiryCRMPayeeList is done!";
                //output.transactionId = content.ticketNo;
                output.transactionId = "InquiryCRMPayeeList";
                output.transactionDateTime = DateTime.Now.ToString();
                //InquiryCRMPayeeListOutput.ticketNo = "ticketNo: " + content.ticketNo;
                //InquiryCRMPayeeListOutput. = "{1} ticketNo need to be added from stored";
                //InquiryCRMPayeeListOutput.claimNotiNo = "{1} claimNotiNo need to be added from stored";
                output.data = new List<InquiryCRMPayeeListDataOutputModel_Pass>();
                output.data.Add(new InquiryCRMPayeeListDataOutputModel_Pass());
            }
            catch (Exception e)
            {

                var errorMessage = e.GetType().FullName + ": " + e.Message + Environment.NewLine;
                errorMessage += "StackTrace: " + e.StackTrace;

                if (e.InnerException != null)
                {
                    errorMessage += Environment.NewLine + "(InnerException)" + e.InnerException.GetType().FullName + " - " + e.InnerException.Message + Environment.NewLine;
                    errorMessage += "StackStrace: " + e.InnerException.StackTrace;
                }
                _log.Error("RequestId - " + _logImportantMessage);
                _log.Error(errorMessage);
                output.description = "Mapping Error";

            }

            return output;
        }

    }
}