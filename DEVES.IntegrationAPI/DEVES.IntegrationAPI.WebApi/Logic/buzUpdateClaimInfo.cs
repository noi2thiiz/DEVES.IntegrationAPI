using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.UpdateClaimInfo;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzUpdateClaimInfo : BuzCommand
    {
        public override BaseDataModel ExecuteInput(object input)
        {
            // Deserialize Input
            UpdateClaimInfoInputModel contentInput = (UpdateClaimInfoInputModel)input;

            // Preparation Variable
            UpdateClaimInfoOutputModel_Pass output = new UpdateClaimInfoOutputModel_Pass();

            // Preparation Linq query to CRM
            ServiceContext svcContext;
            var _serviceProxy = GetOrganizationServiceProxy(out svcContext);

            var query = from c in svcContext.IncidentSet
                        where c.pfc_claim_noti_number == contentInput.claimNotiNo && c.TicketNumber == contentInput.ticketNo
                        select c;

            if(query.FirstOrDefault<Incident>() == null)
            {
                output.code = CONST_CODE_FAILED;
                output.message = "ไม่สามารถ Update ได้";
                output.description = "claimNotiNo หรือ ticketNo ไม่มีในระบบ CRM";
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

                    retrievedIncident.pfc_locus_claim_status_code = contentInput.claimStatusCode;
                    retrievedIncident.pfc_locus_claim_status_desc = contentInput.claimStatusDesc;
                    retrievedIncident.pfc_customer_vip = convertBool(contentInput.vipCaseFlag); //contentInputInput.vipCaseFlag;
                    retrievedIncident.pfc_high_loss_case_flag = convertBool(contentInput.highLossCaseFlag); //contentInputInput.highLossCaseFlag;
                    retrievedIncident.pfc_legal_case_flag = convertBool(contentInput.LegalCaseFlag); //contentInput.LegalCaseFlag;

                    _serviceProxy.Update(retrievedIncident);

                }
                catch (Exception e)
                {
                    output.code = "501";
                    output.message = "False: Update Problem";
                    output.description = e.Message;
                    output.transactionId = TransactionId;
                    output.transactionDateTime = DateTime.Now.ToString();
                    output.data = null;

                    return output;
                }

                output.code = "200";
                output.message = "Success";
                output.description = "Update claim info is done!";
                output.transactionId = TransactionId;
                output.transactionDateTime = System.DateTime.Now.ToString();
                output.data = new UpdateClaimInfoDataOutputModel_Pass();
                output.data.message = "ClaimNoti Number: " + contentInput.claimNotiNo +
                    " Claim Number: " + contentInput.claimNo +
                    " Claim Status code: " + contentInput.claimStatusCode +
                    " Claim Status Description: " + contentInput.claimStatusDesc;

                return output;
            }
        }

        private string convertOptionSet(object entity, string fieldName, string value)
        {
            string valOption = "10000000" + value;
            return valOption;
        }

        private bool convertBool(string value)
        {
            bool valBool = false;

            if (value.Equals("Y"))
            {
                valBool = true;
            }
            else
            {
                valBool = false;
            }

            return valBool;
        }


    }
}