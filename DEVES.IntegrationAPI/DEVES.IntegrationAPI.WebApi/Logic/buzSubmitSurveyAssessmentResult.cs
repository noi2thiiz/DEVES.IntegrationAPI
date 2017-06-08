using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.SubmitSurveyAssessmentResult;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzSubmitSurveyAssessmentResult : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {

            // Preparation Variable
            SubmitSurveyAssessmentResultOutputModel_Pass output = new SubmitSurveyAssessmentResultOutputModel_Pass();
            output.data = new SubmitSurveyAssessmentResultDataModel_Pass();

            // Deserialize Input
            // SubmitSurveyAssessmentResultInputModel contentModel = DeserializeJson<SubmitSurveyAssessmentResultInputModel>(input.ToString());
            SubmitSurveyAssessmentResultInputModel contentModel = (SubmitSurveyAssessmentResultInputModel)input;

            // Connect SDK and query
            var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
            OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
            ServiceContext svcContext = new ServiceContext(_serviceProxy);

            var query = from c in svcContext.pfc_assessmentSet
                        where c.pfc_assessment_ref_code == contentModel.assessmentrefcode
                        select c;


            // Condition check if ref_code don't have in CRM -> RETURN ERROR
            if (query.FirstOrDefault<pfc_assessment>() == null)
            {
                output.code = AppConst.CODE_FAILED;
                output.message = "ไม่สามารถบันทึกคะแนนได้";
                output.description = "ไม่สามารถบันทึกคะแนนได้เนื่องจากไม่พบแบบสำรวจความพึงพอใจ";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now.ToString();

                return output;
            }
            // if already have ref_code in CRM -> RETURN message
            else
            {

                pfc_assessment firstQuery = query.FirstOrDefault<pfc_assessment>();
                // Guid guid = new Guid(firstQuery.Id.ToString());
                // pfc_assessment retrievedAssessment = (pfc_assessment)_serviceProxy.Retrieve(pfc_assessment.EntityLogicalName, guid, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

                // if Type = 1 (Survey)
                if (contentModel.assessmentType == 1)
                {
                    // check that survey assessment was done??
                    // ทำการประเมินแล้ว
                    if (firstQuery.pfc_assessment_survey_status.Value == 100000001)
                    {
                        output.code = AppConst.CODE_FAILED;
                        output.message = "ไม่สามารถบันทึกคะแนนได้";
                        output.description = "เนื่องจากมีการบันทึกคะแนนไปแล้ว จึงไม่สามารถบันทึกคะแนนซ้ำได้";
                        output.transactionId = TransactionId;
                        output.transactionDateTime = DateTime.Now.ToString();

                        return output;
                    }
                    // ยังไม่ได้ทำการประเมิน
                    else
                    {
                        Guid guid = new Guid(firstQuery.Id.ToString());
                        pfc_assessment retrievedAssessment = (pfc_assessment)_serviceProxy.Retrieve(pfc_assessment.EntityLogicalName, guid, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

                        // survey assessment
                        retrievedAssessment.pfc_assessment_claim_noti_date = DateTime.Now;
                        retrievedAssessment.pfc_assessment_claim_noti_score = contentModel.assessmentClaimNotiScore;
                        retrievedAssessment.pfc_assessment_claim_noti_comment = contentModel.assessmentClaimNotiComment;
                        retrievedAssessment.pfc_assessment_survey_date = DateTime.Now;
                        retrievedAssessment.pfc_assessment_survey_score = contentModel.assessmentSurveyScore;
                        retrievedAssessment.pfc_assessment_survey_speed_score = contentModel.assessmentSurveySpeedScore;
                        retrievedAssessment.pfc_assessment_survey_timeusage_score = contentModel.assessmentSurveyTimeUsageScore;
                        retrievedAssessment.pfc_assessment_survey_comment = contentModel.assessmentSurveyComment;
                        retrievedAssessment.pfc_assessment_survey_status = new OptionSetValue(100000001);
                        retrievedAssessment.pfc_assessment_claim_noti_by = string.IsNullOrEmpty(contentModel.assessmentSurveyByUserid) ? new OptionSetValue(100000000) : new OptionSetValue(100000001);
                        retrievedAssessment.pfc_assessment_survey_by = string.IsNullOrEmpty(contentModel.assessmentSurveyByUserid) ? new OptionSetValue(100000000) : new OptionSetValue(100000001);
                        if (!string.IsNullOrEmpty(contentModel.assessmentSurveyByUserid)) { 
                            retrievedAssessment.pfc_assessment_claim_noti_by_userid = new EntityReference(pfc_assessment.EntityLogicalName, new Guid(contentModel.assessmentSurveyByUserid));
                        }
                        if (!string.IsNullOrEmpty(contentModel.assessmentSurveyByUserid))
                        {
                            retrievedAssessment.pfc_assessment_survey_by_userid = new EntityReference(pfc_assessment.EntityLogicalName, new Guid(contentModel.assessmentSurveyByUserid));
                        }
                        if (!string.IsNullOrEmpty(contentModel.assessmentSurveyByUserid))
                        {
                            retrievedAssessment.pfc_assessment_garage_by_userid = new EntityReference(pfc_assessment.EntityLogicalName, new Guid(contentModel.assessmentGarageByUserid));
                        }
                        retrievedAssessment.pfc_assessment_survey_ipaddress = HttpContext.Current.Request.UserHostAddress;

                        try
                        {
                            _serviceProxy.Update(retrievedAssessment);
                        }
                        catch (Exception e)
                        {
                            output.code = AppConst.CODE_FAILED;
                            output.message = "ไม่สามารถบันทึกคะแนนได้";
                            output.description = e.Message;
                            output.transactionId = TransactionId;
                            output.transactionDateTime = DateTime.Now.ToString();
                            return output;
                        }

                        output.code = AppConst.CODE_SUCCESS;
                        output.message = AppConst.MESSAGE_SUCCESS;
                        output.description = "บันทึกคะแนนเรียบร้อยแล้ว";
                        output.transactionId = TransactionId;
                        output.transactionDateTime = DateTime.Now.ToString();
                        return output;
                    }
                }
                // if Type = 2 (Garage)
                else if (contentModel.assessmentType == 2)
                {
                    // check that survey assessment was done??
                    // ทำการประเมินแล้ว
                    if (firstQuery.pfc_assessment_garage_status.Value == 100000001)
                    {
                        output.code = AppConst.CODE_FAILED;
                        output.message = "ไม่สามารถบันทึกคะแนนได้";
                        output.description = "เนื่องจากมีการบันทึกคะแนนไปแล้ว จึงไม่สามารถบันทึกคะแนนซ้ำได้";
                        output.transactionId = TransactionId;
                        output.transactionDateTime = DateTime.Now.ToString();

                        return output;
                    }
                    // ยังไม่ได้ทำการประเมิน
                    else
                    {
                        Guid guid = new Guid(firstQuery.Id.ToString());
                        pfc_assessment retrievedAssessment = (pfc_assessment)_serviceProxy.Retrieve(pfc_assessment.EntityLogicalName, guid, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

                        // garage assessment
                        retrievedAssessment.pfc_assessment_garage_date = DateTime.Now;
                        retrievedAssessment.pfc_assessment_garage_service_score = contentModel.assessmentGarageServiceScore;
                        retrievedAssessment.pfc_assessment_garage_commit_score = contentModel.assessmentGarageCommitScore;
                        retrievedAssessment.pfc_assessment_garage_repair_score = contentModel.assessmentGarageRepairScore;
                        retrievedAssessment.pfc_assessment_garage_comment = contentModel.assessmentGarageComment;
                        retrievedAssessment.pfc_assessment_garage_status = new OptionSetValue(100000001);
                        retrievedAssessment.pfc_assessment_garage_by = string.IsNullOrEmpty(contentModel.assessmentSurveyByUserid) ? new OptionSetValue(100000000) : new OptionSetValue(100000001);
                        if (!string.IsNullOrEmpty(contentModel.assessmentSurveyByUserid))
                        {
                            retrievedAssessment.pfc_assessment_claim_noti_by_userid = new EntityReference(pfc_assessment.EntityLogicalName, new Guid(contentModel.assessmentSurveyByUserid));
                        }
                        if (!string.IsNullOrEmpty(contentModel.assessmentSurveyByUserid))
                        {
                            retrievedAssessment.pfc_assessment_survey_by_userid = new EntityReference(pfc_assessment.EntityLogicalName, new Guid(contentModel.assessmentSurveyByUserid));
                        }
                        if (!string.IsNullOrEmpty(contentModel.assessmentSurveyByUserid))
                        {
                            retrievedAssessment.pfc_assessment_garage_by_userid = new EntityReference(pfc_assessment.EntityLogicalName, new Guid(contentModel.assessmentGarageByUserid));
                        }
                        retrievedAssessment.pfc_assessment_garage_ipaddress = HttpContext.Current.Request.UserHostAddress;

                        try
                        {
                            _serviceProxy.Update(retrievedAssessment);
                        }
                        catch (Exception e)
                        {
                            output.code = AppConst.CODE_FAILED;
                            output.message = "ไม่สามารถบันทึกคะแนนได้";
                            output.description = e.Message;
                            output.transactionId = TransactionId;
                            output.transactionDateTime = DateTime.Now.ToString();
                            return output;
                        }

                        output.code = AppConst.CODE_SUCCESS;
                        output.message = AppConst.MESSAGE_SUCCESS;
                        output.description = "บันทึกคะแนนเรียบร้อยแล้ว";
                        output.transactionId = TransactionId;
                        output.transactionDateTime = DateTime.Now.ToString();
                        return output;
                    }
                }
                // CASE assessmentType != 1 and 2
                else
                {
                    output.code = AppConst.CODE_FAILED;
                    output.message = "ไม่สามารถบันทึกคะแนนได้";
                    output.description = "assessmentType ไม่ใช่สถานะทั้ง Survey และ Garage";
                    output.transactionId = TransactionId;
                    output.transactionDateTime = DateTime.Now.ToString();

                    return output;
                }

            }

        }


    }
}