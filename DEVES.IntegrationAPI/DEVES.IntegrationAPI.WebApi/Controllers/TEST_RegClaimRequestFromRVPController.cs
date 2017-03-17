using DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP;
using DEVES.IntegrationAPI.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class TEST_RegClaimRequestFromRVPController : ApiController
    {
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(TEST_RegClaimRequestFromRVPController));

        public object Post([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new TEST_RegClaimRequestFromRVPOutputModel_Pass();
            var outputFail = new TEST_RegClaimRequestFromRVPOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<RegClaimRequestFromRVPInputModel>(contentText);
            
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/RegClaimRequestFromRVP_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new TEST_RegClaimRequestFromRVPOutputModel_Pass();
                _logImportantMessage += "rvpCliamNo: " + contentModel.rvpCliamNo;
                outputPass = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<TEST_RegClaimRequestFromRVPOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new TEST_RegClaimRequestFromRVPOutputModel_Fail();
                outputFail.data = new TEST_RegClaimRequestFromRVPDataOutputModel_Fail();
                outputFail.data.fieldErrors = new List<TEST_RegClaimRequestFromRVPFieldErrors>();

                List<string> errorMessage = JsonHelper.getReturnError();

                foreach(var text in errorMessage) {
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

                    outputFail.data.fieldErrors.Add(new TEST_RegClaimRequestFromRVPFieldErrors(fieldName, fieldMessage));
                }

                outputFail.message = "Invalid Input(s)";
                outputFail.description = "Some of your input is invalid. Please recheck again.";

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", outputFail.code, Environment.NewLine, outputFail.description);
                return Request.CreateResponse<TEST_RegClaimRequestFromRVPOutputModel_Fail>(outputFail);
            }
        }

        private TEST_RegClaimRequestFromRVPOutputModel_Pass HandleMessage(string valueText, RegClaimRequestFromRVPInputModel content)
        {
            //TODO: Do what you want
            var output = new TEST_RegClaimRequestFromRVPOutputModel_Pass();

            _log.Info("HandleMessage");

            try
            {
                var RegClaimRequestFromRVPOutput = new TEST_RegClaimRequestFromRVPDataOutputModel_Pass();

                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "ReqClaimRequestFromRVP is done!";
                //output.transactionId = content.ticketNo;
                output.transactionId = "{0} ticketNo need to be added from stored";
                output.transactionDateTime = DateTime.Now.ToString();
                //RegClaimRequestFromRVPOutput.ticketNo = "ticketNo: " + content.ticketNo;
                RegClaimRequestFromRVPOutput.ticketNo = "{1} ticketNo need to be added from stored";
                RegClaimRequestFromRVPOutput.claimNotiNo = "{1} claimNotiNo need to be added from stored";
                output.data = RegClaimRequestFromRVPOutput;
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