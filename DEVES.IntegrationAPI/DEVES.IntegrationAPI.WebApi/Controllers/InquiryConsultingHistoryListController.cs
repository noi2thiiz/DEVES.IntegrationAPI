using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.Enum;
using DEVES.IntegrationAPI.Model.Input;
using DEVES.IntegrationAPI.Model.Output;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class InquiryConsultingHistoryListController : ApiController
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(InquiryConsultingHistoryListController));

        public object Post([FromBody]object value)
        {
            _log.Info("HttpMethod: POST");
            _log.InfoFormat("IP ADDRESS: {0}", GetIpAddress());

            var output = new OutputModel();

            if (!TryValidateNullMessage(value, out output))
            {
                return Request.CreateResponse<OutputModel>(output);
            }

            var valueText = value.ToString();
            var crmApiLog = new CrmApiLog();

            string outvalidate = string.Empty;
            if (TryValidateJson(value.ToString(), out outvalidate))
            {
                var entityRef = crmApiLog.Create(valueText);
                output = HandleMessage(value.ToString(), HttpMethodType.POST, entityRef);
            }
            else
            {
                var entityRef = crmApiLog.Create(valueText);
                output.ErrorCode = (int)ApiResultType.InvalidMessage;
                output.ErrorDescription = string.Format("Invalid message: {0}", outvalidate);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", output.ErrorCode, Environment.NewLine, output.ErrorDescription);
            }
            return Request.CreateResponse<OutputModel>(output);
        }

        private OutputModel HandleMessage(string valueText, HttpMethodType httpMethod, EntityReference entityRef)
        {
            var apiRequestId = string.Empty;
            var output = new OutputModel();
            _log.Info("HandleMessage");
            var sourceSystemName = string.Empty;
            var sourceSubSystemName = string.Empty;
            var requestId = string.Empty;
            var resultCode = 0;
            var resultDetail = string.Empty;
            var errorCode = 0;
            var errorDescription = string.Empty;
            //Create to CRM
            try
            {
                var dataModel = JsonConvert.DeserializeObject<InquiryConsultingHistoryListModel>(valueText);
                apiRequestId = dataModel.RequestId;
                sourceSystemName = dataModel.SourceSystemName;
                sourceSubSystemName = dataModel.SourceSubSystemName;
                requestId = dataModel.RequestId;
                dataModel.HttpMethod = httpMethod;

                if (dataModel.SourceSystemName.ToLower() == "contactcenter")
                {
                    CRMInput crmApiInput = new CRMInput(dataModel);
                    crmApiInput.Start(entityRef);

                    resultCode = (int)ApiResultType.OK;
                    resultDetail = "Ok";

                    _log.InfoFormat("ResultCode: {0} {1} ResultDetail: {1}", output.ResultCode, Environment.NewLine, output.ResultDetail);
                }
                else if (dataModel.SourceSystemName.ToLower() == "pruksa.com")
                {
                    CRMInput crmApiInput = new CRMInput(dataModel);
                    crmApiInput.Start(entityRef);

                    resultCode = (int)ApiResultType.OK;
                    resultDetail = "Ok";

                    _log.InfoFormat("ResultCode: {0} {1} ResultDetail: {1}", output.ResultCode, Environment.NewLine, output.ResultDetail);
                }
                else
                {
                    resultCode = (int)ApiResultType.OK;
                    resultDetail = "Ok";
                    _log.Info("Pruksa.com");
                }
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
                _log.Error("RequestId - " + apiRequestId);
                _log.Error(errorMessage);
                errorCode = (int)ApiResultType.InternalProcessError;
                errorDescription = ApiResultType.InternalProcessError.ToString();
            }

            output = new OutputModel()
            {
                RequestId = requestId,
                ResultCode = resultCode,
                ResultDetail = resultDetail,
                ErrorCode = errorCode,
                ErrorDescription = errorDescription,
                SourceSystem = sourceSystemName,
                SourceSubSystemName = sourceSubSystemName
            };

            return output;
        }

        private bool TryValidateNullMessage(object value, out OutputModel output)
        {
            output = new OutputModel();
            if (value == null)
            {
                output.ErrorCode = (int)ApiResultType.NoMessage;
                output.ErrorDescription = "There is no message.";
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {2}", output.ErrorCode, Environment.NewLine, output.ErrorDescription);
                return false;
            }
            return true;
        }

        private bool TryValidateJson(string jsontext, out string output)
        {
            var validatedText = "TryValidateJson: {0}";
            output = string.Empty;
            try
            {
                var schemaType = GetSchemaType(jsontext);
                var schemaPath = string.Format("~/App_Data/JsonSchema/{0}.json", schemaType);
                _log.InfoFormat(validatedText, string.Empty);
                var filePath = HttpContext.Current.Server.MapPath(schemaPath);
                var schemaText = FileHelper.ReadTextFile(filePath);
                if (string.IsNullOrEmpty(schemaText))
                {
                    schemaText = "{\"$schema\":\"http://json-schema.org/draft-04/schema#\",\"definitions\":{\"telephone\":{\"type\":\"string\",\"maxLength\":200},\"dateTime\":{\"type\":\"string\",\"pattern\":\"[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2}\",\"minLength\":19,\"maxLength\":19},\"appointment\":{\"type\":\"object\",\"properties\":{\"Id\":{\"type\":\"string\",\"maxLength\":30},\"ContactType\":{\"type\":\"string\",\"enum\":[\"INBOUND\",\"OUTBOUND\"],\"maxLength\":30},\"Type\":{\"type\":\"string\",\"enum\":[\"CALL\",\"VISIT\",\"EMAIL\",\"SMS\",\"MAIL\",\"VISITPOSTPONED\",\"VISITCANCEL\",\"VISITED\"],\"maxLength\":30},\"Memo\":{\"anyOf\":[{\"type\":\"string\",\"maxLength\":4000},{\"type\":\"null\"}]},\"DateTime\":{\"$ref\":\"#/definitions/dateTime\"},\"Status\":{\"type\":\"string\",\"enum\":[\"NEW\",\"CLOSED\"],\"maxLength\":30}},\"required\":[\"Type\",\"ContactType\"]},\"serviceHistory\":{\"type\":\"object\",\"properties\":{\"Id\":{\"type\":\"string\",\"maxLength\":30},\"TypeCode\":{\"type\":\"integer\"},\"OrderNo\":{\"type\":\"integer\"},\"CallingTransferRecipientType\":{\"anyOf\":[{\"type\":\"string\",\"enum\":[\"SALES\",\"VENDOR\",\"OPERATOR\",\"UNKNOWN\"]},{\"type\":\"null\"}]},\"CallingTransferTelephoneNo\":{\"anyOf\":[{\"$ref\":\"#/definitions/telephone\"},{\"type\":\"null\"}]},\"CallingRecipientName\":{\"anyOf\":[{\"type\":\"string\",\"maxLength\":200},{\"type\":\"null\"}]},\"CallingTransferResult\":{\"type\":\"boolean\"},\"CallingTransferStatus\":{\"type\":\"string\",\"maxLength\":50,\"enum\":[\"ติดต่อสำเร็จส่ง SMS\",\"ติดต่อประสานงานสำเร็จ\",\"ติดต่อได้สำเร็จ(ปิดงานสำเร็จ)\",\"ส่ง SMS หรือ ให้ข้อมูลลูกค้า\"]},\"Detail\":{\"anyOf\":[{\"type\":\"string\",\"maxLength\":500},{\"type\":\"null\"}]},\"CreatedUser\":{\"anyOf\":[{\"type\":\"string\",\"maxLength\":200},{\"type\":\"null\"}]},\"LatestModifiedUser\":{\"anyOf\":[{\"type\":\"string\",\"maxLength\":200},{\"type\":\"null\"}]},\"CreatedDateTime\":{\"$ref\":\"#/definitions/dateTime\"}},\"required\":[\"Id\",\"TypeCode\",\"OrderNo\",\"CallingTransferResult\",\"CallingTransferStatus\",\"CreatedUser\",\"LatestModifiedUser\",\"CreatedDateTime\"]},\"inquiry\":{\"type\":\"object\",\"properties\":{\"Type\":{\"type\":\"string\",\"enum\":[\"NONE\",\"LANDOFFER\",\"REQUESTPROJECTINFO\",\"PRODUCTSERVICEOFFER\",\"BILLBOARDOFFER\",\"ONLINEMEDIAOFFER\",\"SUGGESTIONS\",\"OTHERS\"]},\"InboundPhoneNo\":{\"$ref\":\"#/definitions/telephone\"},\"SmsPhoneNo\":{\"anyOf\":[{\"$ref\":\"#/definitions/telephone\"},{\"type\":\"null\"}]},\"InboundType1Code\":{\"type\":\"integer\"},\"InboundType2Code\":{\"type\":\"integer\"},\"InboundType3Code\":{\"anyOf\":[{\"type\":\"integer\"},{\"type\":\"null\"}]},\"ContactTypeCode\":{\"type\":\"integer\"},\"MediaTypeCode\":{\"anyOf\":[{\"type\":\"integer\"},{\"type\":\"null\"}]},\"MediaDetail\":{\"anyOf\":[{\"type\":\"string\",\"maxLength\":200},{\"type\":\"null\"}]},\"CommunicationChannel\":{\"anyOf\":[{\"type\":\"string\",\"enum\":[\"โทรศัพท์\",\"Outbound\",\"Leave Message\",\"Line@\",\"ddProperty\",\"Email\",\"Fax\",\"Live Chat\",\"Facebook\",\"Internet\",\"สำนักงานกรรมการผู้จัดการ\",\"CG\",\"DotProperty\"],\"maxLength\":200},{\"type\":\"null\"}]},\"CommunicationChannelOther\":{\"anyOf\":[{\"type\":\"string\",\"maxLength\":200},{\"type\":\"null\"}]},\"MediaCode\":{\"anyOf\":[{\"type\":\"integer\"},{\"type\":\"null\"}]},\"ProjectInterestLevel\":{\"type\":\"string\",\"enum\":[\"20\",\"50\",\"100\"]},\"ContactExplanation\":{\"type\":\"string\",\"maxLength\":8000},\"Detail\":{\"type\":\"string\",\"maxLength\":8000}},\"required\":[\"Type\",\"InboundPhoneNo\",\"InboundType1Code\",\"InboundType2Code\",\"ContactTypeCode\",\"ContactExplanation\",\"Detail\"]}},\"type\":\"object\",\"properties\":{\"AsOfDate\":{\"$ref\":\"#/definitions/dateTime\"},\"RequestDate\":{\"$ref\":\"#/definitions/dateTime\"},\"RequestId\":{\"type\":\"string\",\"maxLength\":30},\"SourceSystemName\":{\"type\":\"string\",\"maxLength\":200},\"SourceSubSystemName\":{\"type\":\"string\",\"maxLength\":200},\"ServiceId\":{\"type\":\"string\",\"maxLength\":30},\"CustomerId\":{\"type\":\"string\",\"maxLength\":200},\"CustomerType\":{\"type\":\"string\",\"enum\":[\"ลูกค้าทั่วไป\",\"นิติบุคคลโครงการ\",\"เจ้าหน้าที่พฤกษา\",\"ช่างโครงการ\",\"ลูกค้า VIP\",\"ผู้บริหารพฤกษา\",\"ลูกค้า Special\"],\"maxLength\":50},\"Email\":{\"type\":\"string\",\"maxLength\":200},\"Salutation\":{\"type\":\"string\",\"enum\":[\"นาย\",\"นาง\",\"นางสาว\"],\"maxLength\":30},\"FirstName\":{\"type\":\"string\",\"maxLength\":200},\"LastName\":{\"type\":\"string\",\"maxLength\":200},\"Gender\":{\"type\":\"string\",\"enum\":[\"MALE\",\"FEMALE\",\"UNDEFINED\"],\"maxLength\":30},\"Language\":{\"type\":\"string\",\"enum\":[\"THAI\",\"ENGLISH\",\"CHINESE\",\"OTHER\"],\"maxLength\":15},\"Telephone\":{\"$ref\":\"#/definitions/telephone\"},\"MobilePhone\":{\"$ref\":\"#/definitions/telephone\"},\"AgentId\":{\"type\":\"string\",\"maxLength\":200},\"IsReceivedNews\":{\"type\":\"boolean\"},\"NewsDetail\":{\"anyOf\":[{\"type\":\"string\",\"maxLength\":500},{\"type\":\"null\"}]},\"Remark\":{\"anyOf\":[{\"type\":\"string\",\"maxLength\":500},{\"type\":\"null\"}]},\"ServiceStatus\":{\"type\":\"string\",\"enum\":[\"กำลังดำเนินการ\",\"ปิด\"]},\"IsEndCallSurvey\":{\"type\":\"boolean\"},\"EndCallReason\":{\"type\":\"string\",\"enum\":[\"ลูกค้าไม่สะดวกประเมิน\",\"ลูกค้าวางสายไปทันทีเมื่อได้รับข้อมูลเรียบร้อยแล้ว\",\"ลูกค้าสอบถามมากว่า 1 รายการ\",\"โอนสายระหว่างศูนย์ OTO และ GSC\",\"โอนสายไป สนง. ขาย /สำนักงานใหญ่/ผู้รับเหมา\",\"งานโทร Outbound ประเภทต่างๆ\",\"พนักงานพฤกษาติดต่อเข้ามาเพื่อให้ 1739 ประสานงานลูกค้า\",\"รับเรื่องจากช่องทาง Online\",\"ลูกค้าสาย  Eng\",\"โทรผิด/โรคจิต\"]},\"ServicesHistories\":{\"anyOf\":[{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/serviceHistory\"},\"minItems\":0},{\"type\":\"null\"}]},\"Appointment\":{\"anyOf\":[{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/appointment\"}},{\"type\":\"null\"}]},\"Projects\":{\"anyOf\":[{\"type\":\"array\",\"properties\":{\"SBU\":{\"type\":\"string\",\"maxLength\":30},\"Brand\":{\"type\":\"string\",\"maxLength\":200},\"ProjectCode\":{\"type\":\"string\",\"maxLength\":10},\"ProjectNameThai\":{\"anyOf\":[{\"type\":\"string\",\"maxLength\":200},{\"type\":\"null\"}]},\"ProjectNameEng\":{\"anyOf\":[{\"type\":\"string\",\"maxLength\":200},{\"type\":\"null\"}]},\"ResidentType\":{\"type\":\"string\",\"enum\":[\"CONDO\",\"TOWNHOUSE\",\"SINGLEDETACHEDHOUSE\"],\"maxLength\":20},\"Inquiry\":{\"anyOf\":[{\"$ref\":\"#/definitions/inquiry\"},{\"type\":\"null\"}]},\"Appointment\":{\"anyOf\":[{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/appointment\"}},{\"type\":\"null\"}]}},\"required\":[\"SBU\",\"Brand\",\"ProjectCode\",\"ResidentType\"]},{\"type\":\"null\"}]}},\"required\":[\"AsOfDate\",\"RequestDate\",\"RequestId\",\"SourceSystemName\",\"SourceSubSystemName\",\"CustomerId\",\"CustomerType\",\"Email\",\"Salutation\",\"FirstName\",\"LastName\",\"Gender\",\"Language\",\"Telephone\",\"MobilePhone\",\"AgentId\",\"IsReceivedNews\",\"ServiceStatus\",\"IsEndCallSurvey\"]}";
                }

                var schema = JSchema.Parse(schemaText);
                var jsonObj = JObject.Parse(jsontext);
                IList<string> errorMessages;
                bool valid = jsonObj.IsValid(schema, out errorMessages);
                _log.InfoFormat(validatedText, valid);
                output = valid.ToString() + Environment.NewLine;
                if (!valid)
                {
                    _log.ErrorFormat(validatedText, jsontext);
                }
                foreach (var errorMessage in errorMessages)
                {
                    output += errorMessage + Environment.NewLine;
                    _log.WarnFormat(validatedText, errorMessage);
                }
                return valid;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat(validatedText, ex.Message);
                _log.ErrorFormat(validatedText, ex.StackTrace);
                return false;
            }
        }

        private string GetIpAddress()
        {
            string ipaddress;
            ipaddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipaddress))
                ipaddress += HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            return ipaddress;
        }
    }
}