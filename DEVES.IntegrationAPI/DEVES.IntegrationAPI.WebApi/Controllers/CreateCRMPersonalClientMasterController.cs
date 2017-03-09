using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.CreateCRMPersonalClientMaster;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.Crm.Sdk;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using System.Linq;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class CreateCRMPersonalClientMasterController : ApiController
    {

        private string _logImportantMessage;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(CreateCRMPersonalClientMasterController));


        /**
         *  Method สำหรับรับค่าที่จะยิงมาใน [Postman, Web service (ที่จะสร้างในอนาคต), หรือ จากอีกฝั่ง] ครับ
         **/
        public object Post([FromBody]object value)
        {

            _log.InfoFormat("IP ADDRESS: {0}, HttpMethod: POST", CommonHelper.GetIpAddress());

            // อิงตาม spec จะมีรูปแบบ output 2 แบบครับ (pass or fail) ก็เลยต้องกำหนด output ไว้ 2 แบบ
            var outputPass = new CreateCRMPersonalClientMasterOutputModel_Pass();
            var outputFail = new CreateCRMPersonalClientMasterOutputModel_Fail();

            // การรับค่า input เก็บในตัวแปร contentText
            var contentText = value.ToString();
            // ตัวนี้จริง ๆ ถ้าเรายิงค่าจาก [Postman, Web service] นั่นจะไม่จำเป็นครับ แต่ถ้าในอนาคดเขาส่งมาในรูปแบบ Json จะต้องทำการ Deserialize Json format ก่อน
            var contentModel = JsonConvert.DeserializeObject<CreateCRMPersonalClientMasterInputModel>(contentText);

            string outvalidate = string.Empty;
            // map file path ที่จะเข้าไปเช็ค output เรากับไฟล์ Schema ที่เราสร้างครับ
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/CreateCRMPersonalClientMaster_Input_Schema.json");

            // Method สำหรับ Validate Input ว่าตรงตาม Format Schema หรือป่าว
            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new CreateCRMPersonalClientMasterOutputModel_Pass();
                _logImportantMessage += "TicketNo: " + contentModel.generalHeader;
                // เรียก Method HandleMessage สำหรับจัดการค่า output ถ้าหาก Validate Json ผ่าน
                outputPass = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<CreateCRMPersonalClientMasterOutputModel_Pass>(outputPass);
            }
            else
            {
                // จะใช้งานหาก validate json ไม่ผ่าน หรือก็คือ input ที่อีกฝั่งส่งมามีค่าที่ผิดจาก Schema ครับ

                outputFail = new CreateCRMPersonalClientMasterOutputModel_Fail();
                outputFail.data = new CreateCRMPersonalClientMasterDataOutputModel_Fail();
                var dataFail = outputFail.data;
                dataFail.name = "Invalid Input(s)";
                dataFail.message = "Some of your input is invalid. Please recheck again.";
                // dataFail.fieldError = "Field xxx is ";

                _log.Error(_logImportantMessage);
                _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", dataFail.name, Environment.NewLine, dataFail.message);
                return Request.CreateResponse<CreateCRMPersonalClientMasterDataOutputModel_Fail>(dataFail);
            }
        }

        /**
         *  Method สำหรับการจัดการค่า Output ครับ
         *  parameter ที่รับมาเป็น Input จะต้องเอาไป set value ใน output บางตัวครับ
         **/
        private CreateCRMPersonalClientMasterOutputModel_Pass HandleMessage(string valueText, CreateCRMPersonalClientMasterInputModel content)
        {
            var output = new CreateCRMPersonalClientMasterOutputModel_Pass();
            _log.Info("HandleMessage");

            try
            {
                var CreateCRMPersonalClientMasterOutput = new CreateCRMPersonalClientMasterDataOutputModel_Pass();
                // map Input ไป set value ใน output 
                CreateCRMPersonalClientMasterOutput.cleasingId = content.generalHeader.cleansingId;
                CreateCRMPersonalClientMasterOutput.clientId = content.generalHeader.clientId;
                CreateCRMPersonalClientMasterOutput.sapId = content.generalHeader.sapId;
                CreateCRMPersonalClientMasterOutput.salutationText = content.profileInfo.salutation;
                CreateCRMPersonalClientMasterOutput.personalName = content.profileInfo.personalName;
                CreateCRMPersonalClientMasterOutput.personalSurname = content.profileInfo.personalSurname;

                output.data = CreateCRMPersonalClientMasterOutput;
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
                output.description = "ไม่พบ claimNotiNo";
            
            }
            return output;

        }
    }

    

}