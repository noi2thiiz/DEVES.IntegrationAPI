using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.ClaimRegistration;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class BuzClaimRegistrationCommand: BaseCommand
    {
        const string sqlcmd_Get_RegClaimInfo = "sp_CustomApp_RegClaimInfo_Incident";
        const string ewiEndpointKeyClaimRegistration = "EWI_ENDPOINT_ClaimRegistration";

        public override BaseDataModel Execute( object value)
        {
            //+ Deserialize Input
            ClaimRegistrationInputModel contentModel = DeserializeJson<ClaimRegistrationInputModel>(value.ToString());

            //+ Prepare input data model
            LocusClaimRegistrationInputModel data = new LocusClaimRegistrationInputModel();
            data.claimHeader = new LocusClaimheaderModel();
            data.claimAssignSurv = new LocusClaimassignsurvModel();
            data.claimInform = new LocusClaiminformModel();
            data.claimSurvInform = new LocusClaimsurvinformModel();
            BaseDataModel input = data;

            //+ Call SQL to get data
            List<CommandParameter> listParam = new List<CommandParameter>();
            listParam.Add(new CommandParameter("incidentId", contentModel.IncidentId ));
            listParam.Add(new CommandParameter("CurrentUserId", contentModel.CurrentUserId ));
            FillModelUsingSQL(ref input , sqlcmd_Get_RegClaimInfo, listParam);

            //+ Call Locus_RegisterClaim through ServiceProxy
            string uid = GetDomainName(contentModel.CurrentUserId);
            Model.EWI.EWIResponseContent ret = CallEWIService(ewiEndpointKeyClaimRegistration, input, uid);
            LocusClaimRegistrationDataOutputModel locusClaimRegOutput = new LocusClaimRegistrationDataOutputModel(ret.data);

            //+ Update CRM
            using (OrganizationServiceProxy crmSvc = GetCrmServiceProxy())
            {
                crmSvc.EnableProxyTypes();

                pfc_claim claim = new pfc_claim();
                claim.pfc_claim_number = locusClaimRegOutput.claimNo;
                claim.pfc_zrepclmno = data.claimHeader.claimNotiNo;
                claim.pfc_ref_caseId = new Microsoft.Xrm.Sdk.EntityReference(Incident.EntityLogicalName, contentModel.IncidentId);

                //crmSvc.Create(claim);

                Incident crmCase = new Incident
                {
                    IncidentId = contentModel.IncidentId,
                    pfc_locus_claim_id = locusClaimRegOutput.claimId,
                    pfc_locus_claim_status_on = DateTime.Now,
                    pfc_locus_claim_status_code = "fixed",
                    pfc_locus_claim_status_desc = "fixed",
                    pfc_claim_number = locusClaimRegOutput.claimNo
                };

                ExecuteTransactionRequest tranReq = new ExecuteTransactionRequest()
                {
                    Requests = new OrganizationRequestCollection(),
                    ReturnResponses = true
                };

                CreateRequest createClaimReq = new CreateRequest() { Target = claim };
                tranReq.Requests.Add(createClaimReq);
                UpdateRequest updateCaseReq = new UpdateRequest { Target = crmCase };
                tranReq.Requests.Add(updateCaseReq);
                ExecuteTransactionResponse tranRes = (ExecuteTransactionResponse)crmSvc.Execute(tranReq); 

                //using (var serviceContext = new ServiceContext(crmSvc))
                //{
                //    serviceContext.Attach(crmCase);
                //    serviceContext.UpdateObject(crmCase);
                //    serviceContext.SaveChanges();
                //}

            }

            //+ Response
            ClaimRegistrationOutputModel output = new ClaimRegistrationOutputModel();
            output.claimID = locusClaimRegOutput.claimId;
            output.claimNo = locusClaimRegOutput.claimNo;
            return output;
        }
    }
}