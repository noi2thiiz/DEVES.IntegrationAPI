using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.UpdateClaimNo;
using System.Configuration;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Client;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzUpdateClaimNo : BuzCommand
    {
        public override BaseDataModel ExecuteInput(object input)
        {
            // Deserialize Input
            UpdateClaimNoInputModel contentInput = (UpdateClaimNoInputModel)input;

            // Preparation Variable
            UpdateClaimNoOutputModel_Pass output = new UpdateClaimNoOutputModel_Pass();

            // Preparation Linq query to CRM
            var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
            OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
            ServiceContext svcContext = new ServiceContext(_serviceProxy);

            var queryCase = from c in svcContext.IncidentSet
                            where c.TicketNumber == contentInput.ticketNo
                            select c;

            if (queryCase.FirstOrDefault<Incident>() == null)
            {
                output.code = CONST_CODE_FAILED;
                output.message = "ไม่สามารถ Update ได้";
                output.description = "claimNotiNo ไม่มีในระบบ CRM";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now.ToString();

                return output;
            }

            Incident getGuidIncident = queryCase.FirstOrDefault<Incident>();
            // Incident GUID
            Guid _accountId = new Guid(getGuidIncident.IncidentId.ToString());

            Guid claimId = new Guid();

            // Create
            bool isCreate = false;

            var queryClaim = from c in svcContext.pfc_claimSet
                             where c.pfc_claim_number == contentInput.claimNo
                             select c;

            pfc_claim claim = new pfc_claim();

            if (queryClaim.FirstOrDefault<pfc_claim>() == null)
            {

                claim.pfc_claim_number = contentInput.claimNo;
                claim.pfc_zrepclmno = contentInput.claimNotiNo;
                claim.pfc_ref_caseId = new Microsoft.Xrm.Sdk.EntityReference(pfc_claim.EntityLogicalName, _accountId);
                claim.pfc_policy_additional = new Microsoft.Xrm.Sdk.EntityReference(Incident.EntityLogicalName, getGuidIncident.pfc_policy_additionalId.Id);

                claimId = _serviceProxy.Create(claim);

                isCreate = true;
            }
            else
            {
                isCreate = false;
            }

            // Create
            if (isCreate)
            {
                var query = from c in svcContext.IncidentSet
                            where c.TicketNumber == contentInput.ticketNo && c.pfc_claim_noti_number == contentInput.claimNotiNo
                            select c;

                if(query.FirstOrDefault<Incident>() == null)
                {
                    output.code = CONST_CODE_FAILED;
                    output.message = "ไม่สามารถ Create ได้";
                    output.description = "TicketNumber และ claimNotiNo ไม่มีในระบบ CRM";
                    output.transactionId = TransactionId;
                    output.transactionDateTime = DateTime.Now.ToString();

                    return output;
                }

                Incident incident = query.FirstOrDefault<Incident>();

                _accountId = new Guid(incident.IncidentId.ToString());
                Incident retrievedIncident = (Incident)_serviceProxy.Retrieve(Incident.EntityLogicalName, _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

                string DB_claimNumber = retrievedIncident.pfc_claim_number;
                string DB_claimId = retrievedIncident.pfc_locus_claim_id;
                string DB_claimStatusCode = retrievedIncident.pfc_locus_claim_status_code;
                string DB_claimStatusDesc = retrievedIncident.pfc_locus_claim_status_desc;

                retrievedIncident.pfc_claim_number = contentInput.claimNo;
                retrievedIncident.pfc_locus_claim_id = contentInput.claimId;
                retrievedIncident.pfc_locus_claim_status_code = contentInput.claimStatusCode;
                retrievedIncident.pfc_locus_claim_status_desc = contentInput.claimStatusDesc;
                retrievedIncident.pfc_locus_claim_status_on = DateTime.Now;

                _serviceProxy.Update(retrievedIncident);
            }

            //TODO: Do something
            output.code = "200";
            output.message = "Success";
            output.description = "Update ClaimNo is done!";
            output.transactionId = TransactionId;
            output.transactionDateTime = DateTime.Now.ToString();
            output.data = new UpdateClaimNoDataOutputModel_Pass();
            output.data.message = "ticketNo: " + contentInput.ticketNo
                + " claimNotiNo: " + contentInput.claimNotiNo
                + " claimNo: " + contentInput.claimNo;
            
            return output
        }
    }
}