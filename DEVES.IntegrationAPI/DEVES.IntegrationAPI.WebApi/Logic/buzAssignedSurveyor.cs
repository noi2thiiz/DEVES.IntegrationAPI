using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.AssignedSurveyor;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzAssignedSurveyor : BuzCommand
    {
        public override BaseDataModel ExecuteInput(object input)
        {
            // Deserialize Input
            AssignedSurveyorInputModel contentInput = (AssignedSurveyorInputModel)input;

            // Preparation Variable
            AssignedSurveyorOutputModel_Pass output = new AssignedSurveyorOutputModel_Pass();

            // Preparation Linq query to CRM
            var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
            OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
            ServiceContext svcContext = new ServiceContext(_serviceProxy);

            // Incident incident = new earlybound.Incident();
            var query = from c in svcContext.IncidentSet
                        where c.pfc_claim_noti_number == contentInput.claimNotiNo
                        select c;

            if (query.FirstOrDefault<Incident>() == null)
            {
                output.code = CONST_CODE_FAILED;
                output.message = "ไม่สามารถ Update ได้";
                output.description = "claimNotiNo ไม่มีในระบบ CRM";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now.ToString();

                return output;
            }
            else
            {

                Incident incident = query.FirstOrDefault<Incident>();
                Guid _accountId = new Guid(incident.IncidentId.ToString());
                Incident retrievedIncident = (Incident)_serviceProxy.Retrieve(Incident.EntityLogicalName, _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

                try
                {

                    retrievedIncident.pfc_survey_meeting_latitude = contentInput.surveyMeetingLatitude;
                    retrievedIncident.pfc_survey_meeting_longitude = contentInput.surveyMeetingLongtitude;
                    retrievedIncident.pfc_survey_meeting_district = contentInput.surveyMeetingDistrict;
                    retrievedIncident.pfc_survey_meeting_province = contentInput.surveyMeetingProvince;
                    retrievedIncident.pfc_survey_meeting_place = contentInput.surveyMeetingPlace;
                    retrievedIncident.pfc_survey_meeting_date = Convert.ToDateTime(contentInput.surveyMeetingDate);
                    retrievedIncident.pfc_surveyor_code = contentInput.surveyorCode;
                    retrievedIncident.pfc_surveyor_client_number = contentInput.surveyorClientNumber;
                    retrievedIncident.pfc_surveyor_name = contentInput.surveyorName;
                    retrievedIncident.pfc_surveyor_company_name = contentInput.surveyorCompanyName;
                    retrievedIncident.pfc_surveyor_company_mobile = contentInput.surveyorCompanyMobile;
                    retrievedIncident.pfc_surveyor_type = new OptionSetValue(Int32.Parse(convertOptionSet(incident, "pfc_surveyor_type", contentInput.surveyType)));
                    retrievedIncident.pfc_surveyor_team = contentInput.surveyTeam;
                    retrievedIncident.pfc_surveyor_mobile = contentInput.surveyorMobile;
                    retrievedIncident.pfc_isurvey_status = new OptionSetValue(Int32.Parse("100000015"));
                    retrievedIncident.pfc_isurvey_status_on = DateTime.Now;

                    _serviceProxy.Update(retrievedIncident);
                }
                catch(Exception e)
                {
                    output.code = CONST_CODE_FAILED;
                    output.message = "ไม่สามารถ Update ได้";
                    output.description = e.Message;
                    output.transactionId = TransactionId;
                    output.transactionDateTime = DateTime.Now.ToString();

                    return output;
                }

                output.code = "200";
                output.message = "Success";
                output.description = "Assigning surveyor is done!";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now.ToString();
                output.data = new AssignedSurveyorDataOutputModel_Pass();
                output.data.message = "ClaimNoti Number: " + contentInput.claimNotiNo +
                    ", Survey type: " + contentInput.surveyType +
                    ", Surveyor code: " + contentInput.surveyorCode +
                    ", Surveyor name: " + contentInput.surveyorName;

                return output;
            }

        }

        private string convertOptionSet(object entity, string fieldName, string value)
        {
            string valOption = "";
            if (value.Equals("0"))
            {
                valOption = "100000001"; // In-house
            }
            else if (value.Equals("1"))
            {
                valOption = "100000002"; // Outsource
            }

            return valOption;
        }

    }
}