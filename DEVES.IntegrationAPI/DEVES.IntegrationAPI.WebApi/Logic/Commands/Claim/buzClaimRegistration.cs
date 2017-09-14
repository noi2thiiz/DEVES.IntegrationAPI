﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.ClaimRegistration;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class BuzClaimRegistrationCommand: BuzCommand
    {

        public override BaseDataModel ExecuteInput(object input)
        {
            //+ Deserialize Input
            ClaimRegistrationInputModel contentModel = (ClaimRegistrationInputModel)input;

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

            // Convert BaseDataModel tobe LocusInput and fix some variable (in case of "claimType = "O" <เตลมแห้ง>)
            data = (LocusClaimRegistrationInputModel)inputData;
            // transform some data that is required from polisy400
            if (data.claimHeader.informByCrmId == null)
            {
                data.claimHeader.informByCrmId = data.claimHeader.submitByCrmId;
            }
            if (data.claimHeader.informByCrmName == null)
            {
                data.claimHeader.informByCrmName = data.claimHeader.submitByCrmName;
            }
            if (data.claimInform.informerOn == null)
            {
                data.claimInform.informerOn = DateTime.Now;
            }
            if (data.claimAssignSurv.surveyTeam == null)
            {
                data.claimAssignSurv.surveyTeam = "TEAM0099";
            }
            /**
             * Force value if informerClientId, driverClientId, insuredClientId to be "88888888"
             
            data.claimInform.informerClientId = "88888888";
            data.claimInform.driverClientId = "88888888";
            data.claimInform.insuredClientId = "88888888";
            **/
            /*
            if (data.claimAssignSurv.surveyorClientNumber == null)
            {
                data.claimAssignSurv.surveyorClientNumber = "16960747";
            }
            if (data.claimAssignSurv.surveyorName == null)
            {
                data.claimAssignSurv.surveyorName = "พนักงานเทเวศออกตรวจ";
            }
            */
            inputData = data;
            
            //+ Call Locus_RegisterClaim through ServiceProxy
            
            string uid = GetDomainName(contentModel.CurrentUserId);
            LocusClaimRegistrationContentOutputModel ret = new LocusClaimRegistrationContentOutputModel();
            try
            {
                var service = new LOCUSClaimRegistrationService(TransactionId, ControllerName);
                ret = service.Execute(inputData);



                //   Model.EWI.EWIResponseContent ret = (Model.EWI.EWIResponseContent)CallDevesJsonProxy<Model.EWI.EWIResponse>
                //       (CommonConstant.ewiEndpointKeyLOCUSClaimRegistration, inputData, uid);


                if (ret?.data == null)
                {

                    //+ Response
                    ClaimRegistrationContentOutputModel contentOutputFail = new ClaimRegistrationContentOutputModel();
                    contentOutputFail.data = new ClaimRegistrationDataOutputModel();
                    ClaimRegistrationDataOutputModel outputFail = new ClaimRegistrationDataOutputModel();
                    outputFail.claimID = null;
                    outputFail.claimNo = null;
                    outputFail.errorMessage = ret.message;
                    // contentOutputFail.data.Add(outputFail);

                    return outputFail;
                }

                LocusClaimRegistrationDataOutputModel locusClaimRegOutput = new LocusClaimRegistrationDataOutputModel(ret.data);


                if (locusClaimRegOutput.claimNo == null || locusClaimRegOutput.claimId == null)
                {
                    //+ Response
                    ClaimRegistrationContentOutputModel contentOutputFail = new ClaimRegistrationContentOutputModel();
                    contentOutputFail.data = new ClaimRegistrationDataOutputModel();
                    ClaimRegistrationDataOutputModel outputFail = new ClaimRegistrationDataOutputModel();
                    outputFail.claimID = locusClaimRegOutput.claimId;
                    outputFail.claimNo = locusClaimRegOutput.claimNo;
                    outputFail.errorMessage = ret.message;

                    /*
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
                    */

                    // contentOutputFail.data.Add(outputFail);
                    return outputFail;
                }

                //+ Update CRM
                using (OrganizationServiceProxy crmSvc = GetCrmServiceProxy())
                {
                    crmSvc.EnableProxyTypes();

                    pfc_claim claim = new pfc_claim();
                    claim.pfc_claim_number = locusClaimRegOutput.claimNo;
                    claim.pfc_zrepclmno = data.claimHeader.claimNotiNo;
                    claim.pfc_ref_caseId = new Microsoft.Xrm.Sdk.EntityReference(Incident.EntityLogicalName, contentModel.IncidentId);
                    claim.pfc_policy_additional = new Microsoft.Xrm.Sdk.EntityReference(Incident.EntityLogicalName, data.claimHeader.policyAdditionalID);

                    //crmSvc.Create(claim);

                    Incident crmCase = new Incident
                    {
                        IncidentId = contentModel.IncidentId,
                        pfc_locus_claim_id = locusClaimRegOutput.claimId,
                        pfc_locus_claim_status_on = DateTime.Now,
                        pfc_locus_claim_status_code = "1",
                        pfc_locus_claim_status_desc = "ลงทะเบียนสินไหมแล้ว",
                        pfc_claim_number = locusClaimRegOutput.claimNo,
                        pfc_register_claim_by = new Microsoft.Xrm.Sdk.EntityReference(Incident.EntityLogicalName, contentModel.CurrentUserId),
                        pfc_register_claim_on = DateTime.Now
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
                contentOutput.data = new ClaimRegistrationDataOutputModel();
                ClaimRegistrationDataOutputModel output = new ClaimRegistrationDataOutputModel();
                output.claimID = locusClaimRegOutput.claimId;
                output.claimNo = locusClaimRegOutput.claimNo;
                output.errorMessage = null;
                //contentOutput.data.Add(output);
                return output;
            }
            catch (BuzErrorException e)
            {
                AddDebugInfo("ClaimRegistration Error BuzErrorException:" + e.Message,e.StackTrace);
                
                ClaimRegistrationDataOutputModel outputFail = new ClaimRegistrationDataOutputModel
                {
                    claimID = null,
                    claimNo = null,
                    errorMessage = e.Message
                };
               

                return outputFail;
            }
            catch (Exception e)
            {
                AddDebugInfo("ClaimRegistration Error Exception:" + e.Message, e.StackTrace);
               
                ClaimRegistrationDataOutputModel outputFail = new ClaimRegistrationDataOutputModel
                {
                    claimID = null,
                    claimNo = null,
                    errorMessage = e.Message
                };
               

                return outputFail;
            }

            
            
        
        }
    }
}