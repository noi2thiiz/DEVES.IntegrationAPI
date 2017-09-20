using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RequestSurveyor;
using DEVES.IntegrationAPI.Model.EWI;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Controllers;
using DEVES.IntegrationAPI.Model.QuerySQL;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;
using DEVES.IntegrationAPI.WebApi.DataAccessService.QuerySQLAdapter;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzRequestSurveyor : BuzCommand
    {
        private QueryInfo q = new QueryInfo();
        private System.Data.DataTable dt = new System.Data.DataTable();

        public override BaseDataModel ExecuteInput(object input)
        {
            // Deserialize Input
            RequestSurveyorInputModel_WebService contentInput = (RequestSurveyorInputModel_WebService)input;
            // Preparation Variable
            RequestSurveyorDataOutputModel output = new RequestSurveyorDataOutputModel();

            EWIResponseContent_ReqSur iSurveyOutput = new EWIResponseContent_ReqSur();
            try
            {
               
                iSurveyOutput = RequestSurveyorOniSurvey(contentInput.incidentId, contentInput.currentUserId);
                output.eventID = iSurveyOutput.eventid;
                output.errorMessage = iSurveyOutput.errorMessage;
            }
            catch (BuzErrorException e)
            {
                Console.WriteLine(e.StackTrace);
                output.eventID = "";
                output.errorMessage = e.Message;
                if (e.Message.Contains("เลขรับแจ้งมีอยู่แล้วในระบบ"))
                {
                    iSurveyOutput.eventid = "";
                    iSurveyOutput.errorMessage = "ส่งข้อมูลเข้า i-Survey ไม่สำเร็จ: เลขรับแจ้งมีอยู่แล้วในระบบ";
                }
                else
                {
                    iSurveyOutput.eventid = "";
                    iSurveyOutput.errorMessage = "ส่งข้อมูลเข้า i-Survey ไม่สำเร็จ: " + e.Message;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                output.eventID = "";
                output.errorMessage = e.Message;
            }
            

            return output;
        }

        private EWIResponseContent_ReqSur RequestSurveyorOniSurvey(string incidentId, string currentUserId)
        {
            RequestSurveyorInputModel iSurveyInputModel = Mapping(incidentId, currentUserId);
            var service = new MOTORRequestSurveyor(TransactionId, ControllerName);
            var ewiRes = service.ExecuteEWI(iSurveyInputModel);

            EWIResponseContent_ReqSur iSurveyOutput = new EWIResponseContent_ReqSur();

            if (ewiRes.content.ToString().Equals("{}"))
            {
                iSurveyOutput.eventid = "";
                iSurveyOutput.errorMessage = ewiRes.responseMessage;
            }
            else if (ewiRes.responseCode != "EWI-0000I")
            {
                iSurveyOutput.eventid = "";
                iSurveyOutput.errorMessage = "ส่งข้อมูลเข้า i-Survey ไม่สำเร็จ: " + ewiRes.responseMessage.ToString();
            }
            else
            {
                iSurveyOutput.eventid = ewiRes.content.eventId;
                iSurveyOutput.errorMessage = null;
            }
           
            return iSurveyOutput;
        }

        private RequestSurveyorInputModel Mapping(string incidentId, string currentUserId)
        {
            QuerySqlService sql = QuerySqlService.Instance;

            string sqlCommand = string.Format(q.SQL_RequestSurveyor, incidentId, currentUserId).Trim('\n');
            Console.WriteLine(sqlCommand);
            QuerySQLOutputModel mappingOutput = new QuerySQLOutputModel();

            if (AppConst.IS_PRODUCTION)
            {
                 mappingOutput = sql.GetQuery("CRM_MSCRM", sqlCommand);
            } else
            {
                try
                {
                    mappingOutput = sql.GetQuery("CRMQA_MSCRM", sqlCommand);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message+e.StackTrace);
                }
                 
            }
            
            dt = new System.Data.DataTable();
            // dt = q.Queryinfo_RequestSurveyor(incidentId, currentUserId);
            dt = mappingOutput.dt;
            RequestSurveyorInputModel rsModel = new RequestSurveyorInputModel();

            rsModel.CaseID = isStringNull("ticketNunber");
            rsModel.claimNotiNo = isStringNull("EventCode");
            rsModel.claimNotirefer = isStringNull("claimnotirefer");
            rsModel.insureID = isStringNull("InsureID");
            rsModel.rskNo = isStringNull("RSKNo");
            rsModel.tranNo = isStringNull("TranNo");
            rsModel.notifyName = isStringNull("NotifyName");
            rsModel.mobile = isStringNull("Mobile");
            rsModel.driver = isStringNull("Driver");
            rsModel.driverTel = isStringNull("DriverTel");
            rsModel.currentVehicleLicense = isStringNull("current_VehicleLicence");
            rsModel.currentProvince = isStringNull("current_Province");
            rsModel.eventDate = isDateTimeNull(isStringNull("EventDate"));
            rsModel.activityDate = isDateTimeNull(isStringNull("ActivityDate"));
            rsModel.eventDetail = isStringNull("EventDetail");
            rsModel.isCasualty = isStringNull("isCasualty");
            if (rsModel.isCasualty.Equals(""))
            {
                rsModel.isCasualty = "0";
            }
            rsModel.eventLocation = isStringNull("EventLocation");
            rsModel.accidentLocation = isStringNull("accidentLocation");
            rsModel.accidentLat = isStringNull("accidentLat");
            if (rsModel.accidentLat.Equals(""))
            {
                rsModel.accidentLat = "0";
            }
            rsModel.accidentLng = isStringNull("accidentLng");
            if (rsModel.accidentLng.Equals(""))
            {
                rsModel.accidentLng = "0";
            }
            rsModel.ISVIP = isStringNull("IsVIP");
            rsModel.remark = isStringNull("Remark");
            rsModel.claimTypeID = isStringNull("ClameTypeID");
            rsModel.subClaimtypeID = isStringNull("SubClameTypeID");
            rsModel.empCode = isStringNull("informBy");
            rsModel.appointLat = isStringNull("appointLat");
            rsModel.appointLong = isStringNull("appointLong");
            rsModel.appointLocation = isStringNull("appointLocation");
            rsModel.appointDate = isDateTimeNull(isStringNull("appointDate"));
            rsModel.appointName = isStringNull("appointName");
            rsModel.appointPhone = isStringNull("appointPhone");
            rsModel.contractName = isStringNull("contractName");
            rsModel.contractPhone = isStringNull("contractPhone");
            rsModel.surveyTeam = isStringNull("surveyTeam");

            return rsModel;
        }

        protected string isStringNull(string a)
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
        protected int isIntNull(string a)
        {
            if (string.IsNullOrEmpty(dt.Rows[0][a].ToString()))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(dt.Rows[0][a]);
            }
        }
        protected double isDoubleNull(string a)
        {
            if (string.IsNullOrEmpty(dt.Rows[0][a].ToString()))
            {
                return 0;
            }
            else
            {
                return Convert.ToDouble(dt.Rows[0][a]);
            }
        }

        protected DateTime isDateTimeNull(string a)
        {
            string datetime = "";

            if (a.Equals(""))
            {
                datetime = "01/01/1900 0:00:00 AM";
            }
            else
            {
                datetime = a.ToString();
            }

            return Convert.ToDateTime(datetime);
        }

    }
}