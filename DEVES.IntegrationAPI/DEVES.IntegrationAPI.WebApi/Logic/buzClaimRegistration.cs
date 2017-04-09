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

        public override BaseDataModel Execute(object input)
        {
            //+ Deserialize Input
            ClaimRegistrationInputModel contentModel = DeserializeJson<ClaimRegistrationInputModel>(input.ToString());

            //+ Prepare input data model
            LocusClaimRegistrationInputModel data = new LocusClaimRegistrationInputModel();
            data.claimHeader = new LocusClaimheaderModel();
            data.claimAssignSurv = new LocusClaimassignsurvModel();
            data.claimInform = new LocusClaiminformModel();
            data.claimSurvInform = new LocusClaimsurvinformModel();
            BaseDataModel inputData = data;

            //+ Call SQL to get data
            List<CommandParameter> listParam = new List<CommandParameter>();
            listParam.Add(new CommandParameter("incidentId", contentModel.IncidentId ));
            listParam.Add(new CommandParameter("CurrentUserId", contentModel.CurrentUserId ));
            FillModelUsingSQL(ref inputData, CommonConstant.sqlcmd_Get_RegClaimInfo, listParam);

            //+ Call Locus_RegisterClaim through ServiceProxy
            string uid = GetDomainName(contentModel.CurrentUserId);
            Model.EWI.EWIResponseContent ret = (Model.EWI.EWIResponseContent)CallDevesJsonProxy<Model.EWI.EWIResponse>(CommonConstant.ewiEndpointKeyLOCUSClaimRegistration, inputData, uid);
            LocusClaimRegistrationDataOutputModel locusClaimRegOutput = new LocusClaimRegistrationDataOutputModel(ret.data);
        
            
            if(locusClaimRegOutput.claimNo == null || locusClaimRegOutput.claimId == null)
            {
                //+ Response
                ClaimRegistrationContentOutputModel contentOutputFail = new ClaimRegistrationContentOutputModel();
                contentOutputFail.data = new List<ClaimRegistrationOutputModel>();
                ClaimRegistrationOutputModel outputFail = new ClaimRegistrationOutputModel();
                outputFail.claimID = locusClaimRegOutput.claimId;
                outputFail.claimNo = locusClaimRegOutput.claimNo;
                outputFail.errorMessage = "";

                if (locusClaimRegOutput.claimNo == null || locusClaimRegOutput.claimId == null)
                {
                    outputFail.errorMessage = "claimNo and claimId are null";
                }
                else if (locusClaimRegOutput.claimNo == null)
                {
                    outputFail.errorMessage = "claimNo is null";
                } 
                else if (locusClaimRegOutput.claimId == null)
                {
                    outputFail.errorMessage = "claimId is null";
                }
                
                contentOutputFail.data.Add(outputFail);
                return contentOutputFail;
            }

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
            ClaimRegistrationContentOutputModel contentOutput = new ClaimRegistrationContentOutputModel();
            contentOutput.data = new List<ClaimRegistrationOutputModel>();
            ClaimRegistrationOutputModel output = new ClaimRegistrationOutputModel();
            output.claimID = locusClaimRegOutput.claimId;
            output.claimNo = locusClaimRegOutput.claimNo;
            output.errorMessage = null;
            contentOutput.data.Add(output);
            return contentOutput;
        }
    }
}