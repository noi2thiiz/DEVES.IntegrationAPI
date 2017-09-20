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
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;
using DEVES.IntegrationAPI.WebApi.DataAccessService.QuerySQLAdapter;
using DEVES.IntegrationAPI.Model.QuerySQL;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class BuzClaimRegistrationCommand : BuzCommand
    {
        QueryInfo q = new QueryInfo();
        System.Data.DataTable dt = new System.Data.DataTable();

        Guid g = new Guid();
        LocusClaimRegistrationInputModel inputTest = new LocusClaimRegistrationInputModel();

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

            /*
            //+ Call SQL to get data
            List<CommandParameter> listParam = new List<CommandParameter>();
            listParam.Add(new CommandParameter("incidentId", contentModel.IncidentId));
            listParam.Add(new CommandParameter("CurrentUserId", contentModel.CurrentUserId));
            FillModelUsingSQL(ref inputData, CommonConstant.sqlcmd_Get_RegClaimInfo, listParam);
            */

            data = Mapping(contentModel.IncidentId.ToString(), contentModel.CurrentUserId.ToString());
            
            
            if (data.claimHeader == null && data.claimInform == null && data.claimAssignSurv == null && data.claimSurvInform == null)
            {
                ClaimRegistrationContentOutputModel contentOutputFail = new ClaimRegistrationContentOutputModel();
                contentOutputFail.data = new ClaimRegistrationDataOutputModel();
                ClaimRegistrationDataOutputModel outputFail = new ClaimRegistrationDataOutputModel();
                outputFail.claimID = null;
                outputFail.claimNo = null;
                outputFail.errorMessage = "ไม่พบ incidentId นี้ในระบบ CRM";

                return outputFail;
            }

            // Convert BaseDataModel tobe LocusInput and fix some variable (in case of "claimType = "O" <เตลมแห้ง>)
            // data = (LocusClaimRegistrationInputModel)inputData;
            // transform some data that is required from polisy400
            if (String.IsNullOrEmpty(data.claimHeader.informByCrmId))
            {
                data.claimHeader.informByCrmId = data.claimHeader.submitByCrmId;
            }
            if (String.IsNullOrEmpty(data.claimHeader.informByCrmName))
            {
                data.claimHeader.informByCrmName = data.claimHeader.submitByCrmName;
            }
            if (String.IsNullOrEmpty(data.claimInform.informerOn.ToString()))
            {
                data.claimInform.informerOn = DateTime.Now;
            }
            if (String.IsNullOrEmpty(data.claimAssignSurv.surveyTeam))
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

            // string uid = GetDomainName(contentModel.CurrentUserId);
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
                AddDebugInfo("ClaimRegistration Error BuzErrorException:" + e.Message, e.StackTrace);

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

        private LocusClaimRegistrationInputModel Mapping(string incidentId, string currentUserId)
        {
            QuerySqlService sql = QuerySqlService.Instance;

            string sqlCommand = string.Format(q.SQL_ClaimRegistration, incidentId, currentUserId).Trim('\n');
            QuerySQLOutputModel mappingOutput = new QuerySQLOutputModel();

            if (AppConst.IS_PRODUCTION)
            {
                mappingOutput = sql.GetQuery("CRM_MSCRM", sqlCommand);
            }
            else
            {
                try
                {
                    mappingOutput = sql.GetQuery("CRMQA_MSCRM", sqlCommand);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + e.StackTrace);
                }
            }

            dt = new System.Data.DataTable();
            dt = mappingOutput.dt;

            LocusClaimRegistrationInputModel rsModel = new LocusClaimRegistrationInputModel();

            if (dt.Rows.Count == 0)
            {
                return rsModel;
            }

            rsModel.claimHeader = new LocusClaimheaderModel();
            rsModel.claimInform = new LocusClaiminformModel();
            rsModel.claimAssignSurv = new LocusClaimassignsurvModel();
            rsModel.claimSurvInform = new LocusClaimsurvinformModel();

            // claimHeader
            rsModel.claimHeader.premiumClass = isStringNull("premiumClass");
            rsModel.claimHeader.ticketNumber = isStringNull("ticketNumber");
            rsModel.claimHeader.claimNotiNo = isStringNull("claimNotiNo");
            rsModel.claimHeader.claimNotiRefer = isStringNull("claimNotiRefer");
            rsModel.claimHeader.policyNo = isStringNull("policyNo");
            rsModel.claimHeader.fleetCarNo = isIntNull("fleetCarNo");
            rsModel.claimHeader.policySeqNo = isIntNull("policySeqNo");
            rsModel.claimHeader.renewalNo = isIntNull("renewalNo");
            rsModel.claimHeader.barcode = isStringNull("barcode");
            rsModel.claimHeader.insureCardNo = isStringNull("insureCardNo");
            rsModel.claimHeader.policyIssueDate = isDateTimeNull("policyIssueDate");
            rsModel.claimHeader.policyEffectiveDate = isDateTimeNull("policyEffectiveDate");
            rsModel.claimHeader.policyExpiryDate = isDateTimeNull("policyExpiryDate");
            rsModel.claimHeader.policyProductTypeCode = isStringNull("policyProductTypeCode");
            rsModel.claimHeader.policyProductTypeName = isStringNull("policyProductTypeName");
            rsModel.claimHeader.policyGarageFlag = isStringNull("policyGarageFlag");
            rsModel.claimHeader.policyPaymentStatus = isStringNull("policyPaymentStatus");
            rsModel.claimHeader.policyCarRegisterNo = isStringNull("policyCarRegisterNo");
            rsModel.claimHeader.policyCarRegisterProv = isStringNull("policyCarRegisterProv");
            rsModel.claimHeader.carChassisNo = isStringNull("carChassisNo");
            rsModel.claimHeader.carVehicleType = isStringNull("carVehicleType");
            rsModel.claimHeader.carVehicleModel = isStringNull("carVehicleModel");
            rsModel.claimHeader.carVehicleYear = isStringNull("carVehicleYear");
            rsModel.claimHeader.carVehicleBody = isStringNull("carVehicleBody");
            rsModel.claimHeader.carVehicleSize = isStringNull("carVehicleSize");
            rsModel.claimHeader.policyDeduct = isIntNull("policyDeduct");
            rsModel.claimHeader.agentCode = isStringNull("agentCode");
            rsModel.claimHeader.agentName = isStringNull("agentName");
            rsModel.claimHeader.agentBranch = isStringNull("agentBranch ");
            rsModel.claimHeader.vipCaseFlag = isStringNull("vipCaseFlag");
            rsModel.claimHeader.privilegeLevel = isStringNull("privilegeLevel");
            rsModel.claimHeader.highLossCaseFlag = isStringNull("highLossCaseFlag");
            rsModel.claimHeader.legalCaseFlag = isStringNull("legalCaseFlag");
            rsModel.claimHeader.claimNotiRemark = isStringNull("claimNotiRemark");
            rsModel.claimHeader.claimType = isStringNull("claimType");
            rsModel.claimHeader.informByCrmId = isStringNull("informByCrmId");
            rsModel.claimHeader.informByCrmName = isStringNull("informByCrmName");
            rsModel.claimHeader.submitByCrmId = isStringNull("submitByCrmId");
            rsModel.claimHeader.submitByCrmName = isStringNull("submitByCrmName");
            rsModel.claimHeader.serviceBranch = isStringNull("serviceBranch");
            rsModel.claimHeader.policyAdditionalID = new Guid(dt.Rows[0]["pfc_policy_additionalId"].ToString());
            rsModel.claimHeader.policyBranch = isStringNull("policyBranch");

            // claimInform
            rsModel.claimInform.informerClientId = isStringNull("informerClientId");
            rsModel.claimInform.informerFullName = isStringNull("informerFullName");
            rsModel.claimInform.informerMobile = isStringNull("informerMobile");
            rsModel.claimInform.informerPhoneNo = isStringNull("informerPhoneNo");
            rsModel.claimInform.driverClientId = isStringNull("driverClientId");
            rsModel.claimInform.driverFullName = isStringNull("driverFullName");
            rsModel.claimInform.driverMobile = isStringNull("driverMobile");
            rsModel.claimInform.driverPhoneNo = isStringNull("driverPhoneNo");
            rsModel.claimInform.insuredClientId = isStringNull("insuredClientId");
            rsModel.claimInform.insuredFullName = isStringNull("insuredFullName");
            rsModel.claimInform.insuredMobile = isStringNull("insuredMobile");
            rsModel.claimInform.insuredPhoneNo = isStringNull("insuredPhoneNo");
            rsModel.claimInform.relationshipWithInsurer = isStringNull("relationshipWithInsurer");
            rsModel.claimInform.currentCarRegisterNo = isStringNull("currentCarRegisterNo");
            rsModel.claimInform.currentCarRegisterProv = isStringNull("currentCarRegisterProv");
            rsModel.claimInform.informerOn = isDateTimeNull("informerOn");
            rsModel.claimInform.accidentOn = isDateTimeNull("accidentOn");
            rsModel.claimInform.accidentDescCode = isStringNull("accidentDescCode");
            rsModel.claimInform.accidentDesc = isStringNull("accidentDesc");
            rsModel.claimInform.numOfExpectInjury = isIntNull("numOfExpectInjury");
            rsModel.claimInform.accidentPlace = isStringNull("accidentPlace");
            rsModel.claimInform.accidentLatitude = isStringNull("accidentLatitude");
            rsModel.claimInform.accidentLongitude = isStringNull("accidentLongitude");
            rsModel.claimInform.accidentProvn = isStringNull("accidentProvn");
            rsModel.claimInform.accidentDist = isStringNull("accidentDist");
            rsModel.claimInform.sendOutSurveyorCode = isStringNull("sendOutSurveyorCode");

            // claimAssignSurv
            rsModel.claimAssignSurv.surveyorCode = isStringNull("surveyorCode");
            rsModel.claimAssignSurv.surveyorClientNumber = isStringNull("surveyorClientNumber");
            rsModel.claimAssignSurv.surveyorName = isStringNull("surveyorName");
            rsModel.claimAssignSurv.surveyorCompanyName = isStringNull("surveyorCompanyName");
            rsModel.claimAssignSurv.surveyorCompanyMobile = isStringNull("surveyorCompanyMobile");
            rsModel.claimAssignSurv.surveyorMobile = isStringNull("surveyorMobile");
            rsModel.claimAssignSurv.surveyorType = isStringNull("surveyorType");
            rsModel.claimAssignSurv.surveyTeam = isStringNull("surveyTeam");

            // claimSurvInform
            rsModel.claimSurvInform.deductibleFee = isIntNull("deductibleFee");
            rsModel.claimSurvInform.excessFee = isIntNull("excessFee");
            rsModel.claimSurvInform.reportAccidentResultDate = isDateTimeNull("reportAccidentResultDate");
            rsModel.claimSurvInform.accidentLegalResult = isStringNull("accidentLegalResult");
            rsModel.claimSurvInform.policeStation = isStringNull("policeStation");
            rsModel.claimSurvInform.policeRecordId = isStringNull("policeRecordId");
            rsModel.claimSurvInform.policeRecordDate = isDateTimeNull("policeRecordDate");
            rsModel.claimSurvInform.policeBailFlag = isStringNull("policeBailFlag");
            rsModel.claimSurvInform.damageOfPolicyOwnerCar = isStringNull("damageOfPolicyOwnerCar");
            rsModel.claimSurvInform.numOfTowTruck = isIntNull("numOfTowTruck");
            rsModel.claimSurvInform.nameOfTowCompany = isStringNull("nameOfTowCompany");
            rsModel.claimSurvInform.detailOfTowEvent = isStringNull("detailOfTowEvent");
            rsModel.claimSurvInform.numOfAccidentInjury = isIntNull("numOfAccidentInjury");
            rsModel.claimSurvInform.detailOfAccidentInjury = isStringNull("detailOfAccidentInjury");
            rsModel.claimSurvInform.numOfDeath = isIntNull("numOfDeath");
            rsModel.claimSurvInform.detailOfDeath = isStringNull("detailOfDeath");
            rsModel.claimSurvInform.caseOwnerCode = isStringNull("caseOwnerCode");
            rsModel.claimSurvInform.caseOwnerFullName = isStringNull("caseOwnerFullName");

            return rsModel;
        }

        protected string isStringNull(string a)
        {
            if (string.IsNullOrEmpty(dt.Rows[0][a].ToString()))
            {
                return null;
            }
            else
            {
                return dt.Rows[0][a].ToString();
            }
        }
        protected int? isIntNull(string a)
        {
            if (string.IsNullOrEmpty(dt.Rows[0][a].ToString()))
            {
                return null;
            }
            else
            {
                return Convert.ToInt32(dt.Rows[0][a]);
            }
        }
        protected double? isDoubleNull(string a)
        {
            if (string.IsNullOrEmpty(dt.Rows[0][a].ToString()))
            {
                return null;
            }
            else
            {
                return Convert.ToDouble(dt.Rows[0][a]);
            }
        }

        protected DateTime? isDateTimeNull(string a)
        {
            string datetime = "";

            if (String.IsNullOrEmpty(dt.Rows[0][a].ToString()))
            {
                datetime = null;

                return null;
            }
            else
            {
                datetime = dt.Rows[0][a].ToString();

                return Convert.ToDateTime(datetime);
            }

            
        }

        /*
        protected LocusClaimRegistrationInputModel UnitTest(ClaimRegistrationInputModel contentModel)
        {
            // Create Case
            ServiceContext svcContext;
            var _serviceProxy = GetOrganizationServiceProxy(out svcContext);

            Incident crmCase = new Incident
            {
                CustomerId = new Microsoft.Xrm.Sdk.EntityReference(Contact.EntityLogicalName, new Guid("4F2858B8-C3CE-E611-80D1-0050568D1874")),
                OwnerId = new Microsoft.Xrm.Sdk.EntityReference(SystemUser.EntityLogicalName, new Guid("50529F88-2BE9-E611-80D4-0050568D1874"))
            };

            g = _serviceProxy.Create(crmCase);

            var queryCase = from c in svcContext.IncidentSet
                            where c.IncidentId == g
                            select c;

            // Retrieve Case
            Incident incident = queryCase.FirstOrDefault<Incident>();
            string ticketNo = incident.TicketNumber;

            // Query claimNotiNo
            string SQL_ClaimNotiNo = @"DECLARE @ticketNo AS NVARCHAR(20) = '{0}'; DECLARE @uniqueID AS UNIQUEIDENTIFIER = '{1}'; DECLARE @requestBy AS NVARCHAR(250) = '{2}'; DECLARE @resultCode AS NVARCHAR(10) DECLARE @resultDesc AS NVARCHAR(1000) select @uniqueID = ISNULL(IncidentId, @uniqueID) from dbo.IncidentBase a with(nolock) where a.TicketNumber = @ticketNo EXEC[dbo].[sp_ReqMotorClaimNotiNo] @uniqueID, @requestBy, @resultCode OUTPUT, @resultDesc OUTPUT select @ticketNo as [@ticketNo],@uniqueID as [@uniqueID],@requestBy as [@requestBy],@resultCode as [@resultCode], @resultDesc as [@resultDesc]";
            string sqlCommand = string.Format(SQL_ClaimNotiNo, ticketNo, g, contentModel.CurrentUserId.ToString());

            QuerySqlService sql = QuerySqlService.Instance;
            QuerySQLOutputModel mappingOutput = new QuerySQLOutputModel();
            mappingOutput = sql.GetQuery("CRMQA_MSCRM", sqlCommand);

            // prepare variable for returning 
            LocusClaimRegistrationInputModel inputMock = new LocusClaimRegistrationInputModel();
            inputMock.claimHeader = new LocusClaimheaderModel();
            inputMock.claimAssignSurv = new LocusClaimassignsurvModel();
            inputMock.claimInform = new LocusClaiminformModel();
            inputMock.claimSurvInform = new LocusClaimsurvinformModel();

            // Mocking Variable to Case
            inputMock.claimHeader.premiumClass = "MVS";
            inputMock.claimHeader.ticketNumber = ticketNo;
            inputMock.claimHeader.claimNotiNo = mappingOutput.dt.Rows[0]["@resultDesc"].ToString();
            inputMock.claimHeader.policyNo = "V0756004";
            inputMock.claimHeader.fleetCarNo = 1;
            inputMock.claimHeader.policySeqNo = 1;
            inputMock.claimHeader.renewalNo = 0;
            inputMock.claimHeader.barcode = "2080003047183";
            inputMock.claimHeader.policyIssueDate = Convert.ToDateTime("2015-12-17 00:00:00");
            inputMock.claimHeader.policyEffectiveDate = Convert.ToDateTime("2016-01-29 00:00:00");
            inputMock.claimHeader.policyExpiryDate = Convert.ToDateTime("2017-10-29 00:00:00");
            inputMock.claimHeader.policyProductTypeCode = "FE";
            inputMock.claimHeader.policyProductTypeName = "ประเภท 2++";
            inputMock.claimHeader.policyGarageFlag = "N";
            inputMock.claimHeader.policyPaymentStatus = "N";
            inputMock.claimHeader.policyCarRegisterNo = "ษม7045";
            inputMock.claimHeader.policyCarRegisterProv = "กท";
            inputMock.claimHeader.carChassisNo = "MR053BK3005017550@";
            inputMock.claimHeader.carVehicleModel = "TOYOTA CAMRY";
            inputMock.claimHeader.carVehicleYear = "2004";
            inputMock.claimHeader.carVehicleBody = "SEDAN";
            inputMock.claimHeader.agentCode = "90001521";
            inputMock.claimHeader.agentName = "หักส่วนลดหน้าตาราง";
            inputMock.claimHeader.agentBranch = "00";
            inputMock.claimHeader.vipCaseFlag = "N";
            inputMock.claimHeader.privilegeLevel = "1";
            inputMock.claimHeader.highLossCaseFlag = "N";
            inputMock.claimHeader.legalCaseFlag = "N";
            inputMock.claimHeader.claimNotiRemark = "วันที่เกิดเหตุ : 28-8-2017 เวลา 08:00 น.\n การเกิดเหตุ : คู่กรณีชนท้ายประกัน";
            inputMock.claimHeader.claimType = "O";
            inputMock.claimHeader.informByCrmId = "crmtest1";
            inputMock.claimHeader.informByCrmName = "crm test1";
            inputMock.claimHeader.submitByCrmId = "crmtest1";
            inputMock.claimHeader.submitByCrmName = "crm test1";
            inputMock.claimHeader.serviceBranch = "00";
            inputMock.claimHeader.policyAdditionalID = new Guid("D78D5356-F4B6-E611-80CA-0050568D1874");
            inputMock.claimHeader.policyBranch = "00";

            inputMock.claimInform.informerClientId = "14514669";
            inputMock.claimInform.informerFullName = "ธวัชชัย จันทน์แดง";
            inputMock.claimInform.informerMobile = "0863156332";
            inputMock.claimInform.driverClientId = "14514669";
            inputMock.claimInform.driverFullName = "ธวัชชัย จันทน์แดง";
            inputMock.claimInform.driverMobile = "0863156332";
            inputMock.claimInform.insuredClientId = "10033581";
            inputMock.claimInform.insuredFullName = "ธวัชชัย จันทน์แดง";
            inputMock.claimInform.relationshipWithInsurer = "00";
            inputMock.claimInform.currentCarRegisterNo = "ษม7045";
            inputMock.claimInform.currentCarRegisterProv = "กท";
            inputMock.claimInform.informerOn = Convert.ToDateTime("2017-08-30 17:36:00");
            inputMock.claimInform.accidentOn = Convert.ToDateTime("2017-08-28 08:00:00");
            inputMock.claimInform.accidentDescCode = "201";
            inputMock.claimInform.accidentDesc = "วันที่เกิดเหตุ : 28-8-2017 เวลา 08:00 น.\n การเกิดเหตุ : คู่กรณีชนท้ายประกัน";
            inputMock.claimInform.numOfExpectInjury = 0;
            inputMock.claimInform.accidentPlace = "97 ถนน ดินสอ แขวง วัดบวรนิเวศ เขต พระนคร กรุงเทพมหานคร 10200 ประเทศไทย";
            inputMock.claimInform.accidentLatitude = "13.757";
            inputMock.claimInform.accidentLongitude = "100.502";
            inputMock.claimInform.sendOutSurveyorCode = "01";

            inputMock.claimAssignSurv.surveyTeam = "TEAM0099";

            inputMock.claimSurvInform.policeBailFlag = "N";

            return inputMock;
        }
        */

    }
}