using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.UpdateClaimNo;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class UpdateClaimNoController : ApiController
    {

        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UpdateClaimNoController));

        OrganizationServiceProxy _serviceProxy;
        private Guid _accountId;

        private Guid _transactionId;
        public object Post([FromBody]object value)
        {
            _transactionId = new Guid();

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new UpdateClaimNoOutputModel_Pass();
            var outputFail = new UpdateClaimNoOutputModel_Fail();
            outputFail.data = new UpdateClaimNoDataOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<UpdateClaimNoInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/UpdateClaimNo_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new UpdateClaimNoOutputModel_Pass();
                _logImportantMessage += "ticketNo: " + contentModel.ticketNo;
                outputPass = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<UpdateClaimNoOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new UpdateClaimNoOutputModel_Fail();
                outputFail.data = new UpdateClaimNoDataOutputModel_Fail();
                outputFail.data.fieldErrors = new List<UpdateClaimNoFieldErrors>();

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
                            outputFail.data.fieldErrors.Add(new UpdateClaimNoFieldErrors(splitProp[i].Trim(), fieldMessage));
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
                        outputFail.data.fieldErrors.Add(new UpdateClaimNoFieldErrors(fieldName, fieldMessage));
                    }
                    
                }

                outputFail.code = "400";
                outputFail.message = "Invalid Input(s)";
                outputFail.description = "Some of your input is invalid. Please recheck again.";
                outputFail.transactionId = _transactionId.ToString();
                outputFail.transactionDateTime = DateTime.Now.ToString();

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", outputFail.code, Environment.NewLine, outputFail.description);
                return Request.CreateResponse<UpdateClaimNoOutputModel_Fail>(outputFail);

            }
        }
        private UpdateClaimNoOutputModel_Pass HandleMessage(string valueText, UpdateClaimNoInputModel content)
        {
            //TODO: Do what you want
            var output = new UpdateClaimNoOutputModel_Pass();

            _log.Info("HandleMessage");
            try
            {
                var UpdateClaimNoOutput = new UpdateClaimNoDataOutputModel_Pass();

                var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
                _serviceProxy = connection.OrganizationServiceProxy;
                ServiceContext svcContext = new ServiceContext(_serviceProxy);

                var queryCase = from c in svcContext.IncidentSet
                            where c.TicketNumber == content.ticketNo
                            select c;

                Incident getGuidIncident = queryCase.FirstOrDefault<Incident>();
                // Incident GUID
                _accountId = new Guid(getGuidIncident.IncidentId.ToString());

                Guid claimId = new Guid();
                // Create
                try
                {
                    pfc_claim claim = new pfc_claim();
                    claim.pfc_claim_number = content.claimNo;
                    claim.pfc_zrepclmno = content.claimNotiNo;
                    claim.pfc_ref_caseId = new Microsoft.Xrm.Sdk.EntityReference(pfc_claim.EntityLogicalName, _accountId);

                    claimId = _serviceProxy.Create(claim);
                }
                catch (Exception e)
                {
                    output.code = "501";
                    output.message = "Create data PROBLEM";
                    output.description = e.ToString();
                    output.transactionId = _transactionId.ToString();
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = null;

                    return output;
                }
                // Update
                try
                {
                    
                    var query = from c in svcContext.IncidentSet
                                where c.TicketNumber == content.ticketNo && c.pfc_claim_noti_number == content.claimNotiNo
                                select c;

                    Incident incident = query.FirstOrDefault<Incident>();

                    _accountId = new Guid(incident.IncidentId.ToString());
                    Incident retrievedIncident = (Incident)_serviceProxy.Retrieve(Incident.EntityLogicalName, _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

                    string DB_claimNumber = retrievedIncident.pfc_claim_number;
                    string DB_claimId = retrievedIncident.pfc_locus_claim_id;
                    string DB_claimStatusCode = retrievedIncident.pfc_locus_claim_status_code;
                    string DB_claimStatusDesc = retrievedIncident.pfc_locus_claim_status_desc;

                    retrievedIncident.pfc_claim_number = content.claimNo;
                    retrievedIncident.pfc_locus_claim_id = content.claimId; 
                    retrievedIncident.pfc_locus_claim_status_code = content.claimStatusCode;
                    retrievedIncident.pfc_locus_claim_status_desc = content.claimStatusDesc;
                    retrievedIncident.pfc_locus_claim_status_on = DateTime.Now;

                    _serviceProxy.Update(retrievedIncident);

                }
                catch (Exception e)
                {
                    output.code = "501";
                    output.message = "(Update data PROBLEM) or (query ticketNo and claimNotiNo and get null value)";
                    output.description = e.ToString();
                    output.transactionId = _transactionId.ToString();
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = null;

                    return output;
                }

                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "Update ClaimNo is done!";
                output.transactionId = _transactionId.ToString();
                output.transactionDateTime = DateTime.Now.ToString();
                UpdateClaimNoOutput.message = "ticketNo: " + content.ticketNo
                    + " claimNotiNo: " + content.claimNotiNo
                    + " claimNo: " + content.claimNo;
                output.data = UpdateClaimNoOutput;
            }

            catch (System.ServiceModel.FaultException e)
            {
                output.code = "500";
                output.message = "False";
                output.description = "CRM PROBLEM";
                output.transactionId = _transactionId.ToString();
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
                output.description = "ไม่พบ ticketNo";
                output.transactionId = _transactionId.ToString();
                output.transactionDateTime = DateTime.Now.ToString();
                output.data = null;

            }

            return output;
        }
    }
}