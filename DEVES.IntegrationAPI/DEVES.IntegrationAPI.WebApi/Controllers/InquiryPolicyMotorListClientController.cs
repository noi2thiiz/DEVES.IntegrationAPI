using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.InquiryPolicyMotorList;
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
    public class InquiryPolicyMotorListClientController : ApiController
    {

        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(InquiryPolicyMotorListClientController));

        private QueryInfo q = new QueryInfo();
        private System.Data.DataTable dt = new System.Data.DataTable();

        private string isStringNull(string a)
        {
            if (dt.Rows[0][a] == null)
            {
                return null;
            }
            else
            {
                return dt.Rows[0][a].ToString();
            }
        }

        private InquiryPolicyMotorListDataOutputModel_Pass Mapping(string policyNo, string carChassisNo, string carRegisNo, string carRegisProv)
        {
            dt = new System.Data.DataTable();
            dt = q.Queryinfo_InquiryPolicyMotorList(policyNo, carChassisNo, carRegisNo, carRegisProv);

            InquiryPolicyMotorListDataOutputModel_Pass ipModel = new InquiryPolicyMotorListDataOutputModel_Pass();
            ipModel.crmPolicyDetailId = isStringNull("crmPolicyDetailId");
            ipModel.crmPolicyDetailCode = isStringNull("crmPolicyDetailCode");
            ipModel.policyNo = isStringNull("policyNo");
            ipModel.renewalNo = isStringNull("renewalNo");
            ipModel.fleetCarNo = isStringNull("fleetCarNo");
            ipModel.barcode = isStringNull("barcode");
            ipModel.insureCardNo = isStringNull("insureCardNo");
            ipModel.policySeqNo = isStringNull("policySeqNo");
            ipModel.endorseNo = isStringNull("endorseNo");
            ipModel.branchCode = isStringNull("branchCode");
            ipModel.contractType = isStringNull("contractType");
            ipModel.policyProductTypeCode = isStringNull("policyProductTypeCode");
            ipModel.policyProductTypeName = isStringNull("policyProductTypeName");
            ipModel.policyIssueDate = isStringNull("policyIssueDate");
            ipModel.policyEffectiveDate = isStringNull("policyEffectiveDate");
            ipModel.policyExpiryDate = isStringNull("policyExpiryDate");
            ipModel.policyGarageFlag = isStringNull("policyGarageFlag");
            ipModel.policyPaymentStatus = isStringNull("policyPaymentStatus");
            ipModel.policyCarRegisterNo = isStringNull("policyCarRegisterNo");
            ipModel.policyCarRegisterProv = isStringNull("policyCarRegisterProv");
            ipModel.carChassisNo = isStringNull("carChassisNo");
            ipModel.carVehicleType = isStringNull("carVehicleType");
            ipModel.carVehicleModel = isStringNull("carVehicleModel");
            ipModel.carVehicleYear = isStringNull("carVehicleYear");
            ipModel.carVehicleBody = isStringNull("carVehicleBody");
            ipModel.carVehicleSize = isStringNull("carVehicleSize");
            ipModel.policyDeduct = isStringNull("policyDeduct");
            ipModel.agentCode = isStringNull("agentCode");
            ipModel.agentName = isStringNull("agentName");
            ipModel.agentBranch = isStringNull("agentBranch");
            ipModel.insuredCleansingId = isStringNull("insuredCleansingId");
            ipModel.insuredClientId = isStringNull("insuredClientId");
            ipModel.insuredClientType = isStringNull("insuredClientType");
            ipModel.insuredFullName = isStringNull("insuredFullName");
            ipModel.policyStatus = isStringNull("policyStatus");

            return ipModel;
        }

        public object Post([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            var outputPass = new InquiryPolicyMotorListOutputModel_Pass();
            var outputFail = new InquiryPolicyMotorListOutputModel_Fail();
            outputFail.data = new InquiryPolicyMotorListDataOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<InquiryPolicyMotorListInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/InquiryPolicyMotorList_Input_Schema.json");

            if (contentModel.policyNo.Equals(""))
            {
                outputFail.code = "412";
                outputFail.message = "Failed";
                outputFail.description = "Cannot get an infomation from Inquiry Policy Motor List!";
                outputFail.transactionId = contentModel.policyNo;
                outputFail.transactionDateTime = DateTime.Now.ToString();
                string fieldText = "policyNo";
                string messageText = "policyNo must not be null";
                outputFail.data.fieldErrors = fieldText;
                outputFail.data.message = messageText;
                return Request.CreateResponse<InquiryPolicyMotorListOutputModel_Fail>(outputFail);
            }
            if (contentModel.carChassisNo.Equals("") && contentModel.carRegisNo.Equals(""))
            {
                outputFail.code = "412";
                outputFail.message = "Failed";
                outputFail.description = "Cannot get an infomation from Inquiry Policy Motor List!";
                outputFail.transactionId = contentModel.policyNo;
                outputFail.transactionDateTime = DateTime.Now.ToString();
                string fieldText = "carChassisNo or carRegisNo";
                string messageText = "carChassisNo or carRegisNo must not be null";
                outputFail.data.fieldErrors = fieldText;
                outputFail.data.message = messageText;
                return Request.CreateResponse<InquiryPolicyMotorListOutputModel_Fail>(outputFail);
            }
            if (!contentModel.carRegisNo.Equals("") && contentModel.carRegisProv.Equals(""))
            {
                outputFail.code = "412";
                outputFail.message = "Failed";
                outputFail.description = "Cannot get an infomation from Inquiry Policy Motor List!";
                outputFail.transactionId = contentModel.policyNo;
                outputFail.transactionDateTime = DateTime.Now.ToString();
                string fieldText = "carRegisProv";
                string messageText = "carRegisProv must not be null if carRegisNo has vaule";
                outputFail.data.fieldErrors = fieldText;
                outputFail.data.message = messageText;
                return Request.CreateResponse<InquiryPolicyMotorListOutputModel_Fail>(outputFail);
            }


            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new InquiryPolicyMotorListOutputModel_Pass();
                _logImportantMessage += "policyNo: " + contentModel.policyNo;
                outputPass = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<InquiryPolicyMotorListOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new InquiryPolicyMotorListOutputModel_Fail();
                outputFail.data = new InquiryPolicyMotorListDataOutputModel_Fail();

                outputFail.message = "Failed";
                outputFail.description = "Cannot get an infomation from Inquiry Policy Motor List!";
                outputFail.transactionId = contentModel.policyNo;
                outputFail.transactionDateTime = DateTime.Now.ToString();


                if (contentModel.policyNo.Length >= 50 || contentModel.carChassisNo.Length >= 30 || contentModel.carRegisNo.Length >= 20 || contentModel.carRegisProv.Length >= 5)
                {
                    outputFail.code = "411";
                    string fieldText = "";
                    string messageText = "";
                    if (contentModel.policyNo.Length >= 50)
                    {
                        fieldText += "policyNo, ";
                        messageText += "length of policyNo must not over than 50, ";
                    }
                    if (contentModel.carChassisNo.Length >= 30)
                    {
                        fieldText += "carChassisNo, ";
                        messageText += "length of carChassisNo must not over than 30, ";
                    }
                    if (contentModel.carRegisNo.Length >= 20)
                    {
                        fieldText += "carRegisNo, ";
                        messageText += "length of carRegisNo must not over than 20, ";
                    }
                    if (contentModel.carRegisProv.Length >= 5)
                    {
                        fieldText += "carRegisProv, ";
                        messageText += "length of carRegisProv must not over than 5, ";
                    }

                    string lastFieldText = "";
                    string lastMessageText = "";

                    if (fieldText.LastIndexOf(" ") == fieldText.Length-1) 
                    {
                        lastFieldText = fieldText.Substring(0, fieldText.Length - 2);
                    }
                    if (messageText.LastIndexOf(" ") == messageText.Length - 1)
                    {
                        lastMessageText = messageText.Substring(0, messageText.Length - 2);
                    }
                    outputFail.data.fieldErrors = lastFieldText;
                    outputFail.data.message = lastMessageText;

                    return Request.CreateResponse<InquiryPolicyMotorListOutputModel_Fail>(outputFail);
                }

                var dataFail = outputFail.data;
                dataFail.fieldErrors = "Invalid type of Input(s)";
                dataFail.message = "Some of your input is invalid. Please recheck again.";

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", dataFail.fieldErrors, Environment.NewLine, dataFail.message);
                return Request.CreateResponse<InquiryPolicyMotorListOutputModel_Fail>(outputFail);
            }
        }

        private InquiryPolicyMotorListOutputModel_Pass HandleMessage(string valueText, InquiryPolicyMotorListInputModel content)
        {
            //TODO: Do what you want
            var output = new InquiryPolicyMotorListOutputModel_Pass();

            _log.Info("HandleMessage");
            try
            {
                var InquiryPolicyMotorListOutput = new InquiryPolicyMotorListDataOutputModel_Pass();
                InquiryPolicyMotorListOutput = Mapping(content.policyNo, content.carChassisNo, content.carRegisNo, content.carRegisProv);
                
                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "Inquiry Policy Motor List is done!";
                output.transactionId = content.policyNo;
                output.transactionDateTime = DateTime.Now.ToString();
                output.data = InquiryPolicyMotorListOutput;
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
                output.description = "ไม่พบ policyNo";

            }

            return output;
        }

    }
}