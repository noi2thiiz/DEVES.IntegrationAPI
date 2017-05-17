using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.UpdateSurveyStatus;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Linq;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class UpdateSurveyStatusController : ApiController
    {

        // log
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UpdateSurveyStatusController));

        OrganizationServiceProxy _serviceProxy;
        private Guid _accountId;

        public object Post([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new UpdateSurveyStatusOutputModel_Pass();
            var outputFail = new UpdateSurveyStatusOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<UpdateSurveyStatusInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/UpdateSurveyStatus_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new UpdateSurveyStatusOutputModel_Pass();
                _logImportantMessage += "TicketNo: " + contentModel.ticketNo;
                outputPass = HandleMessage(contentText, contentModel);

                return Request.CreateResponse<UpdateSurveyStatusOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new UpdateSurveyStatusOutputModel_Fail();
                outputFail.data = new UpdateSurveyStatusDataOutputModel_Fail();
                outputFail.data.fieldError = new List<UpdateSurveyStatusFieldErrorOutputModel_Fail>();

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
                            outputFail.data.fieldError.Add(new UpdateSurveyStatusFieldErrorOutputModel_Fail(splitProp[i].Trim(), fieldMessage));
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

                    if (!text.Contains("Required properties"))
                    {
                        outputFail.data.fieldError.Add(new UpdateSurveyStatusFieldErrorOutputModel_Fail(fieldName, fieldMessage));
                    }
                    
                }

                outputFail.code = "400";
                outputFail.message = "Invalid Input(s)";
                outputFail.description = "Some of your input is invalid. Please recheck again.";
                outputFail.transactionId = "Ticket ID: " + contentModel.ticketNo + ", Claim Noti No: " + contentModel.claimNotiNo;
                outputFail.transactionDateTime = DateTime.Now.ToString();

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", outputFail.code, Environment.NewLine, outputFail.description);
                return Request.CreateResponse<UpdateSurveyStatusOutputModel_Fail>(outputFail);
            }
        }

        private UpdateSurveyStatusOutputModel_Pass HandleMessage(string valueText, UpdateSurveyStatusInputModel content)
        {
            //TODO: Do what you want
            var output = new UpdateSurveyStatusOutputModel_Pass();

            _log.Info("HandleMessage");
            try
            {
                var UpdateSurveyStatusOutput = new UpdateSurveyStatusDataOutputModel_Pass();

                var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
                _serviceProxy = connection.OrganizationServiceProxy;
                ServiceContext svcContext = new ServiceContext(_serviceProxy);

                var query = from c in svcContext.IncidentSet
                            where c.pfc_claim_noti_number == content.claimNotiNo
                            select c;

                Incident incident = query.FirstOrDefault<Incident>();
                _accountId = new Guid(incident.IncidentId.ToString());

                if(Int32.Parse(content.iSurveyStatus) <= incident.pfc_isurvey_status.Value)
                {
                    output.code = "200";
                    output.message = "Success";
                    output.description = "Update survey status is done!";
                    output.transactionId = "";
                    output.transactionDateTime = System.DateTime.Now.ToString();
                    output.data = UpdateSurveyStatusOutput;
                    output.data.message = "ClaimNoti Number: " + content.claimNotiNo +
                        " i-Survey status: " + content.iSurveyStatus +
                        " i-Survey status on: " + content.iSurveyStatusOn;

                    return output;
                }

                Incident retrievedIncident = (Incident)_serviceProxy.Retrieve(Incident.EntityLogicalName, _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                try
                {

                    retrievedIncident.pfc_isurvey_status = new OptionSetValue(Int32.Parse(convertOptionSet(Incident.EntityLogicalName, "pfc_isurvey_status", content.iSurveyStatus)));
                    retrievedIncident.pfc_isurvey_status_on = Convert.ToDateTime(content.iSurveyStatusOn);

                    _serviceProxy.Update(retrievedIncident);

                }
                catch (Exception e)
                {
                    output.code = "501";
                    output.message = "False";
                    output.description = "Retrieving data PROBLEM";
                    output.transactionId = "";
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = null;

                    return output;
                }

                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "Update survey status is done!";
                output.transactionId = "";
                output.transactionDateTime = System.DateTime.Now.ToString();
                output.data = UpdateSurveyStatusOutput;
                output.data.message = "ClaimNoti Number: " + content.claimNotiNo +
                    " i-Survey status: " + content.iSurveyStatus +
                    " i-Survey status on: " + content.iSurveyStatusOn;
                // string strSql = string.Format(output.data.message, content.claimNotiNo, content.surveyType, content.surveyorCode, content.surveyorName);
            }

            catch (System.ServiceModel.FaultException e)
            {
                output.code = "500";
                output.message = "False";
                output.description = "CRM PROBLEM";
                output.transactionId = "";
                output.transactionDateTime = DateTime.Now.ToString();
                output.data = null;

                return output;
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

                output.code = "400";
                output.message = "False";
                output.description = "ไม่พบ claimNotiNo";
                output.transactionId = "Claim Noti No: null";
                output.transactionDateTime = DateTime.Now.ToString();
                output.data = null;
            }

            return output;
        }

        private string convertOptionSet(object entity, string fieldName, string value)
        {
            string valOption;
            if (value.Length == 1)
            {
                valOption = "10000000" + value;
            }
            else
            {
                valOption = "1000000" + value;
            }
            
            return valOption;
        }

        /*
        //To use with log
        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UpdateSurveyStatusController));

        public object Post([FromBody]object value)
        {
            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var output = new UpdateSurveyStatusOutputModel_Pass();

            var contentText = value.ToString();
            //_logImportantMessage = string.Format(_logImportantMessage, output.transactionId);
            var contentModel = JsonConvert.DeserializeObject<UpdateSurveyStatusInputModel>(contentText);
            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/UpdateSurveyStatus_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                _logImportantMessage += "Code: " + contentModel.ticketNo;
                output = HandleMessage(contentText, contentModel);
            }
            else
            {

                output = new UpdateSurveyStatusOutputModel_Pass();
                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.code, Environment.NewLine, output.message);
            }
            return Request.CreateResponse<UpdateSurveyStatusOutputModel_Pass>(output);
        }

        private UpdateSurveyStatusOutputModel_Pass HandleMessage(string valueText, UpdateSurveyStatusInputModel content)
        {
            //TODO: Do what you want
            var output = new UpdateSurveyStatusOutputModel_Pass();
            // var UpdateSurveyStatusOutput = new UpdateSurveyStatusDataOutputModel();
            _log.Info("HandleMessage");
            try
            {
                var UpdateSurveyStatusOutput = new UpdateSurveyStatusDataOutputModel_Pass();
                //TODO: Do something
                output.code = 200;
                output.message = "Success";

                output.data = UpdateSurveyStatusOutput;

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


            }

            return output;
        }
        */

    }
}