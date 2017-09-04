using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.UpdateSurveyStatus;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzUpdateSurveyStatus : BuzCommand
    {
        public override BaseDataModel ExecuteInput(object input)
        {
            // Deserialize Input
            UpdateSurveyStatusInputModel contentInput = (UpdateSurveyStatusInputModel)input;

            // Preparation Variable
            UpdateSurveyStatusOutputModel_Pass output = new UpdateSurveyStatusOutputModel_Pass();

            // Preparation Linq query to CRM
            ServiceContext svcContext;
            var _serviceProxy = GetOrganizationServiceProxy(out svcContext);

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

                int statusInput = Int32.Parse(contentInput.iSurveyStatus);
                int statusCRM = incident.pfc_isurvey_status == null ? 0 : (incident.pfc_isurvey_status.Value % 100000000);
                if (statusInput <= statusCRM)
                {
                    output.code = "200";
                    output.message = "Success";
                    output.description = "Update survey status is done! (i-Survey status is not changed)";
                    output.transactionId = TransactionId;
                    output.transactionDateTime = System.DateTime.Now.ToString();
                    output.data = new UpdateSurveyStatusDataOutputModel_Pass();
                    output.data.message = "ClaimNoti Number: " + contentInput.claimNotiNo +
                        " i-Survey status: " + contentInput.iSurveyStatus +
                        " i-Survey status on: " + contentInput.iSurveyStatusOn;

                    return output;
                }

                Incident retrievedIncident = (Incident)_serviceProxy.Retrieve(Incident.EntityLogicalName, _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                try
                {

                    retrievedIncident.pfc_isurvey_status = new OptionSetValue(Int32.Parse(convertOptionSet(Incident.EntityLogicalName, "pfc_isurvey_status", contentInput.iSurveyStatus)));
                    retrievedIncident.pfc_isurvey_status_on = Convert.ToDateTime(contentInput.iSurveyStatusOn);

                    _serviceProxy.Update(retrievedIncident);

                }
                catch (Exception e)
                {
                    output.code = "501";
                    output.message = "False: Retrieving data PROBLEM";
                    output.description = e.Message;
                    output.transactionId = TransactionId;
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = null;

                    return output;
                }

                //TODO: Do something
                output.code = "200";
                output.message = "Success";
                output.description = "Update survey status is done!";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now.ToString();
                output.data = new UpdateSurveyStatusDataOutputModel_Pass();
                output.data.message = "ClaimNoti Number: " + contentInput.claimNotiNo +
                    " i-Survey status: " + contentInput.iSurveyStatus +
                    " i-Survey status on: " + contentInput.iSurveyStatusOn;

                return output;
            }

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
    }
}